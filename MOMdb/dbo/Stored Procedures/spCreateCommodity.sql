CREATE PROCEDURE [dbo].[spCreateCommodity]

@Code nvarchar(15)= null ,
@Desc nvarchar(75)=null

as
	begin

		declare @id int

		insert into Commodity (CommodityCode,CommodityDesc,CommodityIsActive)
		values (@Code,@Desc,1)

		
		set @id=(select @@IDENTITY)

		select @id
	end
