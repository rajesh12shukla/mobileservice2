CREATE FUNCTION [dbo].[SplitSentence](@Sentence varchar(max), @length int)       
    returns @Table TABLE (id int, items varchar(8000))       
    as       
    begin  
set @Sentence = rtrim(ltrim(@Sentence))
declare @item  varchar(8000)
declare @newsentence VARCHAR(MAX)=''
declare @c int = 1
declare @index int = 1
DECLARE db_cursor CURSOR FOR select * from split(@Sentence,' ')
       
    OPEN db_cursor
    FETCH NEXT FROM db_cursor INTO @item

    WHILE @@FETCH_STATUS = 0
      BEGIN
      
			  if(LEN( @newsentence + @item)<=@length )
			  begin
				set @newsentence += @item +' '
			  end
			  else
			  begin 
			  insert into @Table values (@index,rtrim(ltrim(@newsentence)))	 
			  set @index=@index+1  
			  set @newsentence =@item +' '
			  end	
			  
			 if( @c = (select @@CURSOR_ROWS))
			 begin
			 insert into @Table values (@index,rtrim(ltrim(@newsentence)))
			 set @index=@index+1	
			 end
	  	
	  		set @c =  @c + 1
          FETCH NEXT FROM db_cursor INTO @item
      END
    CLOSE db_cursor
    DEALLOCATE db_cursor
    
    return       
    end
