using KitX.Shared.CSharp.Device;
using KitX.Shared.CSharp.WebCommand.Infos;

namespace KitX.Shared.CSharp.WebCommand;

public struct Request
{
    public string Type { get; set; }

    public string Version { get; set; }

    public DeviceLocator Sender { get; set; }

    public DeviceLocator Target { get; set; }

    public EncryptionInfo EncryptionInfo { get; set; }

    public CompressionInfo CompressionInfo { get; set; }

    public string Content { get; set; }
}
