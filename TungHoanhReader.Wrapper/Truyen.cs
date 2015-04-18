using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    public class Truyen
    {

        public Truyen()
        {
            // so chuong cache la 5
            _currentChapeter = new List<Chapter>(_chapterPerPage);
            _cacheChapeter = new List<Chapter>(_chapterPerPage);
            CurrentIndexChapter = 0;
            CurrentPageCacheRead = CurrentPageRead + 1;
        }

        public bool IsLoading = false;
        public string Category { get; set; }
        public string Author { get; set; }
        public string ChapterUrl { get; set; }
        public string StartDateTime { get; set; }
        public int NumberChaper { get; set; }
        public int NumberView { get; set; }
        public int CurrentIndexChapter { get; set; }
        public int CurrentPageRead { get; set; }
        private int CurrentPageCacheRead { get; set; }
        public string Title { get; set; }

        private List<Chapter> _cacheChapeter;
        private List<Chapter> _currentChapeter;
        private int _chapterPerPage = 10;

        public Chapter GetChapterByIndex(int index)
        {
            if (index < _currentChapeter.Count)
            {
                return _currentChapeter[index];
            }
            return null;
        }


        public Chapter GetChapter(int numChapter)
        {
            if (numChapter - CurrentPageRead * _chapterPerPage > _currentChapeter.Count)
            {
                return _currentChapeter[numChapter - numChapter - CurrentPageRead * _chapterPerPage];
            }
            return null;
        }

        public void LoadCacheChapter()
        {
            var wrapper = new LuongSonBacWrapper();
            _cacheChapeter = wrapper.GetPageChapTer(this, CurrentPageCacheRead);


        }

        public void LoadCurrentChapter()
        {
            IsLoading = true;
            if (_cacheChapeter != null && CurrentPageRead == CurrentPageCacheRead)
            {
                _currentChapeter = _cacheChapeter;
                IsLoading = false;
                return;
            }
            var wrapper = new LuongSonBacWrapper();
            _currentChapeter = wrapper.GetPageChapTer(this, CurrentPageRead);
            IsLoading = false;
        }

        public void LoadNextChapter()
        {
            IsLoading = true;
            CurrentPageRead += 1;
            LoadCurrentChapter();
        }


    }

    public class Chapter
    {
        public string TenChuong { get; set; }
        public string SoThuTu { get; set; }
        public string Quyen { get; set; }
        public string NoiDung { get; set; }
        public string Nguon { get; set; }
    }
}
