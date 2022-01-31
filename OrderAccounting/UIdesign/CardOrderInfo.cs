using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrderAccounting.UIdesign
{
    class CardOrderInfo : Control
    {
        StringFormat SF = new StringFormat();

        bool MouseEntered;
        bool MousePressed;

        public int ID { get; set; }
        public string Phone { get; set; }
        public int Price { get; set; }
        public int Prepayment { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime DateOfOrderGet { get; set; }
        public DateTime DateOfOrderFinished { get; set; }
        public DateTime LayoutRaedy { get; set; }
        public bool Refusal { get; set; }
        public bool Ready { get; set; }
        public bool Issued { get; set; }
        public bool ToTheWorkShop { get; set; }
        public Image Picture { get; set; }

        public Rectangle[] recLoc { get; set; }

        public CardOrderInfo()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(Form1.FormWidth * 8 / 9, 100);

            BackColor = Color.FromArgb(0, 100, 102);
            ForeColor = Color.FromArgb(224, 251, 252);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;

            Font = new Font("TimesNewRoman", 20F);

            recLoc = new Rectangle[11];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.Clear(Parent.BackColor);

            Rectangle rec = new Rectangle(0, 0, Width - 1, Height - 1);

            GraphicsPath path = Rounding.RoundedRectangle(rec, 20);

            g.DrawPath(new Pen(Color.White), path);
            g.FillPath(new SolidBrush(BackColor), path);

            if (MouseEntered)
            {
                g.DrawPath(new Pen(Color.FromArgb(60, Color.White)), path);
                g.FillPath(new SolidBrush(Color.FromArgb(60, Color.White)), path);
            }

            if (MousePressed)
            {
                g.DrawPath(new Pen(Color.FromArgb(30, Color.White)), path);
                g.FillPath(new SolidBrush(Color.FromArgb(30, Color.White)), path);
            }

            DrawStr(g, 30  , 0, 190, Height, Phone);
            DrawStr(g, 230 , 0, 100, Height, Price.ToString());
            DrawStr(g, 340 , 0, 100, Height, Prepayment.ToString());
            DrawStr(g, 450 , 0, 210, Height, ShortDescription);
            DrawStr(g, 670 , 0, 150, Height, DateOfOrderGet.ToShortDateString());
            DrawStr(g, 830, 0, 150, Height, LayoutRaedy.ToShortDateString());
            DrawStr(g, 990 , 0, 150, Height, DateOfOrderFinished.ToShortDateString());
            DrawStr(g, 1150, 0, 150, Height, Ready ? "Готво" : "не гтово");
            DrawStr(g, 1310, 0, 150, Height, Refusal ? "Отказ" : "Одобрено");
            DrawStr(g, 1470, 0, 150, Height, Issued ? "Выдан" : "Не выдан");
            DrawStr(g, 1630, 0, 70 , Height, ToTheWorkShop ? "Да" : "Нет");
        }

        private void DrawStr(Graphics g, int x, int y, int w, int h, string text)
        {
            Rectangle strRec = new Rectangle(x, y, w, h);
            recLoc.Append(strRec);
            g.DrawRectangle(new Pen(Color.FromArgb(60, Color.White)), strRec);
            g.DrawString(text, Font, new SolidBrush(ForeColor), strRec, SF);
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
