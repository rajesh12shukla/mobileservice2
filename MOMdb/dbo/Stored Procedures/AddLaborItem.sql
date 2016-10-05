CREATE proc [dbo].[AddLaborItem]
@item varchar(100),
@amount numeric(30,2),
@ID int,
@mode smallint
as
    IF( @mode = 0 )
      BEGIN
      
        IF NOT EXISTS(SELECT 1 FROM tblEstimateLabour WHERE  Item = @item)
            BEGIN
                INSERT INTO tblEstimateLabour
                            ( Item ,Amount)
                VALUES      ( @item ,@amount)
            END
          ELSE
            BEGIN
                RAISERROR ('Name already exists, please use different name !',16,1)
                RETURN
            END
      END
            
    ELSE IF( @mode = 1 )
      BEGIN
          IF NOT EXISTS(SELECT 1 FROM   tblEstimateLabour WHERE  Item = @item AND ID <> @ID)
            BEGIN
                UPDATE tblEstimateLabour
                SET    
						Item = @item,Amount=@amount
                WHERE  ID = @ID
            END
          ELSE
            BEGIN
                RAISERROR ('Name already exists, please use different name !',16,1)
                RETURN
            END
          
      END
    ELSE IF( @mode = 2 )
      BEGIN
      
      DELETE FROM tblEstimateLabour
      WHERE  ID = @ID 

      END
