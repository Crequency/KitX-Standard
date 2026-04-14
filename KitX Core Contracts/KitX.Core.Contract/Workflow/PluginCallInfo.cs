using System;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// 插件调用信息，用于传递给 IPluginManager.Call 的参数。
/// Copy of Kscript.CSharp.Parser.Models.PluginCallInfo for BlockScripting use
/// without depending on the KCS parser assembly.
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
    /// 方法参数值数组
    /// </summary>
    public object[] Parameters { get; set; } = Array.Empty<object>();

    /// <summary>
    /// 参数类型数组
    /// </summary>
    public Type[] ParameterTypes { get; set; } = Array.Empty<Type>();

    /// <summary>
    /// 参数名称数组
    /// </summary>
    public string[] ParameterNames { get; set; } = Array.Empty<string>();

    public PluginCallInfo()
    {
    }

    public PluginCallInfo(string pluginName, string methodName, object[] parameters, Type[] parameterTypes, string[] parameterNames)
    {
        PluginName = pluginName;
        MethodName = methodName;
        Parameters = parameters;
        ParameterTypes = parameterTypes;
        ParameterNames = parameterNames;
    }

    public override string ToString()
    {
        return $"{PluginName}.{MethodName}({string.Join(", ", Parameters)})";
    }
}