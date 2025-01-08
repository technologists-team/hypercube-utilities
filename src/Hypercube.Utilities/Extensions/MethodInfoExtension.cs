using System.Reflection;

namespace Hypercube.Utilities.Extensions;

public static class MethodInfoExtension
{
    public static bool HasCustomAttribute<T>(this MethodInfo method)
        where T : Attribute
    {
        return method.GetCustomAttributes(typeof(T), false).Length != 0;
    }
}