using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader
{
    public class LuongSonBacWrapper
    {
        public static string HOME_PAGE = "http://www.lsb-thuquan.eu/index.php";
        private string _cookie;
        private static string Home_Top_XPath;
        public static string DANHSACHTOPTRUYEN_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[1]/div[2]/table[1]/tr[1]/td[1]/table[1]/tr[1]/td[1]/table[1]//tr";
        public static string HOME_PAGE_TOP = "http://www.lsb-thuquan.eu/index.php?page={0}&ipp=35";


        public static string TRUYEN_URL = "http://www.lsb-thuquan.eu/{0}?page={1}&ipp=10";

        public async Task<List<Truyen>> GetTopTruyen(int pageIndex = 1, TagTruyen tag = TagTruyen.Default, string order = "")
        {
            var url = string.Format(HOME_PAGE_TOP, pageIndex);
            if (tag!=TagTruyen.Default)
            {
                url += string.Format("&do=tag&tag={0}", tag.ToLsbString());
            }
            if (!string.IsNullOrEmpty(order))
            {
                url += string.Format("&order=");
            }
            var htmlData =await WebUtils.DoRequestSimpleGet(url, null, "", HOME_PAGE);
            if (htmlData.Status)
            {
                var dom = new HtmlDocument();
                dom.LoadHtml(htmlData.Data);
                var tdom = dom.DocumentNode.SelectNodes(DANHSACHTOPTRUYEN_XPATH);
                if (tdom != null)
                {
                    var i = 0;
                    var result = new List<Truyen>();
                    foreach (var idom in tdom)
                    {
                        i++;
                        // bo? 2 node dau`
                        if (i < 3) continue;
                        var truyen = new Truyen();
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
                                truyen.ChapterUrl = cdom.GetAttributeValue("href", "");
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
                        result.Add(truyen);
                    }

                    return result;
                }
            }
            return null;
        }


        public string DANH_SACH_CHUONG_XPATH = @"/html[1]/body[1]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]/div";


        public static string CHUONGNUMBER_XPATH = @".//font[@color='DarkRed']";
        public static string TENCHUONG_XPATH = @".//font[@color='Red']";
        public static string SOURCE_XPATH = @".//font[@color='Navy']";
        public static string NOIDUNG_XPATH = @".//div[@class='maincontent']";
        public List<Chapter> GetPageChapTer(Truyen truyen, int page)
        {
            var pageUrl = string.Format(TRUYEN_URL, truyen.ChapterUrl, page);
            var htmlData = Task.Run(() => WebUtils.DoRequestSimpleGet(pageUrl, null, "", HOME_PAGE)).Result;
            if (htmlData.Status)
            {
                var dom = new HtmlDocument();
                dom.LoadHtml(htmlData.Data);
                var chapterDoms = dom.DocumentNode.SelectNodes(DANH_SACH_CHUONG_XPATH);

                if (chapterDoms != null)
                {
                    var i = 0;
                    var lchapter = new List<Chapter>();
                    foreach (var chapDom in chapterDoms)
                    {
                        i++;
                        if (i < 3) continue;
                        var chap = new Chapter();
                        HtmlNode tdom = null;
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
                        tdom = chapDom.SelectSingleNode(NOIDUNG_XPATH);
                        if (tdom != null)
                        {
                            List<HtmlNode> listRemove = tdom.ChildNodes.Where(iNode => iNode.Name == "span").ToList();
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
                        lchapter.Add(chap);
                    }
                    return lchapter;
                }

            }
            return null;
        }

    }

    public enum SiteTruyen
    {
        LuongSonBac,
        TungHoanh,
        TangThuVien,
        TruyenConver
    }
    public enum TagTruyen
    {
        [LSBStringValue("Tất cả")]
        [LSBHienThiStringValue("Tất cả")]
        Default,
        [LSBStringValue("kiếm+hiệp")]
        [LSBHienThiStringValue("Kiếm hiệp")]
        KiemHiep,

        [LSBStringValue("kiếm+hiệp+việt+nam")]
        [LSBHienThiStringValue("Kiếm Hiệp Hiệt Nam")]
        KiemHiepVN,

        [LSBStringValue("tiên+hiệp")]
        [LSBHienThiStringValue("Tiên Hiệp")]
        TienHiep,

        [LSBStringValue("huyền+ảo")]
        [LSBHienThiStringValue("Huyền Ảo")]
        HuyenAo,
        [LSBStringValue("sắc+hiệp")]
        [LSBHienThiStringValue("Sắc Hiệp")]
        SacHiep,
        [LSBStringValue("dị+hiệp")]
        [LSBHienThiStringValue("Dị Hiệp")]
        DiHiep,
        [LSBStringValue("đô+thị")]
        [LSBHienThiStringValue("Đô+Thị")]
        DoThi,
        [LSBStringValue("khoa+huyễn")]
        [LSBHienThiStringValue("Khoa Huyễn")]
        KhoaHuyen,

        [LSBStringValue("võng+du")]
        [LSBHienThiStringValue("Võng Du")]
        VongDu,
        [LSBStringValue("truyện+sáng+tác")]
        [LSBHienThiStringValue("Truyện Sáng Tác")]
        TruyenSangTac,
        [LSBStringValue("ngôn+tình")]
        [LSBHienThiStringValue("Ngôn Tình")]
        NgonTinh,
        [LSBStringValue("trinh+thám")]
        [LSBHienThiStringValue("Trinh Thám")]
        TrinhTham,
        [LSBStringValue("kinh+dị")]
        [LSBHienThiStringValue("Kinh Dị")]
        KinhDi,
        [LSBStringValue("chiến+tranh")]
        [LSBHienThiStringValue("Chiến Tranh")]
        ChienTranh,
        [LSBStringValue("dã+sử+việt+nam")]
        [LSBHienThiStringValue("Dã Sử Việt Nam")]
        DaSuVietNam,
        [LSBStringValue("dã+sử+trung+quốc")]
        [LSBHienThiStringValue("Dã Sử Trung Quốc")]
        DaSuTrungQuoc,
        [LSBStringValue("lịch+sử+việt+nam")]
        [LSBHienThiStringValue("Lịch Sử Việt Nam")]
        LichSuVietNam,
        [LSBStringValue("truyện+dài")]
        [LSBHienThiStringValue("Truyện Dài")]
        TruyenDai,
        [LSBStringValue("thể+loại+khác")]
        [LSBHienThiStringValue("Thể Loại Khác")]
        TheLoaiKhac,
    }
    public class LSBHienThiStringValue : System.Attribute
    {
        private string _value;

        public LSBHienThiStringValue(string value)
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
    public static class Utils
    {

        public static string ToLsbString(this TagTruyen enumerationValue)
        {
            Type type = enumerationValue.GetType();

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
           var memberInfo = type.GetRuntimeFields();
            if (memberInfo != null && memberInfo.First(o=>o.Name==enumerationValue.ToString())!=null)
            {
                var attr = memberInfo.First(o => o.Name == enumerationValue.ToString()).GetCustomAttribute(typeof(LSBStringValue), false) as LSBStringValue;
                if (attr != null) return attr.Value;
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        public static string ToLsbHienThiString(this TagTruyen enumerationValue)
        {
            Type type = enumerationValue.GetType();

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetRuntimeFields();
            if (memberInfo != null && memberInfo.First(o => o.Name == enumerationValue.ToString()) != null)
            {
                var attr = memberInfo.First(o => o.Name == enumerationValue.ToString()).GetCustomAttribute(typeof(LSBHienThiStringValue), false) as LSBHienThiStringValue;
                if (attr != null) return attr.Value;
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        /// <summary>
        /// Extension method for xpath support selectNodes with simple xpath query on ly support xpat with / and xpath [number]
        /// example xpath /html/body/div[2]/div[2]/di
        /// </summary>
        /// <param name="dom"></param>
        /// <param name="xpathquery"></param>
        /// <returns></returns>
        public static List<HtmlNode> SelectNodes(this HtmlNode dom, string xpathquery)
        {
            if (xpathquery.Contains(":"))
            {
                throw new InvalidDataException("This method don't support this query");
            }
            HtmlNode currentNode = dom;
            if (xpathquery.StartsWith("/"))
            {
                xpathquery = xpathquery.Substring(1, xpathquery.Length - 1);
                currentNode = currentNode.OwnerDocument.DocumentNode;
            }
            else if (xpathquery.StartsWith("./"))
            {
                xpathquery = xpathquery.Substring(2, xpathquery.Length - 2);
            }
            var listQuery = xpathquery.Split('/');
            List<HtmlNode> listResultNode = null;
            for (int i = 0; i < listQuery.Length; i++)
            {
                listResultNode = null;
                var currentQuery = listQuery[i];
                if (string.IsNullOrEmpty(currentQuery))
                {
                    // process // query for one node only 
                    // find first child node for next node
                    //lay node ke tiep ra tim kiem
                    var nextquery = listQuery[++i];
                    foreach (var iNode in currentNode.Descendants())
                    {
                        var tNode = iNode.SelectNodes(nextquery);
                        listResultNode = tNode;
                        if (tNode != null && tNode.Count >= 1)
                        {
                            currentNode = tNode[0];
                            break;
                        }
                    }
                    if (currentNode == null) return null;

                }
                else if (currentQuery.Contains("[@"))
                {
                    currentQuery = currentQuery.Replace("\"", "'");
                    var nodeNameFound = currentQuery.Substring(0, currentQuery.IndexOf("["));
                    var atributeName = currentQuery.Substring(currentQuery.IndexOf("@") + 1, currentQuery.IndexOf("=") - currentQuery.IndexOf("@") - 1);
                    var atributeValue = currentQuery.Substring(currentQuery.IndexOf("'") + 1, currentQuery.LastIndexOf("'") - currentQuery.IndexOf("'") - 1);
                    foreach (var iNode in currentNode.Descendants(nodeNameFound))
                    {
                        if (iNode.GetAttributeValue(atributeName, "") != null &&
                            iNode.GetAttributeValue(atributeName, "") == atributeValue)
                        {
                            currentNode = iNode;
                            break;
                        }
                    }
                    if (currentNode == null || currentNode.Name != nodeNameFound) return null;
                    if (i >= listQuery.Length - 1)
                    {
                        var result = new List<HtmlNode>();
                        result.Add(currentNode);
                        return result;
                    }
                }
                else if (currentQuery.Contains("["))
                {
                    //neu' k0 co' node tra? ve null;

                    // lay' thu' tu. cua? node
                    var numberStr = currentQuery.Substring(currentQuery.IndexOf("[") + 1, currentQuery.IndexOf("]") - currentQuery.IndexOf("[") - 1);
                    var number = 0;
                    int.TryParse(numberStr, out number);
                    currentQuery = currentQuery.Substring(0, currentQuery.IndexOf("["));

                    if (dom.ChildNodes.Where(o => o.Name == currentQuery) == null) return null;

                    var listNode = currentNode.ChildNodes.Where(o => o.Name == currentQuery);
                    var htmlNodes = listNode.ToList();
                    if (htmlNodes.Count() > number - 1)
                    {
                        currentNode = htmlNodes[number - 1];
                    }
                    else
                    {
                        return null;
                    }
                    if (i >= listQuery.Length - 1)
                    {
                        var result = new List<HtmlNode>();
                        result.Add(currentNode);
                        return result;
                    }
                }
                else
                {
                    if (dom.ChildNodes.Where(o => o.Name == currentQuery) == null) return null;
                    if (i >= listQuery.Length - 1)
                    {
                        return currentNode.ChildNodes.Where(o => o.Name == currentQuery).ToList();
                    }
                    currentNode = dom.ChildNodes.First(o => o.Name == currentQuery);
                }
            }

            return listResultNode;
        }
        /// <summary>
        /// Extension method for xpath support selectNodes with simple xpath query on ly support xpat with / and xpath [number]
        /// example xpath /html/body/div[2]/div[2]/di
        /// </summary>
        /// <param name="dom"></param>
        /// <param name="xpathquery"></param>
        /// <returns></returns>
        public static HtmlNode SelectSingleNode(this HtmlNode dom, string xpathquery)
        {
            if (xpathquery.Contains(":"))
            {
                throw new InvalidDataException("This method don't support this query");
            }

            var result = dom.SelectNodes(xpathquery);
            if (result == null || result.Count != 1) return null;
            if (result.Count == 1)
            {
                return result[0];
            }
            return null;
        }

    }

}
