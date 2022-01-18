using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowRequest : BaseViewModel
    {
        private string _activeRequestCount;
        public string ActiveRequestCount
        {
            get { return _activeRequestCount; }
            set
            {
                _activeRequestCount = value;
                OnPropertyChanged();
            }
        }
        private List<DeliveryRequest> _activeRequestList;
        public List<DeliveryRequest> ActiveRequestList
        {
            get { return _activeRequestList; }
            set
            {
                _activeRequestList = value;

                ActiveRequestCount = "Активных заявок: " + _activeRequestList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        private string _requestCount;
        public string RequestCount
        {
            get { return _requestCount; }
            set
            {
                _requestCount = value;
                OnPropertyChanged();
            }
        }
        private List<DeliveryRequest> _requestList;
        public List<DeliveryRequest> RequestList
        {
            get { return _requestList; }
            set
            {
                _requestList = value;

                RequestCount = "Всего заявок: " + _requestList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        public ShowRequest()
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
            RequestList = DeliveryRequest.Collection.FindAll(s => s.Name.ToLower().Contains(AppManager.GetSearchText()));
            ActiveRequestList = DeliveryRequest.GetActiveRequests().FindAll(s => s.Name.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
