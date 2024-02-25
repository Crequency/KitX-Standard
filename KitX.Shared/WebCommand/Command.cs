using KitX.Shared.Plugin;
using System;
using System.Collections.Generic;

namespace KitX.Shared.WebCommand;

public struct Command
{
    public DateTime SendTime { get; set; }

    public string Request { get; set; }

    public byte[] Body { get; set; }

    public int BodyLength { get; set; }

    public string FunctionName { get; set; }

    public List<Parameter> FunctionArgs { get; set; }

    public Dictionary<string, string> Tags { get; set; }
}
