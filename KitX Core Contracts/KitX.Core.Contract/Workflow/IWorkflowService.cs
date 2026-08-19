using System;
using KitX.Shared.CSharp.Plugin;

namespace KitX.Core.Contract.Workflow;

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
