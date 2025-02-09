using KitX.Shared.CSharp.Device;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Composer : IComposer
    {
        private readonly NetworkConnector _connector = NetworkConnector.Instance;
        private IEnumerable<DeviceInfo> _cachedDeviceList;

        public async Task<IDevice> RequestLocalDevice()
        {
            var deviceInfo = await _connector.GetLocalDeviceInfo();
            return new Device(deviceInfo);
        }

        public async Task<IDevice> RequestMainController()
        {
            var devices = await RequestDeviceList();
            var mainController = devices.FirstOrDefault(x => x.IsMainController);
            return mainController != null ? new Device(mainController) : null;
        }

        public async Task<IDevice> RequestRandomDesktopDevice()
        {
            var devices = await RequestDeviceList();
            var desktopDevices = devices.Where(x => x.IsDesktopDevice).ToList();
            if (!desktopDevices.Any()) return null;
            
            var random = new Random();
            var randomDevice = desktopDevices[random.Next(desktopDevices.Count)];
            return new Device(randomDevice);
        }

        public async Task<IDevice> RequestRandomMobileDevice()
        {
            var devices = await RequestDeviceList();
            var mobileDevices = devices.Where(x => x.IsMobileDevice).ToList();
            if (!mobileDevices.Any()) return null;
            
            var random = new Random();
            var randomDevice = mobileDevices[random.Next(mobileDevices.Count)];
            return new Device(randomDevice);
        }

        public async Task<IDevice> RequestDeviceByFilter(Func<DeviceInfo, bool> filter)
        {
            var devices = await RequestDeviceList();
            var matchedDevice = devices.FirstOrDefault(filter);
            return matchedDevice != null ? new Device(matchedDevice) : null;
        }

        public async Task<IDevice> RequestUserSelectedDevice(IEnumerable<DeviceInfo> candidates)
        {
            // 这里需要调用本地UI让用户选择设备
            // 简化实现，返回第一个设备
            var device = candidates.FirstOrDefault();
            return device != null ? new Device(device) : null;
        }

        public async Task<IEnumerable<DeviceInfo>> RequestDeviceList()
        {
            _cachedDeviceList = await _connector.GetDeviceList();
            return _cachedDeviceList;
        }
    }
}
