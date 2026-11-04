using System;
using System.Collections.Generic;
using System.Text.Json;
using KitX.Shared.CSharp.WebCommand;
using KitX.Shared.CSharp.WebCommand.Infos;

namespace KitX.Contract.CSharp;

/// <summary>
/// 插件触发器辅助工具。插件通过此类发送触发信号到 Dashboard。
/// 触发器是纯信号，不携带业务数据。工作流被触发后通过调用插件函数获取数据。
/// </summary>
public static class TriggerHelper
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = false,
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// 触发一个信号事件
    /// </summary>
    /// <param name="sendAction">SetSendCommandAction 提供的发送回调</param>
    /// <param name="triggerName">触发器名称</param>
    public static void FireTrigger(Action<Request>? sendAction, string triggerName)
    {
        if (sendAction is null) return;

        var request = new Request
        {
            Type = RequestTypes.Command,
            Version = RequestVersions.V1,
            Content = JsonSerializer.Serialize(new Command
            {
                Request = CommandRequestInfo.TriggerFired,
                Tags = new Dictionary<string, string> { { "TriggerName", triggerName } }
            }, _options)
        };

        sendAction.Invoke(request);
    }
}
