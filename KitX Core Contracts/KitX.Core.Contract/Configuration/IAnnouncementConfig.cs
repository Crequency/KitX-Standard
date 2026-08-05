using System.Collections.Generic;

namespace KitX.Core.Contract.Configuration;

public interface IAnnouncementConfig
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
