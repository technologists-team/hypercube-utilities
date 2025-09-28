using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities;

public static class HyperUnsafe
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult AsUnmanaged<TValue, TResult>(in TValue value)
        where TValue : unmanaged
        where TResult : unmanaged
    {
        fixed (TValue* ptr = &value)
            return *(TResult*)ptr;
    }
    
    [PublicAPI]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult AsManaged<TValue, TResult>(ref TValue value)
        where TValue : class
        where TResult : class
    {
        return Unsafe.As<TValue, TResult>(ref value);
    }
}
