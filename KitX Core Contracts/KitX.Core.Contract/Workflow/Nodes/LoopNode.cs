namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Loop node - iterative execution
/// </summary>
public class LoopNode : BlueprintNode
{
    public LoopNode()
    {
        NodeType = BlueprintNodeType.Loop;
        Name = "Loop";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 80,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 30),
            new PinDescriptor("Condition", PinType.Boolean, 50)
        ],
        OutputPins: [
            new PinDescriptor("LoopBody", PinType.Execution, 30),
            new PinDescriptor("LoopEnd", PinType.Execution, 50)
        ],
        DisplayName: "Loop"
    );
}