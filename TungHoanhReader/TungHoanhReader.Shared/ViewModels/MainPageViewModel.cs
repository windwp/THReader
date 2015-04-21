using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using TungHoanhReader.Common;
using TungHoanhReader.Models;
using TungHoanhReader.Wrapper;
using TungHoanhReader.Services;
using TungHoanhReader.Services.DialogService;

namespace TungHoanhReader.ViewModels
{
    internal partial class MainPageViewModel : ViewModel
    {
        private bool _isLoading;

        private bool _isSearch;
        private string _currentSearchQuery;

        [RestorableState]
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        [RestorableState]
        public bool IsSearch
        {
            get { return _isSearch; }
            set
            {
                _isSearch = value;
                OnPropertyChanged("IsSearch");

            }
        }

        [RestorableState]
        public Wrapper.TagTruyen TheLoaiDangXem
        {
            get { return _theLoaiDangXem; }
            set { _theLoaiDangXem = value; OnPropertyChanged("TheLoaiDangXem"); }
        }

        [RestorableState]
        public Wrapper.SiteTruyen TrangDocTruyen
        {
            get { return _trangDocTruyen; }
            set { _trangDocTruyen = value; OnPropertyChanged("TrangDocTruyen"); }
        }

        private Wrapper.TagTruyen _theLoaiDangXem;
        private Wrapper.SiteTruyen _trangDocTruyen;

        [RestorableState]
        public double ListViewScrollValue { get; set; }
        [RestorableState]
        public ObservableCollection<TagTruyenModel> ListTheLoai { get; set; }
        [RestorableState]
        public ObservableCollection<SiteTruyenVM> ListSiteTruyen { get; set; }
        [RestorableState]
        public ObservableCollection<Truyen> DanhSachTruyen { get; set; }

        [RestorableState]
        public Truyen TruyenSelectedItems { get; set; }

        private DelegateCommand _loadingTheLoaiCommand = null;


        public int CurrentPageIndex { get; set; }
        public int MaxPageIndex { get; set; }
        private readonly INavigationService _navService;
        private readonly IDialogService _dialogService;
        private readonly ISettingService _settingService;
        private readonly IResourceLoader _resourceLoader;
        public MainPageViewModel(INavigationService navService, IDialogService dialogService, ISettingService settingService, IResourceLoader resourceLoader)
        {
            _navService = navService;
            _dialogService = dialogService;
            _settingService = settingService;
            _resourceLoader = resourceLoader;
            CurrentPageIndex = 1;
            MaxPageIndex = 2;
            TrangDocTruyen = Wrapper.SiteTruyen.LuongSonBac;
            TheLoaiDangXem = Wrapper.TagTruyen.Default;
            GenerateData();
            IsLoading = false;
            JumpPageCommand = new DelegateCommand<string>(AddPage);
            LoadingNextPageCommand = new CompositeCommand();
            LoadingNextPageCommand.RegisterCommand(JumpPageCommand);
            LoadingNextPageCommand.RegisterCommand(LoadingTheLoaiCommand);
            LoadingPreviousCommand = new CompositeCommand();
            LoadingPreviousCommand.RegisterCommand(JumpPageCommand);
            LoadingPreviousCommand.RegisterCommand(LoadingTheLoaiCommand);


            NavigateChapterCommand = new DelegateCommand<object>(NavigateChapter);
            FirstLoadCommand = new DelegateCommand(FisrtLoad);
            SearchPageCommand = new DelegateCommand<string>(Search);
            ClearSearchPageCommand = new DelegateCommand(ClearSearch);
            ChangeSiteTruyenCommand = new DelegateCommand<SiteTruyenVM>(
                async (value) =>
                {
                    ChangeSiteTruyen(value);
                    await LoadingTheLoaiCommand.Execute();
                });
            ListViewScrollValue = 0;


        }

