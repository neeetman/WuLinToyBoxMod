namespace HaxxToyBox.Utilities.Extensions;

public static class CollectionExtensions
{
    public static void InsertInOrder<T>(this List<T> list, T item, Comparison<T> comparison)
    {
        int index = list.BinarySearch(item, Comparer<T>.Create(comparison));
        if (index < 0)
            index = ~index; // bitwise complement to get the insertion point
        list.Insert(index, item);
    }
}
