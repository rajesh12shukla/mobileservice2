CREATE PROCEDURE [dbo].[spAddBills]
	@GLItem tblTypeGL readonly,
	@Vendor int,
	@Date datetime,
	@PostingDate datetime,
	@Due datetime,
	@Ref varchar(50),
	@Memo varchar(max),
	@DueIn smallint,
	@PO int = null,
	@ReceivePo int = null,
	@Status smallint,
	@Disc numeric(30,4),
	@Custom1 varchar(50),
	@Custom2 varchar(50)
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @PJID INT
	DECLARE @tAcctID int
	DECLARE @tfDesc varchar(max)
	DECLARE @tAmount numeric(30,2)
	DECLARE @tQuan numeric(30,2)
	DECLARE @tPrice numeric(30,2)
	DECLARE @tUtax numeric(30,2)
	DECLARE @JobId int
	DECLARE @PhaseId smallint
	DECLARE @ItemId int
	DECLARE @IsUseTax bit
	DECLARE @totalUtax numeric(30,2) =0
	DECLARE @TransId int = null
	DECLARE @MAXBatch int
	DECLARE @LineCount int = 0
	DECLARE @TransStatus varchar(10) = null
	DECLARE @Sel smallint = 0
	DECLARE @EN int = 0
	DECLARE @UtaxName varchar(25)
	DECLARE @PreAmountTotal numeric(30,2) =0
	DECLARE @ApAcct int
	DECLARE @TypeID int
	DECLARE @ItemDesc varchar(30)

	BEGIN TRANSACTION

	SELECT @MAXBatch = ISNULL(MAX(Batch),0)+1 FROM Trans
	
	-------------------------------begin --- add bom and non-inventory items------------------------------------------
	CREATE table #temp
	(
	ID int null,
	AcctID int null,
	fDesc varchar(max) null,
	Amount numeric(30,2) null,
	UseTax numeric(30,4) null,
	UtaxName varchar(25) null,
	JobID int null,
	PhaseID int null,
	ItemID int null,
	ItemDesc varchar(150) null,
	TypeID int null,
	TypeDesc varchar(150) null
	)
	
	DECLARE db_cursor1 CURSOR FOR 
	
	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, TypeID, ItemDesc FROM @GLItem 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO 
		@TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @TypeID, @ItemDesc
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		

		if(@JobId is not null and (@PhaseId = 0 or @PhaseId is null) and (@TypeID = 1 or @TypeID = 2))
		begin
			if(@ItemID is not null)
			begin
				-- add into inv table

				exec @PhaseId = spAddBOMItem @JobId, null, null, @TypeID, @ItemId, null, null, null, null, null, 'true'

			end
			else if (@ItemID is null or @ItemID = 0)
			begin
				-- add into inv table (as non inventory type) and add as bom item

				INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
				VALUES (@ItemDesc,@tfDesc,0,0,'Each',0,0,0,2,@tAcctID,0,0)
				
				SET @ItemId = SCOPE_IDENTITY()

				exec @PhaseId = spAddBOMItem @JobId, null, null, @TypeID, @ItemId, null, null, null, null, null, 'true'
	
			end
		end

		insert into #temp (ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, TypeID, ItemDesc)
		values (@TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @TypeID, @ItemDesc)

	FETCH NEXT FROM db_cursor1 INTO 
		 @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName, @TypeID, @ItemDesc
	END  

	-------------------------------end --- add bom and non-inventory items------------------------------------------


	DECLARE db_cursor CURSOR FOR 

	SELECT ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName FROM #temp 

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF(@JobId = 0)  BEGIN SET @JobId = null END
		IF(@PhaseId = 0)  BEGIN SET @PhaseId = null END

		
		SET @IsUseTax = 0
		DECLARE @tUtaxAmt numeric(30,2)
		SET @PreAmountTotal = @PreAmountTotal + @tAmount

		IF (@tUtax > 0)
		BEGIN
			SET @IsUseTax = 1
			SET @tUtaxAmt = (@tAmount * @tUtax) / 100
			SET @tfDesc = @tfDesc + ' (Amount Before Use Tax - $'+ CONVERT(varchar, cast(cast(isnull(@tUtaxAmt,0) as decimal) as money), 1) +')'
			
			SET @tAmount = @tAmount + @tUtaxAmt
			SET @totalUtax = @totalUtax + @tUtaxAmt
		END
		

		EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@Date,41,@LineCount,0,@tfDesc,@tAmount,@tAcctID,@ItemId,@TransStatus,@Sel,@JobId,@PhaseId,@EN,@Ref
	
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
		RETURN
		END

		IF @JobId IS NOT NULL AND @PhaseId IS NOT NULL
		BEGIN
			INSERT INTO [dbo].[JobI]
					   ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax])
				 VALUES
					   (@JobId,@PhaseId,@Date,@Ref,@tfDesc,@tAmount,@TransId,1,@IsUseTax)
		END

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
		RETURN
		END

		SET @LineCount = @LineCount + 1
		IF(@IsUseTax = 1)
		BEGIN
			SET @tfDesc = 'Use Tax Payable'
			SET @tAmount = @tUtaxAmt * -1

			EXEC [dbo].[AddJournal] null,@MAXBatch,@Date,41,@LineCount,0,@tfDesc,@tAmount,@tAcctID,null,@TransStatus,@Sel,@JobId,@PhaseId,@EN,@Ref
			
			 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
					ROLLBACK TRANSACTION    
				RETURN
				END

			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax])
			 VALUES
				   (@TransId
				   ,@UtaxName
				   ,@tUtaxAmt
				   ,@tUtax)
			SET @LineCount = @LineCount + 1
		
			 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
					ROLLBACK TRANSACTION    
				RETURN
				END
		END

	FETCH NEXT FROM db_cursor INTO @TransId, @tAcctID, @tfDesc, @tAmount, @tUtax, @JobId, @PhaseId, @ItemId, @UtaxName
	END

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	-- credit transaction ------------------------------------------------------------------

	SELECT TOP 1 @ApAcct=ID FROM Chart WHERE DefaultNo='D2000' AND Status=0 ORDER BY ID 

	SET @tAmount = @PreAmountTotal * -1
	
	EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@Date,40,@LineCount,0,@Memo,@tAmount,@ApAcct,@Vendor,@TransStatus,@Sel,null,null,@EN,@Ref

	 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
		RETURN
		END


	-- insert AP details in PJ and OpenAP --------------------------------------------------------------------
	
	SELECT @PJID = ISNULL(MAX(ID),0)+1 FROM PJ;

	INSERT INTO [dbo].[PJ]
           ([ID],[fDate],[Ref],[fDesc],[Amount],[Vendor],[Status],[Batch],[Terms],[PO],[TRID],[Spec],[IDate],[UseTax],[Disc],[Custom1],[Custom2],[ReqBy],[VoidR],[ReceivePO])
     VALUES
           (@PJID,@PostingDate,@Ref,@Memo,@PreAmountTotal,@Vendor,0,@MAXBatch,@DueIn,@PO,@TransId,@Status,@Date,@totalUtax,@Disc,@Custom1,@Custom2,0,null,@ReceivePo)
		   -- default : open status
	 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
		RETURN
		END
			
	INSERT INTO [dbo].[OpenAP]
			   ([Vendor],[fDate],[Due],[Type],[fDesc],[Original],[Balance],[Selected],[Disc],[PJID],[TRID],[Ref])
		 VALUES
			   (@Vendor,@Date,@Due,0 ,@Memo,@PreAmountTotal,@PreAmountTotal,0,@Disc,@PJID,@TransId,@Ref)
			   -- default : type = 0 for AP invoice
	 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
		RETURN
		END
				
	-- update vendor balance ----------------------------------------------------------------------------------
	UPDATE v
	SET Balance = ((SELECT isnull(Balance,0) as Balance FROM Vendor WHERE ID = @Vendor) - @PreAmountTotal)
	FROM Vendor v 
		WHERE v.ID = @Vendor

	 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
		RETURN
		END

	EXEC [dbo].[spCalChartBalance]

	DROP TABLE #temp

	COMMIT TRANSACTION
 return @PJID
END
