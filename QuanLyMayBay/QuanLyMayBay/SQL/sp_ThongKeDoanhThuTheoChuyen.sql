-- ======================================================
-- WRAPPER STORED PROCEDURE FOR CURSOR EXAMPLE
-- ======================================================
-- This procedure wraps the cursor example from QLMayBay.sql (lines 950-978)
-- to return results as a table instead of PRINT statements
-- 
-- Purpose: Calculate revenue for each flight using cursor iteration
-- Usage: EXEC sp_ThongKeDoanhThuTheoChuyen
-- Returns: Table with columns (MaChuyenBay, TongDoanhThu)

USE QUANLYMAYBAY;
GO

CREATE PROCEDURE sp_ThongKeDoanhThuTheoChuyen
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @MaCB CHAR(10);
    DECLARE @TongDoanhThu MONEY;
    
    -- Create temporary table to store results
    CREATE TABLE #TempResult (
        MaChuyenBay CHAR(10),
        TongDoanhThu MONEY
    );
    
    -- Declare cursor (same as lines 954-956 in QLMayBay.sql)
    DECLARE cur_ThongKeDoanhThu CURSOR FOR
        SELECT MACB
        FROM CHUYENBAY;
    
    -- Open cursor and fetch first record
    OPEN cur_ThongKeDoanhThu;
    FETCH NEXT FROM cur_ThongKeDoanhThu INTO @MaCB;
    
    -- Iterate through all flights (same logic as lines 961-974)
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calculate total revenue for this flight (lines 963-966)
        SELECT @TongDoanhThu = ISNULL(SUM(CT.GIATIEN), 0)
        FROM CHITIETVE CT
        JOIN VEMAYBAY V ON CT.MAVE = V.MAVE
        WHERE V.MACB = @MaCB;
        
        -- Handle NULL case (lines 968-969)
        IF @TongDoanhThu IS NULL
            SET @TongDoanhThu = 0;
        
        -- Instead of PRINT, insert into temp table
        INSERT INTO #TempResult (MaChuyenBay, TongDoanhThu)
        VALUES (@MaCB, @TongDoanhThu);
        
        -- Fetch next record (line 973)
        FETCH NEXT FROM cur_ThongKeDoanhThu INTO @MaCB;
    END;
    
    -- Close and deallocate cursor (lines 976-977)
    CLOSE cur_ThongKeDoanhThu;
    DEALLOCATE cur_ThongKeDoanhThu;
    
    -- Return results as table
    SELECT * FROM #TempResult ORDER BY MaChuyenBay;
    
    -- Clean up
    DROP TABLE #TempResult;
END;
GO
