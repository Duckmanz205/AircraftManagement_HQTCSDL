using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class AuthTests : BaseTest
    {
        // Helper: Thực hiện đăng nhập khách hàng
        private void LoginUser(string email, string password)
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangNhap");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys(email);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
        }

        // Helper: Thực hiện đăng nhập nhân viên/admin
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

        // TC_AUTH_01: Đăng ký thành công
        [TestMethod]
        public void TC_AUTH_01_DangKyThanhCong()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn Anh Tài");
            driver.FindElement(By.Id("email")).SendKeys("test_new_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc123");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/DangNhap"));
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống không chuyển về trang đăng nhập sau khi đăng ký thành công.");
        }

        // TC_AUTH_02: Đăng ký - Email trùng
        [TestMethod]
        public void TC_AUTH_02_DangKyEmailTrung()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn B");
            driver.FindElement(By.Id("email")).SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc123");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Hệ thống không chặn đăng ký khi trùng email.");
        }

        // TC_AUTH_03: Đăng ký - Bỏ trống họ tên
        [TestMethod]
        public void TC_AUTH_03_DangKyBoTrongHoTen()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("email")).SendKeys("test_auth03_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc123");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Hệ thống vẫn cho submit khi bỏ trống họ tên.");
        }

        // TC_AUTH_04: Đăng ký - Mật khẩu < 6 ký tự
        [TestMethod]
        public void TC_AUTH_04_DangKyMatKhauDuoi6KyTu()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("test_auth04_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Ab123"); // 5 ký tự
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Ab123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Hệ thống không báo lỗi khi mật khẩu dưới 6 ký tự.");
        }

        // TC_AUTH_05: Đăng ký - Mật khẩu = 6 ký tự
        [TestMethod]
        public void TC_AUTH_05_DangKyMatKhauExactly6KyTu()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("test_auth05_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Ab1234"); // Đúng 6 ký tự
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Ab1234");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/DangNhap"));
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Mật khẩu 6 ký tự là hợp lệ nhưng đăng ký thất bại.");
        }

        // TC_AUTH_06: Đăng ký - Email sai định dạng
        [TestMethod]
        public void TC_AUTH_06_DangKyEmailSaiDinhDang()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("email_khong_co_a_cong");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc123");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Hệ thống không chặn email sai định dạng.");
        }

        // TC_AUTH_07: Đăng ký - SĐT sai định dạng
        [TestMethod]
        public void TC_AUTH_07_DangKySDTSaiDinhDang()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("test_auth07_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("091234"); // 6 số
            driver.FindElement(By.Id("password")).SendKeys("Abc123");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Hệ thống không bắt lỗi số điện thoại ngắn hơn 10 số.");
        }

        // TC_AUTH_08: Đăng nhập User thành công
        [TestMethod]
        public void TC_AUTH_08_DangNhapUserThanhCong()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));
            Assert.IsTrue(driver.Url.Contains("/User/TrangChu"), "Đăng nhập thành công nhưng không chuyển về TrangChu.");
        }

        // TC_AUTH_09: Đăng nhập - Sai mật khẩu
        [TestMethod]
        public void TC_AUTH_09_DangNhapSaiMatKhau()
        {
            LoginUser("khoa@gmail.com", "SaiMk999");
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống không ở lại trang đăng nhập khi sai mật khẩu.");
        }

        // TC_AUTH_10: Đăng nhập - Email không tồn tại
        [TestMethod]
        public void TC_AUTH_10_DangNhapEmailKhongTonTai()
        {
            LoginUser("khongton@gmail.com", "123456");
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống không chặn đăng nhập với email không tồn tại.");
        }

        // TC_AUTH_11: Đăng nhập Admin - CV05 thành công
        [TestMethod]
        public void TC_AUTH_11_DangNhapAdminCV05ThanhCong()
        {
            LoginAdmin("NV05", "admin123");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/Admin/TrangChu"));
            Assert.IsTrue(driver.Url.Contains("/Admin/TrangChu"), "Admin CV05 đăng nhập hợp lệ nhưng không chuyển đến Admin/TrangChu.");
        }

        // TC_AUTH_12: Đăng nhập Admin - CV09 thành công
        [TestMethod]
        public void TC_AUTH_12_DangNhapAdminCV09ThanhCong()
        {
            LoginAdmin("NV09", "admin123");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/Admin/TrangChu"));
            Assert.IsTrue(driver.Url.Contains("/Admin/TrangChu"), "Admin CV09 đăng nhập hợp lệ nhưng không chuyển đến Admin/TrangChu.");
        }

        // TC_AUTH_13: Đăng nhập Admin - CV01 bị từ chối
        [TestMethod]
        public void TC_AUTH_13_DangNhapAdminCV01BiTuChoi()
        {
            LoginAdmin("NV01", "123456");
            // Nhân viên CV01 không có quyền truy cập trang quản trị Admin
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap") || !driver.Url.Contains("/Admin/TrangChu"), "Nhân viên CV01 không được phép vào trang Admin.");
        }

        // TC_AUTH_14: Đổi mật khẩu thành công
        [TestMethod]
        public void TC_AUTH_14_DoiMatKhauThanhCong()
        {
            // 1. Setup & Đổi mật khẩu
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/HoSo");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                IWebElement btnOpenModal = wait.Until(d => d.FindElement(By.XPath("//button[contains(@onclick, 'openPasswordModal')]")));
                btnOpenModal.Click();

                wait.Until(d => d.FindElement(By.Id("currentPassword"))).SendKeys("123456");
                driver.FindElement(By.Id("newPassword")).SendKeys("1234567");
                driver.FindElement(By.Id("confirmPassword")).SendKeys("1234567");
                driver.FindElement(By.CssSelector("#passwordForm button[type='submit']")).Click();

                IWebElement successMsg = wait.Until(d => d.FindElement(By.CssSelector("#passwordForm p.text-green-600")));

                // NẾU ASSERT NÀY FAIL, CODE SẼ NHẢY THẲNG XUỐNG KHỐI FINALLY
                Assert.IsTrue(successMsg.Text.Contains("Đổi mật khẩu thành công!"), "Không hiển thị thông báo đổi mật khẩu thành công.");
            }
            finally
            {
                // 2. Teardown: Đảm bảo luôn đổi lại mật khẩu cũ dù Test Pass hay Fail
                // Lưu ý: Cần refresh lại trang hoặc xử lý UI để đảm bảo form sẵn sàng nhập lại
                driver.Navigate().GoToUrl(baseUrl + "User/HoSo");
                var btnOpenModal = wait.Until(d => d.FindElement(By.XPath("//button[contains(@onclick, 'openPasswordModal')]")));
                btnOpenModal.Click();
                wait.Until(d => d.FindElement(By.Id("currentPassword"))).SendKeys("1234567");
                driver.FindElement(By.Id("newPassword")).SendKeys("123456");
                driver.FindElement(By.Id("confirmPassword")).SendKeys("123456");
                driver.FindElement(By.CssSelector("#passwordForm button[type='submit']")).Click();
            }
        }

        // TC_AUTH_15: Đổi mật khẩu - Mật khẩu hiện tại sai
        [TestMethod]
        public void TC_AUTH_15_DoiMatKhauMatKhauHienTaiSai()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/HoSo");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnOpenModal = wait.Until(d => d.FindElement(By.XPath("//button[contains(@onclick, 'openPasswordModal')]")));
            btnOpenModal.Click();
            
            wait.Until(d => d.FindElement(By.Id("currentPassword"))).SendKeys("SaiMKMoiDung");
            driver.FindElement(By.Id("newPassword")).SendKeys("NewPass7");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("NewPass7");
            driver.FindElement(By.CssSelector("#passwordForm button[type='submit']")).Click();
            
            IWebElement errorMsg = wait.Until(d => d.FindElement(By.CssSelector("#passwordForm p.text-red-600")));
            Assert.IsTrue(errorMsg.Text.Contains("Mật khẩu hiện tại không chính xác."), "Hệ thống không báo lỗi khi mật khẩu cũ bị sai.");
        }

        // TC_AUTH_16: Đổi mật khẩu - Xác nhận không khớp
        [TestMethod]
        public void TC_AUTH_16_DoiMatKhauXacNhanKhongKhop()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/HoSo");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnOpenModal = wait.Until(d => d.FindElement(By.XPath("//button[contains(@onclick, 'openPasswordModal')]")));
            btnOpenModal.Click();
            
            wait.Until(d => d.FindElement(By.Id("currentPassword"))).SendKeys("123456");
            driver.FindElement(By.Id("newPassword")).SendKeys("NewPass7");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("DiffPass8");
            driver.FindElement(By.CssSelector("#passwordForm button[type='submit']")).Click();
            
            IWebElement errorMsg = wait.Until(d => d.FindElement(By.Id("confirmPasswordError")));
            Assert.IsTrue(errorMsg.Text.Contains("Mật khẩu xác nhận không khớp"), "Hệ thống không kiểm tra mật khẩu xác nhận.");
        }

        // TC_AUTH_17: Đổi mật khẩu - Mật khẩu mới < 6 ký tự
        [TestMethod]
        public void TC_AUTH_17_DoiMatKhauMatKhauMoiNgan()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/HoSo");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement btnOpenModal = wait.Until(d => d.FindElement(By.XPath("//button[contains(@onclick, 'openPasswordModal')]")));
            btnOpenModal.Click();
            
            wait.Until(d => d.FindElement(By.Id("currentPassword"))).SendKeys("123456");
            driver.FindElement(By.Id("newPassword")).SendKeys("Ab123");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Ab123");
            driver.FindElement(By.CssSelector("#passwordForm button[type='submit']")).Click();
            
            IWebElement errorMsg = wait.Until(d => d.FindElement(By.Id("newPasswordError")));
            Assert.IsTrue(errorMsg.Text.Contains("Mật khẩu phải ít nhất 6 ký tự"), "Hệ thống không giới hạn độ dài mật khẩu mới tối thiểu 6 ký tự.");
        }

        // TC_AUTH_18_DangXuatThanhCong: Kiểm tra đăng xuất thành công
        [TestMethod]
        public void TC_AUTH_18_DangXuatThanhCong()
        {
            LoginUser("khoa@gmail.com", "123456");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            driver.FindElement(By.CssSelector(".hover\\3A bg-gray-100")).Click();
            driver.FindElement(By.CssSelector(".hover\\3A bg-red-50")).Click();
            
            wait.Until(d => d.Url.TrimEnd('/') == baseUrl.TrimEnd('/'));
            Assert.IsTrue(driver.Url.TrimEnd('/') == baseUrl.TrimEnd('/'), "Không chuyển hướng về trang chủ sau khi đăng xuất.");
        }
    }
}