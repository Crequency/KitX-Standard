namespace KitX.Core.Contract.Workflow;

/// <summary>
/// 工作流触发器配置
/// </summary>
public class TriggerConfig
{
    /// <summary>
    /// 触发器类型：
    /// - "Manual": 手动触发（默认）
    /// - "PluginEvent": 插件事件触发
    /// Cron/FileWatcher/Webhook 等场景由专门插件通过 PluginEvent 实现
    /// </summary>
    public string TriggerType { get; set; } = "Manual";

    /// <summary>
    /// PluginEvent 类型：监听的插件名称
    /// </summary>
    public string? PluginName { get; set; }

    /// <summary>
    /// PluginEvent 类型：监听的触发器名称（null = 该插件的所有触发器）
    /// </summary>
    public string? TriggerName { get; set; }

    /// <summary>
    /// EntryNode / PluginTriggerNode 在蓝图画布上的 X 坐标。
    /// EntryNode 是纯合成节点（不对应任何 IrBlock），其坐标不进入 IR 注解体系，
    /// 而是随触发配置一同持久化（入口标记与触发方式天然关联）。
    /// </summary>
    public double EntryNodeX { get; set; }

    /// <summary>
    /// EntryNode / PluginTriggerNode 在蓝图画布上的 Y 坐标。
    /// </summary>
    public double EntryNodeY { get; set; }
}
