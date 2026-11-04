using System;
using System.Collections.Generic;

namespace KitX.Core.Contract.Configuration;

public interface IAnnouncementConf
{
    /// <summary>
    /// Gets or sets the list of accepted announcement IDs
    /// </summary>
    List<string> Accepted { get; set; }

    /// <summary>
    /// Gets or sets the config file location
    /// </summary>
    string? ConfigFileLocation { get; set; }
}

/// <summary>
/// Backward-compatible alias of <see cref="IAnnouncementConf"/>.
/// Deprecated: use <see cref="IAnnouncementConf"/> instead.
/// </summary>
[Obsolete("Use IAnnouncementConf instead.")]
public interface IAnnouncementConfig : IAnnouncementConf
{
}
