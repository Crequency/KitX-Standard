using System;

namespace KitX.Shared.CSharp.Device;

public class DeviceInfo
{
    public DeviceLocator Device { get; set; } = new();

    public string DeviceOSVersion { get; set; } = string.Empty;

    public int PluginsServerPort { get; set; }

    public int PluginsCount { get; set; }

    public DateTime SendTime { get; set; }

    public bool IsMainDevice { get; set; }

    public int DevicesServerPort { get; set; }

    public DateTime DevicesServerBuildTime { get; set; }

    public OperatingSystems DeviceOSType { get; set; }
}
