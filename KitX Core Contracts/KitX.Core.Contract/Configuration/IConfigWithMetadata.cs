using System;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Interface for configurations with metadata fields
/// </summary>
public interface IConfigWithMetadata
{
    /// <summary>
    /// Gets or sets the configuration file location
    /// </summary>
    string? ConfigFileLocation { get; set; }

    /// <summary>
    /// Gets or sets the configuration file watcher name
    /// </summary>
    string? ConfigFileWatcherName { get; set; }

    /// <summary>
    /// Gets or sets the configuration generated time
    /// </summary>
    DateTime? ConfigGeneratedTime { get; set; }
}
