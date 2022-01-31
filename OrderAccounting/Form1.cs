using OrderAccounting.AboutOrderMenu;
using OrderAccounting.UIdesign;
using OrderAccounting.UIdesign.Anim;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TestC.fokus;

namespace OrderAccounting
{
    public partial class Form1 : Form
    {
        AboutOrder chooseOrder;
        DBconn dataBase = new DBconn();
        SignUpWindow signUpWindow = new SignUpWindow();

        Graphics g;

        StringFormat SF = new StringFormat();

        DateTimePicker PickDateOrderGet;
        DateTimePicker PickDateOrderFinished;
        DateTimePicker PickDateLayoutReady;

        MenuButton[] btnUserArr;
        MenuButton[] btnMonthsArr;
        MenuButton mBtnAdd;
        MenuButton mBtnArchive;
        MenuButton mBtnVisisble;
        MenuButton mBtnBack;
        MenuButton mBtnSave;
        MenuButton mBtnAddOrder;
        MenuButton mBtnAddOrderBack;
        MenuButton mBtnArchiveBack;
        MenuButton mBtnWhoWork;
        MenuButton mBtnMoneyEarned;
        MenuButton mBtnWhoWorkBack;
        MenuButton mBtnMark;

        Toggle tRefusal;
        Toggle tReady;
        Toggle tIssued;
        Toggle tWShop;

        TableLayoutPanel pCardOrderBox;
        TableLayoutPanel pCardOrderBoxArchive;

        MoraMonthCalendar userDateWork;

        GradientPanel gradPanel;

        PictureBox pBox;

        Label lbUsrName;
        Label lbPrep;

        Dictionary<string, TextBoxOrder> tbDict = new Dictionary<string, TextBoxOrder>()
        {
            {"Номер" , new TextBoxOrder() },
            {"Цена" , new TextBoxOrder() },
            {"Оплачено" , new TextBoxOrder() },
            {"Описание" , new TextBoxOrder() },
            {"Подробное описание" , new TextBoxOrder() },
            {"Дата получения" , new TextBoxOrder() },
            {"Готовность макета" , new TextBoxOrder() },
            {"Дата завершения" , new TextBoxOrder() },
            {"Отказ" , new TextBoxOrder() },
            {"Готовность" , new TextBoxOrder() },
            {"Выдача" , new TextBoxOrder() },
            {"Мастер" , new TextBoxOrder() },
        };

        List<AboutUser> userName;

        private int userSelectId;
        private int ScrollChange;
        private static int formWidth;
        private static int formHeigth;

        public static int FormWidth { get => formWidth; }
        public static int FormHeigth { get => formHeigth; }
        public static int ID { get; private set; }

        private string[] Months = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        private string[] labelName = new string[] { "Номер", "Цена", "Оплачено", "Описание", "Дата получения", "Готовность макета", "Дата завершения",  "Готовность", "Отказ", "Выдача", "Мастер" };
        private string[] AboutOrderName = new string[] { "", "Номер", "Цена", "Оплачено", "Описание", "Подробное описание", "Дата получения", "Готовность макета", "Дата завершения",  "Отказ", "Готовность", "Выдача", "Мастер" };
        private string[] UserAdminName = new string[] { "Фатима", "Мадина", "Амина" };
        public static string Login { get; private set; }
        public static string Password { get; private set; }

        bool MoveDown = true;
        bool MoveUp = false;
        bool UserAdmin = false;

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(17, 21, 28);

            timer1.Interval = 10;

            g = CreateGraphics();

            SF.Alignment = StringAlignment.Near;
            SF.LineAlignment = StringAlignment.Center;

            Animator.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Login = "bala";
            this.Hide();
            signUpWindow.ShowDialog();
            if (Login == null)
            {
                this.Close();
                return;
            }
            this.Show();

            if (UserAdminName.Contains(Login)) UserAdmin = true;

            formWidth = this.Width;
            formHeigth = this.Height;

            userName = dataBase.GetUserName();

            if (Login == "bala")
                DrawOrderCards(dataBase.GetOrdersToWS());
            else
                DrawOrderCards(dataBase.GetAllOrders());

            DrawTopMenu();
            LoadAboutMenu();
        }

        public static void GetData(Dictionary<string, string> user)
        {
            ID = int.Parse(user["id"]);
            Login = user["login"];
            Password = user["pass"];
        }

