using System;
using System.Linq;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Reflection;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Reflection;
#endif

namespace Coligo.Platform.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsSubType(this Type element, Type baseType)
        {
            if (element != null && baseType != null)
            {
#if WINDOWS_PHONE_APP
//                TypeInfo ti = element.GetTypeInfo();
//                return ti != null && ti.IsSubclassOf(baseType);

                TypeInfo ti = baseType.GetTypeInfo();
                return ti.IsAssignableFrom(element.GetTypeInfo());
#else
                return baseType.IsAssignableFrom(element);
//                return element.IsSubclassOf(baseType);
#endif
            }

            return false;
        }

        public static PropertyInfo GetProperty(this Type element, string name)
        {
            PropertyInfo pi = null;

            if (element != null && !string.IsNullOrEmpty(name))
            {
#if WINDOWS_PHONE_APP
                pi = element.GetRuntimeProperty(name);
#else
                pi = element.GetProperty(name);
#endif
            }

            return pi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="methodname"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(this Type element, string methodname)
        {
            MethodInfo mi = null;

            if (element != null && !string.IsNullOrEmpty(methodname))
            {
#if WINDOWS_PHONE_APP
                mi = element.GetRuntimeMethods().FirstOrDefault(m => m.Name == methodname);
#else
                mi = element.GetMethod(methodname);
#endif
            }

            return mi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="eventname"></param>
        /// <returns></returns>
        public static EventInfo GetEventInfo(this Type element, string eventname)
        {
            EventInfo ei = null;

            if (element != null && !string.IsNullOrEmpty(eventname))
            {
#if WINDOWS_PHONE_APP
                ei = element.GetRuntimeEvent(eventname);
#else
                ei = element.GetEvent(eventname);
#endif
            }

            return ei;
        }

    }

}