using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Call node - plugin function call
/// </summary>
public class CallNode : BlueprintNode
{
    /// <summary>
    /// Plugin name
    /// </summary>
    public string PluginName { get; set; } = string.Empty;

    /// <summary>
    /// Function name
    /// </summary>
    public string FunctionName { get; set; } = string.Empty;

    /// <summary>
    /// Target device name for cross-device calls.
    /// If null or empty, call is routed locally via PluginCall.
    /// </summary>
    public string? TargetDevice { get; set; }

    /// <summary>
    /// Extra arguments beyond plugin name, method name, and target device.
    /// Used by PluginCallWithTarget and similar functions with variable arguments.
    /// Stored as raw argument expressions (e.g., "cityId", "__pubVar1").
    /// </summary>
    public List<string> ExtraArguments { get; set; } = new();

    public CallNode()
    {
        NodeType = BlueprintNodeType.Call;
        Name = "Call";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        OutputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Return", PinType.Any, 40)
        ],
        DisplayName: "Call"
    );

    public override string GetDisplayTitle()
    {
        var baseTitle = string.IsNullOrEmpty(PluginName) ? $"Call: {FunctionName}" : $"Call: {PluginName}.{FunctionName}";
        return string.IsNullOrEmpty(TargetDevice) ? baseTitle : $"{baseTitle} @ {TargetDevice}";
    }
}