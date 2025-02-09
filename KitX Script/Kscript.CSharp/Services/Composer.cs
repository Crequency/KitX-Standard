using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Composer : IComposer
    {
        private readonly Connector _connector = Connector.Instance;
        private IEnumerable<DeviceInfo>? _cachedDeviceList;

        public async Task<IDevice> RequestLocalDevice() => throw new NotImplementedException();

        public async Task<IDevice?> RequestMainController()
        {
            var devices = await RequestDeviceList();
            var mainController = devices.FirstOrDefault(x => x.IsMainDevice);
            return mainController != null ? new Device(mainController) : null;
        }

        public async Task<IDevice?> RequestRandomDesktopDevice()
        {
            var devices = await RequestDeviceList();
            var desktopDevices = devices.Where(x => x.DeviceOSType == OperatingSystems.Windows
                                                                                                || x.DeviceOSType == OperatingSystems.MacOS
                                                                                                || x.DeviceOSType == OperatingSystems.Linux).ToList();
            if (!desktopDevices.Any()) return null;

            var random = new Random();
            var randomDevice = desktopDevices[random.Next(desktopDevices.Count)];
            return new Device(randomDevice);
        }

        public async Task<IDevice?> RequestRandomMobileDevice()
        {
            var devices = await RequestDeviceList();
            var mobileDevices = devices.Where(x => x.DeviceOSType == OperatingSystems.Android
                                                                                               || x.DeviceOSType == OperatingSystems.IOS).ToList();
            if (!mobileDevices.Any()) return null;
            
            var random = new Random();
            var randomDevice = mobileDevices[random.Next(mobileDevices.Count)];
            return new Device(randomDevice);
        }

        public async Task<IDevice?> RequestDeviceByFilter(Func<DeviceInfo, bool> filter)
        {
            var devices = await RequestDeviceList();
            var matchedDevice = devices.FirstOrDefault(filter);
            return matchedDevice != null ? new Device(matchedDevice) : null;
        }

        // todo: implement this method
        public Task<IDevice?> RequestUserSelectedDevice(IEnumerable<DeviceInfo> candidates) => throw new NotImplementedException();

        public async Task<IEnumerable<DeviceInfo>> RequestDeviceList() => throw new NotImplementedException();

        private async Task HandleResponse(Request response)
        {
            response.Match(
                response.GetContent(content => content), // 这里需要提供实际的解密函数
                matchCommand: ProcessCommandResponse
            );
        }

        private void ProcessCommandResponse(string content)
        {
            // 处理命令响应
        }
    }
}
