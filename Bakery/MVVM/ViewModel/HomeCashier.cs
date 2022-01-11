using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class HomeCashier : BaseViewModel
    {
        #region PreparedFood

        private string _preparedFoodCount;
        public string PreparedFoodCount
        {
            get { return _preparedFoodCount; }
            set
            {
                _preparedFoodCount = value;
                OnPropertyChanged();
            }
        }

        private List<ShowCaseFood> _preparedFood;
        public List<ShowCaseFood> PreparedFood
        {
            get { return _preparedFood; }
            set
            {
                _preparedFood = value;

                int count = 0;
                _preparedFood.ForEach(s => count += s.Count);

                PreparedFoodCount = "На витрине: " + count + " (шт.)";

                OnPropertyChanged();
            }
        }

        #endregion

        //#region ReadyOrders

        //private string _readyOrdersCount;
        //public string ReadyOrdersCount
        //{
        //    get { return _readyOrdersCount; }
        //    set
        //    {
        //        _readyOrdersCount = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private List<Order> _readyOrders;
        //public List<Order> ReadyOrders
        //{
        //    get { return _readyOrders; }
        //    set
        //    {
        //        _readyOrders = value;
        //        ReadyOrdersCount = "Готовых заказов " + _readyOrders.Count + " шт.";
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region InProcessOrders

        //private string _inProcessOrdersCount;
        //public string InProcessOrdersCount
        //{
        //    get { return _inProcessOrdersCount; }
        //    set
        //    {
        //        _inProcessOrdersCount = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private List<Order> _inProcessOrders;
        //public List<Order> InProcessOrders
        //{
        //    get { return _inProcessOrders; }
        //    set
        //    {
        //        _inProcessOrders = value;
        //        InProcessOrdersCount = "Заказов в процессе " + _inProcessOrders.Count + " (шт.)";
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region DoneOrders

        //private string _doneOrdersCount;
        //public string DoneOrdersCount
        //{
        //    get { return _doneOrdersCount; }
        //    set
        //    {
        //        _doneOrdersCount = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private List<Order> _doneOrders;
        //public List<Order> DoneOrders
        //{
        //    get { return _doneOrders; }
        //    set
        //    {
        //        _doneOrders = value;
        //        DoneOrdersCount = "Выполненных заказов " + _doneOrders.Count + " шт.";
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        public HomeCashier()
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
            PreparedFood = ShowCase.GetPreparedFood().FindAll(s => s.PreparedFood.Name.ToLower().Contains(AppManager.GetSearchText()));
            //ReadyOrders = Order.Get().FindAll(s => s.Food.Exists(d => d.Name.ToLower().Contains(AppManager.GetSearchText())));
            //InProcessOrders = Order.Get().FindAll(s => s.Food.Exists(d => d.Name.ToLower().Contains(AppManager.GetSearchText())));
            //DoneOrders = Order.Get().FindAll(s => s.Food.Exists(d => d.Name.ToLower().Contains(AppManager.GetSearchText())));
        }
    }
}
