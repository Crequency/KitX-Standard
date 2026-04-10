using System;
using KitX.Core.Contract.Configuration;
using KitX.Shared.CSharp.Plugin;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Plugin;

/// <summary>
/// Plugin management service interface
/// </summary>
public interface IPluginService
{
    /// <summary>
    /// Gets all installed plugins
    /// </summary>
    IReadOnlyList<IPluginInstallation> GetInstalledPlugins();

    /// <summary>
    /// Gets a plugin by its ID
    /// </summary>
    /// <param name="pluginId">The plugin ID</param>
    /// <returns>The plugin installation or null if not found</returns>
    IPluginInstallation? GetPlugin(Guid pluginId);

    /// <summary>
    /// Imports a plugin package (.kxp file)
    /// </summary>
    /// <param name="kxpFilePath">Path to the .kxp file</param>
    /// <returns>True if import was successful</returns>
    Task<bool> ImportPluginAsync(string kxpFilePath);

    /// <summary>
    /// Removes a plugin
    /// </summary>
    /// <param name="pluginId">The plugin ID</param>
    /// <returns>True if removal was successful</returns>
    Task<bool> RemovePluginAsync(Guid pluginId);

    /// <summary>
    /// Starts a plugin
    /// </summary>
    /// <param name="pluginId">The plugin ID</param>
    /// <returns>True if start was successful</returns>
    Task<bool> StartPluginAsync(Guid pluginId);

    /// <summary>
    /// Stops a plugin
    /// </summary>
    /// <param name="pluginId">The plugin ID</param>
    /// <returns>True if stop was successful</returns>
    Task<bool> StopPluginAsync(Guid pluginId);

    /// <summary>
    /// Calls a plugin function
    /// </summary>
    /// <param name="pluginId">The plugin ID</param>
    /// <param name="functionName">The function name</param>
    /// <param name="parameters">Optional parameters</param>
    /// <returns>The function result</returns>
    Task<object?> CallPluginFunctionAsync(Guid pluginId, string functionName, Dictionary<string, object>? parameters = null);

    /// <summary>
    /// Event raised when plugin status changes
    /// </summary>
    event EventHandler<PluginStatusChangedEventArgs>? PluginStatusChanged;
}

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

/// <summary>
/// Plugin connector interface for managing individual plugin connections
/// </summary>
public interface IPluginConnector
{
    /// <summary>
    /// Gets the connection ID
    /// </summary>
    string ConnectionId { get; }

    /// <summary>
    /// Gets the plugin info
    /// </summary>
    PluginInfo? PluginInfo { get; }

    /// <summary>
    /// Sends a request to the plugin
    /// </summary>
    /// <param name="request">The request to send</param>
    void Request(object request);

    /// <summary>
    /// Event raised when a plugin response is received
    /// </summary>
    event EventHandler<PluginResponseEventArgs>? PluginResponse;

    /// <summary>
    /// Event raised when plugin reports status
    /// </summary>
    event EventHandler<PluginStatusReportEventArgs>? StatusReport;
}

/// <summary>
/// Plugin status enumeration
/// </summary>
public enum PluginStatus
{
    /// <summary>
    /// Unknown status
    /// </summary>
    Unknown,

    /// <summary>
    /// Plugin is installed but not running
    /// </summary>
    Installed,

    /// <summary>
    /// Plugin is running
    /// </summary>
    Running,

    /// <summary>
    /// Plugin was running but is now stopped
    /// </summary>
    Stopped,

    /// <summary>
    /// Plugin encountered an error
    /// </summary>
    Error
}

/// <summary>
/// Plugin status changed event arguments
/// </summary>
public class PluginStatusChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the plugin ID
    /// </summary>
    public Guid PluginId { get; set; }

    /// <summary>
    /// Gets or sets the plugin name
    /// </summary>
    public string PluginName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the old status
    /// </summary>
    public PluginStatus OldStatus { get; set; }

    /// <summary>
    /// Gets or sets the new status
    /// </summary>
    public PluginStatus NewStatus { get; set; }
}

/// <summary>
/// Plugin response event arguments
/// </summary>
public class PluginResponseEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the request ID
    /// </summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the response content
    /// </summary>
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// Plugin status report event arguments
/// </summary>
public class PluginStatusReportEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the connection ID
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status message
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Plugin registered event arguments
/// </summary>
public class PluginRegisteredEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the plugin info
    /// </summary>
    public PluginInfo? PluginInfo { get; set; }
}

/// <summary>
/// Plugin unregistered event arguments
/// </summary>
public class PluginUnregisteredEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the plugin info
    /// </summary>
    public PluginInfo? PluginInfo { get; set; }
}
