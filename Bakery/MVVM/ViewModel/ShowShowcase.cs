using Bakery.Extra;
using Bakery.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    class ShowShowcase : BaseViewModel
    {
        private string _showcaseCount;
        public string ShowcaseCount
        {
            get { return _showcaseCount; }
            set
            {
                _showcaseCount = value;
                OnPropertyChanged();
            }
        }

        private List<ShowCaseFood> _showcaseList;
        public List<ShowCaseFood> ShowcaseList
        {
            get { return _showcaseList; }
            set
            {
                _showcaseList = value;

                int count = 0;
                _showcaseList.ForEach(s => count += s.Count);

                ShowcaseCount = "На витрине: " + count + " (шт.) / В меню: " + Food.Collection.Count + " (тов.)";

                OnPropertyChanged();
            }
        }

        public ShowShowcase()
        {
            AppManager.OnSearchChanged += OnSearchChanged;

            Load();
        }

        private void OnSearchChanged(object sender, EventArgs e)
        {
            Load();
        }

        private void Load()
        {
            ShowcaseList = ShowCase.Collection.FindAll(s => s.PreparedFood.Name.ToLower().Contains(AppManager.GetSearchText()));
        }
    }
}
