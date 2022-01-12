using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public class Food
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Product> Consistency { get; set; }
        public float Price { get; set; }

        #region SQL

        public static List<Food> Get()
        {
            var result = new List<Food>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM food", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Food()
                        {
                            ID = dr.GetInt32("ID"),
                            Name = dr.GetString("Name"),
                            Price = dr.GetFloat("Price"),
                            Consistency = Product.GetFoodConsistency(dr.GetInt32("ID")),
                        });
                db.CloseConnection();
            }

            return result;
        }

        public static List<ShowCaseFood> GetOrderFood(int orderID)
        {
            var result = new List<ShowCaseFood>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM foodorders WHERE OrderID = " + orderID, db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new ShowCaseFood() { PreparedFood = Get().Find(s => s.ID == dr.GetInt32("FoodID")), Count = dr.GetInt32("Count") });
                db.CloseConnection();
            }

            return result;
        }

        public void Add()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO food " +
                    $"( ID, Price, Name ) " +
                    $"VALUES " +
                    $"( 0, {Price}, @Name );";

                MySqlParameter[] parames = new[]
                {
                    new MySqlParameter("Name", MySqlDbType.String) { Value = Name },
                };

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.Parameters.AddRange(parames);
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                ID = Get().Last().ID;

                foreach (var product in Consistency)
                    product.AddFoodConsistancy(this);

                ShowCase.Add(this);
            }
        }

        public void AddFoodToOrder(Order order, int count)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO foodorders " +
                    $"( OrderID, FoodID, Count ) " +
                    $"VALUES " +
                    $"( {order.ID}, {ID}, {count} );";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }
        }

        public void Edit()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE food SET " +
                    $"Price = {Price} ," +
                    $"Name =  @Name " +
                    $"WHERE ID = {ID};";

                MySqlParameter[] parames = new[]
                {
                    new MySqlParameter("Name", MySqlDbType.String) { Value = Name },
                };

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.Parameters.AddRange(parames);
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Product.DeleteFoodConsistency(this);
                foreach (var product in Consistency)
                    product.AddFoodConsistancy(this);
            }
        }

        #endregion
    }
}
