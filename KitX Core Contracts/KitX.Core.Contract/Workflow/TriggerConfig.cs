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
}
