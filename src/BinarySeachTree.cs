namespace BinarySearchTree
{
    public class BinarySearchTree
    {
        public class Node
        {
            public Node(double v)
            {
                Value = v;
            }
            
            public double Value;
            
            public Node Source;
            
            public int Distance;
            public Node Greater;
            public Node Lesser;
        }
        
        internal Node _source;
        
        public void Clear()
        {
            _source = null;
        }
        
        public void Add(double value)
        {
            Node newNode = new Node(value);
            
            // Tree empty
            if (_source == null)
            {
                _source = newNode;
                return;
            }
            
            Node pre = null;
            Node current = _source;
            // Determine whether current is from pre.Greater or pre.Lesser
            bool greater = false;
            
            while (true)
            {
                // Found branch of tree
                if (current == null)
                {
                    newNode.Source = pre;
                    
                    if (greater)
                    {
                        pre.Greater = newNode;
                        newNode.Distance = pre.Distance + 1;
                        return;
                    }
                    
                    pre.Lesser = newNode;
                    newNode.Distance = pre.Distance - 1;
                    return;
                }
                
                pre = current;
                
                // Value is down Lesser branch of current
                if (value < current.Value)
                {
                    current = current.Lesser;
                    greater = false;
                    continue;
                }
                
                // Value is down Greater branch of current
                current = current.Greater;
                greater = true;
            }
        }
        
        public Node Find(double value)
        {
            Node current = _source;
            
            while (true)
            {
                // Value isn't in tree
                if (current == null) { return null; }
                
                // Found value
                if (current.Value == value)
                {
                    break;
                }
                
                if (value < current.Value)
                {
                    current = current.Lesser;
                    continue;
                }
                
                current = current.Greater;
            }
            
            return current;
        }
        
        public bool Remove(double value)
        {
            Node v = Find(value);
            
            if (v == null) { return false; }
            
            DeleteNode(v);
            
            return true;
        }
        private void DeleteNode(Node v)
        {
            if (v == null) { return; }
            
            bool isSource = v.Source == null;
            
            // From greater?
            bool greater = !isSource &&
                v.Source.Greater == v;
            
            ref Node srcRef = ref _source;
            
            if (greater)
            {
                srcRef = ref v.Source.Greater;
            }
            else if (!isSource)
            {
                srcRef = ref v.Source.Lesser;
            }
            
            // No children
            if (v.Greater == null &&
                v.Lesser == null)
            {
                if (isSource)
                {
                    _source = null;
                    return;
                }
                if (greater)
                {
                    v.Source.Greater = null;
                    return;
                }
                
                v.Source.Lesser = null;
                return;
            }
            
            // One child - greater
            if (v.Greater != null &&
                v.Lesser == null)
            {
                v.Greater.Source = v.Source;
                srcRef = v.Greater;
                
                OffsetDist(v.Greater, -1);
                return;
            }
            // One child - lesser
            if (v.Greater == null &&
                v.Lesser != null)
            {
                v.Lesser.Source = v.Source;
                srcRef = v.Lesser;
                
                OffsetDist(v.Lesser, 1);
                return;
            }
            
            // Two children
            
            DeleteNodeTwoChildren(greater, v, ref srcRef);
            return;
        }
        
        private void DeleteNodeTwoChildren(bool greater, Node v, ref Node srcRef)
        {
            // Lesser is valid replacement
            if (v.Lesser.Greater == null)
            {
                v.Lesser.Source = v.Source;
                srcRef = v.Lesser;
                
                OffsetDist(v.Lesser, 1);
                
                v.Greater.Source = v.Lesser;
                v.Lesser.Greater = v.Greater;
                return;
            }
            
            // Greater is valid replacement
            if (v.Greater.Lesser == null)
            {
                v.Greater.Source = v.Source;
                srcRef = v.Greater;
                
                OffsetDist(v.Greater, -1);
                
                v.Lesser.Source = v.Greater;
                v.Greater.Lesser = v.Lesser;
                return;
            }
            
            // Children not valid
            Node newN;
            if (greater)
            {
                newN = FindLargestTip(v.Lesser);
            }
            else
            {
                newN = FindSmallestTip(v.Greater);
            }
            v.Value = newN.Value;
            DeleteNode(newN);
        }
        
        private void OffsetDist(Node n, int o)
        {
            if (n == null) { return; }
            
            n.Distance += o;
            
            OffsetDist(n.Greater, o);
            OffsetDist(n.Lesser, o);
        }
        
        private Node FindLargestTip(Node n)
        {
            Node current = n;
            
            while (true)
            {
                // Reached end
                if (current.Lesser == null &&
                    current.Greater == null)
                {
                    return current;
                }
                
                if (current.Greater == null)
                {
                    current = current.Lesser;
                    continue;
                }
                
                current = current.Greater;
            }
        }
        private Node FindSmallestTip(Node n)
        {
            Node current = n;
            
            while (true)
            {
                // Reached end
                if (current.Lesser == null &&
                    current.Greater == null)
                {
                    return current;
                }
                
                if (current.Lesser == null)
                {
                    current = current.Greater;
                    continue;
                }
                
                current = current.Lesser;
            }
        }
    }
}