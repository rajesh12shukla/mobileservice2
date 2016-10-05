CREATE PROCEDURE [dbo].[spAddProject]
@job int,
@owner int,
@loc int,
@fdesc varchar(75),
@status smallint=null,
@type smallint,
@Remarks varchar(max), 
@ctype varchar(15),
@ProjCreationDate datetime,
@PO varchar(25),
@SO varchar(25),
@Certified smallint,
@Custom1 varchar(75),
@Custom2 varchar(75),
@Custom3 varchar(75),
@Custom4 varchar(75),
@Custom5 datetime=null,
@template int,
@RolName varchar(75),
@city varchar(50),
@state varchar(2),
@zip varchar(10),
@country varchar(50),
@phone varchar(28),
@cellular varchar(28),
@fax varchar(28),
@contact varchar(50),
@email varchar(50),
@rolRemarks varchar(max),
@rolType smallint,
@InvExp int,
@InvServ int,
@Wage int,
@GLInt int,
@jobtCType varchar(10),
@Post smallint,
@Charge smallint,
@JobClose smallint,
@fInt smallint,
@Items as tblTypeProjectItem readonly,
@TeamItems as tblTypeTeamItem readonly,
@BomItem AS tblTypeBomItem readonly,
@MilestonItem AS tblTypeMilestoneItem readonly, 
@CustomItem AS tblTypeCustomTabItem readonly,
@BillRate numeric(30,2) = null,
@RateOT numeric(30,2) = null,
@RateNT numeric(30,2) = null,
@RateDT numeric(30,2) = null,
@RateTravel numeric(30,2) = null,
@Mileage numeric(30,2) = null

as

declare @rolAddress varchar(255)
declare @website varchar(50)
select @owner=Owner from Loc where Loc = @loc
declare @rolid int

DECLARE @jobTItemId int
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
DECLARE @ProjAcquDate datetime
DECLARE @jtype smallint
DECLARE @MileName varchar(150)
DECLARE @RequiredBy datetime
DECLARE @LeadTime numeric(30,2)
DECLARE @OrgDep int
DECLARE @Amount numeric(30,2)
DECLARE @tblCustomFieldsId int
DECLARE @Value varchar(50)
DECLARE @tblTabID int
DECLARE @Label varchar(50)
DECLARE @TabLine smallint
DECLARE @Format smallint
DECLARE @UpdatedDate datetime
DECLARE @Username varchar(50)

--select 
--@project=e.Job, 
--@loc= e.LocID, 
--@owner=(select owner from loc where loc=e.locid)
--from Estimate e 
--where e.ID= @estimate

