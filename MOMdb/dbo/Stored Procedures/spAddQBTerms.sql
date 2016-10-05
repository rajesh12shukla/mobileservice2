CREATE PROC [dbo].[spAddQBTerms] @QBtermsID      VARCHAR(100),
                         @Name           VARCHAR(50),
                         @LastUpdateDate DATETIME
AS
    IF NOT EXISTS(SELECT 1
                  FROM   tblterms
                  WHERE  QBtermsID = @QBtermsID)
      BEGIN
      
       IF NOT EXISTS(SELECT 1
                  FROM   tblterms
                  WHERE  Name = @Name)
      begin
      
          INSERT INTO tblTerms
                      (Name,
                       QBTermsID)
          VALUES      (@Name,
                       @QBtermsID)
                       
		end
		else
		begin
		UPDATE tblTerms
          SET    QBTermsID = @QBtermsID
          WHERE  Name = @Name
		
		end
        
      END
    ELSE
      BEGIN
          UPDATE tblTerms
          SET    Name = @Name
          WHERE  QBTermsID = @QBtermsID
                 AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate
      END
