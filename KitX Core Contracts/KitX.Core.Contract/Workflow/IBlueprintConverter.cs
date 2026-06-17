using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Interface for Blueprint service.
///
/// Kept in the public Contract surface (Dashboard consumes it). The lower-level
/// converters (<c>IBlockScriptToBlueprintConverter</c>,
/// <c>IBlueprintToBlockScriptConverter</c>) are internal to the workflow pipeline
/// and live in the KitX.Workflow library.
/// </summary>
public interface IBlueprintService
{
    /// <summary>
    /// Create a new empty blueprint
    /// </summary>
    /// <returns>New blueprint</returns>
    Blueprint CreateBlueprint();

    /// <summary>
    /// Import blueprint from BlockScript
    /// </summary>
    /// <param name="sourceCode">BlockScript source code</param>
    /// <param name="helperFunctions">Helper functions</param>
    /// <returns>Imported blueprint, or null if conversion failed</returns>
    Blueprint? ImportFromBlockScript(string sourceCode, List<HelperFunction>? helperFunctions = null);

    /// <summary>
    /// Export blueprint to BlockScript
    /// </summary>
    /// <param name="blueprint">Blueprint to export</param>
    /// <returns>BlockScript source code</returns>
    string ExportToBlockScript(Blueprint blueprint);

    /// <summary>
    /// Execute blueprint by converting to BlockScript and running
    /// </summary>
    /// <param name="blueprint">Blueprint to execute</param>
    /// <returns>Execution result</returns>
    Task<BlockScriptExecutionResult> ExecuteBlueprintAsync(Blueprint blueprint);

    /// <summary>
    /// Build a mapping from CFG statement IDs to Blueprint node IDs for debug highlighting.
    /// Key = statementId (also equals BlueprintNode.Id), Value = BlueprintNode.Id
    /// </summary>
    /// <param name="blueprint">Blueprint to analyze</param>
    /// <returns>StatementId → NodeId mapping, or empty if conversion failed</returns>
    Dictionary<string, string> GetDebugNodeMapping(Blueprint blueprint);
}
