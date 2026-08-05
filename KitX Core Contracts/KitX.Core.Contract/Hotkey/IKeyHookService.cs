using System;

namespace KitX.Core.Contract.Hotkey;

/// <summary>
/// Key hook service interface for global hotkeys
/// </summary>
public interface IKeyHookService
{
    /// <summary>
    /// Starts the key hook
    /// </summary>
    void StartHook();

    /// <summary>
    /// Stops the key hook
    /// </summary>
    void StopHook();

    /// <summary>
    /// Registers a hotkey handler
    /// </summary>
    /// <param name="keysSequence">The keys sequence</param>
    /// <param name="handler">The handler</param>
    void RegisterHotKeyHandler(string keysSequence, Action handler);

    /// <summary>
    /// Unregisters a hotkey handler
    /// </summary>
    /// <param name="keysSequence">The keys sequence</param>
    void UnregisterHotKeyHandler(string keysSequence);
}
