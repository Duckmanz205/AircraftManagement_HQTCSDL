using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class ChiTietGioHang
    {

        public string MaGH { get; set; }
        public string MaCB { get; set; }
        public List<TTKhachHang> Passengers { get; set; }
        public int totalPrice { get; set; }

    }
}