using System;
using System.Collections.Generic;
using System.Text;


namespace DataCompression
{
    public class HuffmanCompressor
    {
        private readonly IComparisonSorter<ListNode>? _sorter;
        private readonly Translator? _translator;

        public HuffmanCompressor(IComparisonSorter<ListNode>? sorter, Translator? translator)
        {
            _sorter = sorter;
            _translator = translator;
        }

        public (string decompressText, Dictionary<string, string>) Compress(string uncompressedText)
        {
            if (string.IsNullOrEmpty(uncompressedText))
            {
                return (string.Empty, new Dictionary<string, string>());
            }

            if (uncompressedText.Distinct().Count() == 1)
            {
                var dict = new Dictionary<string, string>{
                    {"1",uncompressedText[0].ToString()}
                };

                return (new string('1', uncompressedText.Length), dict);
            }

            var nodes = GetListNodesFromText(uncompressedText);
            var tree = GenerateHuffmanTree(nodes);
            var (compressionKeys, decompressionKeys) = GetKeys(tree);
            return (_translator.Translate(uncompressedText, compressionKeys), decompressionKeys);
        }

        private static ListNode[] GetListNodesFromText(string text)
        {
            var occurenceCounts = new Dictionary<char, int>();
            foreach (var ch in text)
            {
                if (!occurenceCounts.ContainsKey(ch))
                {
                    occurenceCounts.Add(ch, 0);
                }
                occurenceCounts[ch]++;
            }
            return occurenceCounts.Select(kvp => new ListNode(kvp.Key, 1d * kvp.Value / text.Length)).ToArray();
        }

        private (Dictionary<string, string> compressionKeys, Dictionary<string, string> decompressionKeys) GetKeys(ListNode tree)
        {
            var compressionKeys = new Dictionary<string, string>();
            var decompressionKeys = new Dictionary<string, string>();
            if (tree.HasData)
            {
                compressionKeys.Add(tree.Data.ToString(), string.Empty);
                decompressionKeys.Add(string.Empty, tree.Data.ToString());
                return (compressionKeys, decompressionKeys);
            }

            if (tree.LeftChild is not null)
            {
                var (lsck, lsdk) = GetKeys(tree.LeftChild);
                compressionKeys.AddMany(lsck.Select(kvp => (kvp.Key, "0" + kvp.Value)));
                decompressionKeys.AddMany(lsdk.Select(kvp => ("0" + kvp.Key, kvp.Value)));
            }

            if (tree.RightChild is not null)
            {
                var (rsck, rsdk) = GetKeys(tree.RightChild);
                compressionKeys.AddMany(rsck.Select(kvp => (kvp.Key, "1" + kvp.Value)));
                decompressionKeys.AddMany(rsdk.Select(kvp => ("1" + kvp.Key, kvp.Value)));

                return (compressionKeys, decompressionKeys);
            }

            return (compressionKeys, decompressionKeys);

        }

        private ListNode GenerateHuffmanTree(ListNode[] nodes)
        {
            var comparer = new ListNodeComparer();
            while (nodes.Length > 1)
            {
                _sorter.Sort(nodes, comparer);

                var left = nodes[0];
                var right = nodes[1];

                var newNodes = new ListNode[nodes.Length - 1];
                Array.Copy(nodes, 2, newNodes, 1, nodes.Length - 2);
                newNodes[0] = new ListNode(left, right);
                nodes = newNodes;
            }

            return nodes[0];
        }

    }
}