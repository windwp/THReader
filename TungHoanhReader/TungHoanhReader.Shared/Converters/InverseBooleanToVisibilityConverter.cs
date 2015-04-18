using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TungHoanhReader.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return ((bool) value) ? Visibility.Collapsed : Visibility.Visible;
            }

            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
