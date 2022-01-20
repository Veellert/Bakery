using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class CheckFoodConsistency : BaseViewModel
    {
        private List<Product> _consistencyList;
        public List<Product> ConsistencyList
        {
            get { return _consistencyList; }
            set
            {
                _consistencyList = value;
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
                FillConsistency();
                OnPropertyChanged();
            }
        }

        private string _foodSearch;
        public string FoodSearch
        {
            get { return _foodSearch?.ToLower().Trim(); }
            set
            {
                _foodSearch = value;
                Search();
                OnPropertyChanged();
            }
        }

        public Command COM_ClearSearch { get; set; }
        public Command COM_AddConsistency { get; set; }

        public CheckFoodConsistency(Food food)
        {
            FoodList = Food.Collection;
            SelectedFood = FoodList.Find(s => s.ID == food.ID);
            FillConsistency();

            COM_ClearSearch = new Command(s => ClearSearch());
            COM_AddConsistency = new Command(s => AddConsistency());
        }

        public void RedactConsistency(Product product)
        {
            AppManager.OpenWindow(new View.EditFoodConsistency(), new EditFoodConsistency(SelectedFood, product), true);
            FillConsistency();
        }

        public void RemoveFromConsistency(Product product)
        {
            Product.DeleteFoodConsistency(SelectedFood, product);
            FillConsistency();
        }

        private void AddConsistency()
        {
            AppManager.OpenWindow(new View.AddFoodConsistency(), new AddFoodConsistency(SelectedFood), true);
        }

        private void FillConsistency()
        {
            if (SelectedFood != null)
                ConsistencyList = Product.GetFoodConsistency(SelectedFood.ID);
        }

        private void ClearSearch()
        {
            FoodSearch = "";
        }

        private void Search()
        {
            var result = Food.Collection;
            if (FoodSearch != "")
                result = Food.Collection.FindAll(s => s.Name.ToLower().Contains(FoodSearch));

            FoodList = result;

            if (FoodList.Count > 0)
                SelectedFood = FoodList.First();
        }
    }
}
