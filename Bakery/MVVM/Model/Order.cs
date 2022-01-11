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
        public List<Food> Food { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public float TotalAmaunt
        {
            get
            {
                float result = 0;

                foreach (var food in Food)
                    result += food.Price;
                return result;
            }
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
                            Status = (eOrderStatus)dr.GetInt32("Status"),
                            StartDate = dr.GetDateTime("StartDate"),
                            EndDate = dr.GetDateTime("EndDate"),
                            Food = Model.Food.GetOrderFood(dr.GetInt32("ID")),
                        });
                db.CloseConnection();
            }

            return result;
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

                ID = Get().Last().ID;

                foreach (var food in Food)
                    food.AddFoodToOrder(this);
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

                Model.Food.DeleteOrderFood(this);
                foreach (var food in Food)
                    food.AddFoodToOrder(this);
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
