-- ======================================================
-- SCRIPT BACKFILL DỮ LIỆU VÀO BẢNG THONGKE_DOANHTHU
-- ======================================================
-- Mục đích: Đưa dữ liệu lịch sử từ CHITIETVE vào THONGKE_DOANHTHU
--           để đảm bảo bảng thống kê có đầy đủ dữ liệu cũ
-- Chạy script này MỘT LẦN sau khi deploy trigger mới
-- ======================================================

USE QUANLYMAYBAY;
GO

-- Bước 1: Kiểm tra xem đã có dữ liệu trong THONGKE_DOANHTHU chưa
SELECT COUNT(*) AS SoBanGhiHienTai FROM THONGKE_DOANHTHU;
GO

-- Bước 2: Backfill dữ liệu cũ
-- Chỉ thêm những bản ghi chưa tồn tại trong THONGKE_DOANHTHU
INSERT INTO THONGKE_DOANHTHU (MACB, NGAY, TONGDOANHTHU, SOLUONGVE)
SELECT 
    V.MACB,
    CAST(CT.NGAYDAT AS DATE) AS NGAY,
    SUM(CT.GIATIEN) AS TONGDOANHTHU,
    COUNT(CT.MAVE) AS SOLUONGVE
FROM CHITIETVE CT
JOIN VEMAYBAY V ON CT.MAVE = V.MAVE
WHERE NOT EXISTS (
    -- Kiểm tra xem đã có bản ghi này trong THONGKE_DOANHTHU chưa
    SELECT 1 FROM THONGKE_DOANHTHU TK 
    WHERE TK.MACB = V.MACB 
      AND TK.NGAY = CAST(CT.NGAYDAT AS DATE)
)
GROUP BY V.MACB, CAST(CT.NGAYDAT AS DATE);
GO

-- Bước 3: Kiểm tra kết quả
SELECT COUNT(*) AS SoBanGhiSauBackfill FROM THONGKE_DOANHTHU;
GO

-- Bước 4: Xem một số bản ghi mẫu
SELECT TOP 10 * FROM THONGKE_DOANHTHU ORDER BY NGAY DESC;
GO

-- ======================================================
-- KIỂM TRA TRIGGER HOẠT ĐỘNG
-- ======================================================
-- Test: Thêm một vé mới và kiểm tra xem trigger có tự động cập nhật không

-- Lưu ý: Chỉ chạy phần test này nếu muốn kiểm tra
-- Uncomment các dòng dưới đây để test:

/*
-- Tạo vé test
DECLARE @TestMaVe CHAR(10) = 'VETEST01';
DECLARE @TestMaPhieu CHAR(10) = 'PD01';
DECLARE @TestNgay DATE = CAST(GETDATE() AS DATE);
DECLARE @TestGia MONEY = 1500000;

-- Kiểm tra số lượng bản ghi trước khi insert
DECLARE @CountBefore INT;
SELECT @CountBefore = COUNT(*) 
FROM THONGKE_DOANHTHU 
WHERE MACB = 'CB01' AND NGAY = @TestNgay;

PRINT N'Số bản ghi trước khi insert: ' + CAST(@CountBefore AS NVARCHAR(10));

-- Insert vé test (giả sử VE01 thuộc chuyến bay CB01)
INSERT INTO CHITIETVE (MAVE, MAPHIEU, NGAYDAT, GIATIEN)
VALUES (@TestMaVe, @TestMaPhieu, @TestNgay, @TestGia);

-- Kiểm tra số lượng bản ghi sau khi insert
DECLARE @CountAfter INT;
SELECT @CountAfter = COUNT(*) 
FROM THONGKE_DOANHTHU 
WHERE MACB = 'CB01' AND NGAY = @TestNgay;

PRINT N'Số bản ghi sau khi insert: ' + CAST(@CountAfter AS NVARCHAR(10));

-- Xem dữ liệu thống kê
SELECT * FROM THONGKE_DOANHTHU WHERE MACB = 'CB01' AND NGAY = @TestNgay;

-- Xóa dữ liệu test
DELETE FROM CHITIETVE WHERE MAVE = @TestMaVe;
PRINT N'Đã xóa dữ liệu test';
*/

PRINT N'=== HOÀN THÀNH BACKFILL DỮ LIỆU ===';
