using System.Reflection;
using System.Linq;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
#endif

namespace Coligo.Platform.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsLoaded(this FrameworkElement element)
        {
            if (element != null)
            {
#if WINDOWS_PHONE_APP
                return element.Parent != null ||
                    VisualTreeHelper.GetParent(element) != null ||
                    Window.Current.Content == element;
#else

                return element.IsLoaded;
#endif
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="propname"></param>
        /// <returns></returns>
        public static DependencyProperty GetDependencyProperty(this FrameworkElement element, string propname)
        {
            if (element != null && !string.IsNullOrEmpty(propname))
            {
                var origname = propname;
                propname += "Property";

#if WINDOWS_PHONE_APP

                var ti = element.GetType().GetTypeInfo();
                while (true)
                {
                    var dp = ti.DeclaredProperties.FirstOrDefault(p => p.Name == propname);
                    if (dp != null)
                        return dp.GetValue(element) as DependencyProperty;

                    if (ti.BaseType == null)
                        break;

                    ti = ti.BaseType.GetTypeInfo();
                }
                

                //var f = element.GetType().GetRuntimeFields().Where(ff => ff.Name.Contains(origname)).ToList();
                //var m = element.GetType().GetRuntimeMethods().Where(mm => mm.Name.Contains(origname)).ToList();
                //var p = element.GetType().GetRuntimeProperties().Where(pp => pp.Name.Contains(origname)).ToList();

                var prop = element.GetType().GetRuntimeProperty(propname);
                if (prop != null)
                {
                    var value = prop.GetValue(element) as DependencyProperty;
                    return value;
                }
                
#else
                FieldInfo field = element.GetType().GetField(propname, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (field != null)
                {
                    var value = field.GetValue(element) as DependencyProperty;

                    return value;
                }
#endif
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="eventname"></param>
        /// <returns></returns>
        public static EventInfo GetEvent(this FrameworkElement element, string eventname)
        {
            if (element != null && !string.IsNullOrEmpty(eventname))
            {
                eventname += "Changed";

#if WINDOWS_PHONE_APP
                var evts = element.GetType().GetRuntimeEvents();
                var ev = evts.FirstOrDefault(e => e.Name == eventname);
#else
                var ev = element.GetType().GetEvent(eventname);
#endif
                if (ev != null)
                {
                    return ev;
                }
            }

            return null;
        }

    }

}