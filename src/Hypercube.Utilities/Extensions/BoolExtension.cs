using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

[PublicAPI]
public static class BoolExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}