using System;
using System.Collections.Generic;
using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Loader;
using KitX.Shared.CSharp.Plugin;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Plugins configuration interface
/// </summary>
public interface IPluginsConfig
{
    /// <summary>
    /// Gets or sets the list of plugin installations
    /// </summary>
    IList<IPluginInstallation> Plugins { get; set; }
}

/// <summary>
/// Plugin installation interface
/// </summary>
public interface IPluginInstallation
{
    /// <summary>
    /// Gets the unique identifier for this plugin installation
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the installation path
    /// </summary>
    string? InstallPath { get; }

    /// <summary>
    /// Gets or sets the plugin information
    /// </summary>
    PluginInfo? PluginInfo { get; set; }

    /// <summary>
    /// Gets or sets the loader information
    /// </summary>
    LoaderInfo? LoaderInfo { get; set; }

    /// <summary>
    /// Gets or sets the list of installed devices
    /// </summary>
    IList<DeviceLocator> InstalledDevices { get; set; }
}
