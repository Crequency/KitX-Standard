﻿using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using System.Text.Json;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Device : IDevice
    {
        private readonly Connector _connector;
        private Dictionary<string, PluginInfo> _pluginCache = new();
        
        public DeviceInfo Info { get; }

        public Device(DeviceInfo info, Connector? connector = null)
        {
            Info = info;
            _connector = connector ?? Connector.Instance;
        }

        public async Task<IPlugin> RequestPlugin(string Name)
        {
            var plugins = await GetPluginList();
            var pluginInfo = plugins.FirstOrDefault(p => 
                p.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            
            return pluginInfo != null ? await CreatePluginInstance(pluginInfo) : null;
        }

        public async Task<IEnumerable<PluginInfo>> GetPluginList()
        {
            if (_pluginCache.Any())
                return _pluginCache.Values;

            var tcs = new TaskCompletionSource<IEnumerable<PluginInfo>>();

            void OnResponse(Request response)
            {
                try
                {
                    response.Match(
                        response.GetContent(content =>
                        {
                            if (string.IsNullOrEmpty(content))
                            {
                                tcs.SetResult(Array.Empty<PluginInfo>());
                                return content;
                            }

                            // With the following code
                            var plugins = JsonSerializer.Deserialize<List<PluginInfo>>(content);
                            
                            // Update cache
                            _pluginCache = plugins.ToDictionary(
                                p => p.Name,
                                p => p,
                                StringComparer.OrdinalIgnoreCase
                            );

                            tcs.SetResult(plugins);
                            return content;
                        }),
                        matchCommand: content =>
                        {
                            if (content.StartsWith("Error:"))
                            {
                                var error = content.Substring("Error:".Length);
                                tcs.SetException(new InvalidOperationException($"Failed to get plugin list: {error}"));
                            }
                            else if (content.StartsWith("PluginList:"))
                            {
                                var pluginListJson = content.Substring("PluginList:".Length);
                                var plugins = JsonSerializer.Deserialize<List<PluginInfo>>(pluginListJson);
                                
                                // Update cache
                                _pluginCache = plugins.ToDictionary(
                                    p => p.Name,
                                    p => p,
                                    StringComparer.OrdinalIgnoreCase
                                );

                                tcs.SetResult(plugins);
                            }
                        }
                    );
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            // Request plugin list from device
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = "GetPluginList";
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Type = RequestTypes.Command;
                    req.Version = RequestVersions.V1;
                    req.Target = Info.Device;
                    return req;
                })
                .SetSender(OnResponse)
                .Send();

            return await tcs.Task;
        }

        public bool HasPlugin(string Name)
        {
            return _pluginCache.ContainsKey(Name);
        }

        public async Task<IPlugin> CreatePluginInstance(PluginInfo info)
        {
            return new Plugin(info, Info, _connector);
        }
    }
}
