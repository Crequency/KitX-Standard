﻿using System.Collections.Generic;

namespace KitX.Shared.CSharp.Plugin;

public struct Parameter
{
    public string Name { get; set; }

    public Dictionary<string, string> DisplayNames { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public bool IsAppendable { get; set; }
}
