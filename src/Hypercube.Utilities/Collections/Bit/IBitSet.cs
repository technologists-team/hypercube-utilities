using JetBrains.Annotations;

namespace Hypercube.Utilities.Collections.Bit;

[PublicAPI]
public interface IBitSet
{
    void Set(int index);
    void Reset(int index);
    bool Has(int index);
    void Clear();
}