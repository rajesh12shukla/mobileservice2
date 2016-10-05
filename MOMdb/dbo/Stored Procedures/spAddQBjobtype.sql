CREATE proc [dbo].[spAddQBjobtype]
@type varchar(50),
@remarks text,
@QBlocTypeID varchar(100)

as

IF NOT EXISTS(SELECT 1 
              FROM   LocType 
              WHERE  QBlocTypeID =  @QBlocTypeID) 
  BEGIN 
  
  while exists(select 1 from LocType where Type=@type)
  begin
  set @type += '1'
  end
  
      INSERT INTO LocType 
                  (Type, 
                   remarks, 
                   QBlocTypeID) 
      VALUES      (@type, 
                   @remarks, 
                   @QBlocTypeID) 
  END
