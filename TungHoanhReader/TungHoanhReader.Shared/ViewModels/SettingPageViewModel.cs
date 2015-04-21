using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using TungHoanhReader.Services;

namespace TungHoanhReader.ViewModels
{
    class SettingPageViewModel:ViewModel
    {


        private double _fontChuSizeValue;

        public double FontChuSizeValue
        {
            get { return _fontChuSizeValue; }
            set
            {
                _fontChuSizeValue = value;
                OnPropertyChanged("FontChuSizeValue");
            }
        }

        private INavigationService _navigationService;
        private ISettingService _settingService;
        public SettingPageViewModel(ISettingService settingService, INavigationService navigationService)
        {
            _settingService = settingService;
            _navigationService = navigationService;
            FontChuSizeValue = _settingService.USetting.FontSize;
            SaveConfigCommand=new DelegateCommand(SaveConfig);
        }

        private void SaveConfig()
        {
            _settingService.USetting.FontSize = FontChuSizeValue;
            _settingService.SaveStorage();
            _navigationService.GoBack();
        }


        public DelegateCommand SaveConfigCommand { get; set; }
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
