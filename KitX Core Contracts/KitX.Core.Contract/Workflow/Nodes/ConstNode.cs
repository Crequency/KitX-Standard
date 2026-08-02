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
    /// User value — the override entered on the BP node (initialised empty; maps to the
    /// KS editor's Variable Constants panel UserValue). Empty means "use the default".
    /// </summary>
    public string? ConstValue { get; set; }

    /// <summary>
    /// Default value — the initial value from the KS script declaration
    /// (<c>const { int x = 5 }</c> → "5"). Read-only on the BP side; the node displays
    /// <see cref="ConstValue"/> when set, otherwise falls back to this default.
    /// </summary>
    public string? DefaultValue { get; set; }

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