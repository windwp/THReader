using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TungHoanhReader.Wrapper
{
    public class Truyen : INotifyPropertyChanged
    {


        private IWraper _wraper;
        private readonly SiteTruyen _wrapertype;
        public Truyen(SiteTruyen wrapertype)
        {
            _wrapertype = wrapertype;
            // so chuong cache la 5
            _currentChapters = new List<Chapter>();
            //_cacheChapeters = new List<Chapter>(_chapterPerPage);
            CurrentIndexChapter = 0;
            CurrentPageCacheRead = CurrentPageRead + 1;
        }

        public bool IsLoading = false;


        public string Category { get; set; }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public string Author { get; set; }
        public string TruyenUrl { get; set; }
        public string StartDateTime { get; set; }
        public int NumberChaper { get; set; }
        public int NumberView { get; set; }
        public int CurrentIndexChapter { get; set; }
        public int CurrentPageRead { get; set; }
        private int CurrentPageCacheRead { get; set; }
        public string Title { get; set; }
        public int MaxPageTruyen { get; set; }

        public List<Chapter> CurrentChapters
        {
            get { return _currentChapters; }
            set { _currentChapters = value; OnPropertyChanged(); }
        }

        public IWraper Wrapper
        {
            get
            {
                if (_wraper != null) return _wraper;
                _wraper = WrapperCreator.Create(_wrapertype);
                return _wraper;
            }
        }

        public SiteTruyen Site
        {
            get { return _wrapertype; }
        }
 


        private List<Chapter> _currentChapters;
        private string _description;

        public Chapter GetChapter(int numChapter)
        {
            if (numChapter - CurrentPageRead * Wrapper.NumberChapterPerPage > _currentChapters.Count)
            {
                return _currentChapters[numChapter - numChapter - CurrentPageRead * Wrapper.NumberChapterPerPage];
            }
            return null;
        }

        public async Task LoadCurrentListChapter(bool allowHaveNoiDung = false)
        {
            IsLoading = true;
            if (CurrentPageRead > MaxPageTruyen) CurrentPageRead = MaxPageTruyen - 1;
            if (CurrentPageRead < 1) CurrentPageRead = 1;
            var result = await Wrapper.GetPageChapTer(this, CurrentPageRead, allowHaveNoiDung);
            if (result == null)
            {//k0 tim thay' chap tiep theo
                CurrentChapters.Clear();
                return;
            }
            
            CurrentChapters = result.ListChapter;
            MaxPageTruyen = result.MaxPageIndex;
            IsLoading = false;
        }

        public async Task LoadNextChapter()
        {
            IsLoading = true;
            CurrentPageRead += 1;
            await LoadCurrentListChapter();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageOfRead"> page can` doc.</param>
        /// <param name="indexOfPage"> vi. tri' page can` doc.</param>
        /// <returns></returns>
        public async Task<int> LoadChapterWithNoiDung(int pageOfRead, int indexOfPage)
        {
            // tinh toan page can phai? lay' 
            // neu' vi. tri' k0 con` trong thi` load tiep
            if (Site == SiteTruyen.LuongSonBac)
            {
                if (indexOfPage >= Wrapper.NumberChapterPerPage)
                {
                    pageOfRead += 1;
                    indexOfPage = 0;
                }
                else if (pageOfRead == 1 && indexOfPage < 0)
                {
                    //Trang cuoi' cung roi`
                    return -1;
                }
                else if (indexOfPage < 0)
                {
                    // quay lai. trang cu~ hon
                    pageOfRead -= 1;
                    indexOfPage = Wrapper.NumberChapterPerPage - 1;
                }
            }
            else
            {
                pageOfRead = indexOfPage;
            }
            if (pageOfRead == 0) return indexOfPage;
            CurrentPageRead = pageOfRead;
            MaxPageTruyen = CurrentPageRead + 1;
            await LoadCurrentListChapter(true);
            return indexOfPage;
        }

        public async Task<int> GotoChapter(int chapterIndex)
        {
            IsLoading = true;
            var result = await Wrapper.GetChapterNumber(this,chapterIndex);
            CurrentChapters = result.ListChapter;
            MaxPageTruyen = result.MaxPageIndex;
            IsLoading = false;
            return result.IndexStartOfChapter;

        }
        public Chapter GetChapterByIndex(int index)
        {

            foreach (var currentChapter in CurrentChapters)
            {
                if (currentChapter.IndexNumberPageOfChapter == index)
                {
                    return currentChapter;
                }
            }
            return null;
        }

        public Chapter GetNextChapterByIndex(int index)
        {

            foreach (var currentChapter in CurrentChapters)
            {
                if (currentChapter.IndexNumberPageOfChapter == index + 1)
                {
                    return currentChapter;
                }
            }
            return null;
        }

        public Chapter GetPreviousChapterByIndex(int index)
        {

            foreach (var currentChapter in CurrentChapters)
            {
                if (currentChapter.IndexNumberPageOfChapter == index - 1)
                {
                    return currentChapter;
                }
            }
            return null;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Chapter
    {
        public string TenChuong { get; set; }
        public string SoThuTu { get; set; }
        public string Quyen { get; set; }
        public string NoiDung { get; set; }
        public string Nguon { get; set; }

        public string TruyenUrl { get; set; }
        /// <summary>
        /// Trang truyen.
        /// </summary>
        public int PageOfChapter { get; set; }
        /// <summary>
        /// Vi. tri' cua? chapter trong trang truyen
        /// </summary>
        public int IndexNumberPageOfChapter { get; set; }
        /// <summary>
        /// vi. tri' cua? chapter neu' ko phan biet. trang
        /// </summary>
        public int IndexChapter { get; set; }
    }
}
