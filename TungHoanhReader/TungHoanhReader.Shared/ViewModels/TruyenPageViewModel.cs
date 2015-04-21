using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using TungHoanhReader.Services;
using TungHoanhReader.Wrapper;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TungHoanhReader.ViewModels
{
    internal partial class TruyenPageViewModel : ViewModel
    {
        private Truyen _truyen;
        private bool _isLoading;
        [RestorableState]
        public Wrapper.SiteTruyen TrangDocTruyen
        {
            get { return _trangDocTruyen; }
            set { _trangDocTruyen = value; OnPropertyChanged("TrangDocTruyen"); }
        }


        private Wrapper.SiteTruyen _trangDocTruyen;
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

        private bool _isDiplayLoadMorChapter;
        private int _lastReadChapterBefore;
        private bool _isReadingBefore;


        [RestorableState]
        public ObservableCollection<Chapter> Chapters { get; set; }
        [RestorableState]
        public Truyen Truyen
        {
            get { return _truyen; }
            set
            {
                _truyen = value;
                OnPropertyChanged("Truyen");
            }
        }
        private readonly INavigationService _navigationService;
        private readonly ISettingService _settingService;
        public TruyenPageViewModel(INavigationService navigationService, ISettingService settingService)
        {
            _navigationService = navigationService;
            _settingService = settingService;
            Chapters = new ObservableCollection<Chapter>();
            Truyen = new Truyen(TrangDocTruyen);
            Truyen.Title = "Thần Mộ";
            FirstLoadCommand = new DelegateCommand(async () =>
            {
                PrcocessDisplayForSiteTruyen();
                await LoadChapter();
            });
            LoadNextListChapterCommand = new DelegateCommand(async () =>
            {
                _truyen.CurrentPageRead += 1;
                await LoadChapter();
            });
            NavigateToChapterCommand = new DelegateCommand<SelectionChangedEventArgs>(NavigateToChapter);
            LoadEndChapterCommand = new DelegateCommand(LoadEndChapter);
            LoadBeginChapterCommand = new DelegateCommand(LoadBeginChapter);
            AddFavoriteCommand = new DelegateCommand(AddFavorite);
            RemoveFavoriteCommand = new DelegateCommand(RemoveFavorite);
            ReadFirstChapterCommand = new DelegateCommand(() =>
            {
                NavigateToChapter(1, _truyen.Site == Wrapper.SiteTruyen.LuongSonBac ? 0 : 1);
            });
            ReadLastChapterCommand = new DelegateCommand(() =>
            {
                NavigateToChapter(_truyen.MaxPageTruyen, _truyen.Site == Wrapper.SiteTruyen.LuongSonBac ? 0 : _truyen.MaxPageTruyen);
            });

            ReadNewChapterCommand = new DelegateCommand(async () =>
            {
                var truyenStorage = await _settingService.GetTruyenStorageData(_truyen.Site, _truyen.TruyenUrl);
                if (truyenStorage == null)
                {
                    await ReadFirstChapterCommand.Execute();
                }
                else
                {
                    NavigateToChapter(truyenStorage.LastPageRead, truyenStorage.LastChapterRead);
                }
            });
        }

        private async void PrcocessDisplayForSiteTruyen()
        {
            switch (TrangDocTruyen)
            {
                case Wrapper.SiteTruyen.LuongSonBac:
                    IsDiplayLoadMoreChapter = true;
                    break;
                case Wrapper.SiteTruyen.TruyenConvert:
                case Wrapper.SiteTruyen.TungHoanh:
                    IsDiplayLoadMoreChapter = false;
                    break;
            }

            IsFavorite = await _settingService.IsStoryFavorite(Truyen);
            var storageData = await _settingService.GetTruyenStorageData(Truyen.Site, Truyen.TruyenUrl);
            var numberLastestRead = 1;
            if (storageData != null)
            {
                numberLastestRead = storageData.LastChapterRead;
            }
            LastReadChapterBefore = numberLastestRead;
        }



        private void NavigateToChapter(int pageread, int chapter)
        {

            var chap = new Chapter();
            chap.TruyenUrl = _truyen.TruyenUrl;
            chap.IndexChapter = chapter;
            chap.IndexNumberPageOfChapter = chapter;
            chap.PageOfChapter = pageread;
            var navObj = new ThNavParamater()
            {
                Chap = chap,
                TrangTruyen = TrangDocTruyen,
                TruyenDoc = null
            };
            _navigationService.Navigate(AppPages.Detail, navObj);

        }

        private void RemoveFavorite()
        {
            _settingService.RemoveStoryFavorite(_truyen);
            IsFavorite = false;
        }

        private void AddFavorite()
        {
            _settingService.AddStoryFavorite(_truyen);
            IsFavorite = true;
        }
        private async void LoadBeginChapter()
        {
            _truyen.CurrentPageRead = 1;
            await LoadChapter();
        }

        private async void LoadEndChapter()
        {
            _truyen.CurrentPageRead = _truyen.MaxPageTruyen;
            await LoadChapter();
        }

        private void NavigateToChapter(SelectionChangedEventArgs item)
        {
            if (item != null && item.AddedItems.Count > 0)
            {
                if (IsLoading) return;
                var value = item.AddedItems[0];
                if (value is Chapter)
                {
                    var navObj = new ThNavParamater()
                    {
                        Chap = value as Chapter,
                        TrangTruyen = TrangDocTruyen,
                        TruyenDoc = null
                    };
                    _navigationService.Navigate(AppPages.Detail, navObj);
                }

            }
        }

        void _truyen_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentChapters")
            {
                if (_truyen.CurrentChapters != null)
                    foreach (var chapter in _truyen.CurrentChapters)
                    {
                        Chapters.Add(chapter);
                    }
                OnPropertyChanged("Chapters");
            }
        }

        private async Task LoadChapter()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                await _truyen.LoadCurrentListChapter();
                IsLoading = false;
            }
        }

        public DelegateCommand LoadNextListChapterCommand { get; set; }
        public DelegateCommand ReadFirstChapterCommand { get; set; }
        public DelegateCommand ReadLastChapterCommand { get; set; }
        public DelegateCommand ReadNewChapterCommand { get; set; }

        public DelegateCommand LoadBeginChapterCommand { get; set; }
        public DelegateCommand LoadEndChapterCommand { get; set; }
        public DelegateCommand<SelectionChangedEventArgs> NavigateToChapterCommand { get; set; }
        public DelegateCommand FirstLoadCommand { get; set; }
        public DelegateCommand AddFavoriteCommand { get; set; }
        public DelegateCommand RemoveFavoriteCommand { get; set; }

        public bool IsReadingBefore
        {
            get { return _isReadingBefore; }
            set
            {
                _isReadingBefore = value;
                OnPropertyChanged("IsReadingBefore");
            }
        }

        public int LastReadChapterBefore
        {
            get { return _lastReadChapterBefore; }
            set
            {
                _lastReadChapterBefore = value;
                OnPropertyChanged("LastReadChapterBefore");
            }
        }
        [RestorableState]
        public bool IsDiplayLoadMoreChapter
        {
            get { return _isDiplayLoadMorChapter; }
            set
            {
                _isDiplayLoadMorChapter = value;
                OnPropertyChanged("IsDiplayLoadMorChapter");
            }
        }


        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (navigationParameter != null && navigationMode != NavigationMode.Back)
            {
                if (_truyen != null && navigationParameter is Truyen)
                {
                    _truyen = (Truyen)navigationParameter;
                    TrangDocTruyen = _truyen.Site;
                    _truyen.PropertyChanged += _truyen_PropertyChanged;
                    FirstLoadCommand.Execute();
                }

            }
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
