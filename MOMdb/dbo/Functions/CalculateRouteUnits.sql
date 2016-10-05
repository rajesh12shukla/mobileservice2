create FUNCTION [dbo].[CalculateRouteUnits](@route int)       
    returns int     
    as       
    begin       
       
			DECLARE @units INT = 0 
			DECLARE @loc INT
			DECLARE db_cursor CURSOR			
			 FOR			
			SELECT l.loc from loc l 
			inner join contract c on l.loc=c.loc 
			where l.route=@route and c.status=0 
								
			OPEN db_cursor
			FETCH NEXT
			FROM db_cursor INTO @loc
			WHILE @@FETCH_STATUS = 0
			BEGIN
			
				set @units += (select count(1) from Elev el where el.loc =  @loc)
				
				FETCH NEXT
				FROM db_cursor INTO @loc
			
			END
			CLOSE db_cursor
			DEALLOCATE db_cursor
           
    return       @units
    end
