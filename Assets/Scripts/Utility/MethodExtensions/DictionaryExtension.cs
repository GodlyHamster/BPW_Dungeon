using System.Collections.Generic;

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
}
