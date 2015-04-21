using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using HtmlAgilityPack;

namespace TungHoanhReader.Wrapper
{
    public class TruyenConvertWrapper : IWraper
    {


        public TruyenConvertWrapper()
        {

            NumberChapterPerPage = 2;
        }
        public string HOME_PAGE_TOP = "http://truyencv.com/danh-sach/moi-cap-nhat/trang-{0}/";
        public string HOME_PAGE_THE_LOAI_TOP = "http://truyencv.com/{0}/trang-{1}/";
        public string TIM_KIEM_THE_URL = "http://truyencv.com/tim-kiem/all/{0}/trang-{1}/";
        public string DANHSACHTOPTRUYEN_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/ul[1]/li";
        public string MAXPAGEINDEX_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/div[1]/div[2]/a";


        public async Task<SiteTruyenData> GetTopTruyen(int pageIndex = 1, TagTruyen tag = TagTruyen.Default, string order = "")
        {
            var siteUrl = string.Format(HOME_PAGE_TOP, pageIndex);
            if (tag != TagTruyen.Default)
            {
                siteUrl = string.Format(HOME_PAGE_THE_LOAI_TOP, tag.ToTruyenConvertString(), pageIndex);
            }
            var htmlData = await WebUtils.DoRequestSimpleGet(siteUrl);
            if (htmlData.Status)
            {
                var siteTruyen = ReadSiteTruyenHtmlData(htmlData.Data);
                return siteTruyen;
            }
            return null;
        }

