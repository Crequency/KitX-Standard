using System.Collections.Generic;
using KitX.Shared.CSharp.Device;
using KitXIDeviceKey = KitX.Core.Contract.Configuration.IDeviceKey;

namespace KitX.Core.Contract.Security;

/// <summary>
/// Device key management service interface
/// </summary>
public interface IDeviceKeyService
{
    /// <summary>
    /// Gets all device keys
    /// </summary>
    IReadOnlyList<KitXIDeviceKey> GetDeviceKeys();

    /// <summary>
    /// Adds a device key
    /// </summary>
    /// <param name="macAddress">The MAC address</param>
    /// <param name="deviceName">The device name</param>
    /// <param name="publicKey">The public key</param>
    /// <returns>True if addition was successful</returns>
    bool AddDeviceKey(string macAddress, string deviceName, string publicKey);

    /// <summary>
    /// Removes a device key
    /// </summary>
    /// <param name="macAddress">The MAC address</param>
    /// <returns>True if removal was successful</returns>
    bool RemoveDeviceKey(string macAddress);

    /// <summary>
    /// Searches for a device key by device locator
    /// </summary>
    /// <param name="locator">The device locator</param>
    /// <returns>The device key if found, otherwise null</returns>
    DeviceKey? SearchDeviceKey(DeviceLocator locator);

    /// <summary>
    /// Checks if a device key is correct
    /// </summary>
    /// <param name="locator">The device locator</param>
    /// <param name="key">The device key to verify</param>
    /// <returns>True if the key is correct</returns>
    bool IsDeviceKeyCorrect(DeviceLocator locator, DeviceKey key);

    /// <summary>
    /// Checks if a device is authorized
    /// </summary>
    /// <param name="device">The device locator</param>
    /// <returns>True if the device is authorized</returns>
    bool IsDeviceAuthorized(DeviceLocator device);

    /// <summary>
    /// Gets the private device key for local device
    /// </summary>
    /// <returns>The private device key, or null if not available</returns>
    DeviceKey? GetPrivateDeviceKey();
}
