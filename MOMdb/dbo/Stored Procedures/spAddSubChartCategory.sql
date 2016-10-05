-- =============================================================================
-- Create date: 11/17/2015
-- Description: To insert chart of account sub category details from chart table.
-- =============================================================================
CREATE PROCEDURE [dbo].[spAddSubChartCategory]
AS BEGIN
CREATE TABLE #tempSubCat(
		Type int,
		Sub varchar(150),
		SortOrder int,
	)
CREATE TABLE #tempType(
		Type int
	)

	INSERT INTO #tempSubCat ([Type], [Sub])		--Get all sub categories from chart table
	SELECT DISTINCT  [Type], [Sub]
	FROM  Chart
	WHERE [Sub] <> ''
	ORDER BY [Type]

	DECLARE @CountRow int;					--Get type details from chart table
	DECLARE @CountSubCatRow int;			
	INSERT INTO #tempType([Type])
	SELECT [Type] FROM #tempSubCat
	GROUP BY [Type]


SELECT @CountRow = COUNT(*) FROM #tempType
--SELECT @CountSubCatRow = COUNT(ID) FROM [dbo].[SubCat]

DECLARE @Iterator INT
SET @Iterator = 1

--IF @CountSubCatRow <= 0
--BEGIN

WHILE  @Iterator <= @CountRow
	BEGIN
		DECLARE @Type INT;
		DECLARE @orderNum INT = 0;

		SELECT TOP 1 @Type=[Type] FROM #tempType
	
		INSERT INTO tempSubCat(CType, SubType, SortOrder)	
		SELECT [Type], Sub, ROW_NUMBER() OVER (ORDER BY @orderNum) FROM #tempSubCat WHERE [Type]=@Type

		DELETE FROM #tempType WHERE [Type]=@Type
		SET @Iterator = @Iterator + 1;
	END
END
   
--END
