namespace Hypercube.Utilities.Profiling;

/// <summary>
/// Represents the result of a profiling group.
/// </summary>
public readonly struct ProfilerResult
{
    /// <summary>
    /// The name of the profiling group.
    /// </summary>
    public string GroupName { get; }

    /// <summary>
    /// The elapsed time in the specified unit.
    /// </summary>
    public double ElapsedTime { get; }

    /// <summary>
    /// The unit of time measurement.
    /// </summary>
    public TimeUnit Unit { get; }

    /// <summary>
    /// The memory used in bytes (if measured).
    /// </summary>
    public long? MemoryUsed { get; }

    /// <summary>
    /// The memory allocated in bytes (if measured).
    /// </summary>
    public long? Allocations { get; }

    /// <summary>
    /// The CPU time used in milliseconds (if measured).
    /// </summary>
    public double? CpuUsed { get; }

    public ProfilerResult(string groupName, double elapsedTime, TimeUnit unit, long? memoryUsed, long? allocations, double? cpuUsed)
    {
        GroupName = groupName;
        ElapsedTime = elapsedTime;
        Unit = unit;
        MemoryUsed = memoryUsed;
        Allocations = allocations;
        CpuUsed = cpuUsed;
    }
}