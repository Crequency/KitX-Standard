namespace KitX.Shared.Device;

public struct DeviceLocator
{
    public string DeviceName { get; set; }

    public string IPv4 { get; set; }

    public string IPv6 { get; set; }

    public string MacAddress { get; set; }

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
