using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bakery.MVVM.Model
{
    public class Order
    {
        public static List<Order> Collection { get; set; }

        public int ID { get; set; }
        public eOrderStatus Status { get; set; }
        public List<ShowCaseFood> FoodList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Name => $"Заказ №{ID}";
        public string StatusName
        {
            get
            {
                string result = "";

                switch (Status)
                {
                    case eOrderStatus.Done:
                        result = "Выполнен";
                        break;
                    case eOrderStatus.Ready:
                        result = "Ожидает оплаты";
                        break;
                    case eOrderStatus.InProcess:
                        result = "В процессе";
                        break;
                    case eOrderStatus.Cancel:
                        result = "Отменен";
                        break;
                }

                return result;
            }
        }
        public string TTotalPrice => $"{TotalPrice} руб.";
        public string TStartDate => $"Дата создания: {StartDate}";
        public string TEndDate
        {
            get
            {
                string result = "Дата окончания: ";

                if (EndDate < StartDate)
                    result += "нет";
                else
                    result += EndDate;

                return result;
            }
        }
        public float TotalAmount
        {
            get
            {
                float result = 0;

                foreach (var food in FoodList)
                    result += food.Count;

                return result;
            }
        }
        public float TotalPrice
        {
            get
            {
                float result = 0;

                foreach (var food in FoodList)
                    result += food.PreparedFood.Price * food.Count;

                return result;
            }
        }
        public string TFoodList
        {
            get
            {
                string result = "";

                foreach (var food in FoodList)
                    result += food.PreparedFood.Name + " (x" + food.Count + "), ";

                return result.Remove(result.Length - 2, 1);
            }
        }

        public Command COM_Cancel => new Command(c =>
        {
            if (Status == eOrderStatus.Cancel)
            {
                MessageBox.Show("Заказ уже отменен.");
                return;
            }
            if (Status == eOrderStatus.Done)
            {
                MessageBox.Show("Невозможно отменить. Заказ уже выполнен.");
                return;
            }
            
            Status = eOrderStatus.Cancel;
            EndDate = DateTime.Now;

            DataContextExtracter<ViewModel.ManageOrder>.Extract().NewOrderStatusName = StatusName;
            MessageBox.Show("Установлен статус: " + StatusName);
        });
        public Command COM_InProcess => new Command(c =>
        {
            Status = eOrderStatus.InProcess;
            EndDate = StartDate.AddDays(-1);

            DataContextExtracter<ViewModel.ManageOrder>.Extract().NewOrderStatusName = StatusName;
            MessageBox.Show("Установлен статус: " + StatusName);
        });
        public Command COM_Ready => new Command(c =>
        {
            if (Status != eOrderStatus.InProcess)
            {
                MessageBox.Show("Заказ должен находиться в процессе");
                return;
            }

            Status = eOrderStatus.Ready;
            EndDate = StartDate.AddDays(-1);

            DataContextExtracter<ViewModel.ManageOrder>.Extract().NewOrderStatusName = StatusName;
            MessageBox.Show("Установлен статус: " + StatusName);
        });
        public Command COM_Done => new Command(c =>
        {
            if (Status != eOrderStatus.Ready)
            {
                MessageBox.Show("Этот заказ еще не готов к выдаче");
                return;
            }

            bool existInShowcase = true;

            foreach (var food in FoodList)
                existInShowcase &= ShowCase.GetPreparedFood().Exists(s => s.PreparedFood.ID == food.PreparedFood.ID && s.Count >= food.Count);

            if(!existInShowcase)
            {
                MessageBox.Show("Заказ не может быть выполнен, не хватает товаров на витрине.");
                return;
            }

            Status = eOrderStatus.Done;
            EndDate = DateTime.Now;

            DataContextExtracter<ViewModel.ManageOrder>.Extract().NewOrderStatusName = StatusName;
            MessageBox.Show("Установлен статус: " + StatusName);
        });

        public Command COM_Manage => new Command(c =>
        {
            AppManager.OpenWindow(new View.ManageOrder(), new ViewModel.ManageOrder(this));
        });
        public Command COM_OpenInfo => new Command(c =>
        {
            AppManager.OpenWindow(new View.OpenOrder(), new ViewModel.OpenOrder(this));
        });

        public Order()
        {

        }

        public Order(List<ShowCaseFood> orderedFood)
        {
            ID = Collection.Count + 1;
            FoodList = orderedFood;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(-1);
            Status = eOrderStatus.InProcess;
        }

        #region SQL

        public static void Fill()
        {
            Collection = new List<Order>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM orders", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new Order()
                        {
                            ID = dr.GetInt32("ID"),
                            StartDate = dr.GetDateTime("StartDate"),
                            EndDate = dr.GetDateTime("EndDate"),
                            FoodList = Food.GetOrderFood(dr.GetInt32("ID")),
                            Status = (eOrderStatus)dr.GetInt32("Status"),
                        });
                db.CloseConnection();
            }
        }

        public static List<Order> GetActiveOrders() =>
            Collection.FindAll(s => s.Status == eOrderStatus.InProcess ||  s.Status == eOrderStatus.Ready);

        public void Add()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO orders " +
                    $"( ID, Status, StartDate, EndDate ) " +
                    $"VALUES " +
                    $"( 0, {(int)Status}, @StartDate, @EndDate );";

                MySqlParameter[] parames = new[]
                {
                    new MySqlParameter("StartDate", MySqlDbType.DateTime) { Value = StartDate },
                    new MySqlParameter("EndDate", MySqlDbType.DateTime) { Value = EndDate },
                };

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.Parameters.AddRange(parames);
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                foreach (var food in FoodList)
                    food.PreparedFood.AddFoodToOrder(this, food.Count);

                Fill();
            }
        }

        public void Edit()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE orders SET " +
                    $"StartDate = @StartDate, " +
                    $"EndDate = @EndDate, " +
                    $"Status = {(int)Status} " +
                    $"WHERE ID = {ID};";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("StartDate", MySqlDbType.DateTime) { Value = StartDate },
                        new MySqlParameter("EndDate", MySqlDbType.DateTime) { Value = EndDate },
                };

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.Parameters.AddRange(parames);
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                DeleteFood();
                foreach (var food in FoodList)
                    food.PreparedFood.AddFoodToOrder(this, food.Count);

                Fill();
            }
        }

        public void DeleteFood()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("DELETE FROM foodorders WHERE OrderID = " + ID, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }
        }

        #endregion
    }

    public enum eOrderStatus
    {
        Done,
        Ready,
        InProcess,
        Cancel,
    }
}
