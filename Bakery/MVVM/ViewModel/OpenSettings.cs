using Bakery.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class OpenSettings : BaseViewModel
    {
        public Command COM_CreateProduct { get; set; }
        public Command COM_CreateFood { get; set; }
        public Command COM_CreateEmployee { get; set; }
        public Command COM_CreateProvider { get; set; }

        public OpenSettings()
        {
            COM_CreateProduct = new Command(c =>
            {
                AppManager.OpenWindow(new View.CreateProduct(), new CreateProduct(), true);
            });

            COM_CreateFood = new Command(c =>
            {
                AppManager.OpenWindow(new View.CreateFood(), new CreateFood(), true);
            });

            COM_CreateEmployee = new Command(c =>
            {
                AppManager.OpenWindow(new View.CreateEmployee(), new CreateEmployee(), true);
            });

            COM_CreateProvider = new Command(c =>
            {
                AppManager.OpenWindow(new View.CreateProvider(), new CreateProvider(), true);
            });
        }
    }
}
