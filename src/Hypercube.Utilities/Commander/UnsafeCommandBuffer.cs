using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hypercube.Utilities.Commander;

/// <inheritdoc />
public sealed unsafe class UnsafeCommandBuffer : IUnsafeCommandBuffer
{
    private byte* _buffer;
    private int _readOffset;
    private bool _disposed;

    public int Capacity { get; private set; }
    public int Length { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsafeCommandBuffer"/> with a default capacity of 256 bytes.
    /// </summary>
    public UnsafeCommandBuffer() : this(256)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsafeCommandBuffer"/> with a specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity in bytes. Must be greater than zero.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="capacity"/> is less than or equal to zero.</exception>
    public UnsafeCommandBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

        Capacity = capacity;
        _buffer = Allocate(capacity);
    }
    
    /// <inheritdoc />
    public void Push<T>(T command, ushort type) where T : unmanaged
    {
        ThrowIfDisposed();

        var commandSize = sizeof(T);
        var totalSize = sizeof(UnsafeCommandHeader) + commandSize;

        if (Length + totalSize > Capacity)
            Grow(totalSize);

        var header = new UnsafeCommandHeader { Type = type, Size = commandSize };

        // Write header and payload using unaligned access to avoid alignment faults and improve performance
        Unsafe.WriteUnaligned(_buffer + Length, header);
        Length += sizeof(UnsafeCommandHeader);

        Unsafe.WriteUnaligned(_buffer + Length, command);
        Length += commandSize;
    }

    /// <inheritdoc />
    public bool TryGetNext(out ushort type, out void* data)
    {
        ThrowIfDisposed();
        
        type = 0;
        data = null;

        if (_readOffset >= Length)
            return false;

        var header = Unsafe.ReadUnaligned<UnsafeCommandHeader>(_buffer + _readOffset);
        _readOffset += sizeof(UnsafeCommandHeader);

        // Validate payload bounds to prevent reading corrupted or out-of-range data
        if (_readOffset + header.Size > Length)
            throw new InvalidOperationException("Command buffer is corrupted: header size exceeds available data.");

        type = header.Type;
        data = _buffer + _readOffset;
        _readOffset += header.Size;

        return true;
    }

    /// <inheritdoc />
    public void Reset()
    {
        ThrowIfDisposed();
        
        Length = 0;
        _readOffset = 0;
    }
    
    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;
        
        if (_buffer is not null)
        {
            NativeMemory.Free(_buffer);
            _buffer = null;
        }

        Capacity = 0;
        Length = 0;
        _readOffset = 0;
        _disposed = true;
    }

    /// <summary>
    /// Dynamically increases the buffer capacity to accommodate the requested size.
    /// </summary>
    /// <param name="requiredSize">The minimum additional size needed.</param>
    private void Grow(int requiredSize)
    {
        Debug.Assert(Capacity > 0);

        var newCapacity = Capacity;
        while (Length + requiredSize > newCapacity)
            newCapacity *= 2;

        var newBuffer = Allocate(newCapacity);

        if (Length > 0)
            Buffer.MemoryCopy(_buffer, newBuffer, newCapacity, Length);

        if (_buffer is not null)
            NativeMemory.Free(_buffer);

        _buffer = newBuffer;
        Capacity = newCapacity;
    }

    private static byte* Allocate(int capacity)
    {
        return (byte*) NativeMemory.Alloc((nuint) capacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ThrowIfDisposed()
    {
        if (!_disposed)
            return;
        
        throw new ObjectDisposedException(nameof(UnsafeCommandBuffer));
    }
}
