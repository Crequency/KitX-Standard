namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Performance configuration section
/// </summary>
public interface IPerformanceConf
{
    /// <summary>
    /// Maximum number of compiled workflow assemblies kept in the WorkflowV6
    /// ScriptCompiler in-memory LRU cache. Applies at startup.
    /// </summary>
    int ScriptCompilerCacheCapacity { get; set; }

    /// <summary>
    /// Maximum number of Completed instances retained by the ToolKit instance manager.
    /// Applies at startup.
    /// </summary>
    int CompletedInstanceCap { get; set; }
}
