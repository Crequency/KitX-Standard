namespace KitX.Core.Contract.Configuration;

/// <summary>
/// IO configuration section
/// </summary>
public interface IIOConf
{
    int UpdatingCheckPerThreadFilesCount { get; set; }
    int OperatingSystemVersionUpdateInterval { get; set; }
}
