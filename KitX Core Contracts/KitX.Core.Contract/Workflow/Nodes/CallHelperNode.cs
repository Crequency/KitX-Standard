namespace KitX.Core.Contract.Workflow;

/// <summary>
/// CallHelper node - helper function call
/// </summary>
public class CallHelperNode : BlueprintNode
{
    /// <summary>
    /// Helper function name reference
    /// </summary>
    public string HelperFunctionName { get; set; } = string.Empty;

    public CallHelperNode()
    {
        NodeType = BlueprintNodeType.CallHelper;
        Name = "CallHelper";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 25)],
        OutputPins: [
            new PinDescriptor("Exec", PinType.Execution, 25),
            new PinDescriptor("Return", PinType.Any, 40)
        ],
        DisplayName: "CallHelper"
    );

    public override string GetDisplayTitle() => $"Helper: {HelperFunctionName}";
}