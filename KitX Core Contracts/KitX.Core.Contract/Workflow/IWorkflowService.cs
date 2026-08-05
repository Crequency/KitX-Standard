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
