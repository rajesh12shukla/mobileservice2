CREATE PROCEDURE [dbo].[spAddProjectTemplate]
@jobT int,
@fdesc varchar(50),
@Type smallint,
@NRev smallint,
@NDed smallint,
@Count int,
@Remarks varchar(max), 
@InvExp int,
@InvServ int,
@Wage int,
@CType varchar(15),
@Status smallint = 0,
@Charge smallint,
@Post smallint,
@fInt smallint,
@GLInt smallint,
@JobClose smallint,
@tempRev varchar(150),
@RevRemarks varchar(max),
@alertType smallint,
@alertMgr bit,
@MilestoneMgr bit,
@BomItem AS tblTypeBomItem readonly,
@MilestonItem AS tblTypeMilestoneItem readonly,
@CustomTabItem AS tblTypeCustomTabItem readonly,
@CustomItem AS tblTypeCustomItem readonly

as
DECLARE @mid int
DECLARE @bid int
DECLARE @jobTItemId int
DECLARE @jtype smallint
DECLARE @jfDesc varchar(255)
DECLARE @jCode varchar(10)
DECLARE @jBudget numeric(30,2)
DECLARE @Line smallint
DECLARE @Btype smallint
DECLARE @Bitem int
DECLARE @QtyReq numeric(30,2)
DECLARE @UM varchar(50)
DECLARE @ScrapFact numeric(30,2)
DECLARE @BudgetUnit numeric(30,2)
DECLARE @BudgetExt numeric(30,2)
DECLARE @MileName varchar(150)
DECLARE @RequiredBy datetime
DECLARE @LeadTime numeric(30,2)
DECLARE @ProjAcquDate datetime
DECLARE @OrgDep int
DECLARE @Amount numeric(30,2)

DECLARE @tblCustomFieldsId int
DECLARE @tblTabID int
DECLARE @Label VARCHAR(50)
DECLARE @TabLine SMALLINT
DECLARE @Value VARCHAR(50)
DECLARE @Format smallint
DECLARE @CustomID int

BEGIN TRANSACTION
	
			
				INSERT INTO [dbo].[JobT]
							([fDesc]
							,[Type]
							,[NRev]
							,[NDed]
							,[Count]
							,[Remarks]
							,[InvExp]
							,[InvServ]
							,[Wage]
							,[CType]
							,[Status]
							,[Charge]
							,[Post]
							,[fInt]
							,[GLInt]
							,[JobClose]
							,[AlertType]
							,[AlertMgr]
							)
						VALUES
							(@fdesc
							,@Type
							,@NRev
							,@NDed
							,@Count
							,@Remarks
							,@InvExp
							,@InvServ
							,@Wage
							,@CType
							,@Status
							,@Charge
							,@Post
							,@fInt
							,@GLInt
							,@JobClose
							,@alertType
							,@alertMgr
							)

				SET @jobT=SCOPE_IDENTITY()

				IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
					ROLLBACK TRANSACTION    
					RETURN
					END

	DECLARE db_cursor CURSOR FOR 

	select fdesc, jCode, jBudget, Line, Btype, BItem, QtyReq, UM, ScrapFact, BudgetUnit, BudgetExt from @BomItem 

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @jBudget, @Line, @Btype, @Bitem, @QtyReq, @UM, @ScrapFact, @BudgetUnit, @BudgetExt

	WHILE @@FETCH_STATUS = 0
	BEGIN  					
						INSERT INTO JobTItem
						(
						JobT,
						Job,
						Type,
						fDesc,
						Code,
						Actual,
						Budget,
						Line,
						[Percent],
						Comm,
						Modifier,
						ETC,
						ETCMod,
						Labor, 
						Stored
						)
						values(@jobT, 0, 1, @jfDesc, @jCode, 0, @jBudget, @Line,0, 0, 0, 0, 0, 0, 0)
						SET @jobTItemId = SCOPE_IDENTITY()

						-- JobTItem.Type = 0 is revenue type
						-- JobTItem.Type = 1 is expense type

						IF @@ERROR <> 0 AND @@TRANCOUNT > 0
							BEGIN  
								RAISERROR ('Error Occured', 16, 1)  
									ROLLBACK TRANSACTION    
								RETURN
							END
						
						
						INSERT INTO [dbo].[BOM]
								([JobTItemID]
								,[Type]
								,[Item]
								,[QtyRequired]
								,[UM]
								,[ScrapFactor]
								,[BudgetUnit]
								,[BudgetExt])
						values(@jobTItemId, @Btype, @Bitem, @QtyReq, @UM, @ScrapFact, @BudgetUnit, @BudgetExt)



		
	FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @jBudget, @Line, @Btype, @Bitem, @QtyReq, @UM, @ScrapFact, @BudgetUnit, @BudgetExt
	END  

	CLOSE db_cursor  
	DEALLOCATE db_cursor


	IF @@ERROR <> 0 AND @@TRANCOUNT > 0
		BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
				ROLLBACK TRANSACTION    
			RETURN
		END
					  
			
	DECLARE db_cursor1 CURSOR FOR 

	select jtype, fdesc, jCode, Line, MilesName,RequiredBy,LeadTime,Type,Amount from @MilestonItem 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName,@RequiredBy,@LeadTime,@OrgDep,@Amount

																																																	WHILE @@FETCH_STATUS = 0
