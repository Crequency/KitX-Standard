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
    PubVarBlock
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
    /// Statements in this block
    /// </summary>
    public List<BlockStatement> Statements { get; set; } = [];

    /// <summary>
    /// Line number in source where this block starts
    /// </summary>
    public int LineNumber { get; set; }
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
    /// Loop back to a block while condition is true
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
    /// Continue to next iteration
    /// </summary>
    Continue
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
    /// Target block name when condition is true (for Branch)
    /// </summary>
    public string TrueBlockName { get; set; } = string.Empty;

    /// <summary>
    /// Target block name when condition is false (for Branch)
    /// </summary>
    public string FalseBlockName { get; set; } = string.Empty;

    /// <summary>
    /// Loop body block name (for Loop)
    /// </summary>
    public string? LoopBlockName { get; set; }
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
    /// Raw source code
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;
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
/// Flow control result returned by built-in functions
/// </summary>
public class FlowResult
{
    /// <summary>
    /// Type of flow result
    /// </summary>
    public FlowResultType Type { get; set; }

    /// <summary>
    /// Target block name for jumps
    /// </summary>
    public string? TargetBlock { get; set; }

    /// <summary>
    /// Optional value (for return)
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Whether execution should continue
    /// </summary>
    public bool ShouldContinue { get; set; } = true;

    /// <summary>
    /// Continue to next statement
    /// </summary>
    public static FlowResult Continue() => new() { Type = FlowResultType.Continue, ShouldContinue = true };

    /// <summary>
    /// Return with a value
    /// </summary>
    public static FlowResult Return(object? value = null) => new() { Type = FlowResultType.Return, Value = value, ShouldContinue = false };

    /// <summary>
    /// Break from loop
    /// </summary>
    public static FlowResult Break() => new() { Type = FlowResultType.Break, ShouldContinue = false };
}

/// <summary>
/// Flow result types
/// </summary>
public enum FlowResultType
{
    /// <summary>
    /// Continue to next statement
    /// </summary>
    Continue,

    /// <summary>
    /// Return from script
    /// </summary>
    Return,

    /// <summary>
    /// Break from loop
    /// </summary>
    Break,

    /// <summary>
    /// Continue to next loop iteration
    /// </summary>
    ContinueLoop
}
