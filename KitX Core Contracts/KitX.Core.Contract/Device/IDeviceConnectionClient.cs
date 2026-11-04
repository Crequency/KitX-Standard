using System.Threading;
using System.Threading.Tasks;
using KitX.Shared.CSharp.Device;

namespace KitX.Core.Contract.Device;

/// <summary>
/// Result of an initiating device key exchange.
/// </summary>
public class ExchangeKeyResult
{
    /// <summary>
    /// Whether the exchange completed successfully (the remote public key was stored).
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The remote device key exchanged back, when <see cref="Success"/> is true.
    /// </summary>
    public DeviceKey? RemoteDeviceKey { get; set; }

    /// <summary>
    /// Optional error/status description when the exchange failed.
    /// </summary>
    public string? Error { get; set; }
}

/// <summary>
/// Outbound client for the device encrypted-authentication connection.
/// Initiates key exchange and connection against a remote <c>DevicesServer</c>,
/// mirroring the receive-side endpoints implemented in
/// <c>KitX.Core.Device.DevicesServer</c>. The server side is the trust anchor; this
/// client only ever sends public keys and never leaks the local private key.
/// </summary>
public interface IDeviceConnectionClient
{
    /// <summary>
    /// Initiates a device key exchange (leg 1: send local public key, receive remote public key).
    /// POSTs the local public key AES-encrypted with the temporary password, then decrypts
    /// and stores the remote device's public key returned in the response.
    /// </summary>
    /// <param name="targetDevice">Target device info (IPv4 + DevicesServerPort)</param>
    /// <param name="password">Temporary 8-digit password displayed to the user</param>
    /// <param name="ct">Cancellation token</param>
    Task<ExchangeKeyResult> ExchangeKeyAsync(DeviceInfo targetDevice, string password, CancellationToken ct = default);

    /// <summary>
    /// Sends the second leg of a key exchange for protocol compatibility.
    /// </summary>
    Task<bool> ExchangeKeyBackAsync(DeviceInfo targetDevice, string password, CancellationToken ct = default);

    /// <summary>
    /// Cancels an in-progress key exchange on the remote device.
    /// </summary>
    Task<bool> CancelExchangingKeyAsync(DeviceInfo targetDevice, CancellationToken ct = default);

    /// <summary>
    /// Authenticates against the remote device and obtains a session token.
    /// POSTs the base64-encoded local device locator plus the local device name
    /// RSA-encrypted to the target's public key; returns the decrypted session token.
    /// </summary>
    /// <param name="targetDevice">Target device info (IPv4 + DevicesServerPort)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The session token, or null on failure.</returns>
    Task<string?> ConnectAsync(DeviceInfo targetDevice, CancellationToken ct = default);
}
