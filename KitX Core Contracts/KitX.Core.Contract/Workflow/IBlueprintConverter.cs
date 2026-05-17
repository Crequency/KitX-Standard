using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Interface for converting BlockScript to Blueprint
/// </summary>
public interface IBlockScriptToBlueprintConverter
{
    /// <summary>
    /// Convert BlockScript source code to Blueprint
    /// </summary>
    /// <param name="sourceCode">BlockScript source code</param>
    /// <param name="helperFunctions">Helper functions available</param>
    /// <returns>Converted Blueprint</returns>
    Blueprint Convert(string sourceCode, List<HelperFunction>? helperFunctions = null);

    /// <summary>
    /// Convert parsed BlockScript to Blueprint
    /// </summary>
    /// <param name="script">Parsed BlockScript</param>
    /// <returns>Converted Blueprint</returns>
    Blueprint Convert(BlockScript script);
}

/// <summary>
/// Interface for converting Blueprint to BlockScript
/// </summary>
public interface IBlueprintToBlockScriptConverter
{
    /// <summary>
    /// Convert Blueprint to BlockScript source code
    /// </summary>
    /// <param name="blueprint">Blueprint to convert</param>
    /// <returns>BlockScript source code</returns>
    string Convert(Blueprint blueprint);

    /// <summary>
    /// Convert Blueprint to parsed BlockScript
    /// </summary>
    /// <param name="blueprint">Blueprint to convert</param>
    /// <returns>Parsed BlockScript</returns>
    BlockScript ConvertToBlockScript(Blueprint blueprint);
}

/// <summary>
/// Interface for Blueprint service
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
