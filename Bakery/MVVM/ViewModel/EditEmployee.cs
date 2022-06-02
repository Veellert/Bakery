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
    class EditEmployee : BaseViewModel
    {
        public Employee CurrentEmployee { get; set; }

        private string _oldFIO;

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
                _phone = value;
                OnPropertyChanged();
            }
        }

        public Command COM_Save { get; set; }

        public EditEmployee(Employee employee)
        {
            CurrentEmployee = employee;

            _oldFIO = CurrentEmployee.FIO;
            FIO = CurrentEmployee.FIO;
            Phone = CurrentEmployee.Account.Phone;

            COM_Save = new Command(c =>
            {
                if (FIO == "" || Phone == "")
                    return;

                if (Employee.Collection.Exists(s => s.FIO == FIO && s.Account.Phone == Phone))
                {
                    new MessageView("Такой сотрудник уже есть в базе данных");
                    return;
                }

                if(_oldFIO != FIO)
                    new MessageView($"Сотрудник '{_oldFIO}' изменен на '{FIO}'");
                else
                    new MessageView($"Сотрудник успешно изменен");

                CurrentEmployee.FIO = FIO;
                CurrentEmployee.Edit();

                CurrentEmployee.Account.Phone = Phone;
                CurrentEmployee.Account.Edit();

                AppManager.CloseActiveWindow(new View.EditEmployee());
            });
        }
    }
}
