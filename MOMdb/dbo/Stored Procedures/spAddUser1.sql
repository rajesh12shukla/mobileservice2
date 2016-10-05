
CREATE PROCEDURE [dbo].[spAddUser1] @UserName        NVARCHAR(50),
                                  @Password        NVARCHAR(50),
                                  @PDA             TINYINT,
                                  @Field           SMALLINT,
                                  @status          SMALLINT,
                                  @FName           VARCHAR(15),
                                  @MName           VARCHAR(15),
                                  @LName           VARCHAR(25),
                                  @Address         VARCHAR(8000),
                                  @City            VARCHAR(50),
                                  @State           VARCHAR(2),
                                  @Zip             VARCHAR(10),
                                  @Tel             VARCHAR(22),
                                  @Cell            VARCHAR(22),
                                  @Email           VARCHAR(50),
                                  @DtHired         DATETIME,
                                  @DtFired         DATETIME,
                                  @CreateTicket    CHAR(1),
                                  @WorkDate        CHAR(1),
                                  @LocationRemarks CHAR(1),
                                  @ServiceHist     CHAR(1),
                                  @PurchaseOrd     CHAR(1),
                                  @Expenses        CHAR(1),
                                  @ProgFunctions   CHAR(1),
                                  @AccessUser      CHAR(1),
                                  @Remarks         VARCHAR(8000),
                                  @Mapping         INT,
                                  @Schedule        INT,
                                  @DeviceID        VARCHAR(100),
                                  @Pager           VARCHAR(100),
                                  @Super           VARCHAR(50),
                                  @salesp          INT,
                                  @str             NVARCHAR(400),
                                  @userlicID       INT,
                                  @Lang            VARCHAR(25),
                                  @MerchantInfoId  INT,
                                  @DefaultWorker   SMALLINT,
                                  @Dispatch        CHAR(1),
                                  @SalesMgr        SMALLINT,
                                  @MassReview      SMALLINT,
                                  @MSMUser         VARCHAR(50),
                                  @MSMPass         VARCHAR(50),
								  @InServer varchar(100),
									@InServerType varchar(10),
									@InUsername varchar(100),
									@InPassword varchar(50),
									@InPort int,
									@OutServer varchar(100),
									@OutUsername varchar(100),
									@OutPassword varchar(50),
									@OutPort int,
									@SSL bit,
									@EmailAccount int,
									@HourlyRate numeric(30,2),
									@EmployeeMainten smallint,
									@TimestamFixed smallint,
									@PayMethod smallint,
									@PHours numeric(30,2),
									@Salary numeric(30,2),
									@Department varchar(100),
									@Ref varchar(15),
									@PayPeriod smallint,
									@mileagerate numeric(30,2),
									@addequip smallint,
									@editequip smallint,
									
									@FChart smallint,
									@addFChart smallint,
									@editFChart smallint,
									@viewFChart smallint,

									@FGLAdj smallint,
									@addFGLAdj smallint,
									@editFGLAdj smallint,
									@viewFGLAdj smallint,

									@FDeposit smallint,
									@AddDeposit smallint,
									@EditDeposit smallint,
									@ViewDeposit smallint,

									@FCustomerPayment smallint,
									@AddCustomerPayment smallint,
									@EditCustomerPayment smallint,
									@ViewCustomerPayment smallint,
									
									@FStatement smallint,
									@StartDate datetime,
									@EndDate datetime
