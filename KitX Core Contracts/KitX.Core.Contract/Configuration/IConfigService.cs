using System;
using System.Collections.Generic;
using Common.BasicHelper.Graphics.Screen;
using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.Loader;
using KitX.Shared.CSharp.Plugin;
using Serilog.Events;

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

/// <summary>
/// Configuration management service interface
/// </summary>
public interface IConfigService
{
    /// <summary>
    /// Gets the application configuration
    /// </summary>
    IAppConfig AppConfig { get; }

    /// <summary>
    /// Gets the plugins configuration
    /// </summary>
    IPluginsConfig PluginsConfig { get; }

    /// <summary>
    /// Gets the security configuration
    /// </summary>
    ISecurityConfig SecurityConfig { get; }

    /// <summary>
    /// Loads all configurations from files
    /// </summary>
    void Load();

    /// <summary>
    /// Saves all configurations to files
    /// </summary>
    void SaveAll();

    /// <summary>
    /// Reloads all configurations from files
    /// </summary>
    void Reload();

    /// <summary>
    /// Event raised when configuration changes
    /// </summary>
    event EventHandler<ConfigChangedEventArgs>? ConfigChanged;
}

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

/// <summary>
/// Pages configuration section
/// </summary>
public interface IPagesConf
{
    IHomePageConf Home { get; set; }
    IDevicePageConf Device { get; set; }
    IMarketPageConf Market { get; set; }
    ISettingsPageConf Settings { get; set; }
}

/// <summary>
/// Home page configuration
/// </summary>
public interface IHomePageConf
{
    NavigationViewPaneDisplayMode NavigationViewPaneDisplayMode { get; set; }
    string SelectedViewName { get; set; }
    bool IsNavigationViewPaneOpened { get; set; }
    bool UseAreaExpanded { get; set; }
}

/// <summary>
/// Device page configuration
/// </summary>
public interface IDevicePageConf { }

/// <summary>
/// Market page configuration
/// </summary>
public interface IMarketPageConf { }

/// <summary>
/// Settings page configuration
/// </summary>
public interface ISettingsPageConf
{
    NavigationViewPaneDisplayMode NavigationViewPaneDisplayMode { get; set; }
    string SelectedViewName { get; set; }
    bool PaletteAreaExpanded { get; set; }
    bool WebRelatedAreaExpanded { get; set; }
    bool WebRelatedAreaOfNetworkInterfacesExpanded { get; set; }
    bool LogRelatedAreaExpanded { get; set; }
    bool UpdateRelatedAreaExpanded { get; set; }
    bool AboutAreaExpanded { get; set; }
    bool AuthorsAreaExpanded { get; set; }
    bool LinksAreaExpanded { get; set; }
    bool ThirdPartyLicensesAreaExpanded { get; set; }
    bool IsNavigationViewPaneOpened { get; set; }
}

/// <summary>
/// Web configuration section
/// </summary>
public interface IWebConf
{
    double DelayStartSeconds { get; set; }
    string ApiServer { get; set; }
    string ApiPath { get; set; }
    int DevicesViewRefreshDelay { get; set; }
    List<string>? AcceptedNetworkInterfaces { get; set; }
    int? UserSpecifiedDevicesServerPort { get; set; }
    int? UserSpecifiedPluginsServerPort { get; set; }
    int UdpPortSend { get; set; }
    int UdpPortReceive { get; set; }
    int UdpSendFrequency { get; set; }
    string UdpBroadcastAddress { get; set; }
    string IPFilter { get; set; }
    int SocketBufferSize { get; set; }
    int DeviceInfoTTLSeconds { get; set; }
    bool DisableRemovingOfflineDeviceCard { get; set; }
    string UpdateServer { get; set; }
    string UpdatePath { get; set; }
    string UpdateDownloadPath { get; set; }
    string UpdateChannel { get; set; }
    string UpdateSource { get; set; }
    int DebugServicesServerPort { get; set; }
}

/// <summary>
/// Log configuration section
/// </summary>
public interface ILogConf
{
    long LogFileSingleMaxSize { get; set; }
    string LogFilePath { get; set; }
    string LogTemplate { get; set; }
    int LogFileMaxCount { get; set; }
    int LogFileFlushInterval { get; set; }
    public LogEventLevel LogLevel { get; set; }
}

/// <summary>
/// IO configuration section
/// </summary>
public interface IIOConf
{
    int UpdatingCheckPerThreadFilesCount { get; set; }
    int OperatingSystemVersionUpdateInterval { get; set; }
}

/// <summary>
/// Activity configuration section
/// </summary>
public interface IActivityConf
{
    int TotalRecorded { get; set; }
}

/// <summary>
/// Loaders configuration section
/// </summary>
public interface ILoadersConf
{
    string InstallPath { get; set; }
}

public interface IAnnouncementConfig
{
    /// <summary>
    /// Gets or sets the list of accepted announcement IDs
    /// </summary>
    List<string> Accepted { get; set; }

    /// <summary>
    /// Gets or sets the config file location
    /// </summary>
    string? ConfigFileLocation { get; set; }
}

/// <summary>
/// Plugins configuration interface
/// </summary>
public interface IPluginsConfig
{
    /// <summary>
    /// Gets or sets the list of plugin installations
    /// </summary>
    IList<IPluginInstallation> Plugins { get; set; }
}

/// <summary>
/// Security configuration interface
/// </summary>
public interface ISecurityConfig
{
    /// <summary>
    /// Gets or sets the device keys dictionary
    /// </summary>
    IDictionary<string, IDeviceKey> DeviceKeys { get; set; }
}

/// <summary>
/// Plugin installation interface
/// </summary>
public interface IPluginInstallation
{
    /// <summary>
    /// Gets the installation path
    /// </summary>
    string? InstallPath { get; }

    /// <summary>
    /// Gets or sets the plugin information
    /// </summary>
    PluginInfo? PluginInfo { get; set; }

    /// <summary>
    /// Gets or sets the loader information
    /// </summary>
    LoaderInfo? LoaderInfo { get; set; }

    /// <summary>
    /// Gets or sets the list of installed devices
    /// </summary>
    IList<DeviceLocator> InstalledDevices { get; set; }
}

/// <summary>
/// Device key interface
/// </summary>
public interface IDeviceKey
{
    /// <summary>
    /// Gets the MAC address
    /// </summary>
    string MacAddress { get; }

    /// <summary>
    /// Gets the device name
    /// </summary>
    string DeviceName { get; }

    /// <summary>
    /// Gets the public key
    /// </summary>
    string PublicKey { get; }

    /// <summary>
    /// Gets the time when the key was added
    /// </summary>
    DateTime AddedAt { get; }
}

/// <summary>
/// Navigation view pane display mode enum
/// </summary>
public enum NavigationViewPaneDisplayMode
{
    Auto = 0,
    Left = 1,
    Top = 2,
    LeftCompact = 3,
    LeftMinimal = 4
}

/// <summary>
/// Configuration changed event arguments
/// </summary>
public class ConfigChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the configuration type (e.g., "App", "Plugins", "Security")
    /// </summary>
    public string ConfigType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the property name that changed
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the old value
    /// </summary>
    public object? OldValue { get; set; }

    /// <summary>
    /// Gets or sets the new value
    /// </summary>
    public object? NewValue { get; set; }
}

