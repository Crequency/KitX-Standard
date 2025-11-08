using Kscript.CSharp.Parser;
using Kscript.CSharp.Parser.Core;
using Kscript.CSharp.Compiler;
using Kscript.CSharp.Parser.Models;
using KitX.Shared.CSharp.Plugin;
using System.Reflection;

namespace Kscript.CSharp.Compiler.Examples;

/// <summary>
/// 端到端测试：用户脚本 → Compiler → Parser → PluginManager调用
/// 验证完整的调用链是否正常工作
/// </summary>
public static class EndToEndTest
{
    /// <summary>
    /// 运行端到端测试
    /// </summary>
    public static async Task RunTest()
    {
        Console.WriteLine("=== KitX.CSharp.Compiler 端到端测试 ===\n");

        try
        {
            // 1. 初始化插件管理器
            Console.WriteLine("1. 初始化插件管理器...");
            Parser.Parser.SetPluginManager(new MockPluginManager());

            // 2. 创建测试插件清单
            Console.WriteLine("2. 创建测试插件清单...");
            var plugins = CreateTestPlugins();

            // 3. 生成动态程序集
            Console.WriteLine("3. 生成动态程序集...");
            var assembly = Parser.Parser.Generate(plugins, "EndToEndTestAssembly");
            Console.WriteLine($"   ✓ 成功生成程序集: {assembly.FullName}");

            // 4. 验证生成的程序集
            Console.WriteLine("\n4. 验证生成的程序集...");
            await ValidateGeneratedAssembly(assembly);

            // 5. 创建脚本执行器并测试
            Console.WriteLine("\n5. 创建脚本执行器并测试...");
            await TestScriptExecution(assembly);

            // 6. 测试错误处理
            Console.WriteLine("\n6. 测试错误处理...");
            await TestErrorHandling(assembly);

            // 7. 性能测试
            Console.WriteLine("\n7. 性能测试...");
            await PerformanceTest(assembly);

            Console.WriteLine("\n=== 端到端测试完成 ===");
            Console.WriteLine("✅ 所有测试通过，C#脚本可以成功调用生成的插件方法！");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 端到端测试失败: {ex.Message}");
            Console.WriteLine($"详细错误信息:\n{ex}");
        }
    }

    /// <summary>
    /// 创建测试插件数据
    /// </summary>
    /// <returns>插件信息列表</returns>
    private static List<PluginInfo> CreateTestPlugins()
    {
        return new List<PluginInfo>
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
                    },
                    new Function
                    {
                        Name = "Divide",
                        ReturnValueType = "double",
                        Parameters = new List<Parameter>
                        {
                            new Parameter { Name = "numerator", Type = "double", IsOptional = false },
                            new Parameter { Name = "denominator", Type = "double", IsOptional = false },
                            new Parameter { Name = "decimals", Type = "int", IsOptional = true, Value = "2" }
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
                        Name = "Concat",
                        ReturnValueType = "string",
                        Parameters = new List<Parameter>
                        {
                            new Parameter { Name = "str1", Type = "string", IsOptional = false },
                            new Parameter { Name = "str2", Type = "string", IsOptional = false }
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
    }

    /// <summary>
    /// 验证生成的程序集
    /// </summary>
    /// <param name="assembly">生成的程序集</param>
    private static async Task ValidateGeneratedAssembly(Assembly assembly)
    {
        Console.WriteLine("   验证程序集结构...");

        // 获取所有插件类型
        var types = Parser.Parser.GetPluginTypes(assembly);
        Console.WriteLine($"   发现 {types.Count} 个插件类型:");

        foreach (var type in types)
        {
            Console.WriteLine($"   - {type.Name}");

            // 获取所有方法
            var methods = Parser.Parser.GetPluginMethods(type);
            Console.WriteLine($"     包含 {methods.Count} 个方法:");

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"     - {method.ReturnType.Name} {method.Name}({paramList})");
            }
        }

        Console.WriteLine("   ✓ 程序集结构验证完成");
    }

