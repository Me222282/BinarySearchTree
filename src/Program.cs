using System;
using Zene.Windowing;
using Zene.GUI;
using System.IO;

namespace BinarySearchTree
{
    class Program : GUIWindow
    {
        static void Main(string[] args)
        {
            Core.Init();
            
            Window w = new Program(800, 500, "WORK");
            w.Run();
            
            Core.Terminate();
        }
        
        public Program(int width, int height, string title)
            : base(width, height, title, 4.3)
        {
            LoadXml(File.ReadAllText("GUI.xml"));
            
            _treeRender = Find<TreeRenderElement>();
            _input = Find<TextInput>();
        }
        
        private TreeRenderElement _treeRender;
        private TextInput _input;
        
        private void AddNode(object sender, MouseEventArgs e) => AddNode();
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
        private void DeleteNode(object sender, MouseEventArgs e)
        {
            if (double.TryParse(_input.Text.ToString(), out double v))
            {
                _treeRender.Bst.Remove(v);
                _input.Text = "";
            }
        }
        private void FindNode(object sender, MouseEventArgs e)
        {
            if (double.TryParse(_input.Text.ToString(), out double v))
            {
                _treeRender.SelectedNode = _treeRender.Bst.Find(v);
                _input.Text = "";
            }
        }
        private void ClearTree(object sender, MouseEventArgs e)
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
                    ClearTree(this, null);
                    return;
                }
                return;
            }
            
            if (e[Keys.Delete])
            {   
                DeleteNode(this, null);
                return;
            }
            
            if (e[Keys.F] && this[Mods.Control])
            {
                FindNode(this, null);
                return;
            }
        }
    }
}
