using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Describes a node type's pin configuration.
/// Each node subclass provides its own descriptor via GetDescriptor().
/// Width/Height are intentionally not part of the descriptor: node instances carry
/// their own layout dimensions (BlueprintNode.Width/Height defaults), and no consumer
/// ever reads descriptor-level dimensions.
/// </summary>
/// <param name="InputVariadic">Optional variadic-growth spec for input pins. Null = fixed.</param>
/// <param name="OutputVariadic">Optional variadic-growth spec for output pins. Null = fixed.</param>
public record NodeDescriptor(
    IReadOnlyList<PinDescriptor> InputPins,
    IReadOnlyList<PinDescriptor> OutputPins,
    string DisplayName,
    VariadicPinSpec? InputVariadic = null,
    VariadicPinSpec? OutputVariadic = null
);
