namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Pause node - pause execution
/// </summary>
public class PauseNode : BlueprintNode
{
    public PauseNode()
    {
        NodeType = BlueprintNodeType.Pause;
        Name = "Pause";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 100, Height: 50,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Milliseconds", PinType.Integer, 35)
        ],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 25)],
        DisplayName: "Pause"
    );
}