namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Routes plugin trigger signals to the workflows that subscribed to them.
///
/// A trigger is a pure signal (equivalent to pressing the "Run" button); it carries
/// no business payload. Plugins fire triggers via the TriggerFired command; the
/// <see cref="ITriggerManager"/> matches the firing plugin/trigger against registered
/// <see cref="TriggerConfig"/> subscriptions and runs each matching workflow.
///
/// Subscriptions are purely RUNTIME state: a workflow is armed only while the user
/// keeps it Running (Run = register, Stop = unregister). There is deliberately NO
/// startup re-subscription from persisted TriggerConfig — the Dashboard would
/// otherwise silently arm every saved workflow at launch, desyncing the card's
/// mounted indicator. A user-configurable "auto-start workflows at KitX launch"
/// mechanism is planned as part of the Toolkit system (see Toolkit功能需求文档.md).
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
}
