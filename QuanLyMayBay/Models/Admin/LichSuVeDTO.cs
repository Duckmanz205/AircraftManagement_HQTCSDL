using System;

namespace QuanLyMayBay.Models.Admin
{
    public class LichSuVeDTO
    {
        public string MaVe { get; set; }
        public string NgayDat { get; set; }
        public string MaCB { get; set; }
        public string HanhTrinh { get; set; }
        public string NgayBay { get; set; }
        public int GiaTien { get; set; }
        public string TrangThai { get; set; }
        public DateTime? _NgayDatGoc { get; set; }
        public DateTime _NgayBayGoc { get; set; }
    }
}