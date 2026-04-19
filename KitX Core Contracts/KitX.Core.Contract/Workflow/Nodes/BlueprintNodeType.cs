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
    /// Conditional branch node
    /// </summary>
    Branch,

    /// <summary>
    /// Loop node
    /// </summary>
    Loop,

    /// <summary>
    /// Break from loop node
    /// </summary>
    Break,

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
    /// Get variable value node - reads a PubVar
    /// </summary>
    Get,

    /// <summary>
    /// Set variable value node - writes to a PubVar
    /// </summary>
    Set,

    /// <summary>
    /// Print output node
    /// </summary>
    Print,

    /// <summary>
    /// Pause execution node
    /// </summary>
    Pause,

    /// <summary>
    /// Variable declaration node (ConstBlock variables without initial values).
    /// A floating node with no ports — users can only change the data type.
    /// </summary>
    Variable,

    /// <summary>
    /// 通用内置函数节点。通过 <c>BuiltinFunctionNode.FunctionName</c> 区分具体函数。
    /// 新的内置函数统一使用此类型，无需为每个函数创建专用 enum 值。
    /// </summary>
    BuiltinFunction
}