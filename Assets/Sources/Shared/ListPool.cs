using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class ListPool<TItem> : List<TItem>, IDisposable
{
    private static readonly ConcurrentBag<ListPool<TItem>> _bag = new ConcurrentBag<ListPool<TItem>>();

    public static ListPool<TItem> Rent()
    {
        if (_bag.TryTake(out ListPool<TItem> list)) return list;
        return new ListPool<TItem>();
    }

    public void Dispose()
    {
        Clear();
        _bag.Add(this);
    }
}