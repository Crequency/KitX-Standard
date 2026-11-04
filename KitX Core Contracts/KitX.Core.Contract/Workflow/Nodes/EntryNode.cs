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
    }
}