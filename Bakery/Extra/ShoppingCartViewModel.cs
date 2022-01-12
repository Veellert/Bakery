using Bakery.MVVM.Model;
using Bakery.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Extra
{
    abstract class ShoppingCartViewModel : BaseViewModel
    {
        public abstract void RemoveFromCart(ShowCaseFood food);
    }
}
