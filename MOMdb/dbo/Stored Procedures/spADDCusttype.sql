CREATE proc [dbo].[spADDCusttype]
@Name varchar(50),
@Remarks text
as
if not exists(select 1 from OType where Type =@Name)
begin
insert into otype 
(type, remarks) 
values 
(@Name,@Remarks)
end
else
BEGIN
  RAISERROR ('Customer Type already exists, please use different type !',16,1)
  RETURN
END