using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// KCS (KitX Code Script) 文件格式定义
/// </summary>
public class KcsFileFormat
{
    /// <summary>
    /// 主程序代码
    /// </summary>
    public string MainProgram { get; set; } = string.Empty;

    /// <summary>
    /// 辅助函数列表
    /// </summary>
    public List<HelperFunction> HelperFunctions { get; set; } = [];

    /// <summary>
    /// 可变常量及其用户修改后的值
    /// </summary>
    public Dictionary<string, object?> VariableConstants { get; set; } = [];
}

/// <summary>
/// 辅助函数参数定义
/// </summary>
public class HelperFunctionParameter
{
    /// <summary>
    /// 参数名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参数类型
    /// </summary>
    public string Type { get; set; } = "object";
}

/// <summary>
/// 辅助函数定义
/// </summary>
public class HelperFunction
{
    /// <summary>
    /// 函数名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 函数参数列表
    /// </summary>
    public List<HelperFunctionParameter> Parameters { get; set; } = [];

    /// <summary>
    /// 返回值类型
    /// </summary>
    public string ReturnType { get; set; } = "object";

    /// <summary>
    /// 函数体代码（不包括函数签名）
    /// </summary>
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// 可变常量（用于UI显示）
/// </summary>
public class VariableConstant
{
    /// <summary>
    /// 常量名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 默认值
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// 用户修改后的值
    /// </summary>
    public object? UserValue { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public string Type { get; set; } = "string";
}

/// <summary>
/// 主程序代码分析结果
/// </summary>
public class MainProgramAnalysisResult
{
    /// <summary>
    /// 是否有效
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 禁止原因
    /// </summary>
    public string ForbiddenReason { get; set; } = string.Empty;
}

/// <summary>
/// KCS文件服务接口 - 仅负责KCS文件的读写
/// </summary>
public interface IKcsFileService
{
    /// <summary>
    /// 加载KCS文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>KCS文件内容</returns>
    Task<KcsFileFormat?> LoadKcsFileAsync(string filePath);

    /// <summary>
    /// 保存KCS文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="kcs">KCS文件内容</param>
    Task SaveKcsFileAsync(string filePath, KcsFileFormat kcs);
}

/// <summary>
/// 主程序代码分析器接口
/// </summary>
public interface IMainProgramAnalyzer
{
    /// <summary>
    /// 分析主程序代码
    /// </summary>
    /// <param name="code">要分析的代码</param>
    /// <returns>分析结果</returns>
    MainProgramAnalysisResult Analyze(string code);
}
