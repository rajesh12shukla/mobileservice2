

CREATE PROCEDURE [dbo].[spAddRecurringTickets]
@Loc int,
@Remarks varchar(255),
@PerContract int,
@ContractRemarks int,
@Owner int,
@Worker varchar(50),
@StartDate datetime,
@EndDate datetime,
@OnDemand smallint 

as

declare @LocID int
declare @LocTag varchar(50)
declare @LocAdd varchar(100)
declare @City varchar(50)
declare @State varchar(2)
declare @Zip varchar(100)
declare @scycle int
declare @Phone varchar(28)
declare @Cell varchar(50)
declare @CallDt datetime
declare @SchDt datetime
declare @Status smallint
declare @Category varchar(25)
declare @Unit int
declare @custID int
declare @JobRemarks varchar(max)
declare @job int
declare @Comp int
declare @unitElev varchar(20)
declare @dwork varchar(50)
declare @ID int
declare @customername varchar(75)
declare @locidName varchar(50)
declare @locname varchar(75)
declare @edate datetime
declare @cdate datetime
declare @est numeric(30,2)
declare @rem varchar(255)
declare @text varchar(max)
declare @text1 varchar(max)
declare @WhereText varchar(max)=''
declare @ExpirationDate datetime
declare @ctype varchar(15)
declare @swe smallint

if(@OnDemand=1)
begin
	set @WhereText+= ' and scycle = 12'
end
else
begin
	set @WhereText+= ' and scycle <> 12'
end

if(@loc <> 0)
begin
	set @WhereText+=' and l.loc='+convert(nvarchar(50),@Loc)
end

if(@Owner<>0)
begin
	set @WhereText+=' and l.owner='+convert(nvarchar(50),@Owner)
end

if(@Worker<> '')
begin
	if(@Worker='0')
	begin	
	set @WhereText+=' and j.Custom20=0 '
	end
	else
	begin
	set @WhereText+=' and (select fdesc from tblwork tw where id = (select mech from route w where  w.ID=j.Custom20))=''' +convert(nvarchar(50),@Worker)+''''
	end
end


if(@PerContract = 1)
begin
set @text='select distinct
l.Loc as  loc,
l.Address,
l.City,
l.State,
l.Zip,
scycle,
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as calldate,'

if(@OnDemand=1)
begin
set @text+=' null as edate, null as scheduledt, 0 as assigned, null as worker, null as dwork,'
end
else
begin
set @text+= ' 
CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as edate,
CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as scheduledt,	
case j.Custom20 when 0 then 0 else 1 end as assigned,
(select fdesc from tblwork tw where id = (select mech from route w where  w.ID=j.Custom20)) as worker,
(select fdesc from tblwork tw where id = (select mech from route w where  w.ID=j.Custom20)) as dwork,
'
end

set @text+= '
''Recurring'' as category, 
jej.Elev,
l.Owner,
CONVERT(VARCHAR(MAX), j.remarks)  as jobremarks, 
'''+@Remarks+''' as remarks,
c.job,
0 as comp,
(SELECT Unit FROM   elev WHERE  id = jej.Elev)  AS unit,                   
0 as ID,
(SELECT TOP 1 name
FROM   rol
WHERE  id = (SELECT TOP 1 rol
             FROM   owner
             WHERE  id = l.Owner)) AS customername,
l.id AS locid,
l.tag AS locname,                                 
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as cdate,             
isnull(jej.hours,0.50)  as est,
ExpirationDate,ctype, isnull(swe,0) as swe	

from Contract c
left outer join tblJoinElevJob jej on jej.Job = c.Job
inner join Job j on j.ID = c.Job
inner join loc l on l.loc= c.loc
where l.loc is not null and j.status=0  and c.scycle<>-1
'
set @text+=@WhereText
end
else
begin
set @text=' 
select distinct
l.Loc as loc,
l.Address,
l.City,
l.State,
l.Zip,
scycle,
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as calldate,'

if(@OnDemand=1)
begin
	set @text+=' null as edate, null as scheduledt, 0 as assigned, null as worker, null as dwork,'
end
else
begin
	set @text+= ' 
	CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as edate,
	CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as scheduledt,	
	case j.Custom20 when 0 then 0 else 1 end as assigned,
	(select fdesc from tblwork tw where id =(select mech from route w where  w.ID=j.Custom20)) as worker,
	(select fdesc from tblwork tw where id =(select mech from route w where  w.ID=j.Custom20)) as dwork,
	'
end

set @text+= '''Recurring'' as category, 
null as Elev,
l.Owner,
CONVERT(VARCHAR(MAX), j.remarks)  as jobremarks,
'''+@Remarks+''' as remarks,
j.id as job,
0 as comp,
(SELECT Unit FROM   elev WHERE  id = Elev)  AS unit,                   
0 as ID,
(SELECT TOP 1 name
FROM   rol
WHERE  id = (SELECT TOP 1 rol
             FROM   owner
             WHERE  id = l.Owner)) AS customername,
