namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Entry node - execution entry point
/// </summary>
public class EntryNode : BlueprintNode
{
    public EntryNode()
    {
        NodeType = BlueprintNodeType.Entry;
        Name = "Entry";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 30)],
        DisplayName: "Entry"
    );
}