
CREATE PROC [dbo].[spAddEstimateTemplate] @name varchar(75),
										  @fdesc   VARCHAR(255),
										  @remarks VARCHAR(8000),
										  @template  INT,
										  @mode    INT,
										  @Items   AS [dbo].[tblTypeEstimateItems] Readonly,
										  @LaborItems   AS [dbo].[tblTypeJoinLaborTemplate] Readonly
                                      
AS
    BEGIN TRANSACTION
                
    IF( @mode = 0 )
      BEGIN
      
      --SELECT @template = isnull(Max(ID) ,0) + 1 FROM Estimate
      
          IF NOT EXISTS(SELECT 1 FROM   Estimate WHERE  Name = @name)
            BEGIN
                INSERT INTO Estimate
                            ( Name,fdesc,Remarks,EstTemplate )
                VALUES      ( @name,@fdesc,@remarks,1 )
                
                set  @template = SCOPE_IDENTITY()
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

          INSERT INTO EstimateI
                      (Estimate,Line,[fDesc],Quan,
                       Cost,Price,[Hours],Rate,Labor,Amount,STax,Code,Vendor,Currency,Measure)
			  SELECT @template,Line,fDesc,Quan,Cost,Price,0,0,0,Price,STax,Code,Vendor,ltrim(rtrim(Currency)),Measure
			  FROM   @items

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
            
            Insert into tblJoinLaborTemplate (Line, LabourID, TemplateID, Amount)
            select Line, LabourID, @template, Amount from @LaborItems
            
          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END  
      END
      
      
      
            
    ELSE IF( @mode = 1 )
      BEGIN
          IF NOT EXISTS(SELECT 1 FROM   Estimate WHERE  Name = @name AND ID <> @template)
            BEGIN
                UPDATE Estimate
                SET    
						Name = @name,fdesc=@fdesc,Remarks=@remarks
                WHERE  ID = @template
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

          DELETE FROM EstimateI
          WHERE  Estimate = @template 

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          INSERT INTO EstimateI
                      (Estimate,Line,[fDesc],Quan,
                       Cost,Price,[Hours],Rate,Labor,Amount,STax,Code,Vendor,Currency,Measure)
			  SELECT @template,Line,fDesc,Quan,Cost,Price,0,0,0,Price,STax,Code,Vendor,ltrim(rtrim(Currency)),Measure
			  FROM   @items

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
            
            delete from tblJoinLaborTemplate where TemplateID=@template
            
            IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
            
             Insert into tblJoinLaborTemplate (Line, LabourID, TemplateID, Amount)
            select Line, LabourID, @template, Amount from @LaborItems
            
          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END  
      END
    ELSE IF( @mode = 2 )
      BEGIN
      
      --if exists(select 1 from Estimate where EquipT=@equipt and Elev<>0)
      --begin
      -- RAISERROR ('Template is in use!',16,1)
      --  RETURN
      --end
      --IF @@ERROR <> 0
      --   AND @@TRANCOUNT > 0
      --  BEGIN
      --      RAISERROR ('Error Occured',16,1)

      --      ROLLBACK TRANSACTION

      --      RETURN
      --  END
      
      
      DELETE FROM Estimate
      WHERE  ID = @template 

      IF @@ERROR <> 0
         AND @@TRANCOUNT > 0
        BEGIN
            RAISERROR ('Error Occured',16,1)

            ROLLBACK TRANSACTION

            RETURN
        END

      DELETE FROM EstimateI
      WHERE  Estimate = @template 

      IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      
      DELETE FROM tblJoinLaborTemplate
      WHERE  TemplateID = @template 

      IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      
      END
      
    COMMIT TRANSACTION
