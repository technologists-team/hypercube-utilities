using System.Reflection;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public static class AssemblyHelper
{
    public static readonly string Title = GetAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
    public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
    public static readonly string Configuration = GetAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? string.Empty;
    
    public static T? GetAttribute<T>() where T : Attribute
    {
        return (T?) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
    }
}