CREATE FUNCTION [dbo].[DistanceBetweenEnrouteOnsite] (@ticketID int)
RETURNS real
AS
BEGIN
                   
declare @lat1 varchar(50)
declare @lon1 varchar(50)
declare @lat2 varchar(50)
declare @lon2 varchar(50)
declare @dist real=0
declare @bool bit=0  
declare @date datetime                  
                   
DECLARE CursorMail CURSOR  FOR 
select latitude,longitude,date from  [MSM2_Admin].dbo.mapdata m 
                  inner join Emp e on e.DeviceID=m.deviceId
                  inner join TicketD t on e.fWork=t.fWork
                   where  t.ID=@ticketid and
                 m.date between  CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME)AS DATETIME) and  CAST(CAST(edate AS DATE) AS DATETIME) + CAST( CAST(timesite AS TIME)AS DATETIME)    
union 
select latitude,longitude,date from  [MSM2_Admin].dbo.mapdata m 
                  inner join Emp e on e.DeviceID=m.deviceId
                  inner join TicketO t on e.fWork=t.fWork
                   where  t.ID=@ticketid and
                 m.date between  CAST(CAST(edate AS DATE) AS DATETIME) + CAST(CAST(timeroute AS TIME)AS DATETIME) and  CAST(CAST(edate AS DATE) AS DATETIME) + CAST( CAST(timesite AS TIME)AS DATETIME)    
                  
              order by date                            
      
OPEN CursorMail
FETCH NEXT FROM CursorMail INTO @lat1, @lon1, @date
WHILE @@FETCH_STATUS = 0

BEGIN
if(@bool<>0)
	begin
		set @dist=@dist+dbo.DistanceBetween(@lat1,@lon1,@lat2,@lon2)
    end
    
    set @bool=1
    set @lat2=@lat1
	set @lon2=@lon1
    
FETCH NEXT FROM CursorMail INTO @lat1, @lon1, @date
END

CLOSE CursorMail
DEALLOCATE CursorMail

return (@dist);
END
