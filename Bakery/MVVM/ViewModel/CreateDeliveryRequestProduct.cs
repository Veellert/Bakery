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
    class CreateDeliveryRequestProduct : BaseViewModel
    {
        private List<Provider> _providerList;
        public List<Provider> ProviderList
        {
            get { return _providerList; }
            set
            {
                _providerList = value;
                OnPropertyChanged();
            }
        }
        private Provider _selectedProvider;
        public Provider SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
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
                OnPropertyChanged();
            }
        }
        private DeliveryRequest _selectedRequest;
        public DeliveryRequest SelectedRequest
        {
            get { return _selectedRequest; }
            set
            {
                _selectedRequest = value;
                OnPropertyChanged();
            }
        }

        private string _requestSearch;
        public string RequestSearch
        {
            get { return _requestSearch; }
            set
            {
                _requestSearch = value;

                SearchRequest();

                OnPropertyChanged();
            }
        }

        private string _providerSearch;
        public string ProviderSearch
        {
            get { return _providerSearch; }
            set
            {
                _providerSearch = value;

                SearchProvider();

                OnPropertyChanged();
            }
        }

        public Command COM_CheckCart { get; set; }
        public Command COM_Clear { get; set; }
        public Command COM_CreateDelivery { get; set; }

        public CreateDeliveryRequestProduct()
        {
            InitializeCommand();

            FillDefault();
        }

        private void InitializeCommand()
        {
            COM_CheckCart = new Command(c =>
            {
                if (SelectedRequest == null)
                    return;

                new MessageView(CheckCart());
            });

            COM_Clear = new Command(c =>
            {
                FillDefault();
            });

            COM_CreateDelivery = new Command(c =>
            {
                if (SelectedRequest == null || SelectedProvider == null)
                    return;

                new Delivery()
                {
                    Provider = SelectedProvider,
                    DeliveryDate = DateTime.Now,
                    Products = SelectedRequest.Products,
                }.Add();

                SelectedRequest.Status = eRequestStatus.Completed;
                SelectedRequest.Edit();

                new MessageView("Продукты успешно поставлены на склад");
                AppManager.CloseActiveWindow(new View.CreateDeliveryRequestProduct());
            });
        }

        private void FillDefault()
        {
            RequestList = DeliveryRequest.GetConfirmedRequests();

            if (RequestList.Count > 0)
                SelectedRequest = RequestList[0];

            ProviderList = Provider.Collection;

            if (ProviderList.Count > 0)
                SelectedProvider = ProviderList[0];

            RequestSearch = "";
            ProviderSearch = "";
        }

        private void SearchRequest()
        {
            var result = DeliveryRequest.GetConfirmedRequests();
            if (RequestSearch != "")
                result = DeliveryRequest.GetConfirmedRequests().FindAll
                (s => s.Name.ToLower().Contains(RequestSearch));

            RequestList = result;
            if (RequestList.Count > 0)
                SelectedRequest = RequestList[0];
        }

        private void SearchProvider()
        {
            var result = Provider.Collection;
            if (ProviderSearch != "")
                result = Provider.Collection.FindAll
                (s => s.Name.ToLower().Contains(ProviderSearch));

            ProviderList = result;
            if (ProviderList.Count > 0)
                SelectedProvider = ProviderList[0];
        }

        private string CheckCart()
        {
            string result = "";

            SelectedRequest.Products.ForEach(s => result += s.Name + " - " + s.Weight + " гр/мл/шт" + "\n");
            result += "\n" + SelectedRequest.TProductsCount;

            return result;
        }
    }
}
