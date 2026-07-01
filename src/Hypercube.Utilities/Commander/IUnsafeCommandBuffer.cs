namespace Hypercube.Utilities.Commander;

/// <summary>
/// Represents a dynamically resizable, unmanaged command buffer used for batching render commands.
/// </summary>
/// <remarks>
/// <para>This class is designed for high-performance, single-threaded command recording and playback.</para>
/// <para>It is <b>not thread-safe</b>. Synchronization must be handled externally if used across threads.</para>
/// </remarks>
public unsafe interface IUnsafeCommandBuffer : IDisposable
{
    int Capacity { get; }
    int Length { get; }

    /// <summary>
    /// Pushes a command into the buffer. Automatically resizes the underlying memory if capacity is exceeded.
    /// </summary>
    /// <typeparam name="T">The unmanaged command type. Must be a struct with no references.</typeparam>
    /// <param name="command">The command instance to push.</param>
    /// <param name="type">The render command type identifier.</param>
    void Push<T>(T command, ushort type)
        where T : unmanaged;

    /// <summary>
    /// Attempts to read the next command from the buffer in FIFO order.
    /// </summary>
    /// <param name="type">When this method returns, contains the command type, or default if no command is available.</param>
    /// <param name="data">When this method returns, contains a pointer to the command data, or null if no command is available.</param>
    /// <returns><c>true</c> if a command was successfully read; otherwise, <c>false</c>.</returns>
    bool TryGetNext(out ushort type, out void* data);

    /// <summary>
    /// Resets the read and write offsets to zero, allowing the buffer to be reused without freeing memory.
    /// </summary>
    /// <remarks>
    /// This method does not clear the underlying memory. It is optimized for performance in rendering loops.
    /// </remarks>
    void Reset();
}
