using System.Collections.Generic;

namespace KitX.Core.Contract.Configuration;

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

    /// <summary>The port the View protocol listens on; null or 0 means it does not listen.</summary>
    /// <remarks>
    /// Declared here because the desktop reads configuration through this interface, and a member
    /// added only to <c>Config_Web</c> would be invisible to every caller that takes the interface —
    /// which is all of them.
    /// </remarks>
    int? UserSpecifiedViewServerPort { get; set; }
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
}

