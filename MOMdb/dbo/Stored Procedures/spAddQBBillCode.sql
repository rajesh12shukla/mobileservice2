CREATE proc [dbo].[spAddQBBillCode]
@QBInvID varchar(100),
@ContactName varchar(30),
@SalesDescription varchar(255),
@CatStatus int,
@Balance numeric(30,2),
@Measure varchar(10),
@Type int,
@Remarks varchar(8000),
@WarehouseID varchar(5),
@QBAccountID varchar(100),
@LastUpdateDate datetime

as

IF NOT EXISTS(SELECT 1 
              FROM   Inv 
              WHERE  QBInvID = @QBInvID) 
  BEGIN 
INSERT INTO Inv 
            (Name, 
             fDesc, 
             Cat, 
             --Balance,
             Price1, 
             Measure, 
             tax, 
             AllowZero, 
             inuse, 
             type, 
             sacct, 
             Remarks, 
             warehouse, 
             QBAccountID, 
                   QBInvID) 
VALUES      ( @ContactName, 
              @SalesDescription, 
              @CatStatus, 
              @Balance, 
              @Measure, 
              0, 
              0, 
              0, 
              @Type, 
              10, 
              @Remarks,@WarehouseID, @QBAccountID,@QBInvID) 
  END 
  else 
  begin 
UPDATE Inv 
	SET Name = @ContactName, 
		fDesc = @SalesDescription, 
		--Balance = @Balance, 
		Price1=@Balance,
		QBAccountID = @QBAccountID, 
		Remarks = @Remarks
	WHERE  QBInvID = @QBInvID
		   AND Isnull(LastUpdateDate, '01/01/1900') < @LastUpdateDate   END
