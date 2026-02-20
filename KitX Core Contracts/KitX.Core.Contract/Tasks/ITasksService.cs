using System;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Tasks;

/// <summary>
/// Tasks service interface for background task management
/// </summary>
public interface ITasksService
{
    /// <summary>
    /// Runs a synchronous task
    /// </summary>
    /// <param name="task">The task to run</param>
    /// <param name="taskName">Optional task name</param>
    void RunTask(Action task, string? taskName = null);

    /// <summary>
    /// Runs an asynchronous task
    /// </summary>
    /// <param name="task">The task to run</param>
    /// <param name="taskName">Optional task name</param>
    /// <returns>Task representing the async operation</returns>
    Task RunTaskAsync(Func<Task> task, string? taskName = null);
}
