namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Set variable value node - writes to a PubVar
/// </summary>
public class SetNode : BlueprintNode
{
    /// <summary>
    /// Variable name to write
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    public SetNode()
    {
        NodeType = BlueprintNodeType.Set;
        Name = "Set";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 60,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Value", PinType.Any, 40)
        ],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        DisplayName: "Set"
    );

    public override string GetDisplayTitle() => $"Set: {VarName}";
}