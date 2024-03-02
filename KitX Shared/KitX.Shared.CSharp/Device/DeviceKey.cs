namespace KitX.Shared.CSharp.Device;

public class DeviceKey
{
    public DeviceLocator Device { get; set; } = new();

    public string? RsaPublicKeyModulus { get; set; }

    public string? RsaPublicKeyExponent { get; set; }

    public string? RsaPrivateKeyModulus { get; set; }

    public string? RsaPrivateKeyD { get; set; }
}
