using System;

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

    public WorkflowSavedEventArgs(string workflowId, string workflowName)
    {
        WorkflowId = workflowId;
        WorkflowName = workflowName;
    }
}
