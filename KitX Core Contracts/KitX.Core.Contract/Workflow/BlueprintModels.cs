using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Simple 2D point structure for view positioning
/// </summary>
public struct ViewPoint
{
    public double X { get; set; }
    public double Y { get; set; }

    public ViewPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator (double X, double Y)(ViewPoint p) => (p.X, p.Y);
    public static implicit operator ViewPoint((double X, double Y) p) => new(p.X, p.Y);
}

/// <summary>
/// Connection between two pins
/// </summary>
public class BlueprintConnection
{
    /// <summary>
/// Unique identifier
/// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Source node ID
    /// </summary>
    public string SourceNodeId { get; set; } = string.Empty;

    /// <summary>
    /// Source pin ID
    /// </summary>
    public string SourcePinId { get; set; } = string.Empty;

    /// <summary>
    /// Target node ID
    /// </summary>
    public string TargetNodeId { get; set; } = string.Empty;

    /// <summary>
    /// Target pin ID
    /// </summary>
    public string TargetPinId { get; set; } = string.Empty;

    /// <summary>
    /// Corresponding PubVar name for data flow connections (optional)
    /// </summary>
    public string? PubVarName { get; set; }
}

/// <summary>
/// Records a named block scope within a Blueprint.
/// Captures which nodes belong to a logical block,
/// preserving BlockScript block boundaries for reverse conversion.
/// </summary>
public class BlueprintBlockScope
{
    /// <summary>
    /// Block name (stable across round-trips, e.g. "MainBlock", "LoopBody", "SuccessLogic").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ordered list of node IDs that belong to this block.
    /// </summary>
    public List<string> NodeIds { get; set; } = [];

    /// <summary>
    /// Name of the next block to execute when this block ends naturally
    /// (i.e., not ended by Branch/Loop/ToLoopCond). Null if the block ends
    /// with a control-flow statement or is terminal.
    /// </summary>
    public string? NextBlockName { get; set; }

    /// <summary>
    /// For sub-blocks: the node ID of the Branch/Loop node that created this scope.
    /// Null for the main block scope.
    /// </summary>
    public string? OwnerNodeId { get; set; }

    /// <summary>
    /// For sub-blocks: which output arm of the owner node leads into this scope.
    /// E.g., "True", "False" for Branch; "LoopBody", "LoopEnd" for Loop.
    /// Null for the main block scope.
    /// </summary>
    public string? OwnerArmName { get; set; }

    /// <summary>
    /// Whether this block scope represents the main entry block.
    /// </summary>
    public bool IsMainBlock { get; set; }

    /// <summary>
    /// v5.1: block-local variable declarations (##BlockVars).
    /// Each entry is the complete declaration line, e.g. "int processedCount = 0".
    /// </summary>
    public List<BlockVarEntry> BlockVars { get; set; } = [];

    /// <summary>
    /// v5.1: true when the block uses the explicit ##BlockBody marker.
    /// </summary>
    public bool HasExplicitBlockBody { get; set; }
}

/// <summary>
/// v5.1: a single block-local variable declaration carried through BP round-trip.
/// </summary>
public record BlockVarEntry(string Name, string? Type, string? DefaultValue);

/// <summary>
/// A statement-level (data-connection subgraph) comment. Backs the KS
/// <c>LeadingComment</c> through the BP round-trip: one KS statement maps to one
/// data-connection subgraph, and this comment annotates that whole subgraph.
/// <see cref="AnchorNodeId"/> is the statement's primary node (the node the exec
/// chain enters) so the reverse translator can reattach it. <see cref="NodeIds"/>
/// lists the subgraph's nodes for frontend box-rendering (optional).
/// </summary>
public class BlueprintGroupComment
{
    /// <summary>The comment text (may contain multiple lines joined by <c>\n</c>).</summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// The statement's primary node id (the node the exec chain enters — Branch/Each/
    /// While/Switch/control node, or the last function node of a pipeline). The reverse
    /// translator matches leading comments by this id.
    /// </summary>
    public string AnchorNodeId { get; set; } = string.Empty;

