using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Plugin : IPlugin
    {
        private readonly Connector _connector = Connector.Instance;
        private readonly Dictionary<string, Function> _functionCache = new();

        public PluginInfo Info { get; }
        public DeviceInfo AssociatedDevice { get; }

        public Plugin(PluginInfo info, DeviceInfo deviceInfo)
        {
            Info = info;
            AssociatedDevice = deviceInfo;
        }

        public async Task<IFunction> RequestFunction(string idOrName)
        {
            var functions = await GetFunctionList();
            var function = functions.FirstOrDefault(f => 
                f.Info.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
            return function;
        }

        public async Task<IEnumerable<IFunction>> GetFunctionList()
        {
            var functions = Info.Functions.Select(f => 
                new Function(f, Info, AssociatedDevice, _connector) as IFunction);
            return await Task.FromResult(functions);
        }

        public async Task<IFunction?> GetFunctionByType(string type)
        {
            var functions = await GetFunctionList();
            return functions.FirstOrDefault(f => 
                f.Info.ReturnValueType.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<object> ExecuteFunction(string functionName, params object[] parameters)
        {
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.PluginConnectionId = Info.Id;
                    cmd.FunctionName = functionName;
                    cmd.FunctionArgs = parameters.Select(p => new Parameter { Value = p }).ToList();
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Target = new DeviceLocator { Id = AssociatedDevice.Id };
                    return req;
                })
                .Send();

            // 处理响应
            return null; // 需要处理实际响应
        }

        public bool HasFunction(string idOrName)
        {
            return Info.Functions.Any(f => 
                f.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
