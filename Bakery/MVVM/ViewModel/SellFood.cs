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
    class SellFood : ShoppingCartViewModel
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

                OnPropertyChanged(); }
        }

        private List<ShowCaseFood> _showCaseList;
        public List<ShowCaseFood> ShowCaseList
        {
            get { return _showCaseList; }
            set
            {
                _showCaseList = value;
                OnPropertyChanged();
            }
        }

        private ShowCaseFood _selectedFood;
        public ShowCaseFood SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                _selectedFood = value;
                OnPropertyChanged();
            }
        }

        private string _showCaseSearch;
        public string ShowCaseSearch
        {
            get { return _showCaseSearch?.Trim().ToLower(); }
            set
            {
                _showCaseSearch = value;
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
        public Command COM_Sell { get; set; }

        public SellFood()
        {
            InitializeCommand();

            FillDefault();
        }

        private void InitializeCommand()
        {
            COM_AddToCart = new Command(c =>
            {
                if (SelectedFood == null || SelectedFood.Count == 0)
                    return;

                int id = SelectedFood.PreparedFood.ID;
                var result = new List<ShowCaseFood>();
                result.AddRange(ShoppingList);

                if (!result.Exists(s => s.PreparedFood.ID == SelectedFood.PreparedFood.ID))
                    result.Add(new ShowCaseFood() { PreparedFood = SelectedFood.PreparedFood, Count = 1, });
                else
                    result.Find(s => s.PreparedFood.ID == SelectedFood.PreparedFood.ID).Count++;

                ShoppingList = result;
                GetPreparedFoodWhith(SelectedFood, true);
                if (ShowCaseList.Count > 0)
                    SelectedFood = ShowCaseList.First(s => s.PreparedFood.ID == id);
            });

            COM_CheckCart = new Command(c =>
            {
                new MessageView(CheckCart());
            });

            COM_Clear = new Command(c =>
            {
                FillDefault();
            });

            COM_Sell = new Command(c =>
            {
                if (ShoppingList.Count == 0)
                {
                    new MessageView("Нечего продавать");
                    return;
                }

                AppManager.OpenWindow(new View.SellFoodConfirm(), new SellFoodConfirm(TotalPrice, ShoppingList), eEmployeeType.Cashier);
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
            GetPreparedFoodWhith(food);
            if (ShowCaseList.Count > 0)
                SelectedFood = ShowCaseList.First(s => s.PreparedFood.ID == food.PreparedFood.ID);
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
            ShowCaseList = ShowCase.GetPreparedFood();

            if (ShowCaseList.Count > 0)
                SelectedFood = ShowCaseList[0];

            ShowCaseSearch = "";
        }

        private void GetPreparedFoodWhith(ShowCaseFood food, bool without = false)
        {
            var result = new List<ShowCaseFood>();
            foreach (var showcase in ShowCaseList)
                result.Add(new ShowCaseFood()
                { PreparedFood = showcase.PreparedFood, Count = showcase.Count, });

            if (result.Exists(s => s.PreparedFood.ID == food.PreparedFood.ID))
            {
                if (without)
                    result.Find(s => s.PreparedFood.ID == food.PreparedFood.ID).Count--;
                else
                    result.Find(s => s.PreparedFood.ID == food.PreparedFood.ID).Count++;
            }

            ShowCaseList = result;
        }

        private void Search()
        {
            var result = ShowCase.GetPreparedFood();
            if (ShowCaseSearch != "")
                result = ShowCase.GetPreparedFood().FindAll
                (s => s.PreparedFood.Name.ToLower().Contains(ShowCaseSearch));

            ShowCaseList = result;
            if (ShowCaseList.Count > 0)
                SelectedFood = ShowCaseList[0];
        }
    }
}
