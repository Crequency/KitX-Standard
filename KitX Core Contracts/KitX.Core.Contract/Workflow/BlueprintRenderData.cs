using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Pre-classified rendering data for three-phase blueprint rendering:
/// Phase 1: AllNodes, Phase 2: ExecConnections, Phase 3: DataConnections
/// </summary>
public class BlueprintRenderData
{
    /// <summary>
    /// All nodes in the blueprint
    /// </summary>
    public List<BlueprintNode> AllNodes { get; set; } = [];

    /// <summary>
    /// Execution flow connections (source pin is Execution type)
    /// </summary>
    public List<BlueprintConnection> ExecConnections { get; set; } = [];

    /// <summary>
    /// Data flow connections (source pin is non-Execution type)
    /// </summary>
    public List<BlueprintConnection> DataConnections { get; set; } = [];
}

/// <summary>
/// Service that classifies blueprint connections into Exec and Data groups
/// for phased rendering in the frontend.
/// </summary>
public interface IBlueprintRenderDataService
{
    /// <summary>
    /// Splits a Blueprint's connections into Exec and Data categories
    /// </summary>
    BlueprintRenderData GetRenderData(Blueprint blueprint);
}
