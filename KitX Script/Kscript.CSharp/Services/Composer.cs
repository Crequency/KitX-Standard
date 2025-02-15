using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;
using Kscript.CSharp.Utils;
using System.Text.Json;

namespace Kscript.CSharp.Services
{
    /// <summary>
    /// 设备管理器
    /// </summary>
    public class Composer : IComposer, IDisposable
    {
        private readonly Connector _connector;
        private readonly DeviceCache _deviceCache;
        private readonly DeviceRequestBuilder _requestBuilder;
        private readonly HashSet<IDeviceEventListener> _listeners = new();
        private bool _isDisposed;

        private readonly object _lockObject = new();
        private Dictionary<string, DeviceInfo> _lastKnownDevices = new();
        
        // 设备离线判断阈值（超过这个时间没有收到更新则认为设备离线）
        private static readonly TimeSpan DeviceOfflineThreshold = TimeSpan.FromMinutes(2);

        public Composer(Connector? connector = null)
        {
            _connector = connector ?? Connector.Instance;
            _deviceCache = new DeviceCache();
            _requestBuilder = new DeviceRequestBuilder(_connector);
        }

        /// <summary>
        /// 添加设备事件监听器
        /// </summary>
        public void AddListener(IDeviceEventListener listener)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(Composer));

            lock (_lockObject)
            {
                _listeners.Add(listener);
            }
        }

        /// <summary>
        /// 移除设备事件监听器
        /// </summary>
        public void RemoveListener(IDeviceEventListener listener)
        {
            if (_isDisposed)
                return;

            lock (_lockObject)
            {
                _listeners.Remove(listener);
            }
        }

        /// <summary>
        /// 获取本地设备
        /// </summary>
        public async Task<IDevice?> RequestLocalDevice()
        {
            var devices = await RequestDeviceList();
            var localDevice = devices.FirstOrDefault(NetworkUtils.IsLocalDevice);
            return localDevice != null ? new Device(localDevice, _connector) : null;
        }

        /// <summary>
        /// 获取主控制器设备
        /// </summary>
        public async Task<IDevice?> RequestMainController()
        {
            var devices = await RequestDeviceList();
            var mainController = devices.FirstOrDefault(x => x.IsMainDevice);
            return mainController != null ? new Device(mainController, _connector) : null;
        }

        /// <summary>
        /// 获取随机桌面设备
        /// </summary>
        public async Task<IDevice?> RequestRandomDesktopDevice()
        {
            var devices = await RequestDeviceList();
            var desktopDevices = devices.Where(x => x.DeviceOSType == OperatingSystems.Windows
                                                  || x.DeviceOSType == OperatingSystems.MacOS
                                                  || x.DeviceOSType == OperatingSystems.Linux)
                                      .ToList();
            
            if (!desktopDevices.Any())
                return null;

            var random = new Random();
            var randomDevice = desktopDevices[random.Next(desktopDevices.Count)];
            return new Device(randomDevice, _connector);
        }

        /// <summary>
        /// 获取随机移动设备
        /// </summary>
        public async Task<IDevice?> RequestRandomMobileDevice()
        {
            var devices = await RequestDeviceList();
            var mobileDevices = devices.Where(x => x.DeviceOSType == OperatingSystems.Android
                                                || x.DeviceOSType == OperatingSystems.IOS)
                                     .ToList();
            
            if (!mobileDevices.Any())
                return null;

            var random = new Random();
            var randomDevice = mobileDevices[random.Next(mobileDevices.Count)];
            return new Device(randomDevice, _connector);
        }

        /// <summary>
        /// 根据过滤器获取设备
        /// </summary>
        public async Task<IDevice?> RequestDeviceByFilter(Func<DeviceInfo, bool> filter)
        {
            var devices = await RequestDeviceList();
            var matchedDevice = devices.FirstOrDefault(filter);
            return matchedDevice != null ? new Device(matchedDevice, _connector) : null;
        }

        /// <summary>
        /// 获取用户选择的设备
        /// </summary>
        public async Task<IDevice?> RequestUserSelectedDevice(IEnumerable<DeviceInfo> candidates)
        {
            var response = await _requestBuilder
                .WithFunction("SelectDevice", candidates)
                .ExecuteAsync(async response =>
                {
                    var result = await ResponseHandler.HandleStringResponse(response);
                    if (string.IsNullOrEmpty(result))
                        return null;

                    return candidates.FirstOrDefault(d =>
                        d.Device.MacAddress.ToString().Equals(result, StringComparison.OrdinalIgnoreCase));
                });

            return response != null ? new Device(response, _connector) : null;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        public async Task<IEnumerable<DeviceInfo>> RequestDeviceList()
        {
            return await _deviceCache.GetOrAdd("devices", async () =>
            {
                var response = await _requestBuilder
                    .WithFunction("GetDeviceList")
                    .ExecuteAsync(async response =>
                    {
                        var result = await ResponseHandler.HandlePrefixedResponse<List<DeviceInfo>>(
                            response,
                            "DeviceList:");

                        if (result != null)
                            UpdateDeviceStates(result);

                        return result ?? new List<DeviceInfo>();
                    });

                return response;
            });
        }

        private void UpdateDeviceStates(IEnumerable<DeviceInfo> newDevices)
        {
            if (_isDisposed)
                return;

            lock (_lockObject)
            {
                var newDevicesDict = newDevices.ToDictionary(d => d.Device.MacAddress.ToString());

                // Find removed devices
                var removedDevices = _lastKnownDevices.Keys
                    .Except(newDevicesDict.Keys)
                    .Select(key => _lastKnownDevices[key])
                    .ToList();

                // Find added devices
                var addedDevices = newDevicesDict.Keys
                    .Except(_lastKnownDevices.Keys)
                    .Select(key => newDevicesDict[key])
                    .ToList();

                // Find changed devices
                var changedDevices = _lastKnownDevices.Keys
                    .Intersect(newDevicesDict.Keys)
                    .Where(key => HasDeviceChanged(_lastKnownDevices[key], newDevicesDict[key]))
                    .Select(key => newDevicesDict[key])
                    .ToList();

                // Update last known devices
                _lastKnownDevices = newDevicesDict;

                // Notify listeners
                var listeners = _listeners.ToList();
                foreach (var listener in listeners)
                {
                    try
                    {
                        foreach (var device in removedDevices)
                            listener.OnDeviceRemoved(device);

                        foreach (var device in addedDevices)
                            listener.OnDeviceAdded(device);

                        foreach (var device in changedDevices)
                            listener.OnDeviceStatusChanged(device);

                        listener.OnDeviceListUpdated(newDevices);
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other listeners
                        System.Diagnostics.Debug.WriteLine($"Error in device listener: {ex.Message}");
                    }
                }
            }
        }

        private bool HasDeviceChanged(DeviceInfo oldDevice, DeviceInfo newDevice)
        {
            // 检查设备在线状态（基于最后一次发送时间）
            var oldDeviceOnline = DateTime.UtcNow - oldDevice.SendTime < DeviceOfflineThreshold;
            var newDeviceOnline = DateTime.UtcNow - newDevice.SendTime < DeviceOfflineThreshold;

            if (oldDeviceOnline != newDeviceOnline)
                return true;

            // 检查关键属性变化
            return oldDevice.PluginsCount != newDevice.PluginsCount ||
                   oldDevice.Device.IPv4 != newDevice.Device.IPv4 ||
                   oldDevice.Device.IPv6 != newDevice.Device.IPv6 ||
                   oldDevice.PluginsServerPort != newDevice.PluginsServerPort ||
                   oldDevice.DevicesServerPort != newDevice.DevicesServerPort ||
                   oldDevice.IsMainDevice != newDevice.IsMainDevice ||
                   oldDevice.DeviceOSVersion != newDevice.DeviceOSVersion;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _deviceCache.Dispose();
            lock (_lockObject)
            {
                _listeners.Clear();
                _lastKnownDevices.Clear();
            }
        }
    }
}
