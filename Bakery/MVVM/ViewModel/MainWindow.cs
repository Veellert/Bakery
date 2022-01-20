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
        public Command COM_ShowStorage { get; set; }
        public Command COM_ShowDeliveryRequests { get; set; }
        public Command COM_ShowProviders { get; set; }
        public Command COM_ShowDelivery { get; set; }
        public Command COM_ShowEmloyees { get; set; }

        public Command COM_OpenAccount { get; set; }

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
        
        #region Manager
        
        public Command COM_ManageRequest { get; set; }
        public Command COM_DeliveryProduct { get; set; }
        public Command COM_DeliveryRequestProduct { get; set; }

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
            #region Extra

            COM_OpenAccount = new Command(s =>
            {
                AppManager.OpenWindow(new View.OpenEmployee(), new OpenEmployee(AppManager.CurrentEmployee));
            });

            #endregion

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
            
            COM_ShowStorage = new Command(s =>
            {
                SetCurrentView(new ShowStorage(), Model.eEmployeeType.Baker);
            });

            COM_ShowDeliveryRequests = new Command(s =>
            {
                SetCurrentView(new ShowRequest(), Model.eEmployeeType.Baker);
            });

            COM_ShowProviders = new Command(s =>
            {
                SetCurrentView(new ShowProviders(), Model.eEmployeeType.Manager);
            });
            
            COM_ShowDelivery = new Command(s =>
            {
                SetCurrentView(new ShowDelivery(), Model.eEmployeeType.Manager);
            });
            
            COM_ShowEmloyees = new Command(s =>
            {
                SetCurrentView(new ShowEmployees(), Model.eEmployeeType.Manager);
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
                AppManager.OpenWindow(new View.AddShowcaseOrder(), new AddShowcaseOrder());
            });

            COM_FillShowcaseFood = new Command(s =>
            {
                AppManager.OpenWindow(new View.AddShowcaseFood(), new AddShowcaseFood());
            });

            COM_CreateDeliveryRequest = new Command(s =>
            {
                AppManager.OpenWindow(new View.CreateDeliveryRequest(), new CreateDeliveryRequest());
            });

            #endregion

            #region Manager

            COM_ManageRequest = new Command(s =>
            {
                AppManager.OpenWindow(new View.ManageRequest(), new ManageRequest());
            });

            COM_DeliveryProduct = new Command(s =>
            {
                AppManager.OpenWindow(new View.CreateDeliveryProduct(), new CreateDeliveryProduct());
            });

            COM_DeliveryRequestProduct = new Command(s =>
            {
                AppManager.OpenWindow(new View.CreateDeliveryRequestProduct(), new CreateDeliveryRequestProduct());
            });

            #endregion
        }

        private void SetCurrentView()
        {
            switch (AppManager.CurrentEmployee.Account.Type)
            {
                case Model.eEmployeeType.Manager:
                    CurrentView = new HomeManager();
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
