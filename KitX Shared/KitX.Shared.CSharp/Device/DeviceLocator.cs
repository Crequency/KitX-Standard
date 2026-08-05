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

    public override bool Equals(object? obj)
    {
        if (obj is not DeviceLocator target)
            return false;

        var result = DeviceName.Equals(target.DeviceName) &&
            IPv4.Equals(target.IPv4) &&
            IPv6.Equals(target.IPv6) &&
            MacAddress.Equals(target.MacAddress)
            ;

        return result;
    }

    public override int GetHashCode()
    {
        var hash = Prime5;
        hash = MixFinal(hash + (uint)DeviceName.GetHashCode());
        hash = MixFinal(hash + (uint)IPv4.GetHashCode());
        hash = MixFinal(hash + (uint)IPv6.GetHashCode());
        hash = MixFinal(hash + (uint)MacAddress.GetHashCode());
        return (int)hash;
    }

    private const uint Prime2 = 2246822519U;

    private const uint Prime3 = 3266489917U;

    private const uint Prime5 = 374761393U;

    private static uint MixFinal(uint hash)
    {
        hash ^= hash >> 15;
        hash *= Prime2;
        hash ^= hash >> 13;
        hash *= Prime3;
        hash ^= hash >> 16;
        return hash;
    }
}

public static class DeviceLocatorExtensions
{
    public static bool IsSameDevice(this DeviceLocator current, DeviceLocator target)
        => current.DeviceName.Equals(target.DeviceName)
        && current.MacAddress.Equals(target.MacAddress);
}
