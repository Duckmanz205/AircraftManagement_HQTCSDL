using System;

namespace QuanLyMayBay.Models
{
    public class CheckInViewModel
    {
        public string MaVe { get; set; }
        public string MaCB { get; set; }
        public string MaGhe { get; set; }
        public string SoGhe { get; set; }
        public string HangGhe { get; set; }
        
        // Thông tin chuyến bay
        public string SanBayDiMa { get; set; }
        public string SanBayDiTen { get; set; }
        public string SanBayDiThanhPho { get; set; }
        public string SanBayDenMa { get; set; }
        public string SanBayDenTen { get; set; }
        public string SanBayDenThanhPho { get; set; }
        public DateTime GioCatCanh { get; set; }
        public DateTime GioHaCanh { get; set; }
        public string ThoiGianBay { get; set; }
        public string TrangThaiChuyenBay { get; set; }


        // Thông tin hành khách
        public string TenHanhKhach { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string QuocGia { get; set; }
        
        // Thông tin vé
        public decimal? GiaVe { get; set; }
        public string TrangThaiCheckIn { get; set; }
        public DateTime? ThoiGianCheckIn { get; set; }
        public bool DaCheckIn { get; set; }
        
        // Tính toán thời gian
        public bool CoTheCheckIn { get; set; }
        public string ThoiGianConLai { get; set; }
        
        // Hành lý
        public int? HangLyXachTay { get; set; }
        public int? HangLyKyGui { get; set; }

    }
}

