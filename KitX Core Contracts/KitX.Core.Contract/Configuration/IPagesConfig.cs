namespace KitX.Core.Contract.Configuration;

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
