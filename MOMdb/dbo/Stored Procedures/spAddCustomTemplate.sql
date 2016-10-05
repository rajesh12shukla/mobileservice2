
CREATE PROC [dbo].[spAddCustomTemplate] @fdesc   VARCHAR(255),
                                      @remarks VARCHAR(8000),
                                      @Items   AS [dbo].[tblTypeCustomTempl] Readonly,
                                      @equipt  INT,
                                      @mode    INT,
                                      @ItemsDeleted   AS [dbo].[tblTypeCustomTemplDelet] Readonly,
                                      @CustomValues   AS [dbo].[tblTypeCustomValues] Readonly
AS
    BEGIN TRANSACTION

    IF( @mode = 0 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   ElevT
                        WHERE  fDesc = @fdesc)
            BEGIN
            SET @equipt = (SELECT ISNULL( Max(ID),0)+1 FROM ElevT)
                INSERT INTO ElevT
                            (ID,fDesc,Remarks,Count)
                VALUES      (@equipt,@fdesc,@remarks,0)
            END
          ELSE
            BEGIN
                RAISERROR ('Template name already exists, please use different name !',16,1)
                RETURN
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
                   
				  declare @id int
				  declare @ielev int
				  declare @ifdesc varchar(50)
				  declare @line int
				  declare @format varchar(50) 
			          
					DECLARE db_cursor CURSOR FOR SELECT 
													   Elev,
													   fDesc,
													   Line,													   
													   Format
													   FROM   @Items
					OPEN db_cursor
					FETCH NEXT FROM db_cursor INTO @ielev,@ifdesc,@line,@format

					WHILE @@FETCH_STATUS = 0
					  BEGIN
						  	set @id = (SELECT ISNULL( Max(ID),0)+1 FROM ElevTItem)	  
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
							  SELECT @id,
									 @equipt,
									 @ielev,
									 @id,
									 @ifdesc,
									 @line,
									 null,
									 @format	
									 
						   insert into tblCustomValues		
						   (
						   ElevT,
						   ItemID,
						   Line,
						   Value
						   )	
						   select
						   @equipt,
						   @id,
						   @line,
						   value
						   from @CustomValues where Line=@line
						   					 

						  IF @@ERROR <> 0
							 AND @@TRANCOUNT > 0
							BEGIN
								RAISERROR ('Error Occured',16,1)
								ROLLBACK TRANSACTION
								CLOSE db_cursor
								DEALLOCATE db_cursor
								RETURN
							END

						  FETCH NEXT FROM db_cursor INTO @ielev,@ifdesc,@line,@format
					  END
						CLOSE db_cursor
						DEALLOCATE db_cursor
          
      END
      
      
      
    ELSE IF( @mode = 1 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   ElevT
                        WHERE  fDesc = @fdesc
                               AND ID <> @equipt)
            BEGIN
                UPDATE ElevT
                SET    
						fDesc = @fdesc,
                       Remarks = @remarks
                WHERE  ID = @equipt
            END
          ELSE
            BEGIN
                RAISERROR ('Template name already exists, please use different name !',16,1)
                RETURN
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)
                ROLLBACK TRANSACTION
                RETURN
            END

					DECLARE db_cursor CURSOR FOR SELECT ID,					
													   Elev,
													   fDesc,
													   Line,													   
													   Format
													   FROM   @Items
					OPEN db_cursor
					FETCH NEXT FROM db_cursor INTO @id,@ielev,@ifdesc,@line,@format
					WHILE @@FETCH_STATUS = 0
					  BEGIN
					  
					  delete from tblCustomValues where ItemID in (select ID from @ItemsDeleted) 
					  delete from ElevTItem where CustomID in (select ID from @ItemsDeleted)
					  
					  if (@id=0)
					  begin
						  	set @id = (SELECT ISNULL( Max(ID),0)+1 FROM ElevTItem)	
						  	declare @Customid int 
							set @Customid = @id  
							INSERT INTO ElevTItem
								  (ID,
								   ElevT,
								   Elev,
								   CustomID,
								   fDesc,
								   Line,
								   Value,
								   Format,
								   fExists
								   )
							  SELECT @id,
									 @equipt,
									 @ielev,
									 @Customid,
									 @ifdesc,
									 @line,
									 null,
									 @format,
									 2
									 
							insert into tblCustomValues		
						   (
						   ElevT,
						   ItemID,
						   Line,
						   Value
						   )	
						   select
						   @equipt,
						   @id,
						   @line,
						   value
						   from @CustomValues where Line=@line
						   					 
							
							DECLARE db_cursor1 CURSOR FOR SELECT 
									 Elev from (select distinct Elev from ElevTItem where Elev<>0 AND ElevT=@equipt) as t
										OPEN db_cursor1
										FETCH NEXT FROM db_cursor1 INTO @ielev
										WHILE @@FETCH_STATUS = 0
										  BEGIN
												set @id = (SELECT ISNULL( Max(ID),0)+1 FROM ElevTItem)			 
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
												  SELECT @id,
														 @equipt,
														 @ielev,
														 @Customid,
														 @ifdesc,
														 @line,
														 null,
														 @format
													
										IF @@ERROR <> 0AND @@TRANCOUNT > 0
										BEGIN
											RAISERROR ('Error Occured',16,1)
											ROLLBACK TRANSACTION
											CLOSE db_cursor1
											DEALLOCATE db_cursor1
											RETURN
										END

									FETCH NEXT FROM db_cursor1 INTO @ielev
									END
									CLOSE db_cursor1
									DEALLOCATE db_cursor1
															
							end
							else
							begin
							update ElevTItem set
								   fDesc=@ifdesc,
								   Line=@line,								   
								   Format=@format,
								   fExists=1 
								   where ID=@id
								   
								   delete from tblCustomValues where ElevT=@equipt and ItemID=@id
									
									insert into tblCustomValues		
								   (
								   ElevT,
								   ItemID,
								   Line,
								   Value
								   )	
								   select
								   @equipt,
								   @id,
								   @line,
								   value
								   from @CustomValues where ItemID=@id
								   
								   
								   UPDATE ElevTItem SET 
									fDesc =  @ifdesc, 
									Line =@line,
									Format = @format 
									
									WHERE Elev <> 0 AND ElevT=@equipt AND CustomID = @id
															   					 
							end
									 
						  IF @@ERROR <> 0
							 AND @@TRANCOUNT > 0
							BEGIN
								RAISERROR ('Error Occured',16,1)
								ROLLBACK TRANSACTION
								CLOSE db_cursor
								DEALLOCATE db_cursor
								RETURN
							END

						  FETCH NEXT FROM db_cursor INTO @id,@ielev,@ifdesc,@line,@format
					  END
						CLOSE db_cursor
						DEALLOCATE db_cursor
      END
            
    ELSE IF( @mode = 2 )
      BEGIN
      
      if exists(select 1 from ElevTItem where ElevT=@equipt and Elev<>0)
      begin
       RAISERROR ('Template is in use!',16,1)
        RETURN
      end
      IF @@ERROR <> 0
         AND @@TRANCOUNT > 0
        BEGIN
            RAISERROR ('Error Occured',16,1)

            ROLLBACK TRANSACTION

            RETURN
        END
      
      
      DELETE FROM ElevT
      WHERE  ID = @equipt 

      IF @@ERROR <> 0
         AND @@TRANCOUNT > 0
        BEGIN
            RAISERROR ('Error Occured',16,1)

            ROLLBACK TRANSACTION

            RETURN
        END

      DELETE FROM ElevTItem
      WHERE  ElevT = @equipt and Elev=0

      IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
            
      delete from tblCustomValues where ElevT = @equipt      
            
      END
      
    COMMIT TRANSACTION 
