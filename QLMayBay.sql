-- ======================================================
-- 1. KHỞI TẠO DATABASE
-- ======================================================
CREATE DATABASE QUANLYMAYBAY;
GO
USE QUANLYMAYBAY;
GO

-- ======================================================
-- 2. TẠO CẤU TRÚC BẢNG
-- ======================================================

-- 1. BẢNG CHỨC VỤ
CREATE TABLE CHUCVU (
    MACV CHAR(10) NOT NULL,
    TENCV NVARCHAR(50),
    CONSTRAINT PK_CV PRIMARY KEY (MACV)
);

-- 2. BẢNG NHÂN VIÊN
CREATE TABLE NHANVIEN (
    MANV CHAR(10) NOT NULL,
    TENNV NVARCHAR(50),
    SDT VARCHAR(15),
    MACV CHAR(10),
    CONSTRAINT PK_NV PRIMARY KEY (MANV),
    CONSTRAINT FK_NV_CV FOREIGN KEY (MACV) REFERENCES CHUCVU(MACV)
);

-- 3. BẢNG KHÁCH HÀNG
CREATE TABLE KHACHHANG (
    MAKH CHAR(10) NOT NULL,
    TENKH NVARCHAR(50),
    EMAIL VARCHAR(100),
    MATKHAU VARCHAR(50),
    SDT VARCHAR(20),
    DIACHI NVARCHAR(100),
    GTINH NVARCHAR(3),
    NGSINH DATE,
    QUOCGIA NVARCHAR(50),
    CONSTRAINT PK_KH PRIMARY KEY (MAKH)
);

-- 4. BẢNG MÁY BAY
CREATE TABLE MAYBAY (
    MAMB CHAR(10) NOT NULL,
    TENMB NVARCHAR(50),
    HANG NVARCHAR(50),
    TONGSOGHE INT,
    CONSTRAINT PK_MB PRIMARY KEY (MAMB)
);

-- 5. BẢNG CAUHINH_GHE
CREATE TABLE CAUHINH_GHE (
    MACG CHAR(10) NOT NULL,
    MAMB CHAR(10) NOT NULL,
    HANGGHE NVARCHAR(20) NOT NULL,
    SOLUONG INT,
    CONSTRAINT PK_CG PRIMARY KEY (MACG),
    CONSTRAINT FK_CG_MB FOREIGN KEY (MAMB) REFERENCES MAYBAY(MAMB),
    CONSTRAINT UNQ_CG UNIQUE (MAMB, HANGGHE)
);

-- 6. BẢNG SÂN BAY
CREATE TABLE SANBAY (
    MASB CHAR(10) NOT NULL,
    TENSB NVARCHAR(50),
    THANHPHO NVARCHAR(50),
    CONSTRAINT PK_SB PRIMARY KEY (MASB)
);

-- 7. BẢNG CHUYẾN BAY
CREATE TABLE CHUYENBAY (
    MACB CHAR(10) NOT NULL,
    TRANGTHAI NVARCHAR(20),
    MAMB CHAR(10),
    CONSTRAINT PK_CB PRIMARY KEY (MACB),
    CONSTRAINT FK_CB_MB FOREIGN KEY (MAMB) REFERENCES MAYBAY(MAMB)
);

-- 8. BẢNG LỘ TRÌNH
CREATE TABLE LOTRINH (
    MALT CHAR(10) NOT NULL,
    MACB CHAR(10) NOT NULL,
    SBDI CHAR(10) NOT NULL,
    SBDEN CHAR(10) NOT NULL,
    GIOCATCANH DATETIME NOT NULL,
    GIOHACANH DATETIME NOT NULL,
    CONSTRAINT PK_LT PRIMARY KEY (MALT),
    CONSTRAINT FK_LT_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB),
    CONSTRAINT FK_LT_SBDI FOREIGN KEY (SBDI) REFERENCES SANBAY(MASB),
    CONSTRAINT FK_LT_SBDEN FOREIGN KEY (SBDEN) REFERENCES SANBAY(MASB)
);

-- 9. BẢNG GHẾ
CREATE TABLE GHE (
    MAGHE CHAR(10) NOT NULL,
    MAMB CHAR(10) NOT NULL,
    TENGHE NVARCHAR(10),
    HANGGHE NVARCHAR(20),
    CONSTRAINT PK_GHE PRIMARY KEY (MAGHE),
    CONSTRAINT FK_GHE_MB FOREIGN KEY (MAMB) REFERENCES MAYBAY(MAMB)
);

-- 10. BẢNG HANGGHE_GIA
CREATE TABLE HANGGHE_GIA (
    MAHG CHAR(10) NOT NULL,
    MACB CHAR(10) NOT NULL,
    HANGGHE NVARCHAR(20),
    GIA_COSO MONEY,
    CONSTRAINT PK_HGG PRIMARY KEY (MAHG),
    CONSTRAINT FK_HGG_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB)
);

-- 11. BẢNG VÉ MÁY BAY
CREATE TABLE VEMAYBAY (
    MAVE CHAR(10) NOT NULL,
    MAGHE CHAR(10) NOT NULL,
    MAHG CHAR(10),
    GIAVE MONEY,
    MACB CHAR(10),
    MANV CHAR(10),
    CONSTRAINT PK_VMB PRIMARY KEY (MAVE),
    CONSTRAINT FK_VMB_GHE FOREIGN KEY (MAGHE) REFERENCES GHE(MAGHE),
    CONSTRAINT FK_VMB_HGG FOREIGN KEY (MAHG) REFERENCES HANGGHE_GIA(MAHG),
    CONSTRAINT FK_VMB_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB),
    CONSTRAINT FK_VMB_NV FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV)
);

-- 12. BẢNG GIỎ HÀNG
CREATE TABLE GIOHANG (
    MAGH CHAR(10) NOT NULL,
    MAKH CHAR(10) NOT NULL,
    NGAYTAO DATETIME DEFAULT GETDATE(),
    TRANGTHAI NVARCHAR(20) DEFAULT N'Đang chọn',
    CONSTRAINT PK_GH PRIMARY KEY (MAGH),
    CONSTRAINT FK_GH_KH FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH)
);

-- 13. BẢNG PHIẾU ĐẶT VÉ
CREATE TABLE PHIEUDATVE (
    MAPHIEU CHAR(10) NOT NULL,
    NGLAP DATE,
    MANV CHAR(10),
    MAKH CHAR(10),
    TRANGTHAI NVARCHAR(30),
    MAGH CHAR(10),
    CONSTRAINT PK_PDV PRIMARY KEY (MAPHIEU),
    CONSTRAINT FK_PDV_NV FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV),
    CONSTRAINT FK_PDV_KH FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH),
    CONSTRAINT FK_PDV_GH FOREIGN KEY (MAGH) REFERENCES GIOHANG(MAGH)
);

-- 14. BẢNG CHI TIẾT VÉ
CREATE TABLE CHITIETVE (
    MAVE CHAR(10) NOT NULL,
    MAPHIEU CHAR(10) NOT NULL,
    NGAYDAT DATE,
    GIATIEN MONEY,
    CONSTRAINT PK_CTV PRIMARY KEY (MAVE, MAPHIEU),
    CONSTRAINT FK_CTV_VE FOREIGN KEY (MAVE) REFERENCES VEMAYBAY(MAVE),
    CONSTRAINT FK_CTV_PDV FOREIGN KEY (MAPHIEU) REFERENCES PHIEUDATVE(MAPHIEU)
);

