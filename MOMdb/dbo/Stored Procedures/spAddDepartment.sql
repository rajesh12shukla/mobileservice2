
create proc spAddDepartment
@Name varchar(50),
@Default int,
@Remarks varchar(255)

as

if not exists( select 1 from JobType where Type=@Name)
begin

update JobType set isdefault = 0  

INSERT INTO JobType 
            ( Type, 
              isdefault, 
             Remarks, 
             LastUpdateDate) 
VALUES      ( @Name, 
			  @Default, 
              @Remarks,               
			  GETDATE() 
			  ) 
			  
end
else
BEGIN
  RAISERROR ('Department already exists, please use different name !',16,1)
  RETURN
END