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
    class CreateDeliveryRequest : BaseViewModel
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
                OnPropertyChanged();
            }
        }

        private List<Product> _requestProductList;
        public List<Product> RequestProductList
        {
            get { return _requestProductList; }
            set
            {
                _requestProductList = value;
                OnPropertyChanged();
            }
        }

        private string _productSearch;
        public string ProductSearch
        {
            get { return _productSearch; }
            set
            {
                _productSearch = value;

                Search();

                OnPropertyChanged();
            }
        }

        private int _weight;
        public int Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }

        public Command COM_AddProductToRequest { get; set; }
        public Command COM_Clear { get; set; }
        public Command COM_CreateRequest { get; set; }

        public CreateDeliveryRequest()
        {
            InitializeCommand();

            FillDefault();
        }

        private void InitializeCommand()
        {
            COM_AddProductToRequest = new Command(c => 
            {
                if (SelectedProduct == null || Weight <= 0)
                    return;

                var prodList = new List<Product>();
                prodList.AddRange(RequestProductList);

                if (!prodList.Exists(s => s.ID == SelectedProduct.ID))
                    prodList.Add(new Product() { ID = SelectedProduct.ID, Name = SelectedProduct.Name, Weight = Weight });
                else
                    prodList.Find(s => s.ID == SelectedProduct.ID).Weight += Weight;

                Weight = 1;
                RequestProductList = prodList;
            });
            
            COM_Clear = new Command(c => 
            {
                FillDefault();
            });

            COM_CreateRequest = new Command(c => 
            {
                if (RequestProductList.Count == 0)
                    return;

                new DeliveryRequest()
                {
                    Products = RequestProductList,
                    Status = eRequestStatus.Consideration,
                }.Add();

                MessageBox.Show("Заявка успешно создана");
                AppManager.CloseActiveWindow(new View.CreateDeliveryRequest());
            });
        }

        public void RemoveFromRequest(Product product)
        {
            var prodList = new List<Product>();
            prodList.AddRange(RequestProductList);
            prodList.RemoveAll(s => s.ID == product.ID);
            
            RequestProductList = prodList;
        }

        private void FillDefault()
        {
            RequestProductList = new List<Product>();
            ProductList = Product.Collection;

            if (ProductList.Count > 0)
                SelectedProduct = ProductList[0];

            Weight = 1;
            ProductSearch = "";
        }

        private void Search()
        {
            var result = Product.Collection;
            if (ProductSearch != "")
                result = Product.Collection.FindAll
                (s => s.FullName.ToLower().Contains(ProductSearch));

            ProductList = result;
            if (ProductList.Count > 0)
                SelectedProduct = ProductList[0];
        }
    }
}
