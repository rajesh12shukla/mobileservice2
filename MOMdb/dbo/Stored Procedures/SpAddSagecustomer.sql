CREATE PROCEDURE [dbo].[SpAddSagecustomer] @UserName       NVARCHAR(50),
                                        @Password       NVARCHAR(50),
                                        @status         SMALLINT,
                                        @FName          VARCHAR(75),
                                        @Address        VARCHAR(8000),
                                        @City           VARCHAR(50),
                                        @State          VARCHAR(2),
                                        @Zip            VARCHAR(10),
                                        @country        VARCHAR(50),
                                        @Remarks        VARCHAR(8000),
                                        @Mapping        INT,
                                        @Schedule       INT,
                                        --@ContactData As [dbo].[tblTypeContact] Readonly,
                                        @Internet       INT,
                                        @contact        VARCHAR(50),
                                        @phone          VARCHAR(28),
                                        @Website        VARCHAR(50),
                                        @email          VARCHAR(50),
                                        @Cell           VARCHAR(28),
                                        @Type           VARCHAR(50),
                                        @SageKeyID   VARCHAR(100),
                                        @LastUpdateDate DATETIME,
                                        @Balance numeric(30,2),
                                        @Customer VARCHAR(10)
                                        
AS
    DECLARE @Rol INT
    DECLARE @work INT
    DECLARE @CustID INT

    BEGIN TRANSACTION

     if(@SageKeyID = '0')
      BEGIN
      if not exists(select 1 from Owner where SageID = @Customer)
      begin
          INSERT INTO Rol
                      (Name,
                       City,
                       State,
                       Zip,
                       Address,
                       GeoLock,
                       Remarks,
                       Type,
                       Country,
                       --Contact,
                       Phone,
                       Website,
                       EMail,
                       Cellular
                       )
          VALUES      ( @FName,
                        @City,
                        @State,
                        @Zip,
                        @Address,
                        0,
                        @Remarks,
                        0,
                        @country,
                        --@contact,
                        @phone,
                        @Website,
                        @email,
                        @Cell
                         )

          SET @Rol=Scope_identity()

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          INSERT INTO Owner
                      (fLogin,
                       Password,
                       Status,
                       TicketO,
                       TicketD,
                       Internet,
                       Rol,
                       --Type,                       
                       Balance,
                       SageID, 
                       OwnerID)
          VALUES      ( @UserName,
                        @Password,0,
                        --@status,
                        @Schedule,
                        @Mapping,
                        @Internet,
                        @Rol,
                        --@Type,
						@Balance,
						@Customer,
						@Customer)

          SET @CustID=Scope_identity()

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
          END
      END
    ELSE
      BEGIN
          SELECT @Rol = Rol
          FROM   Owner
          WHERE  ID = @SageKeyID
          --SageID = @Customer and  ltrim(rtrim(ISNULL(@Customer ,''))) <> ''

          DECLARE @lastup INT =0

          UPDATE Rol
          SET    @lastup = 1,
                 Name = @FName,
                 City = @City,
                 State = @State,
                 Zip = @Zip,
                 Address = @Address,
                 Remarks = @Remarks,
                 Country = @country,
                 --Contact = @contact,
                 Phone = @phone,
                 Website = @Website,
                 EMail = @email,
                 Cellular = @Cell
          WHERE  id = @Rol
                 AND ISNULL( LastUpdateDate, '01/01/1900' ) < @LastUpdateDate

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF( @lastup = 1 )
            BEGIN
                UPDATE Owner
                SET    
                --Type = @Type,
                --Status=@status,
                Balance=@Balance,
                SageID=@Customer,
                OwnerID=@Customer
                WHERE  ID = @SageKeyID
                --SageID = @Customer and ltrim(rtrim(ISNULL(@Customer ,''))) <> ''

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
    
    return @CustID
