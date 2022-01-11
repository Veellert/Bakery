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
        public int ID { get; set; }
        public string Name { get; set; }

        #region SQL

        public static List<Provider> Get()
        {
            var result = new List<Provider>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM providers", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Provider()
                        {
                            ID = dr.GetInt32("ID"),
                            Name = dr.GetString("Name"),
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
        }

        #endregion
    }
}
