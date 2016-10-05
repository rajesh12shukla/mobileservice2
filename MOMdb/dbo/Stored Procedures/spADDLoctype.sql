CREATE proc [dbo].[spADDLoctype]
@Name varchar(50),
@Remarks text
as
if not exists(select 1 from LocType where Type =@Name)
begin
insert into LocType 
(type, remarks) 
values 
(@Name,@Remarks)
end
else
BEGIN
  RAISERROR ('Location Type already exists, please use different location !',16,1)
  RETURN
END