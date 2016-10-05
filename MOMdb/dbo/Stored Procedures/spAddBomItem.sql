CREATE PROCEDURE [dbo].[spAddBOMItem]
	@job int,
	@fdesc varchar(255) = null,
	@code varchar(10) = null,
	@type smallint,
	@item int,
	@qty numeric(30,2) = 0.00,
	@um varchar(50) = null,
	@scrapfactor numeric(30,2) = null,
	@budgetunit numeric(30,2) = 0.00,
	@budgetext numeric(30,2) = 0.00, 
	@IsDefault bit = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @jobt int = 0
	DECLARE @line smallint = 1
	DECLARE @jobTItemId int
	SET @scrapfactor = null;
	
	SELECT @jobt=Template FROM Job WHERE ID = @job
	SELECT top 1 @line = isnull(Line,0) + 1 from JobTItem WHERE Job = @job and type = 1 order by Line desc

	IF (@IsDefault = 0)
	BEGIN
		
		INSERT INTO JobTItem (JobT, Job, Type, fDesc, Code, Actual, Budget, Line, [Percent], Comm, Modifier, ETC, ETCMod, Labor, Stored)
		VALUES (@jobt, @job, 1, @fdesc, @code, 0, 0, @line,0, 0, 0, 0, 0, 0, 0)
		SET @jobTItemId = SCOPE_IDENTITY()

		INSERT INTO BOM (JobTItemID, Type, Item, QtyRequired, UM, ScrapFactor, BudgetUnit, BudgetExt) 
		VALUES (@jobTItemId, @type, @item, @qty, @um, @scrapfactor, @budgetunit, @budgetext)
	END
	ELSE IF (@IsDefault = 1)  -- if is default true
	BEGIN

		IF (@type = 1)  -- bomt.Type = Materials
		BEGIN
			SELECT @fdesc = fDesc FROM Inv WHERE ID = @item 
		END
		ELSE IF (@type = 2) -- bomt.Type = Labor
		BEGIN
			SELECT @fdesc = fDesc FROM PRWage WHERE ID = @item 
		END
		ELSE
		BEGIN
			SET @fdesc = ''
		END
		
		INSERT INTO JobTItem (JobT, Job, Type, fDesc, Code, Actual, Budget, Line, [Percent], Comm, Modifier, ETC, ETCMod, Labor, Stored)
		VALUES (@jobt, @job, 1, @fdesc, (select top 1 code FROM JobCode WHERE IsDefault = 1), 0, 0, @line,0, 0, 0, 0, 0, 0, 0)
		SET @jobTItemId = SCOPE_IDENTITY()

		INSERT INTO BOM (JobTItemID, Type, Item, QtyRequired, UM, ScrapFactor, BudgetUnit, BudgetExt) 
		VALUES (@jobTItemId, @type, @item, @qty, 'Each', @scrapfactor, @budgetunit, @budgetext)

	END
	return @line;
END
