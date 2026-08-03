using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

// ─────────────────────────────────────────────────────────────────────────────
// KcsFileFormat v2 — IR as the single source of truth.
//
// v1 stored three redundant representations side-by-side: BS text
// (MainProgram/BlockScriptSource), the mutable Blueprint graph (BlueprintData),
// and the v5.1 CFG DTO (CfgData). Under the v6.0 architecture (IR-Architecture-
// v6.0.md §2) IR is the sole truth and BS/BP are projections produced on demand
// by BsTextLens.Project / BpGraphLens.Project. v2 collapses the three into one
// stored IR blob (IrData) plus the pure-metadata envelope fields that are NOT
// part of IR semantics (identity, authoring, trigger config, user constant
// overrides).
//
// Dropped v1 fields      → how they are recovered
//   MainProgram            BsTextLens.Project(ir)  (on-demand BS view)
//   BlockScriptSource      ditto
//   BlueprintData          BpGraphLens.Project(ir) (on-demand BP view)
//   CfgData                replaced wholesale by IrData (IrDto v6.0)
//   UseBlockMode           meaningless under IR-as-truth (no "mode")
//   HelperFunctions        already carried by IrWorkflow.HelperFunctions
//
// Kept envelope fields (NOT derivable from IR):
//   Id / Name / Description / Author / timestamps — workflow identity/metadata
//   TriggerConfig                                  — deployment/runtime concern
//   VariableConstants                              — user overrides on IrConstant
//
// Migration: KitX.WorkflowMigrator converts v1 → v2 (BS → parse → IR → serialize).
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// KCS (KitX Code Script) 文件格式定义 — v2 (IR as storage).
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
    /// 可变常量及其用户修改后的值。IR 的 <c>Constants</c> 存默认值，这里只存用户的覆盖值。
    /// 加载时合并：IR 提供默认值，信封覆盖值优先。
    /// </summary>
    public Dictionary<string, object?> VariableConstants { get; set; } = [];

    /// <summary>
    /// v2: 工作流的 IR 序列化形式（<c>IrSerializer.Serialize(ir)</c> 产出的 JSON 字符串）。
    /// 这是工作流的唯一真相源——BS 文本与 BP 图都是它的投影，按需生成，不再持久化。
    /// </summary>
    public string IrData { get; set; } = "{}";

    /// <summary>
    /// IR 格式版本："<c>v5</c>"（KitX.Workflow.Serialization.IrSerializer）或
    /// "<c>v6</c>"（KitX.WorkflowV6.Serialization.WorkflowSerializer）。
    /// 默认 "<c>v5</c>" 保持向后兼容——现有 .kcs 文件无此字段，反序列化时取默认值。
    /// V6 工具（KcsBuilder）写入 "<c>v6</c>"。Dashboard 打开时据此选择编辑器。
    /// </summary>
    public string IrVersion { get; set; } = "v5";

    /// <summary>
    /// BP 画布布局（v6）：节点规范 ID → 画布坐标。规范 ID 是节点在
    /// <c>KitX.WorkflowV6.Ir.NodeId.Of(path)</c>（FNV-1a of BpRenderer path）——
    /// 与 <c>BpGraphLens.Project</c> 重投影后的节点 ID 一致，故加载端可直接查表覆写坐标。
    /// 随机画布 ID 永不进入本字典（保存端经 Reverse 的 nodeId→canonical 映射归一化）。
    /// 空/缺省 = 使用布局网格位（LayoutService）。旧 .kcs 文件无此字段（反序列化为 null）。
    /// </summary>
    public Dictionary<string, BlueprintLayoutEntry>? BlueprintLayout { get; set; }
}

/// <summary>
/// BP 画布布局条目：单个节点的画布坐标（<see cref="KcsFileFormat.BlueprintLayout"/> 的值）。
/// </summary>
public class BlueprintLayoutEntry
{
    /// <summary>
    /// 画布 X 坐标
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// 画布 Y 坐标
    /// </summary>
    public double Y { get; set; }
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