    /// <summary>
    /// All node ids belonging to this statement's data-connection subgraph (for frontend
    /// box/highlight rendering). Optional; may be empty.
    /// </summary>
    public List<string> NodeIds { get; set; } = [];
}

/// <summary>
/// Blueprint document container
/// </summary>
public class Blueprint
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Document name
    /// </summary>
    public string Name { get; set; } = "Untitled";

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Last modified timestamp
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// All nodes in this blueprint
    /// </summary>
    public List<BlueprintNode> Nodes { get; set; } = [];

    /// <summary>
    /// All connections in this blueprint
    /// </summary>
    public List<BlueprintConnection> Connections { get; set; } = [];

    /// <summary>
    /// View zoom level (0.1 to 5.0)
    /// </summary>
    public double ZoomLevel { get; set; } = 1.0;

    /// <summary>
    /// View pan offset
    /// </summary>
    public ViewPoint PanOffset { get; set; } = new(0, 0);

    /// <summary>
    /// Helper functions available in this blueprint
    /// </summary>
    public List<HelperFunction> HelperFunctions { get; set; } = [];

    /// <summary>
    /// PubVar variable names (invisible in Blueprint, used for data flow)
    /// </summary>
    public List<string> PubVarNames { get; set; } = [];

    /// <summary>
    /// Constant values (from ConstBlock)
    /// </summary>
    public List<VariableConstant> ConstValues { get; set; } = [];

    /// <summary>
    /// Named block scopes recording which nodes belong to which logical block.
    /// Empty for legacy blueprints that predate this field.
    /// </summary>
    public List<BlueprintBlockScope> BlockScopes { get; set; } = [];

    /// <summary>
    /// Statement-level (data-connection subgraph) comments. Each entry attaches a
    /// leading comment to the set of nodes forming one KS statement's data subgraph
    /// (one KS statement == one data-connection subgraph). <see cref="BlueprintGroupComment.AnchorNodeId"/>
    /// is the statement's primary node (the node the exec chain enters), used by the
    /// reverse translator to reattach the comment as a leading comment.
    /// Empty for blueprints without preserved leading comments.
    /// </summary>
    public List<BlueprintGroupComment> GroupComments { get; set; } = [];

    /// <summary>
    /// Get node by ID
    /// </summary>
    public BlueprintNode? GetNodeById(string nodeId)
    {
        foreach (var node in Nodes)
            if (node.Id == nodeId) return node;
        return null;
    }

    /// <summary>
    /// Get connections from a node
    /// </summary>
    public IEnumerable<BlueprintConnection> GetConnectionsFrom(string nodeId)
    {
        foreach (var conn in Connections)
            if (conn.SourceNodeId == nodeId) yield return conn;
    }

    /// <summary>
    /// Get connections to a node
    /// </summary>
    public IEnumerable<BlueprintConnection> GetConnectionsTo(string nodeId)
    {
        foreach (var conn in Connections)
            if (conn.TargetNodeId == nodeId) yield return conn;
    }

    /// <summary>
    /// Add a node to this blueprint, automatically setting the back-reference
    /// </summary>
    /// <param name="node">Node to add</param>
    public void AddNode(BlueprintNode node)
    {
        node.Blueprint = this;
        Nodes.Add(node);
    }

    /// <summary>
    /// Add a connection to this blueprint (deduplicates by source/target/pin)
    /// </summary>
    /// <param name="connection">Connection to add</param>
    public void AddConnection(BlueprintConnection connection)
    {
        var alreadyExists = Connections.Any(c =>
            c.SourceNodeId == connection.SourceNodeId && c.SourcePinId == connection.SourcePinId &&
            c.TargetNodeId == connection.TargetNodeId && c.TargetPinId == connection.TargetPinId);
        if (!alreadyExists)
            Connections.Add(connection);
    }
}
