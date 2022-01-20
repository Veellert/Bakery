using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Bakery.Extra;

namespace Bakery.MVVM.Model
{
    public class Account
    {
        public static List<Account> Collection { get; set; }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public eEmployeeType Type { get; set; }

        public string TypeName
        {
            get
            {
                string result = "";

                switch (Type)
                {
                    case eEmployeeType.Manager:
                        result = "Менеджер";
                        break;
                    case eEmployeeType.Cashier:
                        result = "Кассир";
                        break;
                    case eEmployeeType.Baker:
                        result = "Пекарь";
                        break;
                }

                return result;
            }
        }

        #region SQL

        public static void Fill()
        {
            Collection = new List<Account>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM accounts", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new Account()
                        {
                            ID = dr.GetInt32("ID"),
                            Username = dr.GetString("Username"),
                            Password = dr.GetString("Password"),
                            Phone = dr.GetString("Phone"),
                            Type = (eEmployeeType)dr.GetInt32("Type"),
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
                string sql = "INSERT INTO accounts " +
                    $"( ID, Username, Password, Phone, Type ) " +
                    $"VALUES " +
                    $"( 0, @Username, @Password, @Phone, {(int)Type} );";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("Username", MySqlDbType.String) { Value = Username },
                        new MySqlParameter("Password", MySqlDbType.String) { Value = Password },
                        new MySqlParameter("Phone", MySqlDbType.String) { Value = Phone },
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
                string sql = "UPDATE accounts SET " +
                    $"Username =  @Username, " +
                    $"Password =  @Password, " +
                    $"Phone =  @Phone, " +
                    $"Type =  {(int)Type} " +
                    $"WHERE ID = {ID};";

                MySqlParameter[] parames = new[]
                {
                        new MySqlParameter("Username", MySqlDbType.String) { Value = Username },
                        new MySqlParameter("Password", MySqlDbType.String) { Value = Password },
                        new MySqlParameter("Phone", MySqlDbType.String) { Value = Phone },
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

    public enum eEmployeeType
    {
        Manager,
        Cashier,
        Baker,
    }
}
