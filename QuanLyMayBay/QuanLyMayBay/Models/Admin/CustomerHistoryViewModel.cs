using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    public class CustomerHistoryViewModel
    {
        public string TenKhachHang { get; set; }
        public List<LichSuVeDTO> DanhSachVe { get; set; }
    }
}