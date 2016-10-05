CREATE PROCEDURE [dbo].[spAddRecurringInvoices]
@fLoc int,
@fOwner int,
@fMonth int,
@fYear int

as

   declare @fdate datetime
   declare @fdesc varchar(max)
   declare @amount numeric(30,2)
   declare @stax int
   declare @taxregion varchar(25)
   --declare @taxregion numeric(30,2)
   declare @total numeric(30,2) 
   declare @taxrate numeric(30,2)
   declare @taxfactor numeric(30,2)
   declare @taxable int
   declare @type int
   declare @job int
   declare @loc int
   declare @terms varchar(10)
   declare @PO varchar(25)
   declare @status int 
   declare @batch int
   declare @remarks varchar(50)
   declare @gtax int
   declare @worker varchar(75)
   declare @taxregion2 varchar(50)
   declare @taxrate2 numeric(30,2)
   declare @billto varchar(1000)
   declare @Idate datetime
   declare @fuser varchar(10)
   declare @acct int
   declare @Quan numeric(30,2)
   declare @price numeric(30,2)
   declare @Jobitem int
   declare @measure varchar(10)
   declare @fdescI varchar(100)
   declare @Frequency varchar(50)
   declare @Name varchar(25)
   declare @customername varchar(75)
   declare @locid varchar(50)
   declare @locname varchar(75)
   declare @dworker varchar(50)
   declare @bcycle int
   declare @ServiceType varchar(15)
   declare @Lid varchar(75)
   declare @ContractBill smallint
   declare @chart int
   declare @text nvarchar(max)
   declare @text1 nvarchar(max)
   declare @custBilling smallint

Create Table #temp(
 fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax int,
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
    remarks varchar(50),
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
    locname varchar(75),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint
)
Create Table #tempMonthly(
 fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax int,
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
    remarks varchar(50),
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
    locname varchar(75),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint
)
Create Table #tempYearly(
 fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax int,
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
    remarks varchar(50),
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
    locname varchar(75),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint
)
Create Table #tempSelect(
 fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax int,
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
    remarks varchar(50),
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
    locname varchar(75),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint
)
Create Table #tempFinal(
 fdate datetime,
    fdesc varchar(max),
    amount numeric(30,2),
    stax int,
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
    remarks varchar(50),
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
    locname varchar(75),
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint
)


