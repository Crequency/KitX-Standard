namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Marks a data-input boundary inside a <see cref="BlockNode"/>'s sub-graph.
/// <para>
/// Each EntryPointNode corresponds to one input port on the collapsed BlockNode.
/// Data flowing into the BlockNode from the outer layer lands on the EntryPoint,
/// which then feeds it to the consuming nodes inside the sub-graph.
/// </para>
/// <para>
/// Used for explicit boundary marking (preferred over implicit "first consumer = port"
/// pass-through) so that refactoring a block's interior does not affect its outer interface.
/// See BlockScriptGrammarRule §11.2.
/// </para>
/// </summary>
public class EntryPointNode : BlueprintNode
{
    /// <summary>
    /// The port name this entry point exposes on the collapsed BlockNode (e.g. "Value", "Source").
    /// </summary>
    public string PortName { get; set; } = "Value";

    public EntryPointNode()
    {
        NodeType = BlueprintNodeType.EntryPoint;
        Name = "EntryPoint";
        InitializePinsFromDescriptor();
    }

    /// <summary>
    /// No input pins (data originates from outside the block); one output pin carrying
    /// the inbound value into the sub-graph.
    /// </summary>
    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [],
        OutputPins: [new PinDescriptor("Value", PinType.Any, 30)],
        DisplayName: "EntryPoint"
    );

    public override string GetDisplayTitle() => $"In: {PortName}";
}
