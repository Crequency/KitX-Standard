using System;

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

    public override bool Equals(object obj)
    {
        if (obj is not DeviceLocator target)
            throw new InvalidOperationException($"Currently you can not compare {nameof(DeviceLocator)} with other types.");

        var result = DeviceName.Equals(target.DeviceName) &&
            IPv4.Equals(target.IPv4) &&
            IPv6.Equals(target.IPv6) &&
            MacAddress.Equals(target.MacAddress)
            ;

        return result;
    }

    public override int GetHashCode() => base.GetHashCode();
}
