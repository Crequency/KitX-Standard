namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Plugin trigger entry node - activated when a specific plugin fires a specific trigger signal.
/// Has same pin structure as Entry (0 input, 1 Exec output) but carries PluginName/TriggerName metadata.
/// </summary>
public class PluginTriggerNode : BlueprintNode
{
    /// <summary>The plugin name to listen for</summary>
    public string PluginName { get; set; } = string.Empty;

    /// <summary>The trigger name to listen for</summary>
    public string TriggerName { get; set; } = string.Empty;

    public PluginTriggerNode()
    {
        NodeType = BlueprintNodeType.PluginTrigger;
        Name = "PluginTrigger";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 160, Height: 60,
        InputPins: [],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 30)],
        DisplayName: "PluginTrigger"
    );

    public override string GetDisplayTitle()
        => string.IsNullOrEmpty(PluginName)
            ? $"Trigger: {TriggerName}"
            : $"Trigger: {PluginName}.{TriggerName}";
}