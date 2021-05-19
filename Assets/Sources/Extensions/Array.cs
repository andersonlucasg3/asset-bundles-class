namespace AssetBundlesClass.Extensions
{
    public static class ArrayExt
    {
        public static bool Contains<TValue>(this TValue[] array, TValue value)
        {
            for (int index = 0; index < array.Length; index++)
            { if (array[index].Equals(value)) return true; }
            return false;
        }
    }
}