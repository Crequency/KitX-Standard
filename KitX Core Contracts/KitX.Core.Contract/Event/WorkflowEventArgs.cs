using System;
using System.Collections.Generic;

namespace KitX.Core.Contract.Event;

/// <summary>
/// Event arguments for workflow rename events
/// </summary>
public class WorkflowRenamedEventArgs : EventArgs
{
    /// <summary>
    /// The workflow ID that was renamed
    /// </summary>
    public string WorkflowId { get; }

    /// <summary>
    /// The new name of the workflow
    /// </summary>
    public string NewName { get; }

    public WorkflowRenamedEventArgs(string workflowId, string newName)
    {
        WorkflowId = workflowId;
        NewName = newName;
    }
}

/// <summary>
/// Event arguments for workflow data saved events
/// </summary>
public class WorkflowSavedEventArgs : EventArgs
{
    /// <summary>
    /// The workflow ID that was saved
    /// </summary>
    public string WorkflowId { get; }

    /// <summary>
    /// The workflow name at time of save
    /// </summary>
    public string WorkflowName { get; }

    /// <summary>
    /// The workflow description at time of save
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The workflow author at time of save
    /// </summary>
    public string Author { get; }

    public WorkflowSavedEventArgs(string workflowId, string workflowName,
        string description = "", string author = "")
    {
        WorkflowId = workflowId;
        WorkflowName = workflowName;
        Description = description;
        Author = author;
    }
}

/// <summary>
/// Event arguments for workflow execution result events
/// </summary>
public class WorkflowExecutionResultEventArgs : EventArgs
{
    /// <summary>
    /// The workflow ID that was executed
    /// </summary>
    public string WorkflowId { get; }

    /// <summary>
    /// Whether the execution succeeded
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Error message if execution failed
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Lines of Print() output produced during execution (null if not captured).
    /// Surfaced to the Debug activity log so users can see what the workflow printed
    /// without opening the editor's output panel.
    /// </summary>
    public IReadOnlyList<string>? Output { get; }

    public WorkflowExecutionResultEventArgs(string workflowId, bool isSuccess,
        string? errorMessage = null, IReadOnlyList<string>? output = null)
    {
        WorkflowId = workflowId;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Output = output;
    }
}
