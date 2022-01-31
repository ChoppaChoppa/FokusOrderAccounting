using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OrderAccounting.UIdesign
{
    class MenuButton : Control
    {
        StringFormat SF = new StringFormat();

        public int ID { get; set; }

        public Color ColorLeft { get; set; }
        public Color ColorRight { get; set; }

        bool MouseEntered = false;
        bool MousePressed = false;

        public MenuButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(150, 60);

            ColorLeft = Color.FromArgb(39, 38, 64);
            ColorRight = Color.FromArgb(77, 25, 77);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(224, 251, 252);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;

            Font = new Font("TimesNewRoman", 10F);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.Clear(Parent.BackColor);

            Rectangle rec = new Rectangle(-1, 0, Width, Height - 1);
            GraphicsPath path = Rounding.RoundedRectangle(rec, 40);
            LinearGradientBrush gradient = new LinearGradientBrush(rec, this.ColorLeft, this.ColorRight, 0F);

            g.DrawPath(new Pen(Color.FromArgb(54, 65, 86)), path);
            g.FillPath(gradient, path);

            if (MouseEntered)
            {
                //LinearGradientBrush gradient = new LinearGradientBrush(rec, Color.FromArgb(60, Color.Gray), Color.FromArgb(60, Color.Gray), 0F);

                g.DrawPath(new Pen(Color.FromArgb(54, 65, 86)), path);
                g.FillPath(new SolidBrush(Color.FromArgb(60, Color.White)), path);
            }

            if (MousePressed)
            {
                g.DrawPath(new Pen(Color.FromArgb(54, 65, 86)), path);
                g.FillPath(new SolidBrush(Color.FromArgb(30, Color.White)), path);
            }

            g.DrawString(Text, Font, new SolidBrush(Color.FromArgb(224, 251, 252)), rec, SF);

            g.SetClip(path);
            Region = new Region(path);

            
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            MouseEntered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            MouseEntered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            MousePressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            MousePressed = false;
            Invalidate();
        }
    }
}
