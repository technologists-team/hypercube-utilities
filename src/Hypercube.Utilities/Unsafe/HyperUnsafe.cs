using System.Runtime.CompilerServices;

namespace Hypercube.Utilities.Unsafe;

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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult AsManaged<TValue, TResult>(ref TValue value)
        where TValue : class
        where TResult : class
    {
        return System.Runtime.CompilerServices.Unsafe.As<TValue, TResult>(ref value);
    }
}