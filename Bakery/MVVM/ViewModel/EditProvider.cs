using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bakery.MVVM.ViewModel
{
    class EditProvider : BaseViewModel
    {
        public Provider CurrentProvider { get; set; }

        private string _oldName;
        private string _providerName;
        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                _providerName = value;
                OnPropertyChanged();
            }
        }

        public Command COM_Save { get; set; }

        public EditProvider(Provider provider)
        {
            CurrentProvider = provider;

            _oldName = CurrentProvider.Name;
            ProviderName = CurrentProvider.Name;

            COM_Save = new Command(c =>
            {
                if (ProviderName == "")
                    return;
                if (Provider.Collection.Exists(s => s.Name == ProviderName))
                {
                    MessageBox.Show("Такой поставщик уже существует");
                    return;
                }

                MessageBox.Show($"Поставщик изменен с '{_oldName}' на '{ProviderName}'");

                CurrentProvider.Name = ProviderName;
                CurrentProvider.Edit();

                AppManager.CloseActiveWindow(new View.EditProvider());
            });
        }
    }
}
