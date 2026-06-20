namespace KitX.Core.Contract.Workflow;

/// <summary>
/// A single outgoing arm of a control-flow statement (Branch / Loop / ToLoopCond / Switch).
/// Replaces the former fixed <c>TrueBlockName</c>/<c>FalseBlockName</c>/<c>ToLoopCondReturnTo</c>
/// triple, generalising the model to any number of arms so that an N-way <c>Switch</c>
/// (and future control-flow builtins) can be expressed without positional hacks.
/// </summary>
public class BranchArm
{
    /// <summary>
    /// The output pin name this arm corresponds to on the Blueprint node
    /// (e.g. <c>"True"</c>, <c>"False"</c>, <c>"LoopBody"</c>, <c>"LoopEnd"</c>,
    /// <c>"Exec"</c>, <c>"Default"</c>, or <c>"0"</c>..<c>"N-1"</c> for Switch).
    /// </summary>
    public string PinName { get; set; } = string.Empty;

    /// <summary>
    /// The target block name this arm transfers control to.
    /// </summary>
    public string TargetBlockName { get; set; } = string.Empty;

    /// <summary>
    /// Whether this arm is a loopback edge (used by <c>ToLoopCond</c>, which returns
    /// to the parent loop's condition block). Drives <c>CFGEdgeType.LoopbackToCondition</c>.
    /// </summary>
    public bool IsLoopback { get; set; }

    /// <summary>
    /// Deep copy of this arm. Centralised so the four converters (BS2CFG / CFG2BS /
    /// BP2CFG / CFGConditionDuplicator) share one clone path instead of four
    /// character-identical <c>Select(a => new BranchArm { ... })</c> blocks.
    /// </summary>
    public BranchArm Clone() => new()
    {
        PinName = PinName,
        TargetBlockName = TargetBlockName,
        IsLoopback = IsLoopback
    };
}
