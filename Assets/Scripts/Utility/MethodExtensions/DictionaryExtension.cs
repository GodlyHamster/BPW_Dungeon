using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class DictionaryExtension
{
    /// <summary>
    /// Gets the key in the dictionary given the value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="W"></typeparam>
    /// <param name="dict"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T KeyFromValue<T, W>(this Dictionary<T, W> dict, W value)
    {
        foreach (KeyValuePair<T, W> item in dict)
        {
            if (item.Value.Equals(value)) return item.Key;
        }
        return default(T);
    }

    /// <summary>
    /// Removes an item at a certain index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="W"></typeparam>
    /// <param name="dict"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool RemoveAt<T, W>(this Dictionary<T, W> dict, int index)
    {
        return dict.Remove(dict.ElementAt(index).Key);
    }
}
