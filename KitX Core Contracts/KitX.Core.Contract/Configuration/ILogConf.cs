namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Log configuration section
/// </summary>
public interface ILogConf
{
    long LogFileSingleMaxSize { get; set; }
    string LogFilePath { get; set; }
    string LogTemplate { get; set; }
    int LogFileMaxCount { get; set; }
    int LogFileFlushInterval { get; set; }
    LogLevel LogLevel { get; set; }
}
