namespace Hypercube.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class FixedArrayAttribute(int length) : Attribute
{
    public int Length { get; } = length;
}
