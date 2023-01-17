namespace DataCompression
{
    public interface IComparisonSorter<T>
    {
        void Sort(T[] array, IComparer<T> comparer);
    }
}