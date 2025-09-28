using System.Diagnostics;

namespace Hypercube.Utilities.Profiling;

/// <summary>
/// A delegate for handling profiling results.
/// </summary>
public delegate void ProfilerResultHandler(ProfilerResult result);


/// <summary>
/// Represents a profiling group that measures execution time, memory usage, CPU usage, and call counts.
/// Disposable to automatically stop profiling when the group goes out of scope.
/// </summary>
public readonly struct ProfilerGroup : IDisposable
{
    private readonly string _groupName;
    private readonly Stopwatch _stopwatch;
    private readonly TimeUnit _unit;
    private readonly Metrics _metrics;
    private readonly long _startMemory;
    private readonly long _startAllocations;
    private readonly TimeSpan _startCpuTime;
    
    /// <summary>
    /// An event or callback for handling profiling results.
    /// </summary>
    public static event ProfilerResultHandler? ResultHandler;
    
    /// <summary>
    /// Initializes a new profiling group.
    /// </summary>
    /// <param name="groupName">The name of the profiling group.</param>
    /// <param name="unit">The unit of time measurement.</param>
    /// <param name="metrics">The metrics to measure.</param>
    public ProfilerGroup(string groupName, TimeUnit unit, Metrics metrics)
    {
        _groupName = groupName;
        _unit = unit;
        _metrics = metrics;
        _stopwatch = Stopwatch.StartNew();

        _startMemory = 0;
        _startAllocations = 0;
        _startCpuTime = Process.GetCurrentProcess().TotalProcessorTime;

        if (_metrics.HasFlag(Metrics.Memory))
        {
            GC.Collect(); // Ensure clean memory measurement
            GC.WaitForPendingFinalizers();
            _startMemory = GC.GetTotalMemory(forceFullCollection: false);
        }

        if (_metrics.HasFlag(Metrics.Allocations))
        {
            _startAllocations = GC.GetAllocatedBytesForCurrentThread();
        }
    }

    /// <summary>
    /// Stops the profiling group and triggers the result handler.
    /// </summary>
    public void Dispose()
    {
        _stopwatch.Stop();

        var elapsedTime = GetElapsedTime(_stopwatch, _unit);
        
        long? memoryUsed = null;
        long? allocations = null;
        double? cpuUsed = null;

        if (_metrics.HasFlag(Metrics.Memory))
        {
            var endMemory = GC.GetTotalMemory(forceFullCollection: false);
            memoryUsed = endMemory - _startMemory;
        }

        if (_metrics.HasFlag(Metrics.Allocations))
        {
            var endAllocations = GC.GetAllocatedBytesForCurrentThread();
            allocations = endAllocations - _startAllocations;
        }

        if (_metrics.HasFlag(Metrics.Cpu))
        {
            var endCpuTime = Process.GetCurrentProcess().TotalProcessorTime;
            cpuUsed = (endCpuTime - _startCpuTime).TotalMilliseconds;
        }

        // Create the result object
        var result = new ProfilerResult(
            _groupName,
            elapsedTime,
            _unit,
            memoryUsed,
            allocations,
            cpuUsed
        );

        // Trigger the result handler
        ResultHandler?.Invoke(result);
    }

    private static double GetElapsedTime(Stopwatch stopwatch, TimeUnit unit)
    {
        return unit switch
        {
            TimeUnit.Ticks => stopwatch.ElapsedTicks,
            TimeUnit.Microseconds => stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1_000_000.0),
            TimeUnit.Milliseconds => stopwatch.ElapsedMilliseconds,
            TimeUnit.Seconds => stopwatch.Elapsed.TotalSeconds,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }
}
