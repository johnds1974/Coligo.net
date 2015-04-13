
#if WINDOWS_PHONE_APP
using Windows.ApplicationModel;
#else
using System.ComponentModel;
using System.Windows;
#endif

namespace Coligo.Platform
{
    public static class DesignerHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool IsDesignMode
        {
            get
            {
                bool inDesignMode = false;

#if WINDOWS_PHONE_APP
                    inDesignMode = DesignMode.DesignModeEnabled;
#elif SILVERLIGHT
                    inDesignMode = DesignerProperties.IsInDesignTool;
#else
                var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                inDesignMode = (bool)descriptor.Metadata.DefaultValue;
#endif
                
                return inDesignMode;

            }
        }
    }
}