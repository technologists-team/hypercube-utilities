using JetBrains.Annotations;

namespace Hypercube.Utilities.Unions;

[PublicAPI]
public interface IUnion
{
    UnionTypeCode Type { get; }
    T Get<T>() where T : unmanaged;
    void Set<T>(T value) where T : unmanaged;
}
