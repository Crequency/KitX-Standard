using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Interfaces
{
    public interface IDevice
    {
        DeviceInfo Info { get; }
        Task<IPlugin> RequestPlugin(string idOrName);
        Task<IEnumerable<PluginInfo>> GetPluginList();
        bool HasPlugin(string idOrName);
        Task<IPlugin> CreatePluginInstance(PluginInfo info);
    }
}
