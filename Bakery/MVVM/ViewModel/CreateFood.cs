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
    class CreateFood : BaseViewModel
    {
        private int _price;
        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private string _foodName;
        public string FoodName
        {
            get { return _foodName; }
            set
            {
                _foodName = value;
                OnPropertyChanged();
            }
        }

        public Command COM_Create { get; set; }

        public CreateFood()
        {
            COM_Create = new Command(c =>
            {
                if (FoodName == "" || Price <= 0)
                    return;
                if (Food.Collection.Exists(s => s.Name == FoodName))
                {
                    MessageBox.Show("Такой товар уже существует");
                    return;
                }

                MessageBox.Show($"Товар '{FoodName}' успешно создан");

                new Food()
                {
                    Name = FoodName,
                    Price = Price,
                    Consistency = new List<Product>(),
                }.Add();
                AppManager.CloseActiveWindow(new View.CreateFood());

                AppManager.OpenWindow(new View.CheckFoodConsistency(), new CheckFoodConsistency(Food.Collection.Last()));
            });
        }
    }
}