        private void GetNewOrderValues(ref AboutOrder order, Dictionary<string, TextBoxOrder> dict)
        {
            order.Phone = dict["Номер"].Text;
            order.Price = int.Parse(dict["Цена"].Text);
            order.Prepayment = int.Parse(dict["Оплачено"].Text);
            order.ShortDescription = dict["Описание"].Text;
            order.LongDescription = dict["Подробное описание"].Text;
            order.DateOfOrderGet = PickDateOrderGet.Value;
            order.DateOfOrderFinished = PickDateOrderFinished.Value;
            order.LayoutRaedy = PickDateLayoutReady.Value;
            order.Refusal = tRefusal.Checked;
            order.Ready = tReady.Checked;
            order.Issued = tIssued.Checked;
            order.ToTheWorkShop = tWShop.Checked;
            order.Picture = pBox.Image;
        }

        //Подгрузка кнопок и пикБокса для всплывающего меню
        private void LoadAboutMenu()
        {
            mBtnBack = new MenuButton();
            mBtnBack.Location = new Point(100, 50);
            mBtnBack.Text = "Назад";
            mBtnBack.Name = "mBtnBack";
            mBtnBack.Click += MBtnBack_Click;

            mBtnSave = new MenuButton();
            mBtnSave.Location = new Point(Width - mBtnSave.Width - 100, 50);
            mBtnSave.Text = "Сохранить";
            mBtnSave.Name = "mBtnSave";
            mBtnSave.Click += MBtnSave_Click;

            mBtnAddOrder = new MenuButton();
            mBtnAddOrder.Location = new Point(this.Width - mBtnAddOrder.Width - 100, 50);
            mBtnAddOrder.Text = "Добавить";
            mBtnAddOrder.Name = "mBtnAddOrder";
            mBtnAddOrder.Click += MBtnAddOrder_Click;

            mBtnAddOrderBack = new MenuButton();
            mBtnAddOrderBack.Location = new Point(100, 50);
            mBtnAddOrderBack.Text = "Назад";
            mBtnAddOrderBack.Name = "mBtnAddOrderBack";
            mBtnAddOrderBack.Click += MBtnAddOrderBack_Click;

            pBox = new PictureBox();
            pBox.Location = new Point(50, 170);
            pBox.Size = new Size(this.Width / 2 - 100, 500);
            pBox.BackColor = Color.Black;
        }

        //Создание Верхних кнопок перехода
        private void DrawTopMenu()
        {
            mBtnAdd = new MenuButton();
            mBtnAdd.Location = new Point(this.Width * 1 / 18 + 13, -90);
            mBtnAdd.Text = "Добавить Заказ";
            mBtnAdd.Name = "mBtnAdd";
            mBtnAdd.Click += MBtnAdd_Click;
            Controls.Add(mBtnAdd);

            mBtnArchive = new MenuButton();
            mBtnArchive.Location = new Point(this.Width * 1 / 18 + mBtnAdd.Location.X + mBtnAdd.Width + 50, -90);
            mBtnArchive.Text = "Архив";
            mBtnArchive.Name = "mBtnArchive";
            mBtnArchive.Click += MBtnArchive_Click;
            Controls.Add(mBtnArchive);

            mBtnVisisble = new MenuButton();
            mBtnVisisble.Size = new Size(50, 50);
            mBtnVisisble.Location = new Point(this.Width / 2 - mBtnVisisble.Width / 2, 10);
            mBtnVisisble.Text = "ᐯ";
            mBtnVisisble.Name = "mBtnVisisble";
            mBtnVisisble.Click += MBtnVisisble_Click;
            Controls.Add(mBtnVisisble);

            if (UserAdmin)
            {
                mBtnWhoWork = new MenuButton();
                mBtnWhoWork.Location = new Point(this.Width * 1 / 18 + mBtnArchive.Location.X + mBtnArchive.Width + 50, -90);
                mBtnWhoWork.Text = "График работников";
                mBtnWhoWork.Name = "mBtnWhoWork";
                mBtnWhoWork.Click += mBtnWhoWork_Click;
                Controls.Add(mBtnWhoWork);

                mBtnMoneyEarned = new MenuButton();
                mBtnMoneyEarned.Location = new Point(this.Width * 1 / 18 + mBtnWhoWork.Location.X + mBtnWhoWork.Width + 50, -90);
                mBtnMoneyEarned.Text = "Заработано Денег";
                mBtnMoneyEarned.Name = "mBtnMoneyEarned";
                mBtnMoneyEarned.Click += MBtnMoneyEarned_Click;
                Controls.Add(mBtnMoneyEarned);
            }

            gradPanel = new GradientPanel();
            gradPanel.Location = new Point(0, 0);
            gradPanel.Size = new Size(this.Width, 170);
            gradPanel.ColorTop = Color.FromArgb(0, 38, 66);
            gradPanel.ColorBot = this.BackColor;
            gradPanel.UserName = Login;

            Controls.Add(gradPanel);
        }

