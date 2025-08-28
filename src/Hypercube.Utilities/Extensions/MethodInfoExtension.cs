using System.Reflection;
using System.Runtime.CompilerServices;

namespace Hypercube.Utilities.Extensions;

public static class MethodInfoExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasCustomAttribute<T>(this MethodInfo method)
        where T : Attribute
    {
        return method.GetCustomAttributes(typeof(T), false).Length != 0;
    }
}