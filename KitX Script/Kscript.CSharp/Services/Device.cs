using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Device : IDevice
    {
        private readonly Connector _connector = Connector.Instance;
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
                p.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
            
            return pluginInfo != null ? await CreatePluginInstance(pluginInfo) : null;
        }

        public async Task<IEnumerable<PluginInfo>> GetPluginList() => throw new NotImplementedException();

        public bool HasPlugin(string idOrName)
        {
            return _pluginCache.Values.Any(p => 
                p.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IPlugin> CreatePluginInstance(PluginInfo info)
        {
            return new Plugin(info, Info);
        }
    }
}
