namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Blueprint node types
/// </summary>
public enum BlueprintNodeType
{
    /// <summary>
    /// Entry point node - triggered by Run button
    /// </summary>
    Entry,

    /// <summary>
    /// Plugin event trigger node - alternative entry point activated by plugin triggers.
    /// Has same pin structure as Entry (0 input, 1 Exec output) but carries PluginName/TriggerName metadata.
    /// </summary>
    PluginTrigger,

    /// <summary>
    /// Constant value node
    /// </summary>
    Const,

    /// <summary>
    /// Plugin function call node
    /// </summary>
    Call,

    /// <summary>
    /// Helper function call node
    /// </summary>
    CallHelper,

    /// <summary>
    /// Variable declaration node (ConstBlock variables without initial values).
    /// A floating node with no ports — users can only change the data type.
    /// </summary>
    Variable,

    /// <summary>
    /// 通用内置函数节点。通过 <c>BuiltinFunctionNode.FunctionName</c> 区分具体函数。
    /// 所有内置函数统一使用此类型，无需为每个函数创建专用 enum 值。
    /// </summary>
    BuiltinFunction,

    /// <summary>
    /// Block function node (v5.0) — a #Block promoted to a first-class collapsible node.
    /// Its body lives in a sub-graph (ChildNodeIds), bounded by EntryPoint/ExitPoint nodes.
    /// </summary>
    Block,

    /// <summary>
    /// Marks a data-input boundary inside a BlockNode's sub-graph.
    /// Each EntryPoint corresponds to one input port on the collapsed BlockNode.
    /// </summary>
    EntryPoint,

    /// <summary>
    /// Marks a data-output boundary inside a BlockNode's sub-graph.
    /// Each ExitPoint corresponds to one output port on the collapsed BlockNode.
    /// </summary>
    ExitPoint
}
