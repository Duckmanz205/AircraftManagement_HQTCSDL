using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Linq; // Thư viện bắt buộc để dùng WebDriverWait và SelectElement
using System.Data.SqlClient;

namespace QuanLyMayBayTest
{
    [TestClass]
    public class TCADMIN01Test
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;

        [TestInitialize]
        public void SetUp()
        {
            // Clean up the temporary booking cart for test user khoa@gmail.com (KH01)
            try
            {
                string connectionString = "data source=.;initial catalog=QUANLYMAYBAY;integrated security=True;MultipleActiveResultSets=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        DECLARE @MaKH NVARCHAR(10) = 'KH01';
                        DELETE FROM CHECKIN WHERE MAKH = @MaKH;
                        DELETE FROM GIOHANG_HANHKHACH WHERE MAGH IN (SELECT MAGH FROM GIOHANG WHERE MAKH = @MaKH AND TRANGTHAI = N'Đang Chọn');
                        DELETE FROM GIOHANG_CHITIET WHERE MAGH IN (SELECT MAGH FROM GIOHANG WHERE MAKH = @MaKH AND TRANGTHAI = N'Đang Chọn');
                        DELETE FROM GIOHANG WHERE MAKH = @MaKH AND TRANGTHAI = N'Đang Chọn';
                    ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database cleanup error: " + ex.Message);
            }

            ChromeOptions options = new ChromeOptions();

            // 1. Tắt hoàn toàn trình quản lý mật khẩu (Không hỏi lưu pass)
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

            // 2. Tắt tính năng cảnh báo lộ mật khẩu (Data Breach popup như trong ảnh của bạn)
            options.AddArgument("--disable-features=PasswordLeakDetection");

            // 3. Sử dụng Guest Mode để tránh các popups, đồng bộ của Chrome
            options.AddArgument("--guest");

            // Truyền options này vào trình duyệt khi khởi tạo
            driver = new ChromeDriver(options);

            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }

