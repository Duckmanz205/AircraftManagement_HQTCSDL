using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuanLyMayBay.Controllers
{
    public class ChuyenBayAPI : ApiController
    {
        QUANLYMAYBAYEntities db = new QUANLYMAYBAYEntities();

        // API: Lấy danh sách tất cả chuyến bay
        // GET: api/ApiFlight/GetAllFlights
        [HttpGet]
        [Route("api/ChuyenBayAPI/GetAllFlights")]
        public IHttpActionResult GetAllFlights()
        {
            try
            {
                // Dùng Select để lấy dữ liệu cần thiết (DTO), tránh lỗi vòng lặp JSON
                var flights = db.CHUYENBAYs.Select(cb => new
                {
                    MaCB = cb.MACB,
                    TrangThai = cb.TRANGTHAI,
                    MayBay = cb.MAYBAY.TENMB, // Lấy tên máy bay từ bảng liên kết
                    Hang = cb.MAYBAY.HANG,

                    // Lấy thông tin lộ trình (nếu có)
                    LoTrinh = db.LOTRINHs
                        .Where(lt => lt.MACB == cb.MACB)
                        .Select(lt => new {
                            SanBayDi = lt.SBDI,
                            SanBayDen = lt.SBDEN,
                            GioCatCanh = lt.GIOCATCANH,
                            GioHaCanh = lt.GIOHACANH
                        }).FirstOrDefault()
                }).ToList();

                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }
    }
}
