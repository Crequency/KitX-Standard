using System.Collections.Generic;
using Common.BasicHelper.Graphics.Screen;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Windows configuration section
/// </summary>
public interface IWindowsConf
{
    IMainWindowConf MainWindow { get; set; }
    IAnnouncementWindowConf AnnouncementWindow { get; set; }
}

/// <summary>
/// Main window configuration
/// </summary>
public interface IMainWindowConf
{
    Resolution Size { get; set; }
    Distances Location { get; set; }
    WindowState WindowState { get; set; }
    bool IsHidden { get; set; }
    Dictionary<string, string> Tags { get; set; }
    bool EnabledMica { get; set; }
    int GreetingTextCount_Morning { get; set; }
    int GreetingTextCount_Noon { get; set; }
    int GreetingTextCount_AfterNoon { get; set; }
    int GreetingTextCount_Evening { get; set; }
    int GreetingTextCount_Night { get; set; }
    int GreetingUpdateInterval { get; set; }
}

/// <summary>
/// Announcement window configuration
/// </summary>
public interface IAnnouncementWindowConf
{
    Resolution Size { get; set; }
    Distances Location { get; set; }
}