        private async void ChangeSiteTruyen(SiteTruyenVM obj)
        {
            TrangDocTruyen = obj.Site;
            switch (obj.Site)
            {
                case Wrapper.SiteTruyen.LuongSonBac:
                    ListTheLoai = new ObservableCollection<TagTruyenModel>();
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.Favorite });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.HuyenAo });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.TienHiep });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.DoThi });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.KiemHiep });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.VongDu });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.NgonTinh });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.KiemHiepVN });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.TrinhTham });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.TruyenDai });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.DaSuVietNam });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.DaSuTrungQuoc });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.TheLoaiKhac });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.Default });
                    break;
                case Wrapper.SiteTruyen.TruyenConvert:
                case Wrapper.SiteTruyen.TungHoanh:
                    ListTheLoai = new ObservableCollection<TagTruyenModel>();
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.Favorite });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.HuyenAo });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.TienHiep });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.DoThi });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.KiemHiep });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.VongDu });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.XuyenKhong });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.NgonTinh });
                    ListTheLoai.Add(new TagTruyenModel { Value = Wrapper.TagTruyen.Default });
                    break;
            }
            OnPropertyChanged("ListTheLoai");
            TheLoaiDangXem = Wrapper.TagTruyen.Default;
        }

        // nhay sang chuong moi'
        private void NavigateChapter(object obj)
        {
            var listview = obj as ListView;
            if (listview != null && listview.SelectedItem != null)
            {
                ListViewScrollValue = listview.GetScrollPosition().Y;
                if (IsLoading) return;
                IsLoading = true;
                var value = listview.SelectedItem as Truyen;
                if (value != null)
                {
                    IsLoading = false;
                    _navService.Navigate(AppPages.Truyen, value);
                }
            }
        }
        // load khi lan dau tien xem
        private async void FisrtLoad()
        {
            DanhSachTruyen.Clear();
            if (await _settingService.IsHaveFavorite())
            {
                await LoadingFavorite();
            }
            else
            {
                await LoadingTheLoaiCommand.Execute();
            }

        }
        /// <summary>
        /// Xoa thong tin search
        /// </summary>
        private async void ClearSearch()
        {
            IsSearch = false;
            _currentSearchQuery = string.Empty;
            CurrentPageIndex = 0;
            await LoadingTheLoaiCommand.Execute();
        }

        public async void Search(string query)
        {
            if (!IsLoading)
            {
                IsLoading = true;
                IsSearch = true;
                _currentSearchQuery = query;
                var wrapper = WrapperCreator.Create(TrangDocTruyen);
                var result = await wrapper.SearchTruyen(query, CurrentPageIndex);
                DanhSachTruyen.Clear();
                foreach (var truyen in result.ListTruyen)
                {
                    DanhSachTruyen.Add(truyen);
                }
                MaxPageIndex = result.MaxPageIndex;
                IsLoading = false;
                this.OnPropertyChanged("DanhSachTruyen");
            }
        }

        private async Task<bool> LoadingFavorite()
        {
            if (await _settingService.IsHaveFavorite())
            {
                if (!IsLoading)
                {
                    IsLoading = true;
                    DanhSachTruyen.Clear();
                    TheLoaiDangXem = Wrapper.TagTruyen.Favorite;
                    var list = await _settingService.ListFavoriteSong();
                    foreach (var truyenKey in list)
                    {
                        DanhSachTruyen.Add(TruyenSqlFavoriteData.Convert(truyenKey));
                    }
                    IsLoading = false;
                }
                return true;
            }

            return false;
        }
        private async void Loading(int pageIndex)
        {
            if (_isSearch) Search(_currentSearchQuery);
            if (TheLoaiDangXem == Wrapper.TagTruyen.Favorite)
            {
                await LoadingFavorite();
                return;
            }
            if (!IsLoading)
            {
                IsLoading = true;
                var currentTheload = TheLoaiDangXem;
                var wrapper = WrapperCreator.Create(TrangDocTruyen);
                var result = await wrapper.GetTopTruyen(pageIndex, currentTheload);
                DanhSachTruyen.Clear();
                if (result != null)
                {
                    foreach (var truyen in result.ListTruyen)
                    {
                        DanhSachTruyen.Add(truyen);
                    }
                    MaxPageIndex = result.MaxPageIndex;
                }
                IsLoading = false;
                this.OnPropertyChanged("DanhSachTruyen");
            }
        }

        void AddPage(string number)
        {
            var n = 0;
            int.TryParse(number, out n);
            AddPage(n);
        }
        void AddPage(int? number)
        {
            if (number == null) return;
            CurrentPageIndex += (int)number;
            if (CurrentPageIndex < 1) CurrentPageIndex = 1;
            if (CurrentPageIndex >= MaxPageIndex) CurrentPageIndex = MaxPageIndex - 1;

        }



        public DelegateCommand<SiteTruyenVM> ChangeSiteTruyenCommand { get; set; }
        public DelegateCommand<object> NavigateChapterCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand FirstLoadCommand { get; set; }
        public CompositeCommand LoadingNextPageCommand { get; set; }
        public CompositeCommand LoadingPreviousCommand { get; set; }
        public DelegateCommand<string> JumpPageCommand { get; set; }
        public DelegateCommand<string> SearchPageCommand { get; set; }
        public DelegateCommand ClearSearchPageCommand { get; set; }

        private DelegateCommand _disPlayGotoPopUpCommand;

        public DelegateCommand DisplayGotoPopUpCommand
        {
            get
            {
                if (_disPlayGotoPopUpCommand != null)
                    return _disPlayGotoPopUpCommand;
                _disPlayGotoPopUpCommand = new DelegateCommand
               (
                   () =>
                   {
                       _dialogService.ShowInput("Nhập số trang cần nhảy đến", "", async (o, s) =>
                       {
                           var outNumberPage = 0;
                           if (int.TryParse(s, out outNumberPage))
                           {
                               if (outNumberPage > MaxPageIndex)
                               {
                                   _dialogService.ShowS("Số trang tối đa là " + MaxPageIndex);
                               }
                               else
                               {
                                   CurrentPageIndex = outNumberPage;
                                   await LoadingTheLoaiCommand.Execute();
                               }
                           }
                           else
                           {
                               _dialogService.ShowS("Vui lòng nhập số ");
                           }

                       });
                   },
                   () => true
               );
                this.PropertyChanged += (s, e) => _disPlayGotoPopUpCommand.RaiseCanExecuteChanged();
                return _disPlayGotoPopUpCommand;
            }
        }
        private DelegateCommand _displaySearchPopUpCommand;
        public DelegateCommand DisplaySearchPopUpCommand
        {
            get
            {
                if (_displaySearchPopUpCommand != null)
                    return _displaySearchPopUpCommand;
                _displaySearchPopUpCommand = new DelegateCommand
               (
                   () =>
                   {
                       _dialogService.ShowInput("Nhập từ khóa cần tìm kiếm", "", async (o, s) =>
                       {
                           await SearchPageCommand.Execute(s);
                       });
                   },
                   () => true
               );
                this.PropertyChanged += (s, e) => _displaySearchPopUpCommand.RaiseCanExecuteChanged();
                return _displaySearchPopUpCommand;
            }
        }




        public DelegateCommand LoadingTheLoaiCommand
        {
            get
            {
                if (_loadingTheLoaiCommand != null)
                    return _loadingTheLoaiCommand;
                _loadingTheLoaiCommand = new DelegateCommand
                (
                    () =>
                    {
                        DanhSachTruyen.Clear();
                        Loading(CurrentPageIndex);
                    }
                );
                this.PropertyChanged += (s, e) => _loadingTheLoaiCommand.RaiseCanExecuteChanged();
                return _loadingTheLoaiCommand;
            }
        }

        public DelegateCommand AboutCommand { get; set; }
        public DelegateCommand XemTruyenCommand { get; set; }




        private void GenerateData()
        {
            ListSiteTruyen = new ObservableCollection<SiteTruyenVM>();
            ListSiteTruyen.Add(new SiteTruyenVM() { Display = "truyenconvert.com", Site = Wrapper.SiteTruyen.TruyenConvert });
            ListSiteTruyen.Add(new SiteTruyenVM() { Display = "lsb-thuquan.eu", Site = Wrapper.SiteTruyen.LuongSonBac });
            ListSiteTruyen.Add(new SiteTruyenVM() { Display = "tunghoanh.com", Site = Wrapper.SiteTruyen.TungHoanh });
            ChangeSiteTruyen(new SiteTruyenVM() { Site = Wrapper.SiteTruyen.TruyenConvert });

            DanhSachTruyen = new ObservableCollection<Truyen>();
            DanhSachTruyen.Add(new Truyen(TrangDocTruyen) { Title = "Thần Mộ", Author = "Bá", Category = "Tiên Hiệp" });
            DanhSachTruyen.Add(new Truyen(TrangDocTruyen) { Title = "Thiên Vu", Author = "Cửu Hanh", Category = "Huyền Ảo" });
            DanhSachTruyen.Add(new Truyen(TrangDocTruyen) { Title = "Thiên Vu", Author = "Cửu Hanh", Category = "Huyền Ảo" });
            DanhSachTruyen.Add(new Truyen(TrangDocTruyen) { Title = "Thiên Vu", Author = "Cửu Hanh", Category = "Huyền Ảo" });
            DanhSachTruyen.Add(new Truyen(TrangDocTruyen) { Title = "Thiên Vu", Author = "Cửu Hanh", Category = "Huyền Ảo" });
            DanhSachTruyen.Add(new Truyen(TrangDocTruyen) { Title = "Thiên Vu", Author = "Cửu Hanh", Category = "Huyền Ảo" });
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {

            base.OnNavigatedFrom(viewModelState, suspending);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (navigationMode == NavigationMode.New)
            {
                if (!viewModelState.ContainsKey("DanhSachTruyen"))
                {
                    FirstLoadCommand.Execute();
                }
            }
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }

    public class SiteTruyenVM
    {
        public Wrapper.SiteTruyen Site { get; set; }
        public string Display { get; set; }
    }
}
