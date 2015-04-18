﻿using System;
using Windows.UI.Xaml.Data;

namespace TungHoanhReader.Converters
{
    class InverseBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
