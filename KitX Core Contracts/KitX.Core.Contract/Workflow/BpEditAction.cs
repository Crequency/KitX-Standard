// v5.2 BP editing action hierarchy (command pattern).
// Pure data records — no dependency on CFG or Workflow types.
// Dashboard creates these to describe a user edit; IBpEditApplier translates them to CFG mutations.

namespace KitX.Core.Contract.Workflow;

public abstract record BpEditAction;

// ── Node-level ──

/// <param name="Position">Insertion index within the block (null = append).</param>
public record AddNodeInBlock(string BlockName, string BpNodeKind, int? Position = null) : BpEditAction;

/// <param name="NodeId">The BP node id to delete.</param>
public record DeleteNode(string NodeId) : BpEditAction;

/// <param name="TargetBlock">Destination block.</param>
/// <param name="Position">Insertion index within the target block (null = append).</param>
public record MoveNodeToBlock(string NodeId, string TargetBlock, int? Position = null) : BpEditAction;

/// <param name="ArgIndex">Zero-based argument index.</param>
public record SetNodeArgument(string NodeId, int ArgIndex, string Value) : BpEditAction;

// ── Connection-level (data flow) ──

public record ConnectData(string SourceNodeId, string SourcePin, string TargetNodeId, string TargetPin, string? PubVarName = null) : BpEditAction;

public record Disconnect(string ConnectionId) : BpEditAction;

// ── Control flow (Exec arms) ──

public record SetControlFlowArm(string NodeId, string ArmPinName, string TargetBlockName) : BpEditAction;

// ── Block-level ──

public record AddBlock(string BlockName) : BpEditAction;

public record RenameBlock(string OldName, string NewName) : BpEditAction;

public record DeleteBlock(string BlockName) : BpEditAction;

// ── Position (G5) ──

public record MoveNodePosition(string NodeId, double X, double Y) : BpEditAction;
