using JetBrains.Annotations;

namespace Hypercube.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class IdStructAttribute : Attribute
{
    [UsedImplicitly] public readonly Type UnderlyingType;
    [UsedImplicitly] public readonly bool NullCast;
    
    public IdStructAttribute(Type underlyingType, bool nullCast = false)
    {
        UnderlyingType = underlyingType;
        NullCast = nullCast;
    }
}
