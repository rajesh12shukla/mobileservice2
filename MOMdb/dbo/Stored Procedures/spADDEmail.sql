
CREATE PROC [dbo].[spADDEmail] @From        VARCHAR(100),
                              @To          TEXT,
                              @Cc          TEXT,
                              @Bcc         TEXT,
                              @Subject     VARCHAR(200),
                              @SentDate    DATETIME,
                              @Attachments SMALLINT,
                              @msgID       VARCHAR(200),
                              @UID         int,
                              @GUID        UNIQUEIDENTIFIER,
                              @type        SMALLINT,
                              @User        INT,
                              @accountID   VARCHAR(100)
AS

declare @success smallint = 0

    IF NOT EXISTS (SELECT 1
                   FROM   tblemail
                   WHERE  uid = @UID
                          AND Type = 0)
      BEGIN
          INSERT INTO tblemail
                      ([From],
                       [To],
                       Cc,
                       Bcc,
                       Subject,
                       SentDate,
                       RecDate,
                       Attachments,
                       msgID,
                       UID,
                       BodyReceived,
                       GUID,
                       Type,
                       [User],
                       AccountID)
          VALUES      ( @From,
                        @To,
                        @Cc,
                        @Bcc,
                        @Subject,
                        @SentDate,
                        Getdate(),
                        @Attachments,
                        @msgID,
                        @UID,
                        1,
                        @GUID,
                        @type,
                        @User,
                        @accountID )

          DECLARE @emailid INT = Scope_identity()
          
			IF( @type <> 0 )
			begin
					INSERT INTO tblEmailRol               
					 SELECT @emailid,ID FROM   Rol
						   WHERE  type in (0,3,4) and RTRIM(LTRIM(EMail)) in 
						   (SELECT items FROM Split(@To, ',') union SELECT items FROM Split(@Cc, ',') union SELECT items FROM Split(@Bcc, ','))
			end
			else
			begin
					INSERT INTO tblEmailRol 
					 select @emailid, ID FROM Rol 
					 WHERE type in (0,3,4) and --EMail = @From
					 RTRIM(LTRIM(EMail)) in 
					(select @from union SELECT items FROM Split(@To, ',') union SELECT items FROM Split(@Cc, ',') union SELECT items FROM Split(@Bcc, ','))
			end
          
          set @success = 1
      END 
      
      insert into tblEmailAddresses 
      SELECT items FROM Split(@To, ',') union SELECT items FROM Split(@Cc, ',') union SELECT items FROM Split(@Bcc, ',')
      except 
      (
		SELECT DISTINCT RTRIM(LTRIM(EMail)) 
		FROM   Rol 
		WHERE  Type IN ( 0, 3, 4 ) 
			   AND Isnull(RTRIM(LTRIM(EMail)), '') <> '' 
		UNION 
		SELECT DISTINCT RTRIM(LTRIM(EMail)) 
		FROM   Phone 
		WHERE  Rol IN (SELECT DISTINCT Rol 
					   FROM   Rol 
					   WHERE  Type IN ( 0, 3, 4 )) 
			   AND Isnull(RTRIM(LTRIM(EMail)), '') <> '' 
		union
		select distinct RTRIM(LTRIM(EMail)) from tblEmailAddresses
      )
            
      --update tblEmailAccounts set LastFetch=GETDATE() where UserId=@User
      
      select @success
