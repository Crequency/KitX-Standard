namespace KitX.Core.Contract.Workflow;

/// <summary>
/// The storage tier a <see cref="VariableNode"/> belongs to (v5.0 three-tier variable model).
/// See BlockScriptGrammarRule §3.4.
/// </summary>
public enum VariableKind
{
    /// <summary>
    /// ConstBlock variable — read-only, must have initial value (§3.1).
    /// </summary>
    Const,

    /// <summary>
    /// PubVarBlock variable — global mutable, cross-block read/write (§3.2).
    /// </summary>
    PubVar,

    /// <summary>
    /// ##BlockVars variable — block-local mutable, lifetime = one block activation (§3.3).
    /// </summary>
    BlockVar,

    /// <summary>
    /// ForLoop-injected index — read-only, loop-injected scope variable (§7.1, §3.4).
    /// Not declared in any block; auto-injected by the ForLoop node.
    /// </summary>
    LoopIndex
}
