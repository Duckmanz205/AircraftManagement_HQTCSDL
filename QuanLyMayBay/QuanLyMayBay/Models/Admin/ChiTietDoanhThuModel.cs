using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class ChiTietDoanhThuModel
    {
        public string MaChuyenBay { get; set; }
        public DateTime NgayBay { get; set; }
        public int SoVe { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal GiaTrungBinh { get; set; }
    }
}