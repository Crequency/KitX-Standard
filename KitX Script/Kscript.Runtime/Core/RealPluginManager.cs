using System.Text.Json;
using Kscript.CSharp.Parser.Models;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;

using Serilog;
using System.Collections.Concurrent;

namespace Kscript.CSharp.Parser.Core;

/// <summary>
/// 实际的插件管理器 - 使用 KitX Dashboard 的插件系统进行真实调用
/// </summary>
public class RealPluginManager : IPluginManager
{
    private readonly IPluginServiceProvider _serviceProvider;
    private readonly Action<string> _infoLogger;
    private readonly Action<string> _errorLogger;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
    };

    // 用于存储等待响应的请求
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests = new();

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider">插件服务提供者实例</param>
    /// <param name="infoLogger">信息日志记录器</param>
    /// <param name="errorLogger">错误日志记录器</param>
    public RealPluginManager(
        IPluginServiceProvider serviceProvider,
        Action<string>? infoLogger = null,
        Action<string>? errorLogger = null)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _infoLogger = infoLogger ?? (message => Console.WriteLine(message));
        _errorLogger = errorLogger ?? (message => Console.WriteLine(message));

        _infoLogger("[RealPluginManager] 正在初始化 RealPluginManager...");

        // 订阅插件响应事件
        _serviceProvider.SubscribeToResponses(HandlePluginResponse);

        _infoLogger("[RealPluginManager] RealPluginManager 初始化完成");
    }

    /// <summary>
    /// 调用插件方法
    /// </summary>
    public T Call<T>(PluginCallInfo callInfo)
    {
        try
        {
            _infoLogger($"[RealPluginManager] 开始调用插件方法: {callInfo}");

            // 查找插件信息
            var pluginInfo = _serviceProvider.FindPlugin(callInfo.PluginName);
            if (pluginInfo == null)
            {
                _infoLogger($"[RealPluginManager] 未找到插件: {callInfo.PluginName}");
                return GetDefaultResult<T>();
            }

            // 查找插件连接器
            var connector = _serviceProvider.FindConnector(pluginInfo);
            if (connector == null)
            {
                _infoLogger($"[RealPluginManager] 插件 {callInfo.PluginName} 未连接");
                return GetDefaultResult<T>();
            }

            // 创建并发送请求
            var result = SendPluginRequest<T>(connector, callInfo).GetAwaiter().GetResult();

            _infoLogger($"[RealPluginManager] 插件调用完成: {callInfo} -> 结果: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 调用插件方法失败: {callInfo} - 异常: {ex.Message}");
            return GetDefaultResult<T>();
        }
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
        try
        {
            var pluginInfo = _serviceProvider.FindPlugin(pluginName);
            var exists = pluginInfo != null;

            _infoLogger($"[RealPluginManager] 检查插件 '{pluginName}' 是否存在: {exists}");
            return exists;
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 检查插件存在性失败: {pluginName} - 异常: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 检查插件方法是否存在
    /// </summary>
    public bool IsMethodExists(string pluginName, string methodName)
    {
        try
        {
            var pluginInfo = _serviceProvider.FindPlugin(pluginName);
            var exists = pluginInfo?.Functions.Any(f => f.Name == methodName) ?? false;

            _infoLogger($"[RealPluginManager] 检查方法 '{pluginName}.{methodName}' 是否存在: {exists}");
            return exists;
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 检查方法存在性失败: {pluginName}.{methodName} - 异常: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 发送插件请求
    /// </summary>
    private async Task<T> SendPluginRequest<T>(object connector, PluginCallInfo callInfo)
    {
        try
        {
            // 创建 Connector 实例
            var connectorInstance = new Connector()
                .SetSerializer(x => JsonSerializer.Serialize(x, _serializerOptions))
                .SetSender(request => {
                    _serviceProvider.SendRequestAsync(connector, request).ConfigureAwait(false);
                });

            // 构建参数列表
            var functionArgs = new List<Parameter>();
            for (int i = 0; i < callInfo.Parameters.Length; i++)
            {
                var paramValue = callInfo.Parameters[i]?.ToString() ?? string.Empty;
                var paramName = callInfo.ParameterNames?.Length > i ? callInfo.ParameterNames[i] : $"param{i}";
                var paramType = callInfo.ParameterTypes?.Length > i ? callInfo.ParameterTypes[i].Name : "string";

                functionArgs.Add(new Parameter
                {
                    Name = paramName,
                    Type = paramType,
                    Value = paramValue,
                    IsOptional = false
                });
            }

            // 生成唯一的请求ID
            var requestId = Guid.NewGuid().ToString();

            // 创建任务完成源，用于等待响应
            var tcs = new TaskCompletionSource<string>();
            _pendingRequests[requestId] = tcs;

            // 发送请求
            var request = connectorInstance
                .Request()
                .ReceiveCommand()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = callInfo.MethodName;
                    cmd.FunctionArgs = functionArgs;
                    cmd.PluginConnectionId = callInfo.PluginName;
                    cmd.Tags = new Dictionary<string, string>
                    {
                        ["RequestId"] = requestId
                    };
                    return cmd;
                })
                .Send();

            // 等待响应，设置超时时间为30秒
            var timeout = TimeSpan.FromSeconds(30);
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(timeout));

            if (completedTask == tcs.Task)
            {
                // 获取到响应
                var responseJson = tcs.Task.Result;
                _infoLogger($"[RealPluginManager] 收到插件响应: {responseJson}");

                // 插件的函数返回值位于响应 Command 的 Body 中（与原生调用路径
                // PluginsManager.CallPluginFunctionAsync 的解码行为对齐）。不解码
                // 会把整个 Command 包装 JSON 当作返回值，导致工作流 PluginCall
                // 拿到的不是函数结果（v6 Trigger 测试暴露的问题）。
                var bodyText = TryDecodeResponseBody(responseJson);
                if (bodyText is not null)
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)bodyText;
                    try
                    {
                        return JsonSerializer.Deserialize<T>(bodyText, _serializerOptions) ?? GetDefaultResult<T>();
                    }
                    catch (JsonException)
                    {
                        return GetDefaultResult<T>();
                    }
                }

                // 尝试反序列化响应
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)responseJson;
                    }
                    else if (typeof(T) == typeof(void))
                    {
                        return default(T)!;
                    }
                    else
                    {
                        return JsonSerializer.Deserialize<T>(responseJson, _serializerOptions) ?? GetDefaultResult<T>();
                    }
                }
                catch (JsonException ex)
                {
                    _errorLogger($"[RealPluginManager] 反序列化响应失败: {ex.Message}");
                    return GetDefaultResult<T>();
                }
            }
            else
            {
                // 超时
                _errorLogger($"[RealPluginManager] 插件调用超时: {callInfo.PluginName}.{callInfo.MethodName}");
                _pendingRequests.TryRemove(requestId, out _);
                return GetDefaultResult<T>();
            }
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 发送插件请求失败: {callInfo.PluginName}.{callInfo.MethodName} - 异常: {ex.Message}");
            return GetDefaultResult<T>();
        }
    }

    /// <summary>
    /// 尝试把插件响应解析为 Command 包装并解码其 Body。响应不是 Command 格式时返回 null，
    /// 由调用方走原逻辑。
    /// </summary>
    private string? TryDecodeResponseBody(string responseJson)
    {
        try
        {
            var resp = JsonSerializer.Deserialize<Command>(responseJson, _serializerOptions);
            if (resp.Body is { Length: > 0 } && resp.BodyLength > 0)
            {
                var bytes = resp.BodyLength <= resp.Body.Length ? resp.Body.AsSpan(0, resp.BodyLength).ToArray() : resp.Body;
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// 处理插件响应
    /// </summary>
    public void HandlePluginResponse(string requestId, string responseJson)
    {
        if (_pendingRequests.TryRemove(requestId, out var tcs))
        {
            tcs.SetResult(responseJson);
        }
        else
        {
            _infoLogger($"[RealPluginManager] 收到未知请求的响应: {requestId}");
        }
    }

    /// <summary>
    /// 获取默认结果
    /// </summary>
    private T GetDefaultResult<T>()
    {
        if (typeof(T) == typeof(void))
            return default(T)!;

        if (typeof(T) == typeof(string))
            return (T)(object)string.Empty;

        if (typeof(T).IsValueType)
            return (T)Activator.CreateInstance(typeof(T))!;

        return default(T)!;
    }
}
