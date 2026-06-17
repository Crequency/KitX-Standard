namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Routes plugin trigger signals to the workflows that subscribed to them.
///
/// A trigger is a pure signal (equivalent to pressing the "Run" button); it carries
/// no business payload. Plugins fire triggers via the TriggerFired command; the
/// <see cref="ITriggerManager"/> matches the firing plugin/trigger against registered
/// <see cref="TriggerConfig"/> subscriptions and runs each matching workflow.
/// </summary>
public interface ITriggerManager
{
    /// <summary>
    /// Registers a workflow's trigger configuration so that matching
    /// TriggerFired events from plugins will run the workflow.
    /// Only <c>PluginEvent</c> triggers with a non-empty plugin name take effect.
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    /// <param name="config">The trigger configuration.</param>
    void RegisterWorkflowTrigger(string workflowId, TriggerConfig config);

    /// <summary>
    /// Removes the trigger subscription for a workflow (e.g. on delete/rename).
    /// </summary>
    /// <param name="workflowId">The workflow identifier.</param>
    void UnregisterWorkflowTrigger(string workflowId);

    /// <summary>
    /// Scans persisted workflow files and re-subscribes all configured triggers.
    /// Called once after DI initialization completes, before plugins connect, so
    /// that TriggerFired events arriving early can still be routed.
    /// </summary>
    void InitializeFromPersistedWorkflows();
}
