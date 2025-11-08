using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;
using System.Reflection.Emit;
using Kscript.CSharp.Parser.Core;
using Kscript.CSharp.Parser.Models;

namespace Kscript.CSharp.Compiler;

/// <summary>
/// C#脚本执行器，用于执行用户编写的C#脚本并调用生成的插件方法
/// </summary>
public class ScriptExecutor : IDisposable
{
    private readonly Dictionary<string, object> _globalVariables = new();
    private readonly List<Assembly> _referencedAssemblies = new();
    private readonly List<string> _usings = new();
    private ScriptOptions? _scriptOptions;
    private bool _disposed = false;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ScriptExecutor()
    {
        _scriptOptions = ScriptOptions.Default
            .WithImports("System", "System.Math", "System.Collections.Generic", "System.Console", "System.Linq")
            .WithEmitDebugInformation(true);
    }

    /// <summary>
    /// 添加程序集引用
    /// </summary>
    /// <param name="assembly">要引用的程序集</param>
    public void AddAssemblyReference(Assembly assembly)
    {
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        if (!_referencedAssemblies.Contains(assembly))
        {
            _referencedAssemblies.Add(assembly);
            Console.WriteLine($"[ScriptExecutor] 已添加程序集引用: {assembly.FullName}");
        }
    }

    /// <summary>
    /// 添加命名空间引用
    /// </summary>
    /// <param name="namespace">命名空间</param>
    public void AddUsing(string @namespace)
    {
        if (string.IsNullOrEmpty(@namespace))
            throw new ArgumentException("命名空间不能为空", nameof(@namespace));

        if (!_usings.Contains(@namespace))
        {
            _usings.Add(@namespace);
            Console.WriteLine($"[ScriptExecutor] 已添加命名空间引用: {@namespace}");
        }
    }

    /// <summary>
    /// 设置全局变量
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="value">变量值</param>
    public void SetGlobalVariable(string name, object value)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("变量名不能为空", nameof(name));

        _globalVariables[name] = value;
        Console.WriteLine($"[ScriptExecutor] 已设置全局变量: {name} = {value}");
    }

    /// <summary>
    /// 执行C#脚本
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="script">脚本代码</param>
    /// <param name="assemblyName">程序集名称（用于调试）</param>
    /// <returns>执行结果</returns>
    public async Task<T> ExecuteAsync<T>(string script, string assemblyName = "ScriptAssembly")
    {
        if (string.IsNullOrEmpty(script))
            throw new ArgumentException("脚本代码不能为空", nameof(script));

        try
        {
            Console.WriteLine($"[ScriptExecutor] 开始执行脚本: {assemblyName}");
            Console.WriteLine($"[ScriptExecutor] 脚本代码: {script.Substring(0, Math.Min(script.Length, 100))}...");

            // 更新脚本选项，包含所有引用
            var options = ScriptOptions.Default
                .WithReferences(_referencedAssemblies)
                .WithImports(_usings.Concat(new[] { "System.Console" }))
                .WithEmitDebugInformation(true);

            // 创建脚本对象
            var csharpScript = CSharpScript.Create<T>(script, options);

            // 执行脚本（不使用全局变量）
            var result = await csharpScript.RunAsync(null);

            Console.WriteLine($"[ScriptExecutor] 脚本执行成功，结果: {result.ReturnValue}");
            return result.ReturnValue;
        }
        catch (CompilationErrorException ex)
        {
            Console.WriteLine($"[ScriptExecutor] 脚本编译错误: {ex.Message}");
            throw new ScriptExecutionException($"脚本编译失败: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScriptExecutor] 脚本执行异常: {ex.Message}");
            throw new ScriptExecutionException($"脚本执行失败: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 执行脚本（无返回值）
    /// </summary>
    /// <param name="script">脚本代码</param>
    /// <param name="assemblyName">程序集名称</param>
    public async Task ExecuteAsync(string script, string assemblyName = "ScriptAssembly")
    {
        await ExecuteAsync<object>(script, assemblyName);
    }

    /// <summary>
    /// 验证脚本语法
    /// </summary>
    /// <param name="script">脚本代码</param>
    /// <returns>验证结果</returns>
    public ScriptValidationResult ValidateScript(string script)
    {
        if (string.IsNullOrEmpty(script))
            return new ScriptValidationResult(false, "脚本代码不能为空");

        try
        {
            // 更新脚本选项，包含所有引用
            var options = ScriptOptions.Default
                .WithReferences(_referencedAssemblies)
                .WithImports(_usings.Concat(new[] { "System.Console" }))
                .WithEmitDebugInformation(true);

            var csharpScript = CSharpScript.Create(script, options);

            // 尝试编译以检查语法
            var diagnostics = csharpScript.Compile();

            if (!diagnostics.Any())
            {
                return new ScriptValidationResult(true, "脚本语法正确");
            }
            else
            {
                var errors = string.Join("\n", diagnostics.Select(d => d.ToString()));
                return new ScriptValidationResult(false, $"脚本语法错误:\n{errors}");
            }
        }
        catch (Exception ex)
        {
            return new ScriptValidationResult(false, $"脚本验证异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 获取当前状态信息
    /// </summary>
    /// <returns>状态信息</returns>
    public ScriptExecutorStatus GetStatus()
    {
        return new ScriptExecutorStatus
        {
            ReferencedAssembliesCount = _referencedAssemblies.Count,
            GlobalVariablesCount = _globalVariables.Count,
            UsingsCount = _usings.Count,
            AssemblyNames = _referencedAssemblies.Select(a => a.GetName().Name).ToList()
        };
    }

    /// <summary>
    /// 清除所有引用和变量
    /// </summary>
    public void Clear()
    {
        _referencedAssemblies.Clear();
        _globalVariables.Clear();
        _usings.Clear();
        Console.WriteLine("[ScriptExecutor] 已清除所有引用和变量");
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            Clear();
            _disposed = true;
            Console.WriteLine("[ScriptExecutor] 资源已释放");
        }
    }
}

/// <summary>
/// 脚本全局变量容器
/// </summary>
public class ScriptGlobals
{
    private readonly Dictionary<string, object> _variables;

    public ScriptGlobals(Dictionary<string, object> variables)
    {
        _variables = variables ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// 动态属性访问器
    /// </summary>
    public object this[string name]
    {
        get
        {
            if (_variables.TryGetValue(name, out var value))
                return value;
            throw new KeyNotFoundException($"未找到全局变量: {name}");
        }
    }
}

/// <summary>
/// 脚本验证结果
/// </summary>
public record ScriptValidationResult(bool IsValid, string Message);

/// <summary>
/// 脚本执行器状态
/// </summary>
public record ScriptExecutorStatus
{
    public int ReferencedAssembliesCount { get; init; }
    public int GlobalVariablesCount { get; init; }
    public int UsingsCount { get; init; }
    public List<string> AssemblyNames { get; init; } = new();
}

/// <summary>
/// 脚本执行异常
/// </summary>
public class ScriptExecutionException : Exception
{
    public ScriptExecutionException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
