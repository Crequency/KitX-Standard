using System.Collections.Concurrent;
using System.Text.RegularExpressions;

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
    /// 自定义类型映射表
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> _customTypeMap = new();

    /// <summary>
    /// 类型缓存
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> _typeCache = new();

    /// <summary>
    /// 泛型类型正则表达式
    /// </summary>
    private static readonly Regex _genericTypeRegex = new(@"^(\w+)<(.+)>$", RegexOptions.Compiled);

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

        Type? resolvedType = null;

        try
        {
            // 1. 检查基础类型
            if (_basicTypeMap.TryGetValue(typeName, out resolvedType))
            {
                _typeCache[typeName] = resolvedType;
                return resolvedType;
            }

            // 2. 检查自定义映射
            if (_customTypeMap.TryGetValue(typeName, out resolvedType))
            {
                _typeCache[typeName] = resolvedType;
                return resolvedType;
            }

            // 3. 处理泛型类型
            var genericMatch = _genericTypeRegex.Match(typeName);
            if (genericMatch.Success)
            {
                resolvedType = ResolveGenericType(genericMatch);
                if (resolvedType != null)
                {
                    _typeCache[typeName] = resolvedType;
                    return resolvedType;
                }
            }

            // 4. 尝试直接解析类型
            resolvedType = Type.GetType(typeName);
            if (resolvedType != null)
            {
                _typeCache[typeName] = resolvedType;
                return resolvedType;
            }

            // 5. 回退到 object 类型
            resolvedType = typeof(object);
            _typeCache[typeName] = resolvedType;
            return resolvedType;
        }
        catch
        {
            // 发生异常时回退到 object 类型
            resolvedType = typeof(object);
            _typeCache[typeName] = resolvedType;
            return resolvedType;
        }
    }

    /// <summary>
    /// 解析泛型类型
    /// </summary>
    private static Type? ResolveGenericType(Match genericMatch)
    {
        var genericTypeName = genericMatch.Groups[1].Value;
        var genericArgs = genericMatch.Groups[2].Value;

        // 解析泛型参数
        var argTypes = ParseGenericArguments(genericArgs);
        if (argTypes.Length == 0)
            return null;

        // 处理常见的泛型类型
        return genericTypeName switch
        {
            "List" => typeof(List<>).MakeGenericType(argTypes),
            "Dictionary" when argTypes.Length == 2 => typeof(Dictionary<,>).MakeGenericType(argTypes),
            "Array" => argTypes[0].MakeArrayType(),
            "Nullable" => typeof(Nullable<>).MakeGenericType(argTypes),
            _ => null
        };
    }

    /// <summary>
    /// 解析泛型参数
    /// </summary>
    private static Type[] ParseGenericArguments(string argsString)
    {
        var args = SplitGenericArguments(argsString);
        return args.Select(MapType).ToArray();
    }

    /// <summary>
    /// 分割泛型参数字符串，处理嵌套泛型
    /// </summary>
    private static string[] SplitGenericArguments(string argsString)
    {
        var args = new List<string>();
        var current = "";
        var depth = 0;

        foreach (char c in argsString)
        {
            if (c == '<')
            {
                depth++;
                current += c;
            }
            else if (c == '>')
            {
                depth--;
                current += c;
            }
            else if (c == ',' && depth == 0)
            {
                args.Add(current.Trim());
                current = "";
            }
            else
            {
                current += c;
            }
        }

        if (!string.IsNullOrEmpty(current))
            args.Add(current.Trim());

        return args.ToArray();
    }

    /// <summary>
    /// 注册自定义类型映射
    /// </summary>
    /// <param name="typeName">类型名字符串</param>
    /// <param name="type">对应的 Type 对象</param>
    public static void RegisterCustomType(string typeName, Type type)
    {
        _customTypeMap[typeName] = type;
        _typeCache.TryRemove(typeName, out _); // 清除缓存
    }

    /// <summary>
    /// 清除类型缓存
    /// </summary>
    public static void ClearCache()
    {
        _typeCache.Clear();
    }

    /// <summary>
    /// 获取所有已注册的自定义类型
    /// </summary>
    public static IReadOnlyDictionary<string, Type> GetCustomTypes()
    {
        return _customTypeMap.AsReadOnly();
    }
}
