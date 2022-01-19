using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public class Provider
    {
        public static List<Provider> Collection { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }

        #region SQL

        public static void Fill()
        {
            Collection = new List<Provider>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM providers", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new Provider()
                        {
                            ID = dr.GetInt32("ID"),
                            Name = dr.GetString("Name"),
                        });
                db.CloseConnection();
            }

            AppManager.UpdateSearchTrigger();
        }

        public void Add()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO providers " +
                    $"( ID, Name ) " +
                    $"VALUES " +
                    $"( 0, @Name );";

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

        public void Edit()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE providers SET " +
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
            }

            Fill();
        }

        #endregion
    }
}
