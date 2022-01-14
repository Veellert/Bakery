using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class OpenOrder : BaseViewModel
    {
        public Order CurrentOrder { get; set; }

        public OpenOrder(Order order)
        {
            CurrentOrder = order;
        }
    }
}
