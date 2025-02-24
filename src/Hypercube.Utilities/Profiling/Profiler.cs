namespace Hypercube.Utilities.Profiling;

public class Profiler : IProfiler
{
    /// <summary>
    /// Starts a new profiling group.
    /// </summary>
    /// <param name="groupName">The name of the profiling group.</param>
    /// <param name="unit">The unit of time measurement (default is milliseconds).</param>
    /// <param name="metrics">The metrics to measure (default is Time).</param>
    /// <returns>A disposable profiling group.</returns>
    public ProfilerGroup Group(string groupName, TimeUnit unit = TimeUnit.Milliseconds, Metrics metrics = Metrics.Time)
    {
        return new ProfilerGroup(groupName, unit, metrics);
    }
}