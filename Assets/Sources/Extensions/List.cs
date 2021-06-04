using System;
using System.Collections.Generic;

namespace AssetBundlesClass.Extensions
{
    public static class ListExt
    {
        public static bool Contains<TItem>(this List<TItem> list, Predicate<TItem> predicate)
        {
            for (int index = 0; index < list.Count; index++)
                if (predicate.Invoke(list[index]))
                    return true;
            return false;
        }
    }
}