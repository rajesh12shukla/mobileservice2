
CREATE PROCEDURE [dbo].[spDeleteAPBill]
	@id int
AS
BEGIN
	
	SET NOCOUNT ON;

 BEGIN TRANSACTION
	declare @vendor int
	declare @batch int
	declare @amount numeric(30,2)
	declare @tid int
	declare @type smallint
	declare @acct int
	declare @tamount numeric(30,2)
	declare @jobId int
	declare @TRID int
	select @vendor=Vendor, @batch=Batch, @amount=Amount, @TRID=TRID FROM PJ WHERE ID=@id


	-- delete vendor balance
	UPDATE VENDOR
	SET Balance = (t.Balance + @amount)
	  FROM Vendor v Right join
		(SELECT t.AcctSub, Sum(t.Amount) AS Balance
			 FROM Trans t, Chart c
		     WHERE c.DefaultNo='D2000' and t.Batch = @batch
		     GROUP BY AcctSub ) t
		     ON v.ID = t.AcctSub

 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	 BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	 END	
	

DECLARE db_cursor CURSOR FOR 
	SELECT ID, Acct, type, Amount, Vint FROM Trans WHERE batch=@batch
OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @tid, @acct, @type, @amount, @jobId

WHILE @@FETCH_STATUS = 0
BEGIN  	
	
	delete from PJItem where TRID=@tid

	 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	 BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	 END	

	if(@jobId>0)
	begin
		delete from JobI where TransID=@tid		-- delete JobI details from JobI 

		 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		 BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
		 END	

	end
	set @amount = @amount * -1
	
	delete from trans where ID=@tid				-- delete transaction from trans
	 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	 BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	 END	

FETCH NEXT FROM db_cursor INTO  @tid, @acct, @type, @amount, @jobId

END	

CLOSE db_cursor  
DEALLOCATE db_cursor

delete from OpenAP where PJID=@id				-- delete from OpenAP 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	 BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	 END	

delete from PJ where ID=@id						-- delete from PJ 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	 BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	 END	

exec spCalChartBalance							-- calculate chart balance

 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
	 BEGIN  
		RAISERROR ('Error Occured', 16, 1)  
		ROLLBACK TRANSACTION    
		RETURN
	 END	

 COMMIT TRANSACTION
END