set @text=' 
SELECT distinct c.BStart                         AS fdate, 
	   '''' as fdesc,
       c.BAmt as amount, 
       (((c.BAmt * 1)* st.Rate)/100) AS stax, 
       (isnull(st.Rate,0)+isnull(c.BAmt,0))                  AS total, 
	   isnull(l.STax,'''')                                AS Taxregion, 
       
       (isnull(st.Rate,0))                             AS taxrate, 
       100.00                              AS taxfactor, 
       0                                   AS taxable, 
       0                                   AS type, 
       j.ID                                AS job, 
       a.Loc,
       ''''                                  AS terms, 
       j.PO, 
       l.status, 
       ''0''                            AS batch, 
       ''Recurring''                         AS remarks, 
       0                                   AS gtax, 
       j.Custom20                          AS worker, 
       ''''                                 AS Taxregion2, 
       0.00                                AS taxrate2, 
	   a.billto,
      c.BStart                         AS Idate, 
       ''''                                  AS fuser, 
      -- (select Top 1  ID from Inv where Name=''recurring'') AS InvID, 
	  -- (select Top 1 ID from Inv where Name=''recurring'') AS acct, 
	   isnull(inv.ID,(select Top 1 ID from Inv where Name=''recurring'')) as acct,
	   isnull(c.chart,(select Top 1 SAcct from Inv where Name=''recurring'')) as chart,
       1.00                                AS Quan, 
       c.BAmt                              AS price, 
	   0                                   AS jobitem, 
       isnull(inv.Measure,(SELECT Top 1 measure 
        FROM   Inv I 
        WHERE  I.Name = ''Recurring''))       AS measure, 
       CASE c.BCycle 
         WHEN 0 THEN ''Monthly recurring billing'' 
         WHEN 1 THEN ''Bi-Monthly recurring billing''
         WHEN 2 THEN ''Quarterly recurring billing''
		 WHEN 3 THEN ''3 Times/Year recurring billing''
         WHEN 4 THEN ''Semi-Annually recurring billing''
         WHEN 5 THEN ''Annually recurring billing''
       END                                 AS fdescI, 
       CASE c.bcycle 
         WHEN 0 THEN ''Monthly''
         WHEN 1 THEN ''Bi-Monthly'' 
         WHEN 2 THEN ''Quarterly''
         WHEN 3 THEN ''3 Times/Year''
         WHEN 4 THEN ''Semi-Annually''
         WHEN 5 THEN ''Annually''
         WHEN 6 THEN ''Never''
       END                                 Frequency, 
       st.Name, 
       (SELECT TOP 1 name 
        FROM   rol 
        WHERE  id = (SELECT TOP 1 rol 
                     FROM   owner 
                     WHERE  id = l.Owner)) AS customername, 
       a.ID as locid,
	   a.Tag as locname,
       (SELECT Name 
        FROM   Route ro 
        WHERE  ro.ID = j.Custom20)         AS dworker ,
        c.bcycle,
		lt.type as serviceType,
        --(select type from ltype lt where lt.type=j.ctype) as serviceType,

		a.lid,
		a.ContractBill,
		isnull(o.Billing,0) as CustBilling
FROM   Loc l 
       LEFT OUTER JOIN STax st 
                    ON l.STax = st.Name 
       INNER JOIN Job j 
               ON l.Loc = j.Loc 
       LEFT JOIN ltype lt on lt.type=j.ctype
	   LEFT JOIN Inv inv on lt.InvID=inv.ID 
	   LEFT JOIN Owner o on l.Owner = o.ID
       INNER JOIN Contract c 
               ON j.ID = c.Job       
		left join
		(select li.Loc,t.Job,t.lid,t.ContractBill,li.ID,li.Tag, li.Address +'','' + li.City+'', ''+li.State+'', ''+li.Zip AS billto from loc as li right join (select distinct c.Job, case when o.Billing=1 then o.Central else l.loc end as lid, isnull(l.Billing,0) As ContractBill
		from loc as l, Owner as o, Contract as c where l.loc =c.Loc and l.Owner=o.ID) t 
		ON li.Loc = t.lid) a
		ON c.Job = a.Job     '    
		 --WHERE  
		 --YEAR(c.BStart) >= '''+ convert(varchar(10),@fYear)+''''
if(@fLoc <> 0)
begin
	set @text+=' where l.loc='+convert(nvarchar(50),@fLoc)

	if(@fOwner<>0)
	begin
		set @text+=' and l.owner='+convert(nvarchar(50),@fOwner)
	end
end

else if(@fOwner<>0)
begin
	set @text+=' where l.owner='+convert(nvarchar(50),@fOwner)
end




Insert into #Temp
exec (@text)


DECLARE db_cursor CURSOR FOR 

select * from #temp 

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @fdate,@fdesc,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@dworker,@bcycle,@ServiceType, @Lid, @ContractBill, @custBilling

WHILE @@FETCH_STATUS = 0
BEGIN  				

	DECLARE @FlagConst INT = 12
		
		DECLARE @Flag INT = 1			
		DECLARE @intFlag INT
		DECLARE @intDayFlag INT	
		SET @intFlag =  DATEPART ( m , @fdate)
		
		WHILE (@Flag <=@FlagConst )
		BEGIN
		
		declare @sdate datetime
	
		
		set @sdate=DATEADD(m, @intFlag - MONTH( @fdate),  @fdate)
	
	
	--	WHILE (@intFlag <=DATEPART ( m , @EndDate))
	--	BEGIN
		if @fdate=@sdate
		begin
		set @sdate=DATEADD(m, @intFlag - MONTH( @fdate),  @fdate)
		end
		else
		begin

		insert into #tempMonthly	
		(
		 fdate ,    fdesc ,    amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO ,
		     status  ,    batch ,    remarks ,    gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser ,  acct , chart,   Quan ,
		         price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,    customername,    locid,    locname ,    dworker ,    bcycle ,ServiceType, Lid,ContractBill, CustBilling
		)	
		values
		(@sdate,@fdesc,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
		@gtax,@worker,@taxregion2,@taxrate2,@billto,@sdate,@fuser,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@dworker,@bcycle ,@ServiceType, @Lid, @ContractBill, @custBilling
		)
		end
		-- Monthly
		if(@bcycle=0)
		begin
		SET @intFlag = @intFlag + 1
		end		
		else
		-- Bi-Monthly
		if(@bcycle=1)
		begin
		SET @intFlag = @intFlag + 2
		end
		else
		-- Quarterly
		if(@bcycle=2)
		begin
		SET @intFlag = @intFlag + 3
		end
		else
	    -- 3 times a year
	    if(@bcycle=3)
		begin
		SET @intFlag = @intFlag + 4
		end
		
		-- Semiannually
		if(@bcycle=4)
		begin
		SET @intFlag = @intFlag + 6
		end
		else
		-- Annually
		if(@bcycle=5)
		begin
		SET @intFlag = @intFlag + 12
		end
		
		SET @Flag = @Flag + 1
END			

		
FETCH NEXT FROM db_cursor INTO  @fdate,@fdesc,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@dworker,@bcycle ,@ServiceType, @Lid, @ContractBill , @custBilling

END  

CLOSE db_cursor  
DEALLOCATE db_cursor


insert into #tempFinal 
select * from #temp 
--where 
--fdate <= @selectedDate
--MONTH(fdate) <= @fMonth or year(fdate) <= @fYear
	

union

select * from #tempMonthly
--where 
--fdate <= @selectedDate
--MONTH(fdate) <= @fMonth or year(fdate) <= @fYear


DECLARE db_cursor CURSOR FOR 

select * from #tempFinal 

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @fdate,@fdesc,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@dworker,@bcycle,@ServiceType, @Lid, @ContractBill, @custBilling

WHILE @@FETCH_STATUS = 0
BEGIN  	
declare @IntYear int =DATEPART ( YEAR , @fdate) 
WHILE ( @IntYear <= @fYear )
		BEGIN
		set @IntYear = @IntYear+1

		set @sdate=DATEADD(YEAR,@IntYear-YEAR(@fdate),@fdate)
		
		insert into #tempYearly 
		(
		 fdate ,    fdesc ,    amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO ,
		     status  ,    batch ,    remarks ,    gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser ,  acct ,  chart,   Quan ,
		         price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,    customername,    locid,    locname ,    dworker ,    bcycle ,ServiceType, Lid, ContractBill, CustBilling
		)	
		values
		(@sdate,@fdesc,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
		@gtax,@worker,@taxregion2,@taxrate2,@billto,@sdate,@fuser,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@dworker,@bcycle ,@ServiceType, @Lid,@ContractBill, @custBilling
		)
		End
		
						
		
FETCH NEXT FROM db_cursor INTO  @fdate,@fdesc,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@dworker,@bcycle ,@ServiceType, @Lid, @ContractBill, @custBilling

END	

CLOSE db_cursor  
DEALLOCATE db_cursor


insert into #tempSelect
select * from #tempFinal
union
select * from #tempYearly




set @text1=
'select * from #tempSelect where 
MONTH(fdate) ='''+ convert(varchar(10),@fMonth)+''' AND YEAR(fdate) = '''+ convert(varchar(10),@fYear)+'''

            
except 

SELECT distinct 
	   i.fDate	 AS fDate,
	   '''' as fdesc,
       c.BAmt as amount, 
        (((c.BAmt * 1)* st.Rate)/100) AS stax, 
	    (isnull(st.Rate,0)+isnull(c.BAmt,0))                    AS total, 
        isnull(l.STax,'''')                             AS Taxregion, 
   
       (isnull(st.Rate,0))                             AS taxrate, 
       100.00                              AS taxfactor, 
       0                                   AS taxable, 
       0                                   AS type, 
       j.ID                                AS job, 
       a.Loc, 
       ''''                                  AS terms, 
       j.PO, 
       l.status, 
       ''0''                            AS batch, 
       ''Recurring''                       AS remarks, 
       0                                   AS gtax, 
       j.Custom20                          AS worker, 
       ''''                                  AS Taxregion2, 
       0.00                                AS taxrate2, 
       a.billto,
       i.fDate	                        AS Idate, 
       ''''                                  AS fuser, 
	  -- (select SAcct from Inv where Name=''recurring'') AS InvID, 
       --(select Top 1 ID from Inv where Name=''recurring'') AS acct, 
	   isnull(inv.ID,(select Top 1 ID from Inv where Name=''recurring'')) as acct,
	   isnull(c.chart,(select Top 1 SAcct from Inv where Name=''recurring'')) as chart,
       1.00                                AS Quan, 
       c.BAmt                              AS price, 
	   0                                   AS jobitem, 
       isnull(inv.Measure,(SELECT Top 1 measure 
        FROM   Inv I 
        WHERE  I.Name = ''Recurring''))       AS measure, 
       CASE c.BCycle 
         WHEN 0 THEN ''Monthly recurring billing''
         WHEN 1 THEN ''Bi-Monthly recurring billing''
         WHEN 2 THEN ''Quarterly recurring billing''
		 WHEN 3 THEN ''3 Times/Year recurring billing''
         WHEN 4 THEN ''Semi-Annually recurring billing''
         WHEN 5 THEN ''Annually recurring billing''
       END                                 AS fdescI, 
       CASE c.bcycle 
         WHEN 0 THEN ''Monthly''
         WHEN 1 THEN ''Bi-Monthly'' 
         WHEN 2 THEN ''Quarterly''
         WHEN 3 THEN ''3 Times/Year''
         WHEN 4 THEN ''Semi-Annually''
         WHEN 5 THEN ''Annually''
         WHEN 6 THEN ''Never''
       END                                 Frequency, 
       st.Name, 
       (SELECT TOP 1 name 
        FROM   rol 
        WHERE  id = (SELECT TOP 1 rol 
                     FROM   owner 
                     WHERE  id = l.Owner)) AS customername, 
       a.ID as locid,
	   a.Tag as locname,
       (SELECT Name 
        FROM   Route ro 
        WHERE  ro.ID = j.Custom20)         AS dworker ,
        c.bcycle,
		lt.Type as serviceType,
		a.lid,
        a.ContractBill,
		isnull(o.Billing,0) as CustBilling
FROM   Loc l 
       LEFT OUTER JOIN STax st 
                    ON l.STax = st.Name 
       INNER JOIN Job j 
               ON l.Loc = j.Loc 
	   LEFT JOIN ltype lt on lt.type=j.ctype
	   LEFT JOIN Inv inv on lt.InvID=inv.ID 
	   LEFT JOIN Owner o on l.Owner = o.ID
       INNER JOIN Contract c 
               ON j.ID = c.Job 
	   INNER JOIN Invoice I on I.Job=j.ID 
	   left join
		(select li.Loc,t.Job,t.lid,t.ContractBill,li.ID,li.Tag, li.Address +'','' + li.City+'', ''+li.State+'', ''+li.Zip AS billto from loc as li right join (select distinct c.Job, case when o.Billing=1 then o.Central else l.loc end as lid, isnull(l.Billing,0) As ContractBill
		from loc as l, Owner as o, Contract as c where l.loc =c.Loc and l.Owner=o.ID) t
		ON li.Loc = t.lid) a
		ON c.Job = a.Job         
       WHERE
			MONTH(i.fDate) ='''+ convert(varchar(10),@fMonth)+''' AND YEAR(i.fDate) = '''+ convert(varchar(10),@fYear)+''''
			
			
if(@fLoc <> 0)
begin
	set @text1+=' and c.Loc='+convert(nvarchar(50),@fLoc)
end
if(@fOwner<>0)
begin
	set @text1+=' and c.owner='+convert(nvarchar(50),@fOwner)
end
set @text1+=' order by job'

exec (@text1)

  
drop table #temp
drop table #tempMonthly
drop table #tempFinal
drop table #tempYearly
drop table #tempSelect
