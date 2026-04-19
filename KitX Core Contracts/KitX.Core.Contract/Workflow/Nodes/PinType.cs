namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Pin type for data flow
/// </summary>
public enum PinType
{
    /// <summary>
    /// Execution flow pin - green
    /// </summary>
    Execution,

    /// <summary>
    /// Boolean pin - cyan
    /// </summary>
    Boolean,

    /// <summary>
    /// Integer pin - orange
    /// </summary>
    Integer,

    /// <summary>
    /// Double pin - purple
    /// </summary>
    Double,

    /// <summary>
    /// String pin - yellow
    /// </summary>
    String,

    /// <summary>
    /// Any type pin - white
    /// </summary>
    Any
}