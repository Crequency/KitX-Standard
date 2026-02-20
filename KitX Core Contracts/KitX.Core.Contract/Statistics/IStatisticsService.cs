using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Statistics;

/// <summary>
/// Statistics service interface
/// </summary>
public interface IStatisticsService
{
    /// <summary>
    /// Starts statistics collection
    /// </summary>
    void Start();

    /// <summary>
    /// Stops statistics collection
    /// </summary>
    void Stop();

    /// <summary>
    /// Gets usage statistics
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Usage statistics</returns>
    IUsageStatistics GetUsageStatistics(DateTime startDate, DateTime endDate);
}

/// <summary>
/// Usage statistics interface
/// </summary>
public interface IUsageStatistics
{
    /// <summary>
    /// Gets the total usage in seconds
    /// </summary>
    double TotalUsageSeconds { get; }

    /// <summary>
    /// Gets the daily usage dictionary
    /// </summary>
    Dictionary<DateTime, double> DailyUsage { get; }
}
