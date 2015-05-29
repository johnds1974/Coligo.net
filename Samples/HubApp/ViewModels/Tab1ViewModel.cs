using System.Collections.ObjectModel;
using Coligo.Core;
using Coligo.Platform;

namespace HubApp.ViewModels
{
    public class Tab1ViewModel : BaseViewModel
    {
        private ObservableCollection<string> _items;

        public Tab1ViewModel()
        {
            Items = new ObservableCollection<string>
                {
                    "item 1",
                    "item 2",
                    "item 3"
                };
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return "Tab 1"; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _postCode;

        private bool _canCheckPostCode;

        public string PostCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                _postCode = value;
                OnPropertyChanged();
                OnPropertyChanged(() => CanCheckPostCode);
            }
        }

        public bool CanCheckPostCode
        {
            get
            {
                return !string.IsNullOrEmpty(_postCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> Items
        {
            get { return _items; }
            private set { 
                _items = value;
                OnPropertyChanged();
            }
        }

        public void CheckPostCode()
        {
            
        }

        public void ItemAction()
        {

        }
    }
}