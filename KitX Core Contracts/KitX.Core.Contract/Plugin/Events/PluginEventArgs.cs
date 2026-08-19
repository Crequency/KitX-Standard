using System;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;

namespace KitX.Core.Contract.Plugin.Events;

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

/// <summary>
/// Plugin disconnected event arguments
/// </summary>
public class PluginDisconnectedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the connection ID
    /// </summary>
    public string? ConnectionId { get; set; }
}

/// <summary>
/// Plugin message received event arguments
/// </summary>
public class PluginMessageReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the connection ID
    /// </summary>
    public string? ConnectionId { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the already-deserialized request, when the raising source parsed it.
    /// <see langword="null"/> means the message was not parsed and consumers must deserialize
    /// <see cref="Message"/> themselves.
    /// </summary>
    public Request? Request { get; set; }

    /// <summary>
    /// Gets or sets the already-deserialized command, when the raising source parsed it.
    /// <see langword="null"/> means the command was not parsed and consumers must deserialize
    /// it from <see cref="Request.Content"/> themselves.
    /// </summary>
    public Command? Command { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message is a plugin response (carries a
    /// <c>RequestId</c> tag). <see langword="null"/> when the source did not determine it.
    /// </summary>
    public bool? IsResponse { get; set; }
}