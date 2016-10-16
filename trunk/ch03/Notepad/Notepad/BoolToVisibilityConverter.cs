using System;
using System.Windows;
using System.Windows.Data;

namespace Notepad
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue;

            // try to parse and see if the value is boolean if not
            // return Collapsed
            if (bool.TryParse(value.ToString(), out boolValue))
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                // By default it will always return Visibility.Collapsed
                // even for the case where the value is not bool
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibilityValue = Visibility.Collapsed;

            // if the value is not of type of Visibility enumeration 
            // Visibility will be set to Collapsed
            try
            {
                visibilityValue = (Visibility)Enum.Parse(typeof(Visibility), (string)value, true);
                return visibilityValue;
            }
            catch (Exception)
            {
                // if fails to conver the value to Visibility
                // it will return Collapsed as default value
                return visibilityValue;
            }
            
        }
    }
}
