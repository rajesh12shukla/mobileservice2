CREATE PROCEDURE [dbo].[spCreateRecurringTickets] @RecurringTicket AS [dbo].[tblTypeRecurringTicket] Readonly,
                                                 @RemarksOpt      VARCHAR(255),
                                                 @JobRemarksOpt   INT,
                                                 @ProcessPeriod   VARCHAR(75)
AS
    DECLARE @LocID INT
    DECLARE @LocTag VARCHAR(50)
    DECLARE @LocAdd VARCHAR(100)
    DECLARE @City VARCHAR(50)
    DECLARE @State VARCHAR(2)
    DECLARE @Zip VARCHAR(100)
    DECLARE @Phone VARCHAR(28)
    DECLARE @Cell VARCHAR(50)
    DECLARE @Worker VARCHAR(50)
    DECLARE @CallDt DATETIME
    DECLARE @SchDt DATETIME
    DECLARE @Status SMALLINT
    DECLARE @Category VARCHAR(25)
    DECLARE @Unit INT
    DECLARE @custID INT
    DECLARE @JobRemarks VARCHAR(max)
    DECLARE @Remarks VARCHAR(255)
    DECLARE @job INT
    declare @ESt float
    declare @cat varchar(25)
    select top 1 @cat = isnull(Type, 'Recurring') from Category where ISDefault = 1
    set @cat = ISNULL(@cat, 'Recurring')
        
    DECLARE db_cursor CURSOR FOR
      SELECT *
      FROM   @RecurringTicket

    OPEN db_cursor

    FETCH NEXT FROM db_cursor INTO 
    @locid, 
    @LocAdd, 
    @City, 
    @State, 
    @Zip,     
    @CallDt, 
    @SchDt, 
    @Status, 
    @Worker, 
    @Category, 
    @Unit, 
    @custID, 
    @JobRemarks, 
    @Remarks, 
    @job,
    @est

    WHILE @@FETCH_STATUS = 0
      BEGIN
          --if (@Unit is not null)
          --begin
          ----update tblJoinElevJob set processdt = GETDATE() where job=@Job and Elev=@Unit
          --update Job set Custom18 =isnull( (SELECT SUBSTRING((SELECT distinct ',' +   items from dbo.split(isnull( Custom18,''),',') FOR XML PATH('')),2,200000)),'')+(','+CONVERT(varchar(50), @Unit)) where ID = @job
          --update Job set Custom19 =CONVERT(varchar(50),GETDATE(),121) where ID = @job
          --end
          --else
          --begin
          BEGIN TRANSACTION

          UPDATE Job
          SET    Custom19 = CONVERT(VARCHAR(50), Getdate(), 121),
                 Custom16 = @ProcessPeriod
          WHERE  ID = @job

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                CLOSE db_cursor

                DEALLOCATE db_cursor

                RETURN
            END

          --end
          IF( @JobRemarksOpt = 0 )
            BEGIN
                SET @JobRemarks=NULL
            END           
            
            declare @recurring datetime
            set @recurring =convert(datetime,CONVERT(date,@SchDt))
            
          EXEC spAddTicket
            @locid,
            NULL,
            @locAdd,
            @City,
            @State,
            @Zip,
            NULL,
            NULL,
            @Worker,
            @CallDt,
            @SchDt,
            @Status,
            NULL,
            NULL,
            NULL,
            @cat,
            @Unit,
            @RemarksOpt,
            NULL,
            @custID,
            @ESt,
            NULL,
            0,
            NULL,
            NULL,
            NULL,
            0.00,
            0.00,
            0.00,
            0.00,
            0.00,
            0.00,
            0,
            0,
            @JobRemarks,
            10,
            0,
            @job,
            '',
            '',
            '',
            '',
            '',
            0,
            0,
            '',
            1,
            0,
            0,
            0,
            0,
            0,
            1,
            '',
            0,
            0,
            0,
            '',
            1,
            null,
            null,
            '',
            '',
            '','','','','','','',0,null,null,null,null,'',@recurring
            

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                CLOSE db_cursor

                DEALLOCATE db_cursor

                RETURN
            END

          COMMIT TRANSACTION

          FETCH NEXT FROM db_cursor INTO @locid, @LocAdd, @City, @State, @Zip,  @CallDt, @SchDt, @Status,@Worker, @Category, @Unit, @custID, @JobRemarks, @Remarks, @job,@est
      END

    CLOSE db_cursor

    DEALLOCATE db_cursor
