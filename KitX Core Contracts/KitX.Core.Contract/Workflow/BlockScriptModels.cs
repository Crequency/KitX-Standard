using System;
using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Block type enumeration
/// </summary>
public enum BlockType
{
    /// <summary>
    /// Constants block - variables are globally scoped and read-only
    /// </summary>
    ConstBlock,

    /// <summary>
    /// Main block - entry point, local scope
    /// </summary>
    MainBlock,

    /// <summary>
    /// Named block - local scope, can be called by name
    /// </summary>
    NamedBlock,

    /// <summary>
    /// Public variable block - globally scoped and writable, but not exposed in UI editor
    /// </summary>
    PubVarBlock,

    /// <summary>
    /// Loop block - auto-generated block containing a Loop statement
    /// </summary>
    LoopBlock
}

/// <summary>
/// Represents a single block definition in the script
/// </summary>
public class BlockDefinition
{
    /// <summary>
    /// Block type
    /// </summary>
    public BlockType Type { get; set; }

    /// <summary>
    /// Block name (for NamedBlock)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Variable declarations in this block
    /// </summary>
    public List<VariableDeclaration> Variables { get; set; } = [];

    /// <summary>
    /// Statements in this block (excluding Loop statements, which are separated)
    /// </summary>
    public List<BlockStatement> Statements { get; set; } = [];

    /// <summary>
    /// Line number in source where this block starts
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Name of the next block to execute when this block ends naturally
    /// (i.e., not ended by Branch/Loop/LoopBodyEnd)
    /// </summary>
    public string? NextBlockName { get; set; }

    /// <summary>
    /// For LoopBlock: the block name containing this loop (i.e., the parent block)
    /// </summary>
    public string? ParentBlockName { get; set; }
}

/// <summary>
/// Variable declaration
/// </summary>
public class VariableDeclaration
{
    /// <summary>
    /// Variable name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Variable type as string
    /// </summary>
    public string Type { get; set; } = "object";

    /// <summary>
    /// Initial value expression as string (for evaluation at parse time or execution time)
    /// </summary>
    public string? InitialValueExpression { get; set; }

    /// <summary>
    /// Default value (pre-evaluated for const block)
    /// </summary>
    public object? DefaultValue { get; set; }
}

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

/// <summary>
/// Expression statement
/// </summary>
public class ExpressionStatement : BlockStatement
{
    /// <summary>
    /// The expression to execute
    /// </summary>
    public string Expression { get; set; } = string.Empty;
}

/// <summary>
/// Variable declaration statement
/// </summary>
public class VariableDeclarationStatement : BlockStatement
{
    /// <summary>
    /// The variable declaration
    /// </summary>
    public VariableDeclaration Declaration { get; set; } = new();
}

/// <summary>
/// Flow control statement types
/// </summary>
public enum FlowControlType
{
    /// <summary>
    /// Branch to another block based on condition
    /// </summary>
    Branch,

    /// <summary>
    /// Loop while condition is true (Loop has three args: condition, trueBlock, falseBlock)
    /// </summary>
    Loop,

    /// <summary>
    /// Return from script execution
    /// </summary>
    Return,

    /// <summary>
    /// Break from current loop
    /// </summary>
    Break,

    /// <summary>
    /// Loop body end - marks the end of a loop body and returns to loop condition
    /// </summary>
    LoopBodyEnd
}

/// <summary>
/// Flow control statement
/// </summary>
public class FlowControlStatement : BlockStatement
{
    /// <summary>
    /// Type of flow control
    /// </summary>
    public FlowControlType ControlType { get; set; }

    /// <summary>
    /// Condition expression (for Branch/Loop)
    /// </summary>
    public string ConditionExpression { get; set; } = string.Empty;

    /// <summary>
    /// Target block name when condition is true (for Branch/Loop)
    /// </summary>
    public string TrueBlockName { get; set; } = string.Empty;

    /// <summary>
    /// Target block name when condition is false (for Branch/Loop)
    /// For Loop: this is the loop exit block
    /// </summary>
    public string FalseBlockName { get; set; } = string.Empty;

    /// <summary>
    /// For LoopBodyEnd: the block name containing the Loop statement to return to
    /// </summary>
    public string? LoopBodyEndReturnTo { get; set; }
}

/// <summary>
/// Parsed block script container
/// </summary>
public class BlockScript
{
    /// <summary>
    /// Global constants block (ConstBlock)
    /// </summary>
    public BlockDefinition? ConstBlock { get; set; }

    /// <summary>
    /// Public variables block (PubVarBlock) - optional
    /// </summary>
    public BlockDefinition? PubVarBlock { get; set; }

    /// <summary>
    /// Main entry block (MainBlock)
    /// </summary>
    public BlockDefinition? MainBlock { get; set; }

