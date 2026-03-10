using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Security;
using KitXIDeviceKey = KitX.Core.Contract.Configuration.IDeviceKey;

namespace KitX.Core.Contract.Security;

/// <summary>
/// Security management service interface
/// </summary>
public interface ISecurityService
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
    /// Encrypts a string using RSA with a specific device's public key
    /// </summary>
    /// <param name="key">The device key containing the public key</param>
    /// <param name="data">The data to encrypt</param>
    /// <returns>The encrypted data as Base64 string</returns>
    string? RsaEncryptString(DeviceKey key, string data);

    /// <summary>
    /// Decrypts a string using RSA with a specific device's private key
    /// </summary>
    /// <param name="key">The device key containing the private key</param>
    /// <param name="encryptedData">The encrypted data as Base64 string</param>
    /// <returns>The decrypted data</returns>
    string? RsaDecryptString(DeviceKey key, string encryptedData);

    /// <summary>
    /// Encrypts content using RSA+AES hybrid encryption
    /// </summary>
    /// <param name="key">The device key</param>
    /// <param name="content">The content to encrypt</param>
    /// <returns>The encrypted content</returns>
    EncryptedContent RsaEncryptContent(DeviceKey key, string content);

    /// <summary>
    /// Decrypts content using RSA+AES hybrid decryption
    /// </summary>
    /// <param name="key">The device key</param>
    /// <param name="content">The encrypted content</param>
    /// <returns>The decrypted content</returns>
    string RsaDecryptContent(DeviceKey key, EncryptedContent content);

    /// <summary>
    /// Encrypts a string with AES
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="key">The encryption key</param>
    /// <returns>The encrypted string</returns>
    string AesEncrypt(string source, string key);

    /// <summary>
    /// Decrypts a string with AES
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="key">The decryption key</param>
    /// <param name="isSourceInBase64">Whether the source is in Base64</param>
    /// <returns>The decrypted string</returns>
    string AesDecrypt(string source, string key, bool isSourceInBase64 = true);

    /// <summary>
    /// Computes a hash
    /// </summary>
    /// <param name="content">The content to hash</param>
    /// <returns>The hash</returns>
    string ComputeHash(string content);

    /// <summary>
    /// Computes SHA1 hash of a string
    /// </summary>
    /// <param name="data">The data to hash</param>
    /// <returns>The SHA1 hash string</returns>
    string GetSHA1(string data);
}
