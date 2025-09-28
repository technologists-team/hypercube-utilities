using System.Runtime.InteropServices;
using Hypercube.Utilities.Unions.Extensions;

namespace Hypercube.Utilities.Unions;

[StructLayout(LayoutKind.Explicit, Size = 9)]
public struct Union8 : IUnion
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
    [FieldOffset(0)] private DateTime _dateTime;
    
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
    
    public DateTime DateTime
    {
        set
        {
            Type = UnionTypeCode.DateTime;
            _dateTime = value;
        }
        get => Type == UnionTypeCode.DateTime ? _dateTime : throw new InvalidCastException();
    }
    
    public Union8(UnionTypeCode type)
    {
        Type = type;
    }

    public Union8(byte value) : this(UnionTypeCode.Byte)
    {
        _byte = value;
    }

    public Union8(sbyte value) : this(UnionTypeCode.SByte)
    {
        _sbyte = value;
    }

    public Union8(short value) : this(UnionTypeCode.Int16)
    {
        _int16 = value;
    }

    public Union8(ushort value) : this(UnionTypeCode.UInt16)
    {
        _uint16 = value;
    }

    public Union8(char value) : this(UnionTypeCode.Char)
    {
        _char = value;
    }

    public Union8(bool value) : this(UnionTypeCode.Boolean)
    {
        _boolean = value;
    }

    public Union8(int value) : this(UnionTypeCode.Int32)
    {
        _int32 = value;
    }
    
    public Union8(uint value) : this(UnionTypeCode.UInt32)
    {
        _uint32 = value;
    }
    
    public Union8(float value) : this(UnionTypeCode.Single)
    {
        _single = value;
    }
    
    public Union8(long value) : this(UnionTypeCode.Int64)
    {
        _int64 = value;
    }

    public Union8(ulong value) : this(UnionTypeCode.UInt64)
    {
        _uint64 = value;
    }

    public Union8(double value) : this(UnionTypeCode.Double)
    {
        _double = value;
    }
    
    public Union8(DateTime value) : this(UnionTypeCode.DateTime)
    {
        _dateTime = value;
    }
    
    public T Get<T>() where T : unmanaged
    {
        var code = typeof(T).GetUnionTypeCode();
        switch (code)
        {
            case UnionTypeCode.Boolean:
                return HyperUnsafe.AsUnmanaged<bool, T>(Bool);
            
            case UnionTypeCode.Char:
                return HyperUnsafe.AsUnmanaged<char, T>(Char);
            
            case UnionTypeCode.SByte:
                return HyperUnsafe.AsUnmanaged<sbyte, T>(SByte);
            
            case UnionTypeCode.Byte:
                return HyperUnsafe.AsUnmanaged<byte, T>(Byte);
            
            case UnionTypeCode.Int16:
                return HyperUnsafe.AsUnmanaged<short, T>(Short);
            
            case UnionTypeCode.UInt16:
                return HyperUnsafe.AsUnmanaged<ushort, T>(UShort);
            
            case UnionTypeCode.Int32:
                return HyperUnsafe.AsUnmanaged<int, T>(Int);
            
            case UnionTypeCode.UInt32:
                return HyperUnsafe.AsUnmanaged<uint, T>(UInt);

            case UnionTypeCode.Single:
                return HyperUnsafe.AsUnmanaged<float, T>(Float);

            case UnionTypeCode.Int64:
                return HyperUnsafe.AsUnmanaged<long, T>(Long);
                
            case UnionTypeCode.UInt64:
                return HyperUnsafe.AsUnmanaged<ulong, T>(ULong);
            
            case UnionTypeCode.Double:
                return HyperUnsafe.AsUnmanaged<double, T>(Double);

            case UnionTypeCode.DateTime:
                return HyperUnsafe.AsUnmanaged<DateTime, T>(DateTime);
            
            default:
                throw new UnionUnsupportedCastException(this, code);
        }
    }

    public void Set<T>(T value) where T : unmanaged
    {
        var code = typeof(T).GetUnionTypeCode();
        switch (code)
        {
            case UnionTypeCode.Boolean:
                Bool = HyperUnsafe.AsUnmanaged<T, bool>(value);
                break;

            case UnionTypeCode.Char:
                Char = HyperUnsafe.AsUnmanaged<T, char>(value);
                break;
                
            case UnionTypeCode.SByte:
                SByte = HyperUnsafe.AsUnmanaged<T, sbyte>(value);
                break;
                
            case UnionTypeCode.Byte:
                Byte = HyperUnsafe.AsUnmanaged<T, byte>(value);
                break;
                      
            case UnionTypeCode.Int16:
                Short = HyperUnsafe.AsUnmanaged<T, short>(value);
                break;          
            
            case UnionTypeCode.UInt16:
                UShort = HyperUnsafe.AsUnmanaged<T, ushort>(value);
                break;          
           
            case UnionTypeCode.Int32:
                Int = HyperUnsafe.AsUnmanaged<T, int>(value);
                break;    
      
            case UnionTypeCode.UInt32:
                UInt = HyperUnsafe.AsUnmanaged<T, uint>(value);
                break;    
            
            case UnionTypeCode.Single:
                Float = HyperUnsafe.AsUnmanaged<T, float>(value);
                break;    
            
            case UnionTypeCode.Int64:
                Long = HyperUnsafe.AsUnmanaged<T, long>(value);
                break; 
                
            case UnionTypeCode.UInt64:
                ULong = HyperUnsafe.AsUnmanaged<T, ulong>(value);
                break;
            
            case UnionTypeCode.Double:
                Double = HyperUnsafe.AsUnmanaged<T, double>(value); 
                break;
                
            case UnionTypeCode.DateTime:
                DateTime = HyperUnsafe.AsUnmanaged<T, DateTime>(value); 
                break;
            
            default:
                throw new UnionUnsupportedCastException(this, code);
        }
    }
}
