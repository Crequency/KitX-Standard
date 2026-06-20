namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Const node - constant value
/// </summary>
public class ConstNode : BlueprintNode
{
    /// <summary>
    /// Constant name
    /// </summary>
    public string ConstName { get; set; } = string.Empty;

    /// <summary>
    /// Constant type
    /// </summary>
    public string ConstType { get; set; } = "string";

    /// <summary>
    /// Constant value
    /// </summary>
    public string? ConstValue { get; set; }

    public ConstNode()
    {
        NodeType = BlueprintNodeType.Const;
        Name = "Const";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [],
        OutputPins: [new PinDescriptor("Value", PinType.Any, 25)],
        DisplayName: "Const"
    );

    public override string GetDisplayTitle() => $"Const: {ConstName}";
}