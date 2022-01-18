using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class OpenRequest : BaseViewModel
    {
        public DeliveryRequest CurrentRequest { get; set; }

        public OpenRequest(DeliveryRequest request)
        {
            CurrentRequest = request;
        }
    }
}
