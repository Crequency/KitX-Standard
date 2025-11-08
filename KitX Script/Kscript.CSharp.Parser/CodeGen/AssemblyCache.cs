using System.Collections.Concurrent;
using System.Text;

namespace Kscript.CSharp.Parser.CodeGen;

/// <summary>
/// 程序集缓存管理器，避免重复生成相同的程序集
/// </summary>
public static class AssemblyCache
{
    /// <summary>
    /// 程序集缓存字典，Key 为插件清单的哈希值，Value 为生成的程序集
    /// </summary>
    private static readonly ConcurrentDictionary<string, Assembly> _assemblyCache = new();

    /// <summary>
    /// 获取或生成程序集
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <returns>缓存的或新生成的程序集</returns>
    public static Assembly GetOrCreateAssembly(List<PluginInfo> plugins, string assemblyName, Core.IPluginManager pluginManager)
    {
        var cacheKey = GenerateCacheKey(plugins, assemblyName);

        // 尝试从缓存获取
        if (_assemblyCache.TryGetValue(cacheKey, out var cachedAssembly))
        {
            return cachedAssembly;
        }

        // 生成新的程序集
        var newAssembly = MethodEmitter.GenerateAssembly(plugins, assemblyName, pluginManager);

        // 添加到缓存
        _assemblyCache.TryAdd(cacheKey, newAssembly);

        return newAssembly;
    }

    /// <summary>
    /// 生成插件清单的缓存键
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns>缓存键字符串</returns>
    private static string GenerateCacheKey(List<PluginInfo> plugins, string assemblyName)
    {
        // 使用轻量级字符串哈希，避免复杂的JSON序列化和SHA256计算
        var keyBuilder = new StringBuilder();
        keyBuilder.Append(assemblyName);
        keyBuilder.Append($"|{plugins.Count}");

        foreach (var plugin in plugins.OrderBy(p => p.Name))
        {
            keyBuilder.Append($"|{plugin.Name}:{plugin.Version}:{plugin.Functions.Count}");
            foreach (var function in plugin.Functions.OrderBy(f => f.Name))
            {
                keyBuilder.Append($":{function.Name}:{function.ReturnValueType}:{function.Parameters.Count}");
            }
        }

        return keyBuilder.ToString().GetHashCode().ToString("X");
    }

    /// <summary>
    /// 清除所有缓存
    /// </summary>
    public static void ClearCache()
    {
        _assemblyCache.Clear();
        // 清除所有程序集加载上下文
        MethodEmitter.ClearAllAssemblyContexts();
    }

    /// <summary>
    /// 获取缓存统计信息
    /// </summary>
    /// <returns>缓存统计信息</returns>
    public static CacheStatistics GetStatistics()
    {
        return new CacheStatistics
        {
            CachedAssemblyCount = _assemblyCache.Count,
            CacheKeys = _assemblyCache.Keys.ToList()
        };
    }

    /// <summary>
    /// 检查是否存在缓存
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns>是否存在缓存</returns>
    public static bool HasCache(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly")
    {
        var cacheKey = GenerateCacheKey(plugins, assemblyName);
        return _assemblyCache.ContainsKey(cacheKey);
    }

    /// <summary>
    /// 强制重新生成程序集（绕过缓存）
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <returns>新生成的程序集</returns>
    public static Assembly ForceRegenerate(List<PluginInfo> plugins, Core.IPluginManager pluginManager, string assemblyName = "DynamicPluginAssembly")
    {
        var cacheKey = GenerateCacheKey(plugins, assemblyName);

        // 移除现有缓存
        _assemblyCache.TryRemove(cacheKey, out _);

        // 生成新的程序集（这会自动卸载旧的程序集加载上下文）
        var newAssembly = MethodEmitter.GenerateAssembly(plugins, assemblyName, pluginManager);

        // 添加到缓存
        _assemblyCache.TryAdd(cacheKey, newAssembly);

        return newAssembly;
    }
}

/// <summary>
/// 缓存统计信息
/// </summary>
public class CacheStatistics
{
    /// <summary>
    /// 缓存的程序集数量
    /// </summary>
    public int CachedAssemblyCount { get; set; }

    /// <summary>
    /// 缓存键列表
    /// </summary>
    public List<string> CacheKeys { get; set; } = new();

    public override string ToString()
    {
        return $"缓存统计: {CachedAssemblyCount} 个程序集";
    }
}
