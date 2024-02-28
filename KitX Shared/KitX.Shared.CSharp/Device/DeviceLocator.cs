namespace KitX.Shared.CSharp.Device;

public class DeviceLocator
{
    public string DeviceName { get; set; } = string.Empty;

    public string IPv4 { get; set; } = string.Empty;

    public string IPv6 { get; set; } = string.Empty;

    public string MacAddress { get; set; } = string.Empty;

    public DeviceLocator ResetDeviceName(string name)
    {
        DeviceName = name;
        return this;
    }

    public DeviceLocator ResetIPv4(string ipv4)
    {
        IPv4 = ipv4;
        return this;
    }

    public DeviceLocator ResetIPv6(string ipv6)
    {
        IPv6 = ipv6;
        return this;
    }

    public DeviceLocator ResetMacAddress(string mac)
    {
        MacAddress = mac;
        return this;
    }
}
