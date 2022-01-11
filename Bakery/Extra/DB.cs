using System.Windows;
using MySql.Data.MySqlClient;

namespace Bakery.Extra
{
    public class DB
    {
        public MySqlConnection connection = null;

        public DB()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Database = "bakery";
            sb.CharacterSet = "UTF8";
            sb.Server = "127.0.0.1";
            sb.UserID = "root";
            sb.Password = "admin";
            connection = new MySqlConnection(sb.ToString());
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
