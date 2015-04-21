using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    public interface IWraper
    {
        Task<SiteTruyenData> GetTopTruyen(int pageIndex = 1, TagTruyen tag = TagTruyen.Default, string order = "");
        Task<SiteTruyenData> SearchTruyen(string query, int pageindex);
        Task<ChapterLoadData> GetPageChapTer(Truyen truyen, int page, bool allowGetNoidung = true);
        int NumberChapterPerPage { get; set; }
        Task<ChapterLoadData> GetChapterNumber(Truyen truyen, int indexOfchapter);
    }
}