AS
    DECLARE @Rol INT
    DECLARE @work INT
    DECLARE @Ticket VARCHAR(10)
    DECLARE @empid INT
    DECLARE @userid INT
    DECLARE @sales VARCHAR(10)
    DECLARE @Employee VARCHAR(10)
    DECLARE @TC VARCHAR(10)
    DECLARE @Elevator VARCHAR(10)
	DECLARE @Chart VARCHAR(10) = 'NNNNNN'
	DECLARE @GLAdj VARCHAR(10) = 'NNNNNN'
	DECLARE @Deposit VARCHAR(10) = 'NNNNNN'
	DECLARE @CustomerPayment VARCHAR(10) = 'NNNNNN'
	DECLARE @FinanceState VARCHAR(10) = 'NNNNNN'
	--DECLARE @viewGLAdj VARCHAR(10)
	--DECLARE @addGLAdj VARCHAR(10)
	--DECLARE @editGLAdj VARCHAR(10)
	
	--DECLARE @viewChart VARCHAR(10)
	--DECLARE @addChart VARCHAR(10)
	--DECLARE @editChart VARCHAR(10)

	IF( @FStatement = 1 )
	  BEGIN
		SET @FinanceState = SUBSTRING(@FinanceState, 1, 5) + 'Y'
	  END
	ELSE
      BEGIN
        SET @FinanceState = SUBSTRING(@FinanceState, 1, 5) + 'N'
      END

	IF( @FChart = 1 )
      BEGIN
          SET @Chart='YYYYYY'
      END
    ELSE
      BEGIN
          SET @Chart='NNNNNN'
      END

	IF(@addFChart = 1)
	BEGIN
		SET @Chart= 'Y'+ SUBSTRING(@Chart, 2, 5)
	END

	IF(@editFChart = 1)
	BEGIN
		SET @Chart= SUBSTRING(@Chart, 1, 1) +'Y'+ SUBSTRING(@Chart, 3, 4)
	END

	IF(@viewFChart = 1)
	BEGIN
		SET @Chart= SUBSTRING(@Chart, 1, 3) +'Y'+ SUBSTRING(@Chart, 5, 2)
	END

	IF( @FGLAdj = 1 )
      BEGIN
          SET @GLAdj='YYYYYY'
      END
    ELSE
      BEGIN
          SET @GLAdj='NNNNNN'
      END
	
	IF(@addFGLAdj = 1)
	BEGIN
		SET @GLAdj= 'Y'+ SUBSTRING(@GLAdj, 2, 5)
	END

	IF(@editFGLAdj = 1)
	BEGIN
		SET @GLAdj= SUBSTRING(@GLAdj, 1, 1) +'Y'+ SUBSTRING(@GLAdj, 3, 4)
	END

	IF(@viewFGLAdj = 1)
	BEGIN
		SET @GLAdj= SUBSTRING(@GLAdj, 1, 3) +'Y'+ SUBSTRING(@GLAdj, 5, 2)
	END

	IF( @FDeposit = 1 )
      BEGIN
          SET @Deposit='YYYYYY'
      END
    ELSE
      BEGIN
          SET @Deposit='NNNNNN'
      END

	IF(@AddDeposit = 1)
	BEGIN
		SET @Deposit= 'Y'+ SUBSTRING(@Deposit, 2, 5)
	END

	IF(@EditDeposit = 1)
	BEGIN
		SET @Deposit= SUBSTRING(@Deposit, 1, 1) +'Y'+ SUBSTRING(@Deposit, 3, 4)
	END

	IF(@ViewDeposit = 1)
	BEGIN
		SET @Deposit= SUBSTRING(@Deposit, 1, 3) +'Y'+ SUBSTRING(@Deposit, 5, 2)
	END

	IF( @FCustomerPayment = 1 )
      BEGIN
          SET @CustomerPayment='YYYYYY'
      END
    ELSE
      BEGIN
          SET @CustomerPayment='NNNNNN'
      END

	IF(@AddCustomerPayment = 1)
	BEGIN
		SET @CustomerPayment= 'Y'+ SUBSTRING(@CustomerPayment, 2, 5)
	END
	IF(@EditCustomerPayment = 1)
	BEGIN
		SET @CustomerPayment= SUBSTRING(@CustomerPayment, 1, 1) +'Y'+ SUBSTRING(@CustomerPayment, 3, 4)
	END
	IF(@ViewCustomerPayment = 1)
	BEGIN
		SET @CustomerPayment= SUBSTRING(@CustomerPayment, 1, 3) +'Y'+ SUBSTRING(@CustomerPayment, 5, 2)
	END

	IF( @addequip = 1 )
      BEGIN
          SET @Elevator='YNNNNN'
      END
    ELSE
      BEGIN
          SET @Elevator='NNNNNN'
      END
      
      IF( @editequip = 1 )
      BEGIN
          SET @Elevator= SUBSTRING( @Elevator,1,1) +'YNNNN'
      END
    ELSE
      BEGIN
          SET @Elevator= SUBSTRING( @Elevator,1,1) +'NNNNN'
      END

    IF( @SalesMgr = 1 )
      BEGIN
          SET @sales='YNNNNN'
      END
    ELSE
      BEGIN
          SET @sales='NNNNNN'
      END

    IF( @Schedule = 1 )
      BEGIN
          SET @Ticket='YYYYYY'
      END
    ELSE
      BEGIN
          SET @Ticket='NYYYYY'
      END

    IF( @Mapping = 1 )
      BEGIN
          SET @Ticket= Substring(@Ticket, 1, 1) + 'YYYYY';
      END
    ELSE
      BEGIN
          SET @Ticket= Substring(@Ticket, 1, 1) + 'YYNYY';
      END
      
       IF( @EmployeeMainten = 1 )
      BEGIN
          SET @Employee='NNNYNN'
      END
    ELSE
      BEGIN
          SET @Employee='NNNNNN'
      END
      
      IF( @TimestamFixed = 1 )
      BEGIN
          SET @TC='NYNNNN'
      END
    ELSE
      BEGIN
          SET @TC='NNNNNN'
      END

    BEGIN TRANSACTION

    IF( @Field <> 2 )
      BEGIN
          IF NOT EXISTS (SELECT 1
                         FROM   tblUser
                         WHERE  fUser = @UserName
                         --WHERE  msmuser = @MSMUser
                         
                         UNION
                                
                            SELECT 1
                           FROM   Owner
                           WHERE  fLogin = @UserName
                           --WHERE  msmuser = @MSMUser
                                                            
                           
                           UNION
                           SELECT 1
                           FROM   tblLocationRole
                           WHERE  Username = @UserName    
                         
                         )
            BEGIN
           
                      IF( @DefaultWorker = 1 )
                        BEGIN
                            UPDATE tbluser
                            SET    defaultworker = 0
                        END

                      INSERT INTO tblUser
                                  (fUser,
                                   Password,
                                   PDA,
                                   Status,
                                   MassResolvePDATickets,
                                   ListsAdmin,
                                   Dispatch,
                                   Location,
                                   PO,
                                   Control,
                                   UserS,
                                   UserType,
                                   Remarks,
                                   Ticket,
                                   Lang,
                                   MerchantInfoID,
                                   LastUpdateDate,
                                   DefaultWorker,
                                   Sales,
                                   MassReview,
                                   EmailAccount ,
                                   Employee,
                                   TC,
                                   Elevator,
								   Chart,
								   GLAdj,
								   Deposit,
								   CustomerPayment,
								   Financial,
								   fStart,
								   fEnd
                                   --,
                                   --msmuser,
                                   --msmpass
                                   )
                      VALUES      ( @UserName,
                                    @Password,
                                    @PDA,
                                    @status,
                                    0,
                                    0,
                                    @CreateTicket + @WorkDate + 'Y' + @ServiceHist + 'Y'
                                    + @Dispatch,
                                    'YYY' + @LocationRemarks + 'YY',
                                    @PurchaseOrd + @Expenses + 'YYYY',
                                    @ProgFunctions + 'YYYYY',
                                    @AccessUser + 'YYYYY',
                                    @Field,
                                    @Remarks,
                                    @Ticket,
                                    @Lang,
                                    @MerchantInfoId,
                                    Getdate(),
                                    @DefaultWorker,
                                    @sales,
                                    @MassReview,
                                    @EmailAccount ,
                                    @Employee,
                                    @TC,
                                    @Elevator,
									@Chart,
									@GLAdj,
									@Deposit,
									@CustomerPayment,
									@FinanceState,
									@StartDate,
									@EndDate
                                    --,
                                    --@MSMUser,
                                    --@MSMPass 
                                    )

                      SET @userid=Scope_identity()
               
            END
          ELSE
            BEGIN
                RAISERROR ('Username already exists, please use different username!',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    INSERT INTO Rol
                (Name,
                 City,
                 State,
                 Zip,
                 Phone,
                 Address,
                 EMail,
                 Cellular,
                 GeoLock,
                 Remarks)
    VALUES      ( @FName + ', ' + @LName,
                  @City,
                  @State,
                  @Zip,
                  @Tel,
                  @Address,
                  @Email,
                  @Cell,
                  0,
                  @Remarks )

    SET @Rol=Scope_identity()

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    --IF( @Field = 2 )
    --  BEGIN
    --      IF NOT EXISTS (
    --      SELECT 1
    --                     FROM   Owner
    --                     WHERE  fLogin = @UserName
    --                     UNION
    --                     SELECT 1
    --                     FROM   tblUser
    --                     WHERE  fUser = @UserName
    --                     )
    --        BEGIN
    --            INSERT INTO Owner
    --                        (fLogin,
    --                         Password,
    --                         Status,
    --                         TicketO,
    --                         TicketD,
    --                         Internet,
    --                         Rol)
    --            VALUES      ( @UserName,
    --                          @Password,
    --                          @status,
    --                          @Schedule,
    --                          @Mapping,
    --                          1,
    --                          @Rol )
    --        END
    --      ELSE
    --        BEGIN
    --            RAISERROR ('Username already exixts, please use different username!',16,1)
    --            ROLLBACK TRANSACTION
    --            RETURN
    --        END
    --  END
    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    IF( @Field = 1 )
      BEGIN
          INSERT INTO tblWork
                      (fDesc,
                       Type,
                       Status,
                       GeoLock,
                       Super,
                       DBoard,
                       HourlyRate)
          VALUES      ( @UserName,
                        0,
                        @status,
                        0,
                        @Super,
                        @Schedule,
                        @HourlyRate )

          SET @work=Scope_identity()

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          INSERT INTO Route
                      (Name,
                       Mech,
                       Loc,
                       Elev,
                       Hour,
                       Amount,
                       Symbol,
                       EN)
          VALUES      ( @UserName,
                        @work,
                        0,
                        0,
                        0,
                        0,
                        1,
                        1 )

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          EXEC Spcreatepda_userid
            @work

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      END
    ELSE
      BEGIN
          SET @work=NULL
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    IF( @Field <> 2 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   emp
                        WHERE  DeviceID = @DeviceID
                               AND @DeviceID <> '')
            BEGIN
                INSERT INTO Emp
                            (Field,
                             Status,
                             fFirst,
                             Middle,
                             Last,
                             DHired,
                             DFired,
                             CallSign,
                             Rol,
                             fWork,
                             Sales,
                             InUse,
                             Name,
                             DeviceID,
                             Pager,
                             PMethod,
                             PHour,
                             Salary,
                             PFixed,
                             Ref,
                             PayPeriod,
                             MileageRate)
                VALUES      ( @Field,
                              @status,
                              @FName,
                              @MName,
                              @LName,
                              @DtHired,
                              @DtFired,
                              @UserName,
                              @Rol,
                              @work,
                              @salesp,
                              0,
                              @FName + ', ' + @LName,
                              @DeviceID,
                              @Pager,
                              case @PayMethod 
                              when 2 then 1
                              else @PayMethod
                              end,
                              case @PayMethod 
                              when 1 then 0
                              else @PHours end,
                              @Salary,
                              case @PayMethod
                              when 2 then 0
                              else 1 end,
                              @Ref,
                              @PayPeriod,
                              @mileagerate
                               )

                SET @empid=Scope_identity()
                
                insert into tblJoinEmpDepartment (Emp, Department) select @empid, * from dbo.Split(@Department,',')
            END
          ELSE
            BEGIN
                RAISERROR ('Device ID already exixts, please use different Device ID!',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF( @salesp = 1 )
            BEGIN
                INSERT INTO Terr
                            (Name,
                             SMan,
                             SDesc,
                             Count,
                             Symbol,
                             EN)
                VALUES      ( @UserName,
                              @empid,
                              @LName + ', ' + @FName,
                              0,
                              1,
                              1 )
                              
                 IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
                  IF( @EmailAccount = 1 )
				  BEGIN
                 exec spAddEmailAccount @InServer,@InServerType,@InUsername,@InPassword,@InPort,
										@OutServer,@OutUsername,@OutPassword,@OutPort, @SSL,
										@UserID
				END
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      END

    IF ( @DefaultWorker = 1 )
      BEGIN
          UPDATE Loc
          SET    Route = (SELECT TOP 1 id
                          FROM   route
                          WHERE  Name = @UserName)
          WHERE  Route IS NULL

          UPDATE Loc
          SET    Terr = (SELECT TOP 1 id
                         FROM   Terr
                         WHERE  Name = @UserName)
          WHERE  Terr IS NULL
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END



    --if @userlicID = 0
    --begin
    --insert into MSM2_Admin.dbo.tblUserAuth
    --(
    --DBname,
    ----UserID,
    --str,
    --used,
    --dateupdate
    --)
    --values
    --(
    --(SELECT DB_NAME()),
    ----@userid,
    --@str,
    --1,
    --GETDATE()
    --)
    --set @lid=SCOPE_IDENTITY()
    --IF @@ERROR <> 0 AND @@TRANCOUNT > 0
    -- BEGIN  
    --	RAISERROR ('Error Occured', 16, 1)  
    --    ROLLBACK TRANSACTION    
    --    RETURN
    -- END
    --insert into MSM2_Admin.dbo.tbljoinauth
    --(
    --userid,lid,date,status,dbname
    --)
    --values
    --(
    --@userid,@lid,GETDATE(),0,(SELECT DB_NAME())
    --)
    --IF @@ERROR <> 0 AND @@TRANCOUNT > 0
    -- BEGIN  
    --	RAISERROR ('Error Occured', 16, 1)  
    --    ROLLBACK TRANSACTION    
    --    RETURN
    -- END 
    --end
    --else
    
    
    
    
    DECLARE @lid INT

    IF @userlicID <> 0
      BEGIN
          IF ( (SELECT Count(1)
                FROM   MSM2_Admin.dbo.tblUserAuth
                WHERE  str = (SELECT str
                              FROM   MSM2_Admin.dbo.tblUserAuth
                              WHERE  ID = @userlicID)) = 1 )
            BEGIN
                INSERT INTO MSM2_Admin.dbo.tbljoinauth
                            (userid,
                             lid,
                             date,
                             status,
                             dbname)
                VALUES      ( @userid,
                              @userlicID,
                              Getdate(),
                              0,
                              (SELECT Db_name()) )

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                UPDATE MSM2_Admin.dbo.tblUserAuth
                SET    str = @str,
                       used = 1,
                       dateupdate = Getdate()
                WHERE  ID = @userlicID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    COMMIT TRANSACTION 


select @userid
