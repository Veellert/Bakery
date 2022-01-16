using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class HomeBaker : BaseViewModel
    {
        #region InProcessOrders

        private string _inProcessOrdersCount;
        public string InProcessOrdersCount
        {
            get { return _inProcessOrdersCount; }
            set
            {
                _inProcessOrdersCount = value;
                OnPropertyChanged();
            }
        }

        private List<Order> _inProcessOrders;
        public List<Order> InProcessOrders
        {
            get { return _inProcessOrders; }
            set
            {
                _inProcessOrders = value;
                InProcessOrdersCount = "Заказов в процессе " + _inProcessOrders.Count + " (шт.)";
                OnPropertyChanged();
            }
        }

        #endregion

        #region EmptyFood

        private string _emptyFoodCount;
        public string EmptyFoodCount
        {
            get { return _emptyFoodCount; }
            set
            {
                _emptyFoodCount = value;
                OnPropertyChanged();
            }
        }

        private List<ShowCaseFood> _emptyFood;
        public List<ShowCaseFood> EmptyFood
        {
            get { return _emptyFood; }
            set
            {
                _emptyFood = value;

                int count = 0;
                _emptyFood.ForEach(s => count += s.Count);

                EmptyFoodCount = "Товаров нет в наличии: " + count + " (шт.)";

                OnPropertyChanged();
            }
        }

        #endregion

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
        
        public HomeBaker()
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
            InProcessOrders = Order.GetActiveOrders().FindAll(s => s.Status == eOrderStatus.InProcess &&
            (s.Name.Contains(AppManager.GetSearchText()) || s.TStartDate.Contains(AppManager.GetSearchText())));

            EmptyFood = ShowCase.GetEmptyFood().FindAll(s => s.PreparedFood.Name.ToLower().Contains(AppManager.GetSearchText()));

            PreparedFood = ShowCase.GetPreparedFood().FindAll(s => s.PreparedFood.Name.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
