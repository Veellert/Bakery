using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowDelivery : BaseViewModel
    {
        private string _weekDeliveryCount;
        public string WeekDeliveryCount
        {
            get { return _weekDeliveryCount; }
            set
            {
                _weekDeliveryCount = value;
                OnPropertyChanged();
            }
        }
        private List<Delivery> _weekDeliveryList;
        public List<Delivery> WeekDeliveryList
        {
            get { return _weekDeliveryList; }
            set
            {
                _weekDeliveryList = value;

                WeekDeliveryCount = "Поставок за неделю: " + _weekDeliveryList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        private string _deliveryCount;
        public string DeliveryCount
        {
            get { return _deliveryCount; }
            set
            {
                _deliveryCount = value;
                OnPropertyChanged();
            }
        }
        private List<Delivery> _deliveryList;
        public List<Delivery> DeliveryList
        {
            get { return _deliveryList; }
            set
            {
                _deliveryList = value;

                DeliveryCount = "Всего поставок: " + _deliveryList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        public ShowDelivery()
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
            DeliveryList = Delivery.Collection.FindAll(s => s.FullName.ToLower().Contains(AppManager.GetSearchText()));
            WeekDeliveryList = DeliveryList.FindAll(s => s.DeliveryDate.Date >= DateTime.Today.AddDays(-7));
        }
    }
}
