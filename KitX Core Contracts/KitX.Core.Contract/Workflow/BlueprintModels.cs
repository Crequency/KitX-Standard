using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Simple 2D point structure for view positioning
/// </summary>
public struct ViewPoint
{
    public double X { get; set; }
    public double Y { get; set; }

    public ViewPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator (double X, double Y)(ViewPoint p) => (p.X, p.Y);
    public static implicit operator ViewPoint((double X, double Y) p) => new(p.X, p.Y);
}

/// <summary>
/// Blueprint node types
/// </summary>
public enum BlueprintNodeType
{
    /// <summary>
    /// Entry point node - triggered by Run button
    /// </summary>
    Entry,

    /// <summary>
    /// Plugin event trigger node - alternative entry point activated by plugin triggers.
    /// Has same pin structure as Entry (0 input, 1 Exec output) but carries PluginName/TriggerName metadata.
    /// </summary>
    PluginTrigger,

    /// <summary>
    /// Conditional branch node
    /// </summary>
    Branch,

    /// <summary>
    /// Loop node
    /// </summary>
    Loop,

    /// <summary>
    /// Break from loop node
    /// </summary>
    Break,

    /// <summary>
    /// Constant value node
    /// </summary>
    Const,

    /// <summary>
    /// Plugin function call node
    /// </summary>
    Call,

    /// <summary>
    /// Helper function call node
    /// </summary>
    CallHelper,

    /// <summary>
    /// Get variable value node - reads a PubVar
    /// </summary>
    Get,

    /// <summary>
    /// Set variable value node - writes to a PubVar
    /// </summary>
    Set,

    /// <summary>
    /// Print output node
    /// </summary>
    Print,

    /// <summary>
    /// Pause execution node
    /// </summary>
    Pause,

    /// <summary>
    /// Variable declaration node (ConstBlock variables without initial values).
    /// A floating node with no ports — users can only change the data type.
    /// </summary>
    Variable,

    /// <summary>
    /// 通用内置函数节点。通过 <c>BuiltinFunctionNode.FunctionName</c> 区分具体函数。
    /// 新的内置函数统一使用此类型，无需为每个函数创建专用 enum 值。
    /// </summary>
    BuiltinFunction
}

/// <summary>
/// Pin direction
/// </summary>
public enum PinDirection
{
    Input,
    Output
}

/// <summary>
/// Pin type for data flow
/// </summary>
public enum PinType
{
    /// <summary>
    /// Execution flow pin - green
    /// </summary>
    Execution,

    /// <summary>
    /// Boolean pin - cyan
    /// </summary>
    Boolean,

    /// <summary>
    /// Integer pin - orange
    /// </summary>
    Integer,

    /// <summary>
    /// Double pin - purple
    /// </summary>
    Double,

    /// <summary>
    /// String pin - yellow
    /// </summary>
    String,

    /// <summary>
    /// Any type pin - white
    /// </summary>
    Any
}

/// <summary>
/// Describes a pin's layout within a node template.
/// Used for self-describing node pin configurations.
/// </summary>
public record PinDescriptor(
    string Name,
    PinType Type,
    double RelativeY
);

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

/// <summary>
/// Base class for all blueprint nodes.
/// Uses polymorphic JSON serialization so that concrete node types
/// round-trip correctly through System.Text.Json.
/// </summary>
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(EntryNode), "Entry")]
[JsonDerivedType(typeof(BranchNode), "Branch")]
[JsonDerivedType(typeof(LoopNode), "Loop")]
[JsonDerivedType(typeof(BreakNode), "Break")]
[JsonDerivedType(typeof(ConstNode), "Const")]
[JsonDerivedType(typeof(CallNode), "Call")]
[JsonDerivedType(typeof(CallHelperNode), "CallHelper")]
[JsonDerivedType(typeof(GetNode), "Get")]
[JsonDerivedType(typeof(SetNode), "Set")]
[JsonDerivedType(typeof(PrintNode), "Print")]
[JsonDerivedType(typeof(PauseNode), "Pause")]
[JsonDerivedType(typeof(VariableNode), "Variable")]
[JsonDerivedType(typeof(BuiltinFunctionNode), "BuiltinFunction")]
[JsonDerivedType(typeof(PluginTriggerNode), "PluginTrigger")]
public abstract class BlueprintNode
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

// Node-specific classes can be defined for additional properties
// Currently using the abstract base with runtime-added properties via dynamics or dedicated subclasses

