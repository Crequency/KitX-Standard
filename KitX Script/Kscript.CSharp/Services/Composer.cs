using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;
using System.Text.Json;

namespace Kscript.CSharp.Services
{
    public class Composer : IComposer
    {
        private readonly Connector _connector = Connector.Instance;
        private IEnumerable<DeviceInfo>? _cachedDeviceList;

        public async Task<IDevice?> RequestLocalDevice()
        {
            var devices = await RequestDeviceList();
            var localDevice = devices.FirstOrDefault(x => 
                (x.Device.IPv4?.Equals("127.0.0.1") ?? false) || // Check localhost
                (x.Device.IPv4?.Equals("::1") ?? false)); // Check IPv6 localhost
                                                          // todo: fixme: 这个检查方案会有问题。改进建议：检查IP地址是否与本地设备的IP地址有交集。因为广播出来的地址可能是本机地址的外网地址

            return localDevice != null ? new Device(localDevice) : null;
        }

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
            if (desktopDevices.Count == 0) return null;

            var random = new Random();
            var randomDevice = desktopDevices[random.Next(desktopDevices.Count)];
            return new Device(randomDevice);
        }

        public async Task<IDevice?> RequestRandomMobileDevice()
        {
            var devices = await RequestDeviceList();
            var mobileDevices = devices.Where(x => x.DeviceOSType == OperatingSystems.Android
                                                                                               || x.DeviceOSType == OperatingSystems.IOS).ToList();
            if (mobileDevices.Count == 0) return null;
            
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

        public async Task<IDevice?> RequestUserSelectedDevice(IEnumerable<DeviceInfo> candidates)
        {
            var tcs = new TaskCompletionSource<DeviceInfo?>();

            void OnResponse(Request response)
            {
                try
                {
                    response.Match(
                        response.GetContent(content => // 这里返回的是用户选择的设备的 MAC 地址
                        {
                            // Parse selected device from response
                            if (string.IsNullOrEmpty(content))
                            {
                                tcs.SetResult(null);
                                return content;
                            }

                            var selectedDevice = candidates.FirstOrDefault(d => 
                                d.Device.MacAddress.ToString().Equals(content, StringComparison.OrdinalIgnoreCase));
                            tcs.SetResult(selectedDevice);
                            return content;
                        }),
                        matchCommand: ProcessCommandResponse
                    );
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            // Send candidates to local device for selection
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = "SelectDevice";
                    cmd.FunctionArgs = candidates.Select(d => new Parameter { Value = d.ToString() }).ToList();
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Type = RequestTypes.Command;
                    req.Version = RequestVersions.V1;
                    req.Target = null; // Send to local device
                    return req;
                })
                .SetSender(OnResponse)
                .Send();

            var selectedDevice = await tcs.Task;
            return selectedDevice != null ? new Device(selectedDevice) : null;
        }

        public async Task<IEnumerable<DeviceInfo>> RequestDeviceList()
        {
            if (_cachedDeviceList != null)
                return _cachedDeviceList;

            var tcs = new TaskCompletionSource<IEnumerable<DeviceInfo>>();

            void OnResponse(Request response)
            {
                try
                {
                    var devices = HandleDeviceListResponse(response);
                    _cachedDeviceList = devices;
                    tcs.SetResult(devices);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            // Request device list from network
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = "GetDeviceList";
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Type = RequestTypes.Command;
                    req.Version = RequestVersions.V1;
                    return req;
                })
                .SetSender(OnResponse)
                .Send();

            return await tcs.Task;
        }

        private IEnumerable<DeviceInfo> HandleDeviceListResponse(Request response)
        {
            var devices = new List<DeviceInfo>();
            
            response.Match(
                response.GetContent(content =>
                {
                    // Parse device list from content
                    // This should be replaced with proper deserialization based on response format
                    // For now returning empty list as placeholder
                    return content;
                }),
                matchCommand: content =>
                {
                    ProcessCommandResponse(content);
                }
            );

            return devices;
        }

        private void ProcessCommandResponse(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            try
            {
                // Handle different command responses based on content
                if (content.StartsWith("DeviceList:"))
                {
                    var deviceListJson = content["DeviceList:".Length..];
                    _cachedDeviceList = JsonSerializer.Deserialize<List<DeviceInfo>>(deviceListJson);
                }
                else if (content.StartsWith("Error:"))
                {
                    var error = content["Error:".Length..];
                    throw new InvalidOperationException($"Command error: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to process command response: {ex.Message}", ex);
            }
        }
    }
}
