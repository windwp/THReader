using System;
using System.Collections.Generic;
using System.Text;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.ViewModels
{
    public class ThNavParamater
    {
        public Chapter Chap { get; set; }
        public Wrapper.SiteTruyen TrangTruyen { get; set; }
        public Truyen TruyenDoc { get; set; }
    }
}
