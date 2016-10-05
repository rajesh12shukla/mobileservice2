CREATE PROCEDURE [dbo].[spAddPO]
	@PO int,
	@fDate datetime,
	@fDesc varchar(max),
	@Amount numeric(30,2), 
	@VendorId int,
	@Status smallint,
	@Due datetime,
	@ShipVia varchar(50),
	@Terms smallint,
	@FOB varchar(50),
	@ShipTo varchar(8000),
	@Approved smallint,
	@Custom1 varchar(50),
	@Custom2 varchar(50),
	@ApprovedBy varchar(25),
	@ReqBy int,
	@fBy varchar(50),
	@POReasonCode varchar(50),
	@CourrierAcct varchar(50),
	@PORevision varchar(3),
	@POItem AS tblTypePOItem readonly
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Line smallint
	DECLARE @Quan numeric(30,2)
	DECLARE @PofDesc varchar(8000)
	DECLARE @PoPrice numeric(30,4)
	DECLARE @PoAmount numeric(30,2)
	DECLARE @Job int
	DECLARE @Phase smallint
	DECLARE @Inv int
	DECLARE @GL int
	DECLARE @TypeID int
	DECLARE @Freight numeric(30,2) = 0.0
	DECLARE @Rquan numeric(30,8)
	DECLARE @Billed int
	DECLARE @Ticket int
	DECLARE @Balance numeric(30,2)
	DECLARE @Selected numeric(30,2) = 0.0
	DECLARE @PoDue datetime
	DECLARE @SelectedQuan numeric(30,2) = 0.0
	DECLARE @BalanceQuan numeric(30,2) = 0.0
	DECLARE @ItemDesc varchar(30)

BEGIN TRY
BEGIN TRANSACTION
		
	--IF(@PO = 0)
	--BEGIN
	--	SELECT @PO = isnull(max(PO),0) +1 FROM PO	
	--END

	SET IDENTITY_INSERT [PO] ON 

	INSERT INTO [dbo].[PO]
           ([PO],[fDate],[fDesc],[Amount],[Vendor],[Status],[Due],[ShipVia],[Terms],[FOB],[ShipTo],[Approved],[Custom1],[Custom2],[ApprovedBy],[ReqBy],[fBy],[PORevision],[POReasonCode],[CourrierAcct])
     VALUES
           (@PO,@fDate,@fDesc,@Amount,@VendorId,@Status,@Due,@ShipVia,@Terms,@FOB,@ShipTo,@Approved,@Custom1,@Custom2,@ApprovedBy,@ReqBy,@fBy,@PORevision,@POReasonCode,@CourrierAcct)

	SET IDENTITY_INSERT [PO] OFF
	
	CREATE table #temp
	(
	ID int null,
	Line smallint null,
	AcctID int null,
	Account varchar(8000) null,
	Quan numeric(30,2) null,
	Price numeric(30,2) null,
	Amount numeric(30,2) null,
	JobID numeric(30,2) null,
	PhaseID int null,
	Inv int null, 
	Freight numeric(30,2) null,
	Rquan numeric(30,8) null,
	Billed int null,
	Ticket int null,
	Due datetime null,
	TypeID int null,
	ItemDesc varchar(30) null
	)
	
	DECLARE db_cursor CURSOR FOR 
	
	SELECT Line, Quan, Account, Price, Amount, JobID, PhaseID, Inv, AcctID, Freight, Rquan, Billed, Ticket, Due, TypeID, ItemDesc FROM @POItem

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO 
		@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Freight, @Rquan, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		

		if(@job is not null and (@Phase = 0 or @Phase is null) and (@TypeID = 1 or @TypeID = 2))
		begin
			if(@Inv is not null)
			begin
				-- add into inv table

				exec @Phase = spAddBOMItem @job, null, null, @TypeID, @Inv, null, null, null, null, null, 'true'

			end
			else if (@Inv is null or @Inv = 0)
			begin
				-- add into inv table (as non inventory type) and add as bom item

				INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
				VALUES (@ItemDesc,@PofDesc,0,0,'Each',0,0,0,2,@GL,0,0)
				
				SET @Inv = SCOPE_IDENTITY()

				exec @Phase = spAddBOMItem @job, null, null, @TypeID, @Inv, null, null, null, null, null, 'true'
	
			end
		end

		insert into #temp (Line, Quan, Account, Price, Amount, JobID, PhaseID, Inv, AcctID, Freight, Rquan, Billed, Ticket, Due, TypeID, ItemDesc )
		values (@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Freight, @Rquan, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc)

	FETCH NEXT FROM db_cursor INTO 
		 @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Freight, @Rquan, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc
	END  



	DECLARE db_cursor1 CURSOR FOR 
	SELECT Line, Quan, Account, Price, Amount, JobID, PhaseID, Inv, AcctID, Freight, Rquan, Billed, Ticket, Due FROM #temp 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO 
		@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Freight, @Rquan, @Billed, @Ticket, @PoDue
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
		IF(@Job = 0) SET @Job =null
		IF(@Phase = 0) SET @Phase =null

		SET @Balance  = @PoAmount
		SET @BalanceQuan = @Quan

		INSERT INTO [dbo].[POItem]
           ([PO],[Line],[Quan],[fDesc],[Price],[Amount],[Job],[Phase],[Inv],[GL],[Freight],[Rquan],[Billed],[Ticket],[Selected],[Balance],[Due],[SelectedQuan],[BalanceQuan])
		VALUES
           (@PO,@Line,@Quan,@PofDesc,@PoPrice,@PoAmount,@Job,@Phase,@Inv,@GL,@Freight,@Rquan,@Billed,@Ticket,@Selected,@Balance,@PoDue,@SelectedQuan,@BalanceQuan)
		
	FETCH NEXT FROM db_cursor1 INTO 
		 @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Freight, @Rquan, @Billed, @Ticket, @PoDue
	END  

	CLOSE db_cursor1  
	DEALLOCATE db_cursor1

	DECLARE @ChartId int

	SELECT TOP 1 @ChartId=ID FROM Chart WHERE DefaultNo='D9991' AND Status=0 ORDER BY ID 
	
	DROP TABLE #temp

	UPDATE Chart 
    SET Balance = ISNULL (p.Balance , 0)
	  FROM Chart c LEFT JOIN
		(SELECT Sum(Amount) AS Balance
			FROM PO) p
			ON c.DefaultNo = 'D9991' AND Status = 0

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


