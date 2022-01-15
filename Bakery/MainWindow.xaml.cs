using Bakery.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Eatable.Height = Uneatable.Height = 0;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Edible_Checked(object sender, RoutedEventArgs e) => Eatable.Height = 40;

        private void Edible_Unchecked(object sender, RoutedEventArgs e)
        {
            Eatable.Height = 0;

            orders.IsChecked = false;
            showcase.IsChecked = false;
            storage.IsChecked = false;
        }

        private void InEdible_Checked(object sender, RoutedEventArgs e) => Uneatable.Height = 40;

        private void InEdible_Unchecked(object sender, RoutedEventArgs e)
        {
            Uneatable.Height = 0;

            request.IsChecked = false;
            deliveries.IsChecked = false;
            staff.IsChecked = false;
            providers.IsChecked = false;
        }

        private void ItemPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Eatable.Height = Uneatable.Height = 0;

            orders.IsChecked = false;
            showcase.IsChecked = false;
            storage.IsChecked = false;

            request.IsChecked = false;
            deliveries.IsChecked = false;
            staff.IsChecked = false;
            providers.IsChecked = false;
        }

        private void Edible_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Edible.IsChecked.Value)
                Eatable.Height = 40;
        }

        private void InEdible_MouseEnter(object sender, MouseEventArgs e)
        {
            if (InEdible.IsChecked.Value)
                Uneatable.Height = 40;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Cashier)
                CashierPopup.Visibility = CashierPopup.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Baker)
                BakerPopup.Visibility = BakerPopup.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Manager)
                ManagerPopup.Visibility = ManagerPopup.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        private void OptionsButton_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Cashier)
                CashierPopup.Visibility = (string)CashierPopup.Tag == "0" ? Visibility.Hidden : Visibility.Visible;
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Baker)
                BakerPopup.Visibility = (string)BakerPopup.Tag == "0" ? Visibility.Hidden : Visibility.Visible;
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Manager)
                ManagerPopup.Visibility = (string)ManagerPopup.Tag == "0" ? Visibility.Hidden : Visibility.Visible;
        }

        private void OptionsPopup_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Cashier)
                CashierPopup.Tag = "1";
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Baker)
                BakerPopup.Tag = "1";
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Manager)
                ManagerPopup.Tag = "1";
        }

        private void OptionsPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Cashier)
            {
                CashierPopup.Tag = "0";
                CashierPopup.Visibility = Visibility.Hidden;
            }
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Baker)
            {
                BakerPopup.Tag = "0";
                BakerPopup.Visibility = Visibility.Hidden;
            }
            else if (AppManager.CurrentEmployee.Account.Type == MVVM.Model.eEmployeeType.Manager)
            {
                ManagerPopup.Tag = "0";
                ManagerPopup.Visibility = Visibility.Hidden;
            }
        }
    }
}
