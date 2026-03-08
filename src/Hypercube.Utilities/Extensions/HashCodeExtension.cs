using System.Runtime.InteropServices;

namespace Hypercube.Utilities.Extensions;

public static class HashCodeExtension
{
    public static HashCode AddSpan<T>(ref this HashCode context, ReadOnlySpan<T> span) where T : unmanaged
    {
        context.AddBytes(MemoryMarshal.AsBytes(span));
        return context;
    }
    
    public static HashCode AddSpan<T>(ref this HashCode context, Span<T> span) where T : unmanaged
    {
        context.AddBytes(MemoryMarshal.AsBytes(span));
        return context;
    }
}