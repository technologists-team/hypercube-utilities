using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Threads;

/// <summary>
/// ThreadBridge is designed to pass events between threads
/// using a strongly typed, blocking channel mechanism.
/// </summary>
/// <seealso cref="ChannelWriter{T}"/>
/// <seealso cref="ChannelReader{T}"/>
[PublicAPI]
public class ThreadBridge<T> : IEnumerable<T> where T : notnull
{
    /// <summary>
    /// Writer is the channel's writer, used to publish events to the bridge.
    /// </summary>
    private ChannelWriter<T> Writer { get; }
    
    /// <summary>
    /// Reader is the channel's reader, used to consume events from the bridge.
    /// </summary>
    private ChannelReader<T> Reader { get; }

    /// <summary>
    /// Initializes a new instance of the ThreadBridge using bounded channel options.
    /// Bounded channels limit the number of items that can be written without blocking.
    /// </summary>
    /// <param name="options">Options for configuring the bounded channel.</param>
    public ThreadBridge(BoundedChannelOptions options)
    {
        var channel = Channel.CreateBounded<T>(options);
        Writer = channel.Writer;
        Reader = channel.Reader;
    }

    /// <summary>
    /// Initializes a new instance of the ThreadBridge using unbounded channel options.
    /// Unbounded channels allow unlimited items, which may cause memory issues if overused.
    /// </summary>
    /// <param name="options">Options for configuring the unbounded channel.</param>
    public ThreadBridge(UnboundedChannelOptions options)
    {
        var channel = Channel.CreateUnbounded<T>(options);
        Writer = channel.Writer;
        Reader = channel.Reader;
    }

    /// <summary>
    /// Blocks the current thread until an item is available to read from the channel.
    /// Useful for synchronization in multi-threaded environments.
    /// </summary>
    public void Wait()
    {
        Reader.WaitToReadAsync().AsTask().Wait();
    }

    /// <summary>
    /// Publishes an event to the bridge.
    /// </summary>
    /// <param name="eventMessage">The event to publish.</param>
    public bool Raise(T eventMessage)
    {
        return Writer.TryWrite(eventMessage);
    }

    /// <summary>
    /// Subscribes to events of the specified type.
    /// Blocks until an event is available.
    /// </summary>
    /// <remarks>
    /// May cause SOH 1 issues if called frequently, it is better to use <see cref="TryRead"/>.
    /// </remarks>
    /// <typeparam name="T">The type of event to subscribe to.</typeparam>
    /// <returns>An enumerable that yields events of the specified type.</returns>
    /// <seealso cref="TryRead"/>
    public IEnumerable<T> Process()
    {
        while (Reader.TryRead(out var eventMessage))
        {
            yield return eventMessage;
        }
    }

    /// <summary>
    /// Attempts to read a single event from the channel without blocking.
    /// </summary>
    /// <param name="ev">The output parameter to store the event if one is available.</param>
    /// <returns>True if an event was read; otherwise, false.</returns>
    public bool TryRead([MaybeNullWhen(false)] out T ev)
    {
        return Reader.TryRead(out ev);
    }
    
    /// <summary>
    /// Signals that no more items will be written to the channel.
    /// After calling this, no new events can be raised.
    /// </summary>
    public void CompleteWrite()
    {
        Writer.Complete();
    }

    /// <summary>
    /// Gets an enumerator to iterate over the events in the channel.
    /// Enumerates over events using the <see cref="Process"/> method.
    /// </summary>
    /// <returns>An enumerator for the events in the channel.</returns>
    [MustDisposeResource]
    public IEnumerator<T> GetEnumerator()
    {
        return Process().GetEnumerator();
    }

    /// <summary>
    /// Gets a non-generic enumerator to iterate over the events in the channel.
    /// Implements <see cref="IEnumerable"/> for compatibility.
    /// </summary>
    /// <returns>A non-generic enumerator for the events in the channel.</returns>
    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}