CREATE PROCEDURE [dbo].[spDeleteCheckDetails]
(
	@CDID INT
)
AS
BEGIN
	SET NOCOUNT ON;

   BEGIN TRANSACTION

   IF EXISTS(SELECT 1 
              FROM   [dbo].[CD] 
              WHERE  ID = @CDID) 
   BEGIN 
		DECLARE @Batch INT;
		DECLARE @Vendor INT
		DECLARE @Bank INT
		DECLARE @Amount numeric(30,2)

		SELECT @Batch=ISNULL(t.Batch,0), @Vendor= c.Vendor, @Bank = c.Bank, @Amount = c.Amount FROM [dbo].[CD] AS c, [dbo].[Trans] AS t WHERE c.ID = @CDID AND c.TransID = t.ID

		UPDATE o 
		SET 
			Selected = (o.Selected - p.Paid),
			Balance = (o.Balance + p.Paid)
				from OpenAP o 
				INNER JOIN PJ pj ON o.PJID = pj.ID
				INNER JOIN Paid p ON p.ref = pj.ref and pj.Vendor = @Vendor
				WHERE p.PITR =@CDID

		UPDATE pj
		SET
			Status = 0
				FROM PJ pj INNER JOIN 
				Paid p ON p.ref = pj.ref and pj.Vendor = @Vendor
				WHERE p.PITR =@CDID
		UPDATE t
		SET
			t.Sel = 0
		FROM Trans t 
			INNER JOIN OpenAP o ON o.TRID = t.ID
			INNER JOIN PJ pj ON o.PJID = pj.ID
			INNER JOIN Paid p ON p.ref = pj.ref 
			WHERE p.PITR =@CDID and pj.Vendor = @Vendor


		DELETE FROM CD WHERE ID = @CDID;
		
		DELETE FROM Paid WHERE PITR = @CDID
		DELETE FROM Trans WHERE Batch = @Batch;

		UPDATE v
			SET Balance = ((SELECT isnull(Balance,0) as Balance FROM Vendor WHERE ID = @Vendor) - @Amount)
			FROM Vendor v 
				WHERE v.ID = @Vendor
		
		UPDATE b
			SET Balance = ((SELECT isnull(Balance,0) as Balance FROM Bank WHERE ID = @Bank) - @Amount)
			FROM Bank b
				WHERE b.ID = @Bank
		EXEC [dbo].[spCalChartBalance]

   END 
   COMMIT TRANSACTION
END
