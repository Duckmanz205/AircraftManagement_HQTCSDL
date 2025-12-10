-- ===================================================================
-- SQL FUNCTION: Thống kê số lượng khách hàng theo quốc gia
-- ===================================================================
-- Mục đích: Đếm số lượng khách hàng theo từng quốc gia
-- Sử dụng: SELECT * FROM fn_ThongKeKhachTheoQuocGia()
-- ===================================================================

CREATE FUNCTION fn_ThongKeKhachTheoQuocGia()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        QUOCGIA,
        COUNT(*) AS SOKHACH
    FROM 
        KHACHHANG
    WHERE 
        QUOCGIA IS NOT NULL
    GROUP BY 
        QUOCGIA
);
GO
