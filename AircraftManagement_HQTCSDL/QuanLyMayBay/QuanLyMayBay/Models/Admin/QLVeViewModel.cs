using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    public class QLVeViewModel
    {
        // Danh sách để hiển thị bảng
        public List<QuanLyVe> DanhSachVe { get; set; }

        // Vé đang được chọn để hiển thị lên Modal (nếu null nghĩa là chưa chọn)
        public TicketDetailAdminViewModel VeDangChon { get; set; }
    }
}