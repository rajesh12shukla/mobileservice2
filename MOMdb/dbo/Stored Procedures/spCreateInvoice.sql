CREATE PROCEDURE [dbo].[spCreateInvoice]
@Invoice As [dbo].[tblTypeInvoice] Readonly,
@fdate datetime,
@Fdesc varchar(max),
@Amount numeric(30,2),
@stax numeric(30,2),
@total numeric(30,2),
@taxRegion varchar(25),
@taxrate numeric(30,4),
@Taxfactor numeric(30,2),
@taxable numeric(30,2),
@type smallint,
@job int,
@loc int,
@terms smallint,
@PO varchar(25),
@Status smallint,
--@Batch int,
@Remarks varchar(max),
@gtax numeric(30,2),
@mech int, 
@TaxRegion2 varchar(25),
@Taxrate2 numeric(30,4),
@BillTo varchar(1000),
@Idate datetime,
@Fuser varchar(50),
@staxI int,
@invoiceID varchar(50),
@TicketIDs varchar(max),
@ddate datetime
--@TransID int

as
BEGIN
declare @Ref int
--declare @batchid int
declare @StaxAmount numeric(30,2)=0.00
declare @Batch int
declare @TransId int
declare @TransId1 int
declare @TransType int
declare @Line int=0
declare @AcctAR int
declare @return_value int
declare @AcctSub int
declare @TransStatus varchar(10)
declare @Sel smallint = 0
declare @SAcct int
declare @Acct int
declare @Quan numeric(30,2)
declare @Price numeric(30,4)
declare @Code int
declare @Measure varchar(15)
declare @Disc numeric(30,4)
declare @StaxAmt numeric(30,4)
declare @TransAmount numeric(30,2)
declare @totalamt numeric(30,2)
declare @LocStax varchar(25)
declare @IsStax bit = 0
declare @preAmount numeric(30,2)

--if(@staxI=1)
--begin
--set @StaxAmount = (@taxrate*@Amount)/100

--end

--select @batchid= isnull(MAX(Batch),0)+1 from Invoice					-- commented by dev on 10th feb, 16 @Batch Trans.Batch number which maps two tables.
BEGIN TRY
BEGIN TRANSACTION

	set @StaxAmount = @stax

	SELECT TOP 1 @AcctAR = ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID -- Get Account receivable account from chart table.

	SELECT @Batch = ISNULL(MAX(Batch),0)+1 FROM Trans									  -- Get maximum batch number from trans table.

	IF(@Status = 1 or @Status = 5)														  -- Status = Paid
	BEGIN
		SET @Sel = 1
	END
	ELSE IF(@Status = 2)																  -- Status = Void
	BEGIN
		SET @Sel = 2
	END
	ELSE																				  -- Status = Open
	BEGIN
		SET @Sel = 0
	END
	SET @TransType = 1;
	SET @totalamt = @Amount+@StaxAmount
	
	EXEC @TransId = [dbo].[AddJournal] null,@Batch,@fdate,@TransType,@Line,@Ref,@fDesc,@totalamt,@AcctAR,@loc,@TransStatus,@Sel
		
	  
    EXEC spUpdateCustomerLocBalance @Loc,@totalamt;									 -- Update Owner, Location balance


insert into Invoice
(
fDate,fDesc,Amount,STax,
Total,TaxRegion,TaxRate,
TaxFactor,Taxable,Type,
Job,Loc,Terms,PO,Status,
Batch,Remarks,TransID,
GTax,Mech,TaxRegion2,
TaxRate2,BillTo,IDate,fUser,Custom1, LastUpdateDate,DDate
)
select @fDate,@fDesc,@Amount,@StaxAmount,
@Amount+@StaxAmount,@TaxRegion,@TaxRate,
@TaxFactor,@Taxable,@Type,
@Job,@Loc,@Terms,@PO,@Status,
@Batch,@Remarks,@TransId,
@GTax,@Mech,@TaxRegion2,
@TaxRate2,@BillTo,@IDate,@fUser ,@invoiceID,GETDATE(),@ddate

set @Ref = SCOPE_IDENTITY()

--select 'Ref' = @Ref


UPDATE Trans SET Ref = @Ref WHERE Batch = @Batch

INSERT INTO [dbo].[OpenAR]
           ([Loc]
           ,[fDate]
           ,[Due]
           ,[Type]
           ,[Ref]
           ,[fDesc]
           ,[Original]
           ,[Balance]
           ,[Selected]
           ,[TransID])
     VALUES
           (@Loc
           ,@fDate
           ,@ddate
           ,0	
           ,@Ref
           ,@fDesc
           ,@Amount+@StaxAmount
           ,@Amount+@StaxAmount
           ,0
           ,@TransId)

