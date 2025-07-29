using System.Text.Json;
using Kscript.CSharp.Parser;
using Kscript.CSharp.Parser.Core;

namespace Kscript.CSharp.Parser.Examples;

/// <summary>
/// 基础用法示例
/// </summary>
public static class BasicUsageExample
{
    /// <summary>
    /// 演示基本功能
    /// </summary>
    public static async Task RunExample()
    {
        Console.WriteLine("=== KitX.CSharp.Parser 基础用法示例 ===\n");

        try
        {
            // 1. 从现有的 example.json 文件加载插件清单
            Console.WriteLine("1. 加载插件清单...");
            var exampleJsonPath = Path.Combine(".", "example.json");
            if (!File.Exists(exampleJsonPath))
            {
                Console.WriteLine($"   警告: example.json 文件不存在于 {exampleJsonPath}");
                Console.WriteLine("   将使用内置示例数据");
                RunWithBuiltInData();
                return;
            }

            var assembly = await Parser.GenerateFromFileAsync(exampleJsonPath, "ExamplePluginAssembly");
            Console.WriteLine($"   ✓ 成功生成程序集: {assembly.FullName}");

            // 2. 获取生成的插件类型
            Console.WriteLine("\n2. 分析生成的插件类型...");
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

            // 3. 动态调用插件方法
            Console.WriteLine("\n3. 动态调用插件方法...");
            DynamicInvokeExample(assembly);

            // 4. 缓存统计
            Console.WriteLine("\n4. 缓存统计信息...");
            var stats = Parser.GetCacheStatistics();
            Console.WriteLine($"   {stats}");

            // 5. 演示缓存效果
            Console.WriteLine("\n5. 测试缓存效果...");
            var assembly2 = await Parser.GenerateFromFileAsync(exampleJsonPath, "ExamplePluginAssembly");
            Console.WriteLine($"   第二次生成是否使用缓存: {assembly == assembly2}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 示例执行失败: {ex.Message}");
            Console.WriteLine($"详细信息: {ex}");
        }
    }

    /// <summary>
    /// 使用内置数据运行示例
    /// </summary>
    private static void RunWithBuiltInData()
    {
        // 创建示例插件数据
        var plugins = new List<PluginInfo>
        {
            new PluginInfo
            {
                Name = "TestCalculator",
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
            }
        };

        Console.WriteLine("   使用内置示例数据...");
        var assembly = Parser.Generate(plugins, "BuiltInExampleAssembly");
        Console.WriteLine($"   ✓ 成功生成程序集: {assembly.FullName}");

        DynamicInvokeExample(assembly);
    }

    /// <summary>
    /// 动态调用示例
    /// </summary>
    private static void DynamicInvokeExample(Assembly assembly)
    {
        var pluginTypes = Parser.GetPluginTypes(assembly);

        foreach (var type in pluginTypes)
        {
            Console.WriteLine($"\n   调用插件: {type.Name}");
            var methods = Parser.GetPluginMethods(type);

            foreach (var method in methods)
            {
                try
                {
                    object? result = null;
                    var parameters = method.GetParameters();

                    // 根据方法名和参数生成测试数据
                    object[] args = GenerateTestArguments(method.Name, parameters);

                    Console.WriteLine($"     调用 {method.Name}({string.Join(", ", args)})");

                    // 动态调用方法
                    result = method.Invoke(null, args);

                    Console.WriteLine($"     → 结果: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"     → 调用失败: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 生成测试参数
    /// </summary>
    private static object[] GenerateTestArguments(string methodName, ParameterInfo[] parameters)
    {
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var param = parameters[i];

            // 根据参数类型和方法名生成合适的测试值
            args[i] = param.ParameterType.Name switch
            {
                "Int32" => methodName switch
                {
                    "Add" => i == 0 ? 10 : 20,
                    "Divide" when param.Name == "decimals" => 2,
                    _ => i + 1
                },
                "Double" => methodName switch
                {
                    "Divide" => i == 0 ? 10.0 : 3.0,
                    "Multiply" => i == 0 ? 2.5 : 4.0,
                    _ => (i + 1) * 1.5
                },
                "String" => methodName switch
                {
                    "Reverse" => "Hello",
                    "ToUpper" => "world",
                    _ => $"test{i}"
                },
                _ => GetDefaultValue(param.ParameterType)
            };
        }

        return args;
    }

    /// <summary>
    /// 获取类型默认值
    /// </summary>
    private static object GetDefaultValue(Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type)!;

        return type == typeof(string) ? "default" : null!;
    }

    /// <summary>
    /// 演示高级功能
    /// </summary>
    public static void RunAdvancedExample()
    {
        Console.WriteLine("\n=== 高级功能演示 ===\n");

        try
        {
            // 1. 注册自定义类型
            Console.WriteLine("1. 注册自定义类型映射...");
            Parser.RegisterCustomType("CustomType", typeof(DateTime));
            Console.WriteLine("   ✓ 已注册 CustomType -> DateTime");

            // 2. 清除缓存
            Console.WriteLine("\n2. 清除所有缓存...");
            Parser.ClearCache();
            Console.WriteLine("   ✓ 缓存已清除");

            // 3. 设置自定义插件管理器
            Console.WriteLine("\n3. 设置自定义插件管理器...");
            Parser.SetDefaultPluginManager(new MockPluginManager());
            Console.WriteLine("   ✓ 已设置默认插件管理器");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 高级功能演示失败: {ex.Message}");
        }
    }
}
