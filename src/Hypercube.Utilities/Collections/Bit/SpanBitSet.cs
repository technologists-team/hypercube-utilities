using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

// Fuck you resharper, it's NORMAL
// ReSharper disable StaticMemberInGenericType

namespace Hypercube.Utilities.Collections.Bit;

[PublicAPI]
public ref struct SpanBitSet<T> : IBitSet
    where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T>
{
    private static readonly int BitsPerElement = Unsafe.SizeOf<T>() * 8;
    private static readonly int Shift = BitOperations.TrailingZeroCount(BitsPerElement);
    private static readonly int OffsetMask = BitsPerElement - 1;

    private readonly Span<T> _bits;

    public int Length => _bits.Length;

    public SpanBitSet(Span<T> bits)
    {
        _bits = bits;
    }

    public ReadOnlySpan<T> AsReadOnlySpan() => _bits;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int index) => _bits[ElementIndex(index)] |= ElementMask(index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset(int index) => _bits[ElementIndex(index)] &= ~ElementMask(index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(int index) => (_bits[ElementIndex(index)] & ElementMask(index)) != T.Zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _bits.Clear();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ElementIndex(int index) => index >> Shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T ElementMask(int index) => T.One << (index & OffsetMask);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T>.Enumerator AsEnumerable() => _bits.GetEnumerator();
}

