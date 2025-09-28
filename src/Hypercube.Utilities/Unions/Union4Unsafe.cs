using System.Runtime.InteropServices;
using Hypercube.Utilities.Unions.Extensions;

namespace Hypercube.Utilities.Unions;

/// <summary>
/// Represents a 4-byte unsafe union capable of storing multiple unmanaged types in the same memory location.
/// Unlike <see cref="Union4"/>, this version does not track the type and does not perform type checks.
/// </summary>
/// <remarks>
/// <para>
/// This struct allows storing a single value of type <see cref="byte"/>, <see cref="sbyte"/>,
/// <see cref="short"/>, <see cref="ushort"/>, <see cref="char"/>, <see cref="bool"/>,
/// <see cref="int"/>, <see cref="uint"/>, or <see cref="float"/>. Only one value can be stored at a time.
/// </para>
/// <para>
/// The structure has a size of 4 bytes, as it stores only the value itself. No extra byte is used
/// for type tracking, which allows for maximum memory efficiency but requires the caller to manage
/// the type manually.
/// </para>
/// <para>
/// Since there is no type checking, reading a value with the wrong type can lead to undefined behavior.
/// Use the <see cref="Get{T}"/> and <see cref="Set{T}"/> methods carefully.
/// </para>
/// </remarks>
/// <seealso cref="Union4"/>
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct Union4Unsafe : IUnion
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
    
    public UnionTypeCode Type => UnionTypeCode.Empty;
    
    public Union4Unsafe(byte value)
    {
        Byte = value;
    }

    public Union4Unsafe(sbyte value)
    {
        SByte = value;
    }

    public Union4Unsafe(short value)
    {
        Short = value;
    }

    public Union4Unsafe(ushort value)
    {
        UShort = value;
    }

    public Union4Unsafe(char value)
    {
        Char = value;
    }

    public Union4Unsafe(bool value)
    {
        Bool = value;
    }

    public Union4Unsafe(int value)
    {
        Int = value;
    }
    
    public Union4Unsafe(uint value)
    {
        UInt = value;
    }
    
    public Union4Unsafe(float value)
    {
        Float = value;
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
            
            default:
                throw new UnionUnsupportedCastException(this, code);
        }
    }
}
