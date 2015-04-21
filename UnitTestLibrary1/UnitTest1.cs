using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TungHoanhReader;
using TungHoanhReader.Wrapper;

namespace UnitTestLibrary1
{
    [TestClass]
    public class TestWrapperSiteTruyen
    {
        Random rand = new Random();
        
        [TestMethod]
        public void TestLSBGetTop()
        {
            var top = WrapperCreator.Create(SiteTruyen.LuongSonBac);
            var result = Task.Run(() => top.GetTopTruyen()).Result;
            Assert.AreNotEqual(result.ListTruyen.Count, 0);
            Assert.AreNotEqual(result.MaxPageIndex, 0);
            Debug.WriteLine("MaxPage" + result.MaxPageIndex);
            Debug.WriteLine("MaxPage" + result.MaxPageIndex);
            var first = result.ListTruyen[7];
            Assert.IsNotNull(first.Author);
            Assert.IsNotNull(first.Title);
            first.CurrentPageRead = 2;
            first.MaxPageTruyen = 10;
            Task.Run(() => first.LoadCurrentListChapter()).Wait();
            var chap = first.GetChapterByIndex(1);
            Assert.IsNotNull(chap, "Chapter Is Null");
            Assert.AreNotEqual(first.NumberView, 0);
            Assert.AreNotEqual(first.MaxPageTruyen, 0);
        }


        [TestMethod]
        public void TestLSBGetListChapter()
        {
            var top = WrapperCreator.Create(SiteTruyen.LuongSonBac);
            var num = rand.Next(10);
            var result = Task.Run(() => top.GetTopTruyen()).Result;
            var truyen = result.ListTruyen[num];
            
            truyen.CurrentPageRead = 2;
            truyen.MaxPageTruyen = 10;
            Task.Run(() => truyen.LoadCurrentListChapter()).Wait();
            for (int i = 0; i < 10; i++)
            {
                var chap = truyen.GetChapterByIndex(i);
                Assert.AreEqual(i,chap.IndexNumberPageOfChapter,"So chuong danh dau k0 bang nhau");

            }
        }
        [TestMethod]
        public void TestLSBGetByTag()
        {
            Assert.AreEqual(TagTruyen.DoThi.ToLsbString(), "đô+thị");
            var top = WrapperCreator.Create(SiteTruyen.LuongSonBac);
            var result = Task.Run(() => top.GetTopTruyen(1, TagTruyen.DoThi)).Result;
            foreach (var truyen in result.ListTruyen)
            {
                Assert.AreEqual(truyen.Category, "Đô Thị");
            }
        }


        [TestMethod]
        public void TestLSBSearch()
        {
            Assert.AreEqual(TagTruyen.DoThi.ToLsbString(), "đô+thị");
            var top = WrapperCreator.Create(SiteTruyen.LuongSonBac);
            var result = Task.Run(() => top.SearchTruyen("thiên", 1)).Result;
            Assert.AreNotEqual(result.ListTruyen.Count, 0);
            var first = result.ListTruyen[0];
            Assert.IsNotNull(first.Author);
            Assert.IsNotNull(first.Title);
            Debug.WriteLine("Tìm kiếm " + first.Author);
        }


        [TestMethod]
        public void TestLSBGetChapter()
        {
            var top = WrapperCreator.Create(SiteTruyen.LuongSonBac);
            var truyen = new Truyen(SiteTruyen.LuongSonBac);
            truyen.TruyenUrl = "trieu-hoan-than-binh-tac-gia-ha-nhat-dich-lanh-134355858.html";
            var result = Task.Run(() => top.GetChapterNumber(truyen, 22)).Result;
            Assert.AreEqual(result.IndexStartOfChapter,2," Chỉ số bị sai");
            var chapter = result.ListChapter[result.IndexStartOfChapter];
            Assert.AreEqual(chapter.TenChuong, "Ngươi là Độc Cô Nhai ?", "Chương bị sai");
            Debug.WriteLine("Tìm kiếm " + chapter.TenChuong);
        }





