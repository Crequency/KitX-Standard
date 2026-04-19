namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Get variable value node - reads a PubVar
/// </summary>
public class GetNode : BlueprintNode
{
    /// <summary>
    /// Variable name to read
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    public GetNode()
    {
        NodeType = BlueprintNodeType.Get;
        Name = "Get";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 60,
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        OutputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Value", PinType.Any, 40)
        ],
        DisplayName: "Get"
    );

    public override string GetDisplayTitle() => $"Get: {VarName}";
}