-- 15. BẢNG HÀNH LÝ
CREATE TABLE HANHLY (
    MAHL CHAR(10) NOT NULL,
    MAVE CHAR(10) NOT NULL,
    MACB CHAR(10) NOT NULL,
    LOAIHL NVARCHAR(20),
    KHOILUONG FLOAT,
    KICHTHUOC FLOAT,
    CONSTRAINT PK_HL PRIMARY KEY (MAHL),
    CONSTRAINT FK_HL_KH FOREIGN KEY (MAVE) REFERENCES VEMAYBAY(MAVE),
    CONSTRAINT FK_HL_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB)
);

-- 16. BẢNG THỐNG KÊ DOANH THU
CREATE TABLE THONGKE_DOANHTHU (
    MACB CHAR(10) NOT NULL,
    NGAY DATE NOT NULL,
    TONGDOANHTHU MONEY,
    SOLUONGVE INT,
    CONSTRAINT PK_TK PRIMARY KEY (MACB, NGAY),
    CONSTRAINT FK_TK_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB)
);

-- 17. BẢNG LOG TRUY CẬP
CREATE TABLE LOG_TRUYCAP (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MANV CHAR(10),
    TENDANGNHAP NVARCHAR(50),
    THAOTAC NVARCHAR(50),
    BANGTACTDONG NVARCHAR(50),
    THOIGIAN DATETIME,
    TRANGTHAI NVARCHAR(50)
);

-- 18. BẢNG CHECKIN
CREATE TABLE CHECKIN (
    MACHECKIN CHAR(10) NOT NULL,
    MAVE CHAR(10) NOT NULL,
    MAKH CHAR(10) NOT NULL,
    MACB CHAR(10) NOT NULL,
    TRANGTHAI NVARCHAR(30),
    THOIGIAN_CHECKIN DATETIME,
    CONSTRAINT PK_CI PRIMARY KEY (MACHECKIN),
    CONSTRAINT FK_CI_VE FOREIGN KEY (MAVE) REFERENCES VEMAYBAY(MAVE),
    CONSTRAINT FK_CI_KH FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH),
    CONSTRAINT FK_CI_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB)
);

-- 19. BẢNG CHI TIẾT GIỎ HÀNG
CREATE TABLE GIOHANG_CHITIET (
    MAGH CHAR(10) NOT NULL,
    MACB CHAR(10) NOT NULL,
    SOLUONG INT DEFAULT 1,
    GIATIEN MONEY,
    THOIGIANGIU DATETIME DEFAULT DATEADD(MINUTE, 15, GETDATE()),
    CONSTRAINT PK_GHCT PRIMARY KEY (MAGH, MACB),
    CONSTRAINT FK_GHCT_GH FOREIGN KEY (MAGH) REFERENCES GIOHANG(MAGH),
    CONSTRAINT FK_GHCT_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB)
);

-- 20. BẢNG GIOHANG_HANHKHACH
CREATE TABLE GIOHANG_HANHKHACH (
    MAHK CHAR(20) NOT NULL PRIMARY KEY,
    SOGHE CHAR(10) NOT NULL,
    MAGH CHAR(10) NOT NULL,
    MACB CHAR(10) NOT NULL,
    HANGGHE NVARCHAR(20) NOT NULL,
    TENHANHKHACH NVARCHAR(100),
    NGAYSINH DATE,
    GIOITINH NVARCHAR(3),
    EMAIL VARCHAR(100),
    SDT VARCHAR(10),
    HANGLY_XACHTAY INT DEFAULT 0,
    HANHLYKYGUI INT DEFAULT 0,
    CONSTRAINT FK_GHKH_GH FOREIGN KEY (MAGH) REFERENCES GIOHANG(MAGH),
    CONSTRAINT FK_GHKH_CB FOREIGN KEY (MACB) REFERENCES CHUYENBAY(MACB)
);
GO

-- ======================================================
-- 3. CHÈN DỮ LIỆU MẪU
-- ======================================================

-- DỮ LIỆU BẢNG CHỨC VỤ
INSERT INTO CHUCVU (MACV, TENCV) VALUES
('CV01', N'Nhân viên bán vé'), ('CV02', N'Nhân viên check-in'),
('CV03', N'Tiếp viên trưởng'), ('CV04', N'Phi công cơ trưởng'),
('CV05', N'Quản lý cấp cao'), ('CV06', N'Kỹ thuật viên bảo trì'),
('CV07', N'Nhân viên hành lý'), ('CV08', N'Nhân viên kế toán'),
('CV09', N'Giám đốc sân bay'), ('CV10', N'Nhân viên Marketing');

-- DỮ LIỆU BẢNG NHÂN VIÊN
INSERT INTO NHANVIEN (MANV, TENNV, SDT, MACV) VALUES
('NV01', N'Nguyễn Văn A', '0901234567', 'CV01'), ('NV02', N'Lê Thị B', '0902345678', 'CV02'),
('NV03', N'Trần Văn C', '0903456789', 'CV03'), ('NV04', N'Phạm Văn D', '0904567890', 'CV04'),
('NV05', N'Hoàng Thị E', '0905678901', 'CV05'), ('NV06', N'Võ Minh F', '0906789012', 'CV06'),
('NV07', N'Đỗ Thanh G', '0907890123', 'CV07'), ('NV08', N'Bùi Văn H', '0908901234', 'CV08'),
('NV09', 'Tô Thị I', '0909012345', 'CV09'), ('NV10', N'Ngô Đức K', '0910123456', 'CV10');

-- DỮ LIỆU BẢNG KHÁCH HÀNG
INSERT INTO KHACHHANG (MAKH, TENKH, EMAIL, MATKHAU, SDT, DIACHI, GTINH, NGSINH, QUOCGIA) VALUES
('KH01', N'Nguyễn Minh Khoa', 'khoa@gmail.com', '123456', '0911111111', N'Hà Nội', N'Nam', '1990-05-12', N'Việt Nam'),
('KH02', N'Lê Thanh Hoa', 'hoa@gmail.com', 'hoa2025', '0922222222', N'Hồ Chí Minh', N'Nữ', '1995-07-20', N'Việt Nam'),
('KH03', N'John Smith', 'john@gmail.com', 'johnpass', '0933333333', N'New York', N'Nam', '1988-09-15', N'Mỹ'),
('KH04', N'Nguyễn Văn Nam', 'nam@gmail.com', 'namvip', '0944444444', N'Đà Nẵng', N'Nam', '1992-01-30', N'Việt Nam'),
('KH05', N'Lý Thu Hà', 'ha@gmail.com', 'ha2025', '0955555555', N'Hải Phòng', N'Nữ', '1998-11-25', N'Việt Nam'),
('KH06', N'Trần Đức Tài', 'tai@gmail.com', 'tai123', '0966666666', N'Cần Thơ', N'Nam', '1985-03-10', N'Việt Nam'),
('KH07', N'Mai Hồng Nhung', 'nhung@gmail.com', 'nhungvip', '0977777777', N'Huế', N'Nữ', '1993-08-05', N'Việt Nam'),
('KH08', N'David Lee', 'david@gmail.com', 'davidp', '0988888888', N'Seoul', N'Nam', '1991-12-24', N'Hàn Quốc'),
('KH09', N'Phan Thúy An', 'an@gmail.com', 'anpass', '0999999999', N'Quy Nhơn', N'Nữ', '2000-04-17', N'Việt Nam'),
('KH10', N'Nguyễn Hùng Sơn', 'son@gmail.com', 'sonpass', '0900000000', N'Nha Trang', N'Nam', '1980-06-28', N'Việt Nam');

