
CREATE PROCEDURE [dbo].[spCreateInventoryManufacturerInformation]
			
           @InventoryManufacturerInformation_InvID [int] ,
			@MPN [varchar](75)= NULL,
			@ApprovedManufacturer[varchar](75)= NULL,
			@ApprovedVendor [varchar](75) =NULL

as
	begin
			DECLARE @success int
			set @success=0
		
			insert into InventoryManufacturerInformation (InventoryManufacturerInformation_InvID,MPN,ApprovedManufacturer,ApprovedVendor)
			values(@InventoryManufacturerInformation_InvID,@MPN,@ApprovedManufacturer,@ApprovedVendor)
				
			

		set @success= @@IDENTITY

		select @success

	end
