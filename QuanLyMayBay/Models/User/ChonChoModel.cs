using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class ChonChoModel
    {
        public string MaCB { get; set; }
        public string MaMB { get; set; }
        public string Hang { get; set; }
        public string DiemDi { get; set; }
        public string DiemDen { get; set; }
        public DateTime GioCatCanh { get; set; }
        public DateTime GioHaCanh { get; set; }
        public int SoHanhKhach { get; set; }
        public List<string> GheDaDat { get; set; }
        public List<CAUHINH_GHE> CauHinhGhe { get; set; }
        public Dictionary<string, decimal> GiaGhe { get; set; }
        public string MaGH { get; set; } // Giỏ hàng
        public List<GHE> ListGhe { get; set; } // Thêm: Danh sách ghế thực từ DB (GHE entity)
    }
}