-- DỮ LIỆU BẢNG MÁY BAY
INSERT INTO MAYBAY (MAMB, TENMB, HANG, TONGSOGHE) VALUES
('MB01', N'Airbus A320', N'Airbus', 180), ('MB02', N'Boeing 737-800', N'Boeing', 150),
('MB03', N'Airbus A321', N'Airbus', 220), ('MB04', N'Boeing 787-9', N'Boeing', 280),
('MB05', N'ATR 72-600', N'ATR', 80), ('MB06', N'Embraer 190', N'Embraer', 100),
('MB07', N'Airbus A350', N'Airbus', 350), ('MB08', N'Boeing 777-300ER', N'Boeing', 380),
('MB09', N'Bombardier CRJ900', N'Bombardier', 90), ('MB10', N'Airbus A330', N'Airbus', 300);

-- DỮ LIỆU BẢNG CAUHINH_GHE
INSERT INTO CAUHINH_GHE (MACG, MAMB, HANGGHE, SOLUONG) VALUES
('CG01', 'MB01', N'Phổ thông', 170), ('CG02', 'MB01', N'Thương gia', 10),
('CG03', 'MB02', N'Phổ thông', 140), ('CG04', 'MB02', N'Thương gia', 10),
('CG05', 'MB03', N'Phổ thông', 200), ('CG06', 'MB03', N'Thương gia', 20),
('CG07', 'MB04', N'Phổ thông', 250), ('CG08', 'MB04', N'Thương gia', 30),
('CG09', 'MB05', N'Phổ thông', 80), ('CG10', 'MB07', N'Hạng nhất', 10);

-- DỮ LIỆU BẢNG SÂN BAY
INSERT INTO SANBAY (MASB, TENSB, THANHPHO) VALUES
('SB01', N'Nội Bài', N'Hà Nội'), ('SB02', N'Tân Sơn Nhất', N'Hồ Chí Minh'),
('SB03', N'Đà Nẵng', N'Đà Nẵng'), ('SB04', N'Cam Ranh', N'Khánh Hòa'),
('SB05', N'Cát Bi', N'Hải Phòng'), ('SB06', N'Cần Thơ', N'Cần Thơ'),
('SB07', N'Phú Bài', N'Huế'), ('SB08', N'Phú Quốc', N'Kiên Giang'),
('SB09', N'Vinh', N'Nghệ An'), ('SB10', N'Liên Khương', N'Lâm Đồng');

-- DỮ LIỆU BẢNG CHUYẾN BAY
INSERT INTO CHUYENBAY (MACB, TRANGTHAI, MAMB) VALUES
('CB01', N'Đúng giờ', 'MB01'), ('CB02', N'Chậm trễ', 'MB02'),
('CB03', N'Đang bay', 'MB03'), ('CB04', N'Đúng giờ', 'MB04'),
('CB05', N'Hủy', 'MB05'), ('CB06', N'Sắp khởi hành', 'MB06'),
('CB07', N'Đúng giờ', 'MB07'), ('CB08', N'Chậm trễ', 'MB08'),
('CB09', N'Đúng giờ', 'MB09'), ('CB10', N'Đang bay', 'MB10');

-- DỮ LIỆU BẢNG LỘ TRÌNH
INSERT INTO LOTRINH (MALT, MACB, SBDI, SBDEN, GIOCATCANH, GIOHACANH) VALUES
('LT01', 'CB01', 'SB02', 'SB01', '2025-09-20 08:00:00', '2025-09-20 10:00:00'),
('LT02', 'CB02', 'SB03', 'SB02', '2025-09-21 14:00:00', '2025-09-21 16:30:00'),
('LT03', 'CB03', 'SB01', 'SB03', '2025-09-22 09:00:00', '2025-09-22 11:00:00'),
('LT04', 'CB04', 'SB01', 'SB02', '2025-09-20 11:00:00', '2025-09-20 13:00:00'),
('LT05', 'CB05', 'SB05', 'SB01', '2025-09-24 06:00:00', '2025-09-24 07:30:00'),
('LT06', 'CB06', 'SB03', 'SB06', '2025-10-15 12:00:00', '2025-10-15 13:30:00'),
('LT07', 'CB07', 'SB02', 'SB01', '2025-10-16 15:00:00', '2025-10-16 17:00:00'),
('LT08', 'CB08', 'SB02', 'SB03', '2025-10-17 18:00:00', '2025-10-17 20:30:00'),
('LT09', 'CB09', 'SB07', 'SB02', '2025-10-18 07:30:00', '2025-10-18 09:00:00'),
('LT10', 'CB10', 'SB03', 'SB05', '2025-10-19 10:00:00', '2025-10-19 11:30:00');

-- DỮ LIỆU BẢNG GHẾ
INSERT INTO GHE (MAGHE, MAMB, TENGHE, HANGGHE) VALUES
('GH01','MB01','12A',N'Phổ thông'), ('GH02','MB01','12B',N'Phổ thông'),
('GH03','MB01','01A',N'Thương gia'), ('GH04','MB02','14B',N'Phổ thông'),
('GH05','MB03','15C',N'Phổ thông'), ('GH06','MB04','05D',N'Phổ thông'),
('GH07','MB06','10A',N'Phổ thông'), ('GH08','MB07','01D',N'Hạng nhất'),
('GH09','MB08','20E',N'Phổ thông'), ('GH10','MB09','03B',N'Thương gia');

-- DỮ LIỆU BẢNG HANGGHE_GIA
INSERT INTO HANGGHE_GIA (MAHG, MACB, HANGGHE, GIA_COSO) VALUES
('HG01', 'CB01', N'Phổ thông', 1950000), ('HG02', 'CB01', N'Thương gia', 4500000),
('HG03', 'CB02', N'Phổ thông', 1500000), ('HG04', 'CB02', N'Thương gia', 3800000),
('HG05', 'CB03', N'Phổ thông', 1800000), ('HG06', 'CB04', N'Phổ thông', 2000000),
('HG07', 'CB04', N'Thương gia', 4600000), ('HG08', 'CB06', N'Phổ thông', 1300000),
('HG09', 'CB07', N'Thương gia', 4000000), ('HG10', 'CB09', N'Phổ thông', 1750000);

