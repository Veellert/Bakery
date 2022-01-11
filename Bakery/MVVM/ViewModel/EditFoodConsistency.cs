using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class EditFoodConsistency : BaseViewModel
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
                {
                    StorageCount = _selectedProduct.TStorageWeight;
                    ProductWeight = _selectedProduct.Weight;
                }

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

        public EditFoodConsistency(Food food, Product product)
        {
            CurrentFood = food;
            ProductList = Product.GetFoodConsistency(CurrentFood.ID);
            if (ProductList.Count != 0)
                SelectedProduct = ProductList.Find(s => s.ID == product.ID);

            COM_ClearSearch = new Command(s => ClearSearch());
            COM_Cancel = new Command(s => Cancel());
            COM_Save = new Command(s => Save());
        }

        private void Search()
        {
            if (ProductSearch != "")
                ProductList = Product.GetFoodConsistency(CurrentFood.ID).FindAll(s => s.Name.ToLower().Contains(ProductSearch));
            else
                ProductList = Product.GetFoodConsistency(CurrentFood.ID);

            if (ProductList.Count != 0)
                SelectedProduct = ProductList[0];
        }

        private void ClearSearch()
        {
            ProductSearch = "";
        }

        private void Cancel()
        {
            ProductWeight = SelectedProduct.Weight;
        }

        private void Save()
        {
            if (ProductWeight == 0 || SelectedProduct == null)
                return;

            var product = SelectedProduct;
            product.Weight = ProductWeight;

            product.EditFoodConsistancy(CurrentFood);

            ClearSearch();
        }
    }
}
