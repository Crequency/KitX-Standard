using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

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