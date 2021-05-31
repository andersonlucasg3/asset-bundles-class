using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AssetBundlesClass.Shared.Pools
{
    public class ListPool<TItem> : List<TItem>, IDisposable
    {
        private static readonly ConcurrentBag<ListPool<TItem>> _bag = new ConcurrentBag<ListPool<TItem>>();

        public static ListPool<TItem> Rent() => _bag.TryTake(out ListPool<TItem> list) ? list : new ListPool<TItem>();

        public void Dispose()
        {
            Clear();
            _bag.Add(this);
        }
    }
}