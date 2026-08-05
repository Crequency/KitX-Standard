using System;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Pin on a blueprint node
/// </summary>
public class BlueprintPin
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Pin name (e.g., "Exec", "Condition", "True", "Value")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Pin direction
    /// </summary>
    public PinDirection Direction { get; set; }

    /// <summary>
    /// Pin data type
    /// </summary>
    public PinType Type { get; set; } = PinType.Any;

    /// <summary>
    /// Default value for input pins
    /// </summary>
    public string? DefaultValue { get; set; }
}