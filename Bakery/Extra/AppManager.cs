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
        private static string _search { get; set; }
        public static string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnSearchChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Employee CurrentEmployee { get; set; }

        public static List<Window> ActiveWindows { get; private set; }

        public static Window GetActiveWindow() => ActiveWindows.Find(s => s.IsActive);

        public static void OpenWindow(Window window, object viewModel)
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

            OnSearchChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void Load()
        {
            //MessageBox.Show("Логин");
            ActiveWindows = new List<Window>();

            CurrentEmployee = LogIn();

            ShowCase.Fill();
        }

        private static Employee LogIn() => new Employee()
        {
            FIO = "Супер работник месяца",
            Account = new Account()
            {
                ID = 1,
                Username = "zmeuga",
                Password = "Sjh23L",
                Phone = "89089979040",
                Type = eEmployeeType.Cashier,
            },
        };

        public static string GetSearchText() => (Search ?? "").ToLower().Trim();
    }
}
