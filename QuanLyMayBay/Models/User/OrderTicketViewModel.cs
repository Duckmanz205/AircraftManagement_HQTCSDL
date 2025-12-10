using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyMayBay.Models
{
    public class OrderTicketViewModel
    {
        public string MaGioHang { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTienGioHang { get; set; }

        // Danh sách hành khách
        public List<TicketDetailViewModel> Tickets { get; set; } = new List<TicketDetailViewModel>();

        // Số lượng hành khách (an toàn khi Tickets null hoặc rỗng)
        public int SoLuongHanhKhach => Tickets?.Count ?? 0;

        public string MaChuyenBay { get; set; }

        public string SanBayDiTen { get; set; }
        public string SanBayDenTen { get; set; }
        public string SanBayDiMa { get; set; }      // ví dụ: HAN
        public string SanBayDenMa { get; set; }     // ví dụ: SGN

        public DateTime GioCatCanh { get; set; }
        public DateTime GioHaCanh { get; set; }

        // Thời gian bay dạng "2h 15m"
        public string ThoiGianBay { get; set; }

        // Ngày bay (chỉ lấy phần ngày)
        public DateTime NgayBay { get; set; }

        // Hạng ghế chính (lấy của hành khách đầu tiên, hoặc mặc định)
        public string HangGheChinh => Tickets?.FirstOrDefault()?.HangGhe ?? "Phổ thông";

        // Thời gian giữ chỗ hết hạn – dùng để countdown chính xác trong View
        public DateTime ThoiGianGiuCho { get; set; } = DateTime.Now.AddMinutes(15);

        // Tính thời gian còn lại (giây) – rất tiện dùng trực tiếp trong JavaScript
        public int RemainingSeconds
        {
            get
            {
                var remaining = (ThoiGianGiuCho - DateTime.Now).TotalSeconds;
                return remaining > 0 ? (int)remaining : 0;
            }
        }

        // Dùng để truy xuất nhanh một số thông tin từ GIOHANG hoặc GIOHANG_CHITIET nếu cần
        // (không bắt buộc nhưng rất tiện khi cần trong View)
        public GIOHANG GioHangEntity { get; set; }
    }
}