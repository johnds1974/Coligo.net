using Coligo.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Coligo.Core;
using Coligo.Platform.Extensions;
using System.Diagnostics;
#else
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Coligo.Core;
using Coligo.Platform.Extensions;
using System.Diagnostics;
#endif


namespace Coligo.Platform.Binder
{
    public static class Binder
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.RegisterAttached(
          "Model",
          typeof(string),
          typeof(Binder),
          new PropertyMetadata(string.Empty, ModelPropertyChanged)
        );

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BindProperty = DependencyProperty.RegisterAttached(
          "Bind",
          typeof(string),
          typeof(Binder),
          new PropertyMetadata(string.Empty, BindPropertyChanged)
        );

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.RegisterAttached(
          "Action",
          typeof(string),
          typeof(Binder),
          new PropertyMetadata(string.Empty, ActionPropertyChanged)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="statement"></param>
        private static void ValidateDesignTime(DependencyObject d, string statement)
        {
            if (d is FrameworkElement && !string.IsNullOrEmpty(statement))
            {
                // if our statement has a '$this', then our FrameworkElement needs to have a Name...
                if (statement.Equals("$this", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrEmpty(((FrameworkElement)d).Name))
                {
                    throw new Exception(String.Format("Name is required to use '$this' on {0}", d));
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerHelper.IsDesignMode)
            {
//                throw new Exception("yo!");
                return;
            }

            var viewModelName = e.NewValue as string;

            if (d is FrameworkElement)
            {
                var element = d as FrameworkElement;

//                Debug.WriteLine("==>> Bind.Model: {0} '{1}'", element, viewModelName);

                var loaded = element.IsLoaded();

                // Ask the IocContainer to give us an instance of the type we need...
                var obj = ColigoEngine.Container.GetInstance(viewModelName);

                BinderHelper.BindModel(element, obj);

/*                
                if (!loaded)
                {
                    element.Loaded += (sender, args) =>
                    {
                        // Ask the IocContainer to give us an instance of the type we need...
                        var obj = ColigoEngine.Container.GetInstance(viewModelName);

                        BinderHelper.BindModel(sender as FrameworkElement, obj);
                    };
                }
                else
                {
                    // Ask the IocContainer to give us an instance of the type we need...
                    var obj = ColigoEngine.Container.GetInstance(viewModelName);

                    BinderHelper.BindModel(element, obj);
                }
*/
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void BindPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var statement = e.NewValue as string;

            if (DesignerHelper.IsDesignMode)
            {
                ValidateDesignTime(d, statement);
                return;
            }

            if (d is FrameworkElement)
            {

//                Debug.WriteLine("==>> BindPropertyChanged() - {0} '{1}'", d, statement);

                BinderHelper.BindElement(d as FrameworkElement, statement);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ActionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerHelper.IsDesignMode)
            {
                return;
            }

            var statement = e.NewValue as string;

            BinderHelper.BindAction(d, statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetModel(DependencyObject element, string value)
        {
            element.SetValue(ModelProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetModel(DependencyObject element)
        {
            return (string)element.GetValue(ModelProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetBind(DependencyObject element, string value)
        {
            element.SetValue(BindProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetBind(DependencyObject element)
        {
            return (string)element.GetValue(BindProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetAction(DependencyObject element, string value)
        {
            element.SetValue(ActionProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetAction(DependencyObject element)
        {
            return (string)element.GetValue(ActionProperty);
        }

    }
}
