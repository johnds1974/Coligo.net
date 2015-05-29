using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coligo.Core;
using HubApp.Services;
using Coligo.Platform;

namespace HubApp.ViewModels
{
    public class HubPageViewModel : BaseViewModel
    {
        public HubPageViewModel(IService service, IWebService webService)
        {
            _tabs = new List<Type>();

            _tabs.Add(typeof(Tab1ViewModel));
            _tabs.Add(typeof(Tab2ViewModel));
        }

        public string Name
        {
            get
            {
                return "John";
            }
        }

        private IList<Type> _tabs;
        public IEnumerable<Type> Tabs
        {
            get
            {
                return _tabs;
            }
        }

        public void SignInOutAction()
        {

        }
    }
}