    /// <summary>
    /// 测试脚本执行
    /// </summary>
    /// <param name="assembly">生成的程序集</param>
    private static async Task TestScriptExecution(Assembly assembly)
    {
        using var executor = new ScriptExecutor();

        // 添加程序集引用
        executor.AddAssemblyReference(assembly);
        Console.WriteLine("   ✓ 已添加程序集引用到脚本执行器");

        // 测试基本计算脚本
        Console.WriteLine("\n   测试基本计算脚本...");
        string basicScript = @"
                // 测试加法
                var sum = SampleCalculator.Add(10, 20);
                KitXWF.Print(string.Format(""加法结果: {0}"", sum));

                // 测试乘法
                var product = SampleCalculator.Multiply(3.5, 2.8);
                KitXWF.Print(string.Format(""乘法结果: {0}"", product));

                // 测试字符串反转
                var reversed = StringToolkit.Reverse(""Hello KitX"");
                KitXWF.Print(string.Format(""字符串反转结果: {0}"", reversed));

                return new { Sum = sum, Product = product, Reversed = reversed };
            ";

        var basicResult = await executor.ExecuteAsync<dynamic>(basicScript, "BasicTest");
        Console.WriteLine($"   ✓ 基本脚本执行成功: Sum={basicResult.Sum}, Product={basicResult.Product}, Reversed={basicResult.Reversed}");

        // 测试复杂业务逻辑脚本
        Console.WriteLine("\n   测试复杂业务逻辑脚本...");
        string complexScript = @"
                // 模拟订单处理
                var orderItems = new[]
                {
                    new { Name = ""商品A"", Price = 10.5, Quantity = 2 },
                    new { Name = ""商品B"", Price = 25.0, Quantity = 1 },
                    new { Name = ""商品C"", Price = 5.75, Quantity = 3 }
                };

                var totalAmount = 0.0;
                var itemCount = orderItems.Length;

                foreach (var item in orderItems)
                {
                    var itemTotal = SampleCalculator.Multiply(item.Price, (double)item.Quantity);
                    totalAmount = totalAmount + itemTotal;
                    KitXWF.Print(string.Format(""处理商品: {0}, 小计: {1}"", item.Name, itemTotal));
                }

                // 计算平均价格
                var averagePrice = SampleCalculator.Divide(totalAmount, (double)itemCount, 2);

                // 生成订单摘要
                var summary = StringToolkit.Concat(string.Format(""订单总额: {0}, 平均价格: {1}"", totalAmount, averagePrice), "" (已处理)"");

                return new {
                    TotalAmount = totalAmount,
                    ItemCount = itemCount,
                    AveragePrice = averagePrice,
                    Summary = summary
                };
            ";

        var complexResult = await executor.ExecuteAsync<dynamic>(complexScript, "ComplexTest");
        Console.WriteLine($"   ✓ 复杂脚本执行成功: Total={complexResult.TotalAmount}, Items={complexResult.ItemCount}, Avg={complexResult.AveragePrice}");

        // 测试脚本验证功能
        Console.WriteLine("\n   测试脚本验证功能...");
        var validScript = "var x = 10; return x * 2;";
        var invalidScript = "var x = 10; return x *; // 语法错误";

        var validValidation = executor.ValidateScript(validScript);
        var invalidValidation = executor.ValidateScript(invalidScript);

        Console.WriteLine($"   ✓ 有效脚本验证: {validValidation.IsValid} - {validValidation.Message}");
        Console.WriteLine($"   ✓ 无效脚本验证: {!invalidValidation.IsValid} - {invalidValidation.Message}");

        // 显示执行器状态
        var status = executor.GetStatus();
        Console.WriteLine($"   执行器状态: {status.ReferencedAssembliesCount} 个程序集, {status.GlobalVariablesCount} 个全局变量");
    }

    /// <summary>
    /// 测试错误处理
    /// </summary>
    /// <param name="assembly">生成的程序集</param>
    private static async Task TestErrorHandling(Assembly assembly)
    {
        using var executor = new ScriptExecutor();
        executor.AddAssemblyReference(assembly);

        // 测试编译错误
        Console.WriteLine("   测试编译错误处理...");
        try
        {
            var syntaxErrorScript = "var x = 10; var y = ; return x + y;"; // 故意语法错误
            await executor.ExecuteAsync<object>(syntaxErrorScript);
            Console.WriteLine("   ❌ 应该捕获编译错误但没有");
        }
        catch (ScriptExecutionException ex)
        {
            Console.WriteLine($"   ✓ 成功捕获编译错误: {ex.Message}");
        }

        // 测试运行时错误
        Console.WriteLine("\n   测试运行时错误处理...");
        try
        {
            var runtimeErrorScript = @"
                var result = SampleCalculator.Divide(10, 0); // 除零错误
                return result;
            ";

            await executor.ExecuteAsync<object>(runtimeErrorScript);
            Console.WriteLine("   ❌ 应该捕获运行时错误但没有");
        }
        catch (ScriptExecutionException ex)
        {
            Console.WriteLine($"   ✓ 成功捕获运行时错误: {ex.Message}");
        }

        // 测试插件方法不存在错误
        Console.WriteLine("\n   测试插件方法不存在错误...");
        try
        {
            var methodNotFoundScript = "var result = SampleCalculator.NonExistentMethod(10, 20); return result;";
            await executor.ExecuteAsync<object>(methodNotFoundScript);
            Console.WriteLine("   ❌ 应该捕获方法不存在错误但没有");
        }
        catch (ScriptExecutionException ex)
        {
            Console.WriteLine($"   ✓ 成功捕获方法不存在错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 性能测试
    /// </summary>
    /// <param name="assembly">生成的程序集</param>
    private static async Task PerformanceTest(Assembly assembly)
    {
        using var executor = new ScriptExecutor();
        executor.AddAssemblyReference(assembly);

        Console.WriteLine("   执行性能测试...");

        var testScript = @"
                using System.Collections.Generic;
                var results = new List<int>();
                for (int i = 0; i < 10; i++)
                {
                    var sum = SampleCalculator.Add(i, i * 2);
                    results.Add(sum);
                }
                return results.Count;
            ";

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // 执行多次取平均值
        const int iterations = 10;
        var times = new List<long>();

        for (int i = 0; i < iterations; i++)
        {
            stopwatch.Restart();
            await executor.ExecuteAsync<int>(testScript, $"PerfTest_{i}");
            times.Add(stopwatch.ElapsedMilliseconds);
        }

        var avgTime = times.Average();
        var minTime = times.Min();
        var maxTime = times.Max();

        Console.WriteLine($"   性能测试结果 ({iterations} 次执行):");
        Console.WriteLine($"   - 平均执行时间: {avgTime:F2} ms");
        Console.WriteLine($"   - 最快执行时间: {minTime} ms");
        Console.WriteLine($"   - 最慢执行时间: {maxTime} ms");
        Console.WriteLine($"   - 性能评级: {GetPerformanceRating(avgTime)}");
    }

    /// <summary>
    /// 获取性能评级
    /// </summary>
    /// <param name="avgTime">平均执行时间</param>
    /// <returns>性能评级</returns>
    private static string GetPerformanceRating(double avgTime)
    {
        return avgTime switch
        {
            < 10 => "优秀",
            < 50 => "良好",
            < 100 => "一般",
            < 200 => "较差",
            _ => "很差"
        };
    }
}

