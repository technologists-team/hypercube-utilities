using System.Runtime.CompilerServices;
using Hypercube.Utilities.Extensions;

namespace Hypercube.Utilities.Unions.Extensions;

public static class UnionTypeExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnionTypeCode GetUnionTypeCode(this Type type)
    {
        return (UnionTypeCode) type.GetTypeCode();
    }
}