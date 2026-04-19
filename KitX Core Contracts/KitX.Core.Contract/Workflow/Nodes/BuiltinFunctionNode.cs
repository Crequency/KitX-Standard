using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

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