
CREATE proc [dbo].[spAddDiagnostic]
@fdesc varchar(255),
@category varchar(15),
@type smallint,
@mode smallint
as
if(@mode=0)
begin
if not exists(select 1 from Diagnostic where fDesc = @fdesc and Type = @type)
begin
insert into Diagnostic(category,type,fdesc) 
values(@category,@type,@fdesc)
end
else
begin
  RAISERROR ('Call Code already exists for selected type !',16,1)
  RETURN
end
end
else
if(@mode=1)
begin
--if not exists(select 1 from Diagnostic where fDesc = @fdesc and Type = @type)
--begin
update Diagnostic set category=@category  where fdesc=@fdesc and Type = @type
--end
--else
--begin
--  RAISERROR ('Call Code already exists for selected type !',16,1)
--  RETURN
--end
end
