using JetBrains.Annotations;

namespace Hypercube.Utilities.Configuration;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
public sealed class ConfigAttribute : Attribute
{
    public readonly string FileName;

    public ConfigAttribute(string fileName)
    {
        FileName = fileName;
    }
}
