using KitX.Shared.Device;
using System.Collections.Generic;

namespace KitX.Shared.Loader;

public class LoaderInfo
{
    public bool SelfLoad { get; set; }

    public string LoaderName { get; set; } = string.Empty;

    public string LoaderVersion { get; set; } = string.Empty;

    public string LoaderLanguage { get; set; } = string.Empty;

    public string LoaderFramework { get; set; } = string.Empty;

    public LoaderRunType LoaderRunType { get; set; }

    public List<OperatingSystems> SupportOS { get; set; } = [];

    public Dictionary<string, string> Tags { get; set; } = [];
}

public enum LoaderRunType
{
    Console, Desktop, Browser
}
