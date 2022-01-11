﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Bakery.Extra;

namespace Bakery.MVVM.Model
{
    public class Account
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public eEmployeeType Type { get; set; }

        #region SQL

        public static List<Account> Get()
        {
            var result = new List<Account>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM accounts", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Account()
                        {
                            ID = dr.GetInt32("ID"),
                            Username = dr.GetString("Username"),
                            Password = dr.GetString("Password"),
                            Phone = dr.GetString("Phone"),
                            Type = (eEmployeeType)dr.GetInt32("Type"),
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