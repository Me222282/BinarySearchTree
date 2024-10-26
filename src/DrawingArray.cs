using Zene.Graphics.Base;
using Zene.Graphics;
using System.Collections.Generic;
using Zene.Structs;
using System;

namespace BinarySearchTree
{
    public unsafe class DrawingArray : VertexArrayGL
    {
        public void AddBuffer<T>(ArrayBuffer<T> buffer, uint index, int dataStart, DataType dataType, AttributeSize attributeSize) where T : unmanaged
        {
            int typeSize = sizeof(T);

            buffer.Bind();
            EnableVertexAttribArray(index);
            if (GL.Version >= 3.3)
            {
                VertexAttribDivisor(index, 0);
            }
            VertexAttribPointer(index, attributeSize, dataType, false, typeSize * (int)buffer.DataSplit, dataStart * typeSize);
        }
        public void AddBuffer<T>(ArrayBuffer<T> buffer, uint index, int dataStart, int stride, DataType dataType, AttributeSize attributeSize) where T : unmanaged
        {
            buffer.Bind();
            EnableVertexAttribArray(index);
            if (GL.Version >= 3.3)
            {
                VertexAttribDivisor(index, 0);
            }
            VertexAttribPointer(index, attributeSize, dataType, false, stride, dataStart);
        }
        
        public void Draw(IDrawingContext context, DrawMode mode, int first, int size) => context.DrawArrays(this, mode, first, size);
        public void Draw<T>(IDrawingContext context, DrawMode mode, int index) where T : unmanaged
        {
            ArrayBuffer<T> buffer = (ArrayBuffer<T>)Properties.GetBuffer(index);
            
            context.DrawArrays(this, mode, 0, buffer.Size);
        }
        
        [ThreadStatic]
        private static double _xOff;
        [ThreadStatic]
        private static double _yOff;
        public static DrawingArray Create(DistanceTree dt, double x, double y)
        {
            List<Vector2> lines = new List<Vector2>();
            _xOff = x;
            _yOff = y;
            
            AddNode(dt._source, (0d, 0d), lines);
            
            // Buffer
            ArrayBuffer<Vector2> ab = new ArrayBuffer<Vector2>(1, BufferUsage.DrawFrequent);
            if (lines.Count > 0)
            {
                ab.SetData(lines.ToArray());
            }
            
            // Vertex array
            DrawingArray ad = new DrawingArray();
            ad.AddBuffer(ab, 0, 0, DataType.Double, AttributeSize.D2);
            
            return ad;
        }
        private static void AddNode(DistanceTree.Node node, Vector2 pos, List<Vector2> lines)
        {
            if (node == null) { return; }
            
            // When not starting node
            if (node.Source.Source != null)
            {
                lines.Add(pos);
            }
            
            if (node.Greater != null)
            {
                lines.Add(pos);
                AddNode(node.Greater, pos + (node.Greater.Offset * _xOff, _yOff), lines);
            }
            if (node.Lesser != null)
            {
                lines.Add(pos);
                AddNode(node.Lesser, pos + (node.Lesser.Offset * _xOff, _yOff), lines);
            }
        }
    }
}