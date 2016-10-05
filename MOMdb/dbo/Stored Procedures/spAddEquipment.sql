
CREATE PROCEDURE [dbo].[spAddEquipment]
@Loc int,
@Unit varchar(20),
@fDesc varchar(50),
@Type varchar(20),
@Cat varchar(20),
@Manuf varchar(20),
@Serial varchar(50),
@State varchar(25),
@Since datetime,
@Last datetime,
@Price numeric(30,2),
@Status smallint,
@Remarks text,
@Install datetime,
@Category varchar(20),
@template int,
@items   AS [dbo].[tblTypeEquipTempItems] Readonly ,
@CustomItems   AS [dbo].[tblTypeCustomTempl] Readonly 
as

 BEGIN TRANSACTION
				
UPDATE ElevT SET [Count] = ([Count] + 1) WHERE ID = @template	

	insert into Elev
	(
	Loc,
	Owner,
	Unit,
	fDesc,
	Type,
	Cat,
	Manuf,
	Serial,
	State,
	Since,
	Last,
	Price,
	Status,
	Building,
	Remarks,
	fGroup,
	Template,
	InstallBy,
	Install,
	category,
	LastUpdateDate
	)
	values
	(
	@Loc,
	(select owner from loc where loc=@loc),
	@Unit,
	@fDesc,
	@Type,
	@Cat,
	@Manuf,
	@Serial,
	@State,
	@Since,
	@Last,
	@Price,
	@Status,
	'',
	@Remarks,
	'',
	@template,
	'',
	@Install,
	@Category,
	GETDATE()
	)

UPDATE Loc SET [Elevs] = ([Elevs] + 1) WHERE Loc = @Loc	
UPDATE [Owner] SET [Elevs] = ([Elevs] + 1) WHERE ID = (select [Owner] from Loc where Loc= @Loc)	
	
	
	IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

	declare @elev int
	SET @elev = Scope_identity()
	
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
			@elev,
			fDesc,
			Frequency,
	    	Lastdate,
			Line, 
			NextDateDue,
			Section
	from	@items
 
	  IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END
      
      
				  declare @itemid int
				  declare @ifdesc varchar(50)
				  declare @line int
				  declare @value varchar(50) 
				  declare @format varchar(50) 
    
					DECLARE db_cursor CURSOR FOR SELECT  ID,fDesc,Line,value,Format FROM   @CustomItems
					OPEN db_cursor
					FETCH NEXT FROM db_cursor INTO @itemid,@ifdesc,@line,@value,@format
					WHILE @@FETCH_STATUS = 0
					  BEGIN
					  
					INSERT INTO ElevTItem
						  (ID,
						   ElevT,
						   Elev,
						   CustomID,
						   fDesc,
						   Line,
						   Value,
						   Format
						   								   
						   )
					  values((SELECT ISNULL( Max(ID),0)+1 FROM ElevTItem),
							 @template,
							 @elev,
							 @itemid,
							 @ifdesc,
							 @line,
							 @value,
							 @format
							 )
					
					 IF @@ERROR <> 0
							 AND @@TRANCOUNT > 0
							BEGIN
								RAISERROR ('Error Occured',16,1)
								ROLLBACK TRANSACTION
								CLOSE db_cursor
								DEALLOCATE db_cursor
								RETURN
							END
					  
					  
					FETCH NEXT FROM db_cursor INTO @itemid,@ifdesc,@line,@value,@format
					END
					CLOSE db_cursor
					DEALLOCATE db_cursor				 

    COMMIT TRANSACTION
