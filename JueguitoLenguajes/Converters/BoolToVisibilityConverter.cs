using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JueguitoLenguajes.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                bool inverted = parameter?.ToString() == "Invert";
                if (inverted) boolValue = !boolValue;
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                bool inverted = parameter?.ToString() == "Invert";
                return inverted ? !result : result;
            }
            return false;
        }
    }
}
