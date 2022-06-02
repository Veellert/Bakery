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
    class CreateDeliveryProduct : BaseViewModel
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

        private List<Product> _deliveryProductList;
        public List<Product> DeliveryProductList
        {
            get { return _deliveryProductList; }
            set
            {
                _deliveryProductList = value;
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

                SearchProduct();

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

        public Command COM_AddProductToDelivery { get; set; }
        public Command COM_Clear { get; set; }
        public Command COM_CreateDelivery { get; set; }

        public CreateDeliveryProduct()
        {
            InitializeCommand();

            FillDefault();
        }
        
        public CreateDeliveryProduct(Product product)
        {
            InitializeCommand();

            FillDefault();

            var prod = new Product()
            {
                ID = product.ID,
                Name = product.Name,
                Weight = product.DeliveryWeight,
            };
            DeliveryProductList.Add(prod);
        }

        private void InitializeCommand()
        {
            COM_AddProductToDelivery = new Command(c =>
            {
                if (SelectedProduct == null || Weight <= 0)
                    return;

                var prodList = new List<Product>();
                prodList.AddRange(DeliveryProductList);

                if (!prodList.Exists(s => s.ID == SelectedProduct.ID))
                    prodList.Add(new Product() { ID = SelectedProduct.ID, Name = SelectedProduct.Name, Weight = Weight });
                else
                    prodList.Find(s => s.ID == SelectedProduct.ID).Weight += Weight;

                Weight = 1;
                DeliveryProductList = prodList;
            });

            COM_Clear = new Command(c =>
            {
                FillDefault();
            });

            COM_CreateDelivery = new Command(c =>
            {
                if (DeliveryProductList.Count == 0 || SelectedProvider == null)
                    return;

                new Delivery()
                { 
                    Provider = SelectedProvider,
                    DeliveryDate = DateTime.Now,
                    Products = DeliveryProductList,
                }.Add();

                new MessageView("Продукты успешно поставлены на склад");
                AppManager.CloseActiveWindow(new View.CreateDeliveryProduct());
            });
        }

        public void RemoveFromDelivery(Product product)
        {
            var prodList = new List<Product>();
            prodList.AddRange(DeliveryProductList);
            prodList.RemoveAll(s => s.ID == product.ID);

            DeliveryProductList = prodList;
        }

        private void FillDefault()
        {
            DeliveryProductList = new List<Product>();
            ProductList = Product.Collection;

            if (ProductList.Count > 0)
                SelectedProduct = ProductList[0];

            ProviderList = Provider.Collection;

            if (ProviderList.Count > 0)
                SelectedProvider = ProviderList[0];

            Weight = 1;
            ProductSearch = "";
            ProviderSearch = "";
        }

        private void SearchProduct()
        {
            var result = Product.Collection;
            if (ProductSearch != "")
                result = Product.Collection.FindAll
                (s => s.FullName.ToLower().Contains(ProductSearch));

            ProductList = result;
            if (ProductList.Count > 0)
                SelectedProduct = ProductList[0];
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
    }
}
