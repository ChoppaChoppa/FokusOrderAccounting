using OrderAccounting.UIdesign;
using OrderAccounting.UIdesign.Anim;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestC.fokus;

namespace OrderAccounting
{
    public partial class SignUpWindow : Form
    {
        DBconn dataBase;

        TextBoxOrder tbLogin;
        TextBoxOrder tbPass;
        MenuButton btnSignUp;
        Label lbCheck;

        public int id { get; private set; }
        public Dictionary<string, string> userData { get; private set; }

        public SignUpWindow()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            //this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(24, 32, 38);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataBase = new DBconn();

            tbLogin = new TextBoxOrder();
            tbPass = new TextBoxOrder();
            btnSignUp = new MenuButton();
            lbCheck = new Label();

            //Animator.Start();
        }

        private void SignUpWindow_Load(object sender, EventArgs e)
        {
            tbLogin.Size = new Size(200, 40);
            tbLogin.Location = new Point(this.Width / 2 - tbLogin.Size.Width / 2 - 5, this.Height / 3 - tbLogin.Size.Height);
            tbLogin.TextPreview = "Логин";
            this.Controls.Add(tbLogin);

            tbPass.Size = new Size(200, 40);
            tbPass.Location = new Point(this.Width / 2 - tbPass.Size.Width / 2 - 5, this.Height / 3);
            tbPass.TextPreview = "Пароль";
            tbPass.UseSystemPasswordChar = true;
            this.Controls.Add(tbPass);

            btnSignUp.Size = new Size(150, btnSignUp.Size.Height);
            btnSignUp.Location = new Point(this.Width / 2 - btnSignUp.Size.Width / 2 - 5, tbPass.Location.Y + tbPass.Size.Height + 5);
            btnSignUp.Text = "Войти";
            btnSignUp.Click += btnSignUp_Click;
            this.Controls.Add(btnSignUp);

            lbCheck.AutoSize = true;
            lbCheck.Location = new Point(this.Width / 2 - lbCheck.Size.Width / 2 - 5, tbLogin.Location.Y - 40);
            lbCheck.ForeColor = Color.FromArgb(33, 158, 188);
            this.Controls.Add(lbCheck);

            //this.ActiveControl = this;
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            userData = dataBase.GetUser(tbLogin.Text, tbPass.Text);

            if (userData != null)
            {
                lbCheck.Text = "Вход выполнен";
                lbCheck.Location = new Point(this.Width / 2 - lbCheck.Size.Width / 2 - 5, tbLogin.Location.Y - 40);
                lbCheck.ForeColor = Color.FromArgb(33, 158, 188);
                Form1.GetData(userData);
                Thread.Sleep(20);
                this.Close();
            }
            else
            {
                lbCheck.ForeColor = Color.FromArgb(230, 57, 70);
                lbCheck.Text = @"                      Ошибка Входа!
Проверти корректность введеных данных!";
                lbCheck.Location = new Point(this.Width / 2 - lbCheck.Size.Width / 2 - 5, lbCheck.Location.Y);
            }
        }

        public void MsgCallBack(string msg, int id)
        {

        }

        private void SignUpWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
