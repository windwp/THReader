
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using TungHoanhReader.Services;
using TungHoanhReader.Services.DialogService;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.ViewModels
{
    internal partial class DetailPageViewModel : ViewModel
    {
        private bool _isLoading;
        private Truyen _truyen;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }


        public ScrollViewer TextScrollViewer { get; set; }

        private bool _isFavorite;
        [RestorableState]
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }
        [RestorableState]
        public Chapter CurrentChapter
        {
            get { return _currentChapter; }
            set { _currentChapter = value;
                _currentIndexOfNumberChapter = value.IndexNumberPageOfChapter;
                OnPropertyChanged("CurrentChapter"); }
        }

        [RestorableState]
        public int CurrentIndexOfNumberChapter
        {
            get { return _currentIndexOfNumberChapter; }
            set { _currentIndexOfNumberChapter = value; }
        }

        public double FontSizeChu
        {
            get { return _fontSizeChu; }
            set
            {
                _fontSizeChu = value;
                OnPropertyChanged("FontSizeChu");
            }
        }
        private double _fontSizeChu;

        public Chapter CacheNextChapter { get; set; }

        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private ISettingService _settingService;
        private Chapter _currentChapter;

        private int _tempIndexOfNumberChapterInPage = 0;
        private int _currentIndexOfNumberChapter = 0;
        public DetailPageViewModel(INavigationService navigationService, IDialogService dialogService, ISettingService settingService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _settingService = settingService;
            //CurrentChapter=new Chapter();
            //CurrentChapter.TenChuong = "AA";
            //CurrentChapter.NoiDung = "a";
            //for (int i = 0; i < 100; i++)
            //{
            //    CurrentChapter.NoiDung += "lsddlsaldlasjddasd\r\n";
            //}
            //CurrentChapter.NoiDung += "Enddd";
            IsLoading = false;
            FirstLoadCommand = new DelegateCommand(async () =>
            {
                IsFavorite = await _settingService.IsStoryFavorite(_truyen);
                _settingService.SetTruyenLastReadChapter(_truyen.Site,_truyen.TruyenUrl,_tempIndexOfNumberChapterInPage,_truyen.CurrentPageRead);
                await LoadCurrent();
            });
            NextChapterCommand = new DelegateCommand(GotoNextChapter);
            PreviousChapterCommand = new DelegateCommand(GotoPreviousChapter);
            DisplayGotoPopUpCommand=new DelegateCommand(ShowGotoPopup);
            AddFavoriteCommand=new DelegateCommand(AddFavorite);
            RemoveFavoriteCommand=new DelegateCommand(RemoveFavorite);
            GotoSettingCommand=new DelegateCommand(()=> { _navigationService.Navigate(AppPages.Setting); });
            FontSizeChu = _settingService.USetting.FontSize;
        }

        private  void RemoveFavorite()
        {
             _settingService.RemoveStoryFavorite(_truyen);
            IsFavorite = false;
        }

        private  void AddFavorite()
        {
            _settingService.AddStoryFavorite(_truyen);
            IsFavorite = true;
        }

        private void ShowGotoPopup()
        {
            _dialogService.ShowInput("Nhập số chương cần nhảy đến", "", async (o, s) =>
            {
                var outNumberPage = 0;
                if (int.TryParse(s, out outNumberPage))
                {
                    if (!IsLoading)
                    {
                        IsLoading = true;
                        _tempIndexOfNumberChapterInPage = await _truyen.GotoChapter(outNumberPage);
                        CacheNextChapter = _truyen.GetChapterByIndex(_tempIndexOfNumberChapterInPage);
                        if (CacheNextChapter != null)
                        {
                            CurrentChapter = CacheNextChapter;
                            _tempIndexOfNumberChapterInPage = _currentIndexOfNumberChapter;
                        }
                        else
                        {
                            _dialogService.ShowS("K0 tìm thấy chương đó", "Lỗi");
                        }
                        IsLoading = false;
                    }
                }
                else
                {
                    _dialogService.ShowS("Vui lòng nhập số ");
                }

            });
        }

        private async void GotoNextChapter()
        {
            if (IsLoading) return;
            CacheNextChapter = _truyen.GetNextChapterByIndex(_currentIndexOfNumberChapter);
            _tempIndexOfNumberChapterInPage += 1;
            if (CacheNextChapter == null)
            {
                await LoadCurrent();
            }
            else
            {
                CurrentChapter = CacheNextChapter;
            }
        }

        private async void GotoPreviousChapter()
        {
            if (IsLoading) return;
            CacheNextChapter = _truyen.GetPreviousChapterByIndex(_currentIndexOfNumberChapter);
            _tempIndexOfNumberChapterInPage -= 1;
            if (CacheNextChapter == null)
            {
                await LoadCurrent();
            }
            else
            {
                CurrentChapter = CacheNextChapter;
            }
        }

        private async Task LoadCurrent()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                _tempIndexOfNumberChapterInPage = await _truyen.LoadChapterWithNoiDung(CurrentChapter.PageOfChapter, _tempIndexOfNumberChapterInPage);
                CacheNextChapter = _truyen.GetChapterByIndex(_tempIndexOfNumberChapterInPage);
                if (CacheNextChapter != null)
                {
                    CurrentChapter = CacheNextChapter;
                    if (TextScrollViewer != null) TextScrollViewer.ChangeView(0, 0, null,true);
                    _tempIndexOfNumberChapterInPage = _currentIndexOfNumberChapter;
                }
                else
                {
                    if (_truyen.Site != SiteTruyen.LuongSonBac)
                    {
                        if (_tempIndexOfNumberChapterInPage <= 0) _tempIndexOfNumberChapterInPage = 1;
                        if (_tempIndexOfNumberChapterInPage > 0) _tempIndexOfNumberChapterInPage -= 1;
                    }
                    _dialogService.ShowS("K0 tìm thấy chương sau đó", "Lỗi");
                }
                IsLoading = false;
            }
        }

        public DelegateCommand FirstLoadCommand { get; set; }
        public DelegateCommand DisplayGotoPopUpCommand { get; set; }
        public DelegateCommand NextChapterCommand { get; set; }
        public DelegateCommand PreviousChapterCommand { get; set; }
        public DelegateCommand AddFavoriteCommand { get; set; }
        public DelegateCommand RemoveFavoriteCommand { get; set; }
        public DelegateCommand GotoSettingCommand { get; set; }
      

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (navigationParameter is ThNavParamater)
            {
                var obj = navigationParameter as ThNavParamater;
                CurrentChapter = obj.Chap;
                _tempIndexOfNumberChapterInPage = CurrentChapter.IndexNumberPageOfChapter;
                _truyen = new Truyen(obj.TrangTruyen);
                _truyen.TruyenUrl = CurrentChapter.TruyenUrl;
                _truyen.CurrentPageRead = CurrentChapter.PageOfChapter;
                FirstLoadCommand.Execute();
            }
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
       
    }
}
