using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyMayBay.Models.Admin
{
    public class PhanQuyenAdmin: ActionFilterAttribute
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

            // 3. Kiểm tra quyền
            // Nếu danh sách roles rỗng -> Cho phép tất cả nhân viên đăng nhập
            // Nếu có roles -> Kiểm tra MACV của nhân viên có nằm trong danh sách cho phép không
            bool coQuyen = false;
            if (_allowedRoles.Length == 0)
            {
                coQuyen = true;
            }
            else
            {
                foreach (var role in _allowedRoles)
                {
                    if (nhanVien.MACV.Trim() == role)
                    {
                        coQuyen = true;
                        break;
                    }
                }
            }

            // 4. Nếu không có quyền -> Chuyển hướng về trang báo lỗi hoặc trang chủ
            if (!coQuyen)
            {
                filterContext.Controller.TempData["Error"] = "Bạn không có quyền truy cập chức năng này!";
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Admin", action = "TrangChu" })
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}