using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using Npgsql;
using OrderAccounting.AboutOrderMenu;

namespace TestC.fokus
{
    public class AboutUser
    {
        public int ID { get; set; }
        public string Login { get; set; }
    }

    public class DBconn
    {
        NpgsqlConnection pgData;

        public string ConnectionString { get; private set; }

        public DBconn(string connection)
        {
            ConnectionString = connection;
            pgData = new NpgsqlConnection(ConnectionString);
        }

        public DBconn()
        {
            ConnectionString = "Server=95.217.232.188;Port=7777;Username=habitov;Password=habitov";
            pgData = new NpgsqlConnection(ConnectionString);
        }

        private void Connection()
        {
            pgData = new NpgsqlConnection(ConnectionString);

            pgData.Open();

            if (pgData.FullState == ConnectionState.Broken || pgData.FullState == ConnectionState.Closed)
            {
                Console.WriteLine("crash...");
                Console.Read();
            }

            Console.WriteLine("OK!");
        }

        public void UpdateOrder(AboutOrder Order)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("UPDATE fokus_order_info SET phone_number = '{0}', price = {1}, prepayment = {2}, short_desc = '{3}', long_desc = '{4}', " +
                "date_of_order_acceptance = to_date('{5}', 'DD/MM/YYYY'), date_of_order_finished = to_date('{6}', 'DD/MM/YYYY'), layout_ready = to_date('{7}', 'DD/MM/YYYY'), refusal = {8}, ready = {9}, issued = {10}, to_the_workshop = {11}, image  = '{12}' WHERE id = {13}", Order.Phone, Order.Price, Order.Prepayment, Order.ShortDescription, Order.LongDescription, Order.DateOfOrderGet,
                Order.DateOfOrderFinished, Order.LayoutRaedy, Order.Refusal, Order.Ready, Order.Issued, Order.ToTheWorkShop, ImgToStr(Order.Picture), Order.ID), 
                pgData);

            comm.ExecuteNonQuery();

