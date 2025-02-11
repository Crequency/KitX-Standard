using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Interfaces
{
    public interface IPlugin
    {
        PluginInfo Info { get; }
        DeviceInfo AssociatedDevice { get; }
        Task<IFunction?> RequestFunction(string idOrName);
        Task<IEnumerable<IFunction>> GetFunctionList();
        Task<IFunction?> GetFunctionByType(string type);
        Task<object> ExecuteFunction(string functionName, params string[] parameters);
        bool HasFunction(string idOrName);
    }
}
