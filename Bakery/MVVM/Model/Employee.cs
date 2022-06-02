using Bakery.Extra;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bakery.MVVM.Model
{
    public class Employee
    {
        public static List<Employee> Collection { get; set; }

        public Account Account { get; set; }
        public string FIO { get; set; }
        public eEmploueeStatus Status { get; set; }

        public Command COM_OpenInfo => new Command(c =>
        {
            AppManager.OpenWindow(new View.OpenEmployee(), new ViewModel.OpenEmployee(this), false);
        });

        public Command COM_Redact => new Command(c =>
        {
            if(Account.ID != 0)
                AppManager.OpenWindow(new View.EditEmployee(), new ViewModel.EditEmployee(this), false);
            else
                new MessageView("Вы находитесь на системном аккаунте");
        });

        public Command COM_LogOut => new Command(c =>
        {
            if (Account.ID == AppManager.CurrentEmployee.Account.ID)
                AppManager.LogOut();
            else
                new MessageView("Это аккаунт не подключен к текущей сессии");
        });

        #region SQL

        public static void Fill()
        {
            Collection = new List<Employee>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM employees", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        Collection.Add(new Employee()
                        {
                            Account = Account.Collection.Find(s => s.ID == dr.GetInt32("Account")),
                            FIO = dr.GetString("FIO"),
                            Status = eEmploueeStatus.Inactive,
                        });
                db.CloseConnection();
            }

            var active = GetActive();
            foreach (var employee in active)
                if (Collection.Exists(s => s.Account.ID == employee.Account.ID))
                    Collection.Find(s => s.Account.ID == employee.Account.ID).Status = eEmploueeStatus.Active;

            AppManager.UpdateSearchTrigger();
        }

        public static List<Employee> GetActiveEmployees() =>
            Collection.FindAll(s => s.Status == eEmploueeStatus.Active);
        
        private static List<Employee> GetActive()
        {
            var result = new List<Employee>();

            var db = new DB();
            if (db.OpenConnection())
            {
                using (var mc = new MySqlCommand("SELECT * FROM activeemployees", db.connection))
                using (var dr = mc.ExecuteReader())
                    while (dr.Read())
                        result.Add(new Employee()
                        {
                            Account = Account.Collection.Find(s => s.ID == dr.GetInt32("Account")),
                        });
                db.CloseConnection();
            }

            return result;
        }

        public void LogIn()
        {
            if (Account.ID == 0)
                return;

            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "INSERT INTO activeemployees " +
                    $"( Account ) " +
                    $"VALUES " +
                    $"( {Account.ID} );";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }

            Fill();
        }
        
        public void LogOut()
        {
            if (Account.ID == 0)
                return;

            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "DELETE FROM activeemployees " +
                    $"WHERE Account = {Account.ID};";

                using (var mc = new MySqlCommand(sql, db.connection))
                {
                    mc.ExecuteNonQuery();
                    db.CloseConnection();
                }
            }

            Fill();
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

            Fill();
        }

        public void Edit()
        {
            var db = new DB();
            if (db.OpenConnection())
            {
                string sql = "UPDATE employees SET " +
                    $"FIO = @FIO " +
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

            Fill();
        }

        #endregion
    }

    public enum eEmploueeStatus
    {
        Inactive,
        Active,
    }
}
