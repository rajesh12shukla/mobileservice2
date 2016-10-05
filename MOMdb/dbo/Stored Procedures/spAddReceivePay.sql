CREATE PROCEDURE [dbo].[spAddReceivePay]
	@receivePay As [dbo].[tblReceivePay] Readonly,
	@loc int,
	@owner int,
	@amount numeric(30,2),
	@dueAmount numeric(30,2),
	@payDate datetime,
	@payMethod smallint,
	@checknum varchar(21),
	@fDesc varchar(250)
AS
BEGIN
	
	SET NOCOUNT ON;
	 declare @id int
	 declare @ptotalAmount numeric(30,2)
	 declare @tid int
     declare @transId int
	 declare @batch int
	 declare @transType int
	 declare @line smallint = 0
	 declare @ref int
	 declare @tamount numeric(30,2)
	 declare @acctRv int = 0 
	 declare @undep int = 0
	 declare @sel smallint = 0
	 declare @transStatus varchar(10)
	 declare @receivePayId int
	 declare @payAmount numeric(30,2)
	 declare @return_value int
	 declare @invoiceId int
	 declare @tfDesc varchar(250)
	 declare @invStatus smallint
	 declare @totalAmount numeric(30,2)
	 declare @acctSub int
	 declare @uAmount numeric(30,2)
	 declare @iLoc int
	 declare @RevTransID int
BEGIN TRY
BEGIN TRANSACTION

	SELECT @id=isnull(max(ID),0) +1 FROM ReceivedPayment 

	SET IDENTITY_INSERT [ReceivedPayment] ON
	
	INSERT INTO [dbo].[ReceivedPayment]
           ([ID], [Loc], [Amount], [PaymentReceivedDate], [PaymentMethod],[CheckNumber],[AmountDue],[fDesc],[Status],[Owner])
     VALUES
           (@id, @loc, @amount, @payDate, @payMethod, @checknum, @dueAmount, @fDesc, 0, @owner)

	SET IDENTITY_INSERT [ReceivedPayment] OFF
	

 
	 SELECT TOP 1 @undep=ID FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID
	 SELECT TOP 1 @acctRv=ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID 


	DECLARE db_cursor CURSOR FOR 

	SELECT InvoiceID, Status, PayAmount FROM @receivePay 

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @invoiceId, @invStatus, @payAmount

	WHILE @@FETCH_STATUS = 0
	BEGIN
		 SELECT @batch = ISNULL(MAX(Batch),0)+1 FROM Trans									  -- Get maximum batch number from trans table.
		 SELECT @transId=ISNULL(MAX(ID),0)+1 FROM Trans										  -- Get maximum TransID from trans table.
		 set @line= 0
		 set @tamount = @payAmount
		 set @transType = 98
		 set @tfDesc = 'Deposit'
		 set @ref = @invoiceId
		 set @tid = @transId
		 set @acctSub = NULL
		 exec AddJournal @transId,@batch,@payDate,@transType,@line,@ref,@tfDesc,@tamount,@undep,@acctSub,@transStatus,@sel 
		 set @totalAmount = @totalAmount + @payAmount

		
		 SELECT @loc= loc, @RevTransID=TransID from Invoice where Ref=@invoiceId
	 
		 SELECT @transId=ISNULL(MAX(ID),0)+1 FROM Trans										  -- Get maximum TransID from trans table.
		 set @transType = 99
		 set @line = @line + 1
		 set @tamount = @tamount * -1
		 set @acctSub = @loc
		 exec AddJournal @transId,@batch,@payDate,@transType,@line,@ref,@fDesc,@tamount,@acctRv,@acctSub,@transStatus,@sel 

		 INSERT INTO [dbo].[PaymentDetails]([ReceivedPaymentID],[TransID],[InvoiceID]) VALUES (@id,@tid,@invoiceId)

	
		 UPDATE Invoice SET [Status] = @invStatus WHERE Ref = @invoiceId

		 UPDATE o SET 
			Selected = (o.Selected+@payAmount), 
			Balance = (o.Balance - @payAmount), 
			InvoiceID = @id 
		 FROM OpenAR o
		 WHERE Ref = @invoiceId AND Type = 0

		 IF (@invStatus = 1)
		 begin
			UPDATE Trans SET Sel =1 WHERE ID=@RevTransID
		 end

		 set @uAmount = @payAmount * -1
		 exec spUpdateCustomerLocBalance @loc, @uAmount

	FETCH NEXT FROM db_cursor INTO @invoiceId, @invStatus, @payAmount
	END

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	exec spCalChartBalance					-- calculate chart balance

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