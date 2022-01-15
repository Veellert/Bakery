using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowOrderList : BaseViewModel
    {

        private string _ordersCount;
        public string OrdersCount
        {
            get { return _ordersCount; }
            set
            {
                _ordersCount = value;
                OnPropertyChanged();
            }
        }

        private List<Order> _orderList;
        public List<Order> OrderList
        {
            get { return _orderList; }
            set
            {
                _orderList = value;
                OrdersCount = "Заказов " + _orderList.Count + " шт.";
                OnPropertyChanged();
            }
        }

        public ShowOrderList()
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
            OrderList = Order.Get().FindAll(s =>
            s.Name.ToLower().Contains(AppManager.GetSearchText()) ||
            s.TStartDate.Contains(AppManager.GetSearchText()) ||
            s.StatusName.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