        private void MBtnMoneyEarned_Click(object sender, EventArgs e)
        {
            
        }

        private void mBtnWhoWork_Click(object sender, EventArgs e)
        {
            MainMenuVisible(false);

            mBtnWhoWorkBack = new MenuButton();
            mBtnWhoWorkBack.Location = new Point(100, 50);
            mBtnWhoWorkBack.Text = "Назад";
            mBtnWhoWorkBack.Click += MBtnWhoWorkBack_Click;
            Controls.Add(mBtnWhoWorkBack);
            mBtnWhoWorkBack.BringToFront();

            ChooseUserMenu();
        }

        private void ChooseUserMenu()
        {
            btnUserArr = new MenuButton[userName.Count];

            int startLocation = this.Width / userName.Count / 2;
            int defLoc = startLocation / 2;
            for (int i = 0; i < userName.Count; i++)
            {
                MenuButton btnUser = new MenuButton();
                btnUser.Location = new Point(startLocation - btnUser.Width / 2, this.Height / 2 - btnUser.Height / 2);
                startLocation += defLoc * 4;
                btnUser.ID = userName[i].ID;
                btnUser.Text = userName[i].Login;
                btnUser.Name = userName[i].Login;
                btnUser.Click += BtnUser_Click;
                btnUserArr[i] = btnUser;
                Controls.Add(btnUser);
            }
        }

        private void MBtnWhoWorkBack_Click(object sender, EventArgs e)
        {
            if(btnMonthsArr == null)
            {
                for(int i = 0; i < btnUserArr.Length; i++)
                {
                    Controls.Remove(btnUserArr[i]);
                    btnUserArr[i] = null;
                }

                btnUserArr = null;
                Controls.Remove(mBtnWhoWorkBack);
                mBtnWhoWorkBack = null;

                MainMenuVisible(true);
            }

            else if(userDateWork == null)
            {
                for (int i = 0; i < btnMonthsArr.Length; i++)
                {
                    Controls.Remove(btnMonthsArr[i]);
                    btnMonthsArr[i] = null;

                    Controls.Remove(lbUsrName);
                    lbUsrName = null;

                    Controls.Remove(mBtnMark);
                    mBtnMark = null;
                }
                btnMonthsArr = null;

                for (int i = 0; i < btnUserArr.Length; i++)
                    btnUserArr[i].Visible = true;
            }

            else
            {
                //Controls.Remove(lbUsrName);
                lbUsrName.Location = new Point(this.Width / 2 - lbUsrName.Width / 2, 750);

                //mBtnMark.Visible = true;

                Controls.Remove(userDateWork);
                userDateWork = null;

                for (int i = 0; i < btnMonthsArr.Length; i++)
                    btnMonthsArr[i].Visible = true;
            }
        }

