using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public class Order
    {
        public int ID { get; set; }
        public eOrderStatus Status { get; set; }
        public List<ShowCaseFood> FoodList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Name => $"Заказ №{ID}";
        public string StartDateText => $"Дата создания: {StartDate}";
        public string EndDateText
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

        public Order()
        {

        }

        public Order(List<ShowCaseFood> orderedFood)
        {
            ID = Get().Count + 1;
            FoodList = orderedFood;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(-1);
            Status = eOrderStatus.InProcess;
        }

        #region SQL

        public static List<Order> Get()
        {
            var result = new List<Order>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM orders", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Order()
                        {
                            ID = dr.GetInt32("ID"),
                            StartDate = dr.GetDateTime("StartDate"),
                            EndDate = dr.GetDateTime("EndDate"),
                            FoodList = Food.GetOrderFood(dr.GetInt32("ID")),
                            Status = (eOrderStatus)dr.GetInt32("Status"),
                        });
                db.CloseConnection();
            }

            return result;
        }

        public static List<Order> GetActiveOrders()
        {
            return Get().FindAll (s => s.Status == eOrderStatus.InProcess || s.Status == eOrderStatus.Ready);
        }

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
            }
        }

        public void DeleteFood()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("DELETE FROM orderfood WHERE OrderID = " + ID, db.connection))
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
