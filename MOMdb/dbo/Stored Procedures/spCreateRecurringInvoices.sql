CREATE PROCEDURE [dbo].[spCreateRecurringInvoices]
@RecurringInvoice As [dbo].[tblTypeRecurringInvoices] Readonly,
@InvoiceDate datetime,
@PayTerms int,
@Notes varchar(max),
@ParaStax int,
@ProcessPeriod varchar(75),
@cfUser varchar(50)
as

declare @fdate datetime
declare @Amount numeric(30,2)
declare @stax numeric(30,2)
declare @total numeric(30,2)
declare @taxRegion varchar(25)
declare @taxrate numeric(30,4)
declare @Taxfactor numeric(30,2)
declare @taxable numeric(30,2)
declare @type smallint
declare @job int
declare @loc int
declare @terms smallint
declare @PO varchar(25)
declare @Status smallint
declare @Batch int
declare @Remarks varchar(max)
declare @gtax numeric(30,2)
declare @mech int
declare @TaxRegion2 varchar(25)
declare @Taxrate2 numeric(30,4)
declare @BillTo varchar(1000)
declare @Idate datetime
declare @Fuser varchar(50)
declare @Acct int
declare @Quan numeric(30,2)
declare @Price numeric(30,4)
declare @Jobitem int
declare @measure varchar(15)
declare @ServiceType varchar(15)
declare @Frequency varchar(50)
declare @locid varchar(50)
declare @locname varchar(100)
declare @chart int
declare @return_value int
declare @ContractBill smallint
declare @IsCombinInv bit = 0
declare @cid int
declare @customername varchar(75)
declare @tempDesc varchar(max)
declare @jobDesc varchar(max)
declare @staxI int =1
declare @invoiceID varchar(50) =''
declare @TicketIDs varchar(max)
declare @custBilling smallint
declare @lid int
declare @fdesci varchar(max)
declare @dworker varchar(55)
declare @bcycle int
declare @name varchar(max)
declare @TempNote varchar(max) 
declare @Ref int

BEGIN TRY
BEGIN TRANSACTION
	
Create Table #temp(
 fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax numeric(30,2),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,2),
    taxfactor numeric(30,2),
    taxable int,
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(max),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,2),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(100),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint
)

DECLARE @tblInvoice dbo.tblTypeInvoice


DECLARE db_cursor CURSOR FOR 
select * from @Recurringinvoice
OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
       @Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@fdesci,@frequency,@name,@customername,@locid,@locname,@dworker,@bcycle,
	   @servicetype,@lid,@ContractBill,@CustBilling
	 
	   
WHILE @@FETCH_STATUS = 0
BEGIN  	
	 
	SET FMTONLY OFF;

	SET @TempNote = @Notes
	SET @tempDesc = replace(@TempNote,'-','')

    if (@ServiceType is not null or @ServiceType <> '')
	begin
		set @tempDesc = REPLACE(CAST(@tempDesc as varchar(max)),'@s',@servicetype+' - ')
	end
	else
	begin
		set @tempDesc = REPLACE(CAST(@tempDesc as varchar(max)),'@s ','')
	end

	set @tempDesc = REPLACE(@tempDesc,'@f',@Frequency)

	select @jobDesc = isnull(fDesc,'') from Job where ID = @job

	if(@jobDesc <> '')																	-- get contract description and if exist then replace it with @d
	begin
		set @tempDesc = REPLACE(@tempDesc,'@d',@jobDesc)
	end
	else
	begin
		set @tempDesc = REPLACE(@tempDesc,'@d ','')
	end
																						-- replace with @p with invoice date month and year 
	set @tempDesc = REPLACE(@tempDesc,'@p',DATENAME(MONTH,convert(datetime, convert(varchar(50),@Idate))) +' - ' + CONVERT(varchar(10),DATEPART(yyyy,@Idate)))

	
	insert into #temp
	values(@fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
@Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@fdesci,@frequency,@name,@customername,@locid,@locname,@dworker,@bcycle,@servicetype,@lid,@contractbill,@custBilling)



FETCH NEXT FROM db_cursor INTO @fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
@Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@fdesci,@frequency,@name,@customername,@locid,@locname,@dworker,@bcycle,
	   @servicetype,@lid,@ContractBill,@CustBilling


