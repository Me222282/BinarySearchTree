using System;

namespace BinarySearchTree
{
    public class DistanceTree
    {
        public DistanceTree(BinarySearchTree tree)
        {
            _source = SetNode(tree._source, false);
        }
        
        internal class Node
        {
            public Node(float o, BinarySearchTree.Node src)
            {
                Offset = o;
                Source = src;
            }
            
            public float Offset;
            public float Size;
            
            public BinarySearchTree.Node Source;
            
            public Node Greater;
            public Node Lesser;
        }
        
        internal Node _source;
        
        private Node SetNode(BinarySearchTree.Node source, bool great)
        {
            if (source == null) { return null; }
            
            Node n = new Node(0, source);
            
            Node g = SetNode(source.Greater, true);
            Node l = SetNode(source.Lesser, false);
            
            n.Greater = g;
            n.Lesser = l;
            
            if (source.Source != null &&
                source.Source.Greater == source)
            {
                n.Offset = -(n.Lesser != null ? n.Lesser.Size : 0f) + 1f;
                n.Size = (n.Greater != null ? n.Greater.Size : 0f) + n.Offset;
            }
            else if (source.Source != null &&
                source.Source.Lesser == source)
            {
                n.Offset = -(n.Greater != null ? n.Greater.Size : 0f) - 1f;
                n.Size = (n.Lesser != null ? n.Lesser.Size : 0f) + n.Offset;
            }
            
            return n;
        }
        
        private void FindLargestG(BinarySearchTree.Node n, ref int right)
        {
            if (n == null) { return; }
            
            if (n.Distance > right)
            {
                right = n.Distance;
            }
            
            FindLargestG(n.Lesser, ref right);
            FindLargestG(n.Greater, ref right);
        }
        private void FindLargestL(BinarySearchTree.Node n, ref int left)
        {
            if (n == null) { return; }
            
            if (n.Distance < left)
            {
                left = n.Distance;
            }
            
            FindLargestL(n.Lesser, ref left);
            FindLargestL(n.Greater, ref left);
        }
    }
}