using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    public class QLPersonViewModel
    {
        // Dữ liệu cho Tab Nhân viên
        public List<NHANVIEN> ListNhanVien { get; set; }

        // Dữ liệu cho Tab Khách hàng
        public List<KhachHangViewModel> ListKhachHang { get; set; }

        // Dữ liệu Lịch sử (nếu đang chọn 1 khách hàng)
        public CustomerHistoryViewModel SelectedHistory { get; set; }
    }
}