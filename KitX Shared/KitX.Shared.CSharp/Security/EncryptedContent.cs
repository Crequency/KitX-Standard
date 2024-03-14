using KitX.Shared.CSharp.Device;

namespace KitX.Shared.CSharp.Security;

public class EncryptedContent
{
    public DeviceLocator? Device { get; set; }

    public string? RsaEncryptedAesKeyBase64 { get; set; }

    public string? AesEncryptedContentBase64 { get; set; }
}
