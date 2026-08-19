using System;
using KitX.Shared.CSharp.Plugin;
using KitX.Core.Contract.Device;
using KitX.Core.Contract.Plugin.Events;
using CTask = System.Threading.Tasks.Task;

namespace KitX.Core.Contract.Plugin;

/// <summary>
/// Plugin connection interface
/// </summary>
public interface IPluginConnection : IPluginConnector
{
    /// <summary>
    /// Gets or sets the plugin info
    /// </summary>
    new PluginInfo? PluginInfo { get; set; }

    /// <summary>
    /// Gets the connection status
    /// </summary>
    ServerStatus Status { get; }

    /// <summary>
    /// Event raised when a message is received. The event args carry the raw
    /// <see cref="PluginMessageReceivedEventArgs.Message"/> and, when the source parsed it,
    /// the already-deserialized <see cref="PluginMessageReceivedEventArgs.Request"/> and
    /// <see cref="PluginMessageReceivedEventArgs.Command"/> so downstream handlers do not
    /// re-deserialize the same message.
    /// </summary>
    event EventHandler<PluginMessageReceivedEventArgs>? MessageReceived;

    /// <summary>
    /// Event raised when connection is closed
    /// </summary>
    event EventHandler? Closed;

    /// <summary>
    /// Initializes the connection
    /// </summary>
    void Initialize();

    /// <summary>
    /// Sends a message
    /// </summary>
    /// <param name="message">The message to send</param>
    void Send(string message);

    /// <summary>
    /// Closes the connection
    /// </summary>
    CTask CloseAsync();
}
