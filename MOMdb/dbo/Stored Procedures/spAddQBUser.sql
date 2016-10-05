
CREATE PROCEDURE [dbo].[spAddQBUser] @UserName        NVARCHAR(50),
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
                                    @QBEmployeeID    VARCHAR(100),
                                    @LastUpdateDate  DATETIME
AS
    DECLARE @Rol INT
    DECLARE @work INT = NULL
    DECLARE @Ticket VARCHAR(10)
    DECLARE @empid INT
    DECLARE @userid INT

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

    BEGIN TRANSACTION

    IF NOT EXISTS(SELECT 1
                  FROM   tblUser
                  WHERE  QBEmployeeID = @QBEmployeeID)
      BEGIN
          --IF EXISTS (SELECT 1
          --           FROM   tblUser
          --           WHERE  fUser = @UserName
          --           UNION
          --           SELECT 1
          --           FROM   Owner
          --           WHERE  fLogin = @UserName)
          --  BEGIN
                --if(@MName is not null and @MName <> '')
                --begin      
                --set @UserName+='_'+REPLACE(@MName,'.','')      
                --end
                IF( @LName IS NOT NULL
                    AND @LName <> '' )
                  BEGIN
                      SET @UserName+=Space(1)
                                     + Substring(Replace(@LName, '.', ''), 1, 1)
                  END

                IF EXISTS (SELECT 1
                           FROM   tblUser
                           WHERE  fUser = @UserName
                           UNION
                           SELECT 1
                           FROM   Owner
                           WHERE  fLogin = @UserName)
                  BEGIN
                      DECLARE @usernamecount INT=0

                      SELECT @usernamecount = Count(1)
                      FROM   tblUser
                      WHERE  (SELECT TOP 1 *
                              FROM   dbo.Split(fUser, Space(1))) = (SELECT TOP 1 *
                                                                    FROM   dbo.Split(@UserName, Space(1)))

                      SET @UserName= @UserName
                                     + Cast( @usernamecount AS VARCHAR(10) )
                  END
            --END

          IF( @FName <> '' )
            BEGIN
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
                             QBEmployeeID)
                VALUES      ( @UserName,
                              @Password,
                              @PDA,
                              @status,
                              0,
                              0,
                              @CreateTicket + @WorkDate + 'Y' + @ServiceHist
                              + 'YY',
                              'YYY' + @LocationRemarks + 'YY',
                              @PurchaseOrd + @Expenses + 'YYYY',
                              @ProgFunctions + 'YYYYY',
                              @AccessUser + 'YYYYY',
                              0,--@Field
                              @Remarks,
                              @Ticket,
                              @Lang,
                              @MerchantInfoId,
                              @QBEmployeeID )

                SET @userid=Scope_identity()

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                INSERT INTO Rol
                            (NAME,
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

                --INSERT INTO tblWork
                --            (fDesc,
                --             Type,
                --             Status,
                --             GeoLock,
                --             Super,
                --             DBoard)
                --VALUES      ( @UserName,
                --              0,
                --              @status,
                --              0,
                --              @Super,
                --              @Schedule )
                --SET @work=Scope_identity()
                --IF @@ERROR <> 0
                -- AND @@TRANCOUNT > 0
                --BEGIN
                --    RAISERROR ('Error Occured',16,1)
                --    ROLLBACK TRANSACTION
                --    RETURN
                --END
                --INSERT INTO Route
                --            (Name,
                --             Mech,
                --             Loc,
                --             Elev,
                --             Hour,
                --             Amount,
                --             Symbol,
                --             EN)
                --VALUES      ( @UserName,
                --              @work,
                --              0,
                --              0,
                --              0,
                --              0,
                --              1,
                --              1 )
                --IF @@ERROR <> 0
                --   AND @@TRANCOUNT > 0
                --  BEGIN
                --      RAISERROR ('Error Occured',16,1)
                --      ROLLBACK TRANSACTION
                --      RETURN
                --  END
                --EXEC Spcreatepda_userid
                --  @work
                --IF @@ERROR <> 0
                --   AND @@TRANCOUNT > 0
                --  BEGIN
                --      RAISERROR ('Error Occured',16,1)
                --      ROLLBACK TRANSACTION
                --      RETURN
                --  END
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
                             NAME,
                             DeviceID,
                             Pager)
                VALUES      ( 0,--@Field
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
                              @Pager )

                SET @empid=Scope_identity()

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                ------------------------ User Registration Code -------------------------
                --DECLARE @lid INT
                --IF @userlicID <> 0
                --  BEGIN
                --      IF ( (SELECT Count(1)
                --            FROM   MSM2_Admin.dbo.tblUserAuth
                --            WHERE  str = (SELECT str
                --                          FROM   MSM2_Admin.dbo.tblUserAuth
                --                          WHERE  ID = @userlicID)) = 1 )
                --        BEGIN
                --            INSERT INTO MSM2_Admin.dbo.tbljoinauth
                --                        (userid,
                --                         lid,
                --                         date,
                --                         status,
                --                         dbname)
                --            VALUES      ( @userid,
                --                          @userlicID,
                --                          Getdate(),
                --                          0,
                --                          (SELECT Db_name()) )
                --            IF @@ERROR <> 0
                --               AND @@TRANCOUNT > 0
                --              BEGIN
                --                  RAISERROR ('Error Occured',16,1)
                --                  ROLLBACK TRANSACTION
                --                  RETURN
                --              END
                --            UPDATE MSM2_Admin.dbo.tblUserAuth
                --            SET    str = @str,
                --                   used = 1,
                --                   dateupdate = Getdate()
                --            WHERE  ID = @userlicID
                --            IF @@ERROR <> 0
                --               AND @@TRANCOUNT > 0
                --              BEGIN
                --                  RAISERROR ('Error Occured',16,1)
                --                  ROLLBACK TRANSACTION
                --                  RETURN
                --              END
                --        END
                --  END
                --IF @@ERROR <> 0
                --   AND @@TRANCOUNT > 0
                --  BEGIN
                --      RAISERROR ('Error Occured',16,1)
                --      ROLLBACK TRANSACTION
                --      RETURN
                --  END
                SELECT @UserName AS UserName,
                       @userid   AS userid
            END
      END
    ELSE
      BEGIN
          DECLARE @exists INT = NULL

          SELECT @exists = 1,
                 @userid = u.ID,
                 @UserName = fUser,
                 @Rol = e.rol,
                 @empid = e.id
          FROM   tblUser u
                 INNER JOIN emp e
                         ON u.fUser = e.CallSign
          WHERE  qbemployeeid = @QBEmployeeID
                 AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate

          IF( @exists IS NOT NULL )
            BEGIN
                UPDATE tblUser
                SET    Status = @status,
                       Remarks = @remarks
                WHERE  ID = @UserID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                UPDATE tblWork
                SET    Status = @status
                WHERE  fDesc = @UserName

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                UPDATE Rol
                SET    NAME = @FName + ', ' + @LName,
                       City = @City,
                       State = @State,
                       Zip = @Zip,
                       Phone = @Tel,
                       Address = @Address,
                       EMail = @Email,
                       Cellular = @Cell
                WHERE  ID = @Rol

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                UPDATE Emp
                SET    Status = @status,
                       fFirst = @FName,
                       Middle = @MName,
                       Last = @LName,
                       DHired = @DtHired,
                       DFired = @DtFired,
                       NAME = @FName + ', ' + @LName
                WHERE  ID = @EmpID

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END
            END
      END

    COMMIT TRANSACTION
