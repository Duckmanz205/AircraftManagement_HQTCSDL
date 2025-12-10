using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class AdminThongKeViewModel
    {
        // Thẻ Tóm tắt (Summary Cards)
        public decimal TongDoanhThu { get; set; }
        public int TongSoVeBan { get; set; }
        public int TongSoChuyenBay { get; set; }
        public decimal GiaVeTrungBinh { get; set; }

        // Dữ liệu Bảng (Data Table)
        public List<ChiTietDoanhThuModel> ChiTietDoanhThu { get; set; } = new List<ChiTietDoanhThuModel>();
    }
}