using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    public class KhachHangViewModel
    {
        public string MAKH { get; set; }
        public string TENKH { get; set; }
        public string EMAIL { get; set; }
        public string QUOCGIA { get; set; }
        public int SoVeDaDat { get; set; }
    }
}