using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Block function node (v5.0) — a <c>#Block</c> promoted to a first-class collapsible node.
/// <para>
/// In the blueprint's outer layer a BlockNode appears as a single node with Exec + data ports.
/// Its body lives in a sub-graph: the node IDs in <see cref="ChildNodeIds"/> are the statements
/// inside the block, bounded by <see cref="EntryPointNode"/>/<see cref="ExitPointNode"/> markers.
/// </para>
/// <para>
/// The CFG stays single-layer flat (BlockNode is a BP-side presentation abstraction):
/// BS→CFG→BP collects same-block statement nodes into a BlockNode; BP→CFG→BS expands the
/// BlockNode's sub-graph back to flat CFG blocks. See BlockScriptGrammarRule §11.
/// </para>
/// </summary>
public class BlockNode : BlueprintNode
{
    /// <summary>
    /// Name of the block this node represents (e.g. "MainBlock", "LoopBody", "ProcessItem").
    /// Stable across round-trips — matches the <c>#Block Name</c> marker.
    /// </summary>
    public string BlockName { get; set; } = string.Empty;

    /// <summary>
    /// Ordered node IDs belonging to this block's sub-graph (the block body).
    /// These nodes live in the same Blueprint.Nodes list; BlockNode groups them.
    /// </summary>
    public List<string> ChildNodeIds { get; set; } = [];

    /// <summary>
    /// Whether this block is the script entry (MainBlock).
    /// </summary>
    public bool IsMainBlock { get; set; }

    /// <summary>
    /// Name of the next block to execute when this block ends via sequential fall-through
    /// (Goto target). Null when the block ends with Branch/ForLoop/Switch/Break.
    /// </summary>
    public string? NextBlockName { get; set; }

    public BlockNode()
    {
        NodeType = BlueprintNodeType.Block;
        Name = "Block";
        InitializePinsFromDescriptor();
    }

    /// <summary>
    /// Default descriptor: a single Exec input + Exec output. Sub-graph EntryPoint/ExitPoint
    /// nodes and control-flow arms may add data ports at conversion time; the descriptor here
    /// is the minimal baseline. Pin layout is mutable on the instance (via InputPins/OutputPins).
    /// </summary>
    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 30)],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 30)],
        DisplayName: "Block"
    );

    public override string GetDisplayTitle() => $"Block: {BlockName}";
}
