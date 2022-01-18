using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowStorage : BaseViewModel
    {
        private string _emptyCount;
        public string EmptyCount
        {
            get { return _emptyCount; }
            set
            {
                _emptyCount = value;
                OnPropertyChanged();
            }
        }
        private List<Product> _emptyProductList;
        public List<Product> EmptyProductList
        {
            get { return _emptyProductList; }
            set
            {
                _emptyProductList = value;

                EmptyCount = "Закончилось продуктов: " + _emptyProductList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        private string _storageCount;
        public string StorageCount
        {
            get { return _storageCount; }
            set
            {
                _storageCount = value;
                OnPropertyChanged();
            }
        }
        private List<Product> _productList;
        public List<Product> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;

                StorageCount = "Всего продуктов: " + _productList.Count + " (шт.)";

                OnPropertyChanged();
            }
        }

        public ShowStorage()
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
            ProductList = Product.Collection.FindAll(s => s.Name.ToLower().Contains(AppManager.GetSearchText()));
            EmptyProductList = Product.Collection.FindAll(s => s.Weight == 0 && s.Name.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
