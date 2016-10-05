CREATE FUNCTION [dbo].[IDSplit](@String varchar(8000), @Delimiter char(1))       
    returns @temptable TABLE (row int, items varchar(8000))       
    as       
    begin   
        set @String=RTRIM(LTRIM(@string))  
    
        declare @idx int       
        declare @slice varchar(8000)       
          
        select @idx = 1       
            if len(@String)<1 or @String is null  return       
          
        while @idx!= 0       
        begin       
            set @idx = charindex(@Delimiter,@String)       
            if @idx!=0       
                set @slice = left(@String,@idx - 1)       
            else       
                set @slice = @String       
              
            if(len(replace(replace(ltrim(rtrim(@slice)),CHAR(13),''),CHAR(10),''))>0)  
                insert into @temptable(row,Items) values((select isnull(max(row),0)+1 from @temptable),@slice)       
      
            set @String = right(@String,len(@String) - @idx)       
            if len(@String) = 0 break       
        end   
    return       
    end
