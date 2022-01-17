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
    class AddShowcaseFood : BaseViewModel
    {
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

                int count = 0;
                if (_selectedFood != null)
                    count = ShowCase.Collection.Find(s => s.PreparedFood.ID == _selectedFood.ID).Count;
                ShowcaseFoodCount = count;

                OnPropertyChanged();
            }
        }

        private int _showcaseFoodCount;
        public int ShowcaseFoodCount
        {
            get { return _showcaseFoodCount; }
            set
            {
                _showcaseFoodCount = value;
                OnPropertyChanged();
            }
        }

        private int _foodCount;
        public int FoodCount
        {
            get { return _foodCount; }
            set
            {
                _foodCount = value;
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

        public Command COM_ShowConsistency { get; set; }
        public Command COM_Clear { get; set; }
        public Command COM_AddToShowcase { get; set; }

        public AddShowcaseFood()
        {
            InitializeCommand();

            FillDefault();
        }

        private void InitializeCommand()
        {
            COM_ShowConsistency = new Command(c =>
            {
                if (SelectedFood != null)
                    AppManager.OpenWindow(new View.CheckFoodConsistency(), new CheckFoodConsistency(SelectedFood));
            });

            COM_Clear = new Command(c =>
            {
                FillDefault();
            });

            COM_AddToShowcase = new Command(c =>
            {
                if (SelectedFood == null || FoodCount <= 0)
                    return;

                var storageConsistency = new List<Product>();
                foreach (var product in SelectedFood.Consistency)
                {
                    var prod = Product.Collection.Find(s => s.ID == product.ID);
                    int totalWeight = product.Weight * FoodCount;

                    if (prod.Weight < totalWeight)
                        storageConsistency.Add(prod);
                }

                if(storageConsistency.Count != 0)
                {
                    string message = "Не хватает: \n";

                    foreach (var product in storageConsistency)
                    {
                        int totalWeight = SelectedFood.Consistency.Find(s => s.ID == product.ID).Weight * FoodCount;
                        int count = Math.Abs(product.Weight - totalWeight);

                        message += product.Name + " - " + count + " гр/мл/шт \n";
                    }

                    MessageBox.Show(message);
                    return;
                }

                ShowCase.Increment(SelectedFood, FoodCount);
                foreach (var product in SelectedFood.Consistency)
                {
                    var storageProd = Product.Collection.Find(s => s.ID == product.ID);
                    storageProd.Weight -= product.Weight * FoodCount;
                    storageProd.Edit();
                }

                int id = SelectedFood.ID;
                FillDefault();
                if (FoodList.Count > 0)
                    SelectedFood = FoodList.Find(s => s.ID == id);
            });
        }

        private void FillDefault()
        {
            FoodList = Food.Collection;
            FoodCount = 0;

            if (FoodList.Count > 0)
            {
                SelectedFood = FoodList[0];
                FoodCount = 1;
            }

            FoodSearch = "";
        }

        private void Search()
        {
            var result = Food.Collection;
            if (FoodSearch != "")
                result = Food.Collection.FindAll
                (s => s.Name.ToLower().Contains(FoodSearch.Trim()));

            FoodList = result;
            if (FoodList.Count > 0)
                SelectedFood = FoodList[0];
        }
    }
}
