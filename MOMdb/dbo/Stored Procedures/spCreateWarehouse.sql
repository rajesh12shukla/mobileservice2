
CREATE PROCEDURE [dbo].[spCreateWarehouse]
			@ID varchar(5),
           @Name varchar(25),
           @Type int=null,
           @Location int =null,
           @Remarks varchar(800) =null,
           @Count int =null  

as
	begin
			DECLARE @success int
			set @success=0
			begin try
			if not exists(select 1 from Warehouse where Warehouse.ID=@ID)
				BEGIN
					INSERT INTO [dbo].[Warehouse]
				   ([ID]
				   ,[Name]
				   ,[Type]
				   ,[Location]
				   ,[Remarks]
				   ,[Count])
					 VALUES
				   (@ID,@Name,@Type,@Location,@Remarks,@Count)
			   END
			else
				begin
			declare @msg nvarchar
			set @msg='Warehouse '+@ID+' could not be created as it already exists.'
				RAISERROR (@msg, 16, 1)
				end
			end try

			begin catch
			IF @@ERROR >0
				 BEGIN  
				 set @success=-1
					--RAISERROR ('Error Occured', 16, 1)  
					
				 END
			end catch

		select @success

	end