l.id AS locid,
l.tag AS locname,            
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as cdate,             
isnull(hours,0.50)  as est,
ExpirationDate, ctype, isnull(swe,0) as swe	
from Job j 
inner join loc l on l.loc=j.loc
inner join contract c on c.job=j.id
where l.loc is not null and j.status=0  and c.scycle<>-1
'
set @text+=@WhereText
end

set @text +=' order by Loc'


if(@OnDemand<>1)
begin
set @text1=
'
select distinct *  from #tempFinal 
where edate>= convert(datetime, '''+convert(varchar(50),@StartDate,101)+''') and edate<  convert(datetime,'''+convert(varchar(50),DATEADD(DAY,1 ,@EndDate),101)+''')
and edate <= isnull(ExpirationDate , cast(''12/31/9999'' as datetime))  
except
(
select distinct tf.*  from #tempFinal tf
 INNER JOIN ticketo t
               ON t.Job = tf.job 
					--and t.recurring = convert(datetime, convert(date,  tf.scheduledt))
                  and datepart(month,  t.edate)=  datepart(month,  scheduledt) 
                  and datepart(year,  t.edate)=  datepart(year,  scheduledt) 
                  ----AND isnull(t.lelev,0) = isnull(tf.elev,0)
                  --and t.lid=tf.loc
where 
tf.edate>= convert(datetime, '''+convert(varchar(50),@StartDate,101)+''') 
and tf.edate<  convert(datetime,'''+convert(varchar(50),DATEADD(DAY,1 ,@EndDate),101)+''')
and t.job <>0 
--and t.cat=''Recurring'' 
union
select distinct tf.*  from #tempFinal tf
 INNER JOIN ticketd t
               ON t.Job = tf.job 
					--and t.recurring = convert(datetime, convert(date,  tf.scheduledt) )    
                  and datepart(month,  t.edate)=  datepart(month,  scheduledt) 
                  and datepart(year,  t.edate)=  datepart(year,  scheduledt) 
                 ----AND isnull(t.elev,0) = isnull(tf.elev,0)
                  --and t.loc=tf.loc
where tf.edate>= convert(datetime, '''+convert(varchar(50),@StartDate,101)+''') 
and tf.edate<  convert(datetime,'''+convert(varchar(50),DATEADD(DAY,1 ,@EndDate),101)+''')
and t.job <>0 
--and t.cat=''Recurring'' 
)
'



Create Table #temp(
Loc int,
Address varchar(255),
city varchar(50),
state varchar(2),
zip varchar(10),
scycle int,
calldate datetime,
edate datetime,
scheduledt datetime,
assigned int,
worker varchar(50),
dwork varchar(50),
category varchar(50),
Elev int,
Owner int,
jobremarks varchar(max),
remarks varchar(max),
job int,
Comp int,
unit varchar(20),
ID int,
customername varchar(75),
locid varchar(50),
locname varchar(75),
cdate datetime,
est numeric(30,2),
ExpirationDate	datetime, 
ctype varchar(15),
swe smallint
)

