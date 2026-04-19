using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using KitX.Shared.CSharp.Device;

namespace KitX.Core.Contract.Device;

/// <summary>
/// Device management service interface
/// </summary>
public interface IDeviceService
{
    /// <summary>
    /// Gets the discovered devices list
    /// </summary>
    IReadOnlyList<IDeviceCase> DiscoveredDevices { get; }

    /// <summary>
    /// Gets the authorized devices list
    /// </summary>
    IReadOnlyList<IDeviceCase> AuthorizedDevices { get; }

    /// <summary>
    /// Gets the self device information
    /// </summary>
    DeviceInfo SelfDeviceInfo { get; }

    /// <summary>
    /// Gets a value indicating whether this device is the main device
    /// </summary>
    bool IsMainDevice { get; }

    /// <summary>
    /// Authorizes a device
    /// </summary>
    /// <param name="deviceId">The device ID</param>
    /// <param name="deviceKey">The device key</param>
    /// <returns>True if authorization was successful</returns>
    Task<bool> AuthorizeDeviceAsync(string deviceId, string deviceKey);

    /// <summary>
    /// Unauthorizes a device
    /// </summary>
    /// <param name="deviceId">The device ID</param>
    /// <returns>True if unauthorization was successful</returns>
    Task<bool> UnauthorizeDeviceAsync(string deviceId);

    /// <summary>
    /// Connects to a device
    /// </summary>
    /// <param name="deviceId">The device ID</param>
    /// <returns>True if connection was successful</returns>
    Task<bool> ConnectToDeviceAsync(string deviceId);

    /// <summary>
    /// Event raised when a device is discovered
    /// </summary>
    event EventHandler<DeviceDiscoveredEventArgs>? DeviceDiscovered;

    /// <summary>
    /// Event raised when a device goes offline
    /// </summary>
    event EventHandler<DeviceOfflineEventArgs>? DeviceOffline;

    /// <summary>
    /// Event raised when the main device changes
    /// </summary>
    event EventHandler<MainDeviceChangedEventArgs>? MainDeviceChanged;
}

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
}

/// <summary>
/// Devices organizer interface
/// </summary>
public interface IDevicesOrganizer
{
    /// <summary>
    /// Updates the source and adds device cards
    /// </summary>
    /// <param name="deviceInfo">The device info</param>
    void UpdateSourceAndAddCards(DeviceInfo deviceInfo);

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
/// Main device changed event arguments
/// </summary>
public class MainDeviceChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the old main device ID
    /// </summary>
    public string OldMainDeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new main device ID
    /// </summary>
    public string NewMainDeviceId { get; set; } = string.Empty;
}
