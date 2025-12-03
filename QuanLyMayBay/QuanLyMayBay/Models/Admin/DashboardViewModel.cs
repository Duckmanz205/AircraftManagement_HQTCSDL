using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    public class DashboardViewModel
    {
        // 1. Số liệu cho các thẻ KPI
        public decimal DoanhThuHomNay { get; set; }
        public double PhanTramDoanhThu { get; set; } // So với hôm qua

        public int VeBanHomNay { get; set; }
        public double PhanTramVeBan { get; set; } // So với hôm qua

        public int ChuyenBayDangBay { get; set; }
        public int ChuyenBayDungGio { get; set; }

        public double TyLeTreChuyen { get; set; }
        public double PhanTramTreChuyen { get; set; } // So với hôm qua (giả định)

        // 2. Dữ liệu cho Biểu đồ (Charts)
        public List<string> ChartLabels { get; set; } // Ngày (cho biểu đồ doanh thu)
        public List<decimal> ChartRevenueData { get; set; } // Số tiền

        public List<string> TopFlightLabels { get; set; } // Tên chuyến bay
        public List<int> TopFlightData { get; set; } // Số lượng vé

        // 3. Danh sách phiếu đặt vé (Cũ)
        public List<QuanLyMayBay.Models.Admin.PhieuDatVeDisplay> DanhSachPhieuDat { get; set; }
    }

    public class PhieuDatVeDisplay
    {
        public string MAPHIEU { get; set; }
        public string MAVE { get; set; }
        public string TENKH { get; set; }
        public DateTime? NGAYDAT { get; set; }
        public decimal GIATIEN { get; set; }
        public string TRANGTHAI { get; set; }
    }

}