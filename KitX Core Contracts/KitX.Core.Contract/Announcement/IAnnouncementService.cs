using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using KitX.Core.Contract.Configuration;

namespace KitX.Core.Contract.Announcement;

/// <summary>
/// Announcement service interface
/// </summary>
public interface IAnnouncementService
{
    /// <summary>
    /// Gets the announcement configuration
    /// </summary>
    IAnnouncementConf AnnouncementConfig { get; }

    /// <summary>
    /// Checks for new announcements
    /// </summary>
    /// <returns>List of new announcements</returns>
    Task<IReadOnlyList<IAnnouncement>> CheckNewAnnouncementsAsync();

    /// <summary>
    /// Marks an announcement as read
    /// </summary>
    /// <param name="announcementId">The announcement ID</param>
    void MarkAsRead(string announcementId);

    /// <summary>
    /// Gets all read announcement IDs
    /// </summary>
    /// <returns>List of read announcement IDs</returns>
    IReadOnlyList<string> GetReadAnnouncementIds();

    /// <summary>
    /// Saves the announcement configuration
    /// </summary>
    void SaveAnnouncementConfig();

    /// <summary>
    /// Event raised when new announcements are available
    /// </summary>
    event EventHandler<NewAnnouncementsEventArgs>? NewAnnouncementsAvailable;
}

/// <summary>
/// Announcement interface
/// </summary>
public interface IAnnouncement
{
    /// <summary>
    /// Gets the announcement ID
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the announcement title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets the announcement content
    /// </summary>
    string Content { get; }

    /// <summary>
    /// Gets the publish date
    /// </summary>
    DateTime PublishDate { get; }

    /// <summary>
    /// Gets the version
    /// </summary>
    string Version { get; }
}

/// <summary>
/// New announcements event arguments
/// </summary>
public class NewAnnouncementsEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the announcements
    /// </summary>
    public IReadOnlyList<IAnnouncement> Announcements { get; set; } = Array.Empty<IAnnouncement>();

    /// <summary>
    /// Gets or sets announcements as dictionary (date -> content)
    /// </summary>
    public Dictionary<string, string>? AnnouncementsDict { get; set; }
}

/// <summary>
/// Announcement error event arguments
/// </summary>
public class AnnouncementErrorEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the error message
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stack trace
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;
}
