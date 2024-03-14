namespace KitX.Shared.CSharp.WebCommand.Infos;

public class EncryptionInfo
{
    public bool IsEncrypted { get; set; } = false;

    public EncryptionMethods EncryptionMethod { get; set; } = EncryptionMethods.RSA;
}

public enum EncryptionMethods
{
    Custom = 0,
    RSA = 1,
    AES = 2,
}
