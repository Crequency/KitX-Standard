namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Marker/bridge interface for the concrete plugin manager implementation
/// used by the workflow subsystem.
///
/// Dashboard resolves this interface once at startup (via
/// GetRequiredService&lt;IRealPluginManagerBridge&gt;()) purely to force eager
/// singleton construction — the concrete RealPluginManager subscribes to plugin
/// message events in its constructor, so the instance must exist before plugins
/// connect. The interface itself carries no members; callers that need to
/// invoke plugin methods depend on the workflow library's own IPluginManager
/// contract instead.
/// </summary>
public interface IRealPluginManagerBridge
{
}
