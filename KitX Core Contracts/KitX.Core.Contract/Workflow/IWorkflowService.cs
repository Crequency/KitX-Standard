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
    /// Gets the workflow list
    /// </summary>
    IReadOnlyList<IWorkflowCase> GetWorkflows();

    /// <summary>
    /// Adds a workflow
    /// </summary>
    void AddWorkflow(IWorkflowCase workflow);

    /// <summary>
    /// Removes a workflow
    /// </summary>
    void RemoveWorkflow(string workflowId);

    /// <summary>
    /// Runs a workflow
    /// </summary>
    Task<bool> RunWorkflowAsync(string workflowId);

    /// <summary>
    /// Stops a workflow
    /// </summary>
    Task<bool> StopWorkflowAsync(string workflowId);
}

/// <summary>
/// Script execution interface
/// </summary>
public interface IScriptExecutionService
{
    /// <summary>
    /// Executes a workflow script
    /// </summary>
    Task<object?> ExecuteScriptAsync(string script, Dictionary<string, object>? parameters = null);

    /// <summary>
    /// Executes workflow script codes with plugin dependencies
    /// </summary>
    Task<string?> ExecuteCodesAsync(
        string code,
        List<PluginInfo>? requiredPlugins = null,
        bool includeTimestamp = true,
        System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes KCS codes
    /// </summary>
    Task<string?> ExecuteKcsCodesAsync(
        string mainCode,
        List<HelperFunction> helperFunctions,
        List<VariableConstant> constants,
        List<PluginInfo>? requiredPlugins = null,
        bool includeTimestamp = true,
        System.Threading.CancellationToken cancellationToken = default);
}

/// <summary>
/// Plugin service interface for workflow constant and helper function handling
/// </summary>
public interface IWorkflowPluginService
{
    /// <summary>
    /// Initializes the plugin manager
    /// </summary>
    void InitializePluginManager();

    /// <summary>
    /// Updates the available plugins list
    /// </summary>
    void UpdateAvailablePlugins(List<PluginInfo> plugins);

    /// <summary>
    /// Parses constants from code
    /// </summary>
    List<VariableConstant> ParseConstantsFromCode(string code);

    /// <summary>
    /// Applies constants to code
    /// </summary>
    string ApplyConstantsToCode(string code, List<VariableConstant> constants);

    /// <summary>
    /// Merges helper functions into code
    /// </summary>
    string MergeHelperFunctions(string mainCode, List<HelperFunction> helperFunctions);
}

/// <summary>
/// Block script service interface
/// </summary>
public interface IBlockScriptService
{
    /// <summary>
    /// Parses a block script
    /// </summary>
    BlockScriptParseResult ParseBlockScript(string sourceCode);

    /// <summary>
    /// Parses a block script asynchronously
    /// </summary>
    Task<BlockScriptParseResult> ParseBlockScriptAsync(string sourceCode);

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
    /// Executes a block script
    /// </summary>
    Task<BlockScriptExecutionResult> ExecuteBlockScriptAsync(
        BlockScript script,
        Dictionary<string, object?>? parameters = null,
        System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a block script from source code
    /// </summary>
    Task<BlockScriptExecutionResult> ExecuteBlockScriptAsync(
        string sourceCode,
        Dictionary<string, object?>? parameters = null,
        System.Threading.CancellationToken cancellationToken = default);

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
}

/// <summary>
/// Workflow service interface - composite interface for backward compatibility
/// </summary>
public interface IWorkflowService : IWorkflowManagementService, IScriptExecutionService,
    IWorkflowPluginService, IBlockScriptService
{
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
    /// Gets the workflow name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the workflow description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the icon path
    /// </summary>
    string IconPath { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the workflow is running
    /// </summary>
    bool IsRunning { get; set; }

    /// <summary>
    /// Gets or sets the script file path
    /// </summary>
    string? ScriptPath { get; set; }
}
