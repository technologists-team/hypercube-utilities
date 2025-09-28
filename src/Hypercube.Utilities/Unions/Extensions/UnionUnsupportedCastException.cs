namespace Hypercube.Utilities.Unions.Extensions;

public class UnionUnsupportedCastException : InvalidCastException
{
    public UnionUnsupportedCastException(object union, UnionTypeCode code) : base($"The current union {union.GetType().Name} does not support conversion to {code}")
    {
    }
}
