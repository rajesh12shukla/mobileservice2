CREATE PROCEDURE [dbo].[spAddBankDetails](
     @ID      INT = NULL OUTPUT
    ,@fDesc   VARCHAR(75)
    ,@Rol INT 
	,@NBranch VARCHAR(20) =  NULL
	,@NAcct VARCHAR(20) =  NULL
	,@NRoute VARCHAR(20) =  NULL
	,@NextC INT =  NULL
	,@NextD INT =  NULL
	,@NextE INT =  NULL
	,@Rate NUMERIC(30,2) =  NULL
	,@CLimit NUMERIC(30,2) =  NULL
	,@Warn SMALLINT =  NULL
	,@Recon NUMERIC(30,2) =  0.00
	,@Balance NUMERIC(30,2) =  0.00
	,@Status SMALLINT =  NULL
	,@InUse SMALLINT =  0
	,@Chart INT
)

AS
BEGIN

	SET NOCOUNT ON;

	IF @ID IS NULL SELECT @ID=ISNULL(MAX(ID),0)+1 FROM [dbo].[Bank]

	--SELECT @ID=SCOPE_IDENTITY()
	INSERT INTO [dbo].[Bank]
           ([ID]
           ,[fDesc]
           ,[Rol]
           ,[NBranch]
           ,[NAcct]
           ,[NRoute]
           ,[NextC]
           ,[NextD]
           ,[NextE]
           ,[Rate]
           ,[CLimit]
           ,[Warn]
           ,[Recon]
           ,[Balance]
           ,[Status]
           ,[InUse]
		   ,[Chart]
           --,[ACHFileHeaderStringA]
           --,[ACHFileHeaderStringB]
           --,[ACHFileHeaderStringC]
           --,[ACHCompanyHeaderString1]
           --,[ACHCompanyHeaderString2]
           --,[ACHBatchControlString1]
           --,[ACHBatchControlString2]
           --,[ACHBatchControlString3]
           --,[ACHFileControlString1]
           --,[APACHCompanyID]
           --,[APImmediateOrigin]
		   )
     VALUES
           (@ID
           ,@fDesc
           ,@Rol
           ,@NBranch
           ,@NAcct
           ,@NRoute
           ,@NextC
           ,@NextD
           ,@NextE
           ,@Rate
           ,@CLimit
           ,@Warn
           ,@Recon
           ,@Balance
           ,@Status
           ,@InUse
		   ,@Chart
           --,ACHFileHeaderStringA
           --,ACHFileHeaderStringB
           --,ACHFileHeaderStringC
           --,ACHCompanyHeaderString1
           --,ACHCompanyHeaderString2
           --,ACHBatchControlString1
           --,ACHBatchControlString2
           --,ACHBatchControlString3
           --,ACHFileControlString1
           --,APACHCompanyID
           --,APImmediateOrigin
		   )

END
