using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace OrderAccounting.UIdesign
{
    class MoraMonthCalendar : Control
    {
        StringFormat SF = new StringFormat();
        StringFormat SFAllDay = new StringFormat();
        StringFormat SFMonth = new StringFormat();

        public List<DateTime> SelectDate { get; set; }
        public bool SelectedMonth { get; set; }

        private string[] Months = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public int Month { get; set; }
        public int Year { get; set; }

        public MoraMonthCalendar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(1400, 850);

            BackColor = Color.FromArgb(0, 38, 66);
            ForeColor = Color.FromArgb(224, 251, 252);

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;

            SFAllDay.Alignment = StringAlignment.Near;
            SFAllDay.LineAlignment = StringAlignment.Far;

            SFMonth.Alignment = StringAlignment.Far;
            SFMonth.LineAlignment = StringAlignment.Far;

            Font = new Font("TimesNewRoman", 20F);

            SelectedMonth = false;
            SelectDate = new List<DateTime>();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Parent.BackColor);

            Rectangle rec = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath path = Rounding.RoundedRectangle(rec, 30);


            g.DrawPath(new Pen(BackColor), path);
            g.FillPath(new SolidBrush(BackColor), path);

            int startX = 5;
            int startY = 5;

            int dayInMonth = DateTime.DaysInMonth(Year, Month);

            for (int i = 0; i < dayInMonth; i++)
            {
                Rectangle dayRec = new Rectangle(startX, startY, 190, 150);
                GraphicsPath dayPath = Rounding.RoundedRectangle(dayRec, 30);

                string date = String.Format("{0}.{1}.{2}", (i + 1), Month, Year);

                if (SelectDate.Contains(DateTime.Parse(date)))
                {
                    g.DrawPath(new Pen(Color.FromArgb(132, 0, 50)), dayPath);
                    g.FillPath(new SolidBrush(Color.FromArgb(132, 0, 50)), dayPath);
                }
                else
                {
                    g.DrawPath(new Pen(Color.FromArgb(229, 149, 0)), dayPath);
                    g.FillPath(new SolidBrush(Color.FromArgb(229, 149, 0)), dayPath);
                }

                g.DrawString((i + 1).ToString(), Font, new SolidBrush(ForeColor), dayRec, SF);

                startX += 200;

                if ((i + 1) % 7 == 0)
                {
                    startX = 5;
                    startY += 160;
                }
            }

            g.DrawString("Проработано дней в этом месяце: " + SelectDate.Count, Font, new SolidBrush(ForeColor), rec, SFAllDay);
            g.DrawString(Months[Month - 1], Font, new SolidBrush(ForeColor), rec, SFMonth);
        }
    }
}
