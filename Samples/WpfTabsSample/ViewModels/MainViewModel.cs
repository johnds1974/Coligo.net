using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coligo.Platform.Binder;
using Coligo.Core;
using Coligo.Platform;

namespace WpfTabsSample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            _tabs = new List<Type>();

            _tabs.Add(typeof(Tab1ViewModel));
            _tabs.Add(typeof(Tab2ViewModel));
        }

        public string Name
        {
            get { return "John"; }
        }

        private IList<Type> _tabs;
        public IEnumerable<Type> Tabs
        {
            get { return _tabs; }
        }
    }
}
