using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    public class TicketDetailAdminViewModel
    {
        // Thông tin vé
        public string MaVe { get; set; }
        public decimal GiaVe { get; set; }
        public string TrangThaiVe { get; set; } // Trạng thái thanh toán

        // Thông tin khách
        public string TenKhach { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }

        // Thông tin chuyến bay
        public string MaCB { get; set; }
        public string HangBay { get; set; }
        public string SanBayDi { get; set; }
        public string SanBayDen { get; set; }
        public string NgayBay { get; set; }
        public string GioBay { get; set; }
        public string SoGhe { get; set; }
        public string HangGhe { get; set; }

        // Trạng thái Check-in
        public string TrangThaiCheckIn { get; set; } // "Đã check-in" hoặc "Chưa check-in"
        public DateTime? ThoiGianCheckIn { get; set; }

    }
}