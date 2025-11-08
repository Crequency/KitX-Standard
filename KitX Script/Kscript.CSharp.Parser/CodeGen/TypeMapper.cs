using System.Collections.Concurrent;

namespace Kscript.CSharp.Parser.CodeGen;

/// <summary>
/// 类型映射器，将字符串类型名映射到 System.Type
/// </summary>
public static class TypeMapper
{
    /// <summary>
    /// 基础类型映射表
    /// </summary>
    private static readonly Dictionary<string, Type> _basicTypeMap = new()
    {
        { "void", typeof(void) },
        { "bool", typeof(bool) },
        { "byte", typeof(byte) },
        { "sbyte", typeof(sbyte) },
        { "short", typeof(short) },
        { "ushort", typeof(ushort) },
        { "int", typeof(int) },
        { "uint", typeof(uint) },
        { "long", typeof(long) },
        { "ulong", typeof(ulong) },
        { "float", typeof(float) },
        { "double", typeof(double) },
        { "decimal", typeof(decimal) },
        { "char", typeof(char) },
        { "string", typeof(string) },
        { "object", typeof(object) }
    };

    /// <summary>
    /// 类型缓存
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> _typeCache = new();

    /// <summary>
    /// 映射字符串类型名到 System.Type
    /// </summary>
    /// <param name="typeName">类型名字符串</param>
    /// <returns>对应的 Type 对象，如果找不到则返回 typeof(object)</returns>
    public static Type MapType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            return typeof(object);

        // 先检查缓存
        if (_typeCache.TryGetValue(typeName, out var cachedType))
            return cachedType;

        // 检查基础类型
        if (_basicTypeMap.TryGetValue(typeName, out var basicType))
        {
            _typeCache[typeName] = basicType;
            return basicType;
        }

        // 尝试直接解析类型
        try
        {
            var resolvedType = Type.GetType(typeName);
            if (resolvedType != null)
            {
                _typeCache[typeName] = resolvedType;
                return resolvedType;
            }
        }
        catch
        {
            // 发生异常时回退到 object 类型
        }

        // 回退到 object 类型
        _typeCache[typeName] = typeof(object);
        return typeof(object);
    }

    /// <summary>
    /// 清除类型缓存
    /// </summary>
    public static void ClearCache()
    {
        _typeCache.Clear();
    }
}