-- DỮ LIỆU BẢNG VÉ MÁY BAY
INSERT INTO VEMAYBAY (MAVE, MAGHE, MAHG, GIAVE, MACB, MANV) VALUES
('VE01','GH01','HG01',1950000,'CB01','NV01'), ('VE02','GH02','HG01',2050000,'CB01','NV01'),
('VE03','GH03','HG02',4800000,'CB01','NV02'), ('VE04','GH04','HG03',1500000,'CB02','NV03'),
('VE05','GH05','HG05',1800000,'CB03','NV04'), ('VE06','GH06','HG06',2000000,'CB04','NV01'),
('VE07','GH07','HG08',1300000,'CB06','NV05'), ('VE08','GH08',NULL,5000000,'CB07','NV05'),
('VE09','GH09','HG04',3900000,'CB08','NV02'), ('VE10','GH10','HG10',1750000,'CB09','NV03');

-- DỮ LIỆU BẢNG GIỎ HÀNG
INSERT INTO GIOHANG (MAGH, MAKH, NGAYTAO, TRANGTHAI) VALUES
('GH01', 'KH01', '2025-11-18 10:00:00', N'Đã thanh toán'), 
('GH02', 'KH02', '2025-11-17 11:30:00', N'Đã thanh toán'), 
('GH03', 'KH03', '2025-11-18 15:45:00', N'Đã thanh toán'), 
('GH04', 'KH04', '2025-11-16 09:00:00', N'Đã thanh toán'), 
('GH05', 'KH05', '2025-11-18 18:20:00', N'Đã thanh toán'), 
('GH06', 'KH06', '2025-11-15 14:00:00', N'Đã thanh toán'), 
('GH07', 'KH07', '2025-11-14 12:00:00', N'Đã thanh toán'), 
('GH08', 'KH08', '2025-11-18 20:00:00', N'Đang chọn'),
('GH09', 'KH09', '2025-11-13 08:00:00', N'Đã hết hạn'),
('GH10', 'KH10', '2025-11-18 21:30:00', N'Đang chọn');

-- DỮ LIỆU BẢNG PHIẾU ĐẶT VÉ
INSERT INTO PHIEUDATVE (MAPHIEU, NGLAP, MANV, MAKH, TRANGTHAI, MAGH) VALUES
('PD01', '2025-09-10', 'NV01', 'KH01', N'Đã thanh toán', 'GH01'), 
('PD02', '2025-09-11', 'NV01', 'KH02', N'Đã thanh toán', 'GH02'), 
('PD03', '2025-09-12', 'NV02', 'KH03', N'Đã thanh toán', 'GH01'), 
('PD04', '2025-09-13', 'NV03', 'KH04', N'Hủy', 'GH04'), 
('PD05', '2025-09-14', 'NV04', 'KH05', N'Đã thanh toán', 'GH03'), 
('PD06', '2025-10-01', 'NV05', 'KH06', N'Đã thanh toán', 'GH02'), 
('PD07', '2025-10-02', 'NV05', 'KH07', N'Đã thanh toán', 'GH05'), 
('PD08', '2025-10-03', 'NV02', 'KH08', N'Đang xử lý', 'GH07'), 
('PD09', '2025-10-04', 'NV03', 'KH09', N'Đã thanh toán', 'GH06'), 
('PD10', '2025-10-05', 'NV04', 'KH10', N'Đang xử lý', 'GH10');

-- DỮ LIỆU BẢNG CHI TIẾT VÉ
INSERT INTO CHITIETVE (MAVE, MAPHIEU, NGAYDAT, GIATIEN) VALUES
('VE01', 'PD01', '2025-09-10', 1950000), ('VE02', 'PD01', '2025-09-10', 2050000),
('VE03', 'PD03', '2025-09-12', 4800000), ('VE04', 'PD04', '2025-09-13', 1500000),
('VE05', 'PD05', '2025-09-14', 1800000), ('VE06', 'PD02', '2025-09-11', 2000000),
('VE07', 'PD06', '2025-10-01', 1300000), ('VE08', 'PD07', '2025-10-02', 5000000),
('VE09', 'PD08', '2025-10-03', 3900000), ('VE10', 'PD09', '2025-10-04', 1750000);

-- DỮ LIỆU BẢNG HÀNH LÝ
INSERT INTO HANHLY (MAHL, MAVE, MACB, LOAIHL, KHOILUONG, KICHTHUOC) VALUES
('HL01', 'VE01', 'CB01', N'Xách tay', 7, 50), ('HL02', 'VE06', 'CB04', N'Ký gửi', 20, 100),
('HL03', 'VE03', 'CB01', N'Ký gửi', 25, 120), ('HL04', 'VE04', 'CB02', N'Xách tay', 10, 60),
('HL05', 'VE05', 'CB03', N'Ký gửi', 15, 80), ('HL06', 'VE07', 'CB06', N'Xách tay', 5, 40),
('HL07', 'VE08', 'CB07', N'Ký gửi', 30, 150), ('HL08', 'VE09', 'CB08', N'Xách tay', 8, 55),
('HL09', 'VE10', 'CB09', N'Ký gửi', 12, 70), ('HL10', 'VE02', 'CB01', N'Xách tay', 6, 45);

-- DỮ LIỆU BẢNG THỐNG KÊ DOANH THU
INSERT INTO THONGKE_DOANHTHU (MACB, NGAY, TONGDOANHTHU, SOLUONGVE) VALUES
('CB01', '2025-09-10', 4000000, 2), ('CB01', '2025-09-12', 4800000, 1),
('CB02', '2025-09-13', 1500000, 1), ('CB03', '2025-09-14', 1800000, 1),
('CB04', '2025-09-11', 2000000, 1), ('CB06', '2025-10-01', 1300000, 1),
('CB07', '2025-10-02', 5000000, 1), ('CB08', '2025-10-03', 3900000, 1),
('CB09', '2025-10-04', 1750000, 1), ('CB10', '2025-10-05', 0, 0);

-- DỮ LIỆU BẢNG LOG TRUY CẬP
INSERT INTO LOG_TRUYCAP (MANV, TENDANGNHAP, THAOTAC, BANGTACTDONG, THOIGIAN, TRANGTHAI) VALUES
('NV01', 'nva_sale', N'Thêm', 'PHIEUDATVE', '2025-09-10 10:00:00', N'Thành công'),
('NV02', 'ltb_checkin', N'Cập nhật', 'CHECKIN', '2025-09-20 07:10:00', N'Thành công'),
('NV04', 'pvd_pilot', N'Xem', 'CHUYENBAY', '2025-09-22 08:30:00', N'Thành công'),
('NV06', 'vmf_tech', N'Sửa', 'MAYBAY', '2025-10-01 14:20:00', N'Thành công'),
('NV07', 'dtg_bag', N'Thêm', 'HANHLY', '2025-10-02 09:00:00', N'Thành công'),
('NV08', 'bvh_acc', N'Xem', 'THONGKE_DOANHTHU', '2025-10-03 16:00:00', N'Thành công'),
('NV01', 'nva_sale', N'Cập nhật', 'PHIEUDATVE', '2025-10-04 10:30:00', N'Thành công'),
('NV05', 'hte_manager', N'Thêm', 'CHUCVU', '2025-10-05 11:00:00', N'Thành công'),
('NV09', 'tti_director', N'Xem', 'LOTRINH', '2025-10-06 09:15:00', N'Thành công'),
('NV10', 'ndk_market', N'Sửa', 'KHACHHANG', '2025-10-07 13:45:00', N'Thành công');

