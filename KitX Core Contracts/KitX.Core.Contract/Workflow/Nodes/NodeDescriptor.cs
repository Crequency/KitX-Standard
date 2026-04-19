using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Describes a node type's layout and pin configuration.
/// Each node subclass provides its own descriptor via GetDescriptor().
/// </summary>
public record NodeDescriptor(
    double Width,
    double Height,
    IReadOnlyList<PinDescriptor> InputPins,
    IReadOnlyList<PinDescriptor> OutputPins,
    string DisplayName
);