using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowProviders : BaseViewModel
    {
        private string _providerCount;
        public string ProviderCount
        {
            get { return _providerCount; }
            set
            {
                _providerCount = value;
                OnPropertyChanged();
            }
        }
        private List<Provider> _providerList;
        public List<Provider> ProviderList
        {
            get { return _providerList; }
            set
            {
                _providerList = value;

                ProviderCount = "Всего поставщиков: " + _providerList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        public ShowProviders()
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
            ProviderList = Provider.Collection.FindAll(s => s.Name.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
