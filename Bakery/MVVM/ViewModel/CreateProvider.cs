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
    class CreateProvider : BaseViewModel
    {
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

        public Command COM_Create { get; set; }

        public CreateProvider()
        {
            COM_Create = new Command(c =>
            {
                if (ProviderName == "")
                    return;
                if (Provider.Collection.Exists(s => s.Name == ProviderName))
                {
                    new MessageView("Такой поставщик уже существует");
                    return;
                }

                new MessageView($"Поставщик '{ProviderName}' успешно создан");

                new Provider() { Name = ProviderName }.Add();
                AppManager.CloseActiveWindow(new View.CreateProvider());
            });
        }
    }
}
