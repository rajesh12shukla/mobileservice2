
CREATE PROCEDURE [dbo].[spAddTransBankAdj](
            @Batch   INT
           ,@Bank   INT
           ,@IsRecon BIT = 'false'
		   ,@Amount NUMERIC(30,2)
)
AS
BEGIN
	SET NOCOUNT ON;
		
		IF @Amount < 0  
		BEGIN
						-- transaction is of check
			INSERT INTO [dbo].[TransChecks]
					   ([Batch]
					   ,[Bank]
					   ,[IsRecon])
				 VALUES
					   (@Batch
					   ,@Bank
					   ,@IsRecon)
		END
		ELSE	
						-- transaction is of deposit
			INSERT INTO [dbo].[TransDeposits]
				   ([Batch]
				   ,[Bank]
				   ,[IsRecon])
			 VALUES
				   (@Batch
				   ,@Bank
				   ,@IsRecon)

END
