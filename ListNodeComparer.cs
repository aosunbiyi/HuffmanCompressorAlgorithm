namespace DataCompression
{
       public class ListNodeComparer : IComparer<ListNode>
        {
            public int Compare(ListNode? x, ListNode? y)
            {
                if (x is null || y is null)
                    return 0;

                return x.Frequency.CompareTo(y.Frequency);
            }
        }
}