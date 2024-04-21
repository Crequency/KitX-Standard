using System;

namespace KitX.Contract.CSharp.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class FunctionAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
