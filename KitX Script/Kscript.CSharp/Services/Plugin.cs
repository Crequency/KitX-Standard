using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Plugin : IPlugin
    {
        private readonly NetworkConnector _connector;
        private readonly Dictionary<string, Function> _functionCache = new();

        public PluginInfo Info { get; }
        public DeviceInfo AssociatedDevice { get; }

        public Plugin(PluginInfo info, DeviceInfo deviceInfo, NetworkConnector connector)
        {
            Info = info;
            AssociatedDevice = deviceInfo;
            _connector = connector;
        }

        public async Task<IFunction> RequestFunction(string idOrName)
        {
            var functions = await GetFunctionList();
            var function = functions.FirstOrDefault(f => 
                f.Info.Id.Equals(idOrName, StringComparison.OrdinalIgnoreCase) || 
                f.Info.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
            return function;
        }

        public async Task<IEnumerable<IFunction>> GetFunctionList()
        {
            var functions = Info.Functions.Select(f => 
                new Function(f, Info, AssociatedDevice, _connector) as IFunction);
            return await Task.FromResult(functions);
        }

        public async Task<IFunction> GetFunctionByType(string type)
        {
            var functions = await GetFunctionList();
            return functions.FirstOrDefault(f => 
                f.Info.ReturnType.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<object> ExecuteFunction(string functionName, params object[] parameters)
        {
            return await _connector.ExecuteFunction(
                AssociatedDevice.Id,
                Info.Id,
                functionName,
                parameters
            );
        }

        public bool HasFunction(string idOrName)
        {
            return Info.Functions.Any(f => 
                f.Id.Equals(idOrName, StringComparison.OrdinalIgnoreCase) || 
                f.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
