using System.Threading.Tasks;
using KitX.Shared.CSharp.Security;

namespace KitX.Core.Contract.Security;

/// <summary>
/// Encryption service interface
/// </summary>
public interface IEncryptionService
{
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
    string? RsaEncryptString(Shared.CSharp.Device.DeviceKey key, string data);

    /// <summary>
    /// Decrypts a string using RSA with a specific device's private key
    /// </summary>
    /// <param name="key">The device key containing the private key</param>
    /// <param name="encryptedData">The encrypted data as Base64 string</param>
    /// <returns>The decrypted data</returns>
    string? RsaDecryptString(Shared.CSharp.Device.DeviceKey key, string encryptedData);

    /// <summary>
    /// Encrypts content using RSA+AES hybrid encryption
    /// </summary>
    /// <param name="key">The device key</param>
    /// <param name="content">The content to encrypt</param>
    /// <returns>The encrypted content</returns>
    EncryptedContent RsaEncryptContent(Shared.CSharp.Device.DeviceKey key, string content);

    /// <summary>
    /// Decrypts content using RSA+AES hybrid decryption
    /// </summary>
    /// <param name="key">The device key</param>
    /// <param name="content">The encrypted content</param>
    /// <returns>The decrypted content</returns>
    string RsaDecryptContent(Shared.CSharp.Device.DeviceKey key, EncryptedContent content);

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
    /// Computes SHA1 hash of a string
    /// </summary>
    /// <param name="data">The data to hash</param>
    /// <returns>The SHA1 hash string</returns>
    string GetSHA1(string data);
}
