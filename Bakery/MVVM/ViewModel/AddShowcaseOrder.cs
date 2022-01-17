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
    class AddShowcaseOrder : BaseViewModel
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
                OnPropertyChanged();
            }
        }

        private string _orderSearch;
        public string OrderSearch
        {
            get { return _orderSearch?.Trim().ToLower(); }
            set
            {
                _orderSearch = value;
                Search();
                OnPropertyChanged();
            }
        }

        public Command COM_AddToShowcase { get; set; }

        public AddShowcaseOrder()
        {
            InitializeCommand();

            FillDefault();
        }

        private void InitializeCommand()
        {
            COM_AddToShowcase = new Command(c =>
            {
                if (SelectedOrder == null)
                    return;

                var storageConsistency = new List<Product>();
                foreach (var food in SelectedOrder.FoodList)
                    foreach (var product in food.PreparedFood.Consistency)
                    {
                        var prod = Product.Collection.Find(s => s.ID == product.ID);
                        int totalWeight = product.Weight * food.Count;

                        if (prod.Weight < totalWeight)
                            storageConsistency.Add(prod);
                    }

                if (storageConsistency.Count != 0)
                {
                    string message = "Не хватает: \n";

                    foreach (var product in storageConsistency)
                    {
                        int totalWeight = 0;
                        SelectedOrder.FoodList.ForEach(v => totalWeight += v.PreparedFood.Consistency.Find(s => s.ID == product.ID).Weight * v.Count);

                        int count = Math.Abs(product.Weight - totalWeight);

                        message += product.Name + " - " + count + " гр/мл/шт \n";
                    }

                    MessageBox.Show(message);
                    return;
                }

                foreach (var food in SelectedOrder.FoodList)
                {
                    ShowCase.Increment(food.PreparedFood, food.Count);

                    foreach (var product in food.PreparedFood.Consistency)
                    {
                        var storageProd = Product.Collection.Find(s => s.ID == product.ID);
                        storageProd.Weight -= product.Weight * food.Count;
                        storageProd.Edit();
                    }
                }

                SelectedOrder.Status = eOrderStatus.Ready;
                SelectedOrder.Edit();

                FillDefault();
            });
        }

        private void FillDefault()
        {
            OrderList = GetOrders();

            if (OrderList.Count > 0)
                SelectedOrder = OrderList[0];

            OrderSearch = "";
        }

        private void Search()
        {
            var result = GetOrders();
            if (OrderSearch != "")
                result = GetOrders().FindAll
                (s => s.FullName.ToLower().Contains(OrderSearch.Trim()));

            OrderList = result;
            if (OrderList.Count > 0)
                SelectedOrder = OrderList[0];
        }

        private List<Order> GetOrders() => Order.GetActiveOrders().FindAll(s => s.Status == eOrderStatus.InProcess);
    }
}
