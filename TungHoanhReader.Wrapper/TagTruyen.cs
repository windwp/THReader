namespace TungHoanhReader
{
    public enum TagTruyen
    {
       
        [LSBStringValue("Tất cả")]
        [HienThiStringValue("Tất cả")]
        Default,
        [LSBStringValue("kiếm+hiệp")]
        [TruyenConvertStringValue("kiem-hiep")]
        [HienThiStringValue("Kiếm hiệp")]
        KiemHiep,

        [LSBStringValue("kiếm+hiệp+việt+nam")]
        [HienThiStringValue("Kiếm Hiệp Hiệt Nam")]
        KiemHiepVN,

        [LSBStringValue("tiên+hiệp")]
        [TruyenConvertStringValue("tien-hiep")]
        [HienThiStringValue("Tiên Hiệp")]
        TienHiep,

        [LSBStringValue("huyền+ảo")]
        [TruyenConvertStringValue("huyen-ao")]
        [HienThiStringValue("Huyền Ảo")]
        HuyenAo,
        [LSBStringValue("sắc+hiệp")]
        [HienThiStringValue("Sắc Hiệp")]
        SacHiep,
        [LSBStringValue("dị+hiệp")]
        [TruyenConvertStringValue("di-nang")]
        [HienThiStringValue("Dị Hiệp")]
        DiHiep,
        [LSBStringValue("đô+thị")]
        [TruyenConvertStringValue("do-thi")]
        [HienThiStringValue("Đô Thị")]
        DoThi,
        [LSBStringValue("khoa+huyễn")]
        [TruyenConvertStringValue("khoa-huyen")]
        [HienThiStringValue("Khoa Huyễn")]
        KhoaHuyen,

        [LSBStringValue("võng+du")]
        [TruyenConvertStringValue("vong-du")]
        [HienThiStringValue("Võng Du")]
        VongDu,
        [LSBStringValue("truyện+sáng+tác")]
        [HienThiStringValue("Truyện Sáng Tác")]
        TruyenSangTac,
        [LSBStringValue("ngôn+tình")]
        [TruyenConvertStringValue("ngon+tinh")]
        [HienThiStringValue("Ngôn Tình")]
        NgonTinh,
        [LSBStringValue("trinh+thám")]
        [TruyenConvertStringValue("trinh-tham")]
        [HienThiStringValue("Trinh Thám")]
        TrinhTham,
        [LSBStringValue("kinh+dị")]
        [HienThiStringValue("Kinh Dị")]
        KinhDi,
        [LSBStringValue("chiến+tranh")]
        [HienThiStringValue("Chiến Tranh")]
        ChienTranh,
        [LSBStringValue("dã+sử+việt+nam")]
        [HienThiStringValue("Dã Sử Việt Nam")]
        DaSuVietNam,
        [LSBStringValue("dã+sử+trung+quốc")]
        [HienThiStringValue("Dã Sử Trung Quốc")]
        DaSuTrungQuoc,
        [LSBStringValue("lịch+sử+việt+nam")]
        [HienThiStringValue("Lịch Sử Việt Nam")]
        LichSuVietNam,
        [LSBStringValue("truyện+dài")]
        [HienThiStringValue("Truyện Dài")]
        TruyenDai,
        [LSBStringValue("thể+loại+khác")]
        [HienThiStringValue("Thể Loại Khác")]
        TheLoaiKhac,
        [LSBStringValue("xuyen-khon")]
        [HienThiStringValue("Xuyên Không")]
        XuyenKhong,
        [TruyenConvertStringValue("Quan-su")]
        [HienThiStringValue("Quân Sự")]
        QuanSu,

        [LSBStringValue("Yeu+thich")]
        [TruyenConvertStringValue("Yeu-thich")]
        [HienThiStringValue("Yêu thích")]

        Favorite
    }
}