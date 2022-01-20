using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bakery.Extra
{
    public static class AppManager
    {
        public static EventHandler OnSearchChanged { get; set; }
        public static EventHandler OnAccountChanged { get; set; }
        private static string _search { get; set; }
        public static string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                UpdateSearchTrigger();
            }
        }

        public static Employee SystemEmployee => new Employee()
        {
            FIO = "система",
            Status = eEmploueeStatus.Active,
            Account = new Account()
            {
                Password = "admin",
                Username = "admin",
                Phone = "0",
                Type = eEmployeeType.Manager,
            }
        };

        public static Employee CurrentEmployee { get; set; }

        public static List<Window> ActiveWindows { get; private set; }

        public static Window GetActiveWindow() => ActiveWindows.Find(s => s.IsActive);

        public static void OpenWindow(Window window, object viewModel, eEmployeeType neededType)
        {
            if (CurrentEmployee.Account.Type != neededType && 
                CurrentEmployee.Account.Type != eEmployeeType.Manager)
            {
                MessageBox.Show("Доступ запрещен");
                return;
            }

            OpenWindow(window, viewModel);
        }
        public static void OpenWindow(Window window, object viewModel, bool onlyManager)
        {
            if (onlyManager)
            {
                if (CurrentEmployee.Account.Type != eEmployeeType.Manager)
                {
                    MessageBox.Show("Доступ запрещен");
                    return;
                }
            }

            OpenWindow(window, viewModel);
        }
        private static void OpenWindow(Window window, object viewModel)
        {
            window.DataContext = viewModel;
            ActiveWindows.Add(window);
            ActiveWindows.Last().ShowDialog();
        }

        public static void CloseActiveWindow(Window window = null)
        {
            if (window == null)
                window = ActiveWindows.Last();

            ActiveWindows.Find(s => s.Title == window.Title).Close();
            ActiveWindows.RemoveAll(s => s.Title == window.Title);
        }
        
        public static void CloseAllWindows()
        {
            foreach (var window in ActiveWindows)
                window.Close();

            ActiveWindows.Clear();
        }

        public static void Load()
        {
            ActiveWindows = new List<Window>();
            Fill();
            LogOut();
        }

        public static void Fill()
        {
            Account.Fill();
            Employee.Fill();
            Product.Fill();
            Provider.Fill();

            Food.Fill();
            ShowCase.Fill();
            Order.Fill();
            DeliveryRequest.Fill();
            Delivery.Fill();
        }

        public static void LogIn(Account account)
        {
            var employee = SystemEmployee;
            if(account.ID != 0)
                employee = Employee.Collection.Find(s => s.Account.ID == account.ID);

            CurrentEmployee = employee;
            CurrentEmployee.LogIn();

            OnAccountChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void LogOut()
        {
            if (CurrentEmployee != null)
                CurrentEmployee.LogOut();
            CurrentEmployee = null;

            OpenWindow(new MVVM.View.OpenLogin(), new MVVM.ViewModel.OpenLogin());
            OnAccountChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void SimpleLogOut()
        {
            CurrentEmployee.LogOut();
            CurrentEmployee = null;
        }

        public static string GetSearchText() => (Search ?? "").ToLower().Trim();

        public static void UpdateSearchTrigger() => OnSearchChanged?.Invoke(null, EventArgs.Empty);
    }
}
