using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class BVATests : BaseTest
    {
        // TC_BVA_01: Mật khẩu dưới mức cho phép (vd: < 6 ký tự)
        [TestMethod]
        public void TC_BVA_01_MatKhauDuoiBienChoPhep()
        {
            driver.Navigate().GoToUrl(baseUrl + "User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("test_bva_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            driver.FindElement(By.Id("password")).SendKeys("12345");
            driver.FindElement(By.Id("confirmPassword")).SendKeys("12345");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Không bắt lỗi giá trị biên dưới của mật khẩu.");
        }

        // TC_BVA_02: Mật khẩu vượt quá mức cho phép (vd: > 50 ký tự)
        [TestMethod]
        public void TC_BVA_02_MatKhauVuotBienChoPhep()
        {
            driver.Navigate().GoToUrl(baseUrl + "/User/DangKy");
            driver.FindElement(By.Id("fullName")).SendKeys("Nguyễn Văn A");
            driver.FindElement(By.Id("email")).SendKeys("test_bva2_" + DateTime.Now.Ticks + "@gmail.com");
            driver.FindElement(By.Id("phone")).SendKeys("0912345678");
            string longPassword = new string('A', 51);
            driver.FindElement(By.Id("password")).SendKeys(longPassword);
            driver.FindElement(By.Id("confirmPassword")).SendKeys(longPassword);
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();
            Assert.IsTrue(driver.Url.Contains("/User/DangKy"), "Không bắt lỗi giá trị biên trên của mật khẩu.");
        }
    }
}
