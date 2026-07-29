namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Variable node (v5.0) — unified read/write node for all variable tiers.
/// <para>
/// Replaces the v4.0 Get/Set builtin function nodes: a read is a data edge leaving the
/// <c>Value</c> output pin; a write is a data edge entering the <c>Value</c> input pin.
/// The tap semantics (<c>0 &gt; x &gt; Print</c>) is expressed by both edges existing on
/// the same node. See BlockScriptGrammarRule §4.4, §6.3.
/// </para>
/// <para>
/// <see cref="VarKind"/> distinguishes the storage tier (Const / PubVar / BlockVar / LoopIndex),
/// which governs mutability, scope and reset behaviour (§3.4).
/// </para>
/// </summary>
public class VariableNode : BlueprintNode
{
    /// <summary>
    /// Variable name.
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    /// <summary>
    /// Variable type (e.g., "int", "double", "string", "bool").
    /// </summary>
    public string VarType { get; set; } = "int";

    /// <summary>
    /// Storage tier this variable belongs to (v5.0). Governs mutability/scope/reset.
    /// </summary>
    public VariableKind VarKind { get; set; } = VariableKind.PubVar;

    /// <summary>
    /// Optional initial-value payload. For dict-typed vars this carries the JSON-serialised
    /// <c>KsDictLiteral</c> so the BP→IR reverse path can rebuild the structured initialiser
    /// (Package/Dict-Type-Design.md §3.3). For scalar-typed vars it may carry the verbatim
    /// initialiser expression text. Null when the var has no initial value.
    /// </summary>
    public string? VarInitialValue { get; set; }

    public VariableNode()
    {
        NodeType = BlueprintNodeType.Variable;
        Name = "Variable";
        InitializePinsFromDescriptor();
    }

    /// <summary>
    /// One <c>Value</c> input pin (write/Set) and one <c>Value</c> output pin (read/Get).
    /// Both are <c>Any</c>-typed at the node level; the connected data carries the actual type.
    /// </summary>
    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [new PinDescriptor("Value", PinType.Any, 25)],
        OutputPins: [new PinDescriptor("Value", PinType.Any, 75)],
        DisplayName: "Variable"
    );

    public override string GetDisplayTitle() => $"{VarKind}: {VarName}";
}