            Console.WriteLine("OK!");
        }

        #region Добавление значений
        public void AddOrder(AboutOrder Order)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("INSERT INTO fokus_order_info(phone_number, price, prepayment, short_desc, long_desc, " +
                "date_of_order_acceptance, date_of_order_finished, layout_ready, refusal, ready, issued, to_the_workshop, image) " +
                "VALUES ('{0}', {1}, {2}, '{3}', '{4}', to_date('{5}', 'DD/MM/YYYY'), to_date('{6}', 'DD/MM/YYYY'), to_date('{7}', 'DD/MM/YYYY'), {8}, {9}, {10}, {11}, '{12}')", Order.Phone, Order.Price, Order.Prepayment, Order.ShortDescription, Order.LongDescription, Order.DateOfOrderGet.ToShortDateString(),
                Order.DateOfOrderFinished.ToShortDateString(), Order.LayoutRaedy.ToShortDateString(), Order.Refusal, Order.Ready, Order.Issued, Order.ToTheWorkShop, ImgToStr(Order.Picture)), pgData
                );

            comm.ExecuteNonQuery();

            Console.WriteLine("OK!");
        }

        public bool MarkCheck(int id)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand
                (
                String.Format("SELECT work_date FROM fokus_who_work WHERE user_id = {0} AND work_date = to_date('{1}', 'DD/MM/YYYY')", id, DateTime.Now.ToShortDateString()), pgData);
            NpgsqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows) return false;

            return true;
        }

        public void WorkMark(int id)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("INSERT INTO fokus_who_work (user_id, work_date) " +
                              "VALUES ({0}, to_date('{1}', 'DD/MM/YYYY'))", id, DateTime.Now.ToShortDateString()), pgData);

            comm.ExecuteNonQuery();
            Console.WriteLine("OK!");
        }
        #endregion

        #region возвращающие значение

        public List<AboutUser> GetUserName()
        {

            Connection();

            List<AboutUser> userName = new List<AboutUser>();

            NpgsqlCommand comm = new NpgsqlCommand(
                "SELECT id, login FROM fokus_users", pgData
                );

            NpgsqlDataReader reader = comm.ExecuteReader();
            foreach(DbDataRecord record in reader)
            {
                AboutUser user = new AboutUser()
                {
                    ID = int.Parse(record["id"].ToString()),
                    Login = record["login"].ToString()
                };

                userName.Add(user);
            }
                
            return userName;
        }

        public Dictionary<string, string> GetUser(string login, string password)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("SELECT * " +
                "FROM fokus_users " +
                "WHERE '{0}' like login AND '{1}' like password", login, password),
                pgData);

            NpgsqlDataReader reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                Dictionary<string, string> userData;
                foreach (DbDataRecord record in reader)
                {
                    userData = new Dictionary<string, string>
                    {
                        {"id", record["id"].ToString() },
                        {"login", record["login"].ToString() },
                        {"pass", record["password"].ToString() }
                    };

                    return userData;
                }
            }

            return null;
        }

        public List<DateTime> GetUserWorkDate(int id)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("SELECT work_date FROM fokus_who_work WHERE user_id = {0}", id),
                pgData);

            NpgsqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                List<DateTime> dates = new List<DateTime>();
                foreach(DbDataRecord record in reader)
                    dates.Add(DateTime.Parse(record["work_date"].ToString()));
                return dates;
            }

            return new List<DateTime>();
        }

        public List<string> GetUsersByDate(string date)
        {
            Connection();

            string comm = String.Format("SELECT fokus_users.login " +
                "FROM fokus_users " +
                "WHERE fokus_users.id = (SELECT user_id FROM fokus_who_work WHERE to_date('{0}', 'DD/MM/YYYY') = work_date);", date);

            return GetUsersWhoWork(comm);
        }

        public List<string> GetUsersByName(string login)
        {
            Connection();

            string command = String.Format("SELECT fw.work_date " +
                "FROM fokus_who_work fw " +
                "WHERE (SELECT fu.id FROM fokus_users fu WHERE '{0}' like fu.login) = fw.user_id", login);

            return GetUsersWhoWork(command);
        }

        public List<AboutOrder> GetAllOrders()
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("SELECT * FROM fokus_order_info ORDER BY date_of_order_finished"), pgData);

            NpgsqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                List<AboutOrder> orders = new List<AboutOrder>();
                foreach (DbDataRecord record in reader)
                {
                    Image img = StrToImg(record["image"].ToString());

                    var order = new AboutOrder()
                    {
                        ID = (int)record["id"],
                        Phone = record["phone_number"].ToString(),
                        Price = (int)record["price"],
                        Prepayment = (int)record["prepayment"],
                        ShortDescription = record["short_desc"].ToString(),
                        LongDescription = record["long_desc"].ToString(),
                        DateOfOrderGet = DateTime.Parse(record["date_of_order_acceptance"].ToString()),
                        DateOfOrderFinished = DateTime.Parse(record["date_of_order_finished"].ToString()),
                        LayoutRaedy = DateTime.Parse(record["layout_ready"].ToString()),
                        Refusal = (bool)record["refusal"],
                        Ready = (bool)record["ready"],
                        Issued = (bool)record["issued"],
                        ToTheWorkShop = (bool)record["to_the_workshop"],
                        Picture = img
                    };

                    orders.Add(order);
                }
                return orders;
            }
            return null;
        }

        public List<AboutOrder> GetOrdersToWS()
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(
                String.Format("SELECT * FROM fokus_order_info WHERE to_the_workshop = true ORDER BY date_of_order_finished"), pgData);

            NpgsqlDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                List<AboutOrder> orders = new List<AboutOrder>();
                foreach (DbDataRecord record in reader)
                {
                    Image img = StrToImg(record["image"].ToString());

                    var order = new AboutOrder()
                    {
                        ID = (int)record["id"],
                        Phone = record["phone_number"].ToString(),
                        Price = (int)record["price"],
                        Prepayment = (int)record["prepayment"],
                        ShortDescription = record["short_desc"].ToString(),
                        LongDescription = record["long_desc"].ToString(),
                        DateOfOrderGet = DateTime.Parse(record["date_of_order_acceptance"].ToString()),
                        DateOfOrderFinished = DateTime.Parse(record["date_of_order_finished"].ToString()),
                        LayoutRaedy = DateTime.Parse(record["layout_ready"].ToString()),
                        Refusal = (bool)record["refusal"],
                        Ready = (bool)record["ready"],
                        Issued = (bool)record["issued"],
                        ToTheWorkShop = (bool)record["to_the_workshop"],
                        Picture = img
                    };

                    orders.Add(order);
                }
                return orders;
            }
            return null;
        }

        #endregion

        private List<string> GetUsersWhoWork(string command)
        {
            Connection();

            NpgsqlCommand comm = new NpgsqlCommand(command, pgData);
            NpgsqlDataReader reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                List<string> returnList = new List<string>();
                foreach (DbDataRecord record in reader)
                    returnList.Add(record[0].ToString());

                return returnList;
            }

            return null;
        }

        public Image StrToImg(string StrImg)
        {
            if(StrImg != "")
            {
                byte[] arrayimg = Convert.FromBase64String(StrImg);
                Image imageStr = Image.FromStream(new MemoryStream(arrayimg));
                return imageStr;
            }
            return null;
        }

        public string ImgToStr(Image Img)
        {
            if(Img != null)
            {
                MemoryStream Memostr = new MemoryStream();
                Img.Save(Memostr, Img.RawFormat);
                byte[] arrayimg = Memostr.ToArray();
                return Convert.ToBase64String(arrayimg);
            }
            return "";
        }

    }
}