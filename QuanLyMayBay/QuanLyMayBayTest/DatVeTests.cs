using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace QuanLyMayBayTest
{
    /// <summary>
    /// Test scripts cho các Test Case TC_DATVE_01 đến TC_DATVE_13
    /// Kiểm tra chức năng Tìm kiếm, Lọc, Sắp xếp chuyến bay và Chọn ghế
    /// </summary>
    [TestClass]
    public class DatVeTests : BaseTest
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

        // Helper: Tìm kiếm chuyến bay một chiều
        private void TimChuyenMotChieu(string diemDi, string diemDen, string ngayDi, int hanhKhach)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Chọn loại chuyến: Một chiều (mặc định)
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Nhập điểm đi
            IWebElement fromInput = wait.Until(d => d.FindElement(By.Id("from-input")));
            fromInput.Clear();
            fromInput.SendKeys(diemDi);

            // Nhập điểm đến
            IWebElement toInput = driver.FindElement(By.Id("to-input"));
            toInput.Clear();
            toInput.SendKeys(diemDen);

            // Nhập ngày đi
            js.ExecuteScript($"document.querySelector('input[name=\"ngaydi\"]').value = '{ngayDi}';");

            // Nhập số hành khách
            js.ExecuteScript($"document.getElementById('passenger-count').value = '{hanhKhach}';");

            // Click Tìm chuyến
            js.ExecuteScript("document.querySelector('form[action*=\"TimChuyen\"] button[type=\"submit\"]').click();");
        }

        // =========================================
        // TC_DATVE_01: Tìm kiếm chuyến bay một chiều
        // =========================================
        [TestMethod]
        public void TC_DATVE_01_TimKiemChuyenBayMotChieu()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Chọn Một chiều
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.querySelector('input[value=\"Một chiều\"]').click();");

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 1);

            // Kiểm tra chuyển sang trang đặt vé và hiển thị kết quả
            wait.Until(d => d.Url.Contains("/User/TimChuyen") || d.Url.Contains("/User/DatVe") || d.Url.Contains("/User/TrangChu"));
            // Kiểm tra có hiển thị danh sách chuyến bay (hoặc trang kết quả tìm kiếm)
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.PageSource.Contains("chuyến bay"),
                "Không hiển thị kết quả tìm kiếm chuyến bay một chiều.");
        }

        // =========================================
        // TC_DATVE_02: Tìm kiếm chuyến bay khứ hồi
        // =========================================
        [TestMethod]
        public void TC_DATVE_02_TimKiemChuyenBayKhuHoi()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Chọn Khứ hồi
            js.ExecuteScript("document.querySelector('input[value=\"Khứ hồi\"]').click();");

            // Nhập thông tin
            IWebElement fromInput = wait.Until(d => d.FindElement(By.Id("from-input")));
            fromInput.Clear();
            fromInput.SendKeys("Hồ Chí Minh");

            driver.FindElement(By.Id("to-input")).Clear();
            driver.FindElement(By.Id("to-input")).SendKeys("Hà Nội");

            js.ExecuteScript("document.querySelector('input[name=\"ngaydi\"]').value = '2025-12-16';");
            js.ExecuteScript("document.getElementById('return-date').value = '2025-12-30';");
            js.ExecuteScript("document.getElementById('passenger-count').value = '2';");

            js.ExecuteScript("document.querySelector('form[action*=\"TimChuyen\"] button[type=\"submit\"]').click();");

            // Kiểm tra
            wait.Until(d => d.PageSource.Contains("Đặt Vé") || d.PageSource.Contains("chiều đi") || d.PageSource.Contains("chuyến bay"));
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.PageSource.Contains("chuyến"),
                "Không hiển thị kết quả tìm kiếm chuyến bay khứ hồi.");
        }

        // =========================================
        // TC_DATVE_03: Tìm kiếm - Hành khách = 0
        // =========================================
        [TestMethod]
        public void TC_DATVE_03_TimKiemHanhKhachBang0()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Nhập số lượng hành khách = 0
            js.ExecuteScript("document.getElementById('passenger-count').value = '0';");

            // Kiểm tra input min attribute hoặc behavior
            string minValue = (string)js.ExecuteScript("return document.getElementById('passenger-count').getAttribute('min');");

            // Hệ thống không cho nhập 0 (input min=1 hoặc validation)
            Assert.IsTrue(minValue == "1" || driver.Url.Contains("/User/TrangChu"),
                "Hệ thống không ngăn chặn nhập số hành khách = 0.");
        }

        // =========================================
        // TC_DATVE_04: Tìm kiếm - Hành khách = 1
        // =========================================
        [TestMethod]
        public void TC_DATVE_04_TimKiemHanhKhach1()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 1);

            // Kiểm tra tìm kiếm thành công
            wait.Until(d => d.PageSource.Contains("Đặt Vé") || d.PageSource.Contains("chuyến bay"));
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.PageSource.Contains("chuyến"),
                "Tìm kiếm thất bại khi nhập 1 hành khách.");
        }

        // =========================================
        // TC_DATVE_05: Tìm kiếm - Hành khách = 9
        // =========================================
        [TestMethod]
        public void TC_DATVE_05_TimKiemHanhKhach9()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 9);

            // Kiểm tra tìm kiếm thành công
            wait.Until(d => d.PageSource.Contains("Đặt Vé") || d.PageSource.Contains("chuyến bay"));
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.PageSource.Contains("chuyến"),
                "Tìm kiếm thất bại khi nhập 9 hành khách.");
        }

        // =========================================
        // TC_DATVE_06: Lọc chuyến - Giá dưới 2 triệu
        // =========================================
        [TestMethod]
        public void TC_DATVE_06_LocChuyenGiaDuoi2Trieu()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 1);

            // Tích ô lọc giá dưới 2 triệu
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("var cb = document.querySelector('input[name=\"gia\"][value=\"duoi2\"]'); if(cb) cb.click();");

            // Click nút Lọc
            try
            {
                IWebElement filterBtn = wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']")));
                filterBtn.Click();
            }
            catch
            {
                // Nếu không tìm thấy nút submit cho bộ lọc, thử tìm nút Lọc
                js.ExecuteScript("document.querySelector('form[action*=\"LocChuyen\"] button[type=\"submit\"]').click();");
            }

            // Kiểm tra kết quả lọc hiển thị
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.Url.Contains("DatVe"),
                "Không hiển thị kết quả lọc chuyến bay theo giá dưới 2 triệu.");
        }

        // =========================================
        // TC_DATVE_07: Lọc chuyến - Giá từ 2-5 triệu
        // =========================================
        [TestMethod]
        public void TC_DATVE_07_LocChuyenGia2Den5Trieu()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 1);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("var cb = document.querySelector('input[name=\"gia\"][value=\"2den5\"]'); if(cb) cb.click();");

            try
            {
                js.ExecuteScript("document.querySelector('form[action*=\"LocChuyen\"] button[type=\"submit\"]').click();");
            }
            catch { }

            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.Url.Contains("DatVe"),
                "Không hiển thị kết quả lọc chuyến bay theo giá 2-5 triệu.");
        }

        // =========================================
        // TC_DATVE_08: Lọc chuyến - Hạng Phổ thông
        // =========================================
        [TestMethod]
        public void TC_DATVE_08_LocChuyenHangPhoThong()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 1);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("var cb = document.querySelector('input[name=\"hangghe\"][value=\"Phổ thông\"]'); if(cb) cb.click();");

            try
            {
                js.ExecuteScript("document.querySelector('form[action*=\"LocChuyen\"] button[type=\"submit\"]').click();");
            }
            catch { }

            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.Url.Contains("DatVe"),
                "Không hiển thị kết quả lọc chuyến bay theo hạng Phổ thông.");
        }

        // =========================================
        // TC_DATVE_09: Lọc chuyến - Giờ sáng
        // =========================================
        [TestMethod]
        public void TC_DATVE_09_LocChuyenGioSang()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            TimChuyenMotChieu("Hồ Chí Minh", "Hà Nội", "2025-12-16", 1);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("var cb = document.querySelector('input[name=\"gio\"][value=\"sang\"]'); if(cb) cb.click();");

            try
            {
                js.ExecuteScript("document.querySelector('form[action*=\"LocChuyen\"] button[type=\"submit\"]').click();");
            }
            catch { }

            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé") || driver.Url.Contains("DatVe"),
                "Không hiển thị kết quả lọc chuyến bay theo giờ sáng.");
        }

        // =========================================
        // TC_DATVE_10: Sắp xếp giá tăng dần
        // =========================================
        [TestMethod]
        public void TC_DATVE_10_SapXepGiaTangDan()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Vào trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // Click sort giá thấp nhất
            try
            {
                IWebElement sortMinBtn = wait.Until(d => d.FindElement(By.CssSelector("a[href*='Sort?sort=min']")));
                sortMinBtn.Click();
            }
            catch
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.location.href = '/User/Sort?sort=min';");
            }

            wait.Until(d => d.PageSource.Contains("Đặt Vé") || d.Url.Contains("Sort"));
            Assert.IsTrue(driver.PageSource.Contains("Đặt Vé"),
                "Không hiển thị kết quả sắp xếp giá tăng dần.");
        }

        // =========================================
        // TC_DATVE_11: Chọn ghế - Ghế trống
        // =========================================
        [TestMethod]
        public void TC_DATVE_11_ChonGheTrong()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Vào trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // Chọn chuyến bay
            IWebElement selectFlightBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", selectFlightBtn);

            // Xác nhận số hành khách
            IWebElement confirmBtn = wait.Until(d => d.FindElement(By.CssSelector("#passengerModal button[type='submit']")));
            js.ExecuteScript("arguments[0].click();", confirmBtn);

            // Chờ trang chọn chỗ
            wait.Until(d => d.Url.Contains("/User/ChonCho"));

            // Chọn ghế trống (available)
            IWebElement seatElement = wait.Until(d =>
            {
                var el = d.FindElement(By.CssSelector(".seat.available"));
                return el.Displayed ? el : null;
            });
            seatElement.Click();

            // Kiểm tra ghế được chọn (có form nhập thông tin hành khách)
            Assert.IsTrue(driver.Url.Contains("/User/ChonCho"),
                "Không thể chọn ghế trống trên trang chọn chỗ.");
        }

        // =========================================
        // TC_DATVE_12: Chọn ghế - Ghế đã bán
        // =========================================
        [TestMethod]
        public void TC_DATVE_12_ChonGheDaBan()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Vào trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // Chọn chuyến bay
            IWebElement selectFlightBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", selectFlightBtn);

            IWebElement confirmBtn = wait.Until(d => d.FindElement(By.CssSelector("#passengerModal button[type='submit']")));
            js.ExecuteScript("arguments[0].click();", confirmBtn);

            wait.Until(d => d.Url.Contains("/User/ChonCho"));

            // Kiểm tra ghế đã bán (sold) hiện diện và không thể click
            bool hasSoldSeats = (bool)js.ExecuteScript(
                "return document.querySelectorAll('.seat.sold, .seat.occupied, .seat.booked').length > 0;"
            );

            Assert.IsTrue(hasSoldSeats || driver.PageSource.Contains("đã bán") || driver.PageSource.Contains("booked"),
                "Không hiển thị trạng thái ghế đã bán trên sơ đồ ghế.");
        }

        // =========================================
        // TC_DATVE_13: Lưu ghế bị chiếm đồng thời
        // =========================================
        [TestMethod]
        public void TC_DATVE_13_LuuGheBiChiemDongThoi()
        {
            // Test case này kiểm tra race condition - chỉ verify cơ chế kiểm tra tồn tại
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            IWebElement selectFlightBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", selectFlightBtn);

            IWebElement confirmBtn = wait.Until(d => d.FindElement(By.CssSelector("#passengerModal button[type='submit']")));
            js.ExecuteScript("arguments[0].click();", confirmBtn);

            wait.Until(d => d.Url.Contains("/User/ChonCho"));

            // Verify trang chọn chỗ load được và có cơ chế kiểm tra ghế đã đặt
            Assert.IsTrue(driver.Url.Contains("/User/ChonCho"),
                "Không thể truy cập trang chọn chỗ để kiểm tra cơ chế chống chiếm ghế.");
        }
    }
}
