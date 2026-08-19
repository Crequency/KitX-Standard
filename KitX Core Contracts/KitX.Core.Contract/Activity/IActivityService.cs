using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Activity;

/// <summary>
/// Activity management service interface
/// </summary>
public interface IActivityService
{
    /// <summary>
    /// Records app start event
    /// </summary>
    void RecordAppStart();

    /// <summary>
    /// Records app exit event
    /// </summary>
    void RecordAppExit();

    /// <summary>
    /// Records an activity
    /// </summary>
    /// <param name="type">The activity type</param>
    /// <param name="details">Optional details</param>
    void RecordActivity(string type, Dictionary<string, object>? details = null);

    /// <summary>
    /// Gets activities
    /// </summary>
    /// <param name="startDate">Optional start date</param>
    /// <param name="endDate">Optional end date</param>
    /// <param name="limit">Maximum number of activities to return</param>
    /// <returns>List of activities</returns>
    IList<IActivity> GetActivities(DateTime? startDate = null, DateTime? endDate = null, int limit = 100);

    /// <summary>
    /// Gets activity statistics
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Activity statistics</returns>
    IActivityStatistics GetStatistics(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Reads activities from the current-month collection, newest-first (by descending row
    /// Id). Pass <paramref name="limit"/> &lt;= 0 to return every row; otherwise a
    /// reverse-chronological page of <paramref name="limit"/> rows starting at
    /// <paramref name="skip"/>.
    /// </summary>
    /// <param name="limit">Maximum rows to return; &lt;= 0 means all.</param>
    /// <param name="skip">Rows to skip (used for paging after the first page).</param>
    /// <returns>List of rich activity rows, newest-first.</returns>
    IList<Common.Activity.Activity> ReadActivities(int limit = 0, int skip = 0);

    /// <summary>
    /// Total number of recorded activities in the current-month collection.
    /// </summary>
    long CountActivities();

    /// <summary>
    /// Event raised when activities are updated
    /// </summary>
    event EventHandler? ActivitiesUpdated;
}

/// <summary>
/// Activity interface
/// </summary>
public interface IActivity
{
    /// <summary>
    /// Gets the activity ID
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the activity type
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Gets the timestamp
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// Gets the details
    /// </summary>
    Dictionary<string, object> Details { get; }
}

/// <summary>
/// Activity statistics interface
/// </summary>
public interface IActivityStatistics
{
    /// <summary>
    /// Gets the total number of activities
    /// </summary>
    int TotalActivities { get; }

    /// <summary>
    /// Gets the activities grouped by type
    /// </summary>
    Dictionary<string, int> ActivitiesByType { get; }
}
