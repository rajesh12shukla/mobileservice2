CREATE proc [dbo].[spAddEquipmentMCPItems]
 @items    AS [dbo].[tblTypeEquipTempItems] Readonly
as

 --DELETE FROM EquipTItem
 --   WHERE  Elev in (select Elev from @items)
    
 insert into EquipTItem
	 (
		Code,
		EquipT,
		Elev,
		fDesc,
		Frequency,
		Lastdate,
		Line ,
		NextDateDue,
		Section
	 )
	 select	code,
			EquipT,
			Elev,
			fDesc,
			Frequency,
	    	Lastdate,
			Line, 
			NextDateDue,
			Section
	from	@items