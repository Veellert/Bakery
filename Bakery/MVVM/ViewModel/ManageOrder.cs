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
    class ManageOrder : BaseViewModel
    {
        private List<Order> _orderList;
        public List<Order> OrderList
        {
            get { return _orderList; }
            set
            {
                _orderList = value;
                OnPropertyChanged();
            }
        }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;

                if(_selectedOrder != null)
                {
                    OrderStartDate = _selectedOrder.TStartDate;
                    OrderEndDate = _selectedOrder.TEndDate;
                    OrderStatusName = _selectedOrder.StatusName;

                    PriceText = _selectedOrder.TotalPrice + "₽";
                }
                NewOrderStatusName = "";

                OnPropertyChanged();
            }
        }

        private bool _isActiveOrders;
        public bool IsActiveOrders
        {
            get { return _isActiveOrders; }
            set
            {
                _isActiveOrders = value;

                FillData();

                OnPropertyChanged();
            }
        }
        
        private bool _isNotActiveOrders;
        public bool IsNotActiveOrders
        {
            get { return _isNotActiveOrders; }
            set
            {
                _isNotActiveOrders = value;
                OnPropertyChanged();
            }
        }

        private string _orderSearch;
        public string OrderSearch
        {
            get { return _orderSearch; }
            set
            {
                _orderSearch = value;

                Search();

                OnPropertyChanged();
            }
        }
        
        private string _orderStartDate;
        public string OrderStartDate
        {
            get { return _orderStartDate; }
            set
            {
                _orderStartDate = value;
                OnPropertyChanged();
            }
        }
        
        private string _orderEndDate;
        public string OrderEndDate
        {
            get { return _orderEndDate; }
            set
            {
                _orderEndDate = value;
                OnPropertyChanged();
            }
        }
        
        private string _orderStatusName;
        public string OrderStatusName
        {
            get { return "Статус: " + _orderStatusName; }
            set
            {
                _orderStatusName = value;
                OnPropertyChanged();
            }
        }
        
        private string _newOrderStatusName;
        public string NewOrderStatusName
        {
            get { return "Новый статус: " + _newOrderStatusName; }
            set
            {
                _newOrderStatusName = value;
                OnPropertyChanged();
            }
        }
        
        private string _priceText;
        public string PriceText
        {
            get { return _priceText; }
            set
            {
                _priceText = value;
                OnPropertyChanged();
            }
        }

        public Command COM_CheckCart { get; set; }
        public Command COM_Cancel { get; set; }
        public Command COM_Save { get; set; }

        public ManageOrder()
        {
            InitializeCommand();
            IsActiveOrders = true;
        }
        
        public ManageOrder(Order order)
        {
            InitializeCommand();

            if(order.Status == eOrderStatus.InProcess || order.Status == eOrderStatus.Ready)
                IsActiveOrders = true;
            else
            {
                IsActiveOrders = false;
                IsNotActiveOrders = true;
            }

            SelectedOrder = OrderList.Find(s => s.ID == order.ID);
        }

        private void InitializeCommand()
        {
            COM_CheckCart = new Command(c =>
            {
                if (SelectedOrder == null)
                    return;

                new MessageView(CheckCart());
            });

            COM_Cancel = new Command(c =>
            {
                UpdateCurrent();
            });

            COM_Save = new Command(c =>
            {
                if (SelectedOrder == null)
                    return;


                var order = Order.Collection.Find(s => s.ID == SelectedOrder.ID);
                SelectedOrder.Edit();

                if (SelectedOrder.Status != order.Status && SelectedOrder.Status == eOrderStatus.Done)
                    AppManager.OpenWindow(new View.SellFoodConfirm(), new SellFoodConfirm(SelectedOrder), eEmployeeType.Cashier);

                UpdateCurrent();
            });
        }

        private void FillData()
        {
            if(IsActiveOrders)
                OrderList = Order.GetActiveOrders();
            else
                OrderList = Order.Collection;

            if (OrderList.Count != 0)
                SelectedOrder = OrderList[0];

            OrderSearch = "";
        }

        private void Search()
        {
            var result = new List<Order>();
            if (IsActiveOrders)
                result = Order.GetActiveOrders();
            else
                result = Order.Collection;

            if (OrderSearch != "")
                result = result.FindAll
                (s => s.ID.ToString().Contains(OrderSearch) || s.TStartDate.Contains(OrderSearch));

            OrderList = result;

            if (OrderList.Count != 0)
                SelectedOrder = OrderList[0];
        }

        private void UpdateCurrent()
        {
            int selectedID = 0;
            if (SelectedOrder != null)
                selectedID = SelectedOrder.ID;

            FillData();

            if (OrderList.Count != 0 && OrderList.Exists(s => s.ID == selectedID))
                SelectedOrder = OrderList.Find(s => s.ID == selectedID);
        }

        private string CheckCart()
        {
            string result = "";

            SelectedOrder.FoodList.ForEach(s => result += s.TFullName + "\n");
            result += "\nВсего " + SelectedOrder.TotalPrice + " рублей.";

            return result;
        }
    }
}
