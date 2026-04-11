﻿using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Plugin;
using KitX.Shared.CSharp.WebCommand;
using Kscript.CSharp.Interfaces;

namespace Kscript.CSharp.Services
{
    public class Plugin : IPlugin
    {
        private readonly Connector _connector;

        public PluginInfo Info { get; }
        public DeviceInfo AssociatedDevice { get; }

        public Plugin(PluginInfo info, DeviceInfo deviceInfo, Connector? connector = null)
        {
            Info = info;
            AssociatedDevice = deviceInfo;
            _connector = connector ?? Connector.Instance;
        }

        public async Task<IFunction?> RequestFunction(string Name)
        {
            var functions = await GetFunctionList();
            var function = functions.FirstOrDefault(f =>
                f.Info.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            return function;
        }

        public async Task<IEnumerable<IFunction>> GetFunctionList()
        {
            var functions = Info.Functions.Select(f => 
                new Function(f, Info, AssociatedDevice, _connector) as IFunction);
            return await Task.FromResult(functions);
        }

        public async Task<IFunction?> GetFunctionByType(string type)
        {
            var functions = await GetFunctionList();
            return functions.FirstOrDefault(f => 
                f.Info.ReturnValueType.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<object> ExecuteFunction(string functionName, params string[] parameters)
        {
            _connector.Request()
                .UpdateCommand(cmd =>
                {
                    cmd.FunctionName = functionName;
                    cmd.FunctionArgs = parameters.Select(p => new Parameter { Value = p }).ToList();
                    return cmd;
                })
                .UpdateRequest(req =>
                {
                    req.Target = AssociatedDevice.Device;
                    return req;
                })
                .Send();

            // 处理响应
            var tcs = new TaskCompletionSource<object>();

            void OnResponse(Request response)
            {
                try
                {
                    response.Match(
                        response.GetContent(content =>
                        {
                            var returnType = GetFunctionReturnType(functionName);
                            var result = ParseFunctionResponse(content, returnType);
                            tcs.SetResult(result!);
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
                                var returnType = GetFunctionReturnType(functionName);
                                var result = ParseFunctionResponse(resultJson, returnType);
                                tcs.SetResult(result!);
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

        private Type GetFunctionReturnType(string functionName)
        {
            var function = Info.Functions.FirstOrDefault(f => 
                f.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase));
            
            if (function.Equals(default(Function)))
                throw new InvalidOperationException($"Function '{functionName}' not found in plugin '{Info.Name}'");

            // Try to get the type from the assembly first
            var type = Type.GetType(function.ReturnValueType);
            if (type != null)
                return type;

            // Handle common type names that might not be fully qualified
            switch (function.ReturnValueType.ToLower())
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

        private object? ParseFunctionResponse(string content, Type returnType)
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

        public bool HasFunction(string Name)
        {
            return Info.Functions.Any(f => 
                f.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
