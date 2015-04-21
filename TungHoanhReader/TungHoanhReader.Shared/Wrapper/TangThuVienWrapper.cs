using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    public class TangThuVienWrapper:IWraper
    {
        public Task<SiteTruyenData> GetTopTruyen(int pageIndex = 1, TagTruyen tag = TagTruyen.Default, string order = "")
        {
            throw new NotImplementedException();
        }

        public Task<SiteTruyenData> SearchTruyen(string query, int pageindex)
        {
            throw new NotImplementedException();
        }

        public Task<ChapterLoadData> GetPageChapTer(Truyen truyen, int page, bool allowGetNoidung = true)
        {
            throw new NotImplementedException();
        }

        public int NumberChapterPerPage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Task<ChapterLoadData> GetChapterNumber(Truyen truyen, int indexOfchapter)
        {
            throw new NotImplementedException();
        }
    }
}
