﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace TungHoanhReader.ViewModels
{
    partial class MainPageViewModel:ViewModel
    {
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
    }
}
