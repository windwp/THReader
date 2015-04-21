using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    public class TungHoanhWrapper : TruyenConvertWrapper
    {

        public TungHoanhWrapper()
        {
            CATEGORY_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/span[2]/a[1]";
            MOTA_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[3]";
            CHUONG_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[6]/ul[1]/div[1]/li";
            CHUONGMOINHAT_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div";
            TENCHUONG_XPATH = @"./div[1]/a[1]";
            TRUYEN_CONTENT_XPATH =
        @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]";

            TRUYEN_CONTENT_TIEUDE_XPATH =
                @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/h2[1]";

            HOME_PAGE_TOP = "http://tunghoanh.com/danh-sach/moi-cap-nhat/trang-{0}/";
            HOME_PAGE_THE_LOAI_TOP = "http://tunghoanh.com/{0}/trang-{1}/";
            TIM_KIEM_THE_URL = "http://tunghoanh.com/tim-kiem/all/{0}/trang-{1}/";
            DANHSACHTOPTRUYEN_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/ul[1]/li";
            MAXPAGEINDEX_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/a";

        }
    }
}
