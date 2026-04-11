using Kscript.CSharp.Parser.Core;
using Kscript.CSharp.Parser.Exceptions;
using Kscript.CSharp.Parser.Models;
using KitX.Shared.CSharp.Plugin;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Collections.Concurrent;

namespace Kscript.CSharp.Parser.CodeGen;

/// <summary>
/// 可收集的程序集加载上下文，支持程序集卸载
/// </summary>
public class CollectibleAssemblyLoadContext : AssemblyLoadContext
{
    public CollectibleAssemblyLoadContext(string name) : base(name, isCollectible: true)
    {
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        // 让默认上下文处理核心程序集加载
        return null!;
    }
}

/// <summary>
/// IL 方法生成器，负责生成插件调用的静态方法
/// </summary>
public static class MethodEmitter
{
    /// <summary>
    /// 程序集加载上下文缓存，用于隔离和卸载程序集
    /// </summary>
    private static readonly ConcurrentDictionary<string, CollectibleAssemblyLoadContext> _loadContexts = new();
    /// <summary>
    /// 为插件生成动态程序集
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <returns>生成的动态程序集</returns>
    public static Assembly GenerateAssembly(List<PluginInfo> plugins,
            string assemblyName, IPluginManager pluginManager)
    {
        try
        {
            // 如果已存在相同名称的程序集加载上下文，先卸载它
            if (_loadContexts.TryGetValue(assemblyName, out var existingContext))
            {
                UnloadAssemblyContext(assemblyName, existingContext);
            }

            // 创建新的可卸载的程序集加载上下文
            var loadContext = new CollectibleAssemblyLoadContext(assemblyName);
            _loadContexts.TryAdd(assemblyName, loadContext);

            // 使用PersistedAssemblyBuilder创建可保存的动态程序集
            var persistedAssemblyBuilder = new PersistedAssemblyBuilder(
                new AssemblyName(assemblyName),
                typeof(object).Assembly);

            var moduleBuilder = persistedAssemblyBuilder.DefineDynamicModule($"{assemblyName}.dll");

            // 为每个插件生成静态类
            foreach (var plugin in plugins)
            {
                GeneratePluginClass(moduleBuilder, plugin, pluginManager);
            }

            // 创建所有类型
            foreach (var plugin in plugins)
            {
                // 确保所有类型都已创建
                // 类型在GeneratePluginClass中已经创建
            }

            // 保存到临时文件并重新加载为可引用的程序集
            var tempPath = Path.Combine(Path.GetTempPath(), $"{assemblyName}_{Guid.NewGuid()}.dll");

            using (var fileStream = File.Create(tempPath))
            {
                persistedAssemblyBuilder.Save(fileStream);
            }

            // 使用自定义加载上下文从文件加载程序集
            var assembly = loadContext.LoadFromAssemblyPath(tempPath);

            Console.WriteLine($"[MethodEmitter] 使用CollectibleAssemblyLoadContext生成并加载程序集: {assemblyName} -> {tempPath}");

            // 注册临时文件删除（在程序退出时）
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                try
                {
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }
                catch
                {
                    // 忽略删除错误
                }
            };

            return assembly;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MethodEmitter] 生成程序集失败: {assemblyName}, 详细错误: {ex}");
            throw new ParserException($"生成程序集失败: {assemblyName}", ex);
        }
    }

    /// <summary>
    /// 卸载程序集加载上下文
    /// </summary>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="context">要卸载的加载上下文</param>
    private static void UnloadAssemblyContext(string assemblyName, CollectibleAssemblyLoadContext context)
    {
        try
        {
            Console.WriteLine($"[MethodEmitter] 卸载程序集加载上下文: {assemblyName}");

            // 从缓存中移除
            _loadContexts.TryRemove(assemblyName, out _);

            // 卸载上下文
            context.Unload();

            // 等待卸载完成
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MethodEmitter] 卸载程序集加载上下文失败: {assemblyName}, 错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 清除所有程序集加载上下文
    /// </summary>
    public static void ClearAllAssemblyContexts()
    {
        foreach (var kvp in _loadContexts)
        {
            UnloadAssemblyContext(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// 为单个插件生成静态类
    /// </summary>
    private static void GeneratePluginClass(ModuleBuilder moduleBuilder, PluginInfo plugin, IPluginManager pluginManager)
    {
        try
        {
            // 创建静态类
            var typeBuilder = moduleBuilder.DefineType(
                plugin.Name,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract | TypeAttributes.Sealed);

            // 添加插件管理器字段
            var pluginManagerField = typeBuilder.DefineField(
                "_pluginManager",
                typeof(IPluginManager),
                FieldAttributes.Private | FieldAttributes.Static);

            // 生成静态构造函数来初始化插件管理器
            GenerateStaticConstructor(typeBuilder, pluginManagerField, pluginManager);

            // 为每个功能生成静态方法
            foreach (var function in plugin.Functions)
            {
                GeneratePluginMethod(typeBuilder, plugin.Name, function, pluginManagerField);
            }

            // 创建类型
            typeBuilder.CreateType();
        }
        catch (Exception ex)
        {
            throw new ParserException($"生成IL代码失败: {plugin.Name} 类", ex);
        }
    }

    /// <summary>
    /// 生成静态构造函数
    /// </summary>
    private static void GenerateStaticConstructor(TypeBuilder typeBuilder, FieldBuilder pluginManagerField, IPluginManager pluginManager)
    {
        var constructorBuilder = typeBuilder.DefineConstructor(
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
            CallingConventions.Standard,
            Type.EmptyTypes);

        var il = constructorBuilder.GetILGenerator();

        // 使用提供的插件管理器实例
        il.Emit(OpCodes.Ldtoken, pluginManager.GetType());
        il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle")!);
        il.Emit(OpCodes.Call, typeof(Activator).GetMethod("CreateInstance", new[] { typeof(Type) })!);
        il.Emit(OpCodes.Castclass, typeof(IPluginManager));

        il.Emit(OpCodes.Stsfld, pluginManagerField);
        il.Emit(OpCodes.Ret);
    }

    /// <summary>
    /// 为插件功能生成静态方法
    /// </summary>
    private static void GeneratePluginMethod(TypeBuilder typeBuilder, string pluginName, Function function, FieldBuilder pluginManagerField)
    {
        try
        {
            // 解析参数类型
            var parameterTypes = function.Parameters
                .Select(p => TypeMapper.MapType(p.Type))
                .ToArray();

            // 解析返回值类型
            var returnType = TypeMapper.MapType(function.ReturnValueType);

            // 定义方法
            var methodBuilder = typeBuilder.DefineMethod(
                function.Name,
                MethodAttributes.Public | MethodAttributes.Static,
                returnType,
                parameterTypes);

            // 设置参数名称和属性
            for (int i = 0; i < function.Parameters.Count; i++)
            {
                var param = function.Parameters[i];

                // 根据 IsOptional 属性确定参数属性
                var paramAttributes = ParameterAttributes.None;
                if (param.IsOptional)
                {
                    paramAttributes |= ParameterAttributes.Optional;
                    // 如果有默认值，设置 HasDefault 标志
                    if (!string.IsNullOrEmpty(param.Value))
                    {
                        paramAttributes |= ParameterAttributes.HasDefault;
                    }
                }

                var paramBuilder = methodBuilder.DefineParameter(i + 1, paramAttributes, param.Name);

                // 如果参数是可选的且有默认值，设置默认值
                if (param.IsOptional && !string.IsNullOrEmpty(param.Value))
                {
                    var defaultValue = ConvertDefaultValue(param.Value, parameterTypes[i]);
                    if (defaultValue != null)
                    {
                        paramBuilder.SetConstant(defaultValue);
                    }
                }
            }

            // 生成方法体IL代码
            GenerateMethodBody(methodBuilder, pluginName, function, parameterTypes, returnType, pluginManagerField);
        }
        catch (Exception ex)
        {
            throw new ParserException($"生成IL代码失败: {pluginName}.{function.Name}", ex);
        }
    }

    /// <summary>
    /// 生成方法体IL代码
    /// </summary>
    private static void GenerateMethodBody(MethodBuilder methodBuilder, string pluginName, Function function,
        Type[] parameterTypes, Type returnType, FieldBuilder pluginManagerField)
    {
        var il = methodBuilder.GetILGenerator();

        // 声明局部变量
        var callInfoLocal = il.DeclareLocal(typeof(PluginCallInfo));
        var parametersArrayLocal = il.DeclareLocal(typeof(object[]));
        var parameterTypesArrayLocal = il.DeclareLocal(typeof(Type[]));
        var parameterNamesArrayLocal = il.DeclareLocal(typeof(string[]));
        LocalBuilder? resultLocal = null;

        if (returnType != typeof(void))
        {
            resultLocal = il.DeclareLocal(returnType);
        }

        // 创建参数值数组
        il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
        il.Emit(OpCodes.Newarr, typeof(object));
        il.Emit(OpCodes.Stloc, parametersArrayLocal);

        // 创建参数类型数组
        il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
        il.Emit(OpCodes.Newarr, typeof(Type));
        il.Emit(OpCodes.Stloc, parameterTypesArrayLocal);

        // 创建参数名称数组
        il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
        il.Emit(OpCodes.Newarr, typeof(string));
        il.Emit(OpCodes.Stloc, parameterNamesArrayLocal);

        // 填充参数数组、类型数组和名称数组
        for (int i = 0; i < parameterTypes.Length; i++)
        {
            var param = function.Parameters[i];

            // 填充参数值
            il.Emit(OpCodes.Ldloc, parametersArrayLocal);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldarg, i);

            // 如果是值类型，需要装箱
            if (parameterTypes[i].IsValueType)
            {
                il.Emit(OpCodes.Box, parameterTypes[i]);
            }

            il.Emit(OpCodes.Stelem_Ref);

            // 填充参数类型
            il.Emit(OpCodes.Ldloc, parameterTypesArrayLocal);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldtoken, parameterTypes[i]);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle")!);
            il.Emit(OpCodes.Stelem_Ref);

            // 填充参数名称
            il.Emit(OpCodes.Ldloc, parameterNamesArrayLocal);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldstr, param.Name ?? $"param{i}");
            il.Emit(OpCodes.Stelem_Ref);
        }

        // 创建 PluginCallInfo 实例
        il.Emit(OpCodes.Ldstr, pluginName);
        il.Emit(OpCodes.Ldstr, function.Name);
        il.Emit(OpCodes.Ldloc, parametersArrayLocal);
        il.Emit(OpCodes.Ldloc, parameterTypesArrayLocal);
        il.Emit(OpCodes.Ldloc, parameterNamesArrayLocal);
        il.Emit(OpCodes.Newobj, typeof(PluginCallInfo).GetConstructor(new[]
        {
            typeof(string), typeof(string), typeof(object[]), typeof(Type[]), typeof(string[])
        })!);
        il.Emit(OpCodes.Stloc, callInfoLocal);

        // 调用插件管理器
        il.Emit(OpCodes.Ldsfld, pluginManagerField);
        il.Emit(OpCodes.Ldloc, callInfoLocal);

        if (returnType == typeof(void))
        {
            // 调用无返回值的方法
            var voidCallMethod = typeof(IPluginManager).GetMethods()
                .First(m => m.Name == "Call" && !m.IsGenericMethod && m.GetParameters().Length == 1);
            il.Emit(OpCodes.Callvirt, voidCallMethod);
        }
        else
        {
            // 调用有返回值的泛型方法
            var genericCallMethod = typeof(IPluginManager).GetMethods()
                .First(m => m.Name == "Call" && m.IsGenericMethod && m.GetParameters().Length == 1)
                .MakeGenericMethod(returnType);
            il.Emit(OpCodes.Callvirt, genericCallMethod);
            il.Emit(OpCodes.Stloc, resultLocal!);
            il.Emit(OpCodes.Ldloc, resultLocal!);
        }

        il.Emit(OpCodes.Ret);
    }

    /// <summary>
    /// 转换默认值
    /// </summary>
    private static object? ConvertDefaultValue(string value, Type targetType)
    {
        try
        {
            if (targetType == typeof(string))
                return value;

            if (string.IsNullOrEmpty(value))
                return null;

            return Convert.ChangeType(value, targetType);
        }
        catch
        {
            return null;
        }
    }
}
