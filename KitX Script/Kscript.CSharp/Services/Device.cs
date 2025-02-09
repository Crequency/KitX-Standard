using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Device : IDevice
    {
        private readonly NetworkConnector _connector = NetworkConnector.Instance;
        private Dictionary<string, PluginInfo> _pluginCache = new();
        
        public DeviceInfo Info { get; }

        public Device(DeviceInfo info)
        {
            Info = info;
        }

        public async Task<IPlugin> RequestPlugin(string idOrName)
        {
            var plugins = await GetPluginList();
            var pluginInfo = plugins.FirstOrDefault(p => 
                p.Id.Equals(idOrName, StringComparison.OrdinalIgnoreCase) || 
                p.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
            
            return pluginInfo != null ? await CreatePluginInstance(pluginInfo) : null;
        }

        public async Task<IEnumerable<IPlugin>> RequestPluginsByType(string type)
        {
            var plugins = await GetPluginList();
            var matchedPlugins = plugins.Where(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            var tasks = matchedPlugins.Select(CreatePluginInstance);
            return await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<PluginInfo>> GetPluginList()
        {
            var plugins = await _connector.GetPluginList(Info.Id);
            _pluginCache = plugins.ToDictionary(p => p.Id);
            return plugins;
        }

        public bool HasPlugin(string idOrName)
        {
            return _pluginCache.Values.Any(p => 
                p.Id.Equals(idOrName, StringComparison.OrdinalIgnoreCase) || 
                p.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IPlugin> CreatePluginInstance(PluginInfo info)
        {
            return new Plugin(info, Info, _connector);
        }
    }
}