-- DỮ LIỆU BẢNG CHECKIN
INSERT INTO CHECKIN (MACHECKIN, MAVE, MAKH, MACB, TRANGTHAI, THOIGIAN_CHECKIN) VALUES
('CI01', 'VE01', 'KH01', 'CB01', N'Đã check-in', '2025-09-20 07:00:00'),
('CI02', 'VE02', 'KH01', 'CB01', N'Đã check-in', '2025-09-20 07:05:00'),
('CI03', 'VE03', 'KH03', 'CB01', N'Đã check-in', '2025-09-20 07:15:00'),
('CI04', 'VE06', 'KH02', 'CB04', N'Đã check-in', '2025-09-20 10:00:00'),
('CI05', 'VE05', 'KH05', 'CB03', N'Chưa check-in', NULL),
('CI06', 'VE07', 'KH06', 'CB06', N'Đã check-in', '2025-10-15 11:00:00'),
('CI07', 'VE08', 'KH07', 'CB07', N'Chưa check-in', NULL),
('CI08', 'VE10', 'KH09', 'CB09', N'Đã check-in', '2025-10-18 06:45:00'),
('CI09', 'VE04', 'KH04', 'CB02', N'Hủy check-in', NULL),
('CI10', 'VE09', 'KH08', 'CB08', N'Chưa check-in', NULL);

-- DỮ LIỆU BẢNG CHI TIẾT GIỎ HÀNG
INSERT INTO GIOHANG_CHITIET (MAGH, MACB, SOLUONG, GIATIEN, THOIGIANGIU) VALUES
('GH01', 'CB07', 1, 4500000, DATEADD(MINUTE, 15, '2025-11-18 10:00:00')),
('GH03', 'CB04', 2, 4000000, DATEADD(MINUTE, 15, '2025-11-18 15:45:00')),
('GH05', 'CB09', 1, 1950000, DATEADD(MINUTE, 15, '2025-11-18 18:20:00')),
('GH08', 'CB01', 3, 5850000, DATEADD(MINUTE, 15, '2025-11-18 20:00:00')),
('GH10', 'CB10', 1, 2200000, DATEADD(MINUTE, 15, '2025-11-18 21:30:00')),
('GH02', 'CB02', 1, 1700000, '2025-11-17 11:45:00'),
('GH04', 'CB03', 2, 3600000, '2025-11-16 09:15:00'),
('GH06', 'CB08', 1, 3800000, '2025-11-15 14:15:00'),
('GH07', 'CB06', 1, 1300000, '2025-11-14 12:15:00'),
('GH09', 'CB01', 1, 4500000, '2025-11-13 08:15:00');

-- DỮ LIỆU BẢNG GIOHANG_HANHKHACH (Đã cập nhật đủ cột SDT và định dạng)
INSERT INTO GIOHANG_HANHKHACH 
(MAHK, SOGHE, MAGH, MACB, HANGGHE, TENHANHKHACH, NGAYSINH, GIOITINH, EMAIL, SDT, HANGLY_XACHTAY, HANHLYKYGUI) 
VALUES
('HK01GH01', '01A', 'GH01', 'CB07', N'Hạng nhất',  N'Nguyễn Minh Khoa', '1990-05-12', N'Nam', 'khoa@gmail.com',   '0911111111', 0, 1),
('HK01GH03', '10F', 'GH03', 'CB04', N'Phổ thông',  N'John Smith',       '1988-09-15', N'Nam', 'john@gmail.com',   '0933333333', 0, 0),
('HK02GH03', '10E', 'GH03', 'CB04', N'Phổ thông',  N'Mary Jane',        '1995-02-20', N'Nữ',  'mary@example.com', '0933333334', 0, 0),
('HK01GH05', '05C', 'GH05', 'CB09', N'Phổ thông',  N'Lý Thu Hà',        '1998-11-25', N'Nữ',  'ha@gmail.com',     '0955555555', 1, 0),
('HK01GH08', '20A', 'GH08', 'CB01', N'Phổ thông',  N'David Lee',        '1991-12-24', N'Nam', 'david@gmail.com',  '0988888888', 0, 0),
('HK02GH08', '20B', 'GH08', 'CB01', N'Phổ thông',  N'Lee Min Ho',       '1987-06-22', N'Nam', 'lmh@example.com',  '0988888881', 0, 0),
('HK03GH08', '20C', 'GH08', 'CB01', N'Phổ thông',  N'Kim Ji Won',       '1992-10-19', N'Nữ',  'kjw@example.com',  '0988888882', 0, 0),
('HK01GH10', '15D', 'GH10', 'CB10', N'Phổ thông',  N'Nguyễn Hùng Sơn',  '1980-06-28', N'Nam', 'son@gmail.com',    '0900000000', 0, 2),
('HK01GH02', '12A', 'GH02', 'CB02', N'Phổ thông',  N'Lê Thanh Hoa',     '1995-07-20', N'Nữ',  'hoa@gmail.com',    '0922222222', 1, 0),
('HK01GH04', '18F', 'GH04', 'CB03', N'Phổ thông',  N'Nguyễn Văn Nam',   '1992-01-30', N'Nam', 'nam@gmail.com',    '0944444444', 0, 0);
GO



