
CREATE PROCEDURE [dbo].[Spadddocument] @screen   VARCHAR(20),
                                      @TicketID INT,
                                      @filename VARCHAR(75),
                                      @Path     VARCHAR(255),
                                      @Type     VARCHAR(10),
                                      @TempID   VARCHAR(150),
                                      @subject  VARCHAR(70),
                                      @body     VARCHAR(250),
                                      @mode     SMALLINT,
                                      @id       INT
AS
    IF ( @Type = 'xlsx' )
      BEGIN
          SET @Type='xls'
      END
    ELSE IF( @Type = 'docx' )
      BEGIN
          SET @Type='doc'
      END
    ELSE IF( @Type = 'png'
         OR @Type = 'jpg'
         OR @Type = 'bmp'
         OR @Type = 'gif' )
      BEGIN
          SET @Type='Picture'
      END

    DECLARE @DocType SMALLINT

    SELECT @DocType = ID
    FROM   doctype
    WHERE  fdesc LIKE @Type + '%'

    IF ( @DocType IS NULL )
      BEGIN
          SET @DocType=(SELECT ID
                        FROM   doctype
                        WHERE  fdesc = 'other')
      END

    IF( @mode = 0 )
      BEGIN
          INSERT INTO documents
                      (screen,
                       screenid,
                       line,
                       filename,
                       path,
                       type,
                       tempid,
                       [subject],
                       body)
          VALUES      ( @screen,
                        @TicketID,
                        1,
                        @filename,
                        @Path,
                        @DocType,
                        @TempID,
                        @subject,
                        @body )
      END
    ELSE IF( @mode = 1 )
      BEGIN
      
      if(@filename='')
      begin
      select @filename= filename , @Path =Path, @DocType=Type  from Documents where ID=@id
      end
      
          UPDATE Documents
          SET    filename = @filename,
                 path = @Path,
                 Type = @DocType,
                 [subject] = @subject,
                 body = @body
          WHERE  ID = @id
      END 
