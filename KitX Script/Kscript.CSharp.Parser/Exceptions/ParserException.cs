namespace Kscript.CSharp.Parser.Exceptions;

/// <summary>
/// 解析器异常
/// </summary>
public class ParserException : Exception
{
    public ParserException() : base()
    {
    }

    public ParserException(string message) : base(message)
    {
    }

    public ParserException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// 创建类型映射异常
    /// </summary>
    public static ParserException TypeMappingError(string typeName, Exception? innerException = null)
    {
        return new ParserException($"无法映射类型: {typeName}", innerException!);
    }

    /// <summary>
    /// 创建IL生成异常
    /// </summary>
    public static ParserException ILGenerationError(string methodName, Exception? innerException = null)
    {
        return new ParserException($"生成IL代码失败: {methodName}", innerException!);
    }

    /// <summary>
    /// 创建程序集生成异常
    /// </summary>
    public static ParserException AssemblyGenerationError(string assemblyName, Exception? innerException = null)
    {
        return new ParserException($"生成程序集失败: {assemblyName}", innerException!);
    }
}
