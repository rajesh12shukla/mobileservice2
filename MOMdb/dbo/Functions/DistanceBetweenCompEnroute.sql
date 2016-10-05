CREATE FUNCTION [dbo].[DistanceBetweenCompEnroute] (@ticketID int, @date datetime)--
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

select @edate=edate, @dwork=dwork, @timecomp=CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timecomp AS TIME) AS DATETIME) from ticketo where id=@ticketID

select  top 1 @NextID=ID,@Nexttimert=CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME)AS DATETIME) from ticketo where CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME)AS DATETIME)>(@edate)and  dwork = @dwork 
AND Dateadd(DAY, Datediff(DAY, 0, edate), 0) = @date  AND assigned <> 0   
order by edate 
                   
DECLARE CursorMail CURSOR  FOR 

select distinct latitude,longitude
from  [MSM2_Admin].dbo.mapdata m 
inner join Emp e on e.DeviceID=m.deviceId                  
inner join TicketO t on e.CallSign=t.DWork                          
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
                                   
      
OPEN CursorMail
FETCH NEXT FROM CursorMail INTO @lat1, @lon1
WHILE @@FETCH_STATUS = 0

BEGIN
if(@bool<>0)
	begin
		set @dist=@dist+dbo.DistanceBetween(@lat1,@lon1,@lat2,@lon2)
    end
    
    set @bool=1
    set @lat2=@lat1
	set @lon2=@lon1
    
FETCH NEXT FROM CursorMail INTO @lat1, @lon1
END

CLOSE CursorMail
DEALLOCATE CursorMail

return (@dist);
END
