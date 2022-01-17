using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public class Product
    {
        public static List<Product> Collection { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }

        public string TConsistencyWeight => "Нужно: " + Weight + " гр/мл/шт";
        public string TStorageWeight => "На сладе: " + Collection.Find(s => s.ID == ID).Weight + " гр/мл/шт";

        public Command COM_RemoveFromConsistency => new Command(c =>
        {
            DataContextExtracter<ViewModel.CheckFoodConsistency>.Extract().RemoveFromConsistency(this);
        });
        
        public Command COM_RedactConsistency => new Command(c =>
        {
            DataContextExtracter<ViewModel.CheckFoodConsistency>.Extract().RedactConsistency(this);
        });

        #region SQL

        public static void Fill()
        {
            Collection = new List<Product>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM products", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new Product()
                        {
                            ID = dr.GetInt32("ID"),
                            Name = dr.GetString("Name"),
                            Weight = dr.GetInt32("Weight"),
                        });
                db.CloseConnection();
            }

            AppManager.UpdateSearchTrigger();
        }

        public static List<Product> GetProductsForDelivery(int deliveryID)
        {
            var result = new List<Product>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM productdeliveries WHERE DeliveryID = " + deliveryID, db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Product()
                        {
                            ID = dr.GetInt32("ProductID"),
                            Name = Collection.Find(s => s.ID == dr.GetInt32("ProductID")).Name,
                            Weight = dr.GetInt32("Weight"),
                        });
                db.CloseConnection();
            }

            return result;
        }

        public static List<Product> GetFoodConsistency(int foodID)
        {
            var result = new List<Product>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM consistencies WHERE FoodID = " + foodID, db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Product()
                        {
                            ID = dr.GetInt32("ProductID"),
                            Name = Collection.Find(s => s.ID == dr.GetInt32("ProductID")).Name,
                            Weight = dr.GetInt32("Weight"),
                        });
                db.CloseConnection();
            }

            return result;
        }

        public static void DeleteFoodConsistency(Food food, Product product = null)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                var sql = "DELETE FROM consistencies WHERE FoodID = " + food.ID;
                if (product != null)
                    sql += " AND ProductID = " + product.ID;

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }
        }

        public void AddFoodConsistancy(Food food)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO consistencies " +
                    $"( FoodID, ProductID, Weight ) " +
                    $"VALUES " +
                    $"( {food.ID}, {ID}, {Weight} );";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }
        }

        public static void DeleteProductsFromDelivery(Delivery delivery)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("DELETE FROM productdeliveries WHERE DeliveryID = " + delivery.ID, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }
        }

        public void Add()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO products " +
                    $"( ID, Name, Weight ) " +
                    $"VALUES " +
                    $"( 0, @Name, {Weight} );";

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
            }

            Fill();
        }

        public void AddProductToDelivery(Delivery delivery)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO productdeliveries " +
                    $"( ProductID, DeliveryID, Weight ) " +
                    $"VALUES " +
                    $"( {ID}, {delivery.ID}, {Weight} );";

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
                string sql = "UPDATE products SET " +
                    $"Name =  @Name, " +
                    $"Weight =  {Weight} " +
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
            }

            Fill();
        }

        public void EditFoodConsistancy(Food food)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE consistencies SET " +
                    $"Weight =  {Weight} " +
                    $"WHERE FoodID = {food.ID} " +
                    $"AND ProductID = {ID};";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }
        }

        #endregion
    }
}
