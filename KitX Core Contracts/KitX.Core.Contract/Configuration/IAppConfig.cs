using System.Collections.Generic;

namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Application configuration interface (complete structure)
/// </summary>
public interface IAppConfig
{
    /// <summary>
    /// Gets or sets the application configuration
    /// </summary>
    IAppConf App { get; set; }

    /// <summary>
    /// Gets or sets the windows configuration
    /// </summary>
    IWindowsConf Windows { get; set; }

    /// <summary>
    /// Gets or sets the pages configuration
    /// </summary>
    IPagesConf Pages { get; set; }

    /// <summary>
    /// Gets or sets the web configuration
    /// </summary>
    IWebConf Web { get; set; }

    /// <summary>
    /// Gets or sets the log configuration
    /// </summary>
    ILogConf Log { get; set; }

    /// <summary>
    /// Gets or sets the IO configuration
    /// </summary>
    IIOConf IO { get; set; }

    /// <summary>
    /// Gets or sets the activity configuration
    /// </summary>
    IActivityConf Activity { get; set; }

    /// <summary>
    /// Gets or sets the loaders configuration
    /// </summary>
    ILoadersConf Loaders { get; set; }
}

/// <summary>
/// Application configuration section
/// </summary>
public interface IAppConf
{
    string IconFileName { get; set; }
    string CoverIconFileName { get; set; }
    string AppLanguage { get; set; }
    string Theme { get; set; }
    string ThemeColor { get; set; }
    Dictionary<string, string> SurpportLanguages { get; set; }
    string LocalPluginsFileFolder { get; set; }
    string LocalPluginsDataFolder { get; set; }
    bool DeveloperSetting { get; set; }
    bool ShowAnnouncementWhenStart { get; set; }
    ulong RanTime { get; set; }
    int LastBreakAfterExit { get; set; }
}
