using Kscript.CSharp.Parser.Models;

namespace Kscript.CSharp.Parser.Core;

/// <summary>
/// 插件管理器接口 - 模拟接口，实际实现由外部项目提供
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// 调用插件方法
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="callInfo">调用信息</param>
    /// <returns>插件方法的返回值</returns>
    T Call<T>(PluginCallInfo callInfo);

    /// <summary>
    /// 调用插件方法（无返回值）
    /// </summary>
    /// <param name="callInfo">调用信息</param>
    void Call(PluginCallInfo callInfo);

    /// <summary>
    /// 发送插件方法调用但不等待响应（fire-and-forget）。
    /// 生产实现应真正单向发送；测试/占位实现可退回同步 Call（接口默认实现）。
    /// </summary>
    void Notify(PluginCallInfo callInfo) => Call(callInfo);

    /// <summary>
    /// 检查插件是否存在
    /// </summary>
    /// <param name="pluginName">插件名称</param>
    /// <returns>插件是否存在</returns>
    bool IsPluginExists(string pluginName);

    /// <summary>
    /// 检查插件方法是否存在
    /// </summary>
    /// <param name="pluginName">插件名称</param>
    /// <param name="methodName">方法名称</param>
    /// <returns>方法是否存在</returns>
    bool IsMethodExists(string pluginName, string methodName);
}
