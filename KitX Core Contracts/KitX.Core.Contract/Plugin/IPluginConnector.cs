using System;
using KitX.Shared.CSharp.Plugin;
using KitX.Core.Contract.Plugin.Events;

namespace KitX.Core.Contract.Plugin;

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