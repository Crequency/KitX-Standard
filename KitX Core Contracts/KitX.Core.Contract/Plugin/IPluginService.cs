using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitX.Core.Contract.Configuration;
using KitX.Shared.CSharp.Plugin;
using KitX.Core.Contract.Plugin.Events;

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