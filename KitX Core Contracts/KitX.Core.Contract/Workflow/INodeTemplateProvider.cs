using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// 节点模板接口 - 统一手动和自动创建的节点定义
/// </summary>
public interface INodeTemplateProvider
{
    /// <summary>
    /// 创建指定类型的节点
    /// </summary>
    BlueprintNode CreateNode(BlueprintNodeType type);

    /// <summary>
    /// 获取节点模板
    /// </summary>
    IReadOnlyDictionary<BlueprintNodeType, NodeTemplate> GetTemplates();

    /// <summary>
    /// 获取节点尺寸
    /// </summary>
    (double Width, double Height) GetNodeSize(BlueprintNodeType type);

    /// <summary>
    /// 获取输入引脚的相对 Y 坐标
    /// </summary>
    double GetInputPinY(BlueprintNodeType nodeType, string pinName);

    /// <summary>
    /// 获取输出引脚的相对 Y 坐标
    /// </summary>
    double GetOutputPinY(BlueprintNodeType nodeType, string pinName);
}

/// <summary>
/// 节点模板定义
/// </summary>
public class NodeTemplate
{
    public BlueprintNodeType NodeType { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Width { get; set; } = 120;
    public double Height { get; set; } = 60;
    public List<PinTemplate> InputPins { get; set; } = [];
    public List<PinTemplate> OutputPins { get; set; } = [];
}

/// <summary>
/// 引脚模板定义
/// </summary>
public class PinTemplate
{
    public string Name { get; set; } = string.Empty;
    public PinType Type { get; set; } = PinType.Any;
    public double RelativeY { get; set; }
}
