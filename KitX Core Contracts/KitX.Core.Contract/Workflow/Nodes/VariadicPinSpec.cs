namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Declares a variadic (auto-growing) pin group on a node descriptor.
/// <para>
/// When a node declares <c>InputVariadic</c> or <c>OutputVariadic</c>, the Blueprint editor
/// appends a fresh pin of <see cref="PinType"/> whenever the <em>last</em> pin of that group
/// gets connected — so the user can chain more inputs/outputs without manually adding pins.
/// </para>
/// <para>
/// <see cref="BasePinName"/> is the title prefix for new pins; <see cref="StartIndex"/> is the
/// starting number appended to it. The editor derives each new pin's name from the node's
/// current pin count (not by mutating this record), so every node instance counts independently
/// and survives save/load round-trips.
/// </para>
/// <para>Examples:</para>
/// <list type="bullet">
/// <item>StringConcat input: <c>new("Input ", 3, PinType.String)</c> → "Input 3", "Input 4", ...</item>
/// <item>Switch output: <c>new("", 1, PinType.Execution)</c> → "1", "2", ...</item>
/// </list>
/// </summary>
public record VariadicPinSpec(string BasePinName, int StartIndex, PinType PinType)
{
    /// <summary>
    /// Optional pin-name prefixes for paired/multi-type variadic pin groups.
    /// <para>
    /// When set together with <see cref="PinTypes"/>, the variadic group grows by appending
    /// one pin per prefix (cycling in order) per growth iteration, instead of a single pin.
    /// This supports nodes needing alternating pin types, e.g. DictNew's
    /// Key(String)/Value(Any) pairs: <c>PinNamePrefixes=["Key","Value"], PinTypes=[String,Any]</c>
    /// grows "Key0","Value0","Key1","Value1",... (index from <see cref="StartIndex"/> appended
    /// to every prefix). Null = legacy single-<see cref="PinType"/> behaviour (backward compatible).
    /// </para>
    /// </summary>
    public string[]? PinNamePrefixes { get; init; }

    /// <summary>
    /// Optional pin types paired 1:1 with <see cref="PinNamePrefixes"/> (same length).
    /// Null = legacy single-<see cref="PinType"/> behaviour.
    /// </summary>
    public PinType[]? PinTypes { get; init; }
}
