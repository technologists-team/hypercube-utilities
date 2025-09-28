using System.Runtime.InteropServices;

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
    [FieldOffset(0)] public DateTime DateTime;
    
    public UnionTypeCode Type => UnionTypeCode.Empty;
    
    public Union8Unsafe(byte value)
    {
        Byte = value;
    }

    public Union8Unsafe(sbyte value)
    {
        SByte = value;
    }

    public Union8Unsafe(short value)
    {
        Short = value;
    }

    public Union8Unsafe(ushort value)
    {
        UShort = value;
    }

    public Union8Unsafe(char value)
    {
        Char = value;
    }

    public Union8Unsafe(bool value)
    {
        Bool = value;
    }

    public Union8Unsafe(int value)
    {
        Int = value;
    }
    
    public Union8Unsafe(uint value)
    {
        UInt = value;
    }
    
    public Union8Unsafe(float value)
    {
        Float = value;
    }
    
    public Union8Unsafe(long value)
    {
        Long = value;
    }
    
    public Union8Unsafe(ulong value)
    {
        Ulong = value;
    }
    
    public Union8Unsafe(double value)
    {
        Double = value;
    }
    
    public Union8Unsafe(DateTime value)
    {
        DateTime = value;
    }
}
