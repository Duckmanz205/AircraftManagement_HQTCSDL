using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class TicketDetailViewModel
    {
        // ID xử lý nội bộ (MAHK khi chưa thanh toán, MAVE khi đã thanh toán)
        public string ID { get; set; }

        // Mã vé chính thức (ví dụ: TKTABCD1234) – chỉ có khi đã thanh toán
        public string MaVe { get; set; }

        public string TenHanhKhach { get; set; }

        public string SoGhe { get; set; } = "Chưa chọn"; // SOGHE

        public string HangGhe { get; set; } = "Phổ thông";

        public decimal TongTienVe { get; set; }

        public bool IsPaid { get; set; } = false;

        public bool DaCheckIn { get; set; } = false;

        // === BỔ SUNG ĐỂ VIEW DỄ DÙNG HƠN ===
        public string GioiTinh { get; set; } // "Nam", "Nữ", hoặc null

        public DateTime? NgaySinh { get; set; }

        public string SDT { get; set; }
        public string Email { get; set; }
        public string QuocTich { get; set; }

        // Hiển thị ngày sinh đẹp trong View mà không sợ null
        public string NgaySinhFormatted => NgaySinh.HasValue
            ? NgaySinh.Value.ToString("dd/MM/yyyy")
            : "Chưa nhập";

        public string GioiTinhHienThi { get; set; }
        // Tổng tiền định dạng sẵn (dùng trong View không cần @String.Format nữa)
        public string TongTienFormatted => TongTienVe.ToString("N0") + "đ";
    }
}