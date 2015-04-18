using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace TungHoanhReader.Converters
{
    class TagTruyenToHienThiString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TagTruyen)
            {
                //var site = parameter is SiteTruyen ? (SiteTruyen)parameter : SiteTruyen.LuongSonBac;
                var loai = value is TagTruyen ? (TagTruyen)value : TagTruyen.Default;

                return loai.ToLsbHienThiString();

            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
