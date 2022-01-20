using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public static class ShowCase
    {
        public static List<ShowCaseFood> Collection { get; set; }

        public static List<ShowCaseFood> GetPreparedFood() => Collection.FindAll(s => s.Count != 0);
        
        public static List<ShowCaseFood> GetEmptyFood() => Collection.FindAll(s => s.Count == 0);

        #region SQL

        public static void Fill()
        {
            Food.Fill();
            Collection = new List<ShowCaseFood>();
            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM showcase", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new ShowCaseFood()
                        {
                            PreparedFood = Food.Collection.Find(s => s.ID == dr.GetInt32("FoodID")),
                            Count = dr.GetInt32("Count"),
                        });
                db.CloseConnection();
            }

            AppManager.UpdateSearchTrigger();
        }

        public static void Add(Food food)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO showcase ";
                if (Collection.FindAll(s => s.PreparedFood.ID == food.ID).Count == 0)
                    sql += $"( FoodID, Count ) " +
                        $"VALUES " +
                        $"( {food.ID}, 0 );";
                else
                    sql = "UPDATE showcase SET " +
                        $"Count = {Collection.Find(s => s.PreparedFood.ID == food.ID).Count + 1} " +
                        $"WHERE FoodID = {food.ID};";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Fill();
            }
        }

        public static void Increment(Food food, int count)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE showcase SET " +
                        $"Count = {Collection.Find(s => s.PreparedFood.ID == food.ID).Count + count} " +
                        $"WHERE FoodID = {food.ID};";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Fill();
            }
        }

        public static void Decrement(Food food, int count)
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE showcase SET " +
                        $"Count = {Collection.Find(s => s.PreparedFood.ID == food.ID).Count - count} " +
                        $"WHERE FoodID = {food.ID};";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }

                Fill();
            }
        }

        #endregion
    }

    public class ShowCaseFood
    {
        public Food PreparedFood { get; set; }
        public int Count { get; set; }

        public string TCount => Count + " (шт.)";
        public string TPrice => PreparedFood.Price + " руб.";
        public string TFullName => "(" + Count + " шт) " + PreparedFood.Name;

        public Command COM_RemoveFromCart => new Command(c =>
        {
            DataContextExtracter<ShoppingCartViewModel>.Extract().RemoveFromCart(this);
        });
        
        public Command COM_CheckConsistency => new Command(c =>
        {
            AppManager.OpenWindow(new View.CheckFoodConsistency(), new ViewModel.CheckFoodConsistency(PreparedFood), false);
        });
    }
}
