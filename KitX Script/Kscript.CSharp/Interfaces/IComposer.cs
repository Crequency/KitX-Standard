using KitX.Shared.CSharp.Device;

namespace Kscript.CSharp.Interfaces
{
    public interface IComposer
    {
        Task<IDevice?> RequestLocalDevice();
        Task<IDevice?> RequestMainController();
        Task<IDevice?> RequestRandomDesktopDevice();
        Task<IDevice?> RequestRandomMobileDevice();
        Task<IDevice?> RequestDeviceByFilter(Func<DeviceInfo, bool> filter);
        Task<IDevice?> RequestUserSelectedDevice(IEnumerable<DeviceInfo> candidates);
        Task<IEnumerable<DeviceInfo>> RequestDeviceList();
    }
}
