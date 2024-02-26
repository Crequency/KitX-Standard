using System;
using System.Collections.Generic;

namespace KitX.Shared.Plugin;

public class PluginInfo
{
    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public Dictionary<string, string> DisplayName { get; set; } = [];

    public string AuthorName { get; set; } = string.Empty;

    public string AuthorLink { get; set; } = string.Empty;

    public string PublisherName { get; set; } = string.Empty;

    public string PublisherLink { get; set; } = string.Empty;

    public Dictionary<string, string> SimpleDescription { get; set; } = [];

    public Dictionary<string, string> ComplexDescription { get; set; } = [];

    public Dictionary<string, string> TotalDescriptionInMarkdown { get; set; } = [];

    public string IconInBase64 { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public bool IsMarketVersion { get; set; }

    public Dictionary<string, string> Tags { get; set; } = [];

    public List<Function> Functions { get; set; } = [];

    public string RootStartupFileName { get; set; } = string.Empty;
}
