namespace Hypercube.Utilities.Profiling;

/// <summary>
/// A simple and efficient profiler for measuring execution time, memory usage, CPU usage, and method call counts.
/// Supports nested profiling groups and customizable units of measurement.
/// </summary>
public interface IProfiler
{
    /// <summary>
    /// Starts a new profiling group.
    /// </summary>
    /// <param name="groupName">The name of the profiling group.</param>
    /// <param name="unit">The unit of time measurement (default is milliseconds).</param>
    /// <param name="metrics">The metrics to measure (default is Time).</param>
    /// <returns>A disposable profiling group.</returns>
    ProfilerGroup Group(string groupName, TimeUnit unit = TimeUnit.Milliseconds, Metrics metrics = Metrics.Time);
}
