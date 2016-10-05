
CREATE PROC [dbo].[spAddEmailAccount] @InServer     VARCHAR(100),
                                     @InServerType VARCHAR(10),
                                     @InUsername   VARCHAR(100),
                                     @InPassword   VARCHAR(50),
                                     @InPort       INT,
                                     @OutServer    VARCHAR(100),
                                     @OutUsername  VARCHAR(100),
                                     @OutPassword  VARCHAR(50),
                                     @OutPort      INT,
                                     @SSL          BIT,
                                     @UserId       INT

AS
    IF ( @InServer <> '' )
      BEGIN
          DECLARE @count INT =0

          IF ( @InUsername = ''
                OR @InPassword = ''
                OR @InPort = 0
                OR @OutPort = 0
                OR @OutServer = ''
                OR @OutUsername = ''
                OR @OutPassword = '' )
            BEGIN
                SET @count=1
            END

          IF( @count = 0 )
            BEGIN
                DECLARE @mode INT = 0

                IF EXISTS (SELECT 1
                           FROM   tblEmailAccounts
                           WHERE  UserId = @UserId)
                  BEGIN
                      SET @mode=1
                  END

                IF ( @mode = 0 )
                  BEGIN
                      IF NOT EXISTS (SELECT 1
                                     FROM   tblEmailAccounts
                                     WHERE  InUsername = @InUsername)
                        BEGIN
                            INSERT INTO tblEmailAccounts
                                        (InServer,
                                         InServerType,
                                         InUsername,
                                         InPassword,
                                         InPort,
                                         OutServer,
                                         OutUsername,
                                         OutPassword,
                                         OutPort,
                                         [SSL],
                                         UserId)
                            VALUES      ( @InServer,
                                          @InServerType,
                                          @InUsername,
                                          @InPassword,
                                          @InPort,
                                          @OutServer,
                                          @OutUsername,
                                          @OutPassword,
                                          @OutPort,
                                          @SSL,
                                          @UserId )
                        END
                  END
                ELSE IF( @mode = 1 )
                  BEGIN
                      IF NOT EXISTS (SELECT 1
                                     FROM   tblEmailAccounts
                                     WHERE  InUsername = @InUsername
                                            AND UserId <> @UserId)
                        BEGIN
                            UPDATE tblEmailAccounts
                            SET    InServer = @InServer,
                                   InServerType = @InServerType,
                                   InUsername = @InUsername,
                                   InPassword = @InPassword,
                                   InPort = @InPort,
                                   OutServer = @OutServer,
                                   OutUsername = @OutUsername,
                                   OutPassword = @OutPassword,
                                   OutPort = @OutPort,
                                   [SSL] = @SSL
                            WHERE  UserId = @UserId
                        END
                  END
            END
      END
