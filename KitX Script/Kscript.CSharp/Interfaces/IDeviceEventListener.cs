using KitX.Shared.CSharp.Device;

namespace Kscript.CSharp.Interfaces
{
    /// <summary>
    /// 设备事件监听器接口
    /// </summary>
    public interface IDeviceEventListener
    {
        /// <summary>
        /// 当设备列表更新时触发
        /// </summary>
        /// <param name="devices">更新后的设备列表</param>
        void OnDeviceListUpdated(IEnumerable<DeviceInfo> devices);

        /// <summary>
        /// 当有新设备添加时触发
        /// </summary>
        /// <param name="device">新添加的设备</param>
        void OnDeviceAdded(DeviceInfo device);

        /// <summary>
        /// 当设备被移除时触发
        /// </summary>
        /// <param name="device">被移除的设备</param>
        void OnDeviceRemoved(DeviceInfo device);

        /// <summary>
        /// 当设备状态变化时触发
        /// </summary>
        /// <param name="device">状态发生变化的设备</param>
        void OnDeviceStatusChanged(DeviceInfo device);
    }
}