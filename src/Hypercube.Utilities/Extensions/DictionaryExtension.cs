using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

[PublicAPI]
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