        private SiteTruyenData ReadSiteTruyenHtmlData(string htmlData)
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
                    if (i < 2) continue;
                    var truyen = new Truyen(SiteTruyen.TruyenConvert);
                    try
                    {
                        var cdom = idom.SelectSingleNode("./div[1]/h2[1]/a[1]");
                        if (cdom != null) truyen.Title = cdom.InnerText.Trim();
                        else continue;
                        cdom = idom.SelectSingleNode("./div[2]/a[1]/a[1]");
                        if (cdom != null) truyen.Author = cdom.InnerText.Trim();
                        cdom = idom.SelectSingleNode("./div[3]");
                        if (cdom != null) truyen.NumberChaper = int.Parse(cdom.InnerText.Trim().Replace("Mới nhất: Chương ", ""));
                        truyen.NumberView = 0;
                        truyen.Category = "";
                        cdom = idom.SelectSingleNode("./div[1]/h2[1]/a[1]");
                        if (cdom != null)
                            truyen.TruyenUrl = cdom.GetAttributeValue("href", "");
                        else continue;

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
                            href = href.Substring(href.LastIndexOf('-') + 1, href.Length - href.LastIndexOf('-') - 2);
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

        public async Task<SiteTruyenData> SearchTruyen(string query, int pageindex)
        {
            query = RemoveSign4VietnameseString(query);
            var siteUrl = string.Format(TIM_KIEM_THE_URL, query, pageindex);
            var htmlData = await WebUtils.DoRequestSimpleGet(siteUrl);
            if (htmlData.Status)
            {
                var siteTruyen = ReadSiteTruyenHtmlData(htmlData.Data);
                return siteTruyen;
            }
            return null;
        }


        public string CATEGORY_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/span[2]/a[1]";
        public string MOTA_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[3]";
        public string CHUONG_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[6]/ul[1]/div[1]/li";
        public string CHUONGMOINHAT_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div";
        public string TENCHUONG_XPATH = @"./div[1]/a[1]";
        public async Task<ChapterLoadData> GetPageChapTer(Truyen truyen, int page, bool allowGetNoidung = true)
        {
            if (allowGetNoidung)
            {
                return await GetChapterNumber(truyen, page);
            }
            var htmlData = await WebUtils.DoRequestSimpleGet(truyen.TruyenUrl);
            if (htmlData.Status)
            {
                var dom = new HtmlDocument();
                dom.LoadHtml(htmlData.Data);
                // the loai?
                var cDom = dom.DocumentNode.SelectSingleNode(CATEGORY_XPATH);
                if (cDom != null) truyen.Category = cDom.InnerText.Trim();
                // mo ta?
                cDom = dom.DocumentNode.SelectSingleNode(MOTA_XPATH);
                if (cDom != null) truyen.Description = FixHtmlString(cDom.InnerHtml.Trim());
                var lchapter = new List<Chapter>();
                var maxPage = 0;
                var lDom = dom.DocumentNode.SelectNodes(CHUONGMOINHAT_XPATH);
                if (lDom != null)
                {
                    foreach (var iDom in lDom)
                    {
                        var chap = new Chapter();
                        chap.TruyenUrl = truyen.TruyenUrl;
                        cDom = iDom.SelectSingleNode("./a");
                        if (cDom != null)
                        {
                            var tmpString = cDom.InnerText.Trim();
                            chap.TenChuong = tmpString.Substring(tmpString.IndexOf(':') + 1,
                                tmpString.Length - tmpString.IndexOf(':') - 1).Trim();
                        }
                        else continue;
                        var href = cDom.GetAttributeValue("href", "");
                        try
                        {
                            if (href.LastIndexOf("-") != -1)
                            {
                                href = href.Substring(href.LastIndexOf("-") + 1, href.Length - href.LastIndexOf('-') - 2);
                                var num = int.Parse(href);
                                chap.SoThuTu = "Chương " + num.ToString();
                                chap.IndexNumberPageOfChapter = num;
                                chap.PageOfChapter = num;
                                if (num > maxPage) maxPage = num;
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
                        lchapter.Add(chap);
                    }
                }
                // list chuong tu ddau
                lDom = dom.DocumentNode.SelectNodes(CHUONG_XPATH);
                if (lDom != null)
                {
                    var i = 1;
                    foreach (var iDom in lDom)
                    {
                        i++;
                        var chap = new Chapter();
                        chap.TruyenUrl = truyen.TruyenUrl;
                        cDom = iDom.SelectSingleNode(TENCHUONG_XPATH);
                        if (cDom != null)
                        {
                            var tmpString = cDom.InnerText.Trim();
                            chap.TenChuong = tmpString.Substring(tmpString.IndexOf(':') + 1,
                                tmpString.Length - tmpString.IndexOf(':') - 1).Trim();
                        }
                        var href = cDom.GetAttributeValue("href", "");
                        try
                        {
                            if (href.LastIndexOf("-") != -1)
                            {
                                href = href.Substring(href.LastIndexOf("-") + 1, href.Length - href.LastIndexOf('-') - 2);
                                var num = int.Parse(href);
                                chap.SoThuTu = "Chương " + num;
                                chap.IndexNumberPageOfChapter = num;
                                chap.PageOfChapter = num;
                                if (num > maxPage) maxPage = num;
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

                        lchapter.Add(chap);
                    }
                }
                truyen.MaxPageTruyen = maxPage;
                var result = new ChapterLoadData();
                result.ListChapter = lchapter;
                result.MaxPageIndex = maxPage;
                NumberChapterPerPage = maxPage + 100;
                result.IndexStartOfChapter = 0;
                return result;
            }

            return null;
        }

        public string FixHtmlString(string inpPutString)
        {
            var temp= WebUtility.HtmlDecode(inpPutString);
            var reg = new Regex(@"<br\s*\/?>");
            temp = reg.Replace(temp.ToString(), "\r\n");
            reg = new Regex(@"<.*\s*/?>");
            temp = reg.Replace(temp, "");
            reg = new Regex(@"^(\r\n){2,}");
            return reg.Replace(temp, "\r\n");
        }


        public int NumberChapterPerPage { get; set; }


        public string TRUYEN_CONTENT_XPATH =
            @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]";

        public string TRUYEN_CONTENT_TIEUDE_XPATH =
            @"/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[2]/div[1]/h2[1]";
        public async Task<ChapterLoadData> GetChapterNumber(Truyen truyen, int indexOfchapter)
        {
            var siteUrl = string.Format("{0}chuong-{1}/", truyen.TruyenUrl, indexOfchapter);
            var htmlData = await WebUtils.DoRequestSimpleGet(siteUrl);
            if (htmlData.Status)
            {
                var dom = new HtmlDocument();
                dom.LoadHtml(htmlData.Data);
                var chapter = new Chapter();
                chapter.IndexNumberPageOfChapter = indexOfchapter;
                chapter.PageOfChapter = indexOfchapter;
                var cdom = dom.DocumentNode.SelectSingleNode(TRUYEN_CONTENT_XPATH);
                if (cdom != null)
                {
                    List<HtmlNode> listRemove =
                                 cdom.ChildNodes.Where(iNode => iNode.Name == "font").ToList();
                    foreach (var iNode in listRemove)
                    {
                        cdom.RemoveChild(iNode);
                    }
                    chapter.NoiDung = FixHtmlString(cdom.InnerHtml.ToString());
                }
                else return null;
                cdom = dom.DocumentNode.SelectSingleNode(TRUYEN_CONTENT_TIEUDE_XPATH);
                if (cdom != null)
                {
                    var tmpString = cdom.InnerText.Trim();
                    chapter.TenChuong = tmpString.Substring(tmpString.IndexOf(':') + 1,
                        tmpString.Length - tmpString.IndexOf(':') - 1).Trim();
                }
                chapter.SoThuTu = "Chương " + indexOfchapter;
                chapter.IndexChapter = indexOfchapter;
                var lchapter = new List<Chapter>();
                lchapter.Add(chapter);
                var result = new ChapterLoadData();
                result.ListChapter = lchapter;
                result.MaxPageIndex = truyen.MaxPageTruyen;
                result.IndexStartOfChapter = indexOfchapter;
                return result;
            }
            return null;

        }


        private readonly string[] VietnameseSigns = new string[]
    {

        "aAeEoOuUiIdDyY",

        "áàạảãâấầậẩẫăắằặẳẵ",

        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

        "éèẹẻẽêếềệểễ",

        "ÉÈẸẺẼÊẾỀỆỂỄ",

        "óòọỏõôốồộổỗơớờợởỡ",

        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

        "úùụủũưứừựửữ",

        "ÚÙỤỦŨƯỨỪỰỬỮ",

        "íìịỉĩ",

        "ÍÌỊỈĨ",

        "đ",

        "Đ",

        "ýỳỵỷỹ",

        "ÝỲỴỶỸ"

    };



        private string RemoveSign4VietnameseString(string str)
        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;

        }
    }
}