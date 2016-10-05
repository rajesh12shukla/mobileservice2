CREATE PROCEDURE [dbo].[Spaddmerchant] @MerchantID VARCHAR(100),
                                      @LoginID    VARCHAR(100),
                                      @Username   VARCHAR(20),
                                      @Password   VARCHAR(100),
                                      @ID         INT
AS
    DECLARE @returnval INT

    SET @returnval=@id

    IF NOT EXISTS (SELECT 1
                   FROM   tblGatewayInfo
                   WHERE  ID = @ID)
      BEGIN
          IF NOT EXISTS (SELECT 1
                         FROM   tblGatewayInfo
                         WHERE  MerchantId = @MerchantID)
            BEGIN
                INSERT INTO tblGatewayInfo
                            (MerchantId,
                             LoginId,
                             Username,
                             Password)
                VALUES      ( @MerchantID,
                              @LoginID,
                              @Username,
                              @Password )

                SET @returnval= Scope_identity()
            END
          ELSE
            BEGIN
                RAISERROR ('Merchant ID already exists!',16,1)

                RETURN
            END
      END
    ELSE
      BEGIN
          UPDATE tblGatewayInfo
          SET    LoginId = @LoginID,
                 Username = @Username,
                 Password = @Password
          WHERE  ID = @ID
      END

    SELECT @returnval
