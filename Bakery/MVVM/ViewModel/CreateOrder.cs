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
    class CreateOrder : ShoppingCartViewModel
    {
        private List<ShowCaseFood> _shoppingList;
        public List<ShowCaseFood> ShoppingList
        {
            get { return _shoppingList; }
            set
            {
                _shoppingList = value;

                TotalPrice = 0;
                _shoppingList.ForEach(s => TotalPrice += s.PreparedFood.Price * s.Count);

                OnPropertyChanged();
            }
        }

        private List<Food> _foodList;
        public List<Food> FoodList
        {
            get { return _foodList; }
            set
            {
                _foodList = value;
                OnPropertyChanged();
            }
        }

        private Food _selectedFood;
        public Food SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                _selectedFood = value;
                OnPropertyChanged();
            }
        }

        private string _foodSearch;
        public string FoodSearch
        {
            get { return _foodSearch?.Trim().ToLower(); }
            set
            {
                _foodSearch = value;
                Search();
                OnPropertyChanged();
            }
        }

        private float _totalPrice;
        public float TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                PriceText = _totalPrice + " рублей";
                OnPropertyChanged();
            }
        }

        private string _priceText;
        public string PriceText
        {
            get { return _priceText; }
            set
            {
                _priceText = value;
                OnPropertyChanged();
            }
        }

        public Command COM_AddToCart { get; set; }
        public Command COM_CheckCart { get; set; }
        public Command COM_Clear { get; set; }
        public Command COM_Create { get; set; }

        public CreateOrder()
        {
            InitializeCommand();

            FillDefault();
        }

        private void InitializeCommand()
        {
            COM_AddToCart = new Command(c =>
            {
                if (SelectedFood == null)
                    return;

                var result = new List<ShowCaseFood>();
                result.AddRange(ShoppingList);

                if (!result.Exists(s => s.PreparedFood.ID == SelectedFood.ID))
                    result.Add(new ShowCaseFood() { PreparedFood = SelectedFood, Count = 1, });
                else
                    result.Find(s => s.PreparedFood.ID == SelectedFood.ID).Count++;

                ShoppingList = result;
            });

            COM_CheckCart = new Command(c =>
            {
                new MessageView(CheckCart());
            });

            COM_Clear = new Command(c =>
            {
                FillDefault();
            });

            COM_Create = new Command(c =>
            {
                if (ShoppingList.Count == 0)
                {
                    new MessageView("Корзина пуста");
                    return;
                }

                Order order = new Order(ShoppingList);
                order.Add();

                new MessageView("Создан " + order.Name);
                AppManager.CloseActiveWindow(new View.CreateOrder());
            });
        }

        public override void RemoveFromCart(ShowCaseFood food)
        {
            var result = new List<ShowCaseFood>();
            result.AddRange(ShoppingList);

            if (result.Find(s => s.PreparedFood.ID == food.PreparedFood.ID).Count == 1)
                result.RemoveAll(s => s.PreparedFood.ID == food.PreparedFood.ID);
            else
                result.Find(s => s.PreparedFood.ID == food.PreparedFood.ID).Count--;

            ShoppingList = result;
        }

        private string CheckCart()
        {
            if (ShoppingList.Count == 0)
                return "Пусто";

            string result = "";
            ShoppingList.ForEach(s => result += s.TFullName + "\n");
            result += "\nВсего " + TotalPrice + " рублей.";

            return result;
        }

        private void FillDefault()
        {
            ShoppingList = new List<ShowCaseFood>();
            FoodList = Food.Collection;

            if (FoodList.Count > 0)
                SelectedFood = FoodList[0];

            FoodSearch = "";
        }

        private void Search()
        {
            var result = Food.Collection;
            if (FoodSearch != "")
                result = Food.Collection.FindAll
                (s => s.Name.ToLower().Contains(FoodSearch));

            FoodList = result;
            if (FoodList.Count > 0)
                SelectedFood = FoodList[0];
        }
    }
}
