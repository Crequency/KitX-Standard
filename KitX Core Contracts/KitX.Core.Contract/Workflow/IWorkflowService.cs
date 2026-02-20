using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitX.Shared.CSharp.Plugin;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Workflow service interface
/// </summary>
public interface IWorkflowService
{
    /// <summary>
    /// Gets the workflow list
    /// </summary>
    IReadOnlyList<IWorkflowCase> GetWorkflows();

    /// <summary>
    /// Adds a workflow
    /// </summary>
    /// <param name="workflow">The workflow to add</param>
    void AddWorkflow(IWorkflowCase workflow);

    /// <summary>
    /// Removes a workflow
    /// </summary>
    /// <param name="workflowId">The workflow ID</param>
    void RemoveWorkflow(string workflowId);

    /// <summary>
    /// Runs a workflow
    /// </summary>
    /// <param name="workflowId">The workflow ID</param>
    /// <returns>True if run was successful</returns>
    Task<bool> RunWorkflowAsync(string workflowId);

    /// <summary>
    /// Stops a workflow
    /// </summary>
    /// <param name="workflowId">The workflow ID</param>
    /// <returns>True if stop was successful</returns>
    Task<bool> StopWorkflowAsync(string workflowId);

    /// <summary>
    /// Executes a workflow script
    /// </summary>
    /// <param name="script">The script content</param>
    /// <param name="parameters">Optional parameters</param>
    /// <returns>The execution result</returns>
    Task<object?> ExecuteScriptAsync(string script, Dictionary<string, object>? parameters = null);

    /// <summary>
    /// Executes workflow script codes with plugin dependencies
    /// </summary>
    /// <param name="code">The code to execute</param>
    /// <param name="requiredPlugins">Required plugins for this script</param>
    /// <param name="includeTimestamp">Whether to include timestamp in result</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Execution result as string</returns>
    Task<string?> ExecuteCodesAsync(
        string code,
        List<PluginInfo>? requiredPlugins = null,
        bool includeTimestamp = true,
        System.Threading.CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes the plugin manager
    /// </summary>
    void InitializePluginManager();

    /// <summary>
    /// Updates the available plugins list
    /// </summary>
    /// <param name="plugins">Plugin list</param>
    void UpdateAvailablePlugins(List<PluginInfo> plugins);
}

/// <summary>
/// Plugin service provider interface for workflow integration
/// </summary>
public interface IPluginServiceProvider
{
    /// <summary>
    /// Gets running plugins
    /// </summary>
    IEnumerable<PluginInfo> GetRunningPlugins();

    /// <summary>
    /// Finds a plugin by name
    /// </summary>
    /// <param name="pluginName">The plugin name</param>
    /// <returns>The plugin info or null if not found</returns>
    PluginInfo? FindPlugin(string pluginName);

    /// <summary>
    /// Finds a connector for a plugin
    /// </summary>
    /// <param name="pluginInfo">The plugin info</param>
    /// <returns>The connector or null if not found</returns>
    object? FindConnector(PluginInfo pluginInfo);

    /// <summary>
    /// Sends a request asynchronously
    /// </summary>
    /// <param name="connector">The connector</param>
    /// <param name="request">The request</param>
    Task SendRequestAsync(object connector, object request);

    /// <summary>
    /// Subscribes to plugin responses
    /// </summary>
    /// <param name="responseHandler">The response handler</param>
    void SubscribeToResponses(Action<string, string> responseHandler);
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
