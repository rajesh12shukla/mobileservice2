CREATE PROCEDURE [dbo].[spBankRecon]
	@bank int,
	@endbalance numeric(30,2),
	@ReconDate datetime,
	@ServiceChrg numeric(30,2) = 0,
	@ServiceAcct int = null,
	@ServiceDate datetime = null,
	@InterestChrg numeric(30,2),
	@InterestAcct int = null,
	@InterestDate datetime = null,
	@BankRecon As [dbo].[tblTypeBankRecon] Readonly
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @bankchart int
	declare @batch int
	declare @ref int
	declare @balance numeric(30,2)
	declare @tamount numeric 
	declare @fdesc varchar(150)
	declare @fdate datetime
	declare @type varchar(10)
	declare @typeNum int
	declare @id int


BEGIN TRY
BEGIN TRANSACTION
	
	select @bankchart = chart from Bank where ID = @bank

	exec spUpdateBankBalance @bankchart, @endbalance

	UPDATE Bank SET Recon = @endbalance, LastReconDate=@ReconDate WHERE ID = @bank

	-- insert transaction
	SELECT @batch = ISNULL(MAX(Batch),0)+1 FROM Trans
	SELECT @ref = ISNULL(MAX(Ref),0)+1 FROM GLA

	if (@ServiceChrg > 0  and @InterestChrg > 0)
	begin
		set @balance = (@ServiceChrg - @InterestChrg) * -1
	end
	else if (@ServiceChrg > 0)
	begin
		set @balance = @ServiceChrg * -1
	end
	else if (@InterestChrg > 0)
	begin
		set @balance = @InterestChrg
	end

	if (@ServiceChrg > 0  or @InterestChrg > 0)
	begin
		
		insert into GLA (Ref,fDate,Internal,fDesc,Batch) 
		values (@ref, GETDATE(), DATEPART(mm,getdate())+DATEPART(dd,getdate())+DATEPART(yyyy,getdate()), 'Bank reconciliation '+convert(varchar(12),getdate(),101), @batch)

		set @fdate = GETDATE()
		
		set @fdesc = 'Bank reconciliation '+convert(varchar(12),getdate(),101)

		EXEC [dbo].[AddJournal] null,@Batch,@fdate,30,0,@ref,@fdesc,@balance,@bankchart,@bank,null,1 
	end

	if (@ServiceChrg > 0)
	begin
		EXEC [dbo].[AddJournal] null,@Batch,@ServiceDate,31,1,@ref,'Bank Service Charge',@ServiceChrg,@ServiceAcct,null,null,0 
	end

	if (@InterestChrg > 0)
	begin
		
		set @InterestChrg = @InterestChrg * -1

		EXEC [dbo].[AddJournal] null,@Batch,@InterestDate,31,2,@ref,'Interest',@InterestChrg,@InterestAcct,null,null,0 
	end
	
	UPDATE Trans SET Sel = 1 WHERE ID IN (SELECT ID FROM @BankRecon)

	UPDATE CD SET IsRecon = 'true' WHERE Ref IN (SELECT Ref FROM @BankRecon WHERE TypeNum = 20)

	UPDATE DEP SET IsRecon = 'true' WHERE Ref IN (SELECT Ref FROM @BankRecon WHERE TypeNum = 5)

	UPDATE tc
	SET
	IsRecon = 'true'
				FROM Trans t 
					inner join chart c on t.Acct=c.ID
					inner join (SELECT * FROM @BankRecon WHERE TypeNum = 30) b ON b.Batch = t.Batch
					inner join TransChecks tc ON tc.Batch = b.Batch
					WHERE (c.type = 6 or c.DefaultNo = 'D1000') and t.sel <> 1 and t.type =30

	UPDATE td
	SET
	IsRecon = 'true'
				FROM Trans t 
					inner join chart c on t.Acct=c.ID
					inner join (SELECT * FROM @BankRecon WHERE TypeNum = 30) b ON b.Batch = t.Batch
					inner join TransDeposits td ON td.Batch = b.Batch
					WHERE (c.type = 6 or c.DefaultNo = 'D1000') and t.sel <> 1 and t.type =30



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