Create Table #tempMonthly(
Loc int,
Address varchar(255),
city varchar(50),
state varchar(2),
zip varchar(10),
scycle int,
calldate datetime,
edate datetime,
scheduledt datetime,
assigned int,
worker varchar(50),
dwork varchar(50),
category varchar(50),
Elev int,
Owner int,
jobremarks varchar(max),
remarks varchar(max),
job int,
Comp int,
unit varchar(20),
ID int,
customername varchar(75),
locid varchar(50),
locname varchar(75),
cdate datetime,
est numeric(30,2),
ExpirationDate	datetime,
ctype varchar(15),
swe smallint
)

Create Table #tempFinal(
Loc int,
Address varchar(255),
city varchar(50),
state varchar(2),
zip varchar(10),
scycle int,
calldate datetime,
edate datetime,
scheduledt datetime,
assigned int,
worker varchar(50),
dwork varchar(50),
category varchar(50),
Elev int,
Owner int,
jobremarks varchar(max),
remarks varchar(max),
job int,
Comp int,
unit varchar(20),
ID int,
customername varchar(75),
locid varchar(50),
locname varchar(75),
cdate datetime,
est numeric(30,2),
ExpirationDate	datetime,
ctype varchar(15),
swe smallint
)

Create Table #tempYearly(
Loc int,
Address varchar(255),
city varchar(50),
state varchar(2),
zip varchar(10),
scycle int,
calldate datetime,
edate datetime,
scheduledt datetime,
assigned int,
worker varchar(50),
dwork varchar(50),
category varchar(50),
Elev int,
Owner int,
jobremarks varchar(max),
remarks varchar(max),
job int,
Comp int,
unit varchar(20),
ID int,
customername varchar(75),
locid varchar(50),
locname varchar(75),
cdate datetime,
est numeric(30,2),
ExpirationDate	datetime,
ctype varchar(15),
swe smallint
)

Create Table #tempWeekendFilter(
Loc int,
Address varchar(255),
city varchar(50),
state varchar(2),
zip varchar(10),
scycle int,
calldate datetime,
edate datetime,
scheduledt datetime,
assigned int,
worker varchar(50),
dwork varchar(50),
category varchar(50),
Elev int,
Owner int,
jobremarks varchar(max),
remarks varchar(max),
job int,
Comp int,
unit varchar(20),
ID int,
customername varchar(75),
locid varchar(50),
locname varchar(75),
cdate datetime,
est numeric(30,2),
ExpirationDate	datetime,
ctype varchar(15),
swe smallint
)

Insert into #Temp
exec(@text)



DECLARE db_cursor CURSOR FOR 
select * from #temp 
OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO 
@locid,
@LocAdd,
@City,
@State,
@Zip,
@scycle,	
@CallDt,
@edate,
@SchDt,
@Status,
@Worker,
@dwork,
@Category,
@Unit,
@custID,
@JobRemarks,
@rem,
@job,
@Comp,
@unitElev,
@ID,
@customername,
@locidName,
@locname,
@cdate,
@est,
@ExpirationDate,
@ctype,
@swe 

