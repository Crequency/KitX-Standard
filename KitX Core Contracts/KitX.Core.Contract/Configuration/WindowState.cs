using System;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Window state enumeration (mirrors Avalonia.WindowState)
/// </summary>
public enum WindowState
{
    Normal,
    Minimized,
    Maximized,
    FullScreen,
    NonInteractive
}
