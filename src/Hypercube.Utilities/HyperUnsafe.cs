using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities;

[PublicAPI]
public static class HyperUnsafe
{
    private static readonly MethodInfo SizeOfMethod =
        typeof(Unsafe).GetMethod(nameof(Unsafe.SizeOf))!;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult AsUnmanaged<TValue, TResult>(in TValue value) where TValue : unmanaged where TResult : unmanaged
    {
        fixed (TValue* ptr = &value)
            return *(TResult*)ptr;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult AsManaged<TValue, TResult>(ref TValue value) where TValue : class where TResult : class
    {
        return Unsafe.As<TValue, TResult>(ref value);
    }

    public static int SizeOf(Type type)
    {
        if (!type.IsValueType)
            return nint.Size;
        
        return (int) SizeOfMethod
            .MakeGenericMethod(type)
            .Invoke(null, null)!;
    }
    
    public static int SizeOf<T>()
    {
        return typeof(T).IsValueType
            ? Unsafe.SizeOf<T>()
            : nint.Size;
    }
}
