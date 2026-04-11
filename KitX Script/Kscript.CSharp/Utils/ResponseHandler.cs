using System.Text.Json;
using KitX.Shared.CSharp.WebCommand;

namespace Kscript.CSharp.Utils
{
    /// <summary>
    /// 统一处理网络响应的工具类
    /// </summary>
    public static class ResponseHandler
    {
        /// <summary>
        /// 处理网络响应并返回指定类型的结果
        /// </summary>
        /// <typeparam name="T">响应结果的类型</typeparam>
        /// <param name="response">网络响应</param>
        /// <param name="contentHandler">处理响应内容的函数</param>
        /// <param name="commandHandler">处理命令响应的函数</param>
        /// <returns>处理后的结果</returns>
        public static async Task<T> HandleResponse<T>(
            Request response,
            Func<string, T> contentHandler,
            Func<string, T> commandHandler)
        {
            return await Task.Run(() =>
            {
                try
                {
                    T? result = default;
                    response.Match(
                        response.GetContent(content =>
                        {
                            result = contentHandler(content);
                            return content;
                        }),
                        matchCommand: command =>
                        {
                            result = commandHandler(command);
                        }
                    );
                    return result!;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Failed to handle response: {ex.Message}", ex);
                }
            });
        }

        /// <summary>
        /// 处理JSON响应并反序列化为指定类型
        /// </summary>
        public static async Task<T?> HandleJsonResponse<T>(
            Request response,
            JsonSerializerOptions? options = null)
        {
            return await HandleResponse<T?>(
                response,
                content => string.IsNullOrEmpty(content) ? default :
                    JsonSerializer.Deserialize<T>(content, options),
                command =>
                {
                    if (command.StartsWith("Error:"))
                    {
                        var error = command.Substring("Error:".Length);
                        throw new InvalidOperationException($"Command error: {error}");
                    }
                    return default;
                }
            );
        }

        /// <summary>
        /// 处理带前缀的命令响应
        /// </summary>
        public static async Task<T?> HandlePrefixedResponse<T>(
            Request response,
            string prefix,
            JsonSerializerOptions? options = null)
        {
            return await HandleResponse<T?>(
                response,
                content => string.IsNullOrEmpty(content) ? default :
                    JsonSerializer.Deserialize<T>(content, options),
                command =>
                {
                    if (command.StartsWith("Error:"))
                    {
                        var error = command.Substring("Error:".Length);
                        throw new InvalidOperationException($"Command error: {error}");
                    }
                    else if (command.StartsWith(prefix))
                    {
                        var json = command.Substring(prefix.Length);
                        return JsonSerializer.Deserialize<T>(json, options);
                    }
                    return default;
                }
            );
        }

        /// <summary>
        /// 处理简单的字符串响应
        /// </summary>
        public static async Task<string> HandleStringResponse(
            Request response,
            bool throwOnError = true)
        {
            return await HandleResponse<string>(
                response,
                content => content ?? string.Empty,
                command =>
                {
                    if (throwOnError && command.StartsWith("Error:"))
                    {
                        var error = command.Substring("Error:".Length);
                        throw new InvalidOperationException($"Command error: {error}");
                    }
                    return command;
                }
            );
        }
    }
}