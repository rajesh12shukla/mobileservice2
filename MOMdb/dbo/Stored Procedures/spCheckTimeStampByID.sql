
CREATE PROCEDURE spCheckTimeStampByID
(
 @TableName VARCHAR(150),
 @ID INT,
 @PreviousTimeStamp TIMESTAMP
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION

	DECLARE @CurrentTimeStamp TIMESTAMP;

	IF( @TableName = 'Trans')
		BEGIN
						
		SELECT TOP 1 @CurrentTimeStamp = [TimeStamp] FROM [dbo].[Trans] WHERE ID = @ID

		IF( @CurrentTimeStamp = @PreviousTimeStamp)
		BEGIN
			SELECT CAST('True' AS BIT) AS IsAccessible;
		END
		ELSE
				
			SELECT CAST('False' AS BIT) AS IsAccessible;
		END
			

     ROLLBACK TRANSACTION
END
