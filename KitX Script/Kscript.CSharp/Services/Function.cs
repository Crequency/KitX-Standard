using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Function : IFunction
    {
        private readonly Connector _connector = Connector.Instance;
        private readonly DeviceInfo _deviceInfo;

        public KitX.Shared.CSharp.Plugin.Function Info { get; }
        public PluginInfo AssociatedPlugin { get; }

        public Function(KitX.Shared.CSharp.Plugin.Function info, PluginInfo pluginInfo, DeviceInfo deviceInfo)
        {
            Info = info;
            AssociatedPlugin = pluginInfo;
            _deviceInfo = deviceInfo;
        }

        public async Task<object> Invoke(params string[] parameters)
        {
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = Info.Name;
                    cmd.FunctionArgs = parameters.Select(p => new Parameter { Value = p }).ToList();
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Target = _deviceInfo.Device;
                    return req;
                })
                .Send();

            // 等待并处理响应
            return null; // 需要处理实际响应
        }

        public Type GetReturnType()
        {
            return Type.GetType(Info.ReturnValueType) ?? typeof(object);
        }
    }
}
