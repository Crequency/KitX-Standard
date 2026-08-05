using Kscript.CSharp.Parser.Models;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Parser.Core;

/// <summary>
/// 模拟插件管理器 - 用于调试和测试
/// </summary>
public class MockPluginManager : IPluginManager
{
    /// <summary>
    /// 模拟的插件数据，用于验证调用
    /// </summary>
    private readonly Dictionary<string, Dictionary<string, object>> _mockData = new()
    {
        {
            "SampleCalculator", new Dictionary<string, object>
            {
                { "Add", (int a, int b) => a + b },
                { "Multiply", (double x, double y) => x * y },
                { "Divide", (double numerator, double denominator, int decimals = 2) => Math.Round(numerator / denominator, decimals) }
            }
        },
        {
            "StringToolkit", new Dictionary<string, object>
            {
                { "Reverse", (string text) => new string(text.Reverse().ToArray()) },
                { "ToUpper", (string text) => text.ToUpperInvariant() },
                { "Concat", (string str1, string str2) => str1 + str2 }
            }
        },
        {
            "KitXWF", new Dictionary<string, object>
            {
                { "Print", (string message) => Console.WriteLine(message) }
            }
        }
    };

    /// <summary>
    /// 调用插件方法
    /// </summary>
    public T Call<T>(PluginCallInfo callInfo)
    {
        Console.WriteLine($"[MockPluginManager] 调用插件方法: {callInfo}");
        var paramInfo = callInfo.Parameters.Select((p, i) => {
            var name = callInfo.ParameterNames?.Length > i ? callInfo.ParameterNames[i] : $"param{i}";
            var type = callInfo.ParameterTypes?.Length > i ? callInfo.ParameterTypes[i].Name : "object";
            return $"{name}:{type}={p}";
        });
        Console.WriteLine($"[MockPluginManager] 参数: [{string.Join(", ", paramInfo)}]");
        Console.WriteLine($"[MockPluginManager] 期望返回类型: {typeof(T).Name}");

        // 简单的模拟实现
        if (_mockData.TryGetValue(callInfo.PluginName, out var plugin) &&
            plugin.TryGetValue(callInfo.MethodName, out var method))
        {
            try
            {
                var result = InvokeMockMethod(callInfo, method);
                Console.WriteLine($"[MockPluginManager] 返回结果: {result}");

                if (result is T typedResult)
                    return typedResult;

                // 尝试类型转换
                if (result != null && typeof(T) != typeof(void))
                {
                    var convertedResult = Convert.ChangeType(result, typeof(T));
                    return (T)convertedResult;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MockPluginManager] 调用异常: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"[MockPluginManager] 未找到插件方法: {callInfo.PluginName}.{callInfo.MethodName}");
        }

        // 返回默认值
        if (typeof(T) == typeof(void))
            return default(T)!;

        return (T)GetDefaultValue(typeof(T))!;
    }

    /// <summary>
    /// 调用插件方法（无返回值）
    /// </summary>
    public void Call(PluginCallInfo callInfo)
    {
        Call<object>(callInfo);
    }

    /// <summary>
    /// 检查插件是否存在
    /// </summary>
    public bool IsPluginExists(string pluginName)
    {
        var exists = _mockData.ContainsKey(pluginName);
        Console.WriteLine($"[MockPluginManager] 检查插件 '{pluginName}' 是否存在: {exists}");
        return exists;
    }

    /// <summary>
    /// 检查插件方法是否存在
    /// </summary>
    public bool IsMethodExists(string pluginName, string methodName)
    {
        var exists = _mockData.TryGetValue(pluginName, out var plugin) &&
                    plugin.ContainsKey(methodName);
        Console.WriteLine($"[MockPluginManager] 检查方法 '{pluginName}.{methodName}' 是否存在: {exists}");
        return exists;
    }

    /// <summary>
    /// 调用模拟方法
    /// </summary>
    private object? InvokeMockMethod(PluginCallInfo callInfo, object method)
    {
        // 根据插件和方法名进行简单的模拟计算
        switch (callInfo.PluginName)
        {
            case "SampleCalculator":
                switch (callInfo.MethodName)
                {
                    case "Add" when callInfo.Parameters.Length >= 2:
                        return Convert.ToInt32(callInfo.Parameters[0]) + Convert.ToInt32(callInfo.Parameters[1]);
                    case "Multiply" when callInfo.Parameters.Length >= 2:
                        return Convert.ToDouble(callInfo.Parameters[0]) * Convert.ToDouble(callInfo.Parameters[1]);
                    case "Divide" when callInfo.Parameters.Length >= 2:
                        return callInfo.Parameters.Length >= 3
                            ? Math.Round(Convert.ToDouble(callInfo.Parameters[0]) / Convert.ToDouble(callInfo.Parameters[1]), Convert.ToInt32(callInfo.Parameters[2]))
                            : Math.Round(Convert.ToDouble(callInfo.Parameters[0]) / Convert.ToDouble(callInfo.Parameters[1]), 2);
                }
                break;
            case "KitXWF":
                switch (callInfo.MethodName)
                {
                    case "Print" when callInfo.Parameters.Length >= 1:
                        Console.WriteLine($"[KitXWF] {callInfo.Parameters[0]?.ToString() ?? ""}");
                        return null;
                }
                break;
            case "StringToolkit":
                switch (callInfo.MethodName)
                {
                    case "Reverse" when callInfo.Parameters.Length >= 1:
                        return new string(callInfo.Parameters[0]?.ToString()?.Reverse().ToArray() ?? Array.Empty<char>());
                    case "ToUpper" when callInfo.Parameters.Length >= 1:
                        return callInfo.Parameters[0]?.ToString()?.ToUpperInvariant() ?? string.Empty;
                    case "Concat" when callInfo.Parameters.Length >= 2:
                        return callInfo.Parameters[0]?.ToString() + callInfo.Parameters[1]?.ToString();
                }
                break;
        }

        return $"MockResult_{callInfo.MethodName}";
    }

    /// <summary>
    /// 获取类型的默认值
    /// </summary>
    private static object? GetDefaultValue(Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type);
        return null;
    }
}
