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
    private readonly object _pluginsServer;
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
    /// <param name="pluginsServer">PluginsServer 实例</param>
    /// <param name="infoLogger">信息日志记录器</param>
    /// <param name="errorLogger">错误日志记录器</param>
    public RealPluginManager(object pluginsServer, Action<string>? infoLogger = null, Action<string>? errorLogger = null)
    {
        _pluginsServer = pluginsServer ?? throw new ArgumentNullException(nameof(pluginsServer));
        _infoLogger = infoLogger ?? (message => Console.WriteLine(message));
        _errorLogger = errorLogger ?? (message => Console.WriteLine(message));

        _infoLogger("[WorkflowScriptService] 正在初始化 RealPluginManager...");
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
            var pluginInfo = FindPluginInfo(callInfo.PluginName);
            if (pluginInfo == null)
            {
                _infoLogger($"[RealPluginManager] 未找到插件: {callInfo.PluginName}");
                return GetDefaultResult<T>();
            }

            // 查找插件连接器
            var connector = FindPluginConnector(pluginInfo);
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
            var pluginInfo = FindPluginInfo(pluginName);
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
            var pluginInfo = FindPluginInfo(pluginName);
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
    /// 查找插件信息
    /// </summary>
    private PluginInfo? FindPluginInfo(string pluginName)
    {
        try
        {
            // 通过传入的 PluginsServer 实例查找插件信息
            var pluginsServerType = _pluginsServer.GetType();
            var pluginConnectorsProperty = pluginsServerType.GetProperty("PluginConnectors");

            if (pluginConnectorsProperty != null)
            {
                var pluginConnectors = pluginConnectorsProperty.GetValue(_pluginsServer) as System.Collections.IList;
                if (pluginConnectors != null)
                {
                    foreach (var connector in pluginConnectors)
                    {
                        if (connector != null)
                        {
                            var connectorType = connector.GetType();
                            var pluginInfoProperty = connectorType.GetProperty("PluginInfo");
                            if (pluginInfoProperty != null)
                            {
                                var pluginInfo = pluginInfoProperty.GetValue(connector) as PluginInfo;
                                if (pluginInfo != null && pluginInfo.Name == pluginName)
                                {
                                    return pluginInfo;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 查找插件信息失败: {pluginName} - 异常: {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// 查找插件连接器
    /// </summary>
    private object? FindPluginConnector(PluginInfo pluginInfo)
    {
        try
        {
            var pluginsServerType = _pluginsServer.GetType();
            var findConnectorMethod = pluginsServerType.GetMethod("FindConnector", new[] { typeof(PluginInfo) });

            if (findConnectorMethod != null)
            {
                return findConnectorMethod.Invoke(_pluginsServer, new object[] { pluginInfo });
            }
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 查找插件连接器失败: {pluginInfo.Name} - 异常: {ex.Message}");
        }

        return null;
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
                    SendRequestToConnector(connector, request).ConfigureAwait(false);
                });

            // 构建参数列表
            var functionArgs = new List<Parameter>();
            for (int i = 0; i < callInfo.Parameters.Length; i++)
            {
                var paramType = callInfo.ParameterTypes[i];
                var paramValue = callInfo.Parameters[i];

                functionArgs.Add(new Parameter
                {
                    Name = $"param{i}",
                    Type = paramType.Name,
                    Value = paramValue?.ToString() ?? string.Empty,
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
    /// 向连接器发送请求
    /// </summary>
    private async Task SendRequestToConnector(object connector, Request request)
    {
        try
        {
            var connectorType = connector.GetType();
            var requestMethod = connectorType.GetMethod("Request", new[] { typeof(Request) });
            if (requestMethod != null)
            {
                var result = requestMethod.Invoke(connector, new object[] { request });
                if (result is Task task)
                {
                    await task;
                }
            }
        }
        catch (Exception ex)
        {
            _errorLogger($"[RealPluginManager] 向连接器发送请求失败 - 异常: {ex.Message}");
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
