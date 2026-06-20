using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Workflow storage service interface - manages workflow file persistence
/// </summary>
public interface IWorkflowStorageService
{
    /// <summary>
    /// Gets the storage directory path (e.g., ./Data/Workflows/)
    /// </summary>
    string StorageDirectory { get; }

    /// <summary>
    /// Creates a new workflow with a default BlockScript template
    /// </summary>
    /// <param name="name">Workflow name</param>
    /// <param name="description">Optional description</param>
    /// <returns>The created workflow case</returns>
    Task<IWorkflowCase> CreateWorkflowAsync(string name, string? description = null);

    /// <summary>
    /// Loads workflow data from a .kcs file
    /// </summary>
    /// <param name="workflowId">Workflow ID</param>
    /// <returns>The loaded KCS file data, or null if not found</returns>
    Task<KcsFileFormat?> LoadWorkflowDataAsync(string workflowId);

    /// <summary>
    /// Saves workflow data to a .kcs file
    /// </summary>
    /// <param name="workflowId">Workflow ID</param>
    /// <param name="data">The KCS file data to save</param>
    Task SaveWorkflowDataAsync(string workflowId, KcsFileFormat data);

    /// <summary>
    /// Deletes a workflow and its .kcs file
    /// </summary>
    /// <param name="workflowId">Workflow ID</param>
    Task DeleteWorkflowAsync(string workflowId);

    /// <summary>
    /// Discovers all stored workflows by scanning the storage directory
    /// </summary>
    /// <returns>List of discovered workflow cases</returns>
    Task<IReadOnlyList<IWorkflowCase>> DiscoverWorkflowsAsync();

    /// <summary>
    /// Gets the file path for a workflow
    /// </summary>
    /// <param name="workflowId">Workflow ID</param>
    /// <returns>Full file path</returns>
    string GetWorkflowFilePath(string workflowId);
}
