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
    class ManageRequest : BaseViewModel
    {
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

                if (_selectedRequest != null)
                    RequestStatusName = _selectedRequest.StatusName;

                NewRequestStatusName = "";

                OnPropertyChanged();
            }
        }

        private bool _isActiveRequests;
        public bool IsActiveRequests
        {
            get { return _isActiveRequests; }
            set
            {
                _isActiveRequests = value;

                FillData();

                OnPropertyChanged();
            }
        }

        private bool _isNotActiveRequests;
        public bool IsNotActiveRequests
        {
            get { return _isNotActiveRequests; }
            set
            {
                _isNotActiveRequests = value;
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

                Search();

                OnPropertyChanged();
            }
        }

        private string _requestStatusName;
        public string RequestStatusName
        {
            get { return "Статус: " + _requestStatusName; }
            set
            {
                _requestStatusName = value;
                OnPropertyChanged();
            }
        }

        private string _newRequestStatusName;
        public string NewRequestStatusName
        {
            get { return "Новый статус: " + _newRequestStatusName; }
            set
            {
                _newRequestStatusName = value;
                OnPropertyChanged();
            }
        }

        public Command COM_CheckCart { get; set; }
        public Command COM_Cancel { get; set; }
        public Command COM_Save { get; set; }

        public ManageRequest()
        {
            InitializeCommand();
            IsActiveRequests = true;
        }

        public ManageRequest(DeliveryRequest request)
        {
            InitializeCommand();

            if (request.Status == eRequestStatus.Consideration || request.Status == eRequestStatus.Confirmed)
                IsActiveRequests = true;
            else
                IsActiveRequests = false;

            SelectedRequest = RequestList.Find(s => s.ID == request.ID);
        }

        private void InitializeCommand()
        {
            COM_CheckCart = new Command(c =>
            {
                if (SelectedRequest == null)
                    return;

                MessageBox.Show(CheckCart());
            });

            COM_Cancel = new Command(c =>
            {
                UpdateCurrent();
            });

            COM_Save = new Command(c =>
            {
                if (SelectedRequest == null)
                    return;

                SelectedRequest.Edit();

                UpdateCurrent();
            });
        }

        private void FillData()
        {
            if (IsActiveRequests)
                RequestList = DeliveryRequest.GetActiveRequests();
            else
                RequestList = DeliveryRequest.Collection;

            if (RequestList.Count != 0)
                SelectedRequest = RequestList[0];

            RequestSearch = "";
        }

        private void Search()
        {
            var result = new List<DeliveryRequest>();
            if (IsActiveRequests)
                result = DeliveryRequest.GetActiveRequests();
            else
                result = DeliveryRequest.Collection;

            if (RequestSearch != "")
                result = result.FindAll(s => s.Name.Contains(RequestSearch));

            RequestList = result;

            if (RequestList.Count != 0)
                SelectedRequest = RequestList[0];
        }

        private void UpdateCurrent()
        {
            int selectedID = 0;
            if (SelectedRequest != null)
                selectedID = SelectedRequest.ID;

            FillData();

            if (RequestList.Count != 0 && RequestList.Exists(s => s.ID == selectedID))
                SelectedRequest = RequestList.Find(s => s.ID == selectedID);
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
