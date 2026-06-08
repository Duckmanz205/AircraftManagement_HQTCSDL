using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class SecurityAndNegativeTests : BaseTest
    {
        [TestMethod]
        public void TC_SEC_01_SQLInjection_Login()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangNhap");
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("email")).SendKeys("' OR 1=1 --");
            driver.FindElement(By.Id("password")).SendKeys("anything");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsFalse(driver.Url.Contains("/TrangChu"), "Lỗ hổng SQL Injection! Đã vượt qua đăng nhập.");
            Assert.IsTrue(driver.Url.Contains("/User/DangNhap"), "Hệ thống phải báo lỗi đăng nhập thất bại.");
        }

        [TestMethod]
        public void TC_NEG_01_EmailSaiDinhDang()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("email_khong_co_a_cong");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("Abc12345");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("Abc12345");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Hệ thống không bắt lỗi email không hợp lệ.");
        }
    }
}
