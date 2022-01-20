using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bakery.MVVM.ViewModel
{
    class OpenLogin : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public Command COM_LogIn { get; set; }

        public OpenLogin()
        {
            COM_LogIn = new Command(c =>
            {
                if (Username == "" || Password == "")
                    return;

                if(Username == AppManager.SystemEmployee.Account.Username &&
                    Password == AppManager.SystemEmployee.Account.Password)
                {
                    AppManager.LogIn(AppManager.SystemEmployee.Account);
                    AppManager.CloseAllWindows();
                    return;
                }

                if(!Account.Collection.Exists(s => s.Username == Username))
                {
                    MessageBox.Show("Неправильно введено имя пользователя");
                    return;
                }

                var account = Account.Collection.Find(s => s.Username == Username);
                if (account.Password != Password)
                {
                    MessageBox.Show("Пароль не подходит");
                    return;
                }

                AppManager.LogIn(Account.Collection.Find(s => s.Username == Username));
                AppManager.CloseAllWindows();
            });
        }
    }
}
