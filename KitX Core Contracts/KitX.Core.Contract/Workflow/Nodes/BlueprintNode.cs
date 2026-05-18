using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KitX.Core.Contract.Workflow;

// Forward declaration - Node types are in their own files
/// <summary>
/// Base class for all blueprint nodes.
/// Uses polymorphic JSON serialization so that concrete node types
/// round-trip correctly through System.Text.Json.
/// </summary>
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(EntryNode), "Entry")]
[JsonDerivedType(typeof(ConstNode), "Const")]
[JsonDerivedType(typeof(CallNode), "Call")]
[JsonDerivedType(typeof(CallHelperNode), "CallHelper")]
[JsonDerivedType(typeof(VariableNode), "Variable")]
[JsonDerivedType(typeof(BuiltinFunctionNode), "BuiltinFunction")]
[JsonDerivedType(typeof(PluginTriggerNode), "PluginTrigger")]
public abstract partial class BlueprintNode
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Node type
    /// </summary>
    public BlueprintNodeType NodeType { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// X position on canvas
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Y position on canvas
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Node width
    /// </summary>
    public double Width { get; set; } = 200;

    /// <summary>
    /// Node height
    /// </summary>
    public double Height { get; set; } = 100;

    /// <summary>
    /// Whether node is selected
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Input pins
    /// </summary>
    public List<BlueprintPin> InputPins { get; set; } = [];

    /// <summary>
    /// Output pins
    /// </summary>
    public List<BlueprintPin> OutputPins { get; set; } = [];

    /// <summary>
    /// Parent blueprint reference (set when node is added to blueprint).
    /// Ignored during JSON serialization to prevent circular reference.
    /// </summary>
    [JsonIgnore]
    public Blueprint? Blueprint { get; set; }

    /// <summary>
    /// Get pin by ID
    /// </summary>
    public BlueprintPin? GetPinById(string pinId)
    {
        foreach (var pin in InputPins)
            if (pin.Id == pinId) return pin;
        foreach (var pin in OutputPins)
            if (pin.Id == pinId) return pin;
        return null;
    }

    /// <summary>
    /// Get all pins
    /// </summary>
    public IEnumerable<BlueprintPin> GetAllPins()
    {
        foreach (var pin in InputPins)
            yield return pin;
        foreach (var pin in OutputPins)
            yield return pin;
    }

    /// <summary>
    /// Returns the layout descriptor for this node type.
    /// Each node subclass must define its own pin layout, width, and height.
    /// Used by the node registry and UI rendering to avoid external switch statements.
    /// </summary>
    public abstract NodeDescriptor GetDescriptor();

    /// <summary>
    /// Returns the display title for UI rendering (e.g., "Call: Plugin.Func").
    /// Default implementation returns Name; subclasses override for richer display.
    /// </summary>
    public virtual string GetDisplayTitle() => Name;

    /// <summary>
    /// Initializes InputPins and OutputPins from GetDescriptor().
    /// Subclasses should call this in their constructor instead of manually adding pins.
    /// This ensures the descriptor is the single source of truth for pin layout.
    /// </summary>
    protected void InitializePinsFromDescriptor()
    {
        var desc = GetDescriptor();
        foreach (var pd in desc.InputPins)
            InputPins.Add(new BlueprintPin { Name = pd.Name, Direction = PinDirection.Input, Type = pd.Type });
        foreach (var pd in desc.OutputPins)
            OutputPins.Add(new BlueprintPin { Name = pd.Name, Direction = PinDirection.Output, Type = pd.Type });
    }
}