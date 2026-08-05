namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Logging level for the KitX logger.
/// <para>
/// This is the contract-level logging level used by <see cref="ILogConf.LogLevel"/>.
/// It intentionally mirrors the numeric values of Serilog's
/// <c>Serilog.Events.LogEventLevel</c> (Verbose=0 … Fatal=5) so that pre-existing
/// config files containing the old integer values keep deserializing unchanged.
/// Implementations may map this enum onto their actual logging framework
/// (e.g., Serilog) without requiring a contract-level dependency on it.
/// </para>
/// </summary>
public enum LogLevel
{
    /// <summary>Verbose level (most detailed, e.g. tracing)</summary>
    Verbose = 0,

    /// <summary>Debug level</summary>
    Debug = 1,

    /// <summary>Information level (default)</summary>
    Information = 2,

    /// <summary>Warning level</summary>
    Warning = 3,

    /// <summary>Error level</summary>
    Error = 4,

    /// <summary>Fatal level (least detailed)</summary>
    Fatal = 5
}
