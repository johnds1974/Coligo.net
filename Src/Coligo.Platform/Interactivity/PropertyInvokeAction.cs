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

#if WINDOWS_PHONE_APP

    /// <summary>
    /// 
    /// </summary>
    class PropertyInvokeAction : DependencyObject, IAction
    {
        string _propertyName;
        DependencyObject _associatedObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionname"></param>
        public PropertyInvokeAction(string propname, DependencyObject associatedObject)
        {
            if (string.IsNullOrEmpty(propname))
                throw new ArgumentException("You must provide an action-name.", "propname");

            _propertyName = propname;
            _associatedObject = associatedObject;
        }

        /// <summary>
        /// 
        /// </summary>
        public DependencyObject AssociatedObject
        {
            get
            {
                return _associatedObject;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
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
            SetTargetProperty(parameter);
            return null;
        }

#else

    /// <summary>
    /// 
    /// </summary>
    class PropertyInvokeAction : TriggerAction<DependencyObject>
    {
        string _propertyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionname"></param>
        public PropertyInvokeAction(string propname)
        {
            if (string.IsNullOrEmpty(propname))
                throw new ArgumentException("You must provide a property-name.", "propname");

            _propertyName = propname;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        /// <summary>
        /// This is where we invoke the 'action' (Method) on the AssociatedObject (ViewModel)
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            SetTargetProperty(parameter);
        }

#endif

        private void SetTargetProperty(object parameter)
        {
            if (AssociatedObject != null && AssociatedObject is FrameworkElement)
            {
#if !WINDOWS_PHONE_APP
                if (parameter is RoutedPropertyChangedEventArgs<object>)
                {
                    object value = ((RoutedPropertyChangedEventArgs<object>)parameter).NewValue;

                    // Get the ViewModel object from the FrameworkElement...
                    var vm = ((FrameworkElement)AssociatedObject).DataContext;

                    // Get the 'property' on the ViewModel...
                    var prop = vm.GetType().GetProperty(PropertyName);

                    // Does the porperty exist, and can we write to it...
                    if (prop != null && prop.CanWrite)
                    {
                        // Set the property value...
                        prop.SetValue(vm, value);
                    }
                }
#endif
            }
        }

    }


}
