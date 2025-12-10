using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class CartItemDetailViewModel
    {
        // Thông tin Giỏ hàng & Hành khách
        public string MaGioHang { get; set; }
        public string MaHanhKhachGH { get; set; } // Dùng để xác định từng vé/chỗ ngồi
        public string TenHanhKhach { get; set; }
        public string GioiTinhHK { get; set; }

        // Thông tin Chuyến bay & Giá vé
        public string MaChuyenBay { get; set; } // Ví dụ: CB04
        public string SanBayDiTen { get; set; }
        public string SanBayDenTen { get; set; }
        public DateTime GioCatCanh { get; set; }

        // Thông tin Ghế (Lấy Hạng ghế vì Giỏ hàng chưa lưu Ghế cụ thể)
        public string HangGhe { get; set; } // Ví dụ: Phổ thông, Thương gia
        public int GiaVeCoSo { get; set; } // Giá ước tính cho 1 chỗ ngồi/hạng ghế
    }
}