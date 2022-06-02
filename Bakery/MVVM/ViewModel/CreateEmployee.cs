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
    class CreateEmployee : BaseViewModel
    {
        private int _indexType;
        public int IndexType
        {
            get { return _indexType; }
            set
            {
                _indexType = value;
                OnPropertyChanged();
            }
        }

        private string _fio;
        public string FIO
        {
            get { return _fio; }
            set
            {
                _fio = value;
                OnPropertyChanged();
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                string result = "";
                foreach (var ch in value.Take(11))
                    result += ch;

                _phone = result;
                OnPropertyChanged();
            }
        }

        private string _username;
        public string Username
        {
            get { return _username?.Trim(); }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password?.Trim(); }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        
        public Command COM_Create { get; set; }

        public CreateEmployee()
        {
            IndexType = 0;

            COM_Create = new Command(c =>
            {
                if (Username == "" || Password == "" || Phone == "" || FIO == "")
                    return;
                if (Employee.Collection.Exists(s => s.FIO == FIO && s.Account.Phone == Phone))
                {
                    new MessageView("Такой сотрудник уже существует");
                    return;
                }
                if (Account.Collection.Exists(s => s.Username == Username))
                {
                    new MessageView("Имя пользователя занято");
                    return;
                }

                new MessageView($"Сотрудник '{FIO}' успешно создан");

                new Account()
                {
                    Phone = Phone,
                    Username = Username,
                    Password = Password,
                    Type = (eEmployeeType)IndexType,
                }.Add();

                new Employee()
                {
                    FIO = FIO,
                    Account = Account.Collection.Find(s => s.Username == Username),
                }.Add();

                AppManager.CloseActiveWindow(new View.CreateEmployee());
            });
        }
    }
}
