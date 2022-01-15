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

        public Command COM_ShowHome { get; set; }
        public Command COM_ShowOrders { get; set; }
        public Command COM_ShowShowcase { get; set; }

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
            COM_ShowHome = new Command(s =>
            {
                SetCurrentView();
            });

            COM_ShowOrders = new Command(s =>
            {
                SetCurrentView(new ShowOrderList(), Model.eEmployeeType.Cashier);
            });

            COM_ShowShowcase = new Command(s =>
            {
                SetCurrentView(new ShowShowcase());
            });
            
            COM_SellFood = new Command(s =>
            {
                AppManager.OpenWindow(new View.SellFood(), new SellFood());
            });

            COM_CreateOrder = new Command(s =>
            {
                AppManager.OpenWindow(new View.CreateOrder(), new CreateOrder());
            });

            COM_SellOrder = new Command(s =>
            {
                AppManager.OpenWindow(new View.ManageOrder(), new ManageOrder());
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
            }
        }
        private void SetCurrentView(BaseViewModel currentView, Model.eEmployeeType neededType)
        {
            if(AppManager.CurrentEmployee.Account.Type == neededType || AppManager.CurrentEmployee.Account.Type == Model.eEmployeeType.Manager)
                CurrentView = currentView;
        }
        private void SetCurrentView(BaseViewModel currentView)
        {
            CurrentView = currentView;
        }
    }
}
