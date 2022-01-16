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

        #region Cashier
        
        public Command COM_SellFood { get; set; }
        public Command COM_CreateOrder { get; set; }
        public Command COM_SellOrder { get; set; }

        #endregion
        
        #region Baker
        
        public Command COM_FillShowcaseOrder { get; set; }
        public Command COM_FillShowcaseFood { get; set; }
        public Command COM_CreateDeliveryRequest { get; set; }

        #endregion

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
            #region Show

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

            #endregion

            #region Cashier

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

            #endregion

            #region Baker

            COM_FillShowcaseOrder = new Command(s =>
            {
                //AppManager.OpenWindow(new View.SellFood(), new SellFood());
            });

            COM_FillShowcaseFood = new Command(s =>
            {
                //AppManager.OpenWindow(new View.CreateOrder(), new CreateOrder());
            });

            COM_CreateDeliveryRequest = new Command(s =>
            {
                //AppManager.OpenWindow(new View.ManageOrder(), new ManageOrder());
            });

            #endregion
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
                    CurrentView = new HomeBaker();
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
