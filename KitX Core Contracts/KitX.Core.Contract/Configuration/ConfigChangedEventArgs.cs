using System;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Configuration changed event arguments
/// </summary>
public class ConfigChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the configuration type (e.g., "App", "Plugins", "Security")
    /// </summary>
    public string ConfigType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the property name that changed
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the old value
    /// </summary>
    public object? OldValue { get; set; }

    /// <summary>
    /// Gets or sets the new value
    /// </summary>
    public object? NewValue { get; set; }
}
