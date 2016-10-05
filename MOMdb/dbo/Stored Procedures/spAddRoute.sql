create proc spAddRoute
@name varchar(50),
@mech int,
@remarks varchar(8000),
@id int
as
if (@id = 0)
begin
if not exists(select 1 from Route where Name = @name)
begin 
insert into Route 
(
Name,
Mech,
Remarks
)
values
(
@name,
@mech,
@remarks
)
end
else
BEGIN
    RAISERROR ('Name already exists, please use different name !',16,1)
    RETURN
END
end
else
begin
if not exists(select 1 from Route where Name = @name and ID<>@id)
begin 
update Route set
Name=@name,
Mech=@mech,
Remarks=@remarks
where id= @id
end
else
BEGIN
    RAISERROR ('Name already exists, please use different name !',16,1)
    RETURN
END
end

