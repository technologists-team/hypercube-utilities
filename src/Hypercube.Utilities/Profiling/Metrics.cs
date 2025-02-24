namespace Hypercube.Utilities.Profiling;

/// <summary>
/// Represents the metrics that can be measured by the profiler.
/// </summary>
[Flags]
public enum Metrics
{
    /// <summary>
    /// No metrics are measured.
    /// </summary>
    None = 0,

    /// <summary>
    /// Measure execution time.
    /// </summary>
    Time = 1 << 0,

    /// <summary>
    /// Measure memory usage.
    /// </summary>
    Memory = 1 << 1,

    /// <summary>
    /// Measure memory allocations.
    /// </summary>
    Allocations = 1 << 2,

    /// <summary>
    /// Measure CPU usage.
    /// </summary>
    Cpu = 1 << 3,

    /// <summary>
    /// Measure all available metrics.
    /// </summary>
    All = Time | Memory | Allocations | Cpu
}