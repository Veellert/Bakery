using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.Model
{
    public class Employee
    {
        public Account Account { get; set; }
        public string FIO { get; set; }

        #region SQL

        public static List<Employee> Get()
        {
            var result = new List<Employee>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM employees", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Employee()
                        {
                            Account = Account.Get().Find(s => s.ID == dr.GetInt32("Account")),
                            FIO = dr.GetString("FIO"),
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
                string sql = "INSERT INTO employees " +
                    $"( Account, FIO ) " +
                    $"VALUES " +
                    $"( {Account.ID}, @FIO );";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("FIO", MySqlDbType.String) { Value = FIO },
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
                string sql = "UPDATE employees SET " +
                    $"FIO =  @FIO " +
                    $"WHERE Account = {Account.ID};";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("FIO", MySqlDbType.String) { Value = FIO },
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
