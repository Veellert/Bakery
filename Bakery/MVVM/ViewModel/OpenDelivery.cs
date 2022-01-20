using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class OpenDelivery : BaseViewModel
    {
        public Delivery CurrentDelivery { get; set; }

        public OpenDelivery(Delivery delivery)
        {
            CurrentDelivery = delivery;
        }
    }
}
