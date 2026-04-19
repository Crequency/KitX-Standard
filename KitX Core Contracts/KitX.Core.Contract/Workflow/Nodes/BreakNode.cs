namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Break node - exit loop
/// </summary>
public class BreakNode : BlueprintNode
{
    public BreakNode()
    {
        NodeType = BlueprintNodeType.Break;
        Name = "Break";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 100, Height: 40,
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        OutputPins: [],
        DisplayName: "Break"
    );
}