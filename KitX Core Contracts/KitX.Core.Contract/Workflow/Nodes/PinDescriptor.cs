namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Describes a pin's layout within a node template.
/// Used for self-describing node pin configurations.
/// </summary>
public record PinDescriptor(
    string Name,
    PinType Type,
    double RelativeY
);