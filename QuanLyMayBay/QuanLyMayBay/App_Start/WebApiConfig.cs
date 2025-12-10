using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace QuanLyMayBay
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 1. Bật tính năng Attribute Routing (Quan trọng để dùng [Route("api/...")])
            config.MapHttpAttributeRoutes();

            // 2. Route mặc định cho API
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 3. (Tùy chọn) Xóa XML Formatter để trả về JSON mặc định trên trình duyệt
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}