using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class BaseTest
    {
        protected IWebDriver driver;
        protected string baseUrl = "https://localhost:44373/";

        [TestInitialize]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            
            // Tắt hoàn toàn trình quản lý mật khẩu và popup cảnh báo bảo mật của Google Chrome
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddUserProfilePreference("profile.password_manager_leak_detection", false);
            options.AddArgument("--disable-features=PasswordLeakDetection");
            options.AddArgument("--guest");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (driver != null)
            {
                try
                {
                    // Tự động đăng xuất cả quyền admin và user để dọn sạch session trên server
                    driver.Navigate().GoToUrl(baseUrl + "Admin/DangXuat");
                    driver.Navigate().GoToUrl(baseUrl + "User/DangXuat");
                }
                catch (Exception)
                {
                    // Bỏ qua lỗi nếu driver không còn khả dụng hoặc server không phản hồi
                }
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
