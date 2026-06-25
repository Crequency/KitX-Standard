using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// v5.1: CFG serialization DTO — mirrors ControlFlowGraph for JSON persistence.
/// This is the canonical serialization format for .kcs files in the CFG-as-truth architecture.
/// </summary>
public class CfgDto
{
    public string Version { get; set; } = "5.1";
    public string MainBlockName { get; set; } = "MainBlock";
    public List<CfgBlockDto> Blocks { get; set; } = [];
    public List<string> PubVarDeclarations { get; set; } = [];
    public Dictionary<string, string> PubVarTypes { get; set; } = [];
    public List<CfgConstDto> ConstDeclarations { get; set; } = [];
    public List<HelperFunction> HelperFunctions { get; set; } = [];
    public int PubVarCounter { get; set; } = 1;
    public CfgViewportDto? Viewport { get; set; }
}

public class CfgBlockDto
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "Basic";
    public List<CfgStmtDto> Statements { get; set; } = [];
    public List<CfgEdgeDto> Successors { get; set; } = [];
    public List<CfgVarDeclDto> BlockVars { get; set; } = [];
    public bool HasExplicitBlockBody { get; set; }
    public string? BlockComment { get; set; }
    public double? LayoutX { get; set; }
    public double? LayoutY { get; set; }
}

public class CfgStmtDto
{
    public string StatementId { get; set; } = "";
    public string? Kind { get; set; }
    public bool IsBlockTerminator { get; set; }
    public string? OriginalExpression { get; set; }
    public int SourceLine { get; set; }
    public string? PubVarTarget { get; set; }
    public string? FunctionName { get; set; }
    public string? FullFunctionName { get; set; }
    public List<string> Arguments { get; set; } = [];
    public string? ConditionExpression { get; set; }
    public List<CfgArmDto> Arms { get; set; } = [];
    public string? Comment { get; set; }
    public string? PipelineSource { get; set; }
}

public class CfgEdgeDto
{
    public string Type { get; set; } = "Sequential";
    public string FromBlockName { get; set; } = "";
    public string ToBlockName { get; set; } = "";
    public string? PinName { get; set; }
}

public class CfgArmDto
{
    public string PinName { get; set; } = "";
    public string? TargetBlockName { get; set; }
}

public class CfgConstDto
{
    public string Name { get; set; } = "";
    public string? Type { get; set; }
    public object? DefaultValue { get; set; }
    public string? InitialValueExpression { get; set; }
}

public class CfgVarDeclDto
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "dynamic";
    public object? DefaultValue { get; set; }
}

public class CfgViewportDto
{
    public double ZoomLevel { get; set; } = 1.0;
    public double PanX { get; set; }
    public double PanY { get; set; }
}
