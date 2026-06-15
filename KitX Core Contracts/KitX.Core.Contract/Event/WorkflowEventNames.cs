namespace KitX.Core.Contract.Event;

/// <summary>
/// Event-bus channel names shared between the Workflow library and its host
/// (KitX.Core / KitX.Dashboard). Centralized here so the workflow library can
/// publish/subscribe on the <see cref="IEventService"/> bus without referencing
/// KitX.Core's own <c>EventNames</c> table.
/// </summary>
public static class WorkflowEventNames
{
    /// <summary>
    /// Plugin response event (carries a RequestId so callers can correlate
    /// a fire-and-forget plugin invocation with its reply).
    /// </summary>
    public const string PluginResponse = "PluginResponse";

    /// <summary>
    /// Workflow execution result event (success or failure of a workflow run).
    /// </summary>
    public const string WorkflowExecutionResult = "WorkflowExecutionResult";
}
