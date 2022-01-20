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
    class CreateProduct : BaseViewModel
    {
        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public Command COM_Create { get; set; }

        public CreateProduct()
        {
            COM_Create = new Command(c =>
            {
                if (ProductName == "")
                    return;
                if (Product.Collection.Exists(s => s.Name == ProductName))
                {
                    MessageBox.Show("Такой продукт уже существует");
                    return;
                }

                MessageBox.Show($"Продукт '{ProductName}' успешно создан");

                new Product() { Name = ProductName }.Add();
                AppManager.CloseActiveWindow(new View.CreateProduct());
            });
        }
    }
}