    /// <summary>
    /// Named blocks dictionary by name
    /// </summary>
    public Dictionary<string, BlockDefinition> NamedBlocks { get; set; } = [];

    /// <summary>
    /// All blocks in order of appearance
    /// </summary>
    public List<BlockDefinition> AllBlocks { get; set; } = [];

    /// <summary>
    /// Loop blocks dictionary by parent block name
    /// (e.g., "MainBlock" -> LoopBlock for MainBlock's Loop statement)
    /// </summary>
    public Dictionary<string, BlockDefinition> LoopBlocks { get; set; } = [];

    /// <summary>
    /// Raw source code (parsed input, may not include helper functions)
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// Full source code including merged helper functions (for execution)
    /// </summary>
    public string FullSourceCode { get; set; } = string.Empty;

    /// <summary>
    /// Helper functions to be made available in script execution context
    /// </summary>
    public List<HelperFunction> HelperFunctions { get; set; } = [];


    /// <summary>
    /// Gets a block by name (checks NamedBlocks, MainBlock, ConstBlock, PubVarBlock, then LoopBlocks)
    /// Note: LoopBlocks is checked last because its keys are parent block names (e.g., "MainBlock")
    /// which would otherwise shadow the actual MainBlock when querying by name.
    /// </summary>
    public BlockDefinition? GetBlockByName(string name)
    {
        // First check NamedBlocks (user-defined blocks)
        if (NamedBlocks.TryGetValue(name, out var namedBlock))
            return namedBlock;
        // Then check the standard blocks by name match
        if (MainBlock?.Name == name)
            return MainBlock;
        if (ConstBlock?.Name == name)
            return ConstBlock;
        if (PubVarBlock?.Name == name)
            return PubVarBlock;
        // Finally check LoopBlocks - these are internal and should not shadow standard blocks
        // LoopBlocks keys are parent block names (e.g., "MainBlock" -> LoopBlock for that parent)
        if (LoopBlocks.TryGetValue(name, out var loopBlock))
            return loopBlock;
        return null;
    }
}

/// <summary>
/// Result of parsing operation
/// </summary>
public class BlockScriptParseResult
{
    /// <summary>
    /// Whether parsing was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if parsing failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Line number where error occurred
    /// </summary>
    public int ErrorLine { get; set; }

    /// <summary>
    /// The parsed script if successful
    /// </summary>
    public BlockScript? Script { get; set; }
}

/// <summary>
/// Validation result for block scripts
/// </summary>
public class BlockScriptValidationResult
{
    /// <summary>
    /// Whether the script is valid
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// List of errors found
    /// </summary>
    public List<string> Errors { get; set; } = [];

    /// <summary>
    /// List of warnings
    /// </summary>
    public List<string> Warnings { get; set; } = [];

    /// <summary>
    /// Adds an error
    /// </summary>
    public void AddError(string error) => Errors.Add(error);

    /// <summary>
    /// Adds a warning
    /// </summary>
    public void AddWarning(string warning) => Warnings.Add(warning);
}

/// <summary>
/// Execution result for block scripts
/// </summary>
public class BlockScriptExecutionResult
{
    /// <summary>
    /// Whether execution was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Return value from script (if any)
    /// </summary>
    public object? ReturnValue { get; set; }

    /// <summary>
    /// Error message if execution failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Output from Print() calls
    /// </summary>
    public List<string> Output { get; set; } = [];

    /// <summary>
    /// Number of blocks executed
    /// </summary>
    public int ExecutedBlockCount { get; set; }

    /// <summary>
    /// Execution time in milliseconds
    /// </summary>
    public long ExecutionTimeMs { get; set; }
}

/// <summary>
/// Result of executing a block - used by state machine for flow control
/// </summary>
public class BlockExecutionResult
{
    /// <summary>
    /// Whether execution should continue to next block
    /// </summary>
    public bool ShouldContinue { get; set; } = true;

    /// <summary>
    /// Name of the next block to execute (null means end of script)
    /// </summary>
    public string? NextBlockName { get; set; }

    /// <summary>
    /// Whether this is a return (end of entire script)
    /// </summary>
    public bool IsReturn { get; set; }

    /// <summary>
    /// Return value if IsReturn is true
    /// </summary>
    public object? ReturnValue { get; set; }

    /// <summary>
    /// Create a result for continuing to next block
    /// </summary>
    public static BlockExecutionResult ContinueTo(string? nextBlockName) => new()
    {
        ShouldContinue = true,
        NextBlockName = nextBlockName,
        IsReturn = false
    };

    /// <summary>
    /// Create a result for end of script
    /// </summary>
    public static BlockExecutionResult Return(object? value = null) => new()
    {
        ShouldContinue = false,
        NextBlockName = null,
        IsReturn = true,
        ReturnValue = value
    };
}