--if(@loc <> 0)
--begin
create table #tempRol
( RolID int )

	BEGIN TRANSACTION
	IF (@job = 0)
	BEGIN
				if(@RolName<>'')
				begin
					--INSERT INTO #tempRol
					exec @rolid = spAddRolDetails @RolName, @city, @state, @zip, @phone, @fax, @contact, @rolAddress, @email, @website, @country, @cellular, @rolRemarks, @rolType
					--SELECT TOP 1 @rolid=RolID FROM #tempRol
				end
				
				IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
					RETURN
					END

					INSERT INTO job
					(
					Loc,
					Owner,
					fDate,
					Status,
					Remarks,
					fDesc,
					Type,
					CType,
					PO,
					SO,
					Certified,
					Rev,Mat,Labor,Cost,Profit,Ratio,Reg,OT,DT,TT,Hour,BRev,BMat,BLabor,BCost,BProfit,BRatio,BHour,Comm,NT,Amount,
					Template,
					Custom21,
					Custom22,
					Custom23,
					Custom24,
					Custom25,
					ProjCreationDate, 
					Rol,
					LastUpdateDate,
					BillRate,
					RateOT,
					RateNT,
					RateDT,
					RateTravel,
					RateMileage
					)
					values
					(
					@loc,
					@owner,
					GETDATE(),
					@status,
					@Remarks,
					@fdesc,
					@type,
					@ctype,
					@PO,
					@SO,
					@Certified,
					0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,
					@template,
					@Custom1,
					@Custom2,
					@Custom3,
					@Custom4,
					@Custom5,
					@ProjCreationDate,
					@rolid,
					GETDATE(),
					@BillRate,
					@RateOT,
					@RateNT,
					@RateDT,
					@RateTravel,
					@Mileage
					)

					set @job=@@IDENTITY

					IF(@template<>0)
					BEGIN
						UPDATE [dbo].[JobT]
						   SET [InvExp] = @InvExp
							  ,[InvServ] = @InvServ
							  ,[Wage] = @Wage
							  ,[GLInt] = @GLInt
							  ,[CType] = @jobtCType
							  ,[Post] = @Post
							  ,[Charge] = @Charge
							  ,[fInt] = @fInt
							  ,[JobClose] = @JobClose
							 
						 WHERE ID = @template
					END

					IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END

					INSERT INTO Team (Line, JobID, Title, MomUserID, FirstName, LastName, Email, Mobile)
					select Line, @job, Title, MomUserID, FirstName, LastName, Email, Mobile from @TeamItems
					
					IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
					
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
					Labor
					)
					select JobT, @Job, Type, fDesc, Code, Actual, Budget, Line,[Percent], 0, 0, 0, 0, 0  
					from @Items 

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
			values(@template, @Job, 1, @jfDesc, @jCode, 0, @jBudget, @Line,0, 0, 0, 0, 0, 0, 0)
			SET @jobTItemId = SCOPE_IDENTITY()

			-- JobTItem.Type = 0 is revenue type
			-- JobTItem.Type = 1 is expense type

			IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
					RETURN
					
				END
				DECLARE @bitemVal int = 0
					If(@Bitem != '')
					BEGIN
						SET @bitemVal = CAST(@Bitem AS INT)
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
			values(@jobTItemId, @Btype, @bitemVal, @QtyReq, @UM, @ScrapFact, @BudgetUnit, @BudgetExt)
			 
		FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @jBudget, @Line, @Btype, @Bitem, @QtyReq, @UM, @ScrapFact, @BudgetUnit, @BudgetExt
		END  

		CLOSE db_cursor  
		DEALLOCATE db_cursor


		-- add milestones for project
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
							values(@template, @Job, @jType, @jfDesc, @jCode, 0, @jBudget, @Line,0, 0, 0, 0, 0, 0, 0)
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

	-- add custom details for project
	DECLARE db_cursor2 CURSOR FOR 

	SELECT [ID], [tblTabID], [Label], [Line], [Value], [Format], [UpdatedDate], [Username] FROM @CustomItem

	OPEN db_cursor2
	FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username

	WHILE @@FETCH_STATUS = 0
	BEGIN  	
	
			INSERT INTO [dbo].[tblCustomJob]
			   ([JobID]
			   ,[tblCustomFieldsID]
			   ,[Value]
			   ,[UpdatedDate]
			   ,[Username])
			 VALUES
				   (@Job
				   ,@tblCustomFieldsId
				   ,@Value
				   ,@UpdatedDate
				   ,@Username)

		FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username 
		END  

	CLOSE db_cursor2 
	DEALLOCATE db_cursor2


	END
	ELSE
	BEGIN
		
		SELECT @rolid = isnull(Rol,0) from Job where ID= @Job
		
		if(@rolid<>0)
			begin
					exec spUpdateRolDetails @rolid, @RolName, @city, @state, @zip, @phone, @fax, @contact, @rolAddress, @email, @website, @country, @cellular, @rolType

			end
			else
			begin
			if(@RolName<>'')
			begin

					--insert into #tempRol
					exec @rolid = spAddRolDetails @RolName, @city, @state, @zip, @phone, @fax, @contact, @rolAddress, @email, @website, @country, @cellular, @rolRemarks, @rolType
			END
					--SELECT TOP 1 @rolid=RolID FROM #tempRol
			END

		delete from Team where JobID=@Job
					
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
			END

		INSERT INTO Team (Line, JobID, Title, MomUserID, FirstName, LastName, Email, Mobile)
		select Line, JobID, Title, MomUserID, FirstName, LastName, Email, Mobile  from @TeamItems
					
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
			END

		UPDATE Job SET 
			Loc = @loc ,
			Owner = @owner,
			Remarks=@Remarks,
			fDesc=@fdesc,
			Template=@template,
			type=@type,
			status=@status,
			ctype=@ctype,
			PO=@PO,
			SO=@SO,
			Certified=@certified,
			ProjCreationDate=@ProjCreationDate,
			Custom21=@Custom1,
			Custom22=@Custom2,
			Custom23=@Custom3,
			Custom24=@Custom4,
			Custom25=@Custom5,
			Rol=@rolid,
			LastUpdateDate=Getdate(),
			BillRate=@BillRate,
			RateOT=@RateOT,
			RateNT=@RateNT,
			RateDT=@RateDT,
			RateTravel=@RateTravel,
			RateMileage=@Mileage
		WHERE ID= @Job

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
			END
	
		
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
			RAISERROR ('Error Occured', 16, 1)  
			ROLLBACK TRANSACTION    
			RETURN
			END

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
		Labor
		)
		select JobT, @Job, Type, fDesc, Code, Actual, Budget, Line,[Percent], 0, 0, 0, 0, 0  
		from @Items 

		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
			BEGIN  
				RAISERROR ('Error Occured', 16, 1)  
				ROLLBACK TRANSACTION    
			RETURN
			END

		IF(@template<>0)
		BEGIN
			UPDATE [dbo].[JobT]
				SET [InvExp] = @InvExp
					,[InvServ] = @InvServ
					,[Wage] = @Wage
					,[GLInt] = @GLInt
					,[CType] = @jobtCType
					,[Post] = @Post
					,[Charge] = @Charge
					,[fInt] = @fInt
					,[JobClose] = @JobClose
							 
				WHERE ID = @template
		END

		create table #tbljobitem
		(jobtitem int)
		
		insert into #tbljobitem
		select jobitem.ID from JobtItem jobitem inner join BOM b on b.JobTItemID = jobitem.ID
		where job = @job

		insert into #tbljobitem
		select jobitem.ID from JobtItem jobitem inner join Milestone m on m.JobTItemID = jobitem.ID
		where job = @job

		DELETE b FROM BOM b INNER JOIN JobTItem j ON b.JobTItemID = j.ID 
					WHERE j.Job = @job
		DELETE m FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID 
				WHERE j.Job = @job
		
		DELETE FROM JobTItem WHERE ID IN (select ID from #tbljobitem)		-- to delete only those jobitem which are linked with bom and milestone
													
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
			values(@template, @Job, 1, @jfDesc, @jCode, 0, @jBudget, @Line,0, 0, 0, 0, 0, 0, 0)
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

		DECLARE db_cursor1 CURSOR FOR 

		select jtype, fdesc, jCode, Line, MilesName,RequiredBy,LeadTime,Type, Amount from @MilestonItem 

		OPEN db_cursor1  
		FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName,@RequiredBy,@LeadTime,@OrgDep, @Amount
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
							values(@template, @Job, @jType, @jfDesc, @jCode, 0, @jBudget, @Line,0, 0, 0, 0, 0, 0, 0)
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
				
		FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName,@RequiredBy,@LeadTime,@OrgDep, @Amount
		END  

		CLOSE db_cursor1  
		DEALLOCATE db_cursor1

		delete from tblCustomJob where JobID = @job
		-- update custom details for project
		DECLARE db_cursor2 CURSOR FOR 

		SELECT [ID], [tblTabID], [Label], [Line], [Value], [Format], [UpdatedDate], [Username] FROM @CustomItem

		OPEN db_cursor2
		FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username

		WHILE @@FETCH_STATUS = 0
		BEGIN  	

			INSERT INTO [dbo].[tblCustomJob]
			   ([JobID]
			   ,[tblCustomFieldsID]
			   ,[Value]
			   ,[UpdatedDate]
			   ,[Username])
			 VALUES
				   (@Job
				   ,@tblCustomFieldsId
				   ,@Value
				   ,@UpdatedDate
				   ,@Username)

		FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username
		END  

		CLOSE db_cursor2 
		DEALLOCATE db_cursor2

	END
	

	COMMIT TRANSACTION		
return @job
