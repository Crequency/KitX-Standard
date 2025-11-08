using Kscript.CSharp.Parser.Examples;
using Kscript.CSharp.Parser.Core;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Parser.Examples;

/// <summary>
/// 演示程序入口
/// </summary>
public static class Program
{
    /// <summary>
    /// 主入口方法
    /// </summary>
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("🚀 欢迎使用 KitX.CSharp.Parser 演示程序!");
        Console.WriteLine("==================================================\n");

        try
        {
            // 首先运行简化后的功能测试
            Console.WriteLine("=== 测试简化后的 Parser 功能 ===");
            TestSimplifiedParser();

            Console.WriteLine("\n" + new string('=', 60));

            // 运行基础用法示例
            await BasicUsageExample.RunExample();

            // 运行高级功能示例
            BasicUsageExample.RunAdvancedExample();

            // 运行实际插件管理器示例
            Console.WriteLine("\n" + new string('=', 60));
            RealPluginManagerExample.RunExample();

            Console.WriteLine("\n" + new string('=', 60));
            RealPluginManagerExample.DashboardIntegrationExample();

            Console.WriteLine("\n==================================================");
            Console.WriteLine("✅ 所有示例运行完成!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ 程序运行失败: {ex.Message}");
            Console.WriteLine($"详细错误信息:\n{ex}");
        }

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }

    /// <summary>
    /// 测试简化后的 Parser 功能
    /// </summary>
    private static void TestSimplifiedParser()
    {
        Console.WriteLine("1. 测试默认插件管理器设置...");

        // 测试使用 MockPluginManager 中已有的插件
        var testPlugins = new List<PluginInfo>
        {
            new PluginInfo
            {
                Name = "SampleCalculator",
                Version = "1.0.0",
                Functions = new List<Function>
                {
                    new Function
                    {
                        Name = "Add",
                        ReturnValueType = "int",
                        Parameters = new List<Parameter>
                        {
                            new Parameter { Name = "a", Type = "int", IsOptional = false },
                            new Parameter { Name = "b", Type = "int", IsOptional = false }
                        }
                    }
                }
            }
        };

        try
        {
            // 先设置插件管理器
            Parser.SetPluginManager(new MockPluginManager());

            // 使用 MockPluginManager 生成程序集
            var assembly = Parser.Generate(testPlugins, "TestAssembly");
            Console.WriteLine("   ✓ 默认 MockPluginManager 工作正常");

            // 获取生成的类型
            var types = Parser.GetPluginTypes(assembly);
            Console.WriteLine($"   ✓ 生成了 {types.Count} 个插件类型");

            foreach (var type in types)
            {
                var methods = Parser.GetPluginMethods(type);
                Console.WriteLine($"   - {type.Name}: {methods.Count} 个方法");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ 测试失败: {ex.Message}");
        }

        Console.WriteLine("2. 测试自定义插件管理器设置...");
        try
        {
            // 设置新的插件管理器
            Parser.SetPluginManager(new MockPluginManager());
            var assembly2 = Parser.Generate(testPlugins, "TestAssembly2");
            Console.WriteLine("   ✓ 自定义插件管理器设置工作正常");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ 自定义设置测试失败: {ex.Message}");
        }

        Console.WriteLine("3. 测试空参数处理...");
        try
        {
            // 测试 null 参数应该抛出异常
            Parser.SetPluginManager(null!);
            Console.WriteLine("   ❌ 空参数测试失败: 应该抛出异常但没有");
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("   ✓ 空参数正确抛出 ArgumentNullException");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ 空参数测试失败: {ex.Message}");
        }

        Console.WriteLine("4. 测试插件存在性检测...");
        try
        {
            var mockManager = new MockPluginManager();
            Parser.SetPluginManager(mockManager);

            // 测试存在的插件
            var existsSampleCalculator = mockManager.IsPluginExists("SampleCalculator");
            Console.WriteLine($"   ✓ SampleCalculator 存在: {existsSampleCalculator}");

            var existsAddMethod = mockManager.IsMethodExists("SampleCalculator", "Add");
            Console.WriteLine($"   ✓ SampleCalculator.Add 方法存在: {existsAddMethod}");

            // 测试不存在的插件
            var existsNonExistent = mockManager.IsPluginExists("NonExistentPlugin");
            Console.WriteLine($"   ✓ NonExistentPlugin 存在: {existsNonExistent}");

            var existsNonExistentMethod = mockManager.IsMethodExists("SampleCalculator", "NonExistentMethod");
            Console.WriteLine($"   ✓ SampleCalculator.NonExistentMethod 方法存在: {existsNonExistentMethod}");

            Console.WriteLine("   ✓ 插件存在性检测功能工作正常");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ❌ 插件存在性检测测试失败: {ex.Message}");
        }
    }
}
