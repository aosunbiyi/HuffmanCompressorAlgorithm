namespace DataCompression
{
       public class ListNode
        {
            public ListNode(char data, double frequency)
            {
                HasData = true;
                Data = data;
                Frequency = frequency;
            }
            public ListNode(ListNode leftChild, ListNode rightChild)
            {
                LeftChild = leftChild;
                RightChild = rightChild;
                Frequency = leftChild.Frequency + rightChild.Frequency;
            }
            public char Data { get; }
            public bool HasData { get; }
            public double Frequency { get; }
            public ListNode? RightChild { get; set; }
            public ListNode? LeftChild { get; set; }
        }
}