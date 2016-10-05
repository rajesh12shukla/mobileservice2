
CREATE PROC [dbo].[spAddEquipTemplate] @fdesc   VARCHAR(255),
                                      @remarks VARCHAR(8000),
                                      @Items   AS [dbo].[tblTypeEquipTempItems] Readonly,
                                      @equipt  INT,
                                      @mode    INT
                                      
AS
    BEGIN TRANSACTION

    IF( @mode = 0 )
      BEGIN
          --DECLARE @equipt INT
          IF NOT EXISTS(SELECT 1
                        FROM   EquipTemp
                        WHERE  fdesc = @fdesc)
            BEGIN
                INSERT INTO EquipTemp
                            (
                            fdesc,
                             Remarks)
                VALUES      ( 
								@fdesc,
                              @remarks )
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

          SET @equipt = Scope_identity()

          INSERT INTO EquipTItem
                      (Code,
                      [EquipT],
                       [Elev],
                       [fDesc],
                       [Line],
                       --[Lastdate] ,
                       --[NextDateDue] ,
                       [Frequency],
                       Section)
          SELECT code,
          @equipt,
                 Elev,
                 fdesc,
                 line,
                 --lastdate,NextDateDue,
                 Frequency,
                 Section
          FROM   @items

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
          IF NOT EXISTS(SELECT 1
                        FROM   EquipTemp
                        WHERE  fdesc = @fdesc
                               AND ID <> @equipt)
            BEGIN
                UPDATE EquipTemp
                SET    
						fdesc = @fdesc,
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

          DELETE FROM EquipTItem
          WHERE  EquipT = @equipt and Elev=0

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          INSERT INTO EquipTItem
                      (Code,
                      [EquipT],
                       [Elev],
                       [fDesc],
                       [Line],
                       --[Lastdate] ,
                       --[NextDateDue] ,
                       [Frequency],
                       Section)
          SELECT code,
          @equipt,
                 Elev,
                 fdesc,
                 line,
                 --lastdate,NextDateDue,
                 Frequency,
                 Section
          FROM   @items

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
      
      if exists(select 1 from EquipTItem where EquipT=@equipt and Elev<>0)
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
      
      
      DELETE FROM EquipTemp
      WHERE  ID = @equipt 

      IF @@ERROR <> 0
         AND @@TRANCOUNT > 0
        BEGIN
            RAISERROR ('Error Occured',16,1)

            ROLLBACK TRANSACTION

            RETURN
        END

      DELETE FROM EquipTItem
      WHERE  EquipT = @equipt and Elev=0

      IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      
      
      END
      
      

    COMMIT TRANSACTION
