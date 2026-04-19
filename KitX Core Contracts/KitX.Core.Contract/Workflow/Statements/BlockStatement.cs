namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Base class for statements within a block
/// </summary>
public abstract class BlockStatement
{
    /// <summary>
    /// Line number in source
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Original source code for this statement
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;
}