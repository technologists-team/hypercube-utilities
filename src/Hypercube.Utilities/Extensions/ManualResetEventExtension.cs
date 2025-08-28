using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

[PublicAPI]
public static class ManualResetEventExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task AsTask(this ManualResetEvent ev)
    {
        return Task.Run(ev.WaitOne);
    }
}