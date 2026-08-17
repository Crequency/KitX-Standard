namespace KitX.Core.Contract.Event;

/// <summary>
/// Event names for the event bus
/// </summary>
public static class EventNames
{
    /// <summary>
    /// Language changed event
    /// </summary>
    public const string LanguageChanged = "LanguageChanged";

    /// <summary>
    /// Greeting text interval updated event
    /// </summary>
    public const string GreetingTextIntervalUpdated = "GreetingTextIntervalUpdated";

    /// <summary>
    /// App config changed event
    /// </summary>
    public const string AppConfigChanged = "AppConfigChanged";

    /// <summary>
    /// Develop settings changed event
    /// </summary>
    public const string DevelopSettingsChanged = "DevelopSettingsChanged";

    /// <summary>
    /// Log config updated event
    /// </summary>
    public const string LogConfigUpdated = "LogConfigUpdated";

    /// <summary>
    /// Theme config changed event
    /// </summary>
    public const string ThemeConfigChanged = "ThemeConfigChanged";

    /// <summary>
    /// Use statistics changed event
    /// </summary>
    public const string UseStatisticsChanged = "UseStatisticsChanged";

    /// <summary>
    /// Devices server port changed event
    /// </summary>
    public const string DevicesServerPortChanged = "DevicesServerPortChanged";

    /// <summary>
    /// Plugins server port changed event
    /// </summary>
    public const string PluginsServerPortChanged = "PluginsServerPortChanged";

    /// <summary>
    /// Exiting event
    /// </summary>
    public const string OnExiting = "OnExiting";

    /// <summary>
    /// Accepting device key event
    /// </summary>
    public const string OnAcceptingDeviceKey = "OnAcceptingDeviceKey";

    /// <summary>
    /// Receive exchange device key request event.
    /// Published when a key exchange request is received, requiring user confirmation.
    /// </summary>
    public const string OnReceiveExchangeDeviceKey = "OnReceiveExchangeDeviceKey";

    /// <summary>
    /// Plugin connected event
    /// </summary>

    /// <summary>
    /// Plugin disconnected event
    /// </summary>
    public const string PluginDisconnected = "PluginDisconnected";

    /// <summary>
    /// Plugin registered event
    /// </summary>
    public const string PluginRegistered = "PluginRegistered";

    /// <summary>
    /// Plugin unregistered event
    /// </summary>
    public const string PluginUnregistered = "PluginUnregistered";

    /// <summary>
    /// Plugin response event (has RequestId)
    /// </summary>
    public const string PluginResponse = "PluginResponse";

    /// <summary>
    /// Workflow data saved event
    /// </summary>
    public const string WorkflowDataSaved = "WorkflowDataSaved";

    /// <summary>
    /// Workflow execution result event (success or failure)
    /// </summary>
    public const string WorkflowExecutionResult = "WorkflowExecutionResult";
}
