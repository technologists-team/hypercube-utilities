using System.Runtime.InteropServices;
using Hypercube.Utilities.Unions.Extensions;
using Hypercube.Utilities.Unsafe;

namespace Hypercube.Utilities.Unions;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct Union8Unsafe : IUnion
{
    [FieldOffset(0)] public byte Byte;
    [FieldOffset(0)] public sbyte SByte;
    [FieldOffset(0)] public short Short;
    [FieldOffset(0)] public ushort UShort;
    [FieldOffset(0)] public char Char;
    [FieldOffset(0)] public bool Bool;
    [FieldOffset(0)] public int Int;
    [FieldOffset(0)] public uint UInt;
    [FieldOffset(0)] public float Float;
    [FieldOffset(0)] public long Long;
    [FieldOffset(0)] public ulong Ulong;
    [FieldOffset(0)] public double Double;
    
    public UnionTypeCode Type => UnionTypeCode.Empty;
    
    public Union8Unsafe(byte value) : this()
    {
        Byte = value;
    }

    public Union8Unsafe(sbyte value) : this()
    {
        SByte = value;
    }

    public Union8Unsafe(short value) : this()
    {
        Short = value;
    }

    public Union8Unsafe(ushort value) : this()
    {
        UShort = value;
    }

    public Union8Unsafe(char value) : this()
    {
        Char = value;
    }

    public Union8Unsafe(bool value) : this()
    {
        Bool = value;
    }

    public Union8Unsafe(int value) : this()
    {
        Int = value;
    }
    
    public Union8Unsafe(uint value) : this()
    {
        UInt = value;
    }
    
    public Union8Unsafe(float value) : this()
    {
        Float = value;
    }
    
    public Union8Unsafe(long value) : this()
    {
        Long = value;
    }
    
    public Union8Unsafe(ulong value) : this()
    {
        Ulong = value;
    }
    
    public Union8Unsafe(double value) : this()
    {
        Double = value;
    }
}