CREATE PROCEDURE [dbo].[Spaddcategory] @type    VARCHAR(30),
                               @remarks VARCHAR(8000),
                               @icon    IMAGE,
							   @chargeable bit,
							   @default smallint
AS
	IF not exists( select 1 from category where Type=@type )
	Begin
	
	update Category set ISDefault = 0  
	
			INSERT INTO category
						(type,
						 remarks,
						 icon,
						 Chargeable,
						 ISDefault)
			VALUES      (@type,
						 @remarks,
						 @icon,
						 @chargeable,
						 @default)
	End
	Else
	Begin
			RAISERROR ('Category already exists, please use different name !',16,1)
			RETURN
	End
