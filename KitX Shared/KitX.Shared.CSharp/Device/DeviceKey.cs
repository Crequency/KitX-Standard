namespace KitX.Shared.CSharp.Device;

public class DeviceKey
{
    public DeviceLocator Device { get; set; } = new();

    public string? RsaPublicKeyModulus { get; set; }

    public string? RsaPublicKeyExponent { get; set; }

    public string? RsaPrivateKeyModulus { get; set; }

    public string? RsaPrivateKeyD { get; set; }
}

public static class DeviceKeyExtensions
{
    public static bool IsSameKey(this DeviceKey key, DeviceKey other)
    {
        var result = true;

        if (key.RsaPublicKeyExponent is not null && key.RsaPublicKeyModulus is not null)
            result = result && key.RsaPublicKeyExponent == other.RsaPublicKeyExponent && key.RsaPublicKeyModulus == other.RsaPublicKeyModulus;

        if (key.RsaPrivateKeyD is not null && key.RsaPrivateKeyModulus is not null)
            result = result && key.RsaPrivateKeyD == other.RsaPrivateKeyD && key.RsaPrivateKeyModulus == other.RsaPrivateKeyModulus;

        return result && key.Device.IsSameDevice(other.Device);
    }
}
