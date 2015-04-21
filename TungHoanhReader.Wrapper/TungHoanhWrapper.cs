using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    public class TungHoanhWrapper : TruyenConvertWrapper
    {
        public static string CATEGORY_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/span[2]/a[1]";
        public static string MOTA_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[3]";
        public static string CHUONG_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[6]/ul[1]/div[1]/li";
        public static string CHUONGMOINHAT_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div";
        public static string TENCHUONG_XPATH = @"./div[1]/a[1]";
        public static string TRUYEN_CONTENT_XPATH =
    @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]";

        public static string TRUYEN_CONTENT_TIEUDE_XPATH =
            @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/h2[1]";

        public static string HOME_PAGE_TOP = "http://tunghoanh.com/danh-sach/moi-cap-nhat/trang-{0}/";
        public static string HOME_PAGE_THE_LOAI_TOP = "http://tunghoanh.com/{0}/trang-{1}/";
        public static string TIM_KIEM_THE_URL = "http://tunghoanh.com/tim-kiem/all/{0}/trang-{1}/";
        public static string DANHSACHTOPTRUYEN_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/ul[1]/li";
        public static string MAXPAGEINDEX_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/a";
        public TungHoanhWrapper()
        {

     
        }
    }
}
