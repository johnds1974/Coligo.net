using Coligo.Core;
using Coligo.Platform;
using Coligo.Platform.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Coligo.Platform.Converters;
using System.Reflection;
#else
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Coligo.Platform.Converters;
#endif


namespace Coligo.Platform.Binder
{

    /// <summary>
    /// 
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static FrameworkElement GetViewForModel(Type modelType)
        {
            if (modelType != null)
            {
                // Get the unqualified name...
                var modelTypeName = modelType.Name.Replace(modelType.Namespace, string.Empty).Replace(".",string.Empty);

                modelTypeName = modelTypeName.Replace("ViewModel", "View");

                Type viewType = null;

#if WINDOWS_PHONE_APP
                TypeInfo typeInfo = modelType.GetTypeInfo();
                
                var viewTypeInfo = typeInfo.Assembly.DefinedTypes.FirstOrDefault(t => t.Name.EndsWith(modelTypeName));
                if (viewTypeInfo != null)
                    viewType = viewTypeInfo.AsType();
#else
                viewType = modelType.Assembly.GetTypes().FirstOrDefault(t => t.Name.EndsWith(modelTypeName));
#endif

                if (viewType != null)
                {
#if WINDOWS_PHONE_APP
                    // Create an instance of the view...
                    object view = Activator.CreateInstance(viewType);
#else
                    // Create an instance of the view...
                    object view = Activator.CreateInstance(viewType);
#endif

                    return (FrameworkElement)view;
                }

            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        public static Type GetViewTypeForModel(Type viewModelType)
        {
            Type viewType = null;

            if (viewModelType != null)
            {
                // Get the unqualified name...
                var modelTypeName = viewModelType.Name.Replace(viewModelType.Namespace, string.Empty).Replace(".", string.Empty);

                modelTypeName = modelTypeName.Replace("ViewModel", "View");

#if WINDOWS_PHONE_APP
                TypeInfo typeInfo = viewModelType.GetTypeInfo();

                var viewTypeInfo = typeInfo.Assembly.DefinedTypes.FirstOrDefault(t => t.Name.EndsWith(modelTypeName));
                if (viewTypeInfo != null)
                    viewType = viewTypeInfo.AsType();
#else
                viewType = viewModelType.Assembly.GetTypes().FirstOrDefault(t => t.Name.EndsWith(modelTypeName));
#endif
            }

            return viewType;
        }
    }


}