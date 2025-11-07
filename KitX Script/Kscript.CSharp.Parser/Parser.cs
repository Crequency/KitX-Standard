using System.Text.Json;
using Kscript.CSharp.Parser.CodeGen;
using Kscript.CSharp.Parser.Core;
using Kscript.CSharp.Parser.Exceptions;

namespace Kscript.CSharp.Parser;

/// <summary>
/// KitX.CSharp.Parser 主入口类
/// 将插件清单 JSON 即时编译成可直接调用的 C# 静态 API
/// </summary>
public static class Parser
{
    /// <summary>
    /// 默认插件管理器
    /// </summary>
    private static IPluginManager? _defaultPluginManager;

    /// <summary>
    /// 插件管理器工厂函数
    /// </summary>
    private static Func<IPluginManager>? _pluginManagerFactory;

    /// <summary>
    /// 设置默认插件管理器
    /// </summary>
    /// <param name="pluginManager">插件管理器实例</param>
    public static void SetDefaultPluginManager(IPluginManager pluginManager)
    {
        _defaultPluginManager = pluginManager;
    }

    /// <summary>
    /// 设置插件管理器工厂函数
    /// </summary>
    /// <param name="factory">插件管理器工厂函数</param>
    public static void SetPluginManagerFactory(Func<IPluginManager> factory)
    {
        _pluginManagerFactory = factory;
    }

    /// <summary>
    /// 从插件信息列表生成动态程序集
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例（可选，默认使用 MockPluginManager）</param>
    /// <param name="useCache">是否使用缓存</param>
    /// <returns>生成的动态程序集</returns>
    public static Assembly Generate(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly",
        IPluginManager? pluginManager = null, bool useCache = true)
    {
        if (plugins == null || plugins.Count == 0)
        {
            throw new ArgumentException("插件列表不能为空", nameof(plugins));
        }

        try
        {
            var manager = pluginManager ?? _defaultPluginManager ?? _pluginManagerFactory?.Invoke() ?? new MockPluginManager();

            // 设置静态插件管理器实例
            MethodEmitter.SetStaticPluginManager(manager);

            if (useCache)
            {
                return AssemblyCache.GetOrCreateAssembly(plugins, assemblyName, manager);
            }
            else
            {
                return MethodEmitter.GenerateAssembly(plugins, assemblyName, manager);
            }
        }
        catch (Exception ex) when (!(ex is ParserException))
        {
            throw ParserException.AssemblyGenerationError(assemblyName, ex);
        }
    }

