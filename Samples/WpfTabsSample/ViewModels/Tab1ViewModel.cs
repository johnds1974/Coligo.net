using System.Collections.ObjectModel;
using Coligo.Core;
using Coligo.Platform;

namespace WpfTabsSample.ViewModels
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
            get { return _postCode; }
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
            //set { 
            //    _canCheckPostCode = value; 
            //    OnPropertyChanged();
            //    OnPropertyChanged(() => IsVisibleCheckPostCode);
            //}
        }

        public bool IsVisibleCheckPostCode
        {
            get 
            { 
                return true; 
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

        string _selectedItem;
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        string _selectedTreeItem;
        public string SelectedTreeItem
        {
            get
            {
                return _selectedTreeItem;
            }
            set
            {
                _selectedTreeItem = value;
                OnPropertyChanged();
            }
        }


        public void CheckPostCode()
        {
            
        }

        public void AddItemAction()
        {
            Items.Add("item " + Items.Count + 1);
        }

        public void HelpAction()
        {

        }
    }
}