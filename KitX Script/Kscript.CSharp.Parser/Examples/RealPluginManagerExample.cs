using Kscript.CSharp.Parser;
using Kscript.CSharp.Parser.Core;
using System.Text.Json;

namespace Kscript.CSharp.Parser.Examples;

/// <summary>
/// 实际插件管理器使用示例
/// </summary>
public static class RealPluginManagerExample
{
    /// <summary>
    /// 演示如何使用 RealPluginManager
    /// </summary>
    public static void RunExample()
    {
        Console.WriteLine("=== KitX.CSharp.Parser 实际插件管理器使用示例 ===\n");

        try
        {
            // 1. 设置插件管理器工厂函数
            Console.WriteLine("1. 设置插件管理器工厂函数...");

            // 这里应该传入实际的 PluginsServer 实例和日志记录器
            // 由于这是一个示例，我们使用 null 作为占位符
            Parser.SetPluginManagerFactory(() =>
            {
                // 在实际使用中，这里应该传入真实的 PluginsServer 实例和日志记录器
                // 例如：
                // var pluginsServer = KitX.Dashboard.Network.PluginsNetwork.PluginsServer.Instance;
                // var logger = Serilog.Log.Logger;
                // return new RealPluginManager(pluginsServer, message => logger.Information(message));

                Console.WriteLine("   创建 RealPluginManager 实例...");
                return null; // 示例中返回 null，实际使用时返回真实实例
            });

            Console.WriteLine("   ✓ 插件管理器工厂函数已设置");

            // 2. 创建示例插件数据
            Console.WriteLine("\n2. 创建示例插件数据...");
            var examplePlugins = new List<PluginInfo>
            {
                new PluginInfo
                {
                    Name = "TestPlugin",
                    Version = "1.0.0",
                    Functions = new List<Function>
                    {
                        new Function
                        {
                            Name = "TestMethod",
                            ReturnValueType = "string",
                            Parameters = new List<Parameter>
                            {
                                new Parameter { Name = "input", Type = "string", IsOptional = false }
                            }
                        }
                    }
                }
            };

            Console.WriteLine("   ✓ 示例插件数据已创建");

            // 3. 生成动态程序集
            Console.WriteLine("\n3. 生成动态程序集...");
            var assembly = Parser.Generate(examplePlugins, "RealPluginManagerExampleAssembly");
            Console.WriteLine($"   ✓ 成功生成程序集: {assembly.FullName}");

            // 4. 获取生成的插件类型
            Console.WriteLine("\n4. 分析生成的插件类型...");
            var pluginTypes = Parser.GetPluginTypes(assembly);
            Console.WriteLine($"   发现 {pluginTypes.Count} 个插件类:");

            foreach (var type in pluginTypes)
            {
                Console.WriteLine($"   - {type.Name}");
                var methods = Parser.GetPluginMethods(type);
                foreach (var method in methods)
                {
                    var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($"     └── {method.ReturnType.Name} {method.Name}({parameters})");
                }
            }

            Console.WriteLine("\n=== 示例完成 ===");
            Console.WriteLine("\n注意：在实际使用中，您需要：");
            Console.WriteLine("1. 确保 KitX Dashboard 正在运行");
            Console.WriteLine("2. 传入真实的 PluginsServer.Instance");
            Console.WriteLine("3. 传入真实的日志记录器实例");
            Console.WriteLine("4. 确保插件已正确连接到 Dashboard");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 示例执行失败: {ex.Message}");
            Console.WriteLine($"详细信息: {ex}");
        }
    }

    /// <summary>
    /// 演示如何在 KitX Dashboard 中集成
    /// </summary>
    public static void DashboardIntegrationExample()
    {
        Console.WriteLine("=== KitX Dashboard 集成示例 ===\n");

        Console.WriteLine("在 KitX Dashboard 中使用 RealPluginManager 的步骤：\n");

        Console.WriteLine("1. 在 Dashboard 启动时设置插件管理器工厂：");
        Console.WriteLine("```csharp");
        Console.WriteLine("// 在 Dashboard 的初始化代码中");
        Console.WriteLine("Parser.SetPluginManagerFactory(() => new RealPluginManager(");
        Console.WriteLine("    KitX.Dashboard.Network.PluginsNetwork.PluginsServer.Instance,");
        Console.WriteLine("    message => Log.Information(message)");
        Console.WriteLine("));");
        Console.WriteLine("```");

        Console.WriteLine("\n2. 生成插件程序集：");
        Console.WriteLine("```csharp");
        Console.WriteLine("var assembly = Parser.Generate(plugins, \"DashboardPluginAssembly\");");
        Console.WriteLine("```");

        Console.WriteLine("\n3. 在脚本中使用生成的 API：");
        Console.WriteLine("```csharp");
        Console.WriteLine("// 现在可以在脚本中直接调用");
        Console.WriteLine("var result = TestPlugin.TestMethod(\"Hello World\");");
        Console.WriteLine("```");

        Console.WriteLine("\n这样，脚本中的插件调用将通过真实的 KitX Dashboard 插件系统执行！");
    }
}
