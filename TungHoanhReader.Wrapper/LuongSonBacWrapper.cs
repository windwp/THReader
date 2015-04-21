using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader
{
    internal class LuongSonBacWrapper : IWraper
    {

        public LuongSonBacWrapper()
        {
            NumberChapterPerPage = 10;
        }


        public int NumberChapterPerPage { get; set; }
        public static string HOME_PAGE = "http://www.lsb-thuquan.eu/index.php";
        private string _cookie;
        private static string Home_Top_XPath;
        public static string DANHSACHTOPTRUYEN_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[1]/div[2]/table[1]/tr[1]/td[1]/table[1]/tr[1]/td[1]/table[1]//tr";

        public static string MAXPAGEINDEX_XPATH =
            @"/html[1]/body[1]/div[2]/div[2]/div[1]/div[2]/table[1]/tr[1]/td[1]/table[1]/tr[1]/td[1]/table[1]/tbody[1]/tr[38]/td[1]/table[1]/tr[1]/td[1]/table[1]/tr[1]/td";
        public static string HOME_PAGE_TOP = "http://www.lsb-thuquan.eu/index.php?page={0}&ipp=35";


        public static string TRUYEN_URL = "http://www.lsb-thuquan.eu/{0}?page={1}&ipp=10";

        public async Task<SiteTruyenData> GetTopTruyen(int pageIndex = 1, TagTruyen tag = TagTruyen.Default, string order = "")
        {
            var url = string.Format(HOME_PAGE_TOP, pageIndex);
            if (tag != TagTruyen.Default)
            {
                url += string.Format("&do=tag&tag={0}", tag.ToLsbString());
            }
            if (!string.IsNullOrEmpty(order))
            {
                url += string.Format("&order=");
            }
            var htmlData = await WebUtils.DoRequestSimpleGet(url, null, "", HOME_PAGE);
            if (htmlData.Status)
            {
                var siteTruyen = ReadSiteTruyenHtmlData(htmlData.Data);
                return siteTruyen;
            }
            return null;
        }

        public static string SEARCH_URL = "http://www.lsb-thuquan.eu/index.php/index.php?do=timkiem&page{0}&ipp=35";
        public async Task<SiteTruyenData> SearchTruyen(string query, int pageindex)
        {
            var url = string.Format(SEARCH_URL, pageindex);
            var postData = new Dictionary<string, string> { { "search_this", query } };
            var htmlData = await WebUtils.DoRequestSimplePost(url, postData, null, "", HOME_PAGE);
            if (htmlData.Status)
            {
                var siteTruyen = ReadSiteTruyenHtmlData(htmlData.Data);
                return siteTruyen;
            }
            return null;
        }

        private static SiteTruyenData ReadSiteTruyenHtmlData(string htmlData)
        {

            var siteTruyen = new SiteTruyenData();
            var dom = new HtmlDocument();
            dom.LoadHtml(htmlData);
            var tdom = dom.DocumentNode.SelectNodes(DANHSACHTOPTRUYEN_XPATH);
            if (tdom != null)
            {
                var i = 0;
                var listTruyen = new List<Truyen>();
                foreach (var idom in tdom)
                {
                    i++;
                    // bo? 2 node dau`
                    if (i < 3) continue;
                    var truyen = new Truyen(SiteTruyen.LuongSonBac);
                    try
                    {
                        var cdom = idom.SelectSingleNode("./td[2]");
                        if (cdom != null)
                        {
                            truyen.Category = WebUtility.HtmlDecode(cdom.InnerText.Trim());
                        }
                        else
                        {
                            continue;
                        }
                        cdom = idom.SelectSingleNode("./td[3]");
                        if (cdom != null) truyen.Title = cdom.InnerText.Trim();
                        cdom = idom.SelectSingleNode("./td[4]");
                        if (cdom != null) truyen.Author = cdom.InnerText.Trim();
                        cdom = idom.SelectSingleNode("./td[6]");
                        if (cdom != null) truyen.NumberChaper = int.Parse(cdom.InnerText.Trim());
                        cdom = idom.SelectSingleNode("./td[7]");
                        if (cdom != null) truyen.NumberView = int.Parse(cdom.InnerText.Trim().Replace(".", ""));
                        cdom = idom.SelectSingleNode("./td[3]/a[1]");
                        if (cdom != null)
                        {
                            truyen.TruyenUrl = cdom.GetAttributeValue("href", "");
                        }
                        else
                        {
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    listTruyen.Add(truyen);
                }

                // tim so trang
                tdom = dom.DocumentNode.SelectNodes(MAXPAGEINDEX_XPATH);
                var maxPage = 1;
                if (tdom != null)
                {
                    foreach (var idom in tdom)
                    {

                        var href = idom.GetAttributeValue("href", "");
                        if (href.LastIndexOf('-') != -1)
                        {
                            href = href.Substring(href.LastIndexOf('-', href.Length - href.LastIndexOf("-")));
                            var n = 0;
                            int.TryParse(href, out n);
                            if (n > maxPage)
                            {
                                maxPage = n;
                            }
                        }
                    }
                }
                siteTruyen.ListTruyen = listTruyen;
                siteTruyen.MaxPageIndex = maxPage;
                return siteTruyen;
            }
            return siteTruyen;
        }

        public string DANH_SACH_CHUONG_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]/div";

        public static string MAX_PAGE_TRUYEN_XPATH =
            @"/html[1]/body[1]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]/div[2]/table[1]/tr[3]/td[1]/table[1]/tr[1]/td";

        public static string CHUONGNUMBER_XPATH = @".//font[@color='DarkRed']";
        public static string TENCHUONG_XPATH = @".//font[@color='Red']";
        public static string SOURCE_XPATH = @".//font[@color='Navy']";
        public static string NOIDUNG_XPATH = @".//div[@class='maincontent']";

        public async Task<ChapterLoadData> GetPageChapTer(Truyen truyen, int page, bool allowGetNoidung = true)
        {
            var pageUrl = string.Format(TRUYEN_URL, truyen.TruyenUrl, page);
            var htmlData = await WebUtils.DoRequestSimpleGet(pageUrl, null, "", HOME_PAGE);
            if (htmlData.Status)
            {
                var dom = new HtmlDocument();
                dom.LoadHtml(htmlData.Data);
                var chapterDoms = dom.DocumentNode.SelectNodes(DANH_SACH_CHUONG_XPATH);

                var maxPage = 1;
                if (chapterDoms != null)
                {
                    var i = 0;
                    var j = 0;
                    var lchapter = new List<Chapter>();
                    foreach (var chapDom in chapterDoms)
                    {
                        i++;
                        if (i < 3) continue;
                        var chap = new Chapter();
                        HtmlNode tdom = null;
                        chap.TruyenUrl = truyen.TruyenUrl;
                        chap.PageOfChapter = page;
                        tdom = chapDom.SelectSingleNode(CHUONGNUMBER_XPATH);
                        if (tdom != null)
                        {
                            chap.SoThuTu = tdom.InnerText.Trim();
                        }
                        tdom = chapDom.SelectSingleNode(TENCHUONG_XPATH);
                        if (tdom != null)
                        {
                            chap.TenChuong = tdom.InnerText.Trim();
                        }
                        else
                        {
                            continue;
                        }
                        tdom = chapDom.SelectSingleNode(SOURCE_XPATH);
                        if (tdom != null)
                        {
                            chap.Nguon = tdom.InnerText.Trim();
                        }
                        if (allowGetNoidung)
                        {
                            tdom = chapDom.SelectSingleNode(NOIDUNG_XPATH);
                            if (tdom != null)
                            {
                                List<HtmlNode> listRemove =
                                    tdom.ChildNodes.Where(iNode => iNode.Name == "span").ToList();
                                foreach (var iNode in listRemove)
                                {
                                    tdom.RemoveChild(iNode);
                                }
                                chap.NoiDung = tdom.InnerText.Trim();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        chap.IndexNumberPageOfChapter = j++;
                        lchapter.Add(chap);
                    }

                    var ldom = dom.DocumentNode.SelectNodes(MAX_PAGE_TRUYEN_XPATH);
                    if (ldom != null)
                    {
                        foreach (var idom in ldom)
                        {
                            var cdom = idom.SelectSingleNode("./a");
                            if (cdom != null)
                            {
                                var href = cdom.GetAttributeValue("href", "");
                                if (href.IndexOf("=") != -1 && href.IndexOf("&") != -1)
                                {
                                    try
                                    {
                                        href = href.Substring(href.IndexOf("=") + 1, href.IndexOf("&") - href.IndexOf("=") - 1);
                                    }
                                    catch (Exception)
                                    {
                                        // ignored
                                    }
                                }
                                var n = 0;
                                int.TryParse(href, out n);
                                if (n > maxPage)
                                {
                                    maxPage = n;
                                }
                            }
                        }
                    }
                    var result = new ChapterLoadData();
                    result.ListChapter = lchapter;
                    result.MaxPageIndex = maxPage;
                    result.IndexStartOfChapter = 0;
                    return result;
                }

            }
            return null;
        }



        public async Task<ChapterLoadData> GetChapterNumber(Truyen truyen, int indexOfchapter)
        {
            var page = indexOfchapter / NumberChapterPerPage + 1;// trang bat' dau` la` 1
            var result = await GetPageChapTer(truyen, page, true);
            result.IndexStartOfChapter = indexOfchapter - ((page - 1) * NumberChapterPerPage);
            return result;
        }


    }

    public class SiteTruyenData
    {
        public List<Truyen> ListTruyen { get; set; }
        public int MaxPageIndex { get; set; }
    }

    public class ChapterLoadData
    {
        public List<Chapter> ListChapter { get; set; } // chapter chua' noi. dung ban da`u va` ca? cache chapter
        public int IndexStartOfChapter { get; set; } // chapter chua noi. dung khoi? dau`
        public int MaxPageIndex { get; set; }// so' trang max da~ load
    }

    public enum SiteTruyen
    {
        LuongSonBac,
        TungHoanh,
        TangThuVien,
        TruyenConvert,
        WebTruyen
    }

    public class HienThiStringValue : System.Attribute
    {
        private string _value;

        public HienThiStringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
    public class LSBStringValue : System.Attribute
    {
        private string _value;

        public LSBStringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
    public class TruyenConvertStringValue : System.Attribute
    {
        private string _value;

        public TruyenConvertStringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
}