        private void BtnMonth_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < btnMonthsArr.Length; i++)
                btnMonthsArr[i].Visible = false;

            var sen = sender as MenuButton;

            //mBtnMark.Visible = false;
            lbUsrName.Location = new Point(mBtnWhoWorkBack.Location.X, 750);

            int month = 0;
            for (int i = 0; i < Months.Length; i++) if(sen.Text == Months[i]) { month = i + 1; break; }
            List<DateTime> WorkDate = dataBase.GetUserWorkDate(userSelectId);
            List<DateTime> selectDate = new List<DateTime>();
            for (int i = 0; i < WorkDate.Count; i++)
            {
                if(WorkDate[i].Month == month)
                {
                    selectDate.Add(WorkDate[i]);
                }
            }

            userDateWork = new MoraMonthCalendar();
            userDateWork.Location = new Point(this.Width / 2 - userDateWork.Width / 2, 200);
            userDateWork.SelectDate = selectDate;
            userDateWork.Month = month;
            userDateWork.Year = DateTime.Now.Year;
            Controls.Add(userDateWork);
        }

        private void BtnUser_Click(object sender, EventArgs e)
        {
            var sen = sender as MenuButton;
            userSelectId = sen.ID;

            for(int i = 0; i < btnUserArr.Length; i++)
                btnUserArr[i].Visible = false;

            btnMonthsArr = new MenuButton[DateTime.Now.Month];

            mBtnMark = new MenuButton();
            mBtnMark.Location = new Point(this.Width - mBtnMark.Width - 100, 50);
            mBtnMark.Text = "Отметить";
            mBtnMark.ID = sen.ID;
            mBtnMark.Click += MBtnMark_Click;
            Controls.Add(mBtnMark);
            mBtnMark.BringToFront();

            lbUsrName = new Label();
            lbUsrName.Font = new Font("TimesNewRoman", 20F);
            lbUsrName.ForeColor = Color.White;
            lbUsrName.AutoSize = true;
            lbUsrName.Text = sen.Text;
            lbUsrName.Location = new Point(this.Width / 2 - lbUsrName.Width / 2, 750);
            lbUsrName.BringToFront();
            Controls.Add(lbUsrName);

            int startLocationX = this.Width / 2 - 155;
            int startLocationY = this.Height / 2 - 330;
            int defLoc = 155;
            for(int i = 0; i < DateTime.Now.Month; i++)
            {
                MenuButton btnMonth = new MenuButton();
                btnMonth.Location = new Point(startLocationX - btnMonth.Width / 2, startLocationY - btnMonth.Height / 2);
                startLocationX += defLoc;
                btnMonth.Text = Months[i];
                btnMonth.Name = Months[i];
                btnMonth.Click += BtnMonth_Click;
                btnMonthsArr[i] = btnMonth;
                Controls.Add(btnMonth);

                if((i + 1) % 3 == 0)
                {
                    startLocationY += 110;
                    startLocationX = this.Width / 2 - 155;
                }
            }
        }

        private void MBtnMark_Click(object sender, EventArgs e)
        {

            if (dataBase.MarkCheck(mBtnMark.ID))
            {
                dataBase.WorkMark(mBtnMark.ID);

                mBtnMark.Text = "Отмечено";
            }
            else
                mBtnMark.Text = "Уже отмечено";
        }

        private void MBtnArchive_Click(object sender, EventArgs e)
        {
            MainMenuVisible(false);

            mBtnArchiveBack = new MenuButton();
            mBtnArchiveBack.Location = new Point(100, 50);
            mBtnArchiveBack.Text = "Назад";
            mBtnArchiveBack.Name = "mBtnArchiveBack";
            mBtnArchiveBack.Click += MBtnArchiveBack_Click;

            Controls.Add(mBtnArchiveBack);

            mBtnArchiveBack.BringToFront();
            pCardOrderBoxArchive.Visible = true;
        }

        private void MBtnArchiveBack_Click(object sender, EventArgs e)
        {
            if (pCardOrderBoxArchive.Visible)
            {
                mBtnArchiveBack.Visible = false;
                pCardOrderBoxArchive.Visible = false;
                MainMenuVisible(true);
                
            }
            else
            {
                AboutMenuRemove(tbDict, pBox, null, mBtnSave);
                pCardOrderBoxArchive.Visible = true;
            }
        }

        //Анимация движения кнопок
        private void MoveBtnDown()
        {
            if (Controls["mBtnAdd"].Location.Y <= 10)
            {
                Controls["mBtnAdd"].Location = new Point(Controls["mBtnAdd"].Location.X, Controls["mBtnAdd"].Location.Y + 5);
                Controls["mBtnArchive"].Location = new Point(Controls["mBtnArchive"].Location.X, Controls["mBtnArchive"].Location.Y + 5);
                Controls["mBtnVisisble"].Location = new Point(Controls["mBtnVisisble"].Location.X, Controls["mBtnVisisble"].Location.Y + 5);
                if (UserAdmin)
                {
                    mBtnWhoWork.Location = new Point(mBtnWhoWork.Location.X, mBtnWhoWork.Location.Y + 5);
                    mBtnMoneyEarned.Location = new Point(mBtnMoneyEarned.Location.X, mBtnMoneyEarned.Location.Y + 5);
                }
                
                return;
            }
            timer1.Enabled = false;
            MoveDown = false;
            MoveUp = true;
            Controls["mBtnVisisble"].Enabled = true;
            Controls["mBtnVisisble"].Text = "ᐱ";
            Invalidate();
        }

        private void MoveBtnUp()
        {
            if (Controls["mBtnAdd"].Location.Y > -100)
            {
                Controls["mBtnAdd"].Location = new Point(Controls["mBtnAdd"].Location.X, Controls["mBtnAdd"].Location.Y - 5);
                Controls["mBtnArchive"].Location = new Point(Controls["mBtnArchive"].Location.X, Controls["mBtnArchive"].Location.Y - 5);
                Controls["mBtnVisisble"].Location = new Point(Controls["mBtnVisisble"].Location.X, Controls["mBtnVisisble"].Location.Y - 5);
                if (UserAdmin)
                {
                    mBtnWhoWork.Location = new Point(mBtnWhoWork.Location.X, mBtnWhoWork.Location.Y - 5);
                    mBtnMoneyEarned.Location = new Point(mBtnMoneyEarned.Location.X, mBtnMoneyEarned.Location.Y - 5);
                }
                
                return;
            }
            MoveUp = false;
            MoveDown = true;
            timer1.Enabled = false;
            Controls["mBtnVisisble"].Enabled = true;
            Controls["mBtnVisisble"].Text = "ᐯ";
        }

        //Создание и распределение в основной и архивный блок карточек с заказами
        private void UpdateOrderCards(List<AboutOrder> list)
        {
            pCardOrderBox.Controls.Clear();
            pCardOrderBoxArchive.Controls.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                CardOrderInfo cardOrder1 = new CardOrderInfo();
                cardOrder1.ID = list[i].ID;
                cardOrder1.Phone = list[i].Phone;
                cardOrder1.Price = list[i].Price;
                cardOrder1.Prepayment = list[i].Prepayment;
                cardOrder1.ShortDescription = list[i].ShortDescription;
                cardOrder1.LongDescription = list[i].LongDescription;
                cardOrder1.DateOfOrderFinished = list[i].DateOfOrderFinished;
                cardOrder1.LayoutRaedy = list[i].LayoutRaedy;
                cardOrder1.DateOfOrderGet = list[i].DateOfOrderGet;
                
                cardOrder1.Ready = list[i].Ready;
                cardOrder1.Refusal = list[i].Refusal;
                cardOrder1.Issued = list[i].Issued;
                cardOrder1.ToTheWorkShop = list[i].ToTheWorkShop;
                cardOrder1.Picture = list[i].Picture;
                
                cardOrder1.Name = "cardOrder" + list[i].ID;

                if (list[i].Issued || list[i].Refusal)
                {
                    cardOrder1.BackColor = Color.Gray;
                    cardOrder1.Click += new EventHandler(CardOrder1Archive_Click);
                    pCardOrderBoxArchive.Controls.Add(cardOrder1);
                }
                else
                {
                    if (cardOrder1.DateOfOrderFinished.Subtract(TimeSpan.FromHours(24)) <= DateTime.Now)
                    {
                        cardOrder1.BackColor = Color.FromArgb(132, 0, 50);
                    }
                    else if (cardOrder1.DateOfOrderFinished.Subtract(TimeSpan.FromHours(48)) <= DateTime.Now)
                        cardOrder1.BackColor = Color.FromArgb(229, 149, 0);

                    cardOrder1.Click += new EventHandler(CardOrder1_Click);
                    pCardOrderBox.Controls.Add(cardOrder1);
                }
            }
        }

        private void CardOrder1Archive_Click(object sender, EventArgs e)
        {
            pCardOrderBoxArchive.Visible = false;

            CreateAboutMenu(sender);
            Controls.Remove(mBtnBack);
        }

        //Создание панелей и надписей с названиями таблиц
        private void DrawOrderCards(List<AboutOrder> list)
        {
            pCardOrderBox = new TableLayoutPanel();
            pCardOrderBox.Location = new Point(this.Width * 1 / 18 - 5, 200);
            pCardOrderBox.Size = new Size(Form1.FormWidth * 8 / 9 + 125, Height - 240);

            pCardOrderBoxArchive = new TableLayoutPanel();
            pCardOrderBoxArchive.Location = new Point(this.Width * 1 / 18 - 5, 200);
            pCardOrderBoxArchive.Size = new Size(Form1.FormWidth * 8 / 9 + 125, Height - 240);
            pCardOrderBoxArchive.AutoScroll = true;
            pCardOrderBoxArchive.HorizontalScroll.Visible = false;
            pCardOrderBoxArchive.BackColor = this.BackColor;
            pCardOrderBoxArchive.Visible = false;
            Controls.Add(pCardOrderBoxArchive);

            pCardOrderBox.AutoScroll = true;

            int varticalScroll = SystemInformation.VerticalScrollBarWidth;
            pCardOrderBox.Padding = new Padding(0, 0, varticalScroll, 0);
            pCardOrderBox.BackColor = this.BackColor;
            Controls.Add(pCardOrderBox);

            int[] recLoc = new int[] { 30, 230, 340, 450, 670, 830, 990, 1150, 1310, 1470, 1630 };
            int[] recSize = new int[] { 190,
                                       100,
                                       100,
                                       210,
                                       150,
                                       150,
                                       150,
                                       150,
                                       150,
                                       150,
                                       60 };

            UpdateOrderCards(list);

            for (int i = 0; i < labelName.Length - 1; i++)
            {
                Label label = new Label();
                label.AutoSize = false;
                label.Visible = true;
                label.Size = new Size(recSize[i], 30);
                label.Font = new Font("TimesNewRoman", 10F);
                label.Text = labelName[i];
                label.ForeColor = Color.White;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.BackColor = Color.Transparent;
                label.Name = "label" + i;
                label.Location = new Point(pCardOrderBox.Location.X + recLoc[i], pCardOrderBox.Location.Y - label.Height);

                Controls.Add(label);
            }
        }

        //Показ или скрытие основного меню
        private void MainMenuVisible(bool visible)
        {
            mBtnAdd.Visible = visible;
            mBtnArchive.Visible = visible;
            mBtnVisisble.Visible = visible;
            if (UserAdmin)
            {
                mBtnWhoWork.Visible = visible;
                mBtnMoneyEarned.Visible = visible;
            }
            
            pCardOrderBox.Visible = visible;

            for (int i = 0; i < labelName.Length - 1; i++)
                Controls["label" + i].Visible = visible;
        }

        //Удаление контролов Всплывающего меню
        private void AboutMenuRemove(Dictionary<string, TextBoxOrder> dict, PictureBox pBox, MenuButton btn1, MenuButton btn2)
        {
            chooseOrder = null;

            if(btn1 != null)
                Controls.Remove(btn1);
            if(btn2 != null)
                Controls.Remove(btn2);
            pBox.Image = null;
            Controls.Remove(pBox);

            if(PickDateOrderGet != null)
            {
                Controls.Remove(PickDateOrderGet);
                Controls.Remove(PickDateOrderFinished);
                Controls.Remove(PickDateLayoutReady);
                PickDateOrderGet = null;
                PickDateOrderFinished = null;
                PickDateLayoutReady = null;
            }

            Controls.Remove(lbPrep);
            lbPrep = null;

            Controls.Remove(tRefusal);
            Controls.Remove(tReady);
            Controls.Remove(tIssued);
            Controls.Remove(tWShop);

            tRefusal = null;
            tReady = null;
            tIssued = null;
            tWShop = null;

            for (int i = 1; i < AboutOrderName.Length; i++)
            {
                Controls.Remove(dict[AboutOrderName[i]]);
                dict[AboutOrderName[i]] = new TextBoxOrder();
            }
        }

        //Создние всплывающего меню
        private void CreateAboutMenu(object sender)
        {
            Controls.Add(mBtnBack);
            Controls.Add(mBtnSave);
            mBtnBack.BringToFront();
            mBtnSave.BringToFront();

            var SelectedOrder = sender as CardOrderInfo;

            chooseOrder = new AboutOrder()
            {
                ID = SelectedOrder.ID,
                Phone = SelectedOrder.Phone,
                Price = SelectedOrder.Price,
                Prepayment = SelectedOrder.Prepayment,
                ShortDescription = SelectedOrder.ShortDescription,
                LongDescription = SelectedOrder.LongDescription,
                DateOfOrderGet = SelectedOrder.DateOfOrderGet,
                DateOfOrderFinished = SelectedOrder.DateOfOrderFinished,
                LayoutRaedy = SelectedOrder.LayoutRaedy,
                Refusal = SelectedOrder.Refusal,
                Ready = SelectedOrder.Ready,
                Issued = SelectedOrder.Issued,
                ToTheWorkShop = SelectedOrder.ToTheWorkShop,
            };

            PropertyInfo[] info = chooseOrder.GetType().GetProperties();
            object obj = new object();

            CreateMenu(tbDict, AboutOrderName.Length, info, chooseOrder, SelectedOrder.Picture, "tbAbout");
        }

        private void MBtnAddOrderBack_Click(object sender, EventArgs e)
        {
            AboutMenuRemove(tbDict, pBox, mBtnAddOrder, mBtnAddOrderBack);
            MainMenuVisible(true);
            pCardOrderBox.VerticalScroll.Value = ScrollChange;
            g.Clear(BackColor);
            Invalidate();
        }

        //Создание текстбоксов для всплывающего меню
        private void CreateMenu(Dictionary<string, TextBoxOrder> tbOrder,int length, PropertyInfo[] info, AboutOrder Order, Image picture, string name)
        {
            int startLocation = 170;
            for (int i = 1; i < length - 7; i++)
            {
                
                tbOrder[AboutOrderName[i]].Location = new Point(this.Width / 2, startLocation);
                tbOrder[AboutOrderName[i]].ForeColor = Color.White;
                if (AboutOrderName[i] == "Подробное описание")
                    tbOrder[AboutOrderName[i]].Size = new Size(tbOrder[AboutOrderName[i]].Width, tbOrder[AboutOrderName[i]].Height + 50);
                tbOrder[AboutOrderName[i]].TextPreview = AboutOrderName[i];
                tbOrder[AboutOrderName[i]].Font = new Font("TimesNewRoman", 15F);
                object Value = new object();
                if (Order != null)
                {
                    Value = info[i].GetValue(Order);
                    tbOrder[AboutOrderName[i]].Text = Value.ToString(); // ПЕРЕДАЧА ДАТЫ ИЗ ПИКЕРА
                }
                    
                else
                    tbOrder[AboutOrderName[i]].Text = "";

                tbOrder[AboutOrderName[i]].Name = name + i;
                Controls.Add(tbOrder[AboutOrderName[i]]);
                g.DrawLine(new Pen(Color.FromArgb(0, 100, 102)), Width / 2, startLocation + tbOrder[AboutOrderName[i]].Height + 15, Width - 50, startLocation + tbOrder[AboutOrderName[i]].Height + 15);

                startLocation += tbOrder[AboutOrderName[i]].Height + 30;
            }

            tbDict["Оплачено"].TextChanged += Form1_TextChanged;
            tbDict["Цена"].TextChanged += Form1_TextChanged;

            
            lbPrep = new Label();
            lbPrep.Font = new Font("TimesNewRoman", 15F);
            lbPrep.ForeColor = Color.White;
            lbPrep.Location = new Point(tbDict["Оплачено"].Location.X + tbDict["Оплачено"].Width + 10, tbDict["Оплачено"].Location.Y);
            if (chooseOrder != null)
            {
                int dev = chooseOrder.Price - chooseOrder.Prepayment;
                lbPrep.Text = dev.ToString();
            }
            else
                lbPrep.Text = "";

            Controls.Add(lbPrep);

            
            CreateDatePicker(ref PickDateOrderGet, Width / 2, startLocation, chooseOrder != null ? chooseOrder.DateOfOrderGet : DateTime.Now, "Дата получения");
            startLocation += PickDateOrderGet.Height + 30;

            CreateDatePicker(ref PickDateLayoutReady, Width / 2, startLocation, chooseOrder != null ? chooseOrder.LayoutRaedy : DateTime.Now, "Готовность Макета");
            startLocation += PickDateLayoutReady.Height + 30;

            CreateDatePicker(ref PickDateOrderFinished, Width / 2, startLocation, chooseOrder != null ? chooseOrder.DateOfOrderFinished : DateTime.Now, "Дата завершения");
            startLocation += PickDateOrderFinished.Height + 30;

            CreateToggle(ref tRefusal, ref startLocation, AboutOrderName[AboutOrderName.Length - 4], info != null ? bool.Parse(info[info.Length - 5].GetValue(Order).ToString()) : false);
            CreateToggle(ref tReady, ref startLocation, AboutOrderName[AboutOrderName.Length - 3], info != null ? bool.Parse(info[info.Length - 4].GetValue(Order).ToString()) : false);
            CreateToggle(ref tIssued, ref startLocation, AboutOrderName[AboutOrderName.Length - 2], info != null ? bool.Parse(info[info.Length - 3].GetValue(Order).ToString()) : false);
            CreateToggle(ref tWShop, ref startLocation, AboutOrderName[AboutOrderName.Length - 1], info != null ? bool.Parse(info[info.Length - 2].GetValue(Order).ToString()) : false);

            pBox.Size = new Size(this.Width / 2 - 100, startLocation - 180);
            pBox.Image = picture;
            pBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pBox.DoubleClick += PBox_DoubleClick;
            Controls.Add(pBox);

            void CreateToggle(ref Toggle toggle, ref int startLoc, string Name, bool value)
            {
                //g.DrawString(Name, new Font("TimesNewRoman", 10F), new SolidBrush(Color.FromArgb(0, 100, 102)), this.Width / 2, startLoc - 5, SF);
                toggle = new Toggle();
                toggle.Location = new Point(this.Width / 2, startLoc);
                toggle.ForeColor = Color.FromArgb(0, 100, 102);
                toggle.Text = Name;
                toggle.Checked = value;
                Controls.Add(toggle);
                g.DrawLine(new Pen(Color.FromArgb(0, 100, 102)), Width / 2, startLoc + toggle.Height + 15, Width - 50, startLoc + toggle.Height + 15);
                startLoc += toggle.Height + 30;
            }
        }

        private void Form1_TextChanged(object sender, EventArgs e)
        {
            if(tbDict["Оплачено"].Text != "")
            {
                int prep = int.Parse(tbDict["Оплачено"].Text);
                int price = int.Parse(tbDict["Цена"].Text);

                int dev = price - prep;

                lbPrep.Text = dev.ToString();
            }
            else
                lbPrep.Text = "";

        }

        private void CreateDatePicker(ref DateTimePicker picker, int x, int y, DateTime date, string name)
        {
            picker = new DateTimePicker();
            picker.Size = new Size(picker.Width, 60);
            picker.Location = new Point(x, y);
            picker.CalendarMonthBackground = Color.FromArgb(0, 100, 102);
            //picker.CalendarTitleForeColor = Color.White;
            picker.Value = date;
            Controls.Add(picker);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            g.DrawString(name, new Font("TimesNewRoman", 15F), new SolidBrush(Color.White), x + picker.Width + 10, y, sf);
            g.DrawLine(new Pen(Color.FromArgb(0, 100, 102)), x, y + picker.Height + 15, Width, y + picker.Height + 15);
        }

        private void PBox_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            if (fileDialog.ShowDialog() == DialogResult.OK)
                pBox.Image = Image.FromFile(fileDialog.FileName);
        }

        private void MBtnAdd_Click(object sender, EventArgs e)
        {
            MainMenuVisible(false);

            Controls.Add(mBtnAddOrder);
            Controls.Add(mBtnAddOrderBack);

            mBtnAddOrder.BringToFront();
            mBtnAddOrderBack.BringToFront();

            CreateMenu(tbDict, AboutOrderName.Length, null, null, null, "tbAdd");

            ScrollChange = pCardOrderBox.VerticalScroll.Value;
        }

        private void MBtnVisisble_Click(object sender, EventArgs e)
        {
            ScrollChange = pCardOrderBox.VerticalScroll.Value;
            timer1.Enabled = true;
            Controls["mBtnVisisble"].Enabled = false;
        }

        private void MBtnSave_Click(object sender, EventArgs e)
        {
            GetNewOrderValues(ref chooseOrder, tbDict);

            dataBase.UpdateOrder(chooseOrder);

            chooseOrder = null;
            AboutMenuRemove(tbDict, pBox, mBtnBack, mBtnSave);
            UpdateOrderCards(dataBase.GetAllOrders());
            MainMenuVisible(true);

            if (mBtnArchiveBack != null && mBtnArchiveBack.Visible)
                mBtnArchiveBack.Visible = false;
        }

        private void MBtnBack_Click(object sender, EventArgs e)
        {
            AboutMenuRemove(tbDict, pBox, mBtnBack, mBtnSave);
            MainMenuVisible(true);
            pCardOrderBox.VerticalScroll.Value = ScrollChange;
            g.Clear(BackColor);
            Invalidate();
        }

        private void MBtnAddOrder_Click(object sender, EventArgs e)
        {
            AboutOrder AddOrder = new AboutOrder();
            GetNewOrderValues(ref AddOrder, tbDict);

            dataBase.AddOrder(AddOrder);
            AboutMenuRemove(tbDict, pBox, mBtnAddOrder, mBtnAddOrderBack);

            UpdateOrderCards(dataBase.GetAllOrders());
            MainMenuVisible(true);
        }

        private void CardOrder1_Click(object sender, EventArgs e)
        {
            MainMenuVisible(false);
            pCardOrderBoxArchive.Visible = false;
            CreateAboutMenu(sender);
            ScrollChange = pCardOrderBox.VerticalScroll.Value;

            this.ActiveControl = pBox;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MoveUp)
            {
                pCardOrderBox.VerticalScroll.Value = ScrollChange;
                MoveBtnUp();
            }
                
            if (MoveDown)
            {
                pCardOrderBox.VerticalScroll.Value = ScrollChange;
                MoveBtnDown();
            }
        }
    }
}

