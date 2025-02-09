using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Function : IFunction
    {
        private readonly NetworkConnector _connector;
        private readonly DeviceInfo _deviceInfo;

        public Function Info { get; }
        public PluginInfo AssociatedPlugin { get; }

        public Function(KitX.Shared.CSharp.Plugin.Function info, PluginInfo pluginInfo, 
            DeviceInfo deviceInfo, NetworkConnector connector)
        {
            Info = info;
            AssociatedPlugin = pluginInfo;
            _deviceInfo = deviceInfo;
            _connector = connector;
        }

        public async Task<object> Invoke(params object[] parameters)
        {
            if (!ValidateParameters(parameters))
            {
                throw new ArgumentException("Invalid parameters");
            }

            return await _connector.ExecuteFunction(
                _deviceInfo.Id,
                AssociatedPlugin.Id,
                Info.Name,
                parameters
            );
        }

        public bool ValidateParameters(object[] parameters)
        {
            if (parameters.Length != Info.Parameters.Length)
                return false;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (!Info.Parameters[i].ParameterType.IsInstanceOfType(parameters[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public Type GetReturnType()
        {
            return Type.GetType(Info.ReturnType) ?? typeof(object);
        }
    }
}
