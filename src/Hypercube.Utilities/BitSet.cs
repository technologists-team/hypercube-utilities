using System.Runtime.CompilerServices;
using System.Text;

namespace Hypercube.Utilities;

/// <summary>
/// A bit set that supports an arbitrary number of bits beyond 64.
/// </summary>
public sealed class BitSet : IEquatable<BitSet>
{
    private const int BitsPerElement = 64;
    private const int MinSizeValue = 1;
    
    /// <summary>
    /// Gets the number of bits in the bitset.
    /// </summary>
    public readonly int Size;
    
    private readonly ulong[] _bits;

    /// <summary>
    /// Initializes a new instance of the <see cref="BitSet"/> class with the specified number of bits.
    /// </summary>
    /// <param name="size">The total number of bits.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if size is less than 1.</exception>
    public BitSet(int size)
    {
        if (size < MinSizeValue)
            throw new ArgumentOutOfRangeException(nameof(size), $"Size must be at least {MinSizeValue}.");

        Size = size;
        _bits = new ulong[(size + 63) / BitsPerElement];
    }
    
    /// <summary>
    /// Sets the bit at the specified index to 1.
    /// </summary>
    /// <param name="index">The bit index to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int index)
    {
        ValidateIndex(index);
        _bits[index / BitsPerElement] |= 1ul << (index % BitsPerElement);
    }

    /// <summary>
    /// Clears the bit at the specified index (sets it to 0).
    /// </summary>
    /// <param name="index">The bit index to clear.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(int index)
    {
        ValidateIndex(index);
        _bits[index / BitsPerElement] &= ~(1ul << (index % BitsPerElement));
    }

    /// <summary>
    /// Checks if the bit at the specified index is set (1).
    /// </summary>
    /// <param name="index">The bit index to check.</param>
    /// <returns>True if the bit is set, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(int index)
    {
        ValidateIndex(index);
        return (_bits[index / BitsPerElement] & (1ul << (index % BitsPerElement))) != 0;
    }

    /// <summary>
    /// Throws an exception if the index is out of bounds.
    /// Uses unsigned comparison for performance optimization.
    /// </summary>
    /// <param name="index">The bit index to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateIndex(int index)
    {
        if ((uint) index < (uint) Size)
            return;
        
        throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {Size - 1}.");
    }
    
    /// <summary>
    /// Checks if two BitSet objects are equal (bitwise comparison).
    /// </summary>
    /// <param name="other">Another BitSet to compare with.</param>
    /// <returns>True if both bitsets have the same size and identical bits.</returns>
    public bool Equals(BitSet? other)
    {
        if (other is null || Size != other.Size)
            return false;

        for (var i = 0; i < _bits.Length; i++)
        {
            if (_bits[i] != other._bits[i])
                return false;
        }

        return true;
    }

    /// <summary>
    /// Overrides the default Equals method for object comparison.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is BitSet other && Equals(other);
    }

    /// <summary>
    /// Computes a hash code for the BitSet.
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = Size;
            foreach (var value in _bits)
                hash = (hash * 31) ^ value.GetHashCode();
            
            return hash;
        }
    }
    
    /// <summary>
    /// Returns a string representation of the bitset, with the highest index on the left.
    /// </summary>
    /// <returns>A string of 1s and 0s representing the bitset.</returns>
    public override string ToString()
    {
        var builder = new StringBuilder(Size);
        for (var i = Size - 1; i >= 0; i--)
            builder.Append(Has(i) ? '1' : '0');
        return builder.ToString();
    }
    
    /// <summary>
    /// Applies a given bitwise operation between two bitsets.
    /// </summary>
    /// <param name="a">The first bitset.</param>
    /// <param name="b">The second bitset.</param>
    /// <param name="operation">The bitwise operation to apply.</param>
    /// <returns>A new bitset resulting from the operation.</returns>
    /// <exception cref="ArgumentException">Thrown if the sizes do not match.</exception>
    public static BitSet ApplyOperator(BitSet a, BitSet b, Func<ulong, ulong, ulong> operation)
    {
        if (a.Size != b.Size)
            throw new ArgumentException("BitSets must have the same size.");

        var result = new BitSet(a.Size);
        
        // Write the result without validation directly
        for (var i = 0; i < a._bits.Length; i++)
            result._bits[i] = operation(a._bits[i], b._bits[i]);

        return result;
    }
    
    /// <summary>
    /// Overloads the equality operator (==) to compare two <see cref="BitSet"/> objects.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BitSet a, BitSet b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// Overloads the inequality operator (!=) to compare two <see cref="BitSet"/> objects.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BitSet a, BitSet b)
    {
        return !a.Equals(b);
    }

    /// <summary>
    /// Performs a bitwise OR operation between two bitsets.
    /// </summary>
    public static BitSet operator |(BitSet a, BitSet b)
    {
        if (a.Size != b.Size)
            throw new ArgumentException("BitSets must have the same size.");

        var result = new BitSet(a.Size);
        for (var i = 0; i < a._bits.Length; i++)
            result._bits[i] = a._bits[i] | b._bits[i];

        return result;
    }

    /// <summary>
    /// Performs a bitwise AND operation between two bitsets.
    /// </summary>
    public static BitSet operator &(BitSet a, BitSet b)
    {
        if (a.Size != b.Size)
            throw new ArgumentException("BitSets must have the same size.");

        var result = new BitSet(a.Size);
        for (var i = 0; i < a._bits.Length; i++)
            result._bits[i] = a._bits[i] & b._bits[i];

        return result;
    }

    /// <summary>
    /// Performs a bitwise XOR operation between two bitsets.
    /// </summary>
    public static BitSet operator ^(BitSet a, BitSet b)
    {
        if (a.Size != b.Size)
            throw new ArgumentException("BitSets must have the same size.");

        var result = new BitSet(a.Size);
        for (var i = 0; i < a._bits.Length; i++)
            result._bits[i] = a._bits[i] ^ b._bits[i];

        return result;
    }

    /// <summary>
    /// Performs a bitwise NOT operation (flipping all bits).
    /// </summary>
    public static BitSet operator ~(BitSet a)
    {
        var result = new BitSet(a.Size);

        // Invert all bits in each block
        for (var i = 0; i < a._bits.Length; i++)
            result._bits[i] = ~a._bits[i];

        // Trim excess bits in the last block
        var lastBitCount = a.Size % BitsPerElement;
        if (lastBitCount > 0)
            result._bits[^1] &= (1ul << lastBitCount) - 1;

        return result;
    }
}