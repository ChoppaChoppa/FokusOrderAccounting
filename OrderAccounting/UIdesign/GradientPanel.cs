using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderAccounting.UIdesign
{
    class GradientPanel : Panel
    {
        public Color ColorTop { get; set; }
        public Color ColorBot { get; set; }

        StringFormat SF = new StringFormat();

        public string UserName { get; set; }

        public GradientPanel()
        {
            SF.Alignment = StringAlignment.Near;
            SF.LineAlignment = StringAlignment.Near;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = new Rectangle(0, 0, Width - 1, Height - 1);
            LinearGradientBrush gradient = new LinearGradientBrush(rec, this.ColorTop, this.ColorBot, 90F);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillRectangle(gradient, rec);
            g.DrawString(UserName, new Font("TimesNewRoman", 20F), new SolidBrush(Color.FromArgb(229, 218, 218)), rec, SF);
            base.OnPaint(e);
        }
    }
}
