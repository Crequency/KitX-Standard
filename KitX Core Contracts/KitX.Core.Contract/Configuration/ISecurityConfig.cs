using System;
using System.Collections.Generic;
using KitX.Shared.CSharp.Device;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Security configuration interface
/// </summary>
public interface ISecurityConfig
{
    /// <summary>
    /// Gets or sets the device keys list
    /// </summary>
    IList<IDeviceKey> DeviceKeys { get; set; }
}

/// <summary>
/// Device key interface
/// </summary>
public interface IDeviceKey
{
    /// <summary>
    /// Gets the device locator
    /// </summary>
    DeviceLocator Device { get; }

    /// <summary>
    /// Gets the RSA public key in PEM format
    /// </summary>
    string? RsaPublicKeyPem { get; }

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