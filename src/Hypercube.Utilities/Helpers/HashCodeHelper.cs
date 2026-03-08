using Hypercube.Utilities.Extensions;

namespace Hypercube.Utilities.Helpers;

public static class HashCodeHelper
{
    public static int HashFromSpan<T>(ReadOnlySpan<T> span) where T : unmanaged
    {
        var context = new HashCode();
        context.AddSpan(span);
        return context.ToHashCode();
    }
    
    public static int HashFromSpan<T>(Span<T> span) where T : unmanaged
    {
        var context = new HashCode();
        context.AddSpan(span);
        return context.ToHashCode();
    }
}