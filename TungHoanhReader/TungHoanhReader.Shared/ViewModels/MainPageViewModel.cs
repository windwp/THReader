using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.ViewModels
{
    public partial class MainPageViewModel : ViewModel
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public TagTruyen TheLoaiDangXem { get; set; }
        public ObservableCollection<TagTruyenModel> ListTheLoai { get; set; }
        public ObservableCollection<Truyen> DanhSachTruyen { get; set; }
        public SiteTruyen TrangDocTruyen { get; set; }
        public int PageIndex;
        private readonly INavigationService _navService;
        public MainPageViewModel(INavigationService navService)
        {
            _navService = navService;
            TrangDocTruyen=SiteTruyen.LuongSonBac;
            TheLoaiDangXem = TagTruyen.Default;
            GenerateData();
            IsLoading = false;

            LoadingTheLoaiCommand = new DelegateCommand(Loading);
        }



        private  async void Loading()
        {
            IsLoading = true;
            var currentTheload = TheLoaiDangXem;
            var wrapper = new LuongSonBacWrapper();
            var result= await wrapper.GetTopTruyen(PageIndex,currentTheload);
            DanhSachTruyen.Clear();
            foreach (var truyen in result)
            {
                DanhSachTruyen.Add(truyen);
            }
            this.OnPropertyChanged("DanhSachTruyen");
            IsLoading = false;
        } 
        public DelegateCommand LoadingTheLoaiCommand { get; set; }
        public DelegateCommand AboutCommand { get; set; }
        public DelegateCommand XemTruyenCommand { get; set; }
        private void GenerateData()
        {
            ListTheLoai = new ObservableCollection<TagTruyenModel>();
            ListTheLoai.Add(new TagTruyenModel{Value=TagTruyen.HuyenAo});
            ListTheLoai.Add(new TagTruyenModel{Value=TagTruyen.TienHiep});
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.DoThi });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.KiemHiep });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.VongDu });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.NgonTinh });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.KiemHiepVN });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.TrinhTham });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.TruyenDai });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.DaSuVietNam });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.DaSuTrungQuoc });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.TheLoaiKhac });
            ListTheLoai.Add(new TagTruyenModel { Value = TagTruyen.Default });
          
            DanhSachTruyen = new ObservableCollection<Truyen>();
            DanhSachTruyen.Add(new Truyen(){Title = "Thần Mộ",Author = "Bá",Category = "Tiên Hiệp"}); 
            DanhSachTruyen.Add(new Truyen(){Title = "Thiên Vu",Author = "Cửu Hanh",Category = "Huyền Ảo"}); 
            DanhSachTruyen.Add(new Truyen(){Title = "Thiên Vu",Author = "Cửu Hanh",Category = "Huyền Ảo"}); 
            DanhSachTruyen.Add(new Truyen(){Title = "Thiên Vu",Author = "Cửu Hanh",Category = "Huyền Ảo"}); 
            DanhSachTruyen.Add(new Truyen(){Title = "Thiên Vu",Author = "Cửu Hanh",Category = "Huyền Ảo"}); 
            DanhSachTruyen.Add(new Truyen(){Title = "Thiên Vu",Author = "Cửu Hanh",Category = "Huyền Ảo"}); 
        }
    }
}
