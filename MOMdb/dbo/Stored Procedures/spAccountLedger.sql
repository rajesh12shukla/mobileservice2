
CREATE PROCEDURE [dbo].[spAccountLedger] 
	@cid int,
	@sdate datetime,
	@edate datetime
AS
BEGIN
	
	SET NOCOUNT ON;
	
	declare @id int
	declare @acct int
	declare @fDate datetime
	declare @batch int
	declare @ref int
	declare @type int
	declare @cfDesc varchar(75)
	declare @fDesc varchar(max)
	declare @amount numeric(30,2)
	declare @balance numeric(30,2)
	declare @count int = 0
	declare @total numeric(30,2) = 0
	declare @TypeText varchar(150)
	declare @debit numeric(30,2)
	declare @credit numeric(30,2)

	  create table #tempChart(
	  ID int,
	  Acct int, 
	  fDate datetime,
	  Batch int,
	  Ref int,
	  TypeText varchar(150),
	  Type int,
	  ChartfDesc varchar(150),
	  fDesc varchar(max),
	  Amount numeric(30,2),
	  Balance numeric(30,2),
	  Debit numeric(30,2),
	  Credit numeric(30,2)
	  )


DECLARE db_cursor CURSOR FOR 

select c.ID, t.Acct, t.fDate, t.Batch, 
		isnull(t.Ref,0) as Ref, 
		dbo.TransTypeToText(t.type) as TypeText,
       (CASE t.Type WHEN 50 THEN '1' 
					WHEN 40 THEN '2' 
					WHEN 41 THEN '2' 
					WHEN 21 THEN '3' 
					WHEN 20 THEN '3' 
					WHEN 5 THEN '4' 
					WHEN 6 THEN '4' 
					WHEN 5 THEN '5'
					WHEN 6 THEN '5' 
					WHEN 1 THEN '6' 
					WHEN 2 THEN '6' 
					WHEN 3 THEN '6' 
					WHEN 40 THEN '8' 
					WHEN 41 THEN '8' 
					WHEN 98 THEN '9' 
					WHEN 99 THEN '9' 
					WHEN 30 THEN '7' 
					WHEN 31 THEN '7' 
					ELSE t.Type END) as Type,  
       isnull(c.fDesc,'') as ChartName, 
	   isnull(t.fDesc,'') as fDesc, 
	   isnull(t.Amount,0) as Amount, 
	   0 As Balance,
	   (CASE WHEN t.Amount > 0  
			THEN t.Amount 
			ELSE 0 END) As Debit, 
	   (CASE WHEN t.Amount < 0  
			THEN (t.Amount * -1) 
			ELSE 0 END) As Credit
	    FROM   Chart c	INNER JOIN Trans t 
					ON c.ID=t.Acct 
        WHERE c.ID=@cid ORDER BY t.fDate

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @id, @acct, @fDate, @batch, @ref,@TypeText, @type, @cfDesc, @fDesc, @amount, @balance, @debit,@credit

WHILE @@FETCH_STATUS = 0
BEGIN
	
	if(@count = 0)
	begin
		set @total = @amount
	end
	else
	begin
		set @total = @total + @amount
	end
	
	insert into #tempChart values(@id, @acct, @fDate, @batch, @ref,@TypeText, @type, @cfDesc, @fDesc, @amount, @total, @debit, @credit)
	set @count = @count + 1

FETCH NEXT FROM db_cursor INTO @id, @acct, @fDate, @batch, @ref,@TypeText, @type, @cfDesc, @fDesc, @amount, @balance, @debit, @credit
END

CLOSE db_cursor  
DEALLOCATE db_cursor

select * from #tempChart where (fDate >= @sdate) AND ( fDate <= @edate)

select * from chart where id = @cid

drop table #tempChart
END
