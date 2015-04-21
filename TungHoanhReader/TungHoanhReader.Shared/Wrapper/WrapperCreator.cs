using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TungHoanhReader.Wrapper
{
    public class WrapperCreator
    {

        public static IWraper Create(SiteTruyen type)
        {
            switch (type)
            {
                case SiteTruyen.TangThuVien:
                    return new TangThuVienWrapper();
                case SiteTruyen.TungHoanh:
                    return new TungHoanhWrapper();
                case SiteTruyen.WebTruyen:
                    return new WebTruyenWrapper();
                case SiteTruyen.TruyenConvert:
                    return new TruyenConvertWrapper();
                default:
                    return new LuongSonBacWrapper();
            }
        }
    }
}

