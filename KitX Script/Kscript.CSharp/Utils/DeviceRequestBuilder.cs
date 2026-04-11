using System.Text.Json;
using KitX.Shared.CSharp.WebCommand;
using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Utils
{
    /// <summary>
    /// 设备请求构建器
    /// </summary>
    public class DeviceRequestBuilder
    {
        private readonly Connector _connector;
        private Command _command;
        private Request _request;

        public DeviceRequestBuilder(Connector connector)
        {
            _connector = connector;
            _command = new Command();
            _request = new Request
            {
                Type = RequestTypes.Command,
                Version = RequestVersions.V1
            };
        }

        /// <summary>
        /// 设置请求的函数名和参数
        /// </summary>
        public DeviceRequestBuilder WithFunction(string name, params object[] args)
        {
            _command.FunctionName = name;
            _command.FunctionArgs = args.Select(a => new Parameter 
            { 
                Value = JsonSerializer.Serialize(a),
                Type = a.GetType().Name
            }).ToList();
            return this;
        }

        /// <summary>
        /// 设置请求的目标设备
        /// </summary>
        public DeviceRequestBuilder WithTarget(DeviceLocator? target)
        {
            _request.Target = target;
            return this;
        }

        /// <summary>
        /// 设置请求的目标设备信息
        /// </summary>
        public DeviceRequestBuilder WithTargetDevice(DeviceInfo device)
        {
            return WithTarget(device.Device);
        }

        /// <summary>
        /// 执行请求并处理响应
        /// </summary>
        public async Task<T> ExecuteAsync<T>(Func<Request, Task<T>> responseHandler)
        {
            var tcs = new TaskCompletionSource<T>();

            void OnResponse(Request response)
            {
                try
                {
                    var result = responseHandler(response);
                    result.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            tcs.SetException(t.Exception ?? new Exception("Unknown error"));
                        else if (t.IsCanceled)
                            tcs.SetCanceled();
                        else
                            tcs.SetResult(t.Result);
                    });
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = _command.FunctionName;
                    cmd.FunctionArgs = _command.FunctionArgs;
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Type = _request.Type;
                    req.Version = _request.Version;
                    req.Target = _request.Target;
                    return req;
                })
                .SetSender(OnResponse)
                .Send();

            return await tcs.Task;
        }

        /// <summary>
        /// 重置构建器状态
        /// </summary>
        public DeviceRequestBuilder Reset()
        {
            _command = new Command();
            _request = new Request
            {
                Type = RequestTypes.Command,
                Version = RequestVersions.V1
            };
            return this;
        }

        /// <summary>
        /// 创建请求克隆
        /// </summary>
        public DeviceRequestBuilder Clone()
        {
            var clone = new DeviceRequestBuilder(_connector);
            clone._command = new Command
            {
                FunctionName = _command.FunctionName,
                FunctionArgs = _command.FunctionArgs?.ToList() ?? new()
            };
            clone._request = new Request
            {
                Type = _request.Type,
                Version = _request.Version,
                Target = _request.Target
            };
            return clone;
        }
    }
}