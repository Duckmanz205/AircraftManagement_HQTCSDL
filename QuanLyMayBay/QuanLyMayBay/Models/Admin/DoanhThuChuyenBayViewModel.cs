using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models.Admin
{
    /// <summary>
    /// ViewModel for flight revenue statistics from cursor-based stored procedure
    /// Maps to the result of: EXEC sp_ThongKeDoanhThuTheoChuyen
    /// </summary>
    public class DoanhThuChuyenBayViewModel
    {
        /// <summary>
        /// Flight code (e.g., "CB01", "CB02")
        /// </summary>
        public string MaChuyenBay { get; set; }

        /// <summary>
        /// Total revenue for this flight
        /// </summary>
        public decimal TongDoanhThu { get; set; }
    }
}
