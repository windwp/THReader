using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace TungHoanhReader.ViewModels
{
    class AboutPageViewModel:ViewModel
    {

        private ISessionStateService _sessionStateService;
        public AboutPageViewModel(ISessionStateService sessionStateService)
        {
            _sessionStateService = sessionStateService;

            foreach (var value in _sessionStateService.SessionState)
            {
                Debug.WriteLine(value);
            }
            
        }
    }
}
