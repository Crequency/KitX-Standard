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
}
