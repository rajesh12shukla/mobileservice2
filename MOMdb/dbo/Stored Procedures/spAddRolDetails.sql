CREATE PROCEDURE [dbo].[spAddRolDetails]
	--@ID INT = NULL OUTPUT
	@Name VARCHAR(75)
	,@City VARCHAR(50) = ''
	,@State VARCHAR(2) = ''
	,@Zip VARCHAR(10) = ''
	,@Phone VARCHAR(28)
	,@Fax VARCHAR(28) =  NULL
	,@Contact VARCHAR(50) =  NULL
	,@Address VARCHAR(255) =  NULL
	,@Email VARCHAR(50) = ''
	,@Website VARCHAR(50) =  NULL
	,@Country VARCHAR(50) =  NULL
	,@Cellular VARCHAR(28)
	,@Remarks TEXT = ''
	,@Type SMALLINT =  NULL
	,@fLong INT =  NULL
	,@Latt INT =  NULL
	,@GeoLock SMALLINT = 0
	,@Since DATETIME =  NULL
	,@Last DATETIME =  NULL
	,@EN INT =  1
	,@Category VARCHAR(15) =  NULL
	,@Position VARCHAR(255) =  NULL
	,@Lat VARCHAR(50) =  0
	,@Lng VARCHAR(50) =  0
	,@LastUpdateDate DATETIME =  NULL
	,@coords SMALLINT =  NULL
	


AS
BEGIN

	SET NOCOUNT ON;
	declare @id int
		   INSERT INTO [dbo].[Rol]
           ([Name]
		   ,[City]
		   ,[State]
		   ,[Zip]
		   ,[Phone]
		   ,[Fax]
		   ,[Contact]
		   ,[Remarks]
		   ,[Type]
		   ,[fLong]
		   ,[Latt]
		   ,[GeoLock]
		   ,[Since]
		   ,[Last]
		   ,[Address]
		   ,[EN]
		   ,[EMail]
		   ,[Website]
		   ,[Cellular]
		   ,[Category]
		   ,[Position]
		   ,[Country]
		   ,[Lat]
		   ,[Lng]
		   ,[LastUpdateDate]
		   ,[coords])
     VALUES
           (@Name
		   ,@City
		   ,@State
		   ,@Zip
		   ,@Phone
		   ,@Fax
		   ,@Contact
		   ,@Remarks
		   ,@Type
		   ,@fLong
		   ,@Latt
		   ,@GeoLock
		   ,@Since
		   ,@Last
		   ,@Address
		   ,@EN
		   ,@Email
		   ,@Website
		   ,@Cellular
		   ,@Position
		   ,@Category
		   ,@Country
		   ,@Lat
		   ,@Lng
		   ,@LastUpdateDate
		   ,@coords) 
	SET @id = SCOPE_IDENTITY()
			--OUTPUT inserted.ID INTO @IDs(ID)
	--SELECT SCOPE_IDENTITY() AS RolID
	--RETURN @ID

	
	return @id
END
