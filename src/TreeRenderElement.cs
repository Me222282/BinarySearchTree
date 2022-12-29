using System;
using Zene.Structs;
using Zene.Graphics;
using Zene.Windowing;
using Zene.GUI;

namespace BinarySearchTree
{
    class TreeRenderElement : Element
    {
        public TreeRenderElement()
        {
            _font = SampleFont.GetInstance();
            _shader = BasicShader.GetInstance();
            _circleShader = CircleShader.GetInstance();
            
            UpdateDrawingTree();
            
            Layout = new Layout(0d, 0d, 2d, 2d);
            
            CursorStyle = Cursor.ResizeAll;
        }
        
        private BasicShader _shader { get; }
        
        public BinarySearchTree.Node SelectedNode { get; set; }= null;
        
        private BinarySearchTree _bst = new BinarySearchTree();
        private bool _updateTree = false;
        public BinarySearchTree Bst
        {
            get
            {
                _updateTree = true;
                return _bst;
            }
        }
        
        private readonly Font _font;
        private readonly CircleShader _circleShader;
        private DrawingArray _lines;
        
        private DistanceTree _tree;
        
        private double _xDist = 25d;
        private double _yDist = -30d;
        public double TextSize { get; set; } = 20d;
        
        private readonly Colour _baseColour = new Colour(255, 255, 255);
        private readonly Colour _selectColour = new Colour(237, 175, 52);
        
        private void UpdateDrawingTree()
        {
            _tree = new DistanceTree(_bst);
            _lines = DrawingArray.Create(_tree, _xDist, _yDist);
        }
        
        protected override void OnUpdate(FrameEventArgs e)
        {
            base.OnUpdate(e);
            
            if (_updateTree)
            {
                UpdateDrawingTree();
                _updateTree = false;
            }
            
            TextRenderer.Model = Matrix4.CreateScale(TextSize);
            
            if (_move)
            {
                ViewPan += MouseChange();
                _mouseOld = MouseLocation;
            }
            
            _circleShader.Matrix2 = Matrix4.Identity;
            _shader.Matrix2 = Matrix4.Identity;
            
            _circleShader.Size = 1d;
            _circleShader.LineWidth = 0.05d;
            _circleShader.ColourSource = ColourSource.UniformColour;
            _circleShader.Colour = new ColourF(1f, 1f, 1f);
            
            State.DepthTesting = true;
            // Depth always passes
            State.DepthFunction = DepthFunction.Always;
            
            _circleShader.Matrix3 = Projection;
            DrawNode(_tree._source, 0d);
            
            // Depth passes when nothing is written
            State.DepthFunction = DepthFunction.Less;
            
            // Draw lines
            
            _shader.Bind();
            _shader.ColourSource = ColourSource.UniformColour;
            _shader.Colour = new ColourF(1f, 1f, 1f);
            _shader.Matrix1 = Matrix4.Identity;
            _shader.Matrix3 = Projection;
            _lines.Draw<Vector2>(DrawMode.Lines, 0);
        }
        
        private void DrawNode(DistanceTree.Node n, Vector2 pos)
        {
            if (n == null) { return; }
            
            // Set colour if selected
            Colour c = n.Source == SelectedNode ? _selectColour : _baseColour;
            TextRenderer.Colour = c;
            _circleShader.Colour = c;
            
            // Offset matrix
            TextRenderer.Model = Matrix4.CreateScale(TextSize) * Matrix4.CreateTranslation(pos);
            
            // Circle border
            _circleShader.Bind();
            _circleShader.Matrix1 = Matrix4.CreateScale(TextSize * 1.5) * Matrix4.CreateTranslation(pos);
            Shapes.Square.Draw();
            
            // Draw text
            TextRenderer.DrawCentred(n.Source.Value.ToString(), _font, 0, 0);
            
            if (n.Greater != null)
            {
                DrawNode(n.Greater, pos + (n.Greater.Offset * _xDist, _yDist));
            }
            if (n.Lesser != null)
            {
                DrawNode(n.Lesser, pos + (n.Lesser.Offset * _xDist, _yDist));
            }
        }
        
        private Vector2 _mouseOld;
        private bool _move;
        private const double _panValue = 20d;
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            
            _mouseOld = e.Location;
            _move = true;
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            
            if (_move) { _move = false; }
        }
        
        private Vector2 MouseChange() => MouseLocation - _mouseOld;
        
        private void ZoomOnScreenPoint(Vector2 point, double zoom)
        {
            double newZoom = ViewScale + (zoom * 0.1 * ViewScale);

            if (newZoom < 0) { return; }

            double oldZoom = ViewScale;
            ViewScale = newZoom;

            Vector2 pointRelOld = (point - ViewPan) / oldZoom;
            Vector2 pointRelNew = (point - ViewPan) / newZoom;

            ViewPan += (pointRelNew - pointRelOld) * newZoom;
        }
        
        //private const double _minDist = 1d / 1_000_000d;
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            
            if (this[Mods.Control])
            {
                ZoomOnScreenPoint(MouseLocation, e.DeltaY);
                return;
            }
            
            // X pan
            if (this[Mods.Shift])
            {
                ViewPan += (e.DeltaY * _panValue, 0d);
                return;
            }
            
            // Y pan
            ViewPan -= (0d, e.DeltaY * _panValue);
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (e[Keys.Left])
            {
                ViewPan += (_panValue, 0d);
                return;
            }
            if (e[Keys.Right])
            {
                ViewPan -= (_panValue, 0d);
                return;
            }
            if (e[Keys.Up])
            {
                ViewPan -= (0d, _panValue);
                return;
            }
            if (e[Keys.Down])
            {
                ViewPan += (0d, _panValue);
                return;
            }
            if (e[Mods.Control])
            {
                if (e[Keys.Equal] || e[Keys.NumPadAdd])
                {
                    ZoomOnScreenPoint(0d, 1d);
                    return;
                }
                if (e[Keys.Minus] || e[Keys.NumPadSubtract])
                {
                    ZoomOnScreenPoint(0d, -1d);
                    return;
                }
            }
        }
    }
}
