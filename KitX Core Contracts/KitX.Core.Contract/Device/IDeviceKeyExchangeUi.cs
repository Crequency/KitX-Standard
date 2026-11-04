using System;
using System.Threading;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Device;

/// <summary>
/// UI abstraction for the device key-exchange flow. Implemented in the Dashboard
/// (opens the <c>ExchangeDeviceKeyWindow</c>); referenced from KitX.Core so the
/// initiating-side <see cref="IDeviceCase"/> commands can display the temporary
/// password without a UI dependency in Core.
/// </summary>
public interface IDeviceKeyExchangeUi
{
    /// <summary>
    /// Initiator side: display the temporary password on screen so the user can read
    /// it into the target device. The returned handle must be disposed to close the
    /// display once the exchange completes.
    /// </summary>
    /// <param name="password">The temporary password to display</param>
    IDisposable ShowPasswordForInitiator(string password);

    /// <summary>
    /// Receiver side: prompt the user to enter the temporary password read from the
    /// initiating device's screen. Returns the entered password, or <c>null</c> if the
    /// user cancels the exchange.
    /// </summary>
    /// <param name="requestingDeviceAddress">Address of the device requesting the exchange</param>
    /// <param name="ct">Cancellation token</param>
    Task<string?> PromptForPasswordAsync(string requestingDeviceAddress, CancellationToken ct = default);
}
