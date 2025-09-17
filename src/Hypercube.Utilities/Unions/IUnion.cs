using JetBrains.Annotations;

namespace Hypercube.Utilities.Unions;

[PublicAPI]
public interface IUnion
{
    UnionTypeCode Type { get; }
}
