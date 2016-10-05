CREATE PROC [dbo].[spAddQBPayrollWage] 
						 @QBWageID      VARCHAR(100),
                         @Name           VARCHAR(50),
                         @LastUpdateDate DATETIME,
                         @QBAccountID      VARCHAR(100)
AS
    IF NOT EXISTS(SELECT 1
                  FROM   PRWage
                  WHERE  QBWageID = @QBWageID)
      BEGIN
      
       IF NOT EXISTS(SELECT 1
                  FROM   PRWage
                  WHERE  fDesc = @Name)
      begin
      
          INSERT INTO PRWage
                      (fdesc,status,Field,FIT,FICA,MEDI,FUTA,SIT,Vac,WC,Uni,QBWageID,QBAccountID)
          VALUES      (@Name,0,1,1,1,1,1,1,1,1,1,@QBWageID,@QBAccountID)
                       
		end
		else
		begin
		UPDATE PRWage
          SET    QBWageID = @QBWageID,
          QBAccountID=@QBAccountID
          WHERE  fDesc = @Name
		
		end
        
      END
    ELSE
      BEGIN
          UPDATE PRWage
          SET    fDesc = @Name,
				QBAccountID=@QBAccountID
          WHERE  QBWageID = @QBWageID
                 AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate
      END
