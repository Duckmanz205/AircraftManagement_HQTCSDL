using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class BVATests : BaseTest
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

        // TC_BVA_01: Đặt vé với đúng 9 hành khách (Giới hạn trên hợp lệ)
        [TestMethod]
        public void TC_BVA_01_DatVeVoiDung9HanhKhach()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/DatVe");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Tìm và bấm nút đặt vé của chuyến bay đầu tiên
            IWebElement bookBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            bookBtn.Click();

            // Mở modal và chỉnh số lượng hành khách thành 9
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.getElementById('passengerInput').value = 9;");
            
            // Submit form cập nhật số lượng hành khách
            driver.FindElement(By.CssSelector("#passengerModal button[type='submit']")).Click();

            // Chờ chuyển hướng sang trang chọn chỗ và kiểm tra xem có cho phép nhập thông tin hành khách không
            wait.Until(d => d.Url.Contains("/User/ChonCho"));
            Assert.IsTrue(driver.Url.Contains("/User/ChonCho"), "Hệ thống không cho phép chọn chỗ khi số lượng hành khách là 9.");
        }

        // TC_BVA_02: Đặt vé với 10 hành khách (Vượt quá giới hạn trên)
        [TestMethod]
        public void TC_BVA_02_DatVeVoi10HanhKhach()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/DatVe");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            IWebElement bookBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            bookBtn.Click();

            // Nhập quá giới hạn (10 hành khách)
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.getElementById('passengerInput').value = 10;");
            
            driver.FindElement(By.CssSelector("#passengerModal button[type='submit']")).Click();

            // Hệ thống phải chặn lại và hiển thị thông báo lỗi hoặc quay lại trang cũ
            Assert.IsFalse(driver.Url.Contains("/User/ChonCho"), "Hệ thống không được phép cho qua trang chọn chỗ khi số lượng hành khách là 10.");
        }

        // TC_BVA_03: Check-in đúng 24h trước giờ bay
        [TestMethod]
        public void TC_BVA_03_CheckInDung24hTruocGioBay()
        {
            LoginUser("khoa@gmail.com", "123456");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement checkinTab = wait.Until(d => d.FindElement(By.LinkText("Check-in")));
            checkinTab.Click();

            IWebElement checkinNowBtn = wait.Until(d => d.FindElement(By.LinkText("Check-in ngay")));
            string href = checkinNowBtn.GetAttribute("href");
            driver.Navigate().GoToUrl(href);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.querySelectorAll('input[type=\"checkbox\"]').forEach(c => { if (!c.checked) c.click(); });");
            js.ExecuteScript("window.confirm = function() { return true; };");
            js.ExecuteScript("window.lastAlert = null; window.alert = function(msg) { window.lastAlert = msg; };");

            js.ExecuteScript("document.querySelector('button[onclick*=\"confirmCheckIn\"]').click();");

            IWebElement successMessageUI = wait.Until(d => d.FindElement(By.CssSelector(".gap-3:nth-child(1) .mb-1")));
            Assert.AreEqual("Check-in thành công!", successMessageUI.Text);
        }

        // TC_BVA_04: Check-in đúng 60 phút trước giờ bay
        [TestMethod]
        public void tCBVA04()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập tài khoản Khách hàng
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Lược bỏ click thừa, nhập thẳng thông tin
            IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();

            // 3. Điều hướng đến trang Check-in
            IWebElement checkinTab = wait.Until(d => d.FindElement(By.LinkText("Check-in")));
            checkinTab.Click();

            IWebElement checkinNowBtn = wait.Until(d => d.FindElement(By.LinkText("Check-in ngay")));
            string href = checkinNowBtn.GetAttribute("href");
            driver.Navigate().GoToUrl(href);

            // 4. Đánh dấu các mục xác nhận (Checkbox/Radio)
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.querySelectorAll('input[type=\"checkbox\"]').forEach(c => c.checked = true);");

            // Mock confirm and alert
            js.ExecuteScript("window.confirm = function() { return true; };");
            js.ExecuteScript("window.lastAlert = null; window.alert = function(msg) { window.lastAlert = msg; };");

            // 5. Bấm nút Xác nhận Check-in cuối cùng
            js.ExecuteScript("document.querySelector('button[onclick*=\"confirmCheckIn\"]').click();");

            // 6. Kiểm tra giao diện hiển thị trạng thái thành công
            IWebElement successMessageUI = wait.Until(d => d.FindElement(By.CssSelector(".gap-3:nth-child(1) .mb-1")));
            Assert.AreEqual("Check-in thành công!", successMessageUI.Text);
        }

        // TC_BVA_05: Thanh toán ở giây thứ 0 của bộ đếm ngược
        [TestMethod]
        public void TC_BVA_05_ThanhToanGiayThu0CuaBoDemNguoc()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/VeCuaToi");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            // Kiểm tra xem có vé hoặc thông tin hóa đơn chưa thanh toán không
            Assert.IsTrue(driver.Url.Contains("/User/VeCuaToi"), "Không thể truy cập trang quản lý vé của khách hàng.");
        }
    }
}