WHILE @@FETCH_STATUS = 0
BEGIN  				
		DECLARE @FlagConst INT = 12
		if(@scycle=5 or @scycle=6 or @scycle=7)
		begin
		set @FlagConst=DATEPART ( WEEK , '12/31/2013')
		end
		else if(@scycle=4 or @scycle=8 or @scycle =9)
		begin
		set @FlagConst= 0
		end
		
		DECLARE @Flag INT = 1			
		DECLARE @intFlag INT
		DECLARE @intDayFlag INT	
		SET @intFlag =  DATEPART ( m , @SchDt)
		SET @intDayFlag =  DATEPART ( WEEK , @SchDt)	
		
		WHILE (@Flag <=@FlagConst )
		BEGIN
		
		declare @sdate datetime
		if(@scycle=5 or @scycle=6 or @scycle=7)
		begin
			set @sdate=DATEADD(WEEK, @intDayFlag - datepart(WEEK,@SchDt),  @SchDt)				
		end
		else		
		begin
			set @sdate=DATEADD(m, @intFlag - MONTH( @SchDt),  @SchDt)
		end
		
		insert into #tempMonthly
		(Loc ,Address ,city ,state ,zip ,scycle ,worker ,calldate ,scheduledt ,assigned ,category ,Elev ,Owner ,jobremarks ,remarks ,job,
		Comp,unit,dwork,ID,customername,locid,locname,edate,cdate,est,ExpirationDate,ctype,swe
		)
		values
		( @locid,@LocAdd,@City,@State,@Zip,@scycle,@Worker,@CallDt, @sdate ,@Status,@Category,@Unit,@custID,@JobRemarks,@Remarks,@job,
		@Comp,@unitElev,@dwork,@ID,@customername,@locidName,@locname,@sdate,@CallDt,@est,@ExpirationDate,@ctype,@swe
		)
		
		/* Monthly */
		if(@scycle=0)
		begin
		SET @intFlag = @intFlag + 1
		end		
		else
		/* Bi-Monthly */
		if(@scycle=1)
		begin
		SET @intFlag = @intFlag + 2
		end
		else
		/* Quarterly */
		if(@scycle=2)
		begin
		SET @intFlag = @intFlag + 3
		end
		else
		/* Semiannually */
		if(@scycle=3)
		begin
		SET @intFlag = @intFlag + 6
		end
		--else
		/* Annually */
		--if(@scycle=4)
		--begin
		--SET @intFlag = @intFlag + 12
		--end
		/* weekly */
		else
		if(@scycle=5)
		begin
		SET @intDayFlag = @intDayFlag + 1
		end
		/* Bi-weekly */
		else
		if(@scycle=6)
		begin
		SET @intDayFlag = @intDayFlag + 2
		end
		/* 13-weekly */
		else
		if(@scycle=7)
		begin
		SET @intDayFlag = @intDayFlag + 13
		end
						
				
		SET @Flag = @Flag + 1
		END			
		
		
       FETCH NEXT FROM db_cursor INTO 
        @locid,
		@LocAdd,
		@City,
		@State,
		@Zip,
		@scycle,	
		@CallDt,
		@edate,
		@SchDt,
		@Status,
		@Worker,
		@dwork,
		@Category,
		@Unit,
		@custID,
		@JobRemarks,
		@rem,
		@job,
		@Comp,
		@unitElev,
		@ID,
		@customername,
		@locidName,
		@locname,
		@cdate,
		@est,
		@ExpirationDate,
		@ctype,
		@swe
END  
CLOSE db_cursor  
DEALLOCATE db_cursor



insert into #tempFinal       
     select *  from #temp    t      
       union
     select *  from #tempMonthly   
     
drop table #temp
drop table #tempMonthly