--insert into InvoiceI
--(
--Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,jobitem,TransID,Measure,Disc,StaxAmt
--)
--select @Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,TransID,Measure,Disc,StaxAmt from @Invoice 

DECLARE db_cursor CURSOR FOR 

SELECT @Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,@job,Code,TransID,Measure,Disc,StaxAmt FROM @Invoice 

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @Ref, @Line, @Acct, @Quan, @fDesc, @Price, @Amount, @STax, @job, @Code, @TransId1, @Measure, @Disc, @StaxAmt

WHILE @@FETCH_STATUS = 0
BEGIN

	 SET @Line = @Line + 1;
	 SET @IsStax = 0;
	 
	
	IF EXISTS(SELECT distinct 1 FROM Inv WHERE ID=@Acct)
	BEGIN 
		SELECT @SAcct=SAcct FROM Inv WHERE ID=@Acct
		
		SET @TransType = 2
		SET @TransAmount = @Quan * @Price * -1
		SET @preAmount = @Quan * @Price
		
		EXEC @TransId1 = [dbo].[AddJournal] null,@Batch,@fdate,@TransType,@Line,@Ref,@fDesc,@TransAmount,@SAcct,@AcctSub,@TransStatus,@Sel 
		
	
		 SET @Line = @Line + 1;

		 IF(@StaxAmt > 0)
		 BEGIN
		 SET @SAcct =0
			SELECT @LocStax = STax FROM Loc WHERE Loc = @loc
		 	IF EXISTS(SELECT 1 FROM STax WHERE Name = @LocStax)
			BEGIN
				SELECT @SAcct=GL FROM STax WHERE Name = @LocStax
			END
			ELSE IF EXISTS(SELECT 1 FROM Chart WHERE DefaultNo='D2100' AND Status=0)
			BEGIN
				SELECT @SAcct=ID FROM Chart WHERE DefaultNo='D2100' AND Status=0 ORDER BY ID

			END
			IF(@SAcct > 0)
			begin
				SET @IsStax = 1
				SET @TransType = 3
				SET @TransAmount = @StaxAmt * -1

				EXEC [dbo].[AddJournal] null,@Batch,@fdate,@TransType,@Line,@Ref,@LocStax,@TransAmount,@SAcct,@AcctSub,@TransStatus,@Sel 
				
			
			    SET @Line = @Line + 1;
			end
				
			END

		 END
	
	if (@job = 0) SET @job = null 
	if (@Code = 0) SET @Code = null

	IF ((@job IS NOT NULL) and (@Code is null))
	BEGIN
		
		if exists(select top 1 1  from JobTItem as j INNER JOIN Milestone as m
						 ON m.JobTItemID = j.ID
						 WHERE j.Job = 1 and j.Type = 0 and m.Type = 1)
		begin
			select top 1 @Code = j.Line  from JobTItem as j INNER JOIN Milestone as m
						 ON m.JobTItemID = j.ID
						 WHERE j.Job = @job 
						 and j.Type = 0																-- jobtitem.type = revenue
						 and m.Type = (select top 1 ID FROM OrgDep where Department='Finance')		-- milestone.type = Finance 
						 order by j.Code
		end
	END

	IF @job IS NOT NULL AND @Code IS NOT NULL
	BEGIN
		INSERT INTO [dbo].[JobI] ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],[Invoice])
		VALUES (@job,@Code,@fdate,@Ref,@fDesc,@preAmount,@TransId1,0,@IsStax,@Ref)
	END
	
	INSERT INTO InvoiceI (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,jobitem,TransID,Measure,Disc,StaxAmt)
	VALUES (@Ref,@Line,@Acct,@Quan,@fDesc,@Price,@Amount,@STax,@job,@Code,@TransId1,@Measure,@Disc,@StaxAmt)

FETCH NEXT FROM db_cursor INTO @Ref, @Line, @Acct, @Quan, @fDesc, @Price, @Amount, @STax, @Job, @Code, @TransId1, @Measure, @Disc, @StaxAmt
END

CLOSE db_cursor  
DEALLOCATE db_cursor

 
 if(@TicketIDs<>'')
 begin
 update TicketD set Invoice = @Ref , Charge=0  where ID in ( select * from dbo.split(@TicketIDs,',') )
 end
 
 EXEC [dbo].[spCalChartBalance]							-- calculate chart balance
 
	COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

	END CATCH
	
 return @ref
 
END



