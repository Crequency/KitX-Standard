using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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
    /// 程序集引用计数，用于清理不再使用的程序集
    /// </summary>
    private static readonly ConcurrentDictionary<string, int> _referenceCount = new();

    /// <summary>
    /// 缓存锁，确保并发安全
    /// </summary>
    private static readonly object _cacheLock = new();

    /// <summary>
    /// 获取或生成程序集
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <returns>缓存的或新生成的程序集</returns>
    public static Assembly GetOrCreateAssembly(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly", Core.IPluginManager? pluginManager = null)
    {
        var cacheKey = GenerateCacheKey(plugins, assemblyName);

        // 尝试从缓存获取
        if (_assemblyCache.TryGetValue(cacheKey, out var cachedAssembly))
        {
            IncrementReference(cacheKey);
            return cachedAssembly;
        }

        lock (_cacheLock)
        {
            // 双重检查锁定模式
            if (_assemblyCache.TryGetValue(cacheKey, out cachedAssembly))
            {
                IncrementReference(cacheKey);
                return cachedAssembly;
            }

            // 生成新的程序集
            var newAssembly = MethodEmitter.GenerateAssembly(plugins, assemblyName, pluginManager);

            // 添加到缓存
            _assemblyCache[cacheKey] = newAssembly;
            _referenceCount[cacheKey] = 1;

            return newAssembly;
        }
    }

    /// <summary>
    /// 生成插件清单的缓存键
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns>缓存键字符串</returns>
    private static string GenerateCacheKey(List<PluginInfo> plugins, string assemblyName)
    {
        try
        {
            // 创建用于哈希的数据对象
            var cacheData = new
            {
                AssemblyName = assemblyName,
                Plugins = plugins.Select(p => new
                {
                    p.Name,
                    p.Version,
                    Functions = p.Functions.Select(f => new
                    {
                        f.Name,
                        f.ReturnValueType,
                        Parameters = f.Parameters.Select(param => new
                        {
                            param.Name,
                            param.Type,
                            param.IsOptional,
                            param.Value
                        }).OrderBy(param => param.Name)
                    }).OrderBy(f => f.Name)
                }).OrderBy(p => p.Name)
            };

            // 序列化为JSON
            var json = JsonSerializer.Serialize(cacheData, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // 计算SHA256哈希
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
            return Convert.ToHexString(hashBytes);
        }
        catch (Exception ex)
        {
            // 如果哈希计算失败，使用基础信息生成简单键
            var fallbackKey = $"{assemblyName}_{plugins.Count}_{string.Join("_", plugins.Select(p => $"{p.Name}_{p.Functions.Count}"))}_{DateTime.UtcNow.Ticks}";
            Console.WriteLine($"[AssemblyCache] 哈希计算失败，使用回退键: {ex.Message}");
            return fallbackKey;
        }
    }

    /// <summary>
    /// 增加引用计数
    /// </summary>
    /// <param name="cacheKey">缓存键</param>
    private static void IncrementReference(string cacheKey)
    {
        _referenceCount.AddOrUpdate(cacheKey, 1, (key, count) => count + 1);
    }

    /// <summary>
    /// 减少引用计数
    /// </summary>
    /// <param name="cacheKey">缓存键</param>
    public static void DecrementReference(string cacheKey)
    {
        if (_referenceCount.TryGetValue(cacheKey, out var count))
        {
            var newCount = count - 1;
            if (newCount <= 0)
            {
                // 引用计数为0，从缓存中移除
                _assemblyCache.TryRemove(cacheKey, out _);
                _referenceCount.TryRemove(cacheKey, out _);
            }
            else
            {
                _referenceCount[cacheKey] = newCount;
            }
        }
    }

    /// <summary>
    /// 清除所有缓存
    /// </summary>
    public static void ClearCache()
    {
        lock (_cacheLock)
        {
            _assemblyCache.Clear();
            _referenceCount.Clear();
        }
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
            TotalReferences = _referenceCount.Values.Sum(),
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
    public static Assembly ForceRegenerate(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly", Core.IPluginManager? pluginManager = null)
    {
        var cacheKey = GenerateCacheKey(plugins, assemblyName);

        lock (_cacheLock)
        {
            // 移除现有缓存
            _assemblyCache.TryRemove(cacheKey, out _);
            _referenceCount.TryRemove(cacheKey, out _);

            // 生成新的程序集
            var newAssembly = MethodEmitter.GenerateAssembly(plugins, assemblyName, pluginManager);

            // 添加到缓存
            _assemblyCache[cacheKey] = newAssembly;
            _referenceCount[cacheKey] = 1;

            return newAssembly;
        }
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
    /// 总引用数量
    /// </summary>
    public int TotalReferences { get; set; }

    /// <summary>
    /// 缓存键列表
    /// </summary>
    public List<string> CacheKeys { get; set; } = new();

    public override string ToString()
    {
        return $"缓存统计: {CachedAssemblyCount} 个程序集, {TotalReferences} 个引用";
    }
}
