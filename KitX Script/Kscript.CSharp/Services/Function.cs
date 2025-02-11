using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Function : IFunction
    {
        private readonly Connector _connector = Connector.Instance;
        private readonly DeviceInfo _deviceInfo;

        public KitX.Shared.CSharp.Plugin.Function Info { get; }
        public PluginInfo AssociatedPlugin { get; }

        public Function(KitX.Shared.CSharp.Plugin.Function info, PluginInfo pluginInfo, DeviceInfo deviceInfo)
        {
            Info = info;
            AssociatedPlugin = pluginInfo;
            _deviceInfo = deviceInfo;
        }

        public async Task<object> Invoke(params string[] parameters)
        {
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = Info.Name;
                    cmd.FunctionArgs = parameters.Select(p => new Parameter { Value = p }).ToList();
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Target = _deviceInfo.Device;
                    return req;
                })
                .Send();

            var tcs = new TaskCompletionSource<object>();

            void OnResponse(Request response)
            {
                try
                {
                    response.Match(
                        response.GetContent(content =>
                        {
                            if (string.IsNullOrEmpty(content))
                            {
                                tcs.SetResult(null);
                                return content;
                            }

                            // Parse response based on return type
                            var returnType = GetReturnType();
                            var result = ParseResponse(content, returnType);
                            tcs.SetResult(result);
                            return content;
                        }),
                        matchCommand: content =>
                        {
                            if (content.StartsWith("Error:"))
                            {
                                var error = content.Substring("Error:".Length);
                                tcs.SetException(new InvalidOperationException($"Function execution failed: {error}"));
                            }
                            else if (content.StartsWith("Result:"))
                            {
                                var resultJson = content.Substring("Result:".Length);
                                var returnType = GetReturnType();
                                var result = ParseResponse(resultJson, returnType);
                                tcs.SetResult(result);
                            }
                        }
                    );
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            _connector.Request()
                .SetSender(OnResponse);

            return await tcs.Task;
        }

        public Type GetReturnType()
        {
            // Try to get the type from the assembly first
            var type = Type.GetType(Info.ReturnValueType);
            if (type != null)
                return type;

            // Handle common type names that might not be fully qualified
            switch (Info.ReturnValueType.ToLower())
            {
                case "string":
                    return typeof(string);
                case "int":
                case "int32":
                    return typeof(int);
                case "long":
                case "int64":
                    return typeof(long);
                case "float":
                case "single":
                    return typeof(float);
                case "double":
                    return typeof(double);
                case "bool":
                case "boolean":
                    return typeof(bool);
                case "void":
                    return typeof(void);
                default:
                    return typeof(object);
            }
        }

        private object ParseResponse(string content, Type returnType)
        {
            if (string.IsNullOrEmpty(content))
                return null;

            try
            {
                if (returnType == typeof(void))
                    return null;

                // Handle primitive types
                if (returnType == typeof(string))
                    return content;
                if (returnType == typeof(int))
                    return int.Parse(content);
                if (returnType == typeof(long))
                    return long.Parse(content);
                if (returnType == typeof(float))
                    return float.Parse(content);
                if (returnType == typeof(double))
                    return double.Parse(content);
                if (returnType == typeof(bool))
                    return bool.Parse(content);

                // For complex types, deserialize from JSON
                return System.Text.Json.JsonSerializer.Deserialize(content, returnType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse response as {returnType.Name}: {ex.Message}", ex);
            }
        }
    }
}
