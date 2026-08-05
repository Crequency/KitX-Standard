using System;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Configuration management service interface
/// </summary>
public interface IConfigService
{
    /// <summary>
    /// Gets the application configuration
    /// </summary>
    IAppConfig AppConfig { get; }

    /// <summary>
    /// Gets the plugins configuration
    /// </summary>
    IPluginsConfig PluginsConfig { get; }

    /// <summary>
    /// Gets the security configuration
    /// </summary>
    ISecurityConfig SecurityConfig { get; }

    /// <summary>
    /// Loads all configurations from files
    /// </summary>
    void Load();

    /// <summary>
    /// Saves all configurations to files
    /// </summary>
    void SaveAll();

    /// <summary>
    /// Reloads all configurations from files
    /// </summary>
    void Reload();

    /// <summary>
    /// Event raised when configuration changes
    /// </summary>
    event EventHandler<ConfigChangedEventArgs>? ConfigChanged;
}