END  

CLOSE db_cursor  
DEALLOCATE db_cursor


while (select top 1  1 from #temp where CustBilling = 1) = 1
begin

	Select top 1 @customername=customername
		from #temp where CustBilling =1 group by customername
		
		Select @Amount=sum(amount), @stax=sum(stax), @total=sum(total)
		from #temp where CustBilling =1 and customername=@customername

	Select top 1 @fdate=fdate,@tempDesc=fdesc,
	--@stax=stax, @total=total,
	@taxRegion=taxregion,@Taxfactor=taxfactor,@taxable=taxable,@type=type, @job=job, @loc=loc, @terms=terms, @PO=PO, 
		@Status=Status, @Remarks=Remarks, @gtax=gtax, @mech=worker, @TaxRegion2=taxregion2, 
		@Taxrate2=taxrate2, @BillTo=billto, @Idate=Idate, @Fuser=fuser
		from #temp where CustBilling =1 and customername=@customername

	
	insert into @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt) 
	select 0 as Ref,0 as line,t.Acct,t.Quan,i.fDesc,t.Price,(t.Amount+t.STax),@ParaStax,t.job,t.Jobitem,0 as TransID,t.measure, t.stax from #temp t, Inv i where t.Acct=i.ID and customername=@customername
	
	exec @Ref = spCreateInvoice @tblInvoice, @fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@tempDesc,@gtax,@mech,@TaxRegion2,
	@Taxrate2,@BillTo,@Idate,@cfUser, @staxI, @invoiceID, @TicketIDs,@Idate
	
	SELECT @Ref AS 'Ref'
	
	delete from #temp where customername=@customername and CustBilling =1 
	delete from @tblInvoice
	
end

delete from @tblInvoice
DECLARE db_cursor1 CURSOR FOR 
select t.fdate,t.fdesc,i.fDesc ,t.Amount,t.stax,t.total,t.taxRegion,t.taxrate,t.Taxfactor,t.taxable,t.type,t.job,t.loc,@PayTerms,t.PO,t.Status,t.Batch,@Notes,t.gtax,t.worker,t.TaxRegion2,
       t.Taxrate2,t.BillTo,@InvoiceDate,t.Fuser,t.Acct,t.Chart,t.Quan,t.Price,t.Jobitem,t.measure,t.Frequency,t.locid,t.locname,t.ServiceType from #temp  t, Inv i where t.Acct=i.ID
OPEN db_cursor1  
FETCH NEXT FROM db_cursor1 INTO @fdate,@tempDesc,@fdesci,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
       @Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@locid,@locname,@Frequency,@ServiceType

	   
WHILE @@FETCH_STATUS = 0
BEGIN  	
	 
	 SET FMTONLY OFF;
	 delete from @tblInvoice
	INSERT INTO @tblInvoice (Ref,Line,Acct,Quan,fDesc,Price,Amount,STax,Job,Code,TransID,Measure,StaxAmt) 
	values(0,0,@Acct,@Quan,@fdesci,@Price,(@Amount+@stax),@ParaStax,@job,@Jobitem,0,@measure,@stax)
	
	
	exec @Ref = spCreateInvoice @tblInvoice, @fdate,@tempDesc,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@tempDesc,@gtax,@mech,@TaxRegion2,
	@Taxrate2,@BillTo,@Idate,@cfUser, @staxI, @invoiceID, @TicketIDs,@Idate
	
	SELECT @Ref AS 'Ref'

	UPDATE Job
	SET    Custom15 = @ProcessPeriod,
			Custom17 = CONVERT(VARCHAR(50), Getdate(), 121)
	WHERE  ID = @job


FETCH NEXT FROM db_cursor1 INTO @fdate,@tempDesc,@fdesci,@Amount,@stax,@total,@taxRegion,@taxrate,@Taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@Status,@Batch,@Remarks,@gtax,@mech,@TaxRegion2,
@Taxrate2,@BillTo,@Idate,@Fuser,@Acct,@chart,@Quan,@Price,@Jobitem,@measure,@locid,@locname,@Frequency,@ServiceType

END  

CLOSE db_cursor1
DEALLOCATE db_cursor1

COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
