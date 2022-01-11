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
    class SellFoodConfirm : BaseViewModel
    {
        private float _depositedMony;
        public float DepositedMoney
        {
            get { return _depositedMony; }
            set
            {
                _depositedMony = value;
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
                OnPropertyChanged();
            }
        }

        private List<ShowCaseFood> _preparedFood { get; set; }

        public Command COM_Sell { get; set; }

        public SellFoodConfirm(float totalPrice, List<ShowCaseFood> preparedFood)
        {
            COM_Sell = new Command(s => Sell());

            TotalPrice = totalPrice;
            _preparedFood = preparedFood;
        }

        private void Sell()
        {
            if (DepositedMoney < TotalPrice)
                MessageBox.Show("Не хватает " + (TotalPrice - DepositedMoney) + " рублей");
            else
            {
                if (DepositedMoney > TotalPrice)
                    MessageBox.Show("Сдача " + (DepositedMoney - TotalPrice) + " рублей");

                SellFodd();

                AppManager.CloseActiveWindow();
                AppManager.CloseActiveWindow(new View.SellFood());
            }
        }

        private void SellFodd()
        {
            foreach (var food in _preparedFood)
                ShowCase.Decrement(food.PreparedFood, food.Count);
        }
    }
}
