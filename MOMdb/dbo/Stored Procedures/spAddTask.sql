
CREATE PROC [dbo].[spAddTask] @rol        INT,
                             @DateDue    DATETIME,
                             @TimeDue    DATETIME,
                             @subject    VARCHAR(50),
                             @desc       VARCHAR(max),
                             @Fuser      VARCHAR(50),
                             @fby        VARCHAR(50),
                             @contact    VARCHAR(50),
                             @Mode       SMALLINT,
                             @ID         INT,
                             @Status     SMALLINT,
                             @Resolution VARCHAR(max),
                             @UpdateUser VARCHAR(50)
AS
    DECLARE @taskID INT

    SELECT @taskID = Max([NewID]) + 1
    FROM   (SELECT Isnull(Max(ToDo.ID), 0) AS [NewID]
            FROM   ToDo
            UNION ALL
            SELECT Isnull(Max(done.ID), 0) AS [NewID]
            FROM   done) A

    IF( @Mode = 0 )
      BEGIN
      
      if exists (select 1 from ToDo where Subject=@subject and Rol=@rol union select 1 from Done where Subject=@subject and Rol=@rol)
      begin
       raiserror('Task with this subject already exists for the Contact.',16,1)
       return
      end
      
          IF( @Status = 0 )
            BEGIN
                INSERT INTO ToDo
                            (ID,
                             Type,
                             Rol,
                             fDate,
                             fTime,
                             DateDue,
                             TimeDue,
                             Subject,
                             Remarks,
                             Keyword,
                             Level,
                             fUser,
                             fBy,
                             Duration,
                             Contact,
                             Source,
                             CreateDate,
                             CreatedBy,
                             LastUpdateDate,
                             LastUpdatedBy)
                VALUES      ( @taskID,
                              0,
                              @rol,
                              Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                              Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                              + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                              @DateDue,
                              @TimeDue,
                              @subject,
                              @desc,
                              'To Do',
                              1,
                              @Fuser,
                              @fby,
                              0.00,
                              @contact,
                              '',
                              Getdate(),
                              @UpdateUser ,
                              Getdate(),
                              @UpdateUser)
            END
          ELSE
            BEGIN
                INSERT INTO Done
                            (ID,
                             Type,
                             Rol,
                             fDate,
                             fTime,
                             Datedone,
                             Timedone,
                             Subject,
                             Remarks,
                             Keyword,
                             fUser,
                             fBy,
                             Duration,
                             Contact,
                             Source,
                             Result,
                             CreateDate,
                             CreatedBy,
                             LastUpdateDate,
                             LastUpdatedBy)
                VALUES      ( @taskID,
                              0,
                              @rol,
                              Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                              Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                              + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                              @DateDue,
                              @TimeDue,
                              @subject,
                              @desc,
                              'To Do',
                              @Fuser,
                              @fby,
                              0.00,
                              @contact,
                              '',
                              @Resolution,
                              Getdate(),
                              @UpdateUser,
                              Getdate(),
                              @UpdateUser)
            END
            
      END
    ELSE IF( @Mode = 1 )
      BEGIN
      
     
      
          IF( @Status = 0 )
            BEGIN
            
				if exists (select 1 from ToDo where Subject=@subject and Rol=@rol and ID<>@ID union select 1 from Done where Subject=@subject and Rol=@rol and ID<>@ID)
				  begin
				   raiserror('Task with this subject already exists for the Contact.',16,1)
				   return
				  end
            
                UPDATE ToDo
                SET    Rol = @rol,
                       --fDate = Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                       --fTime = Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                       --        + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                       DateDue = @DateDue,
                       TimeDue = @TimeDue,
                       Subject = @subject,
                       Remarks = @desc,
                       fUser = @Fuser,
                       LastUpdateDate = Getdate(),
                       LastUpdatedBy = @UpdateUser
                WHERE  ID = @ID
            END
          ELSE
            BEGIN
                IF EXISTS (SELECT 1
                           FROM   done
                           WHERE  ID = @ID)
                  BEGIN
                  
					if exists (select 1 from ToDo where Subject=@subject and Rol=@rol and ID<>@ID 
					union select 1 from Done where Subject=@subject and Rol=@rol and ID<>@ID)
					  begin
					   raiserror('Task with this subject already exists for the Contact.',16,1)
					   return
					  end
					  
                      UPDATE done
                      SET    Rol = @rol,
                             --fDate = Dateadd(dd, 0, Datediff(dd, 0, Getdate())),
                             --fTime = Cast(Cast('01/01/1900' AS DATE) AS DATETIME)
                             --        + Cast(CONVERT(TIME, Getdate()) AS DATETIME),
                             Datedone = @DateDue,
                             Timedone = @TimeDue,
                             Subject = @subject,
                             Remarks = @desc,
                             fUser = @Fuser,
                             result = @Resolution,
                             LastUpdateDate = Getdate(),
                             LastUpdatedBy = @UpdateUser
                      WHERE  ID = @ID
                  END
                ELSE
                  BEGIN
                  
                  BEGIN TRANSACTION
                      
                     if exists (select 1 from ToDo where Subject=@subject and Rol=@rol and ID<>@ID union
                     select 1 from Done where Subject=@subject and Rol=@rol)
					  begin
					   raiserror('Task with this subject already exists for the Contact.',16,1)
					   ROLLBACK TRANSACTION   
					   return
					  end
					  
					  IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
                  
                      INSERT INTO done
                                  (ID,
                                   Type,
                                   Rol,
                                   fDate,
                                   fTime,
                                   Datedone,
                                   Timedone,
                                   Subject,
                                   Remarks,
                                   Keyword,
                                   fUser,
                                   fBy,
                                   Duration,
                                   Contact,
                                   Source,
                                   Result,
                                   CreateDate,
                                   CreatedBy,
                                   LastUpdateDate,
                                   LastUpdatedBy)
                      VALUES      ( @ID,
                                    0,
                                    @rol,
                                    (SELECT fDate
                                     FROM   todo
                                     WHERE  id = @id),
                                    (SELECT fTime
                                     FROM   todo
                                     WHERE  id = @id),
                                    @DateDue,
                                    @TimeDue,
                                    @subject,
                                    @desc,
                                    'To Do',
                                    @Fuser,
                                    @fby,
                                    0.00,
                                    @contact,
                                    '',
                                    @Resolution,
                                    (SELECT createdate
                                     FROM   todo
                                     WHERE  id = @id),
                                    (SELECT createdby
                                     FROM   todo
                                     WHERE  id = @id),
                                    Getdate(),
                                    @UpdateUser
                                     )
                      
                       IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
					
					 DELETE FROM ToDo WHERE  ID = @ID
                                           
                     IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
					
					COMMIT TRANSACTION
					 
                  END
            END
      END 
