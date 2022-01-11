using Bakery.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class MainWindow : BaseViewModel
    {
        #region Commands

        public Command COM_SellFood { get; set; }
        public Command COM_CreateOrder { get; set; }
        public Command COM_SellOrder { get; set; }

        #endregion

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                AppManager.Search = SearchText;

                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeCommand();
            AppManager.Load();

            SetCurrentView();
        }

        private void InitializeCommand()
        {
            COM_SellFood = new Command(s =>
            {
                AppManager.OpenWindow(new View.SellFood(), new SellFood());
            });

            COM_CreateOrder = new Command(s =>
            {
                //////AppManager.OpenActiveWindow(new CreateOrderView(), new CreateOrderViewModel());
            });

            COM_SellOrder = new Command(s =>
            {
                //AppManager.OpenActiveWindow(new SellOrderView(), new SellOrderViewModel());
            });
        }

        private void SetCurrentView()
        {
            switch (AppManager.CurrentEmployee.Account.Type)
            {
                case Model.eEmployeeType.Manager:
                    CurrentView = null;
                    return;
                case Model.eEmployeeType.Cashier:
                    CurrentView = new HomeCashier();
                    return;
                case Model.eEmployeeType.Baker:
                    CurrentView = null;
                    return;
                default:
                    CurrentView = null;
                    return;
            }
        }
        private void SetCurrentView(BaseViewModel currentView, Model.eEmployeeType neededType)
        {
            if(AppManager.CurrentEmployee.Account.Type == neededType)
                CurrentView = currentView;
        }
    }
}
