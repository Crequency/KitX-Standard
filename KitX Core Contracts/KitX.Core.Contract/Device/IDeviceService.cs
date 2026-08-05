using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using KitX.Shared.CSharp.Device;

namespace KitX.Core.Contract.Device;

/// <summary>
/// Device discovery service interface
/// </summary>
public interface IDeviceDiscoveryService
{
    /// <summary>
    /// Gets the default device information
    /// </summary>
    DeviceInfo DefaultDeviceInfo { get; }

    /// <summary>
    /// Gets the port the discovery service is running on
    /// </summary>
    int? Port { get; }

    /// <summary>
    /// Starts the device discovery service
    /// </summary>
    /// <returns>The service instance</returns>
    IDeviceDiscoveryService Run();

    /// <summary>
    /// Stops the device discovery service
    /// </summary>
    void Stop();

    /// <summary>
    /// Event raised when a device is discovered
    /// </summary>
    event EventHandler<DeviceDiscoveredEventArgs>? DeviceDiscovered;

    /// <summary>
    /// Event raised when a device goes offline
    /// </summary>
    event EventHandler<DeviceOfflineEventArgs>? DeviceOffline;
}

/// <summary>
/// Device server interface for HTTP API
/// </summary>
public interface IDeviceServer
{
    /// <summary>
    /// Gets the port the server is running on
    /// </summary>
    int? Port { get; }

    /// <summary>
    /// Starts the device server
    /// </summary>
    /// <returns>The server instance</returns>
    IDeviceServer Run();

    /// <summary>
    /// Stops the device server
    /// </summary>
    void Stop();

    /// <summary>
    /// Checks if a device is signed in
    /// </summary>
    /// <param name="locator">The device locator</param>
    /// <returns>True if the device is signed in</returns>
    bool IsDeviceSignedIn(DeviceLocator locator);

    /// <summary>
    /// Gets the signed device token for a device locator
    /// </summary>
    /// <param name="locator">The device locator</param>
    /// <returns>The token or null if not found</returns>
    string? GetDeviceToken(DeviceLocator locator);

    /// <summary>
    /// Gets all signed-in device locators
    /// </summary>
    /// <returns>Read-only list of signed-in device locators</returns>
    IReadOnlyList<DeviceLocator> GetSignedInDevices();
}

/// <summary>
/// Devices organizer interface
/// </summary>
public interface IDevicesOrganizer
{
    /// <summary>
    /// Event raised when a device is discovered
    /// </summary>
    event EventHandler<DeviceDiscoveredEventArgs>? DeviceDiscovered;

    /// <summary>
    /// Event raised when a device goes offline
    /// </summary>
    event EventHandler<DeviceOfflineEventArgs>? DeviceOffline;
}

/// <summary>
/// Device case interface
/// </summary>
public interface IDeviceCase
{
    /// <summary>
    /// Gets or sets the device information
    /// </summary>
    DeviceInfo DeviceInfo { get; set; }

    /// <summary>
    /// Gets a value indicating whether the device is authorized
    /// </summary>
    bool IsAuthorized { get; }

    /// <summary>
    /// Gets a value indicating whether this is the main device
    /// </summary>
    bool IsMainDevice { get; }

    /// <summary>
    /// Gets a value indicating whether the device is online
    /// </summary>
    bool IsOnline { get; }

    /// <summary>
    /// Gets the last seen time
    /// </summary>
    DateTime LastSeen { get; }
}

/// <summary>
/// Device discovered event arguments
/// </summary>
public class DeviceDiscoveredEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the device information
    /// </summary>
    public DeviceInfo? DeviceInfo { get; set; }
}

/// <summary>
/// Device offline event arguments
/// </summary>
public class DeviceOfflineEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the device ID
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;
}

/// <summary>
/// Device HTTP client interface — sends requests to remote DevicesServer instances.
/// Used for cross-device plugin invocation via the /Api/V1/Plugin/Invoke endpoint.
/// (Moved to Contract so the Workflow library can depend on the abstraction without
/// referencing KitX.Core.)
/// </summary>
public interface IDeviceHttpClient
{
    /// <summary>
    /// Invokes a plugin method on a remote device via HTTP POST to /Api/V1/Plugin/Invoke.
    /// </summary>
    /// <param name="targetDevice">Target device info (contains IPv4 and DevicesServerPort)</param>
    /// <param name="token">Valid session token for the target device</param>
    /// <param name="request">The Request object to send</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>HTTP response from remote device, or null on network error</returns>
    Task<HttpResponseMessage?> InvokePluginAsync(
        DeviceInfo targetDevice,
        string token,
        KitX.Shared.CSharp.WebCommand.Request request,
        CancellationToken ct = default);
}
