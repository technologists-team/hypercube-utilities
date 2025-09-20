using System.Runtime.InteropServices;
using Hypercube.Utilities.Unions.Extensions;
using Hypercube.Utilities.Unsafe;

namespace Hypercube.Utilities.Unions;

[StructLayout(LayoutKind.Explicit, Size = 9)]
public struct Union8Ref : IUnion
{
    [FieldOffset(0)] private byte _byte;
    [FieldOffset(0)] private sbyte _sbyte;
    [FieldOffset(0)] private short _int16;
    [FieldOffset(0)] private ushort _uint16;
    [FieldOffset(0)] private char _char;
    [FieldOffset(0)] private bool _boolean;
    [FieldOffset(0)] private int _int32;
    [FieldOffset(0)] private uint _uint32;
    [FieldOffset(0)] private float _single;
    [FieldOffset(0)] private long _int64;
    [FieldOffset(0)] private ulong _uint64;
    [FieldOffset(0)] private double _double;
    
    [FieldOffset(0)] private string? _string;
    [FieldOffset(0)] private object? _object;
    
    [field: FieldOffset(8)]
    public UnionTypeCode Type { get; private set; }
    
    public byte Byte
    {
        set
        {
            Type = UnionTypeCode.Byte;
            _byte = value;
        }
        get => Type == UnionTypeCode.Byte ? _byte : throw new InvalidCastException();
    }

    public sbyte SByte
    {
        set
        {
            Type = UnionTypeCode.SByte;
            _sbyte = value;
        }
        get => Type == UnionTypeCode.SByte ? _sbyte : throw new InvalidCastException();
    }

    public short Short
    {
        set
        {
            Type = UnionTypeCode.Int16;
            _int16 = value;
        }
        get => Type == UnionTypeCode.Int16 ? _int16 : throw new InvalidCastException();
    }
    
    public ushort UShort
    {
        set
        {
            Type = UnionTypeCode.UInt16;
            _uint16 = value;
        }
        get => Type == UnionTypeCode.UInt16 ? _uint16 : throw new InvalidCastException();
    }

    public char Char
    {
        set
        {
            Type = UnionTypeCode.Char;
            _char = value;
        }
        get => Type == UnionTypeCode.Char ? _char : throw new InvalidCastException();
    }
    
    public bool Bool
    {
        set
        {
            Type = UnionTypeCode.Boolean;
            _boolean = value;
        }
        get => Type == UnionTypeCode.Boolean ? _boolean : throw new InvalidCastException();
    }

    public int Int
    {
        set
        {
            Type = UnionTypeCode.Int32;
            _int32 = value;
        }
        get => Type == UnionTypeCode.Int32 ? _int32 : throw new InvalidCastException();
    }
    
    public uint UInt
    {
        set
        {
            Type = UnionTypeCode.UInt32;
            _uint32 = value;
        }
        get => Type == UnionTypeCode.UInt32 ? _uint32 : throw new InvalidCastException();
    }
    
    public float Float
    {
        set
        {
            Type = UnionTypeCode.Single;
            _single = value;
        }
        get => Type == UnionTypeCode.Single ? _single : throw new InvalidCastException();
    }
    
    public long Long
    {
        set
        {
            Type = UnionTypeCode.Int64;
            _int64 = value;
        }
        get => Type == UnionTypeCode.Int64 ? _int64 : throw new InvalidCastException();
    }
    
    public ulong ULong
    {
        set
        {
            Type = UnionTypeCode.UInt64;
            _uint64 = value;
        }
        get => Type == UnionTypeCode.UInt64 ? _uint64 : throw new InvalidCastException();
    }
    
    public double Double
    {
        set
        {
            Type = UnionTypeCode.Double;
            _double = value;
        }
        get => Type == UnionTypeCode.Double ? _double : throw new InvalidCastException();
    }
    
    public string? String
    {
        set
        {
            Type = UnionTypeCode.String;
            _string = value;
        }
        get => Type == UnionTypeCode.String ? _string : throw new InvalidCastException();
    }
    
    public object? Object
    {
        set
        {
            Type = UnionTypeCode.Object;
            _object = value;
        }
        get => Type == UnionTypeCode.Object ? _object : throw new InvalidCastException();
    }
    
    public Union8Ref(UnionTypeCode type)
    {
        Type = type;
    }

    public Union8Ref(byte value) : this(UnionTypeCode.Byte)
    {
        _byte = value;
    }

    public Union8Ref(sbyte value) : this(UnionTypeCode.SByte)
    {
        _sbyte = value;
    }

    public Union8Ref(short value) : this(UnionTypeCode.Int16)
    {
        _int16 = value;
    }

    public Union8Ref(ushort value) : this(UnionTypeCode.UInt16)
    {
        _uint16 = value;
    }

    public Union8Ref(char value) : this(UnionTypeCode.Char)
    {
        _char = value;
    }

    public Union8Ref(bool value) : this(UnionTypeCode.Boolean)
    {
        _boolean = value;
    }

    public Union8Ref(int value) : this(UnionTypeCode.Int32)
    {
        _int32 = value;
    }
    
    public Union8Ref(uint value) : this(UnionTypeCode.UInt32)
    {
        _uint32 = value;
    }
    
    public Union8Ref(float value) : this(UnionTypeCode.Single)
    {
        _single = value;
    }
    
    public Union8Ref(long value) : this(UnionTypeCode.Int64)
    {
        _int64 = value;
    }

    public Union8Ref(ulong value) : this(UnionTypeCode.UInt64)
    {
        _uint64 = value;
    }

    public Union8Ref(double value) : this(UnionTypeCode.Double)
    {
        _double = value;
    }
    
    public Union8Ref(string value) : this(UnionTypeCode.String)
    {
        _string = value;
    }
    
    public Union8Ref(object value) : this(UnionTypeCode.Object)
    {
        _object = value;
    }
}