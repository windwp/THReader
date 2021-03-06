﻿using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using TungHoanhReader.Services;

namespace TungHoanhReader
{
    public sealed partial class App : MvvmAppBase
    {



        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            var navigationService = _container.Resolve<INavigationService>();
            navigationService.Navigate(AppPages.Main);
            return Task.FromResult<object>(null);
        }


    }
}