/// <summary>
/// Entry node - execution entry point
/// </summary>
public class EntryNode : BlueprintNode
{
    public EntryNode()
    {
        NodeType = BlueprintNodeType.Entry;
        Name = "Entry";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 60,
        InputPins: [],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 30)],
        DisplayName: "Entry"
    );
}

/// <summary>
/// Plugin trigger entry node - activated when a specific plugin fires a specific trigger signal.
/// Has same pin structure as Entry (0 input, 1 Exec output) but carries PluginName/TriggerName metadata.
/// </summary>
public class PluginTriggerNode : BlueprintNode
{
    /// <summary>The plugin name to listen for</summary>
    public string PluginName { get; set; } = string.Empty;

    /// <summary>The trigger name to listen for</summary>
    public string TriggerName { get; set; } = string.Empty;

    public PluginTriggerNode()
    {
        NodeType = BlueprintNodeType.PluginTrigger;
        Name = "PluginTrigger";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 160, Height: 60,
        InputPins: [],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 30)],
        DisplayName: "PluginTrigger"
    );

    public override string GetDisplayTitle()
        => string.IsNullOrEmpty(PluginName)
            ? $"Trigger: {TriggerName}"
            : $"Trigger: {PluginName}.{TriggerName}";
}

/// <summary>
/// Branch node - conditional execution
/// </summary>
public class BranchNode : BlueprintNode
{
    public BranchNode()
    {
        NodeType = BlueprintNodeType.Branch;
        Name = "Branch";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 80,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 30),
            new PinDescriptor("Condition", PinType.Boolean, 50)
        ],
        OutputPins: [
            new PinDescriptor("True", PinType.Execution, 30),
            new PinDescriptor("False", PinType.Execution, 50)
        ],
        DisplayName: "Branch"
    );
}

/// <summary>
/// Loop node - iterative execution
/// </summary>
public class LoopNode : BlueprintNode
{
    public LoopNode()
    {
        NodeType = BlueprintNodeType.Loop;
        Name = "Loop";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 80,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 30),
            new PinDescriptor("Condition", PinType.Boolean, 50)
        ],
        OutputPins: [
            new PinDescriptor("LoopBody", PinType.Execution, 30),
            new PinDescriptor("LoopEnd", PinType.Execution, 50)
        ],
        DisplayName: "Loop"
    );
}

/// <summary>
/// Break node - exit loop
/// </summary>
public class BreakNode : BlueprintNode
{
    public BreakNode()
    {
        NodeType = BlueprintNodeType.Break;
        Name = "Break";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 100, Height: 40,
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        OutputPins: [],
        DisplayName: "Break"
    );
}

/// <summary>
/// Const node - constant value
/// </summary>
public class ConstNode : BlueprintNode
{
    /// <summary>
    /// Constant name
    /// </summary>
    public string ConstName { get; set; } = string.Empty;

    /// <summary>
    /// Constant type
    /// </summary>
    public string ConstType { get; set; } = "string";

    /// <summary>
    /// Constant value
    /// </summary>
    public string? ConstValue { get; set; }

    public ConstNode()
    {
        NodeType = BlueprintNodeType.Const;
        Name = "Const";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 50,
        InputPins: [],
        OutputPins: [new PinDescriptor("Value", PinType.Any, 25)],
        DisplayName: "Const"
    );

    public override string GetDisplayTitle() => $"Const: {ConstName}";
}

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

    public CallNode()
    {
        NodeType = BlueprintNodeType.Call;
        Name = "Call";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 140, Height: 60,
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

/// <summary>
/// CallHelper node - helper function call
/// </summary>
public class CallHelperNode : BlueprintNode
{
    /// <summary>
    /// Helper function name reference
    /// </summary>
    public string HelperFunctionName { get; set; } = string.Empty;

    public CallHelperNode()
    {
        NodeType = BlueprintNodeType.CallHelper;
        Name = "CallHelper";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 130, Height: 50,
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 25)],
        OutputPins: [
            new PinDescriptor("Exec", PinType.Execution, 25),
            new PinDescriptor("Return", PinType.Any, 40)
        ],
        DisplayName: "CallHelper"
    );

    public override string GetDisplayTitle() => $"Helper: {HelperFunctionName}";
}

/// <summary>
/// Get variable value node - reads a PubVar
/// </summary>
public class GetNode : BlueprintNode
{
    /// <summary>
    /// Variable name to read
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    public GetNode()
    {
        NodeType = BlueprintNodeType.Get;
        Name = "Get";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 60,
        InputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        OutputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Value", PinType.Any, 40)
        ],
        DisplayName: "Get"
    );

    public override string GetDisplayTitle() => $"Get: {VarName}";
}

