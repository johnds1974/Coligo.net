using System;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
using System.Windows.Data;
#endif

namespace Coligo.Platform.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolToVisibilityInverter : IValueConverter
    {

#if WINDOWS_PHONE_APP
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

#else

        /// <summary>
        /// Converts 'true' to Visibility.Visible otherwise Visibility.Collapsed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result=false;

            if (value is bool)
            {
                result = (bool)value;
            }
            else if (value is string)
            {
                bool.TryParse((string)value, out result);
            }

            return result ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Converts Visibility.Visible to 'true' otherwise 'false'.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false;

            if (value is Visibility)
            {
                result = (Visibility)value == Visibility.Collapsed;
            }
            else if (value is string)
            {
                Visibility parsed;
                Enum.TryParse<Visibility>((string)value, false, out parsed);
                result = parsed == Visibility.Collapsed;
            }

            return result;
        }

#endif

    }
}