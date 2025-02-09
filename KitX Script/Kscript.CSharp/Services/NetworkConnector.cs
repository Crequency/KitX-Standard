using KitX.Shared.CSharp.WebCommand;
using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Services
{
    internal class NetworkConnector
    {
        private static readonly NetworkConnector _instance = new();
        private readonly Dictionary<string, DeviceInfo> _deviceCache = new();
        private readonly Connector _connector = new();

        public static NetworkConnector Instance => _instance;

        private NetworkConnector()
        {
            // 初始化连接器
            _connector.Connect();
        }

        public async Task<DeviceInfo> GetLocalDeviceInfo()
        {
            var request = new RequestBuilder()
                .SetCommandType(CommandType.GetLocalDeviceInfo)
                .Build();
            var response = await _connector.SendRequest(request);
            return response.As<DeviceInfo>();
        }

        public async Task<IEnumerable<DeviceInfo>> GetDeviceList()
        {
            var request = new RequestBuilder()
                .SetCommandType(CommandType.GetDeviceList)
                .Build();
            var response = await _connector.SendRequest(request);
            var devices = response.As<IEnumerable<DeviceInfo>>();
            foreach (var device in devices)
            {
                _deviceCache[device.Id] = device;
            }
            return devices;
        }

        public async Task<IEnumerable<PluginInfo>> GetPluginList(string deviceId)
        {
            var request = new RequestBuilder()
                .SetCommandType(CommandType.GetPluginList)
                .SetTarget(deviceId)
                .Build();
            var response = await _connector.SendRequest(request);
            return response.As<IEnumerable<PluginInfo>>();
        }

        public async Task<object> ExecuteFunction(string deviceId, string pluginId, string functionName, object[] parameters)
        {
            var request = new RequestBuilder()
                .SetCommandType(CommandType.ExecuteFunction)
                .SetTarget(deviceId)
                .AddParameter("pluginId", pluginId)
                .AddParameter("functionName", functionName)
                .AddParameter("parameters", parameters)
                .Build();
            var response = await _connector.SendRequest(request);
            return response.Result;
        }
    }
}
