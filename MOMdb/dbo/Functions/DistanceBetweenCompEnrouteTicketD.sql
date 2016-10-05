CREATE FUNCTION [dbo].[DistanceBetweenCompEnrouteTicketD] (@ticketID int, @date datetime)--
RETURNS real
AS
BEGIN
                   
declare @lat1 varchar(50)
declare @lon1 varchar(50)
declare @lat2 varchar(50)
declare @lon2 varchar(50)
declare @dist real=0
declare @bool bit=0       
declare @edate datetime
declare @dwork varchar(50)
declare @NextID int
declare @Nexttimert datetime
declare @timecomp datetime
declare @gpsdate datetime

select @edate= edate,@dwork= dwork,@timecomp=timecomp from (
select edate, dwork, CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timecomp AS TIME) AS DATETIME)as timecomp  from ticketo where id=@ticketID
union
select edate, (select fdesc from tblwork where id=fwork) as dwork, CAST(CAST(edate AS DATE) AS DATETIME) +  CAST(CAST(timecomp AS TIME) AS DATETIME) as timecomp 
from ticketd where id=@ticketID
) A

select top 1 @NextID=ID,@Nexttimert=Nexttimert from(
select edate, ID,CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME) AS DATETIME)  as Nexttimert 
from ticketo 
where edate>(@edate)
and  dwork = @dwork 
AND Dateadd(DAY, Datediff(DAY, 0, edate), 0) = @date  AND assigned <> 0   

union

select edate, ID,CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME) AS DATETIME) as Nexttimert 
from ticketd 
where edate>(@edate)
and  (select fdesc from tblwork where id=fwork) = @dwork 
AND Dateadd(DAY, Datediff(DAY, 0, edate), 0) = @date  

) B order by edate 
                   
DECLARE CursorMail CURSOR  FOR 

select distinct latitude,longitude, date
from  [MSM2_Admin].dbo.mapdata m 
inner join Emp e on e.DeviceID=m.deviceId                  
inner join Ticketo t on e.fWork=t.fWork                         
where
(
	t.ID=@ticketID 
	or 
	t.ID=@NextID	
)
and 
( 
	m.date between 
	@timecomp
	and 
	@Nexttimert
)

union 

select distinct latitude,longitude, date
from  [MSM2_Admin].dbo.mapdata m 
inner join Emp e on e.DeviceID=m.deviceId                  
inner join Ticketd t on e.fWork=t.fWork                         
where
(
	t.ID=@ticketID 
	or 
	t.ID=@NextID	
)
and 
( 
	m.date between 
	@timecomp
	and 
	@Nexttimert
)

order by date
                                   
      
OPEN CursorMail
FETCH NEXT FROM CursorMail INTO @lat1, @lon1,@gpsdate
WHILE @@FETCH_STATUS = 0

BEGIN
if(@bool<>0)
	begin
		set @dist=@dist+dbo.DistanceBetween(@lat1,@lon1,@lat2,@lon2)
    end
    
    set @bool=1
    set @lat2=@lat1
	set @lon2=@lon1
    
FETCH NEXT FROM CursorMail INTO @lat1, @lon1,@gpsdate
END

CLOSE CursorMail
DEALLOCATE CursorMail

return (@dist);
END
