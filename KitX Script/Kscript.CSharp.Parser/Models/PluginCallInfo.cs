namespace Kscript.CSharp.Parser.Models;

/// <summary>
/// 插件调用信息，用于传递给 PluginManager.Call 的参数（模拟，可能根据外部实现发生更改）
/// </summary>
public class PluginCallInfo
{
    /// <summary>
    /// 插件名称
    /// </summary>
    public string PluginName { get; set; } = string.Empty;

    /// <summary>
    /// 方法名称
    /// </summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// 方法参数数组
    /// </summary>
    public object[] Parameters { get; set; } = Array.Empty<object>();

    /// <summary>
    /// 参数类型数组
    /// </summary>
    public Type[] ParameterTypes { get; set; } = Array.Empty<Type>();

    public PluginCallInfo()
    {
    }

    public PluginCallInfo(string pluginName, string methodName, object[] parameters, Type[] parameterTypes)
    {
        PluginName = pluginName;
        MethodName = methodName;
        Parameters = parameters;
        ParameterTypes = parameterTypes;
    }

    public override string ToString()
    {
        return $"{PluginName}.{MethodName}({string.Join(", ", Parameters)})";
    }
}