/// <summary>
/// Set variable value node - writes to a PubVar
/// </summary>
public class SetNode : BlueprintNode
{
    /// <summary>
    /// Variable name to write
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    public SetNode()
    {
        NodeType = BlueprintNodeType.Set;
        Name = "Set";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 60,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Value", PinType.Any, 40)
        ],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 20)],
        DisplayName: "Set"
    );

    public override string GetDisplayTitle() => $"Set: {VarName}";
}

/// <summary>
/// Print node - output information
/// </summary>
public class PrintNode : BlueprintNode
{
    public PrintNode()
    {
        NodeType = BlueprintNodeType.Print;
        Name = "Print";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 100, Height: 50,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Value", PinType.Any, 35)
        ],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 25)],
        DisplayName: "Print"
    );
}

/// <summary>
/// Pause node - pause execution
/// </summary>
public class PauseNode : BlueprintNode
{
    public PauseNode()
    {
        NodeType = BlueprintNodeType.Pause;
        Name = "Pause";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 100, Height: 50,
        InputPins: [
            new PinDescriptor("Exec", PinType.Execution, 20),
            new PinDescriptor("Milliseconds", PinType.Integer, 35)
        ],
        OutputPins: [new PinDescriptor("Exec", PinType.Execution, 25)],
        DisplayName: "Pause"
    );
}

/// <summary>
/// Variable node - ConstBlock variable without initial value.
/// A floating node with no input/output ports. Users can change the data type
/// but not the initial value. Type changes propagate to Get/Set nodes.
/// </summary>
public class VariableNode : BlueprintNode
{
    /// <summary>
    /// Variable name
    /// </summary>
    public string VarName { get; set; } = string.Empty;

    /// <summary>
    /// Variable type (e.g., "int", "double", "string", "bool")
    /// </summary>
    public string VarType { get; set; } = "int";

    public VariableNode()
    {
        NodeType = BlueprintNodeType.Variable;
        Name = "Variable";
        InitializePinsFromDescriptor();
    }

    public override NodeDescriptor GetDescriptor() => new(
        Width: 120, Height: 50,
        InputPins: [],
        OutputPins: [],
        DisplayName: "Variable"
    );

    public override string GetDisplayTitle() => $"Var: {VarName}";
}

/// <summary>
/// 通用内置函数节点。通过 <see cref="FunctionName"/> 区分具体函数。
/// 引脚布局由 <see cref="IBuiltinFunctionDefinition"/> 驱动，
/// 消除了为每个内置函数创建专用节点子类的需要。
/// </summary>
public class BuiltinFunctionNode : BlueprintNode
{
    /// <summary>
    /// BlockScript 函数名（如 "Flip"），作为具体函数的唯一标识
    /// </summary>
    public string FunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 额外属性字典，用于特殊节点（如 Set 的 VarName、Get 的 VarName）
    /// </summary>
    public Dictionary<string, string> Properties { get; set; } = [];

    private NodeDescriptor? _descriptor;

    /// <summary>
    /// 由 BuiltinFunctionRegistry 在创建节点时设置，基于 IBuiltinFunctionDefinition 的引脚描述
    /// </summary>
    public void SetDescriptor(NodeDescriptor descriptor) => _descriptor = descriptor;

    public override NodeDescriptor GetDescriptor() => _descriptor ?? new(
        Width: 120, Height: 60,
        InputPins: [],
        OutputPins: [],
        DisplayName: FunctionName
    );

    public BuiltinFunctionNode()
    {
        NodeType = BlueprintNodeType.BuiltinFunction;
        Name = "BuiltinFunction";
    }

    public override string GetDisplayTitle() => FunctionName;
}

/// <summary>
/// Connection between two pins
/// </summary>
public class BlueprintConnection
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Source node ID
    /// </summary>
    public string SourceNodeId { get; set; } = string.Empty;

    /// <summary>
    /// Source pin ID
    /// </summary>
    public string SourcePinId { get; set; } = string.Empty;

    /// <summary>
    /// Target node ID
    /// </summary>
    public string TargetNodeId { get; set; } = string.Empty;

    /// <summary>
    /// Target pin ID
    /// </summary>
    public string TargetPinId { get; set; } = string.Empty;

    /// <summary>
    /// Corresponding PubVar name for data flow connections (optional)
    /// </summary>
    public string? PubVarName { get; set; }
}

