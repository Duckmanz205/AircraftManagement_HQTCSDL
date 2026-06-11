using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class AuthTests : BaseTest
    {
        // TC_AUTH_01: Đăng ký thành công
        [TestMethod]
        public void TC_AUTH_01_DangKyThanhCong()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn Anh Tài");
            driver.FindElement(By.Id("email")).SendKeys("test_new_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc12345");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc12345");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/DangNhap"));
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống không chuyển về trang đăng nhập sau khi đăng ký.");
        }

        // TC_AUTH_02: Đăng ký - Email trùng
        [TestMethod]
        public void TC_AUTH_02_DangKyEmailTrung()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn B");
            driver.FindElement(By.Id("email")).SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc12345");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc12345");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsFalse(driver.Url.Contains("/User/DangNhap"), "Hệ thống không báo lỗi khi trùng email");
        }

        // TC_AUTH_03: Đăng nhập thành công với tài khoản hợp lệ
        [TestMethod]
        public void TC_AUTH_03_DangNhapThanhCong()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangNhap");
            driver.FindElement(By.Id("email")).SendKeys("john@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("johnpass");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));
            Assert.IsTrue(driver.Url.Contains("/User/TrangChu"), "Đăng nhập thành công nhưng hệ thống không chuyển trang.");
        }

        // TC_AUTH_04: Đăng nhập thất bại do sai mật khẩu
        [TestMethod]
        public void TC_AUTH_04_DangNhapSaiMatKhau()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("email")).SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("SaiMatKhau@123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống không ở lại trang đăng nhập khi sai mật khẩu.");
        }

        // TC_AUTH_05: Đăng nhập thất bại do sai tên đăng nhập (Tài khoản không tồn tại)
        [TestMethod]
        public void TC_AUTH_05_DangNhapSaiTenDangNhap()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("email")).SendKeys("user_khong_ton_tai");
            driver.FindElement(By.Id("password")).SendKeys("Abc12345");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống không ở lại trang đăng nhập khi tài khoản không tồn tại.");
        }

        // TC_AUTH_06: Đăng nhập thất bại do bỏ trống Tên đăng nhập
        [TestMethod]
        public void TC_AUTH_06_BoTrongTenDangNhap()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("password")).SendKeys("Abc12345");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống cho phép submit form khi bỏ trống tên đăng nhập.");
        }

        // TC_AUTH_07: Đăng nhập thất bại do bỏ trống Mật khẩu
        [TestMethod]
        public void TC_AUTH_07_BoTrongMatKhau()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("email")).SendKeys("khoa@gmail.com");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống cho phép submit form khi bỏ trống mật khẩu.");
        }

        // TC_AUTH_08: Đăng nhập thất bại do bỏ trống cả 2 trường
        [TestMethod]
        public void TC_AUTH_08_BoTrongCaHaiTruong()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống cho phép submit khi form rỗng.");
        }

        // TC_AUTH_09: Kiểm tra chức năng Quên mật khẩu (nếu có)
        //[TestMethod]
        //public void TC_AUTH_09_ChuyenHuongQuenMatKhau()
        //{
        //    driver.Navigate().GoToUrl(baseUrl + "User/DangNhap");
        //    driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
        //    driver.FindElement(By.XPath("//a[contains(text(), 'Quên mật khẩu?')]")).Click();
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        //    wait.Until(d => d.Url.Contains("/User/QuenMatKhau"));
        //    Assert.IsTrue(driver.Url.Contains("/User/QuenMatKhau"), "Link Quên mật khẩu không hoạt động.");
        //}

        // TC_AUTH_10: Kiểm tra liên kết chuyển sang tab/trang Đăng Ký
        [TestMethod]
        public void TC_AUTH_10_ChuyenHuongTrangDangKy()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.XPath("//span[contains(text(), 'Đăng ký ngay')] | //a[contains(text(), 'Đăng ký ngay')]")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.Url.Contains("/User/DangKy"));
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Nút chuyển hướng sang Đăng Ký không hoạt động.");
        }

        // TC_AUTH_11: Đăng xuất thành công
        [TestMethod]
        public void TC_AUTH_11_DangXuatThanhCong()
        {
            TC_AUTH_03_DangNhapThanhCong();
            driver.FindElement(By.CssSelector(".hover\\3A bg-gray-100")).Click();
            driver.FindElement(By.CssSelector(".hover\\3A bg-red-50")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.Url.TrimEnd('/') == baseUrl.TrimEnd('/'));
            Assert.IsTrue(driver.Url.TrimEnd('/') == baseUrl.TrimEnd('/'), "Không thể đăng xuất hoặc không chuyển về trang chủ.");
        }
    }
}