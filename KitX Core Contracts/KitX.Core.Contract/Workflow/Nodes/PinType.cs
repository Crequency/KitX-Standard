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
    Any,

    /// <summary>
    /// Structured-data pin (System.Text.Json.JsonElement: Array/Object/scalar) - blue.
    /// The first-class type for collection/object values flowing from plugin returns and
    /// JSON functions (Package/List-Port-And-Json-Functions-Design.md §2.1). Distinct from Any
    /// (which is an untyped catch-all); Json declares "this is structured data".
    /// </summary>
    Json
}