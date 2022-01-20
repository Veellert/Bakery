using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class OpenEmployee : BaseViewModel
    {
        public Employee CurrentEmployee { get; set; }

        public OpenEmployee(Employee employee)
        {
            CurrentEmployee = employee;
        }
    }
}