/// <summary>
/// Records a named block scope within a Blueprint.
/// Captures which nodes belong to a logical block,
/// preserving BlockScript block boundaries for reverse conversion.
/// </summary>
public class BlueprintBlockScope
{
    /// <summary>
    /// Block name (stable across round-trips, e.g. "MainBlock", "LoopBody", "SuccessLogic").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ordered list of node IDs that belong to this block.
    /// </summary>
    public List<string> NodeIds { get; set; } = [];

    /// <summary>
    /// Name of the next block to execute when this block ends naturally
    /// (i.e., not ended by Branch/Loop/ToLoopCond). Null if the block ends
    /// with a control-flow statement or is terminal.
    /// </summary>
    public string? NextBlockName { get; set; }

    /// <summary>
    /// For sub-blocks: the node ID of the Branch/Loop node that created this scope.
    /// Null for the main block scope.
    /// </summary>
    public string? OwnerNodeId { get; set; }

    /// <summary>
    /// For sub-blocks: which output arm of the owner node leads into this scope.
    /// E.g., "True", "False" for Branch; "LoopBody", "LoopEnd" for Loop.
    /// Null for the main block scope.
    /// </summary>
    public string? OwnerArmName { get; set; }

    /// <summary>
    /// Whether this block scope represents the main entry block.
    /// </summary>
    public bool IsMainBlock { get; set; }
}

/// <summary>
/// Blueprint document container
/// </summary>
public class Blueprint
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Document name
    /// </summary>
    public string Name { get; set; } = "Untitled";

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Last modified timestamp
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// All nodes in this blueprint
    /// </summary>
    public List<BlueprintNode> Nodes { get; set; } = [];

    /// <summary>
    /// All connections in this blueprint
    /// </summary>
    public List<BlueprintConnection> Connections { get; set; } = [];

    /// <summary>
    /// View zoom level (0.1 to 5.0)
    /// </summary>
    public double ZoomLevel { get; set; } = 1.0;

    /// <summary>
    /// View pan offset
    /// </summary>
    public ViewPoint PanOffset { get; set; } = new(0, 0);

    /// <summary>
    /// Helper functions available in this blueprint
    /// </summary>
    public List<HelperFunction> HelperFunctions { get; set; } = [];

    /// <summary>
    /// PubVar variable names (invisible in Blueprint, used for data flow)
    /// </summary>
    public List<string> PubVarNames { get; set; } = [];

    /// <summary>
    /// Constant values (from ConstBlock)
    /// </summary>
    public List<VariableConstant> ConstValues { get; set; } = [];

    /// <summary>
    /// Named block scopes recording which nodes belong to which logical block.
    /// Empty for legacy blueprints that predate this field.
    /// </summary>
    public List<BlueprintBlockScope> BlockScopes { get; set; } = [];

    /// <summary>
    /// Get node by ID
    /// </summary>
    public BlueprintNode? GetNodeById(string nodeId)
    {
        foreach (var node in Nodes)
            if (node.Id == nodeId) return node;
        return null;
    }

    /// <summary>
    /// Get connections from a node
    /// </summary>
    public IEnumerable<BlueprintConnection> GetConnectionsFrom(string nodeId)
    {
        foreach (var conn in Connections)
            if (conn.SourceNodeId == nodeId) yield return conn;
    }

    /// <summary>
    /// Get connections to a node
    /// </summary>
    public IEnumerable<BlueprintConnection> GetConnectionsTo(string nodeId)
    {
        foreach (var conn in Connections)
            if (conn.TargetNodeId == nodeId) yield return conn;
    }

    /// <summary>
    /// Add a node to this blueprint, automatically setting the back-reference
    /// </summary>
    /// <param name="node">Node to add</param>
    public void AddNode(BlueprintNode node)
    {
        node.Blueprint = this;
        Nodes.Add(node);
    }

    /// <summary>
    /// Add a connection to this blueprint (deduplicates by source/target/pin)
    /// </summary>
    /// <param name="connection">Connection to add</param>
    public void AddConnection(BlueprintConnection connection)
    {
        var alreadyExists = Connections.Any(c =>
            c.SourceNodeId == connection.SourceNodeId && c.SourcePinId == connection.SourcePinId &&
            c.TargetNodeId == connection.TargetNodeId && c.TargetPinId == connection.TargetPinId);
        if (!alreadyExists)
            Connections.Add(connection);
    }
}
