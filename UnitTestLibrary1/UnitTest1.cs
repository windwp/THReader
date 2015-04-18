using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TungHoanhReader;

namespace UnitTestLibrary1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetTop()
        {
            var top = new LuongSonBacWrapper();
            var result = top.GetTopTruyen();
            Assert.AreNotEqual(result.Count, 0);
            var first = result[0];
            Assert.IsNotNull(first.Author);
            Assert.IsNotNull(first.Title);
            first.CurrentPageRead = 2;
            first.LoadCurrentChapter();
            var chap = first.GetChapterByIndex(1);
            Assert.IsNotNull(chap, "Chapter Is Null");
            Assert.AreNotSame(first.NumberView, 0);
        }

        [TestMethod]
        public void TestGetByTag()
        {
            Assert.AreEqual(TagTruyen.DoThi.ToLsbString(), "đô+thị");
            var top = new LuongSonBacWrapper();
            var result = top.GetTopTruyen(1, TagTruyen.DoThi);
            foreach (var truyen in result)
            {
                Assert.AreEqual(truyen.Category, "Đô Thị");
            }
        }
    }
}
