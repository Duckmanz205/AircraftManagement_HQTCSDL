using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class SecurityAndNegativeTests : BaseTest
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

        // TC_SEC_01: Truy cập Admin không có quyền (Với tài khoản Khách hàng)
        [TestMethod]
        public void TC_SEC_01_TruyCapAdminKhongCoQuyen()
        {
            // Đăng nhập với tài khoản Khách hàng (không có quyền Admin)
            LoginUser("khoa@gmail.com", "123456");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/User/TrangChu"));

            // Cố tình truy cập trực tiếp vào URL quản trị chuyến bay
            driver.Navigate().GoToUrl(baseUrl + "Admin/QLChuyenBay");
            
            // Hệ thống phải chặn và chuyển hướng về trang chủ hoặc đăng nhập
            Assert.IsFalse(driver.Url.Contains("/Admin/QLChuyenBay"), "Khách hàng không có quyền nhưng vẫn truy cập được trang quản trị.");
        }

        // TC_SEC_02: Nhân viên không đủ quyền Backup/Restore
        [TestMethod]
        public void TC_SEC_02_NhanVienKhongDuQuyenBackupRestore()
        {
            // Đăng nhập với NV01 (CV01 - Nhân viên bán vé, không có quyền Backup/Restore)
            LoginAdmin("NV01", "123456");
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Cố tình truy cập trực tiếp URL trang Cài đặt (Backup/Restore)
            driver.Navigate().GoToUrl(baseUrl + "Admin/CaiDat");
            
            // Hệ thống chặn truy cập
            Assert.IsFalse(driver.Url.Contains("/Admin/CaiDat"), "Nhân viên bán vé (CV01) không có quyền Backup/Restore nhưng vẫn vào được trang Cài đặt.");
        }

        // TC_NEG_01: Upload avatar sai định dạng hoặc quá dung lượng
        [TestMethod]
        public void TC_NEG_01_UploadAvatarSaiDinhDang()
        {
            LoginUser("khoa@gmail.com", "123456");
            driver.Navigate().GoToUrl(baseUrl + "User/HoSo");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Tạo file dummy không đúng định dạng (.exe)
            string tempFilePath = Path.Combine(Path.GetTempPath(), "test_malware.exe");
            File.WriteAllText(tempFilePath, "Dummy Executable File Content");

            try
            {
                // Upload file
                IWebElement fileInput = wait.Until(d => d.FindElement(By.Id("avatarInput")));
                
                // Sử dụng JS để hiển thị input nếu bị ẩn để Selenium tương tác được
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].classList.remove('hidden');", fileInput);
                
                fileInput.SendKeys(tempFilePath);

                // Submit form
                driver.FindElement(By.CssSelector("#profileForm button[type='submit']")).Click();

                // Xác nhận hiển thị thông báo lỗi file không hợp lệ
                IWebElement errorText = wait.Until(d => d.FindElement(By.Id("errorText")));
                Assert.IsTrue(
                    errorText.Text.Contains("Định dạng file không hợp lệ") || 
                    errorText.Text.Contains("Dung lượng vượt mức cho phép"), 
                    "Hệ thống không chặn file upload sai định dạng."
                );
            }
            finally
            {
                // Dọn dẹp file tạm
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        // TC_NEG_03: Phục hồi DB bằng file hỏng hoặc sai version
        [TestMethod]
        public void TC_NEG_03_PhucHoiDBFileHong()
        {
            // Đăng nhập với quyền Giám đốc (CV09) để có quyền Restore
            LoginAdmin("NV09", "admin123");
            driver.Navigate().GoToUrl(baseUrl + "Admin/CaiDat");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Tạo file corrupted backup giả lập (.bak)
            string tempFilePath = Path.Combine(Path.GetTempPath(), "corrupted.bak");
            File.WriteAllText(tempFilePath, "Corrupted MS SQL Server Backup Header Info - Invalid Content");

            try
            {
                // Tìm input file-upload
                IWebElement fileInput = wait.Until(d => d.FindElement(By.Id("file-upload")));
                
                // Sử dụng JS để hiển thị input
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].classList.remove('sr-only');", fileInput);
                
                fileInput.SendKeys(tempFilePath);

                // Giả lập confirm box cho tác vụ khôi phục dữ liệu nguy hiểm
                js.ExecuteScript("window.confirm = function() { return true; };");

                // Click Phục hồi
                driver.FindElement(By.Id("restoreText")).Click();

                // Xác nhận hiển thị thông báo lỗi phục hồi
                IWebElement notification = wait.Until(d => d.FindElement(By.Id("notification")));
                Assert.IsTrue(notification.Text.Contains("Lỗi phục hồi"), "Hệ thống không hiển thị thông báo lỗi khi khôi phục bằng file backup hỏng.");
            }
            finally
            {
                // Dọn dẹp file tạm
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
}
