using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Parser.Core;

/// <summary>
/// 插件服务提供者接口 - 由Dashboard实现
/// 用于解耦Parser和Dashboard，避免直接依赖和反射
/// </summary>
public interface IPluginServiceProvider
{
    /// <summary>
    /// 获取所有运行中的插件信息
    /// </summary>
    /// <returns>运行中的插件信息集合</returns>
    IEnumerable<PluginInfo> GetRunningPlugins();

    /// <summary>
    /// 根据插件名称查找插件信息
    /// </summary>
    /// <param name="pluginName">插件名称</param>
    /// <returns>插件信息，如果未找到则返回null</returns>
    PluginInfo? FindPlugin(string pluginName);

    /// <summary>
    /// 查找插件连接器
    /// </summary>
    /// <param name="pluginInfo">插件信息</param>
    /// <returns>插件连接器对象，如果未找到则返回null</returns>
    object? FindConnector(PluginInfo pluginInfo);

    /// <summary>
    /// 向插件连接器发送请求
    /// </summary>
    /// <param name="connector">插件连接器</param>
    /// <param name="request">请求对象</param>
    /// <returns>异步任务</returns>
    Task SendRequestAsync(object connector, object request);

    /// <summary>
    /// 订阅插件响应事件
    /// </summary>
    /// <param name="responseHandler">响应处理器，接收(requestId, responseJson)</param>
    void SubscribeToResponses(Action<string, string> responseHandler);
}
