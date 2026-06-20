using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// KCS (KitX Code Script) 文件格式定义
/// </summary>
public class KcsFileFormat
{
    /// <summary>
    /// 工作流唯一标识符
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 工作流名称
    /// </summary>
    public string Name { get; set; } = "Untitled Workflow";

    /// <summary>
    /// 工作流描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 作者名称
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime LastModifiedTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 触发器配置（包含触发类型、插件名、触发器名等结构化字段）
    /// </summary>
    public TriggerConfig? TriggerConfig { get; set; }

    /// <summary>
    /// 主程序代码
    /// </summary>
    public string MainProgram { get; set; } = string.Empty;

    /// <summary>
    /// 辅助函数列表
    /// </summary>
    public List<HelperFunction> HelperFunctions { get; set; } = [];

    /// <summary>
    /// 可变常量及其用户修改后的值
    /// </summary>
    public Dictionary<string, object?> VariableConstants { get; set; } = [];

    /// <summary>
    /// 是否使用块脚本模式
    /// </summary>
    /// <remarks>
    /// 当为 true 时，使用 BlockScriptSource 作为脚本内容
    /// </remarks>
    public bool UseBlockMode { get; set; } = false;

    /// <summary>
    /// 块脚本源代码（当 UseBlockMode 为 true 时使用）
    /// </summary>
    public string? BlockScriptSource { get; set; }

    /// <summary>
    /// 蓝图可视化数据（包含节点位置、连接关系、视图状态等）
    /// 当 UseBlockMode=true 且此字段非空时，表示该脚本有对应的蓝图编辑状态
    /// </summary>
    public Blueprint? BlueprintData { get; set; }
}

/// <summary>
/// 辅助函数参数定义
/// </summary>
public class HelperFunctionParameter
{
    /// <summary>
    /// 参数名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参数类型
    /// </summary>
    public string Type { get; set; } = "object";
}

/// <summary>
/// 辅助函数定义
/// </summary>
public class HelperFunction
{
    /// <summary>
    /// 函数名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 函数参数列表
    /// </summary>
    public List<HelperFunctionParameter> Parameters { get; set; } = [];

    /// <summary>
    /// 返回值类型
    /// </summary>
    public string ReturnType { get; set; } = "object";

    /// <summary>
    /// 函数体代码（不包括函数签名）
    /// </summary>
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// 可变常量（用于UI显示）
/// </summary>
public class VariableConstant
{
    /// <summary>
    /// 常量名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 默认值
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// 用户修改后的值
    /// </summary>
    public object? UserValue { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public string Type { get; set; } = "string";
}

