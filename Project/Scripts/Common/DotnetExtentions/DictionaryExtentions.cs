using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading.Tasks;
public static class DictionaryExtentions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ProcessOverlapps<KeyT, T1, T2>(this Dictionary<KeyT, T1> dict1, Dictionary<KeyT, T2> dict2, Action<T1, T2> process)
    {
        if (dict1.Count < dict2.Count)
            Parallel.ForEach(dict1.Keys, key =>
            {
                if (dict2.ContainsKey(key))
                    process(dict1[key], dict2[key]);
            });
        else
            Parallel.ForEach(dict2.Keys, key =>
            {
                if (dict1.ContainsKey(key))
                    process(dict1[key], dict2[key]);
            });
    }
}