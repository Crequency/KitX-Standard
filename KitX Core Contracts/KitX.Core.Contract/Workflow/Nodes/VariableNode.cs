namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Variable node - ConstBlock variable without initial value.
/// A floating node with no input/output ports. Users can change the data type
/// but not the initial value. Type changes propagate to Get/Set nodes.
/// </summary>
public class VariableNode : BlueprintNode
{
    /// <summary>
    /// Variable name
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    /// <summary>
    /// Variable type (e.g., "int", "double", "string", "bool")
    /// </summary>
    public string VarType { get; set; } = "int";

    public VariableNode()
    {
        NodeType = BlueprintNodeType.Variable;
        Name = "Variable";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 50,
        InputPins: [],
        OutputPins: [],
        DisplayName: "Variable"
    );

    public override string GetDisplayTitle() => $"Var: {VarName}";
}