DECLARE db_cursor CURSOR FOR 
select * from #tempFinal 
OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO 
@locid,
@LocAdd,
@City,
@State,
@Zip,
@scycle,	
@CallDt,
@edate,
@SchDt,
@Status,
@Worker,
@dwork,
@Category,
@Unit,
@custID,
@JobRemarks,
@rem,
@job,
@Comp,
@unitElev,
@ID,
@customername,
@locidName,
@locname,
@cdate,
@est,
@ExpirationDate,
@ctype,
@swe
WHILE @@FETCH_STATUS = 0
BEGIN
declare @IntYear int =DATEPART ( YEAR , @SchDt) 
WHILE ( @IntYear <= DATEPART ( YEAR , @EndDate) )
		BEGIN
		
		if(@scycle=8)
		begin
		set @IntYear = @IntYear+3
		end
		
		else if(@scycle=9)
		begin
		set @IntYear = @IntYear+5
		end
		
		else if(@scycle=10)
		begin
		set @IntYear = @IntYear+2
		end
		
		else if(@scycle=11)
		begin
		set @IntYear = @IntYear+7
		end
						
		else
		begin
		set @IntYear = @IntYear+1
		end
		set @sdate=DATEADD(YEAR,@IntYear-year(@SchDt),@SchDt)
		
		insert into #tempYearly 
		(Loc ,Address ,city ,state ,zip ,scycle ,worker ,calldate ,scheduledt ,assigned ,category ,Elev ,Owner ,jobremarks ,remarks ,job,
		Comp,unit,dwork,ID,customername,locid,locname,edate,cdate,est,ExpirationDate,ctype,swe
		)
		values
		( @locid,@LocAdd,@City,@State,@Zip,@scycle,@Worker,@CallDt, @sdate ,@Status,@Category,@Unit,@custID,@JobRemarks,@Remarks,@job,
		@Comp,@unitElev,@dwork,@ID,@customername,@locidName,@locname,@sdate,@CallDt,@est,@ExpirationDate,@ctype,@swe
		)
END

FETCH NEXT FROM db_cursor INTO 
@locid,
@LocAdd,
@City,
@State,
@Zip,
@scycle,	
@CallDt,
@edate,
@SchDt,
@Status,
@Worker,
@dwork,
@Category,
@Unit,
@custID,
@JobRemarks,
@rem,
@job,
@Comp,
@unitElev,
@ID,
@customername,
@locidName,
@locname,
@cdate,
@est,
@ExpirationDate,
@ctype,
@swe
END  
CLOSE db_cursor  
DEALLOCATE db_cursor



insert into #tempFinal select * from #tempYearly
drop table #tempYearly

insert into #tempWeekendFilter
exec(@text1)
drop table #tempFinal

update #tempWeekendFilter set 
scheduledt = 
case  
datepart(WEEKDAY, scheduledt) 
when 1 then DATEADD (DAY, 1 ,scheduledt)
when 7 then DATEADD (DAY, 2 ,scheduledt)
else scheduledt end,
edate=case  
datepart(WEEKDAY, edate) 
when 1 then DATEADD (DAY, 1 ,edate)
when 7 then DATEADD (DAY, 2 ,edate)
else edate end
where swe = 0

select * from #tempWeekendFilter order by job
drop table #tempWeekendFilter

end
else
begin

Create Table #tempWeekendFilter1(
Loc int,
Address varchar(255),
city varchar(50),
state varchar(2),
zip varchar(10),
scycle int,
calldate datetime,
edate datetime,
scheduledt datetime,
assigned int,
worker varchar(50),
dwork varchar(50),
category varchar(50),
Elev int,
Owner int,
jobremarks varchar(max),
remarks varchar(max),
job int,
Comp int,
unit varchar(20),
ID int,
customername varchar(75),
locid varchar(50),
locname varchar(75),
cdate datetime,
est numeric(30,2),
ExpirationDate	datetime,
ctype varchar(15),
swe smallint
)

insert into #tempWeekendFilter1
exec(@text)

update #tempWeekendFilter1 set 
scheduledt = 
case  
datepart(WEEKDAY, scheduledt) 
when 1 then DATEADD (DAY, 1 ,scheduledt)
when 7 then DATEADD (DAY, 2 ,scheduledt)
else scheduledt end,
edate=case  
datepart(WEEKDAY, edate) 
when 1 then DATEADD (DAY, 1 ,edate)
when 7 then DATEADD (DAY, 2 ,edate)
else edate end
where swe = 0

select * from #tempWeekendFilter1 order by job
drop table #tempWeekendFilter1

end
