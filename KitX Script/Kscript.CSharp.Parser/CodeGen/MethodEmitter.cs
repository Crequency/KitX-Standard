using Kscript.CSharp.Parser.Core;
using Kscript.CSharp.Parser.Exceptions;
using Kscript.CSharp.Parser.Models;

namespace Kscript.CSharp.Parser.CodeGen;

/// <summary>
/// IL 方法生成器，负责生成插件调用的静态方法
/// </summary>
public static class MethodEmitter
{
    /// <summary>
    /// 为插件生成动态程序集
    /// </summary>
    /// <param name="plugins">插件信息列表</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="pluginManager">插件管理器实例</param>
    /// <returns>生成的动态程序集</returns>
    public static Assembly GenerateAssembly(List<PluginInfo> plugins, string assemblyName = "DynamicPluginAssembly", IPluginManager? pluginManager = null)
    {
        try
        {
            // 创建动态程序集
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{assemblyName}.dll");

            // 为每个插件生成静态类
            foreach (var plugin in plugins)
            {
                GeneratePluginClass(moduleBuilder, plugin, pluginManager);
            }

            return assemblyBuilder;
        }
        catch (Exception ex)
        {
            throw ParserException.AssemblyGenerationError(assemblyName, ex);
        }
    }

    /// <summary>
    /// 为单个插件生成静态类
    /// </summary>
    private static void GeneratePluginClass(ModuleBuilder moduleBuilder, PluginInfo plugin, IPluginManager? pluginManager)
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
            throw ParserException.ILGenerationError($"{plugin.Name} 类", ex);
        }
    }

    /// <summary>
    /// 生成静态构造函数
    /// </summary>
    private static void GenerateStaticConstructor(TypeBuilder typeBuilder, FieldBuilder pluginManagerField, IPluginManager? pluginManager)
    {
        var constructorBuilder = typeBuilder.DefineConstructor(
            MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
            CallingConventions.Standard,
            Type.EmptyTypes);

        var il = constructorBuilder.GetILGenerator();

        if (pluginManager != null)
        {
            // 如果提供了插件管理器实例，直接使用
            il.Emit(OpCodes.Ldtoken, pluginManager.GetType());
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle")!);
            il.Emit(OpCodes.Call, typeof(Activator).GetMethod("CreateInstance", new[] { typeof(Type) })!);
            il.Emit(OpCodes.Castclass, typeof(IPluginManager));
        }
        else
        {
            // 使用默认的 MockPluginManager
            il.Emit(OpCodes.Newobj, typeof(MockPluginManager).GetConstructor(Type.EmptyTypes)!);
        }

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

            // 设置参数名称
            for (int i = 0; i < function.Parameters.Count; i++)
            {
                var param = function.Parameters[i];
                var paramBuilder = methodBuilder.DefineParameter(i + 1, ParameterAttributes.None, param.Name);

                // 如果参数是可选的，设置默认值
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
            throw ParserException.ILGenerationError($"{pluginName}.{function.Name}", ex);
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
        LocalBuilder? resultLocal = null;

        if (returnType != typeof(void))
        {
            resultLocal = il.DeclareLocal(returnType);
        }

        // 创建参数数组
        il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
        il.Emit(OpCodes.Newarr, typeof(object));
        il.Emit(OpCodes.Stloc, parametersArrayLocal);

        // 创建参数类型数组
        il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
        il.Emit(OpCodes.Newarr, typeof(Type));
        il.Emit(OpCodes.Stloc, parameterTypesArrayLocal);

        // 填充参数数组和类型数组
        for (int i = 0; i < parameterTypes.Length; i++)
        {
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
        }

        // 创建 PluginCallInfo 实例
        il.Emit(OpCodes.Ldstr, pluginName);
        il.Emit(OpCodes.Ldstr, function.Name);
        il.Emit(OpCodes.Ldloc, parametersArrayLocal);
        il.Emit(OpCodes.Ldloc, parameterTypesArrayLocal);
        il.Emit(OpCodes.Newobj, typeof(PluginCallInfo).GetConstructor(new[]
        {
            typeof(string), typeof(string), typeof(object[]), typeof(Type[])
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
