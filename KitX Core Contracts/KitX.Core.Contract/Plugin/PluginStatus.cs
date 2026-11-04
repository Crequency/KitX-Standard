namespace KitX.Core.Contract.Plugin;

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