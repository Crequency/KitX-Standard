using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using KitX.Shared.CSharp.Device;

namespace KitX.Core.Contract.Security;

/// <summary>
/// Security management service interface
/// </summary>
public interface ISecurityService
{
    /// <summary>
    /// Gets all device keys
    /// </summary>
    IReadOnlyList<IDeviceKey> GetDeviceKeys();

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
    /// Checks if a device is authorized
    /// </summary>
    /// <param name="device">The device locator</param>
    /// <returns>True if the device is authorized</returns>
    bool IsDeviceAuthorized(DeviceLocator device);

    /// <summary>
    /// Encrypts a string
    /// </summary>
    /// <param name="content">The content to encrypt</param>
    /// <param name="targetDeviceMacAddress">The target device MAC address</param>
    /// <returns>The encrypted content</returns>
    Task<string> EncryptStringAsync(string content, string targetDeviceMacAddress);

    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="encryptedContent">The encrypted content</param>
    /// <param name="sourceDeviceMacAddress">The source device MAC address</param>
    /// <returns>The decrypted content</returns>
    Task<string> DecryptStringAsync(string encryptedContent, string sourceDeviceMacAddress);

    /// <summary>
    /// Computes a hash
    /// </summary>
    /// <param name="content">The content to hash</param>
    /// <returns>The hash</returns>
    string ComputeHash(string content);
}

/// <summary>
/// Device key interface
/// </summary>
public interface IDeviceKey
{
    /// <summary>
    /// Gets the MAC address
    /// </summary>
    string MacAddress { get; }

    /// <summary>
    /// Gets the device name
    /// </summary>
    string DeviceName { get; }

    /// <summary>
    /// Gets the public key
    /// </summary>
    string PublicKey { get; }

    /// <summary>
    /// Gets the time when the key was added
    /// </summary>
    DateTime AddedAt { get; }
}
