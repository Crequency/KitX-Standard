using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Shared control-flow arms model embedded by both <c>CFGStatement</c> (CFG layer) and
/// <c>FlowControlStatement</c> (BlockScript layer). Eliminates the ~80 lines of
/// character-identical <c>Arms</c>/<c>TrueBlockName</c>/<c>FalseBlockName</c>/
/// <c>LoopbackTarget</c>/<c>SetArm</c> duplication that had drifted between the two layers
/// (including a hidden null-vs-string.Empty inconsistency in the convenience accessors).
///
/// <para>Both layers expose this via their own delegating properties (<c>Arms</c>,
/// <c>TrueBlockName</c>, <c>FalseBlockName</c>, <c>LoopbackTarget</c>) so existing call
/// sites (<c>stmt.Arms</c>, <c>flow.TrueBlockName</c>, ...) are unchanged.</para>
///
/// <list type="bullet">
/// <item>Branch: [<c>"True"</c>, <c>"False"</c>]</item>
/// <item>Loop: [<c>"LoopBody"</c>, <c>"LoopEnd"</c>]</item>
/// <item>ToLoopCond: [<c>"Exec"</c> with <see cref="BranchArm.IsLoopback"/>=true]</item>
/// <item>Switch: [<c>"Default"</c>, <c>"0"</c>, <c>"1"</c>, ... , <c>"N-1"</c>]</item>
/// </list>
/// </summary>
public class ControlFlowArms
{
    /// <summary>
    /// Outgoing arms of this control-flow statement. Generalised model replacing the former
    /// fixed <c>TrueBlockName</c>/<c>FalseBlockName</c>/<c>ToLoopCondReturnTo</c> triple.
    /// See <see cref="BranchArm"/> for the per-arm layout of each control-flow kind.
    /// </summary>
    public List<BranchArm> Arms { get; set; } = [];

    /// <summary>Convenience accessor: the true-branch / loop-body target (Arms[0]).</summary>
    public string? TrueBlockName
    {
        get => Arms.Count > 0 ? Arms[0].TargetBlockName : null;
        set => SetArm(0, "True", value);
    }

    /// <summary>Convenience accessor: the false-branch / loop-exit target (Arms[1]).</summary>
    public string? FalseBlockName
    {
        get => Arms.Count > 1 ? Arms[1].TargetBlockName : null;
        set => SetArm(1, "False", value);
    }

    /// <summary>
    /// The ToLoopCond loopback target — the loop condition block this statement returns to.
    /// Unified into <see cref="Arms"/>[0] (PinName="Exec", IsLoopback=true) so ToLoopCond is
    /// treated uniformly with Branch/Loop/Switch: all control-flow targets live in Arms.
    /// The former standalone <c>ToLoopCondReturnTo</c> field was a patch over an Arms[0]
    /// collision that no longer exists; Loop statements no longer carry this metadata at all
    /// (it was dead — never read for control flow, only copied and debug-printed).
    /// </summary>
    public string? LoopbackTarget
    {
        get => Arms.Count > 0 ? Arms[0].TargetBlockName : null;
        set => SetArm(0, "Exec", value, isLoopback: true);
    }

    /// <summary>
    /// Sets the arm at <paramref name="index"/>, growing <see cref="Arms"/> with blank arms
    /// as needed. Normalises a null <paramref name="value"/> to <c>string.Empty</c> so the
    /// convenience accessors never surface null into SourceCode concatenation.
    /// </summary>
    public void SetArm(int index, string pinName, string? value, bool isLoopback = false)
    {
        while (Arms.Count <= index)
            Arms.Add(new BranchArm());
        Arms[index].PinName = pinName;
        Arms[index].TargetBlockName = value ?? string.Empty;
        Arms[index].IsLoopback = isLoopback;
    }
}