namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Print node - output information
/// </summary>
public class PrintNode : BlueprintNode
{
    public PrintNode()
    {
        NodeType = BlueprintNodeType.Print;
        Name = "Print";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 100, Height: 50,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Value", PinType.Any, 35)
        ],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 25)],
        DisplayName: "Print"
    );
}