using System.Runtime.InteropServices;
using Hypercube.Utilities.Unions.Extensions;

namespace Hypercube.Utilities.Unions;

/// <summary>
/// Represents a 4-byte union capable of storing multiple unmanaged types in the same memory location.
/// The actual type stored is tracked by the <see cref="Type"/> property.
/// </summary>
/// <remarks>
/// <para>
/// This struct allows storing a single value of type <see cref="byte"/>, <see cref="sbyte"/>,
/// <see cref="short"/>, <see cref="ushort"/>, <see cref="char"/>, <see cref="bool"/>,
/// <see cref="int"/>, <see cref="uint"/>, or <see cref="float"/>. Only one value can be stored at a time.
/// </para>
/// <para>
/// The structure has a size of 5 bytes: 4 bytes for the value itself (the union of all fields)
/// and 1 byte for <see cref="Type"/>, which stores the current type code. This ensures the type
/// is known at runtime and allows safe reading of the stored value.
/// </para>
/// <para>
/// When accessing a typed property (e.g., <see cref="Int"/> or <see cref="Float"/>),
/// the getter checks that the stored type matches the requested type. If the type does not match,
/// an <see cref="InvalidCastException"/> is thrown. This prevents accidental reads of the wrong type.
/// </para>
/// </remarks>
/// <seealso cref="Union4Unsafe"/>
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
                UShort =HyperUnsafe.AsUnmanaged<T, ushort>(value);
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
            
            default:
                throw new UnionUnsupportedCastException(this, code);
        }
    }
}
