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

    /// <summary>
    /// Maximum number of log entries retained per Log panel control. When a control
    /// appends past this cap the oldest entries are trimmed so the on-screen list stays
    /// bounded. Read at startup and by the Panel host when building a control.
    /// </summary>
    int PanelLogLimit { get; set; }
}
