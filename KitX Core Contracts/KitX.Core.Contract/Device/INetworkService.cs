using System.Threading;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Device;

/// <summary>
/// Unified orchestrator for the device network stack (discovery UDP server,
/// device HTTP server, plugin WebSocket server). Owns startup ordering, port
/// configuration and shutdown. Implemented in KitX.Core.
/// </summary>
public interface INetworkService
{
    /// <summary>Starts all network servers asynchronously (honors DelayStartSeconds and SkipNetworkSystemOnStartup).</summary>
    Task StartAsync(CancellationToken ct = default);

    /// <summary>Stops all running network servers.</summary>
    Task StopAsync(CancellationToken ct = default);

    /// <summary>
    /// Stops and restarts the device discovery + device HTTP servers only
    /// (plugin server untouched). Waits for UDP sockets to settle before restarting.
    /// </summary>
    Task RestartDevicesServersAsync(CancellationToken ct = default);

    /// <summary>Stops the device discovery + device HTTP servers only (plugin server untouched).</summary>
    Task StopDevicesServersAsync(CancellationToken ct = default);

    /// <summary>True when at least one server is running.</summary>
    bool IsRunning { get; }
}
