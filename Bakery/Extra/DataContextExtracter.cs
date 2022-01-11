using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Extra
{
    class DataContextExtracter<T> where T : MVVM.ViewModel.BaseViewModel
    {
        public static T Extract(System.Windows.Window window) => (T)window.DataContext;

        public static T Extract() => (T)AppManager.GetActiveWindow().DataContext;
    }
}
