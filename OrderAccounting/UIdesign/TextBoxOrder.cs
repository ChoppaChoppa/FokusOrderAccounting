using OrderAccounting.UIdesign.Anim;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OrderAccounting.UIdesign
{
    class TextBoxOrder : Control
    {
        #region -- Свойства --

        public string TextPreview { get; set; } = "Input text";

        private Font fontTextPreview = new Font("Arial", 8, FontStyle.Bold);
        public Font FontTextPreview
        {
            get => fontTextPreview;
            set
            {
                // Ограничение, чтобы размер шрифта заголовка нельзя было установить больше, 
                // чем размер основного шрифта
                if (value.Size >= Font.Size)
                    return;
                fontTextPreview = value;
            }
        }

        public Color BorderColor { get; set; } = Color.FromArgb(33, 158, 188);
        public Color BorderColorNotActive { get; set; } = Color.FromArgb(142, 202, 230);

        public string TextInput
        {
            get => tbInput.Text;
            set
            {
                tbInput.Text = value;
                Refresh();
            }
        }

        [Browsable(true)]
        public new event KeyEventHandler KeyUp
        {
            add { tbInput.KeyUp += value; }
            remove { tbInput.KeyUp -= value; }
        }

        public bool UseSystemPasswordChar
        {
            get => tbInput.UseSystemPasswordChar;
            set => tbInput.UseSystemPasswordChar = value;
        }

        //[Browsable(false)]
        //public new string Text { get; set; }

        public new string Text
        {
            get => tbInput.Text;
            set
            {
                tbInput.Text = value;
                Refresh();
            }
        }

        #endregion

        #region -- События / Events --

        [Browsable(true)]
        public new event EventHandler TextChanged
        {
            add { tbInput.TextChanged += value; }
            remove { tbInput.TextChanged -= value; }
        }

        #endregion

        #region -- Переменные --

        StringFormat SF = new StringFormat();

        int TopBorderOffset = 0;
        public int RoundingPrecent { get; set; }

        TextBox tbInput = new TextBox();

        Animation LocationTextPreviewAnim = new Animation();
        Animation FontSizeTextPreviewAnim = new Animation();

        #endregion

        public TextBoxOrder()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(350, 40);
            Font = new Font("Arial", 11.25F, FontStyle.Regular);
            ForeColor = Color.FromArgb(237, 246, 249);
            BackColor = Color.FromArgb(2, 48, 71);

            Cursor = Cursors.IBeam;

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;

            AdjustTextBoxInput();
            Controls.Add(tbInput);

            LocationTextPreviewAnim.Value = tbInput.Location.Y;
            FontSizeTextPreviewAnim.Value = Font.Size;

            RoundingPrecent = 10;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            TextPreviewAction(TextInput.Length > 0);
        }

        private void AdjustTextBoxInput()
        {
            tbInput = new TextBox();
            tbInput.Name = "InputBox";
            tbInput.BorderStyle = BorderStyle.None;
            tbInput.BackColor = BackColor;
            tbInput.ForeColor = ForeColor;
            tbInput.Font = Font;
            tbInput.Visible = false;
            

            int offset = TextRenderer.MeasureText(TextPreview, FontTextPreview).Height / 2;
            tbInput.Location = new Point(5, Height / 2 - offset);
            tbInput.Size = new Size(Width - 10, tbInput.Height);

            tbInput.LostFocus += TbInput_LostFocus;
        }

        private void TbInput_LostFocus(object sender, EventArgs e)
        {
            TextPreviewAction(false);
        }

        #region -- Обновление свойств tbInput --
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            tbInput.BackColor = BackColor;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            tbInput.ForeColor = ForeColor;

        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            tbInput.Font = Font;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            tbInput.Size = new Size(Width - 10, tbInput.Height);
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graph.Clear(Parent.BackColor);

            TopBorderOffset = graph.MeasureString(TextPreview, FontTextPreview).ToSize().Height / 2;

            Font FontTextPreviewActual = new Font(FontTextPreview.FontFamily, FontSizeTextPreviewAnim.Value, FontTextPreview.Style);

            if (tbInput.Visible == false && FontTextPreviewActual.Size <= FontTextPreview.Size)
            {
                tbInput.Visible = true;
                tbInput.Focus();
            }
            else if (tbInput.Visible == true && FontTextPreviewActual.Size > FontTextPreview.Size)
            {
                tbInput.Visible = false;
            }

            Rectangle rectBase = new Rectangle(0, TopBorderOffset, Width - 1, Height - 1 - TopBorderOffset);

            Size TextPreviewRectSize = graph.MeasureString(TextPreview, FontTextPreviewActual).ToSize();
            Rectangle rectTextPreview = new Rectangle(5, (int)LocationTextPreviewAnim.Value, TextPreviewRectSize.Width + 3, TextPreviewRectSize.Height);

            GraphicsPath pathBase = Rounding.RoundedRectangle(rectBase, RoundingPrecent);
            GraphicsPath pathTextPrev = Rounding.RoundedRectangle(rectTextPreview, RoundingPrecent);

            // Обводка
            graph.DrawPath(new Pen(tbInput.Focused == true ? BorderColor : BorderColorNotActive), pathBase);

            // Заголовок/Описание
            if (TextPreview.Length > 0)
            {
                graph.DrawPath(new Pen(Parent.BackColor), pathTextPrev);
                graph.FillPath(new SolidBrush(Parent.BackColor), pathTextPrev);
            }

            // Цвет внутри
            graph.FillPath(new SolidBrush(BackColor), pathBase);

            graph.SetClip(pathBase);
            graph.SetClip(pathTextPrev);

            if (TextPreview.Length > 0)
                graph.DrawString(TextPreview, FontTextPreviewActual, new SolidBrush(tbInput.Focused == true ? BorderColor : BorderColorNotActive), rectTextPreview, SF);
        }

        private void TextPreviewAction(bool OnTop)
        {
            if (OnTop)
            {
                if (tbInput.Visible == false)
                {
                    LocationTextPreviewAnim = new Animation("TextPreviewLocationY" + Handle, Invalidate, LocationTextPreviewAnim.Value, 0);
                    FontSizeTextPreviewAnim = new Animation("TextPreviewFontSize" + Handle, Invalidate, FontSizeTextPreviewAnim.Value, FontTextPreview.Size);
                }
                else
                {
                    tbInput.Focus();
                    return;
                }
            }
            else
            {
                if (TextInput.Length == 0)
                {
                    LocationTextPreviewAnim = new Animation("TextPreviewLocationY" + Handle, Invalidate, LocationTextPreviewAnim.Value, tbInput.Location.Y);
                    FontSizeTextPreviewAnim = new Animation("TextPreviewFontSize" + Handle, Invalidate, FontSizeTextPreviewAnim.Value, Font.Size);
                }
                else
                {
                    return;
                }
            }

            LocationTextPreviewAnim.StepDivider = 4;
            FontSizeTextPreviewAnim.StepDivider = 4;

            Animator.Request(LocationTextPreviewAnim, true);
            Animator.Request(FontSizeTextPreviewAnim, true);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            TextPreviewAction(true);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            TextPreviewAction(true);
        }
    }
}
