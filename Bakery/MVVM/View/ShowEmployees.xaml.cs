using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bakery.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для ShowEmloyees.xaml
    /// </summary>
    public partial class ShowEmployees : UserControl
    {
        public ShowEmployees()
        {
            InitializeComponent();
        }

        private void StackPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = MouseWheelEvent;
            ((UIElement)sender).RaiseEvent(e2);
        }
    }
}
