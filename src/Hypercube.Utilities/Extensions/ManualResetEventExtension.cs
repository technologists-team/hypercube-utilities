namespace Hypercube.Utilities.Extensions;

public static class ManualResetEventExtension
{
    public static Task AsTask(this ManualResetEvent ev)
    {
        return Task.Run(ev.WaitOne);
    }
}