BEGIN  					
					INSERT INTO JobTItem
					(
					JobT,
					Job,
					Type,
					fDesc,
					Code,
					Actual,
					Budget,
					Line,
					[Percent],
					Comm,
					Modifier,
					ETC,
					ETCMod,
					Labor, 
					Stored
					)
					values(@jobT, 0, @jType, @jfDesc, @jCode, 0, @jBudget, @Line,0, 0, 0, 0, 0, 0, 0)
					SET @jobTItemId = SCOPE_IDENTITY()

					IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
							ROLLBACK TRANSACTION    
						RETURN
					END

					

					INSERT INTO [dbo].[Milestone]
						   ([JobTItemID]
						   ,[MilestoneName]
						   ,[RequiredBy]
						   ,[CreationDate]
						   ,[ProjAcquistDate]
						   ,[Type]
						   ,[Amount]
						  )
					 VALUES
						   (@jobTItemId
						   ,@MileName
						   ,@RequiredBy
						   ,GETDATE ()
						   ,@ProjAcquDate
						   ,@OrgDep
						   ,@Amount)
				
	FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName,@RequiredBy,@LeadTime,@OrgDep,@Amount
	END  

	CLOSE db_cursor1  
	DEALLOCATE db_cursor1

	--------------------------------------- insert custom template ---------------------------------------
	
	DECLARE db_cursor2 CURSOR FOR 

	SELECT [ID], [tblTabID], [Label], [Line], [Format] FROM @CustomTabItem

	OPEN db_cursor2  
	FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Format 

	WHILE @@FETCH_STATUS = 0
	BEGIN  		
		--SELECT @CustomID = (ISNULL(MAX(CustomID),0)+1) FROM tblCustomTab
		IF(@tblTabID = 0)
		BEGIN
			SET @tblTabID = NULL
		END

		INSERT INTO [dbo].[tblCustomFields] ([tblTabID], [Label], [Line], [Format])
		VALUES (@tblTabID, @Label, @TabLine, @Format)
		
		SET @tblCustomFieldsId=SCOPE_IDENTITY()

		IF(SELECT TOP 1  1 FROM @CustomItem WHERE Line = @TabLine) = 1
		BEGIN
		
			INSERT INTO [dbo].[tblCustom](tblCustomFieldsID, Line, Value)
			SELECT @tblCustomFieldsId, Line, Value FROM @CustomItem WHERE Line = @TabLine
		
		END

		INSERT INTO [dbo].[tblCustomJobT] ([JobTID],[tblCustomFieldsID])
		VALUES (@jobT ,@tblCustomFieldsId)
		
	FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Format 
	END  

	CLOSE db_cursor2  
	DEALLOCATE db_cursor2

	COMMIT TRANSACTION

select @jobT