        [TestMethod]
        public void TestTruyenConvertGetTop()
        {
            var top = WrapperCreator.Create(SiteTruyen.TruyenConvert);
            var result = Task.Run(() => top.GetTopTruyen()).Result;
            Assert.AreNotEqual(result.ListTruyen.Count, 0);
            Assert.AreNotEqual(result.MaxPageIndex, 0);
            Debug.WriteLine("MaxPage" + result.MaxPageIndex);
            Debug.WriteLine("MaxPage" + result.MaxPageIndex);
            var first = result.ListTruyen[7];
            Assert.IsNotNull(first.Author);
            Assert.IsNotNull(first.Title);
            first.CurrentPageRead = 2;
            first.MaxPageTruyen = 10;
            //Task.Run(() => first.LoadCurrentListChapter()).Wait();
            //var chap = first.GetChapterByIndex(1);
            //Assert.IsNotNull(chap, "Chapter Is Null");
            //Assert.AreNotEqual(first.NumberView, 0);
            //Assert.AreNotEqual(first.MaxPageTruyen, 0);
        }


        [TestMethod]
        public void TestTruyenConvertGetListChapter()
        {
            var top = WrapperCreator.Create(SiteTruyen.TruyenConvert);
            var num = rand.Next(10);
            var result = Task.Run(() => top.GetTopTruyen()).Result;
            var truyen = result.ListTruyen[num];
            truyen.CurrentPageRead = 2;
            truyen.MaxPageTruyen = 10;
            Task.Run(() => truyen.LoadCurrentListChapter()).Wait();
            //for (int i = 0; i < truyen.CurrentChapters.Count; i++)
            //{
            //    var chap = truyen.GetChapterByIndex(i);
            //    Assert.AreEqual(i, chap.IndexNumberPageOfChapter, "So chuong danh dau k0 bang nhau");

            //}
        }

        [TestMethod]
        public void TestTruyenConvertGetChapter()
        {
            var top = WrapperCreator.Create(SiteTruyen.TruyenConvert);
            var truyen = new Truyen(SiteTruyen.TruyenConvert);
            truyen.TruyenUrl = "http://truyencv.com/thien-vuc-thuong-khung/";
            var result = Task.Run(() => top.GetChapterNumber(truyen, 22)).Result;
            Assert.AreEqual(result.IndexStartOfChapter, 22, " Chỉ số bị sai");
            var chapter = result.ListChapter[0];
            Debug.WriteLine("Nội dung " + chapter.NoiDung);
            
        }




        [TestMethod]
        public void TestTungHoanhtGetTop()
        {
            var top = WrapperCreator.Create(SiteTruyen.TungHoanh);
            var result = Task.Run(() => top.GetTopTruyen()).Result;
            Assert.AreNotEqual(result.ListTruyen.Count, 0);
            Assert.AreNotEqual(result.MaxPageIndex, 0);
            Debug.WriteLine("MaxPage" + result.MaxPageIndex);
            Debug.WriteLine("MaxPage" + result.MaxPageIndex);
            var first = result.ListTruyen[7];
            Assert.IsNotNull(first.Author);
            Assert.IsNotNull(first.Title);
            first.CurrentPageRead = 2;
            first.MaxPageTruyen = 10;
            //Task.Run(() => first.LoadCurrentListChapter()).Wait();
            //var chap = first.GetChapterByIndex(1);
            //Assert.IsNotNull(chap, "Chapter Is Null");
            //Assert.AreNotEqual(first.NumberView, 0);
            //Assert.AreNotEqual(first.MaxPageTruyen, 0);
        }


        [TestMethod]
        public void TestTungHoanhGetListChapter()
        {
            var top = WrapperCreator.Create(SiteTruyen.TungHoanh);
            var num = rand.Next(10);
            var result = Task.Run(() => top.GetTopTruyen()).Result;
            var truyen = result.ListTruyen[num];
            truyen.CurrentPageRead = 2;
            truyen.MaxPageTruyen = 10;
            Task.Run(() => truyen.LoadCurrentListChapter()).Wait();
            //for (int i = 0; i < truyen.CurrentChapters.Count; i++)
            //{
            //    var chap = truyen.GetChapterByIndex(i);
            //    Assert.AreEqual(i, chap.IndexNumberPageOfChapter, "So chuong danh dau k0 bang nhau");

            //}
        }

        [TestMethod]
        public void TestTungHoanhGetChapter()
        {
            var top = WrapperCreator.Create(SiteTruyen.TungHoanh);
            var truyen = new Truyen(SiteTruyen.TruyenConvert);
            truyen.TruyenUrl = "http://tunghoanh.com/huan-luyen-vien-huyen-thoai-Cqaaaab.html";
            var result = Task.Run(() => top.GetChapterNumber(truyen, 22)).Result;
            Assert.AreEqual(result.IndexStartOfChapter, 22, " Chỉ số bị sai");
            var chapter = result.ListChapter[0];
            Debug.WriteLine("Nội dung " + chapter.NoiDung);

        }
    }
}
