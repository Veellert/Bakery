using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class HomeManager : BaseViewModel
    {
        private List<Employee> _activeEmployees;
        public List<Employee> ActiveEmployees
        {
            get { return _activeEmployees; }
            set
            {
                _activeEmployees = value;
                OnPropertyChanged();
            }
        }

        private List<Product> _emptyStorage;
        public List<Product> EmptyStorage
        {
            get { return _emptyStorage; }
            set
            {
                _emptyStorage = value;
                OnPropertyChanged();
            }
        }

        private List<DeliveryRequest> _activeRequests;
        public List<DeliveryRequest> ActiveRequests
        {
            get { return _activeRequests; }
            set
            {
                _activeRequests = value;
                OnPropertyChanged();
            }
        }

        public HomeManager()
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
            ActiveEmployees = Employee.GetActiveEmployees().FindAll(s => s.FIO.Contains(AppManager.GetSearchText()) || s.Account.Phone.Contains(AppManager.GetSearchText()));

            EmptyStorage = Product.Collection.FindAll(s => s.Weight == 0 && s.Name.ToLower().Contains(AppManager.GetSearchText()));

            ActiveRequests = DeliveryRequest.GetActiveRequests().FindAll(s => s.Name.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