        [TestCleanup]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit(); // Đóng toàn bộ trình duyệt một cách an toàn
            }
        }

        [TestMethod]
        public void tCADMIN01()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 672);

            // Khởi tạo bộ đợi Explicit Wait (10 giây) để dùng xuyên suốt bài test
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Thực hiện Đăng nhập tài khoản Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("adminLoginTab")).Click();

            driver.FindElement(By.Id("adminId")).Click();
            driver.FindElement(By.Id("adminId")).SendKeys("NV05");

            driver.FindElement(By.Id("adminPassword")).Click();
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");

            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng Menu quản trị
            // Chờ cho menu hiển thị rồi mới click
            IWebElement menuLink = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-gray-50 > .font-medium")));
            menuLink.Click();

            driver.FindElement(By.CssSelector(".bg-blue-600:nth-child(1)")).Click();

            // 4. Xử lý Dropdown Chọn Máy bay (Aircraft)
            IWebElement aircraftDropdown = wait.Until(d => d.FindElement(By.Id("aircraftSelect")));
            SelectElement selectAircraft = new SelectElement(aircraftDropdown);
            try
            {
                // Thử chọn với text có dấu cách ở cuối giống Selenium IDE
                selectAircraft.SelectByText("Airbus A320 (MB01 )");
            }
            catch (NoSuchElementException)
            {
                // Nếu lỗi, tự động sửa sang text viết liền không khoảng trắng thừa
                selectAircraft.SelectByText("Airbus A320 (MB01)");
            }

            // 5. Xử lý Dropdown Chọn Sân bay đến (Arrival Airport)
            IWebElement arrivalDropdown = wait.Until(d => d.FindElement(By.Id("arrivalAirport")));
            SelectElement selectArrival = new SelectElement(arrivalDropdown);
            try
            {
                // Thử chọn với text có dấu cách ở cuối giống Selenium IDE
                selectArrival.SelectByText("Hồ Chí Minh (SB02 )");
            }
            catch (NoSuchElementException)
            {
                // Nếu lỗi, tự động sửa sang text viết liền không khoảng trắng thừa
                selectArrival.SelectByText("Hồ Chí Minh (SB02)");
            }

            // 6. Nhập thời gian khởi hành và thời gian đến
            driver.FindElement(By.Id("departureTime")).Click();
            driver.FindElement(By.Id("departureTime")).SendKeys("2026-01-01T08:00");

            driver.FindElement(By.Id("arrivalTime")).Click();
            driver.FindElement(By.Id("arrivalTime")).SendKeys("2026-01-01T09:00");

            // 7. Click nút Xác nhận/Lưu biểu mẫu
            driver.FindElement(By.CssSelector(".bg-blue-600:nth-child(2)")).Click();
        }
        [TestMethod]
        public void tCADMIN02()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 672);

            // Khởi tạo bộ đợi Explicit Wait (10 giây)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập tài khoản Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("adminLoginTab")).Click();

            driver.FindElement(By.Id("adminId")).Click();
            driver.FindElement(By.Id("adminId")).SendKeys("NV05");

            driver.FindElement(By.Id("adminPassword")).Click();
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");

            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng Menu quản trị (Chờ menu xuất hiện)
            IWebElement menuLink = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-gray-50 > .font-medium")));
            menuLink.Click();

            // 4. Bấm vào nút Chỉnh sửa (Icon bút chì/cập nhật) ở dòng thứ 3 của bảng
            // Chờ cho bảng dữ liệu load xong và nút thao tác có thể bấm được
            IWebElement editButton = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(3) .text-blue-600 > .fas")));
            editButton.Click();

            // 5. Cập nhật thời gian khởi hành
            IWebElement departureTimeInput = wait.Until(d => d.FindElement(By.Id("departureTime")));
            departureTimeInput.Click();
            // Lời khuyên: Khi chỉnh sửa, đôi khi ô input đã có sẵn dữ liệu cũ, bạn có thể cân nhắc dùng departureTimeInput.Clear() trước khi SendKeys nếu cần.
            departureTimeInput.SendKeys("2026-01-01T09:00");

            // 6. Cập nhật thời gian đến (Đã loại bỏ thao tác nhập nhầm 01:00 thừa của IDE)
            driver.FindElement(By.Id("arrivalTime")).Click();
            driver.FindElement(By.Id("arrivalTime")).SendKeys("2026-01-01T11:00");

            // 7. Click nút Xác nhận/Lưu thay đổi
            driver.FindElement(By.CssSelector(".bg-blue-600:nth-child(2)")).Click();
        }
        [TestMethod]
        public void tCADMIN03()
        {
            // 1. Điều hướng và thiết lập kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1050, 652);

            // Khởi tạo bộ đợi Explicit Wait (10 giây)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập với quyền Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("adminLoginTab")).Click();

            // Lược bỏ bớt các lệnh Click thừa, đi thẳng vào SendKeys
            driver.FindElement(By.Id("adminId")).SendKeys("NV05");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");

            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng Menu quản trị
            IWebElement menuLink = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-gray-50 > .font-medium")));
            menuLink.Click();

            // 4. Tương tác với dòng thứ 3 trong bảng
            // Click vào một phần tử trên dòng thứ 3 (có thể là để mở rộng dòng hoặc chọn dòng)
            IWebElement thirdRowElement = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(3) > .px-6 > .rounded")));
            thirdRowElement.Click();

            // Click vào icon Xóa (màu đỏ) ở dòng thứ 3
            IWebElement deleteIcon = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(3) .text-red-600 > .fas")));
            deleteIcon.Click();

            // 5. Xác nhận Xóa trên Popup/Modal
            // Chờ cho nút xác nhận màu đỏ hiện lên rồi click
            IWebElement confirmDeleteButton = wait.Until(d => d.FindElement(By.CssSelector(".bg-red-600")));
            confirmDeleteButton.Click();
        }
        [TestMethod]
        public void tCADMIN04()
        {
            // 1. Điều hướng và thiết lập kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1050, 652);

            // Khởi tạo bộ đợi Explicit Wait (10 giây)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập với quyền Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ tab đăng nhập admin xuất hiện rồi click (Selenium IDE bắt thêm > .fas)
            wait.Until(d => d.FindElement(By.CssSelector("#adminLoginTab > .fas"))).Click();

            // Lược bỏ các lệnh click thừa trước khi nhập text
            driver.FindElement(By.Id("adminId")).SendKeys("NV05");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng Menu "Quản lý Chuyến bay"
            // Sử dụng LinkText rất trực quan và ổn định
            IWebElement flightManagementLink = wait.Until(d => d.FindElement(By.LinkText("Quản lý Chuyến bay")));
            flightManagementLink.Click();

            // 4. Tương tác với dòng ĐẦU TIÊN (thứ 1) trong bảng
            // Click vào phần tử trên dòng thứ 1 để chọn/mở rộng
            IWebElement firstRowElement = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(1) > .px-6 > .rounded")));
            firstRowElement.Click();

            // Click vào icon Xóa (màu đỏ) ở dòng thứ 1
            IWebElement deleteIcon = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(1) .text-red-600 > .fas")));
            deleteIcon.Click();

            // 5. Xác nhận Xóa trên Popup/Modal
            // Chờ cho nút xác nhận màu đỏ trên Modal hiện lên rồi click
            IWebElement confirmDeleteButton = wait.Until(d => d.FindElement(By.CssSelector(".bg-red-600")));
            confirmDeleteButton.Click();
        }
        [TestMethod]
        public void tCADMIN05()
        {
            // 1. Điều hướng và thiết lập kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            // Khởi tạo bộ đợi Explicit Wait (10 giây)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            driver.FindElement(By.Id("adminLoginTab")).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV05");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng Menu quản trị
            IWebElement menuLink = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-gray-50 > .font-medium")));
            menuLink.Click();

            // 4. Bấm nút "Cập nhật trạng thái"
            IWebElement updateStatusBtn = wait.Until(d => d.FindElement(By.LinkText("Cập nhật trạng thái")));
            updateStatusBtn.Click();

            // 5. Kiểm tra thông báo hiển thị (Toast Notification)
            // Chờ cho thông báo xuất hiện
            IWebElement toastNotification = wait.Until(d => d.FindElement(By.CssSelector("#toast-notification .text-sm")));

            // Sử dụng Assert.AreEqual của MSTest (Kỳ vọng, Thực tế)
            Assert.AreEqual("Đã cập nhật trạng thái cho 0 chuyến bay dựa trên giờ thực tế!", toastNotification.Text);

            // 6. Kiểm tra trạng thái của chuyến bay ở dòng thứ 2
            // Chờ cho dòng thứ 2 load trạng thái
            IWebElement flightStatus = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(2) .px-2")));

            Assert.AreEqual("Đã hạ cánh", flightStatus.Text);
        }
        [TestMethod]
        public void tCADMIN06()
        {
            // 1. Điều hướng và thiết lập kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1050, 652);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập Admin với tài khoản NV01
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            IWebElement adminTab = wait.Until(d => d.FindElement(By.Id("adminLoginTab")));
            adminTab.Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV01");
            driver.FindElement(By.Id("adminPassword")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // Chờ một chút để hệ thống xử lý đăng nhập xong (đợi URL đổi sang trang admin)
            wait.Until(d => d.Url.Contains("/Admin"));

            // 3. Cố tình truy cập vào trang không được cấp quyền
            driver.Navigate().GoToUrl("https://localhost:44373/Admin/QLPerson");

            // 4. Chờ hệ thống xử lý phân quyền và tự động Redirect (Chuyển hướng)
            // Đợi tối đa 10s cho đến khi URL thực tế biến thành URL trang chủ
            wait.Until(d => d.Url == "https://localhost:44373/Admin/TrangChu");

            // 5. Lấy URL thực tế sau khi bị chuyển hướng (Sử dụng driver.Url thay vì JavaScript)
            string actualUrl = driver.Url;

            // 6. Kiểm tra (Assert) xem hệ thống có thực sự chặn và đẩy về Trang chủ hay không
            Assert.AreEqual("https://localhost:44373/Admin/TrangChu", actualUrl, "Hệ thống không chuyển hướng người dùng về đúng trang chủ khi không có quyền truy cập!");
        }
        [TestMethod]
        public void tCADMIN07()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            wait.Until(d => d.FindElement(By.CssSelector("#adminLoginTab > .fas"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV05");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Nhân viên
            IWebElement employeeMenu = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(1) > .hover\\3A bg-red-50 > .font-medium")));
            employeeMenu.Click();

            // 4. Mở form Thêm nhân viên
            IWebElement btnAddEmployee = wait.Until(d => d.FindElement(By.Id("btn-add-employee")));
            btnAddEmployee.Click();

            // 5. Điền thông tin nhân viên mới
            // Chờ form hiển thị bằng cách đợi ô nhập liệu đầu tiên
            IWebElement inputMaNV = wait.Until(d => d.FindElement(By.Name("MANV")));
            inputMaNV.SendKeys("NV11");

            driver.FindElement(By.Name("TENNV")).SendKeys("Nguyễn Thị Mới");
            driver.FindElement(By.Name("SDT")).SendKeys("0912345611");

            // Chọn Chức vụ (Dùng SelectElement để đảm bảo ổn định)
            IWebElement jobDropdown = driver.FindElement(By.Id("MACV"));
            SelectElement selectJob = new SelectElement(jobDropdown);
            selectJob.SelectByText("Nhân viên bán vé");

            driver.FindElement(By.Name("MATKHAU")).SendKeys("pass123");

            // 6. Lưu thông tin
            driver.FindElement(By.CssSelector(".bg-blue-600:nth-child(2)")).Click();

            // 7. Kiểm tra dữ liệu nhân viên mới trên bảng (Asserts)
            // Đợi cho dòng thứ 11 xuất hiện sau khi load lại bảng
            IWebElement row11Col1 = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(11) > .px-6:nth-child(1)")));

            // So sánh các giá trị hiển thị thực tế với dữ liệu vừa nhập
            Assert.AreEqual("NV11", row11Col1.Text);

            Assert.AreEqual("Nguyễn Thị Mới",
                driver.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(11) > .px-6:nth-child(2)")).Text);

            Assert.AreEqual("0912345611",
                driver.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(11) > .px-6:nth-child(3)")).Text);

            // Mặc dù chọn "Nhân viên bán vé" ở form, nhưng trên bảng có thể hiển thị mã chức vụ (CV01)
            Assert.AreEqual("CV01",
                driver.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(11) > .px-6:nth-child(4)")).Text);
        }
        [TestMethod]
        public void tCADMIN08()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ form đăng nhập admin hiển thị
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV05");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Nhân viên
            IWebElement employeeMenu = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(1) > .hover\\3A bg-red-50 > .font-medium")));
            employeeMenu.Click();

            // 4. Thực hiện xóa nhân viên ở dòng thứ 11
            // Chờ cho bảng dữ liệu tải xong và nút Xóa (màu đỏ) xuất hiện
            IWebElement deleteBtn = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(11) .text-red-600")));
            deleteBtn.Click();

            // ==========================================
            // BỔ SUNG: XỬ LÝ ALERT XÁC NHẬN XÓA
            // ==========================================
            IAlert confirmAlert = wait.Until(d => {
                try
                {
                    return d.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null; // Chờ cho đến khi Alert hiện ra
                }
            });

            // Nhấn "OK" trên hộp thoại cảnh báo để tiến hành xóa
            confirmAlert.Accept();

            // 5. Kiểm tra thông báo (Toast Notification)
            // Chờ cho Toast "Thành công" xuất hiện sau khi đã nhấn OK
            IWebElement toastNotification = wait.Until(d => d.FindElement(By.CssSelector("#toast-notification .font-bold")));

            Assert.AreEqual("Thành công", toastNotification.Text);
        }
        [TestMethod]
        public void tCADMIN09()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ form đăng nhập admin hiển thị
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Nhân viên
            IWebElement employeeMenu = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(1) > .hover\\3A bg-red-50 > .font-medium")));
            employeeMenu.Click();

            // 4. NV09 cố tình thao tác nhấn Xóa ở dòng của chính mình (dòng số 9)
            IWebElement deleteSelfBtn = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(9) .text-red-600 > .fas")));
            deleteSelfBtn.Click();

            // ==========================================
            // BỔ SUNG: XỬ LÝ ALERT XÁC NHẬN XÓA
            // ==========================================
            IAlert confirmAlert = wait.Until(d => {
                try
                {
                    return d.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null; // Chờ cho đến khi Alert hiện ra
                }
            });

            // Nhấn "OK" trên hộp thoại cảnh báo để kích hoạt luồng xử lý xóa của hệ thống
            confirmAlert.Accept();

            // 5. Kiểm tra thông báo lỗi hiển thị từ hệ thống
            // Chờ cho phần tử chứa thông báo (Toast/Error text) xuất hiện
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("div > .text-sm:nth-child(2)")));

            // So sánh kết quả bằng Assert.AreEqual của MSTest
            Assert.AreEqual("Không thể tự xóa chính tài khoản bản thân!", errorMessage.Text);
        }
        [TestMethod]
        public void tCADMIN10()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Nhân viên
            IWebElement employeeMenu = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(1) > .hover\\3A bg-red-50 > .font-medium")));
            employeeMenu.Click();

            // 4. NV09 thao tác nhấn Cập nhật (icon sửa) ở dòng của chính mình (dòng số 9)
            IWebElement editSelfBtn = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-gray-50:nth-child(9) .mr-3 > .fas")));
            editSelfBtn.Click();

            // 5. Mở dropdown và thay đổi Chức vụ thành "Nhân viên bán vé"
            IWebElement roleDropdown = wait.Until(d => d.FindElement(By.Name("MACV")));
            SelectElement selectRole = new SelectElement(roleDropdown);
            selectRole.SelectByText("Nhân viên bán vé");

            // 6. Nhấn nút Xác nhận/Lưu
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // 7. Nhấn nút "Huỷ" để đóng form (Thao tác được giữ nguyên từ Selenium IDE)
            IWebElement cancelBtn = wait.Until(d => d.FindElement(By.LinkText("Huỷ")));
            cancelBtn.Click();

            // 8. Kiểm tra thông báo Lỗi từ hệ thống
            // Chờ cho phần tử chứa thông báo (Toast/Error text) xuất hiện
            IWebElement toastNotification = wait.Until(d => d.FindElement(By.CssSelector("#toast-notification .font-bold")));

            // So sánh kết quả bằng Assert.AreEqual của MSTest
            Assert.AreEqual("Lỗi", toastNotification.Text);
        }
        [TestMethod]
        public void tCADMIN11()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Mở Menu Thống kê / Báo cáo
            IWebElement menuGroup = wait.Until(d => d.FindElement(By.CssSelector(".sidebar-item > .flex > .font-medium")));
            menuGroup.Click();

            IWebElement subMenu = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(1) > .px-3 > span")));
            subMenu.Click();

            // 4. Chọn bộ lọc Tháng và Năm
            IWebElement monthDropdown = wait.Until(d => d.FindElement(By.Name("thang")));
            SelectElement selectMonth = new SelectElement(monthDropdown);
            selectMonth.SelectByText("Tháng 11");

            IWebElement yearDropdown = driver.FindElement(By.Name("nam"));
            SelectElement selectYear = new SelectElement(yearDropdown);
            selectYear.SelectByText("2025");

            // 5. Bấm nút Lọc / Xem thống kê
            driver.FindElement(By.CssSelector(".px-6")).Click();

            // ==========================================
            // BỔ SUNG: XỬ LÝ LỖI STALE ELEMENT REFERENCE
            // ==========================================
            // Dùng wait để liên tục kiểm tra cho đến khi text cập nhật thành con số kỳ vọng
            // Hoặc cho đến khi hết 10 giây (nếu không ra đúng số sẽ báo fail ở Assert)
            wait.Until(d => {
                try
                {
                    IWebElement element = d.FindElement(By.CssSelector(".text-4xl"));
                    // Sẽ trả về true và thoát vòng chờ nếu text đã được cập nhật đúng
                    return element.Text == "26,050,000 VND";
                }
                catch (StaleElementReferenceException)
                {
                    // Bị lỗi thẻ cũ thì trả về false để wait tiếp tục chạy lại tìm thẻ mới
                    return false;
                }
            });

            // 6. Kiểm tra kết quả hiển thị (Tổng doanh thu)
            // Tới đây thì chắc chắn DOM đã ổn định, ta lấy lại thẻ một lần nữa để Assert
            IWebElement totalRevenue = driver.FindElement(By.CssSelector(".text-4xl"));
            Assert.AreEqual("26,050,000 VND", totalRevenue.Text);
        }
        [TestMethod]
        public void tCADMIN12()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ tab Đăng nhập admin hiển thị
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu chức năng (Menu thứ 4)
            IWebElement menuLink = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(4) .font-medium")));
            menuLink.Click();

            // 4. Nhập Ngày bắt đầu (startDate)
            // Đợi ô input xuất hiện, xóa dữ liệu cũ (nếu có) và nhập ngày chính xác
            IWebElement startDateInput = wait.Until(d => d.FindElement(By.Name("startDate")));
            startDateInput.Clear();
            startDateInput.SendKeys("2025-11-01");

            // 5. Nhập Ngày kết thúc (endDate)
            IWebElement endDateInput = driver.FindElement(By.Name("endDate"));
            endDateInput.Clear();
            endDateInput.SendKeys("2025-11-30");

            // 6. Nhấn nút Lọc / Tìm kiếm (Tương ứng với class .duration-200)
            driver.FindElement(By.CssSelector(".duration-200")).Click();

            // 7. Nhấn vào kết quả đầu tiên hoặc nút hành động (Export, View chi tiết,...)
            // Đợi kết quả load ra màn hình rồi mới thao tác
            wait.Until(driverInstance =>
            {
                try
                {
                    // Thử tìm element
                    IWebElement targetElement = driverInstance.FindElement(By.CssSelector(".gap-2:nth-child(1)"));

                    // Nếu tìm thấy, thực hiện click qua Javascript
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driverInstance;
                    js.ExecuteScript("arguments[0].click();", targetElement);

                    // Nếu click thành công không sinh lỗi, trả về true để thoát khỏi lệnh wait
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // Nếu DOM bị load lại khiến element cũ bị xóa, trả về false để WebDriverWait thử lại ở chu kỳ tiếp theo
                    return false;
                }
                catch (NoSuchElementException)
                {
                    // Nếu kết quả chưa load ra (chưa có phần tử nào), trả về false để tiếp tục chờ
                    return false;
                }
            });
        }
        [TestMethod]
        public void tCADMIN13()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            // Tăng thời gian chờ lên 15 giây vì thao tác Backup DB thường tốn thời gian xử lý
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ tab Đăng nhập admin hiển thị
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Backup/Hệ thống
            IWebElement menuBackup = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-red-50 > .font-medium")));
            menuBackup.Click();

            // 4. Nhấn vào nút "Tạo Backup mới" (Icon màu xanh lá)
            IWebElement createBackupBtn = wait.Until(d => d.FindElement(By.CssSelector(".group-hover\\3Atext-green-700")));
            createBackupBtn.Click();

            // 5. Nhập tên cho bản Backup
            IWebElement backupNameInput = wait.Until(d => d.FindElement(By.Id("backupName")));
            backupNameInput.Clear(); // Dọn dẹp ô input trước khi nhập
            backupNameInput.SendKeys("Backup_Test");

            // 6. Nhấn nút Xác nhận/Lưu màu xanh lá
            driver.FindElement(By.CssSelector(".bg-green-600")).Click();

            // Chờ cho dòng thông tin file backup vừa tạo hiển thị ra màn hình
            IWebElement backupRecord = wait.Until(d => d.FindElement(By.CssSelector("#notification p.text-gray-200")));
            string recordText = backupRecord.GetAttribute("textContent");
            Console.WriteLine("DEBUG BACKUP RECORD TEXT: " + recordText);

            // Thay thế đoạn JavaScript phức tạp bằng hàm C# gốc
            // Kiểm tra xem đoạn text lấy được có chứa chuỗi "QLMayBay_FULL_Backup_Test_" hay không
            bool ketQua = recordText.Contains("QLMayBay_FULL_Backup_Test_");

            // Kiểm tra Assert (Nếu false sẽ in ra câu thông báo lỗi ở tham số thứ 2)
            Assert.IsTrue(ketQua, "Tên file backup hiển thị không khớp hoặc quá trình backup chưa thành công! Message: " + recordText);
        }
        [TestMethod]
        public void tCADMIN14()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1050, 652);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Backup/Restore
            IWebElement menuBackup = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-red-50 > .font-medium")));

            // Sử dụng JS Click để tránh lỗi ElementClickInterceptedException (bị phần tử khác đè lên)
            js.ExecuteScript("arguments[0].click();", menuBackup);

            // 4. Mở vùng chọn file Restore
            IWebElement openRestoreArea = wait.Until(d => d.FindElement(By.CssSelector(".relative > span")));
            js.ExecuteScript("arguments[0].click();", openRestoreArea);

            // 5. TỰ ĐỘNG TÌM FILE BACKUP MỚI NHẤT
            string backupFolderPath = @"D:\Backups_QLMayBay";

            // Kiểm tra xem thư mục có tồn tại không
            if (!Directory.Exists(backupFolderPath))
            {
                Assert.Fail($"Thư mục không tồn tại: {backupFolderPath}. Hãy tạo thư mục này và bỏ file .bak vào để test.");
            }

            // Lấy danh sách các file .bak và chọn file được tạo gần đây nhất
            var directoryInfo = new DirectoryInfo(backupFolderPath);
            var latestBackupFile = directoryInfo.GetFiles("*.bak")
                                                .OrderByDescending(f => f.LastWriteTime)
                                                .FirstOrDefault();

            if (latestBackupFile == null)
            {
                Assert.Fail($"Không tìm thấy bất kỳ file .bak nào trong thư mục: {backupFolderPath}");
            }

            // Tải file Backup mới nhất lên hệ thống
            IWebElement fileUploadInput = wait.Until(d => d.FindElement(By.Id("file-upload")));
            fileUploadInput.SendKeys(latestBackupFile.FullName); // Lấy đường dẫn tuyệt đối của file

            // 6. Nhấn nút "Phục hồi" (Restore)
            IWebElement restoreBtn = wait.Until(d => d.FindElement(By.Id("restoreText")));
            js.ExecuteScript("arguments[0].click();", restoreBtn);

            // 7. Chờ JavaScript Alert xuất hiện và chuyển hướng điều khiển
            IAlert alert = wait.Until(d => {
                try
                {
                    return d.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null;
                }
            });

            // 8. KIỂM TRA NỘI DUNG ALERT (ĐÃ CHUẨN HÓA CHUỖI)
            string expectedAlertText = "CẢNH BÁO CUỐI CÙNG: \n\nBạn sắp xóa toàn bộ dữ liệu hiện tại để thay thế bằng bản backup này. \n\nBạn có chắc chắn muốn tiếp tục?";

            // Chuẩn hóa chuỗi thực tế từ Alert: Đồng bộ dấu xuống dòng và cắt khoảng trắng thừa
            string actualAlertText = alert.Text.Replace("\r\n", "\n").Trim();
            string expectedNormalized = expectedAlertText.Replace("\r\n", "\n").Trim();

            Assert.AreEqual(expectedNormalized, actualAlertText, "Nội dung Alert không khớp do sai lệch ký tự text!");

            // 9. Xử lý đóng Alert (Chọn Dismiss/Hủy để bảo vệ dữ liệu lúc test tự động)
            alert.Dismiss();
        }
        [TestMethod]
        public void tCADMIN15()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ tab Đăng nhập admin hiển thị rồi nhấn
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            // Lược bỏ các thao tác click thừa, đi thẳng vào nhập liệu
            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Backup/Restore
            IWebElement menuBackup = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(2) > .hover\\3A bg-red-50 > .font-medium")));
            menuBackup.Click();

            // 4. Cố tình nhấn nút "Phục hồi" (Restore) khi CHƯA chọn file
            IWebElement restoreBtn = wait.Until(d => d.FindElement(By.Id("restoreText")));
            restoreBtn.Click();

            // 5. Chờ JavaScript Alert cảnh báo lỗi xuất hiện
            IAlert alert = wait.Until(d => {
                try
                {
                    return d.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null; // Tiếp tục chờ nếu Alert chưa kịp hiện ra
                }
            });

            // 6. Kiểm tra nội dung của Alert có đúng với kỳ vọng hay không
            string expectedAlertText = "Vui lòng chọn file backup (.bak) để phục hồi.";
            Assert.AreEqual(expectedAlertText, alert.Text);

            // 7. Nhấn "OK" trên Alert để đóng cảnh báo lại
            alert.Accept();
        }
        [TestMethod]
        public void tCADMIN16()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập quyền Admin bằng tài khoản NV09
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            // Chờ tab Đăng nhập admin hiển thị rồi nhấn
            wait.Until(d => d.FindElement(By.Id("adminLoginTab"))).Click();

            // Lược bỏ click thừa, tiến hành nhập thông tin
            driver.FindElement(By.Id("adminId")).SendKeys("NV09");
            driver.FindElement(By.Id("adminPassword")).SendKeys("admin123");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-900")).Click();

            // 3. Chuyển hướng đến menu Quản lý Nhân viên
            IWebElement employeeMenu = wait.Until(d => d.FindElement(By.CssSelector("li:nth-child(1) > .hover\\3A bg-red-50 > .font-medium")));
            employeeMenu.Click();

            // 4. Tìm kiếm nhân viên bằng mã "NV05"
            IWebElement searchInput = wait.Until(d => d.FindElement(By.Name("manv")));
            searchInput.Clear(); // Dọn dẹp ô input phòng khi có dữ liệu cũ
            searchInput.SendKeys("NV05");

            // Sử dụng phím Enter để kích hoạt tìm kiếm thay vì click nút
            searchInput.SendKeys(Keys.Enter);

            // 5. Kiểm tra kết quả hiển thị trên bảng (Asserts)
            // Chờ cho dòng đầu tiên của bảng kết quả tải xong
            IWebElement col1 = wait.Until(d => d.FindElement(By.CssSelector("#content-nhanvien .hover\\3A bg-gray-50 > .px-6:nth-child(1)")));

            // Kiểm tra từng cột (Mã NV, Tên NV, SĐT, Mã Chức Vụ)
            Assert.AreEqual("NV05", col1.Text);

            Assert.AreEqual("Hoàng Thị E",
                driver.FindElement(By.CssSelector("#content-nhanvien .hover\\3A bg-gray-50 > .px-6:nth-child(2)")).Text);

            Assert.AreEqual("0905678901",
                driver.FindElement(By.CssSelector("#content-nhanvien .hover\\3A bg-gray-50 > .px-6:nth-child(3)")).Text);

            Assert.AreEqual("CV05",
                driver.FindElement(By.CssSelector("#content-nhanvien .hover\\3A bg-gray-50 > .px-6:nth-child(4)")).Text);
        }
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
        [TestMethod]
        public void tCDATVE14()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập tài khoản Khách hàng
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();

            // 3. Chuyển sang trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // 4. Chọn chuyến bay
            IWebElement selectFlightBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            js.ExecuteScript("arguments[0].click();", selectFlightBtn);

            IWebElement confirmPassengerBtn = wait.Until(d => d.FindElement(By.CssSelector("#passengerModal button[type='submit']")));
            js.ExecuteScript("arguments[0].click();", confirmPassengerBtn);

            // 5. Chọn ghế (Chọn ghế Phổ thông đầu tiên còn trống)
            IWebElement seatElement = wait.Until(d => {
                var el = d.FindElement(By.CssSelector(".seat.available"));
                return el.Displayed ? el : null;
            });
            js.ExecuteScript("var seat = Array.from(document.querySelectorAll('.seat.available')).find(s => s.dataset.className.includes('Phổ thông')) || document.querySelector('.seat.available'); if (seat) { seat.click(); }");

            // Nhấn Tiếp tục
            js.ExecuteScript("showPassengerForm();");

            // 6. Điền thông tin hành khách
            IWebElement nameInput = wait.Until(d => {
                var el = d.FindElement(By.Id("p1-name"));
                return el.Displayed ? el : null;
            });
            nameInput.SendKeys("Nguyễn Văn A");
            js.ExecuteScript("document.getElementById('p1-dob').value = '2005-09-24';");

            // Chọn Quốc gia
            SelectElement countryDropdown = new SelectElement(driver.FindElement(By.Id("p1-country")));
            countryDropdown.SelectByText("Việt Nam");

            driver.FindElement(By.Id("p1-email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Id("p1-phone")).SendKeys("0903911132");

            // Chọn Giới tính
            SelectElement genderDropdown = new SelectElement(driver.FindElement(By.Id("p1-gender")));
            genderDropdown.SelectByText("Nam");

            // Chọn Hành lý ký gửi
            SelectElement carryOnDropdown = new SelectElement(driver.FindElement(By.Id("p1-carry-on")));
            carryOnDropdown.SelectByText("10kg (+200,000 VNĐ)");

            // 7. Nhấn Tiếp tục để sang trang tính tiền / thanh toán
            js.ExecuteScript("document.querySelector('#passenger-form button[type=\"submit\"]').click();");

            // 8. Kiểm tra tổng số tiền (Assert)
            IWebElement totalPrice = null;
            try
            {
                totalPrice = wait.Until(d => d.FindElement(By.CssSelector(".sm\\3Atext-3xl")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("DEBUG FAILURE URL: " + driver.Url);
                Console.WriteLine("DEBUG FAILURE PAGE SOURCE: " + driver.PageSource);
                throw;
            }
            Assert.AreEqual("2150000 VNĐ", totalPrice.Text);

            // 9. Xem lại "Vé của tôi"
            driver.FindElement(By.LinkText("Vé của tôi")).Click();

            // Bấm vào thao tác trên vé vừa đặt (Có thể là xem chi tiết hoặc thanh toán sau)
            js.ExecuteScript("arguments[0].click();", wait.Until(d => d.FindElement(By.CssSelector(".flex:nth-child(7) > .sm\\3Aw-auto"))));

            // 10. Đăng xuất khỏi hệ thống
            js.ExecuteScript("document.querySelector('a[href=\"/User/DangXuat\"]').click();");
        }
        [TestMethod]
        public void tCDATVE15()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập tài khoản Khách hàng
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();

            // 3. Chuyển sang trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // 4. Chọn chuyến bay
            IWebElement selectFlightBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            js.ExecuteScript("arguments[0].click();", selectFlightBtn);

            IWebElement confirmPassengerBtn = wait.Until(d => d.FindElement(By.CssSelector("#passengerModal button[type='submit']")));
            js.ExecuteScript("arguments[0].click();", confirmPassengerBtn);

            // 5. Chọn ghế (Chọn ghế Phổ thông đầu tiên còn trống)
            IWebElement seatElement = wait.Until(d => {
                var el = d.FindElement(By.CssSelector(".seat.available"));
                return el.Displayed ? el : null;
            });
            js.ExecuteScript("var seat = Array.from(document.querySelectorAll('.seat.available')).find(s => s.dataset.className.includes('Phổ thông')) || document.querySelector('.seat.available'); if (seat) { seat.click(); }");

            // Nhấn Tiếp tục
            js.ExecuteScript("showPassengerForm();");

            // 6. Điền thông tin hành khách
            IWebElement nameInput = wait.Until(d => {
                var el = d.FindElement(By.Id("p1-name"));
                return el.Displayed ? el : null;
            });
            nameInput.SendKeys("Nguyễn Văn A");
            js.ExecuteScript("document.getElementById('p1-dob').value = '2005-09-24';");

            // Chọn Quốc gia
            SelectElement countryDropdown = new SelectElement(driver.FindElement(By.Id("p1-country")));
            countryDropdown.SelectByText("Việt Nam");

            driver.FindElement(By.Id("p1-email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Id("p1-phone")).SendKeys("0903911132");

            // Chọn Giới tính
            SelectElement genderDropdown = new SelectElement(driver.FindElement(By.Id("p1-gender")));
            genderDropdown.SelectByText("Nam");

            // Chọn Hành lý KÝ GỬI (Lưu ý: ID ở đây là p1-checked khác với p1-carry-on ở TC14)
            SelectElement checkedBagDropdown = new SelectElement(driver.FindElement(By.Id("p1-checked")));
            checkedBagDropdown.SelectByText("20kg (+500,000 VNĐ)");

            // 7. Nhấn Tiếp tục để sang trang tính tiền / thanh toán
            js.ExecuteScript("document.querySelector('#passenger-form button[type=\"submit\"]').click();");

            // 8. Kiểm tra tổng số tiền (Assert)
            IWebElement totalPrice = wait.Until(d => d.FindElement(By.CssSelector(".sm\\3Atext-3xl")));
            Assert.AreEqual("2450000 VNĐ", totalPrice.Text);

            // 9. Xem lại "Vé của tôi"
            driver.FindElement(By.LinkText("Vé của tôi")).Click();

            // Bấm vào thao tác trên vé vừa đặt
            js.ExecuteScript("arguments[0].click();", wait.Until(d => d.FindElement(By.CssSelector(".flex:nth-child(7) > .sm\\3Aw-auto"))));

            // 10. Đăng xuất khỏi hệ thống
            js.ExecuteScript("document.querySelector('a[href=\"/User/DangXuat\"]').click();");
        }
        [TestMethod]
        public void tCDATVE16()
        {
            // 1. Điều hướng và cấu hình kích thước màn hình
            driver.Navigate().GoToUrl("https://localhost:44373/");
            driver.Manage().Window.Size = new System.Drawing.Size(1296, 688);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Đăng nhập tài khoản Khách hàng
            driver.FindElement(By.CssSelector(".bg-blue-600")).Click();

            IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys("khoa@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".hover\\3A bg-blue-700")).Click();

            // 3. Chuyển sang trang Đặt Vé
            IWebElement datVeMenu = wait.Until(d => d.FindElement(By.LinkText("Đặt Vé")));
            datVeMenu.Click();

            // 4. Chọn chuyến bay
            IWebElement selectFlightBtn = wait.Until(d => d.FindElement(By.CssSelector("button[onclick^='checkPassengerAndBook']")));
            js.ExecuteScript("arguments[0].click();", selectFlightBtn);

            IWebElement confirmPassengerBtn = wait.Until(d => d.FindElement(By.CssSelector("#passengerModal button[type='submit']")));
            js.ExecuteScript("arguments[0].click();", confirmPassengerBtn);

            // 5. Chọn ghế (Chọn ghế Phổ thông đầu tiên còn trống)
            IWebElement seatElement = wait.Until(d => {
                var el = d.FindElement(By.CssSelector(".seat.available"));
                return el.Displayed ? el : null;
            });
            js.ExecuteScript("var seat = Array.from(document.querySelectorAll('.seat.available')).find(s => s.dataset.className.includes('Phổ thông')) || document.querySelector('.seat.available'); if (seat) { seat.click(); }");

            // Nhấn Tiếp tục
            js.ExecuteScript("showPassengerForm();");

            // 6. Điền thông tin hành khách
            IWebElement nameInput = wait.Until(d => {
                var el = d.FindElement(By.Id("p1-name"));
                return el.Displayed ? el : null;
            });
            nameInput.SendKeys("Nguyễn Văn A");

            // Điền Ngày Sinh (Cố tình nhập năm 2025 để trigger lỗi "Dưới 2 tuổi")
            js.ExecuteScript("document.getElementById('p1-dob').value = '2025-09-24';");

            // Chọn Quốc gia
            SelectElement countryDropdown = new SelectElement(driver.FindElement(By.Id("p1-country")));
            countryDropdown.SelectByText("Việt Nam");

            driver.FindElement(By.Id("p1-email")).SendKeys("nguyenvana@gmail.com");
            driver.FindElement(By.Id("p1-phone")).SendKeys("0903911132");

            // Chọn Giới tính
            SelectElement genderDropdown = new SelectElement(driver.FindElement(By.Id("p1-gender")));
            genderDropdown.SelectByText("Nam");

            // Bỏ qua chọn hành lý vì IDE ghi nhận click vào p1-carry-on nhưng không chọn option, đi thẳng tới nhấn nút Tiếp tục
            js.ExecuteScript("document.querySelector('#passenger-form button[type=\"submit\"]').click();");

            // 7. Kiểm tra hệ thống có bắt lỗi và hiển thị thông báo chính xác không
            // Chờ khung màu đỏ chứa thông báo lỗi xuất hiện
            IWebElement errorMessage;
            try
            {
                errorMessage = wait.Until(d => d.FindElement(By.CssSelector(".bg-red-100 p")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("DEBUG FAILURE URL: " + driver.Url);
                Console.WriteLine("DEBUG FAILURE PAGE SOURCE: " + driver.PageSource);
                throw;
            }

            Assert.AreEqual("Hành khách bắt buộc phải đạt độ tuổi từ đủ 2 tuổi trở lên!", errorMessage.Text);
        }
    }
}