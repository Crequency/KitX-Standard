using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitX.Shared.CSharp.Plugin;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Workflow management interface
/// </summary>
public interface IWorkflowManagementService
{
    /// <summary>
    /// Runs a workflow
    /// </summary>
    Task<bool> RunWorkflowAsync(string workflowId);

    /// <summary>
    /// Runs a workflow and returns the full execution result including Print() output.
    /// Use this when the caller needs the execution output (e.g. the Debug activity log).
    /// </summary>
    Task<WorkflowRunResult> RunWorkflowWithDetailsAsync(string workflowId);

    /// <summary>
    /// Stops a workflow
    /// </summary>
    Task<bool> StopWorkflowAsync(string workflowId);

    /// <summary>
    /// Compiles a workflow's BlockScript into a persisted assembly on disk.
    /// </summary>
    /// <param name="workflowId">The workflow ID to compile and persist.</param>
    /// <returns>True if compilation and persistence succeeded.</returns>
    Task<bool> CompileAndPersistWorkflowAsync(string workflowId);
}

/// <summary>
/// Plugin service interface for workflow constant and helper function handling
/// </summary>
public interface IWorkflowPluginService
{
    /// <summary>
    /// Parses constants from code
    /// </summary>
    List<VariableConstant> ParseConstantsFromCode(string code);
}

/// <summary>
/// Block script service interface — the public surface consumed by Dashboard.
///
/// Only methods that take primitive/source parameters (string, List&lt;HelperFunction&gt;, ...)
/// or return Dashboard-visible result types are kept here. Pipeline-internal methods that
/// deal in the parsed <c>BlockScript</c> / <c>BlockScriptParseResult</c> models live on the
/// workflow library's internal <c>IBlockScriptPipelineService</c> instead, so that those
/// internal models need not be exposed through Contract.
/// </summary>
public interface IBlockScriptService
{
    /// <summary>
    /// Validates a block script
    /// </summary>
    BlockScriptValidationResult ValidateBlockScript(string sourceCode);

    /// <summary>
    /// Parses constants from a BlockScript source's #ConstBlock section.
    /// Only returns variables that have initial values (DefaultValue != null).
    /// </summary>
    List<VariableConstant> ParseConstantsFromBlockScript(string sourceCode);

    /// <summary>
    /// Executes a block script from source code with helper functions
    /// </summary>
    Task<BlockScriptExecutionResult> ExecuteBlockScriptAsync(
        string sourceCode,
        List<HelperFunction> helperFunctions,
        System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a block script from source code with helper functions and constant overrides.
    /// Constant overrides replace the DefaultValue on ConstBlock variables before execution.
    /// </summary>
    Task<BlockScriptExecutionResult> ExecuteBlockScriptAsync(
        string sourceCode,
        List<HelperFunction> helperFunctions,
        Dictionary<string, object?>? constantOverrides,
        System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Preloads all persisted compiled scripts for a workflow from disk.
    /// </summary>
    /// <param name="workflowId">Workflow ID to preload scripts for.</param>
    /// <returns>Number of scripts loaded from disk.</returns>
    int PreloadCompiledScripts(string workflowId);
}

/// <summary>
/// Workflow case interface
/// </summary>
public interface IWorkflowCase
{
    /// <summary>
    /// Gets the workflow ID
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets or sets the workflow name
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the workflow description
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// Gets or sets the author name
    /// </summary>
    string Author { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the workflow is running
    /// </summary>
    bool IsRunning { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the workflow is in an error state
    /// </summary>
    bool IsError { get; set; }

    /// <summary>
    /// Gets or sets the error message if the workflow is in an error state
    /// </summary>
    string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the script file path
    /// </summary>
    string? ScriptPath { get; set; }

    /// <summary>
    /// Gets the creation time
    /// </summary>
    DateTime CreatedTime { get; }

    /// <summary>
    /// Gets or sets the last modified time
    /// </summary>
    DateTime LastModifiedTime { get; set; }

    /// <summary>
    /// Gets or sets the trigger configuration
    /// </summary>
    TriggerConfig? TriggerConfig { get; set; }
}

/// <summary>
/// Result of a workflow run, including the Print() output lines captured during execution.
/// </summary>
public record WorkflowRunResult(
    bool IsSuccess,
    string? ErrorMessage,
    IReadOnlyList<string>? Output);
