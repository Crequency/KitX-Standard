using System;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Window state enumeration (mirrors Avalonia.WindowState).
/// <para>
/// This is a UI-layer mapping enum: it mirrors the Avalonia UI framework's
/// <c>Avalonia.Controls.WindowState</c> so the UI layer (KitX Dashboard) can
/// serialize/deserialize window state without leaking a UI framework reference
/// into the contract layer. KitX Dashboard has converters depending on this type
/// — do not delete.
/// </para>
/// </summary>
public enum WindowState
{
    Normal,
    Minimized,
    Maximized,
    FullScreen,
    NonInteractive
}
