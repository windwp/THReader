using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace TungHoanhReader.Converter
{
    class SubConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            return System.Convert.ToDouble(value) - System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
