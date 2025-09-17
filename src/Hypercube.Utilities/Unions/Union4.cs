using System.Runtime.InteropServices;
using Hypercube.Utilities.Unions.Extensions;

namespace Hypercube.Utilities.Unions;

[StructLayout(LayoutKind.Explicit, Size = 5)]
public struct Union4 : IUnion
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

    [field: FieldOffset(4)]
    public UnionTypeCode Type { get; private set; } = UnionTypeCode.Empty;
    
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
    
    public Union4(UnionTypeCode type)
    {
        Type = type;
    }

    public Union4(byte value) : this(UnionTypeCode.Byte)
    {
        _byte = value;
    }

    public Union4(sbyte value) : this(UnionTypeCode.SByte)
    {
        _sbyte = value;
    }

    public Union4(short value) : this(UnionTypeCode.Int16)
    {
        _int16 = value;
    }

    public Union4(ushort value) : this(UnionTypeCode.UInt16)
    {
        _uint16 = value;
    }

    public Union4(char value) : this(UnionTypeCode.Char)
    {
        _char = value;
    }

    public Union4(bool value) : this(UnionTypeCode.Boolean)
    {
        _boolean = value;
    }

    public Union4(int value) : this(UnionTypeCode.Int32)
    {
        _int32 = value;
    }
    
    public Union4(uint value) : this(UnionTypeCode.UInt32)
    {
        _uint32 = value;
    }
    
    public Union4(float value) : this(UnionTypeCode.Single)
    {
        _single = value;
    }

    public T Get<T>() where T : struct
    {
        switch (typeof(T).GetUnionTypeCode())
        {
            case UnionTypeCode.Boolean:
                return (T) (object) Bool;
            
            case UnionTypeCode.Char:
                return (T) (object) Char;
            
            case UnionTypeCode.SByte:
                return (T) (object) SByte;
            
            case UnionTypeCode.Byte:
                return (T) (object) Byte;
            
            case UnionTypeCode.Int16:
                return (T) (object) Short;
            
            case UnionTypeCode.UInt16:
                return (T) (object) UShort;
            
            case UnionTypeCode.Int32:
                return (T) (object) Int;
            
            case UnionTypeCode.UInt32:
                return (T) (object) UInt;

            case UnionTypeCode.Single:
                return (T) (object) Float;

            case UnionTypeCode.Empty:
            case UnionTypeCode.Object:
            case UnionTypeCode.Double:
            case UnionTypeCode.Decimal:
            case UnionTypeCode.DateTime:
            case UnionTypeCode.String:
            case UnionTypeCode.Int64:
            case UnionTypeCode.UInt64:
            default:
                throw new InvalidCastException();
        }
    }

    public void Set<T>(T value) where T : struct
    {
        switch (typeof(T).GetUnionTypeCode())
        {
            case UnionTypeCode.Boolean:
                Bool = (bool) (object) value;
                break;

            case UnionTypeCode.Char:
                Char = (char) (object) value;
                break;
                
            case UnionTypeCode.SByte:
                SByte = (sbyte) (object) value;
                break;
                
            case UnionTypeCode.Byte:
                Byte = (byte) (object) value;
                break;
                      
            case UnionTypeCode.Int16:
                Short = (short) (object) value;
                break;          
            
            case UnionTypeCode.UInt16:
                UShort = (ushort) (object) value;
                break;          
           
            case UnionTypeCode.Int32:
                Int = (int) (object) value;
                break;    
      
            case UnionTypeCode.UInt32:
                UInt = (uint) (object) value;
                break;    
            
            case UnionTypeCode.Single:
                Float = (float) (object) value;
                break;    
            
            case UnionTypeCode.Empty:
            case UnionTypeCode.Object:
            case UnionTypeCode.Double:
            case UnionTypeCode.Decimal:
            case UnionTypeCode.DateTime:
            case UnionTypeCode.String:
            case UnionTypeCode.Int64:
            case UnionTypeCode.UInt64:
            default:
                throw new InvalidCastException();
        }
    }
}
