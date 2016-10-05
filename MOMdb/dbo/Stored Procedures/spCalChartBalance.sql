
CREATE PROCEDURE [dbo].[spCalChartBalance]

AS
BEGIN
	
	SET NOCOUNT ON;


BEGIN TRY
BEGIN TRANSACTION
	
	 UPDATE Chart
		SET Balance = ISNULL (t.Balance , 0)
		  FROM Chart c LEFT JOIN
			(SELECT Acct, Sum(Amount) AS Balance
				FROM Trans
				GROUP BY Acct) t
				ON c.ID = t.Acct WHERE c.Type <> 7


COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH

END
