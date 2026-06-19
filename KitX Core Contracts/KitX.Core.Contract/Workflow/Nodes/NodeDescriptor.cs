using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Describes a node type's layout and pin configuration.
/// Each node subclass provides its own descriptor via GetDescriptor().
/// </summary>
/// <param name="InputVariadic">Optional variadic-growth spec for input pins. Null = fixed.</param>
/// <param name="OutputVariadic">Optional variadic-growth spec for output pins. Null = fixed.</param>
public record NodeDescriptor(
    double Width,
    double Height,
    IReadOnlyList<PinDescriptor> InputPins,
    IReadOnlyList<PinDescriptor> OutputPins,
    string DisplayName,
    VariadicPinSpec? InputVariadic = null,
    VariadicPinSpec? OutputVariadic = null
);
