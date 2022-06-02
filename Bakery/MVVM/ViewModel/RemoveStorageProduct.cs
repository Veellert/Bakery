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
    class RemoveStorageProduct : BaseViewModel
    {
        public Product CurrentProduct { get; set; }

        private int _removeCount;
        public int RemoveCount
        {
            get { return _removeCount; }
            set
            {
                _removeCount = value;
                OnPropertyChanged();
            }
        }

        public Command COM_Remove { get; set; }

        public RemoveStorageProduct(Product product)
        {
            CurrentProduct = product;
            RemoveCount = 1;

            COM_Remove = new Command(c => 
            {
                if (RemoveCount <= 0)
                    return;

                var prod = Product.Collection.Find(s => s.ID == CurrentProduct.ID);

                if(RemoveCount > prod.Weight)
                {
                    new MessageView("Слишком большое число");
                    return;
                }

                prod.Weight -= RemoveCount;
                prod.Edit();

                new MessageView($"Со склада списано {RemoveCount}гр/мл/шт \nСейчас на складе: {prod.Weight} гр/мл/шт");
                AppManager.CloseActiveWindow(new View.RemoveStorageProduct());
            });
        }
    }
}
