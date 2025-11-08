using Kscript.CSharp.Parser;
using Kscript.CSharp.Parser.Core;
using Kscript.CSharp.Compiler;
using Kscript.CSharp.Parser.Models;
using System.Text.Json;
using System.Reflection;
using KitX.Shared.CSharp.Plugin;

namespace Kscript.CSharp.Compiler.Examples;

/// <summary>
/// C#脚本执行示例
/// </summary>
public static class ScriptExecutionExample
{
    /// <summary>
    /// 运行脚本执行示例
    /// </summary>
    public static async Task RunExample()
    {
        Console.WriteLine("=== Kscript.CSharp.Compiler 脚本执行示例 ===\n");

        try
        {
            // 1. 初始化插件管理器
            Console.WriteLine("1. 初始化插件管理器...");
            Parser.Parser.SetPluginManager(new MockPluginManager());

            // 2. 创建示例插件数据
            var plugins = new List<PluginInfo>
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
                        },
                        new Function
                        {
                            Name = "Multiply",
                            ReturnValueType = "double",
                            Parameters = new List<Parameter>
                            {
                                new Parameter { Name = "x", Type = "double", IsOptional = false },
                                new Parameter { Name = "y", Type = "double", IsOptional = false }
                            }
                        }
                    }
                },
                new PluginInfo
                {
                    Name = "StringToolkit",
                    Version = "1.0.0",
                    Functions = new List<Function>
                    {
                        new Function
                        {
                            Name = "Reverse",
                            ReturnValueType = "string",
                            Parameters = new List<Parameter>
                            {
                                new Parameter { Name = "text", Type = "string", IsOptional = false }
                            }
                        },
                        new Function
                        {
                            Name = "ToUpper",
                            ReturnValueType = "string",
                            Parameters = new List<Parameter>
                            {
                                new Parameter { Name = "text", Type = "string", IsOptional = false }
                            }
                        }
                    }
                },
                new PluginInfo
                {
                    Name = "KitXWF",
                    Version = "1.0.0",
                    Functions = new List<Function>
                    {
                        new Function
                        {
                            Name = "Print",
                            ReturnValueType = "void",
                            Parameters = new List<Parameter>
                            {
                                new Parameter { Name = "message", Type = "string", IsOptional = false }
                            }
                        }
                    }
                }
            };

            // 3. 生成动态程序集
            Console.WriteLine("2. 生成动态程序集...");
            var assembly = Parser.Parser.Generate(plugins, "ScriptExampleAssembly");
            Console.WriteLine($"   ✓ 成功生成程序集: {assembly.FullName}");

            // 4. 创建脚本执行器
            Console.WriteLine("\n3. 创建脚本执行器...");
            using var executor = new ScriptExecutor();

            // 5. 添加程序集引用
            executor.AddAssemblyReference(assembly);
            Console.WriteLine("   ✓ 已添加程序集引用");

            // 6. 设置全局变量
            executor.SetGlobalVariable("PI", Math.PI);
            executor.SetGlobalVariable("AppName", "KitX Script Engine");
            Console.WriteLine("   ✓ 已设置全局变量");

            // 7. 执行简单计算脚本
            Console.WriteLine("\n4. 执行简单计算脚本...");
            string simpleScript = @"
                var sum = SampleCalculator.Add(15, 25);
                var product = SampleCalculator.Multiply(3.5, 2.8);
                return new { Sum = sum, Product = product };
            ";

            var simpleResult = await executor.ExecuteAsync<dynamic>(simpleScript);
            Console.WriteLine($"   计算结果: Sum = {simpleResult.Sum}, Product = {simpleResult.Product}");

            // 8. 执行字符串处理脚本
            Console.WriteLine("\n5. 执行字符串处理脚本...");
            string stringScript = @"
                var reversed = StringToolkit.Reverse(""Hello World"");
                var upper = StringToolkit.ToUpper(""kitx script"");
                return new { Original = ""Hello World"", Reversed = reversed, Upper = upper };
            ";

            var stringResult = await executor.ExecuteAsync<dynamic>(stringScript);
            Console.WriteLine($"   字符串处理结果:");
            Console.WriteLine($"     Original: {stringResult.Original}");
            Console.WriteLine($"     Reversed: {stringResult.Reversed}");
            Console.WriteLine($"     Upper: {stringResult.Upper}");

            // 9. 执行复杂业务逻辑脚本
            Console.WriteLine("\n6. 执行复杂业务逻辑脚本...");
            string complexScript = @"
                // 模拟一个简单的业务场景：计算订单总价
                var orderItems = new[]
                {
                    new { Name = ""商品A"", Price = 10.5, Quantity = 2 },
                    new { Name = ""商品B"", Price = 25.0, Quantity = 1 },
                    new { Name = ""商品C"", Price = 5.75, Quantity = 3 }
                };

                var total = 0.0;
                foreach (var item in orderItems)
                {
                    var itemTotal = SampleCalculator.Multiply(item.Price, item.Quantity);
                    total = total + itemTotal; // 直接使用double加法
                    KitXWF.Print(string.Format(""商品: {0}, 单价: {1}, 数量: {2}, 小计: {3}"", item.Name, item.Price, item.Quantity, itemTotal));
                }

                // 使用字符串处理生成订单摘要
                var summary = StringToolkit.ToUpper(string.Format(""订单总价: {0}"", total));
                return new { Total = total, Summary = summary, ItemCount = orderItems.Length };
            ";

            var complexResult = await executor.ExecuteAsync<dynamic>(complexScript);
            Console.WriteLine($"   复杂业务逻辑结果:");
            Console.WriteLine($"     订单总价: {complexResult.Total}");
            Console.WriteLine($"     订单摘要: {complexResult.Summary}");
            Console.WriteLine($"     商品数量: {complexResult.ItemCount}");

            // 10. 显示执行器状态
            Console.WriteLine("\n7. 脚本执行器状态:");
            var status = executor.GetStatus();
            Console.WriteLine($"   引用程序集数量: {status.ReferencedAssembliesCount}");
            Console.WriteLine($"   全局变量数量: {status.GlobalVariablesCount}");
            Console.WriteLine($"   命名空间数量: {status.UsingsCount}");
            Console.WriteLine($"   程序集列表: {string.Join("", "", status.AssemblyNames)}");

            // 11. 脚本验证示例
            Console.WriteLine("\n8. 脚本验证示例:");
            var validScript = "var x = 10; var y = 20; return x + y;";
            var invalidScript = "var x = 10; var y = ; return x + y;"; // 故意的语法错误

            var validResult = executor.ValidateScript(validScript);
            Console.WriteLine($"   有效脚本验证: {validResult.IsValid} - {validResult.Message}");

            var invalidResult = executor.ValidateScript(invalidScript);
            Console.WriteLine($"   无效脚本验证: {invalidResult.IsValid} - {invalidResult.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 示例执行失败: {ex.Message}");
            Console.WriteLine($"详细错误信息:\n{ex}");
        }

        Console.WriteLine("\n=== 脚本执行示例完成 ===");
    }

    /// <summary>
    /// 演示脚本安全性和错误处理
    /// </summary>
    public static async Task RunSecurityExample()
    {
        Console.WriteLine("=== 脚本安全性示例 ===\n");

        using var executor = new ScriptExecutor();

        // 1. 测试无限循环检测
        Console.WriteLine("1. 测试无限循环检测...");
        string infiniteLoopScript = @"
            while (true)
            {
                // 这会导致无限循环
            }
        ";

        try
        {
            // 设置超时机制（CSharpScript本身不支持，但可以通过其他方式实现）
            var task = executor.ExecuteAsync<object>(infiniteLoopScript);
            var timeoutTask = Task.Delay(5000); // 5秒超时

            var completedTask = await Task.WhenAny(task, timeoutTask);
            if (completedTask == timeoutTask)
            {
                Console.WriteLine("   ✓ 检测到潜在的超时/无限循环");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✓ 安全机制生效: {ex.Message}");
        }

        // 2. 测试恶意代码检测
        Console.WriteLine("\n2. 测试恶意代码检测...");
        string maliciousScript = @"
            // 尝试访问文件系统
            System.IO.File.WriteAllText(""malicious.txt"", ""This is malicious"");
            return ""File created"";
        ";

        try
        {
            var result = await executor.ExecuteAsync<object>(maliciousScript);
            Console.WriteLine("   ⚠️  警告: 脚本执行成功，可能存在安全风险");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✓ 安全机制阻止了恶意操作: {ex.Message}");
        }
    }
}
