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

namespace Bakery.UI.Cards
{
    /// <summary>
    /// Логика взаимодействия для OrderInfo.xaml
    /// </summary>
    public partial class OrderInfo : UserControl
    {
        public OrderInfo()
        {
            InitializeComponent();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e) => OptionsPopup.Visibility = OptionsPopup.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

        private void OptionsButton_LostFocus(object sender, RoutedEventArgs e) => OptionsPopup.Visibility = (string)OptionsPopup.Tag == "0" ? Visibility.Hidden : Visibility.Visible;

        private void OptionsPopup_MouseEnter(object sender, MouseEventArgs e) => OptionsPopup.Tag = "1";

        private void OptionsPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            OptionsPopup.Tag = "0";
            OptionsPopup.Visibility = Visibility.Hidden;
        }
    }
}