    /// <summary>
    /// 从 JSON 字符串生成动态程序集
    /// </summary>
    /// <param name="jsonString">插件清单 JSON 字符串</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <param name="useCache">是否使用缓存</param>
    /// <returns>生成的动态程序集</returns>
    public static Assembly GenerateFromJson(string jsonString, string assemblyName = "DynamicPluginAssembly",
        IPluginManager? pluginManager = null, bool useCache = true)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            throw new ArgumentException("JSON 字符串不能为空", nameof(jsonString));
        }

        try
        {
            var plugins = JsonSerializer.Deserialize<List<PluginInfo>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            });

            if (plugins == null)
            {
                throw new ParserException("JSON 反序列化失败，结果为 null");
            }

            return Generate(plugins, assemblyName, pluginManager, useCache);
        }
        catch (JsonException ex)
        {
            throw new ParserException($"JSON 格式错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 从 JSON 文件生成动态程序集
    /// </summary>
    /// <param name="jsonFilePath">JSON 文件路径</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <param name="useCache">是否使用缓存</param>
    /// <returns>生成的动态程序集</returns>
    public static async Task<Assembly> GenerateFromFileAsync(string jsonFilePath, string assemblyName = "DynamicPluginAssembly",
        IPluginManager? pluginManager = null, bool useCache = true)
    {
        if (string.IsNullOrWhiteSpace(jsonFilePath))
        {
            throw new ArgumentException("文件路径不能为空", nameof(jsonFilePath));
        }

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"文件不存在: {jsonFilePath}");
        }

        try
        {
            var jsonString = await File.ReadAllTextAsync(jsonFilePath);
            return GenerateFromJson(jsonString, assemblyName, pluginManager, useCache);
        }
        catch (Exception ex) when (!(ex is ParserException))
        {
            throw new ParserException($"读取文件失败: {jsonFilePath}", ex);
        }
    }

    /// <summary>
    /// 强制重新生成程序集（绕过缓存）
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <returns>新生成的动态程序集</returns>
    public static Assembly Regenerate(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly",
        IPluginManager? pluginManager = null)
    {
        if (plugins == null || plugins.Count == 0)
        {
            throw new ArgumentException("插件列表不能为空", nameof(plugins));
        }

        try
        {
            var manager = pluginManager ?? _defaultPluginManager ?? _pluginManagerFactory?.Invoke() ?? new MockPluginManager();

            // 设置静态插件管理器实例
            MethodEmitter.SetStaticPluginManager(manager);

            return AssemblyCache.ForceRegenerate(plugins, assemblyName, manager);
        }
        catch (Exception ex) when (!(ex is ParserException))
        {
            throw ParserException.AssemblyGenerationError(assemblyName, ex);
        }
    }

    /// <summary>
    /// 注册自定义类型映射
    /// </summary>
    /// <param name="typeName">类型名字符串</param>
    /// <param name="type">对应的 Type 对象</param>
    public static void RegisterCustomType(string typeName, Type type)
    {
        TypeMapper.RegisterCustomType(typeName, type);
    }

    /// <summary>
    /// 清除所有缓存
    /// </summary>
    public static void ClearCache()
    {
        AssemblyCache.ClearCache();
        TypeMapper.ClearCache();
    }

    /// <summary>
    /// 获取缓存统计信息
    /// </summary>
    /// <returns>缓存统计信息</returns>
    public static CacheStatistics GetCacheStatistics()
    {
        return AssemblyCache.GetStatistics();
    }

    /// <summary>
    /// 检查插件清单是否已有缓存
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns>是否存在缓存</returns>
    public static bool HasCache(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly")
    {
        return AssemblyCache.HasCache(plugins, assemblyName);
    }

    /// <summary>
    /// 将程序集保存到磁盘（用于调试）
    /// </summary>
    /// <param name="assembly">要保存的程序集</param>
    /// <param name="filePath">保存路径</param>
    /// <param name="overwrite">是否覆盖现有文件</param>
    public static void SaveAssemblyToFile(Assembly assembly, string filePath, bool overwrite = false)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("文件路径不能为空", nameof(filePath));

        if (File.Exists(filePath) && !overwrite)
            throw new InvalidOperationException($"文件已存在且不允许覆盖: {filePath}");

        try
        {
            // 注意：在 .NET 8.0 中，动态程序集默认是不可保存的
            // 这个方法主要用于调试目的，实际保存功能需要特殊处理
            Console.WriteLine($"[Parser] 注意: .NET 8.0 中动态程序集无法直接保存到磁盘");
            Console.WriteLine($"[Parser] 程序集信息: {assembly.FullName}");
            Console.WriteLine($"[Parser] 包含类型: {string.Join(", ", assembly.GetTypes().Select(t => t.Name))}");
        }
        catch (Exception ex)
        {
            throw new ParserException($"保存程序集失败: {filePath}", ex);
        }
    }

    /// <summary>
    /// 获取程序集中的所有插件类型
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <returns>插件类型列表</returns>
    public static List<Type> GetPluginTypes(Assembly assembly)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        try
        {
            return assembly.GetTypes()
                .Where(t => t.IsClass && t.IsAbstract && t.IsSealed) // 静态类
                .ToList();
        }
        catch (Exception ex)
        {
            throw new ParserException("获取插件类型失败", ex);
        }
    }

    /// <summary>
    /// 获取插件类型的所有方法信息
    /// </summary>
    /// <param name="pluginType">插件类型</param>
    /// <returns>方法信息列表</returns>
    public static List<MethodInfo> GetPluginMethods(Type pluginType)
    {
        if (pluginType == null)
            throw new ArgumentNullException(nameof(pluginType));

        try
        {
            return pluginType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => !m.IsSpecialName) // 排除特殊方法如属性访问器
                .ToList();
        }
        catch (Exception ex)
        {
            throw new ParserException($"获取插件方法失败: {pluginType.Name}", ex);
        }
    }
}
