using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class ChuyenBayModel
    {
        public string MaCB { get; set; }
        public string Hang { get; set; }
        public string MaMB { get; set; }  // Mã chuyến bay (MACB từ CHUYENBAY)
        public string DiemDi { get; set; }  // Tên sân bay đi (TENSB từ SANBAY qua SBDI trong LOTRINH)
        public string DiemDen { get; set; }  // Tên sân bay đến (TENSB từ SANBAY qua SBDEN trong LOTRINH)
        public int Gia { get; set; }  // Giá cơ sở (GIA_COSO từ HANGGHE_GIA)
        public string HangGhe { get; set; }  // Hạng ghế (HANGGHE từ HANGGHE_GIA, e.g., 'Phổ thông', 'Thương gia')
        public DateTime GioCatCanh { get; set; }  // GIOCATCANH từ LOTRINH
        public DateTime GioHaCanh { get; set; }  // GIOHACANH từ LOTRINH
        public TimeSpan TongThoiGianBay { get { return GioHaCanh - GioCatCanh; } }  // Tính toán: GioHaCanh - GioCatCanh

        public ChuyenBayModel()
        {
            
        }
    }
}