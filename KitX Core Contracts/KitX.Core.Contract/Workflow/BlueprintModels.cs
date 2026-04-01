using System;
using System.Collections.Generic;
using System.Linq;
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
    Pause
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

    /// <summary>
    /// Connected pin ID (for runtime connections)
    /// </summary>
    public string? ConnectedPinId { get; set; }
}

/// <summary>
/// Base class for all blueprint nodes
/// </summary>
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
    /// Parent blueprint reference (set when node is added to blueprint)
    /// </summary>
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
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        InputPins.Add(new BlueprintPin
        {
            Name = "Condition",
            Direction = PinDirection.Input,
            Type = PinType.Boolean
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "True",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "False",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        InputPins.Add(new BlueprintPin
        {
            Name = "Condition",
            Direction = PinDirection.Input,
            Type = PinType.Boolean
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "LoopBody",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "LoopEnd",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
    }
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
        OutputPins.Add(new BlueprintPin
        {
            Name = "Value",
            Direction = PinDirection.Output,
            Type = PinType.Any
        });
    }
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

    public CallNode()
    {
        NodeType = BlueprintNodeType.Call;
        Name = "Call";
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Return",
            Direction = PinDirection.Output,
            Type = PinType.Any
        });
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Return",
            Direction = PinDirection.Output,
            Type = PinType.Any
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Value",
            Direction = PinDirection.Output,
            Type = PinType.Any
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        InputPins.Add(new BlueprintPin
        {
            Name = "Value",
            Direction = PinDirection.Input,
            Type = PinType.Any
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        InputPins.Add(new BlueprintPin
        {
            Name = "Value",
            Direction = PinDirection.Input,
            Type = PinType.Any
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
    }
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
        InputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Input,
            Type = PinType.Execution
        });
        InputPins.Add(new BlueprintPin
        {
            Name = "Milliseconds",
            Direction = PinDirection.Input,
            Type = PinType.Integer
        });
        OutputPins.Add(new BlueprintPin
        {
            Name = "Exec",
            Direction = PinDirection.Output,
            Type = PinType.Execution
        });
    }
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
