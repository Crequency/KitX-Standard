using System;

namespace KitX.Contract.CSharp.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class ParameterAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
