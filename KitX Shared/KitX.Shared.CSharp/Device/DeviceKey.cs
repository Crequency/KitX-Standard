namespace KitX.Shared.CSharp.Device;

public class DeviceKey
{
    public DeviceLocator Device { get; set; } = new();

    //[Obsolete]
    //public string? RsaPublicKeyModulus { get; set; }

    //[Obsolete]
    //public string? RsaPublicKeyExponent { get; set; }

    //[Obsolete]
    //public string? RsaPrivateKeyModulus { get; set; }

    //[Obsolete]
    //public string? RsaPrivateKeyD { get; set; }

    public string? RsaPublicKeyPem { get; set; }

    public string? RsaPrivateKeyPem { get; set; }
}

public static class DeviceKeyExtensions
{
    public static bool IsSameKey(this DeviceKey key, DeviceKey other)
    {
        var result = true;

        if (key.RsaPublicKeyPem is not null && other.RsaPublicKeyPem is not null)
            result = result && key.RsaPublicKeyPem == other.RsaPublicKeyPem;

        if (key.RsaPrivateKeyPem is not null && other.RsaPrivateKeyPem is not null)
            result = result && key.RsaPrivateKeyPem == other.RsaPrivateKeyPem;

        //if (key.RsaPublicKeyExponent is not null && key.RsaPublicKeyModulus is not null)
        //    result = result && key.RsaPublicKeyExponent == other.RsaPublicKeyExponent && key.RsaPublicKeyModulus == other.RsaPublicKeyModulus;

        //if (key.RsaPrivateKeyD is not null && key.RsaPrivateKeyModulus is not null)
        //    result = result && key.RsaPrivateKeyD == other.RsaPrivateKeyD && key.RsaPrivateKeyModulus == other.RsaPrivateKeyModulus;

        return result && key.Device.IsSameDevice(other.Device);
    }
}
