using System;
using AssetBundlesClass.Shared.Pools;

namespace AssetBundlesClass.Extensions
{
    public static class ArrayExt
    {
        public delegate bool FilterMapFunc<in TValue, TResult>(TValue value, out TResult result);
        
        public static bool Contains<TValue>(this TValue[] array, TValue value)
        {
            for (int index = 0; index < array.Length; index++)
            { if (array[index].Equals(value)) return true; }
            return false;
        }

        public static TValue Find<TValue>(this TValue[] array, Predicate<TValue> predicate)
        {
            for (int index = 0; index < array.Length; index++)
            {
                TValue current = array[index];
                if (predicate.Invoke(current)) return current;
            }
            return default;
        }

        public static TValue[] Filter<TValue>(this TValue[] array, Predicate<TValue> predicate)
        {
            using ListPool<TValue> filtered = ListPool<TValue>.Rent();

            for (int index = 0; index < array.Length; index++)
            {
                TValue current = array[index];
                if (predicate.Invoke(current)) filtered.Add(current);
            }

            return filtered.ToArray();
        }

        public static TResult[] Map<TValue, TResult>(this TValue[] array, Func<TValue, TResult> mapper)
        {
            TResult[] results = new TResult[array.Length];
            for (int index = 0; index < array.Length; index++) results[index] = mapper.Invoke(array[index]);
            return results;
        }

        public static TResult[] FilterMap<TValue, TResult>(this TValue[] array, FilterMapFunc<TValue, TResult> mapper)
        {
            using ListPool<TResult> mapped = ListPool<TResult>.Rent();
            for (int index = 0; index < array.Length; index++)
            {
                TValue current = array[index];
                if (mapper.Invoke(current, out TResult result)) mapped.Add(result);
            }
            return mapped.ToArray();
        }
    }
}