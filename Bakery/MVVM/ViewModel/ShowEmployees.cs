using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowEmployees : BaseViewModel
    {
        private string _employeesCount;
        public string EmployeesCount
        {
            get { return _employeesCount; }
            set
            {
                _employeesCount = value;
                OnPropertyChanged();
            }
        }

        private List<Employee> _employeesList;
        public List<Employee> EmployeesList
        {
            get { return _employeesList; }
            set
            {
                _employeesList = value;

                EmployeesCount = "Всего сотрудников: " + EmployeesList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        public ShowEmployees()
        {
            AppManager.OnSearchChanged += OnSearchChanged;

            Load();
        }

        private void OnSearchChanged(object sender, EventArgs e)
        {
            Load();
        }

        private void Load()
        {
            EmployeesList = Employee.Collection.FindAll(s =>
                s.FIO.ToLower().Contains(AppManager.GetSearchText()) ||
                s.Account.Phone.Contains(AppManager.GetSearchText()));
        }
    }
}
