using System;
using System.Collections.Generic;
using KitX.Shared.CSharp.Plugin;
using KitX.Core.Contract.Plugin.Events;

namespace KitX.Core.Contract.Plugin;

/// <summary>
/// Plugin server interface for managing plugin connections
/// </summary>
public interface IPluginServer
{
    /// <summary>
    /// Gets the port the server is running on
    /// </summary>
    int? Port { get; }

    /// <summary>
    /// Gets the list of currently connected plugins
    /// </summary>
    IReadOnlyList<IPluginConnector> Connections { get; }

    /// <summary>
    /// Starts the plugin server
    /// </summary>
    /// <returns>The server instance</returns>
    IPluginServer Run();

    /// <summary>
    /// Stops the plugin server
    /// </summary>
    void Stop();

    /// <summary>
    /// Finds a connector for a specific plugin
    /// </summary>
    /// <param name="pluginInfo">The plugin info</param>
    /// <returns>The plugin connector or null if not found</returns>
    IPluginConnector? FindConnector(PluginInfo pluginInfo);

    /// <summary>
    /// Event raised when server port changes
    /// </summary>
    event EventHandler<int>? PortChanged;

    /// <summary>
    /// Event raised when a plugin registers with the server
    /// </summary>
    event EventHandler<PluginRegisteredEventArgs>? PluginRegistered;

    /// <summary>
    /// Event raised when a plugin unregisters/disconnects from the server
    /// </summary>
    event EventHandler<PluginUnregisteredEventArgs>? PluginUnregistered;
}