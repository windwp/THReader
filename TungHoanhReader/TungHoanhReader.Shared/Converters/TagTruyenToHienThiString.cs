using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.Converters
{
    class TagTruyenToHienThiString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Wrapper.TagTruyen)
            {
                //var site = parameter is SiteTruyen ? (SiteTruyen)parameter : SiteTruyen.LuongSonBac;
                var loai = value is Wrapper.TagTruyen ? (Wrapper.TagTruyen)value : Wrapper.TagTruyen.Default;

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
