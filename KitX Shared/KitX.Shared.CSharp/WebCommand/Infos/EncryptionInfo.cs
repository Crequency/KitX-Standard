﻿namespace KitX.Shared.CSharp.WebCommand.Infos;

public struct EncryptionInfo
{
    public bool IsEncrypted { get; set; }

    public string EncryptionMethod { get; set; }

    public string EncryptionKeyId { get; set; }
}
