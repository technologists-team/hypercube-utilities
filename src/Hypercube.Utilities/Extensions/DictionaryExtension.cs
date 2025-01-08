using System.Runtime.InteropServices;

namespace Hypercube.Utilities.Extensions;

public static class DictionaryExtension
{
    public static TValue GetOrInstantiate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        where TValue : new()
        where TKey : notnull
    {
        ref var entry = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        return exists ? entry! : entry = new TValue();
    }
}