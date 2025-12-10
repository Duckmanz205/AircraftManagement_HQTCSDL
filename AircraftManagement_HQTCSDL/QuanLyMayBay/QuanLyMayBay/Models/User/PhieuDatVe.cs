using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class PhieuDatVe
    {
        public string MAPHIEU { get; set; }
        public string MAVE { get; set; }
        public string TENKH { get; set; }
        public DateTime? NGAYDAT { get; set; }
        public int? GIATIEN { get; set; }
        public string TRANGTHAI { get; set; }
    }
}