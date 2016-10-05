
CREATE PROCEDURE [dbo].[spCreateInventoryDrawing]
			
           @InventoryDrawing_InvID int,
			@FileName nvarchar(100)= NULL,
			@FileServerPath varchar(75)= NULL,
			@FileBinary varbinary(max)= NULL
as
	begin
		
		
			insert into InventoryDrawing (InventoryDrawing_InvID,FileName,FileServerPath,FileBinary)
			values(@InventoryDrawing_InvID,@FileName,@FileServerPath,@FileBinary)
				
		

	end