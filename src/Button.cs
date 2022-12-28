/*using System;
using Zene.Structs;
using Zene.Graphics;
using Zene.Windowing;
using Zene.GUI;

namespace BinarySearchTree
{
    class Button : Element
    {
        public Button(IBox bounds)
            : base(bounds)
        {
            Font = new SampleFont();
        }

        public Button(ILayout layout)
            : base(layout)
        {
            Font = new SampleFont();
        }
        
        public Font Font { get; set; }
        
        public string Text { get; set; }
        public double TextSize { get; set; } = 20d;
        
        public Colour Background { get; set; } = new Colour(170, 192, 227);
        public Colour Foreground { get; set; } = new Colour(33, 33, 33);
        
        public event MouseEventHandler Click;
        
        private Colour GetDrawColour()
        {
            Colour colour = Background;
            
            if (MouseHover)
            {
                colour.R -= 10;
                colour.G -= 10;
                colour.B -= 10;
            }
            
            if (MouseSelect)
            {
                colour.R -= 10;
                colour.G -= 10;
                colour.B -= 10;
            }
            
            return colour;
        }
        
        protected override void OnUpdate(FrameEventArgs e)
        {
            base.OnUpdate(e);
            
            e.Framebuffer.Clear(GetDrawColour());
            TextRenderer.Colour = Foreground;
            
            TextRenderer.Model = Matrix4.CreateScale(TextSize);
            TextRenderer.DrawCentred(Text, Font, 0, 0);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            
            Console.WriteLine(Framebuffer.Size);
            Console.WriteLine(Framebuffer.View);
            
            Click?.Invoke(this, e);
        }
    }
}
*/