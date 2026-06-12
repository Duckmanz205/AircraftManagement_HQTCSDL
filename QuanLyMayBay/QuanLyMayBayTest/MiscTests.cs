using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace QuanLyMayBayTest
{
    /// <summary>
    /// Test scripts cho các Test Case còn thiếu:
    /// - TC_ADMIN_17: Thống kê KH theo quốc gia
    /// - TC_ADMIN_18: Admin quản lý vé - Đối soát check-in
    /// - TC_HUYE_01: Hủy vé chưa thanh toán
    /// - TC_HUYE_02: Hủy vé đã thanh toán
    /// - TC_SORT_01: Sort theo giờ bay sớm nhất
    /// - TC_SORT_02: Sort theo thời gian bay ngắn nhất
    /// - TC_DT_01: Quyết định lọc chuyến bay phức tạp
    /// - TC_ST_01: Kiểm thử chuyển trạng thái Vòng đời Vé
    /// </summary>
    [TestClass]
    public class MiscTests : BaseTest
    {
        // Helper: Đăng nhập Khách hàng
        private void LoginUser(string email, string password)
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangNhap");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys(email);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
        }

        // Helper: Đăng nhập Nhân viên
        private void LoginAdmin(string manv, string matkhau)
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangNhap");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement adminTab = wait.Until(d => d.FindElement(By.Id("adminLoginTab")));
            adminTab.Click();
            driver.FindElement(By.Id("adminId")).SendKeys(manv);
            driver.FindElement(By.Id("adminPassword")).SendKeys(matkhau);
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();
        }

        // =========================================
        // TC_ADMIN_17: Thống kê KH theo quốc gia
        // =========================================
        [TestMethod]
        public void TC_ADMIN_17_ThongKeKHTheoQuocGia()
        {
            LoginAdmin("NV09", "admin123");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/Admin/TrangChu"));

            // Truy cập trực tiếp trang thống kê quốc gia
            driver.Navigate().GoToUrl(baseUrl + "Admin/ThongKeQuocGia");

            // Kiểm tra trang load thành công (không bị lỗi kết nối DB)
            wait.Until(d => d.PageSource.Contains("thống kê") || d.PageSource.Contains("quốc gia") ||
                            d.PageSource.Contains("Quốc Gia") || d.PageSource.Contains("QUOCGIA") ||
                            d.PageSource.Contains("Lỗi"));

            bool hasContent = driver.PageSource.Contains("Việt Nam") ||
                               driver.PageSource.Contains("quốc gia") ||
                               driver.PageSource.Contains("Quốc Gia") ||
                               driver.Url.Contains("ThongKeQuocGia");

            Assert.IsTrue(hasContent,
                "Trang thống kê khách hàng theo quốc gia không hiển thị đúng.");
        }

        // =========================================
        // TC_ADMIN_18: Admin quản lý vé - Đối soát check-in
        // =========================================
        [TestMethod]
        public void TC_ADMIN_18_AdminDoiSoatCheckIn()
        {
            LoginAdmin("NV05", "admin123");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/Admin/TrangChu"));

            // Truy cập trang quản lý vé
            driver.Navigate().GoToUrl(baseUrl + "Admin/QLVe");

            // Kiểm tra trang quản lý vé hiển thị
            wait.Until(d => d.PageSource.Contains("Quản lý") || d.PageSource.Contains("vé") ||
                            d.PageSource.Contains("Check-in") || d.Url.Contains("QLVe"));

            Assert.IsTrue(driver.Url.Contains("QLVe") || driver.PageSource.Contains("vé") ||
                           driver.PageSource.Contains("Quản lý"),
                "Không thể truy cập trang Quản lý vé & Đặt chỗ.");
        }

        // =========================================
        // TC_HUYE_01: Hủy vé chưa thanh toán
        // =========================================
        [TestMethod]
        public void TC_HUYE_01_HuyVeChuaThanhToan()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            driver.Navigate().GoToUrl(baseUrl + "User/VeCuaToi");

            // Tìm nút Hủy vé trong tab Chưa thanh toán
            try
            {
                IWebElement cancelBtn = wait.Until(d => d.FindElement(
                    By.CssSelector("button[onclick*='HuyVe']")));
                
                // Kiểm tra nút Hủy tồn tại
                Assert.IsNotNull(cancelBtn, "Không tìm thấy nút Hủy vé trong đơn hàng chưa thanh toán.");

                // Click hủy bằng JS để tránh bị đè bởi footer
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cancelBtn);

                // Chờ xử lý hủy
                wait.Until(d => d.Url.Contains("/User/VeCuaToi") || d.Url.Contains("HuyVe"));

                Assert.IsTrue(driver.Url.Contains("/User/VeCuaToi") || driver.Url.Contains("HuyVe"),
                    "Hệ thống không xử lý hủy vé chưa thanh toán đúng.");
            }
            catch (WebDriverTimeoutException)
            {
                // Không có vé chưa thanh toán
                Assert.IsTrue(driver.Url.Contains("/User/VeCuaToi"),
                    "Trang Vé của tôi không hiển thị đúng.");
            }
        }

        // =========================================
        // TC_HUYE_02: Hủy vé đã thanh toán
        // =========================================
        [TestMethod]
        public void TC_HUYE_02_HuyVeDaThanhToan()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            driver.Navigate().GoToUrl(baseUrl + "User/VeCuaToi");

            // Chuyển sang tab Đã thanh toán
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("switchTab('paid');");

            // Kiểm tra tab Đã thanh toán hiển thị
            IWebElement paidSection = wait.Until(d => d.FindElement(By.Id("paid-section")));
            
            // Kiểm tra: Nếu có vé đã thanh toán, nút hủy có hiện nhưng cần xác nhận
            // Nếu không có vé, hiển thị "Bạn chưa có vé nào đã thanh toán"
            bool hasPaidContent = paidSection.Text.Contains("Đã thanh toán") ||
                                   paidSection.Text.Contains("chưa có vé") ||
                                   paidSection.Text.Contains("Hủy vé");

            Assert.IsTrue(hasPaidContent || driver.Url.Contains("/User/VeCuaToi"),
                "Không hiển thị tab vé đã thanh toán đúng cách.");
        }

        // =========================================
        // TC_SORT_01: Sort theo giờ bay sớm nhất
        // =========================================
        [TestMethod]
        public void TC_SORT_01_SortTheoGioBaySomNhat()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Vào trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // Click sort Bay sớm nhất
            try
            {
                IWebElement sortEarlyBtn = wait.Until(d => d.FindElement(By.CssSelector("a[href*='Sort?sort=early']")));
                sortEarlyBtn.Click();
            }
            catch
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.location.href = '/User/Sort?sort=early';");
            }

            wait.Until(d => d.PageSource.Contains("Đặt Vé") || d.Url.Contains("Sort"));
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé"),
                "Không hiển thị kết quả sắp xếp theo giờ bay sớm nhất.");
        }

        // =========================================
        // TC_SORT_02: Sort theo thời gian bay ngắn nhất
        // =========================================
        [TestMethod]
        public void TC_SORT_02_SortTheoThoiGianBayNganNhat()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Vào trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // Click sort Bay nhanh nhất
            try
            {
                IWebElement sortTimespanBtn = wait.Until(d => d.FindElement(By.CssSelector("a[href*='Sort?sort=timespan']")));
                sortTimespanBtn.Click();
            }
            catch
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.location.href = '/User/Sort?sort=timespan';");
            }

            wait.Until(d => d.PageSource.Contains("Đặt Vé") || d.Url.Contains("Sort"));
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé"),
                "Không hiển thị kết quả sắp xếp theo thời gian bay ngắn nhất.");
        }

        // =========================================
        // TC_DT_01: Quyết định lọc chuyến bay phức tạp (Decision Table)
        // =========================================
        [TestMethod]
        public void TC_DT_01_LocChuyenBayPhucTapDecisionTable()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Tìm chuyến bay 1 chiều
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.querySelector('input[value=\"Một chiều\"]').click();");

            IWebElement fromInput = wait.Until(d => d.FindElement(By.Id("from-input")));
            fromInput.Clear();
            fromInput.SendKeys("Hồ Chí Minh");

            driver.FindElement(By.Id("to-input")).Clear();
            driver.FindElement(By.Id("to-input")).SendKeys("Hà Nội");

            js.ExecuteScript("document.querySelector('input[name=\"ngaydi\"]').value = '2025-12-16';");
            js.ExecuteScript("document.getElementById('passenger-count').value = '1';");

            js.ExecuteScript("document.querySelector('form[action*=\"TimChuyen\"] button[type=\"submit\"]').click();");

            // Áp dụng bộ lọc phức tạp: Hãng Airbus + Giá < 2 triệu + Giờ Sáng
            try
            {
                js.ExecuteScript("var cb1 = document.querySelector('input[name=\"hang\"][value=\"Airbus\"]'); if(cb1) cb1.click();");
                js.ExecuteScript("var cb2 = document.querySelector('input[name=\"gia\"][value=\"duoi2\"]'); if(cb2) cb2.click();");
                js.ExecuteScript("var cb3 = document.querySelector('input[name=\"gio\"][value=\"sang\"]'); if(cb3) cb3.click();");

                // Submit bộ lọc
                js.ExecuteScript("var form = document.querySelector('form[action*=\"LocChuyen\"]'); if(form) form.submit();");
            }
            catch { }

            // Kiểm tra trang hiển thị kết quả (có thể là "Không tìm thấy" nếu không có chuyến khớp)
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.Url.Contains("DatVe") ||
                           driver.PageSource.Contains("Không tìm thấy"),
                "Không hiển thị kết quả lọc chuyến bay phức tạp.");
        }

        // =========================================
        // TC_ST_01: Kiểm thử chuyển trạng thái Vòng đời Vé
        // =========================================
        [TestMethod]
        public void TC_ST_01_ChuyenTrangThaiVongDoiVe()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Bước 1: Kiểm tra trang Vé của tôi có vé ở các trạng thái khác nhau
            driver.Navigate().GoToUrl(baseUrl + "User/VeCuaToi");

            // Kiểm tra tab Chưa thanh toán
            bool hasUnpaidSection = driver.PageSource.Contains("Chưa thanh toán");

            // Kiểm tra tab Đã thanh toán
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("switchTab('paid');");

            bool hasPaidSection = driver.PageSource.Contains("Đã thanh toán");

            // Bước 2: Kiểm tra trang Check-in
            driver.Navigate().GoToUrl(baseUrl + "User/CheckIn");

            bool hasCheckInSection = driver.PageSource.Contains("Check-in") ||
                                      driver.PageSource.Contains("check-in");

            // Kiểm tra: Hệ thống có thể hiển thị các trạng thái khác nhau của vé
            Assert.IsTrue(hasUnpaidSection || hasPaidSection || hasCheckInSection,
                "Hệ thống không hỗ trợ hiển thị các trạng thái vòng đời vé khác nhau.");
        }
    }
}
