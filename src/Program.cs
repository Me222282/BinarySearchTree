using System;
using Zene.Structs;
using Zene.Graphics;
using Zene.Windowing;
using Zene.GUI;

namespace BinarySearchTree
{
    class Program : Window
    {
        static void Main(string[] args)
        {
            Core.Init();
            
            Window w = new Program(200, 100, "WORK");
            w.Run();
            
            Core.Terminate();
        }
        
        private void AddNestedElement(Element parent, Element child, int count)
        {
            if (count == 0)
            {
                parent.AddChild(child);
                return;
            }
            
            Container e = new Container(new Layout(0d, 0d, 2d, 2d));
            parent.AddChild(e);
            AddNestedElement(e, child, count - 1);
        }
        
        public Program(int width, int height, string title)
            : base(width, height, title, 4.3)
        {
            _em = new RootElement(this);
            _em.AddChild(_treeRender = new TreeRenderElement());
            
            Element _uiBar;
            _em.AddChild(_uiBar = new Container(
                new MenuLayout(60d))
            {
                Colour = new Colour(53, 55, 57)
            });
            
            _uiBar.AddChild(_input = new TextInput(
                new TextLayout((5d, 5d), Vector2.Zero, (50d, 0d)))
            {
                TextSize = 30d,
                CornerRadius = 0.2,
                BackgroundColour = new ColourF(0f, 0f, 0f, 0f),
                SingleLine = true
            });
            
            ColourF ic = new Colour(170, 192, 227);
            ColourF bc = new Colour(130, 142, 177);
            
            Button addButton;
            _uiBar.AddChild(addButton = new Button(new Layout(-0.67d, 0d, 0.2d, 1.4d))
            {
                Colour = ic,
                BorderColour = bc,
                Text = "Add"
            });
            addButton.Click += (_, _) => AddNode();
            
            Button deleteButton;
            _uiBar.AddChild(deleteButton = new Button(new Layout(-0.43d, 0d, 0.2d, 1.4d))
            {
                Colour = ic,
                BorderColour = bc,
                Text = "Delete"
            });
            deleteButton.Click += (_, _) => DeleteNode();
            
            Button findButton;
            _uiBar.AddChild(findButton = new Button(new Layout(0.5d, 0d, 0.2d, 1.4d))
            {
                Colour = ic,
                BorderColour = bc,
                Text = "Find"
            });
            findButton.Click += (_, _) => FindNode();
            
            Button clearButton;
            _uiBar.AddChild(clearButton = new Button(new Layout(0.74d, 0d, 0.2d, 1.4d))
            {
                Colour = ic,
                BorderColour = bc,
                Text = "Clear"
            });
            clearButton.Click += (_, _) => ClearTree();
        }
        
        private RootElement _em;
        private TreeRenderElement _treeRender;
        private TextInput _input;
        
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);
            
            Framebuffer.Bind();
            Framebuffer.Clear(BufferBit.Colour | BufferBit.Depth);
            
            _em.Render();
        }
        
        private bool AddNode()
        {
            if (double.TryParse(_input.Text.ToString(), out double v))
            {
                _treeRender.Bst.Add(v);
                _input.Text = "";
                return true;
            }
            
            return false;
        }
        private void DeleteNode()
        {
            if (double.TryParse(_input.Text.ToString(), out double v))
            {
                _treeRender.Bst.Remove(v);
                _input.Text = "";
            }
        }
        private void FindNode()
        {
            if (double.TryParse(_input.Text.ToString(), out double v))
            {
                _treeRender.SelectedNode = _treeRender.Bst.Find(v);
                _input.Text = "";
            }
        }
        private void ClearTree()
        {
            _treeRender.Bst.Clear();
            _input.Text = "";
            return;
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (e[Keys.Enter] || e[Keys.NumPadEnter])
            {
                _treeRender.SelectedNode = null;
                
                // Try add node
                if (AddNode()) { return; }
                
                if (_input.Text.ToString().Trim().ToLower() == "c")
                {
                    ClearTree();
                    return;
                }
                return;
            }
            
            if (e[Keys.Delete])
            {   
                DeleteNode();
                return;
            }
            
            if (e[Keys.F] && this[Mods.Control])
            {
                FindNode();
                return;
            }
        }
    }
}
