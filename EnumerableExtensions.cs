﻿using System.Collections;

public static class EnumerableExtensions
{
    public static IEnumerable OfTypeMultiple<T1, T2>(this IEnumerable source)
    {
        foreach (object item in source)
        {
            if (item is T1 || item is T2)
            {
                yield return item;
            }
        }
    }
}