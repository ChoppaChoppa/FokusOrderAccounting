using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccounting.UIdesign
{
    public static class Rounding
    {
        public static GraphicsPath RoundedRectangle(System.Drawing.Rectangle rec, float RoudSize)
        {
            GraphicsPath GP = new GraphicsPath();

            GP.AddArc(rec.X, rec.Y, RoudSize, RoudSize, 180, 90);
            GP.AddArc(rec.X + rec.Width - RoudSize, rec.Y, RoudSize, RoudSize, 270, 90);
            GP.AddArc(rec.X + rec.Width - RoudSize, rec.Y + rec.Height - RoudSize, RoudSize, RoudSize, 0, 90);
            GP.AddArc(rec.X, rec.Y + rec.Height - RoudSize, RoudSize, RoudSize, 90, 90);

            GP.CloseFigure();

            return GP;
        }
    }
}
