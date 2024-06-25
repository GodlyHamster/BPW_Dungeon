using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class DictionaryExtension
{
    public static T KeyFromValue<T, W>(this Dictionary<T, W> dict, W value)
    {
        foreach (KeyValuePair<T, W> item in dict)
        {
            if (item.Value.Equals(value)) return item.Key;
        }
        return default(T);
    }

    public static bool RemoveAt<T, W>(this Dictionary<T, W> dict, int index)
    {
        return dict.Remove(dict.ElementAt(index).Key);
    }
}
