using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public static class MemoryHelper
{    
    private const int UIntBitSize = sizeof(uint) * 8 - 1;

    public static int GetBitChunkCount(int value, int bitSize = UIntBitSize)
    {
        return value > 0 ? (value + bitSize - 1) / bitSize : 0;
    }
    
    public static unsafe int GetBitChunkCount<T>(int value)
        where T : unmanaged
    {
        return GetBitChunkCount(value, sizeof(T) * 8 - 1);
    }
}