-- 1. Trigger: Chặn việc thêm ghế nếu ghế đó đã có người đặt hoặc mua
CREATE TRIGGER trg_CheckSeatAvailability
ON GIOHANG_HANHKHACH
FOR INSERT
AS
BEGIN
    DECLARE @MaCB VARCHAR(20), @SoGhe VARCHAR(10);
    SELECT @MaCB = MACB, @SoGhe = SOGHE FROM inserted;

    -- Kiểm tra trong vé đã bán
    IF EXISTS (SELECT 1 FROM VEMAYBAY v JOIN GHE g ON v.MAGHE = g.MAGHE 
               WHERE v.MACB = @MaCB AND g.TENGHE = @SoGhe)
    BEGIN
        RAISERROR(N'Ghế này đã được bán!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Kiểm tra trong giỏ hàng của người khác (còn hạn giữ chỗ)
    IF EXISTS (SELECT 1 FROM GIOHANG_HANHKHACH hk 
               JOIN GIOHANG_CHITIET ct ON hk.MAGH = ct.MAGH AND hk.MACB = ct.MACB
               WHERE hk.MACB = @MaCB AND hk.SOGHE = @SoGhe 
               AND ct.THOIGIANGIU > GETDATE())
    BEGIN
        RAISERROR(N'Ghế này đang được giữ bởi khách hàng khác!', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO
-----------------------------
--2. function tự sinh mã
CREATE FUNCTION fn_TuDongTangMa (@TenBang VARCHAR(50), @CotMa VARCHAR(50), @TienTo VARCHAR(5))
RETURNS VARCHAR(20)
AS
BEGIN
    DECLARE @MaxMa VARCHAR(20)
    DECLARE @NewID VARCHAR(20)
    DECLARE @SoThuTu INT
    
    -- Lấy mã lớn nhất hiện tại dựa trên Tiền tố (Logic đơn giản hóa cho ví dụ)
    DECLARE @SQL NVARCHAR(MAX)
    
    IF @TenBang = 'PHIEUDATVE'
        SELECT TOP 1 @MaxMa = MAPHIEU FROM PHIEUDATVE WHERE MAPHIEU LIKE @TienTo + '%' ORDER BY LEN(MAPHIEU) DESC, MAPHIEU DESC
    ELSE IF @TenBang = 'VEMAYBAY'
        SELECT TOP 1 @MaxMa = MAVE FROM VEMAYBAY WHERE MAVE LIKE @TienTo + '%' ORDER BY LEN(MAVE) DESC, MAVE DESC
    ELSE IF @TenBang = 'GHE'
        SELECT TOP 1 @MaxMa = MAGHE FROM GHE WHERE MAGHE LIKE @TienTo + '%' ORDER BY LEN(MAGHE) DESC, MAGHE DESC

    IF @MaxMa IS NULL
        SET @SoThuTu = 1
    ELSE
        -- Lấy phần số: Cắt bỏ tiền tố và chuyển sang INT
        SET @SoThuTu = CAST(SUBSTRING(@MaxMa, LEN(@TienTo) + 1, LEN(@MaxMa)) AS INT) + 1

    -- Tạo mã mới: Tiền tố + Số thứ tự (được padding số 0, ví dụ 001)
    SET @NewID = @TienTo + RIGHT('000' + CAST(@SoThuTu AS VARCHAR(10)), 3)
    
    RETURN @NewID
END;
GO
-------------------------
-- 3. procedure xử lý đặt vé từ giỏ hàng
CREATE PROCEDURE sp_DatVeTuGioHang
    @MaGH CHAR(10),
    @MaCB CHAR(10)
AS
BEGIN    
    -- BẮT ĐẦU TRANSACTION
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 1. Tạo Phiếu Đặt Vé
        DECLARE @MaPhieu CHAR(10) = dbo.fn_TuDongTangMa('PHIEUDATVE', 'MAPHIEU', 'PD');
        DECLARE @MaKH CHAR(10);
        
        SELECT @MaKH = MAKH FROM GIOHANG WHERE MAGH = @MaGH;

        INSERT INTO PHIEUDATVE (MAPHIEU, NGLAP, MANV, MAKH, TRANGTHAI, MAGH)
        VALUES (@MaPhieu, GETDATE(), NULL, @MaKH, N'Đã thanh toán', @MaGH);

        -- 2. Khai báo biến cho CURSOR
        DECLARE @MaHK CHAR(20), @TenHK NVARCHAR(100), @SoGhe CHAR(10), @HangGhe NVARCHAR(20);
        DECLARE @GiaVe MONEY, @MaMB CHAR(10), @MaGhe CHAR(10), @MaVe CHAR(10), @MaHG CHAR(10);

        -- Lấy MaMB từ MaCB
        SELECT @MaMB = MAMB FROM CHUYENBAY WHERE MACB = @MaCB;

        -- 3. Khởi tạo CURSOR duyệt qua từng hành khách trong giỏ hàng
        DECLARE cur_HanhKhach CURSOR FOR
            SELECT MAHK, TENHANHKHACH, SOGHE, HANGGHE
            FROM GIOHANG_HANHKHACH
            WHERE MAGH = @MaGH AND MACB = @MaCB;

        OPEN cur_HanhKhach;
        FETCH NEXT FROM cur_HanhKhach INTO @MaHK, @TenHK, @SoGhe, @HangGhe;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- 3a. Kiểm tra Ghế đã tồn tại chưa, nếu chưa thì tạo mới
            SELECT @MaGhe = MAGHE FROM GHE WHERE MAMB = @MaMB AND TENGHE = @SoGhe;
            
            IF @MaGhe IS NULL
            BEGIN
                SET @MaGhe = dbo.fn_TuDongTangMa('GHE', 'MAGHE', 'GH');
                INSERT INTO GHE (MAGHE, MAMB, TENGHE, HANGGHE)
                VALUES (@MaGhe, @MaMB, @SoGhe, @HangGhe);
            END

            -- 3b. Lấy giá vé và mã hạng ghế
            SELECT @GiaVe = GIA_COSO, @MaHG = MAHG 
            FROM HANGGHE_GIA 
            WHERE MACB = @MaCB AND HANGGHE = @HangGhe;

            -- 3c. Tạo Vé Máy Bay (Sinh mã ngẫu nhiên giống C# hoặc tự tăng)
            SET @MaVe = dbo.fn_TuDongTangMa('VEMAYBAY', 'MAVE', 'VE');
            
            -- Insert Vé
            INSERT INTO VEMAYBAY (MAVE, MAGHE, MAHG, GIAVE, MACB, MANV)
            VALUES (@MaVe, @MaGhe, @MaHG, @GiaVe, @MaCB, NULL);

            -- 3d. Tạo Chi Tiết Vé
            INSERT INTO CHITIETVE (MAVE, MAPHIEU, NGAYDAT, GIATIEN)
            VALUES (@MaVe, @MaPhieu, GETDATE(), @GiaVe);

            FETCH NEXT FROM cur_HanhKhach INTO @MaHK, @TenHK, @SoGhe, @HangGhe;
        END

        CLOSE cur_HanhKhach;
        DEALLOCATE cur_HanhKhach;

        -- 4. Cập nhật trạng thái Giỏ hàng
        UPDATE GIOHANG SET TRANGTHAI = N'Đã thanh toán' WHERE MAGH = @MaGH;

        -- Commit nếu không có lỗi
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi bất kỳ
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
--- chức năng thanh toán
-- 4. Trigger ngăn chặn trùng ghế
CREATE TRIGGER trg_KiemTraTrungGhe
ON VEMAYBAY
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu tồn tại vé khác (v) có cùng MACB và MAGHE với vé vừa thêm (i)
    IF EXISTS (
        SELECT 1
        FROM VEMAYBAY v
        JOIN INSERTED i ON v.MACB = i.MACB AND v.MAGHE = i.MAGHE
        WHERE v.MAVE <> i.MAVE -- Loại trừ chính dòng vừa insert
    )
    BEGIN
        -- Báo lỗi và Rollback
        RAISERROR(N'Lỗi: Ghế này đã được bán cho hành khách khác trên chuyến bay này!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO
-- Chức năng thống kê
-- 5. Trigger cập nhật doanh thu tự động (CŨ - ĐÃ THAY THẾ BẰNG trg_CapNhatDoanhThu Ở CUỐI FILE)
/*
CREATE TRIGGER trg_TuDongCapNhatDoanhThu
ON CHITIETVE
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @MaCB CHAR(10);
    DECLARE @NgayDat DATE;
    DECLARE @DoanhThu MONEY;

    -- Lấy thông tin từ dòng vừa insert
    SELECT 
        @MaCB = v.MACB, 
        @NgayDat = CAST(i.NGAYDAT AS DATE),
        @DoanhThu = i.GIATIEN
    FROM INSERTED i
    JOIN VEMAYBAY v ON i.MAVE = v.MAVE;

    -- Kiểm tra xem đã có bản ghi thống kê cho chuyến bay này vào ngày này chưa
    IF EXISTS (SELECT 1 FROM THONGKE_DOANHTHU WHERE MACB = @MaCB AND NGAY = @NgayDat)
    BEGIN
        -- Nếu có rồi: Cộng dồn
        UPDATE THONGKE_DOANHTHU
        SET TONGDOANHTHU = ISNULL(TONGDOANHTHU, 0) + @DoanhThu,
            SOLUONGVE = ISNULL(SOLUONGVE, 0) + 1
        WHERE MACB = @MaCB AND NGAY = @NgayDat;
    END
    ELSE
    BEGIN
        -- Nếu chưa có: Tạo mới
        INSERT INTO THONGKE_DOANHTHU (MACB, NGAY, TONGDOANHTHU, SOLUONGVE)
        VALUES (@MaCB, @NgayDat, @DoanhThu, 1);
    END
END;
*/
GO
-- 6 Function: Lấy báo cáo chi tiết
CREATE FUNCTION fn_LayBaoCaoDoanhThu (@TuNgay DATE, @DenNgay DATE)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        cb.MACB,
        mb.TENMB,
        lt.GIOCATCANH,
        COUNT(ctv.MAVE) AS SoVeBan,
        SUM(ctv.GIATIEN) AS TongDoanhThu
    FROM CHUYENBAY cb
    JOIN LOTRINH lt ON cb.MACB = lt.MACB
    JOIN MAYBAY mb ON cb.MAMB = mb.MAMB
    JOIN VEMAYBAY v ON cb.MACB = v.MACB
    JOIN CHITIETVE ctv ON v.MAVE = ctv.MAVE
    WHERE ctv.NGAYDAT BETWEEN @TuNgay AND @DenNgay
    GROUP BY cb.MACB, mb.TENMB, lt.GIOCATCANH
);
GO
-- Chức năng check-in
-- 7 Function Kiểm tra điều kiện thời gian
-- Mô tả sơ: có logic kiểm tra thời gian: Chỉ được check-in trước giờ bay 24h và đóng trước 1h trước khi bay. Hàm trả về: 1 (OK), 0 (Chưa đến giờ), -1 (Đã quá giờ).
CREATE FUNCTION fn_KiemTraThoiGianCheckIn (@MaCB CHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @GioCatCanh DATETIME;
    SELECT @GioCatCanh = GIOCATCANH FROM LOTRINH WHERE MACB = @MaCB;

    IF @GioCatCanh IS NULL RETURN -2; -- Lỗi không tìm thấy chuyến

    DECLARE @HienTai DATETIME = GETDATE();

    -- Check-in mở trước 24h
    IF @HienTai < DATEADD(HOUR, -24, @GioCatCanh)
        RETURN 0; -- Quá sớm, chưa mở

    -- Check-in đóng trước 1h
    IF @HienTai > DATEADD(HOUR, -1, @GioCatCanh)
        RETURN -1; -- Quá muộn, đã đóng quầy

    RETURN 1; -- Hợp lệ
END;
GO
-- 8 Procedure: Thực hiện Check-in
CREATE PROCEDURE sp_ThucHienCheckIn
    @MaVe CHAR(10),
    @MaKH CHAR(10)
AS
BEGIN
    DECLARE @MaCB CHAR(10);
    SELECT @MaCB = MACB FROM VEMAYBAY WHERE MAVE = @MaVe;

    -- 1. Gọi Function kiểm tra thời gian
    DECLARE @TrangThaiThoiGian INT = dbo.fn_KiemTraThoiGianCheckIn(@MaCB);

    IF @TrangThaiThoiGian = 0
    BEGIN
        RAISERROR(N'Chưa đến giờ check-in (chỉ mở trước 24h).', 16, 1);
        RETURN;
    END
    ELSE IF @TrangThaiThoiGian = -1
    BEGIN
        RAISERROR(N'Đã hết giờ check-in (đóng trước 1h).', 16, 1);
        RETURN;
    END

    -- 2. Thực hiện Check-in (Insert hoặc Update)
    IF EXISTS (SELECT 1 FROM CHECKIN WHERE MAVE = @MaVe)
    BEGIN
        UPDATE CHECKIN 
        SET TRANGTHAI = N'Đã check-in', THOIGIAN_CHECKIN = GETDATE()
        WHERE MAVE = @MaVe;
    END
    ELSE
    BEGIN
        DECLARE @NewID CHAR(10) = 'CI' + RIGHT(CAST(RAND() * 1000000 AS VARCHAR), 6); -- Sinh ID đơn giản
        INSERT INTO CHECKIN (MACHECKIN, MAVE, MAKH, MACB, TRANGTHAI, THOIGIAN_CHECKIN)
        VALUES (@NewID, @MaVe, @MaKH, @MaCB, N'Đã check-in', GETDATE());
    END
END;
GO
-- Chức năng thống kê
--9. Trigger: Chặn xóa chuyến bay đã có vé bán
CREATE TRIGGER trg_ChanXoaChuyenBayDaCoVe
ON CHUYENBAY
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaCB CHAR(10);
    SELECT @MaCB = MACB FROM DELETED;

    -- Kiểm tra nếu có vé máy bay tham chiếu đến chuyến này
    IF EXISTS (SELECT 1 FROM VEMAYBAY WHERE MACB = @MaCB)
    BEGIN
        RAISERROR(N'Không thể xóa chuyến bay này vì đã có vé được bán!', 16, 1);
        RETURN;
    END
    
    -- Kiểm tra nếu đang nằm trong giỏ hàng
    IF EXISTS (SELECT 1 FROM GIOHANG_CHITIET WHERE MACB = @MaCB)
    BEGIN
        RAISERROR(N'Không thể xóa chuyến bay này vì đang có khách chọn trong giỏ hàng!', 16, 1);
        RETURN;
    END

    -- Nếu an toàn, thực hiện xóa thủ công (Vì dùng INSTEAD OF nên phải viết lệnh xóa)
    -- Phải xóa bảng con LOTRINH trước (do FK)
    DELETE FROM LOTRINH WHERE MACB = @MaCB;
    DELETE FROM HANGGHE_GIA WHERE MACB = @MaCB;
    DELETE FROM CHUYENBAY WHERE MACB = @MaCB;
END;
GO
--10 Procedure: Cập nhật thông tin chuyến bay
CREATE PROCEDURE sp_CapNhatChuyenBay
    @MaCB CHAR(10),
    @MaMBNew CHAR(10),
    @TrangThaiNew NVARCHAR(20),
    @GioCatCanhNew DATETIME,
    @GioHaCanhNew DATETIME
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Update bảng CHUYENBAY
        UPDATE CHUYENBAY 
        SET MAMB = @MaMBNew, TRANGTHAI = @TrangThaiNew
        WHERE MACB = @MaCB;

        -- Update bảng LOTRINH
        UPDATE LOTRINH
        SET GIOCATCANH = @GioCatCanhNew, GIOHACANH = @GioHaCanhNew
        WHERE MACB = @MaCB;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- ======================================================
-- PHẦN THỐNG KÊ NÂNG CAO
-- ======================================================

-- ======================================================
-- 3.2.1. TRIGGER – Tự động cập nhật doanh thu
-- ======================================================
-- Mục đích: Tự động cập nhật bảng THONGKE_DOANHTHU mỗi khi có vé mới được thêm vào bảng CHITIETVE
-- Mô tả: Trigger này tự động chạy khi có INSERT vào bảng CHITIETVE.
--        Nó lấy mã chuyến bay, ngày đặt vé, tổng tiền và số lượng vé từ dữ liệu mới thêm.
--        Nếu ngày và chuyến bay đó đã có trong bảng thống kê, hệ thống cập nhật tổng doanh thu;
--        nếu chưa có, thêm mới bản ghi thống kê.
CREATE TRIGGER trg_CapNhatDoanhThu
ON CHITIETVE
FOR INSERT
AS
BEGIN
    DECLARE 
        @MaCB CHAR(10),
        @Ngay DATE,
        @TongTien MONEY,
        @SoLuong INT;
        
    SELECT 
        @MaCB = V.MACB,
        @Ngay = CAST(I.NGAYDAT AS DATE),
        @TongTien = SUM(I.GIATIEN),
        @SoLuong = COUNT(I.MAVE)
    FROM INSERTED I
    JOIN VEMAYBAY V ON I.MAVE = V.MAVE
    GROUP BY V.MACB, CAST(I.NGAYDAT AS DATE);

    IF EXISTS (SELECT 1 FROM THONGKE_DOANHTHU WHERE MACB = @MaCB AND NGAY = @Ngay)
    BEGIN
        UPDATE THONGKE_DOANHTHU
        SET 
            TONGDOANHTHU = TONGDOANHTHU + @TongTien,
            SOLUONGVE = SOLUONGVE + @SoLuong
        WHERE MACB = @MaCB AND NGAY = @Ngay;
    END
    ELSE
    BEGIN
        INSERT INTO THONGKE_DOANHTHU (MACB, NGAY, TONGDOANHTHU, SOLUONGVE)
        VALUES (@MaCB, @Ngay, @TongTien, @SoLuong);
    END
END;
GO

-- ======================================================
-- 3.2.2. STORED PROCEDURE – Tính doanh thu theo tháng
-- ======================================================
-- Mục đích: Tính tổng doanh thu trong một tháng cụ thể
-- Mô tả: Procedure này nhận vào tháng và năm, sau đó trả về tổng doanh thu thông qua biến OUTPUT
CREATE PROCEDURE sp_TinhTongDoanhThu_Thang
    @Thang INT,
    @Nam INT,
    @TongDoanhThu MONEY OUTPUT
AS
BEGIN
    SELECT @TongDoanhThu = ISNULL(SUM(CT.GIATIEN), 0)
    FROM CHITIETVE CT
    WHERE MONTH(CT.NGAYDAT) = @Thang 
      AND YEAR(CT.NGAYDAT) = @Nam;
      
    -- Ghi log vào bảng thống kê (tùy chọn)
    -- INSERT INTO THONGKE_DOANHTHU (THANG, NAM, TONGDOANHTHU, NGAYTHONGKE)
    -- VALUES (@Thang, @Nam, @TongDoanhThu, GETDATE());
END;
GO

-- ======================================================
-- 3.2.3. FUNCTION – Thống kê số lượng khách hàng theo quốc gia
-- ======================================================
-- Mục đích: Thống kê số lượng khách hàng theo từng quốc gia phục vụ cho mục đích phân tích thị trường
-- Mô tả: Function trả về bảng gồm hai cột: QUOCGIA và SOKHACH
--        Có thể sử dụng trực tiếp trong câu lệnh SELECT * FROM fn_ThongKeKhachTheoQuocGia()
CREATE FUNCTION fn_ThongKeKhachTheoQuocGia ()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        KH.QUOCGIA, 
        COUNT(KH.MAKH) AS SOKHACH
    FROM KHACHHANG KH
    GROUP BY KH.QUOCGIA
);
GO

-- ======================================================
-- 3.2.4. CURSOR – Thống kê doanh thu từng chuyến bay
-- ======================================================
-- Mục đích: Thống kê doanh thu của từng chuyến bay bằng cách duyệt qua toàn bộ các chuyến 
--           trong bảng CHUYENBAY và tính tổng tiền vé tương ứng
-- Mô tả: Đoạn mã sử dụng CURSOR để lần lượt lấy từng mã chuyến bay (MACB) trong bảng CHUYENBAY,
--        sau đó tính tổng doanh thu (tổng GIATIEN) của các vé thuộc chuyến bay đó từ bảng CHITIETVE.
--        Nếu chuyến bay chưa có vé, doanh thu được gán bằng 0.
--        Kết quả được in trực tiếp ra màn hình bằng lệnh PRINT.
-- 
-- Lưu ý: Đây là ví dụ minh họa sử dụng CURSOR. Để chạy, copy đoạn code dưới đây vào SQL query window
-- 
-- BEGIN
-- DECLARE @MaCB CHAR(10);
-- DECLARE @TongDoanhThu MONEY;
-- 
-- DECLARE cur_ThongKeDoanhThu CURSOR FOR
--     SELECT MACB
--     FROM CHUYENBAY;
-- 
-- OPEN cur_ThongKeDoanhThu;
-- FETCH NEXT FROM cur_ThongKeDoanhThu INTO @MaCB;
-- 
-- WHILE @@FETCH_STATUS = 0
-- BEGIN
--     SELECT @TongDoanhThu = ISNULL(SUM(CT.GIATIEN), 0)
--     FROM CHITIETVE CT
--     JOIN VEMAYBAY V ON CT.MAVE = V.MAVE
--     WHERE V.MACB = @MaCB;
--     
--     IF @TongDoanhThu IS NULL
--         SET @TongDoanhThu = 0;
--         
--     PRINT N'Chuyến bay ' + @MaCB + N' có tổng doanh thu: ' + CAST(@TongDoanhThu AS NVARCHAR(50));
--     
--     FETCH NEXT FROM cur_ThongKeDoanhThu INTO @MaCB;
-- END;
-- 
-- CLOSE cur_ThongKeDoanhThu;
-- DEALLOCATE cur_ThongKeDoanhThu;
-- END;
-- GO

-- ======================================================
-- HƯỚNG DẪN SỬ DỤNG CÁC CẤU TRÚC THỐNG KÊ
-- ======================================================

-- 1. Trigger trg_CapNhatDoanhThu sẽ tự động chạy khi INSERT vào CHITIETVE
--    Không cần gọi thủ công

-- 2. Sử dụng Stored Procedure tính doanh thu tháng:
--    DECLARE @DoanhThu MONEY;
--    EXEC sp_TinhTongDoanhThu_Thang @Thang = 9, @Nam = 2025, @TongDoanhThu = @DoanhThu OUTPUT;
--    PRINT N'Tổng doanh thu tháng 9/2025: ' + CAST(@DoanhThu AS NVARCHAR(50));

-- 3. Sử dụng Function thống kê khách hàng theo quốc gia:
--    SELECT * FROM fn_ThongKeKhachTheoQuocGia();

-- 4. Sử dụng Cursor - Copy đoạn code trong phần mô tả CURSOR ở trên và chạy trong SQL query window
