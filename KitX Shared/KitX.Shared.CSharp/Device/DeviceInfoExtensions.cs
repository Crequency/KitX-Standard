using System;

namespace KitX.Shared.CSharp.Device;

/// <summary>
/// Extension methods for DeviceInfo
/// </summary>
public static class DeviceInfoExtensions
{
    /// <summary>
    /// Checks if a device is offline (not seen within TTL period)
    /// </summary>
    /// <param name="info">The device info</param>
    /// <param name="ttlSeconds">Time-to-live in seconds</param>
    /// <returns>True if the device is offline</returns>
    public static bool IsOffline(this DeviceInfo info, int ttlSeconds) =>
        DateTime.UtcNow - info.SendTime.ToUniversalTime() > TimeSpan.FromSeconds(ttlSeconds);

    /// <summary>
    /// Checks if this device is the current device
    /// </summary>
    /// <param name="info">The device info</param>
    /// <param name="selfDeviceInfo">The current device info</param>
    /// <returns>True if this is the current device</returns>
    public static bool IsCurrentDevice(this DeviceInfo info, DeviceInfo selfDeviceInfo) =>
        info.IsSameDevice(selfDeviceInfo);

    /// <summary>
    /// Checks if two device infos represent the same device
    /// </summary>
    /// <param name="info">The first device info</param>
    /// <param name="target">The target device info</param>
    /// <returns>True if they are the same device</returns>
    public static bool IsSameDevice(this DeviceInfo info, DeviceInfo target) =>
        info.Device.IsSameDevice(target.Device);

    /// <summary>
    /// Updates this device info with data from another device info
    /// </summary>
    /// <param name="info">The device info to update</param>
    /// <param name="target">The source device info</param>
    public static void UpdateTo(this DeviceInfo info, DeviceInfo target)
    {
        var type = typeof(DeviceInfo);

        var fields = type.GetFields();

        foreach (var field in fields)
        {
            var firstValue = field.GetValue(info);
            var secondValue = field.GetValue(target);

            if (firstValue?.Equals(secondValue) ?? false)
                continue;

            field.SetValue(info, secondValue);
        }

        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (!property.CanWrite)
                continue;

            object? firstValue = property.GetValue(info);
            object? secondValue = property.GetValue(target);

            if (firstValue?.Equals(secondValue) ?? false)
                continue;

            property.SetValue(info, secondValue);
        }
    }
}
