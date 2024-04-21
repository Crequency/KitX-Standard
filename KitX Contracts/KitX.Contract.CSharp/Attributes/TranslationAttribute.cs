using System;

namespace KitX.Contract.CSharp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
public class TranslationAttribute(string field, string lang, string value) : Attribute
{
    public string Field { get; } = field;

    public string Language { get; } = lang;

    public string Value { get; } = value;
}
