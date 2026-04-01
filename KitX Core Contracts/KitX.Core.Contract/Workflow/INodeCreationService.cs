namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Interface for node creation service
/// </summary>
public interface INodeCreationService
{
    /// <summary>
    /// Creates a constant node
    /// </summary>
    ConstNode CreateConstNode(string name, string type, string? defaultValue);

    /// <summary>
    /// Creates an entry node
    /// </summary>
    EntryNode CreateEntryNode(double x, double y);

    /// <summary>
    /// Creates a get variable node
    /// </summary>
    GetNode CreateGetNode(string varName);

    /// <summary>
    /// Creates a set variable node
    /// </summary>
    SetNode CreateSetNode(string varName);

    /// <summary>
    /// Creates a print node
    /// </summary>
    PrintNode CreatePrintNode();

    /// <summary>
    /// Creates a pause node
    /// </summary>
    PauseNode CreatePauseNode();

    /// <summary>
    /// Creates a call function node
    /// </summary>
    CallNode CreateCallNode(string functionName);

    /// <summary>
    /// Creates a call helper function node
    /// </summary>
    CallHelperNode CreateCallHelperNode(string helperFunctionName);

    /// <summary>
    /// Creates a branch node
    /// </summary>
    BranchNode CreateBranchNode();

    /// <summary>
    /// Creates a loop node
    /// </summary>
    LoopNode CreateLoopNode();

    /// <summary>
    /// Creates a break node
    /// </summary>
    BreakNode CreateBreakNode();
}
