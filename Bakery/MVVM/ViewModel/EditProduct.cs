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
    class EditProduct : BaseViewModel
    {
        public Product CurrentProduct { get; set; }

        private string _oldName;
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

        public Command COM_Save { get; set; }

        public EditProduct(Product product)
        {
            CurrentProduct = product;

            _oldName = CurrentProduct.Name;
            ProductName = CurrentProduct.Name;

            COM_Save = new Command(c =>
            {
                if (ProductName == "")
                    return;
                if(Product.Collection.Exists(s => s.Name == ProductName))
                {
                    new MessageView("Такой продукт уже существует");
                    return;
                }

                new MessageView($"Продукт изменен с '{_oldName}' на '{ProductName}'");

                CurrentProduct.Name = ProductName;
                CurrentProduct.Edit();

                AppManager.CloseActiveWindow(new View.EditProduct());
            });
        }
    }
}
