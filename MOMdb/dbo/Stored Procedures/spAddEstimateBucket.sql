
CREATE PROC [dbo].[spAddEstimateBucket] 
@name varchar(75), 
@desc varchar(250),
@bucketID int,
@mode smallint,
@Items   AS [dbo].[tblTypeEstimateBucketItems] Readonly
                                      
AS
    BEGIN TRANSACTION
                
    IF( @mode = 0 )
      BEGIN
     
          IF NOT EXISTS(SELECT 1 FROM tblEstimateBucket WHERE  Name = @name)
            BEGIN
                INSERT INTO tblEstimateBucket
                            ( Name ,[Desc])
                VALUES      ( @name ,@desc)
                
                set  @bucketID = SCOPE_IDENTITY()
            END
          ELSE
            BEGIN
                RAISERROR ('Name already exists, please use different name !',16,1)
                RETURN
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          INSERT INTO tblEstimateBucketItems
                      (BucketID, Line, Item,Vendor,Code,Unit,Cost,Measure)
			  SELECT  @bucketID,Line,Item,Vendor,Code,Unit,Cost,Measure
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
          IF NOT EXISTS(SELECT 1 FROM   tblEstimateBucket WHERE  Name = @name AND ID <> @bucketID)
            BEGIN
                UPDATE tblEstimateBucket
                SET    
						Name = @name,[Desc]=@desc
                WHERE  ID = @bucketID
            END
          ELSE
            BEGIN
                RAISERROR ('Name already exists, please use different name !',16,1)
                RETURN
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          DELETE FROM tblEstimateBucketItems
          WHERE  BucketID = @bucketID 

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

         INSERT INTO tblEstimateBucketItems
                      (BucketID, Line, Item,Vendor,Code,Unit,Cost,Measure)
			  SELECT  @bucketID,Line,Item,Vendor,Code,Unit,Cost,Measure
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
         
      
      DELETE FROM tblEstimateBucket
      WHERE  ID = @bucketID 

      IF @@ERROR <> 0
         AND @@TRANCOUNT > 0
        BEGIN
            RAISERROR ('Error Occured',16,1)

            ROLLBACK TRANSACTION

            RETURN
        END

      DELETE FROM tblEstimateBucketItems
      WHERE  BucketID = @bucketID 

      IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
                
      END
      
    COMMIT TRANSACTION
