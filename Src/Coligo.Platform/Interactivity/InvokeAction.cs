using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
#else
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Windows.Interactivity;
#endif

using Coligo.Platform.Extensions;

namespace Coligo.Platform.Interactivity
{


    /// <summary>
    /// 
    /// </summary>
    class InvokeAction : 
#if WINDOWS_PHONE_APP
        DependencyObject, IAction
#else
        TriggerAction<DependencyObject>
#endif
    {
        string _actionName;
        Action<object, object, object> _actionDelegate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionname"></param>
        public InvokeAction(string actionname)
        {
            if (string.IsNullOrEmpty(actionname))
                throw new ArgumentException("You must provide an action-name.", "actionname");

            _actionName = actionname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDelegate"></param>
        public InvokeAction(Action<object, object, object> actionDelegate)
        {
            _actionDelegate = actionDelegate;
        }


        /// <summary>
        /// 
        /// </summary>
        public string ActionName
        {
            get
            {
                return _actionName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object Execute(object sender, object parameter)
        {
            object vm = null;

            if (sender is FrameworkElement)
            {
                vm = ((FrameworkElement)sender).DataContext;
            }
#if !WINDOWS_PHONE_APP
            else if (sender is FrameworkContentElement)
            {
                vm = ((FrameworkContentElement)sender).DataContext;
            }
#endif

            if (vm != null)
            {
                // Get the 'action' on the ViewModel...
                if (!string.IsNullOrEmpty(_actionName))
                {
                    var mi = vm.GetType().GetMethodInfo(_actionName);

                    if (mi != null)
                    {
                        // Call the 'action' (method)...
                        mi.Invoke(vm, null);
                    }
                }
                else if (_actionDelegate != null)
                {
                    _actionDelegate(sender, parameter, vm);
                }
            }

            return null;
        }

#if !WINDOWS_PHONE_APP
        /// <summary>
        /// This is where we invoke the 'action' (Method) on the AssociatedObject (ViewModel)
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null)
            {
                Execute(AssociatedObject, parameter);
            }
        }
#endif
    }

}
