using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static T RandomItem<T>(this List<T> list)
    {
        if (list.Count == 0) return default(T);
        return list[Random.Range(0, list.Count)];
    }

    public static void RemoveLast<T>(this List<T> list)
    {
        list.RemoveAt(list.Count - 1);
    }
}
