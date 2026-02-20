using System;
using System.ComponentModel;

namespace KitX.Core.Contract.Events;

/// <summary>
/// Base class for event arguments
/// </summary>
public abstract class BaseEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the timestamp when the event occurred
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Generic event arguments for simple event data
/// </summary>
public class GenericEventArgs : BaseEventArgs
{
    /// <summary>
    /// Gets or sets the event data
    /// </summary>
    public object? Data { get; set; }
}

/// <summary>
/// Generic event arguments with type parameter
/// </summary>
public class GenericEventArgs<T> : BaseEventArgs
{
    /// <summary>
    /// Gets or sets the event data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the event type
    /// </summary>
    public string? EventType { get; set; }
}
