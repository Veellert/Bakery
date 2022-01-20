using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class AddFoodConsistency : BaseViewModel
    {
        private List<Product> _productList;
        public List<Product> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;
                OnPropertyChanged();
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;

                if (_selectedProduct != null)
                    StorageCount = _selectedProduct.TStorageWeight;
                ProductWeight = 0;

                OnPropertyChanged();
            }
        }

        private string _productSearch;
        public string ProductSearch
        {
            get { return _productSearch?.ToLower().Trim(); }
            set
            {
                _productSearch = value;
                Search();
                OnPropertyChanged();
            }
        }

        private int _productWeight;
        public int ProductWeight
        {
            get { return _productWeight; }
            set
            {
                _productWeight = value;
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

        public Food CurrentFood { get; set; }

        public Command COM_ClearSearch { get; set; }
        public Command COM_Cancel { get; set; }
        public Command COM_Save { get; set; }

        public AddFoodConsistency(Food food)
        {
            CurrentFood = food;
            ProductList = FillProducts();
            if (ProductList.Count != 0)
                SelectedProduct = ProductList[0];

            COM_ClearSearch = new Command(s => ClearSearch());
            COM_Cancel = new Command(s => Cancel());
            COM_Save = new Command(s => Save());
        }

        private List<Product> FillProducts()
        {
            var result = new List<Product>();
            result.AddRange(Product.Collection);

            foreach (var product in Product.GetFoodConsistency(CurrentFood.ID))
                result.RemoveAll(s => s.ID == product.ID);

            return result;
        }

        private void Search()
        {
            if (ProductSearch != "")
                ProductList = FillProducts().FindAll(s => s.Name.ToLower().Contains(ProductSearch));
            else
                ProductList = FillProducts();

            if (ProductList.Count != 0)
                SelectedProduct = ProductList[0];
        }

        private void ClearSearch()
        {
            ProductSearch = "";
        }

        private void Cancel()
        {
            AppManager.CloseActiveWindow(new View.AddFoodConsistency());
        }

        private void Save()
        {
            if (ProductWeight == 0 || SelectedProduct == null)
                return;

            var product = SelectedProduct;
            product.Weight = ProductWeight;

            product.AddFoodConsistancy(CurrentFood);

            ProductList = FillProducts();
            if (ProductList.Count != 0)
                SelectedProduct = ProductList[0];
        }
    }
}
