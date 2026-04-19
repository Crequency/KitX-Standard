namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Branch node - conditional execution
/// </summary>
public class BranchNode : BlueprintNode
{
    public BranchNode()
    {
        NodeType = BlueprintNodeType.Branch;
        Name = "Branch";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 80,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 30),
            new PinDescriptor("Condition", PinType.Boolean, 50)
        ],
        OutputPins: [
            new PinDescriptor("True", PinType.Execution, 30),
            new PinDescriptor("False", PinType.Execution, 50)
        ],
        DisplayName: "Branch"
    );
}