namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Marks a data-output boundary inside a <see cref="BlockNode"/>'s sub-graph.
/// <para>
/// Each ExitPointNode corresponds to one output port on the collapsed BlockNode.
/// Data produced inside the sub-graph flows into the ExitPoint, which then exposes it
/// to the outer layer as the BlockNode's output.
/// </para>
/// <para>
/// See BlockScriptGrammarRule §11.2 for the boundary-marker model.
/// </para>
/// </summary>
public class ExitPointNode : BlueprintNode
{
    /// <summary>
    /// The port name this exit point exposes on the collapsed BlockNode (e.g. "Result", "Value").
    /// </summary>
    public string PortName { get; set; } = "Value";

    public ExitPointNode()
    {
        NodeType = BlueprintNodeType.ExitPoint;
        Name = "ExitPoint";
        InitializePinsFromDescriptor();
    }

    /// <summary>
    /// One input pin receiving the value to export; no output pins (data leaves the sub-graph).
    /// </summary>
    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [new PinDescriptor("Value", PinType.Any, 30)],
        OutputPins: [],
        DisplayName: "ExitPoint"
    );

    public override string GetDisplayTitle() => $"Out: {PortName}";
}
