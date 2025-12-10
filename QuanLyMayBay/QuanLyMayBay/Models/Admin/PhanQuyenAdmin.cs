using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyMayBay.Models; // Đảm bảo đã using namespace chứa DbContext

namespace QuanLyMayBay.Models.Admin
{
    public class PhanQuyenAdmin : ActionFilterAttribute
    {

        private readonly string[] _allowedRoles;

        public PhanQuyenAdmin(params string[] roles)
        {
            _allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 1. Lấy thông tin nhân viên từ Session
            var nhanVien = HttpContext.Current.Session["AdminUser"] as NHANVIEN;

            // 2. Nếu chưa đăng nhập -> Đá về trang đăng nhập
            if (nhanVien == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "User", action = "DangNhap" })
                );
                return;
            }
            bool coQuyen = false;

            using (var db = new QUANLYMAYBAYEntities())
            {
                try
                {
                    bool isPermissionGranted = db.Database.SqlQuery<bool>(
                        "SELECT dbo.fn_CheckAdminPermission(@p0)",
                        nhanVien.MANV
                    ).FirstOrDefault();

                    if (isPermissionGranted)
                    {
                        coQuyen = true;
                    }
                }
                catch (Exception)
                {
                    coQuyen = false;
                }
            }
            if (!coQuyen)
            {
                filterContext.Controller.TempData["Error"] = "Bạn không có quyền quản trị cấp cao (Admin) để truy cập!";

                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Admin", action = "TrangChu" })
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}