CREATE procedure [dbo].[spConvertProspect]
@ROLID int,
@custid int

as

declare @prospectID int
--declare @custid int
declare @locid int
declare @Address varchar(8000)
declare @City varchar(50)
declare @State varchar(2)
declare @Zip varchar(10)
declare @country varchar(50)
declare @Remarks varchar(8000)
declare @contact varchar(50)
declare @phone varchar(28)
declare @Cell varchar(28)
declare @fax varchar(28)
declare @Website varchar(50)
declare @email varchar(50)

declare @Name varchar(75)
declare @Terr int
declare @RolAddress varchar(255)
declare @RolCity varchar(50)
declare @RolState varchar(2)
declare @RolZip varchar(10)

select @prospectID = ID from Prospect where Rol = @ROLID
if(@prospectID is not null)
begin

BEGIN TRANSACTION

select @Name=r.Name, @RolAddress=r.Address, @RolCity=r.City, @RolState=r.State, 
@RolZip=r.Zip, @country=r.Country, @Remarks=r.Remarks, 
@contact=r.Contact, @phone=r.Phone, @Cell=r.Cellular ,
@terr=P.Terr, @Address=P.Address, @City=P.City, @State=P.State, @Zip=P.Zip,
@fax=r.Fax, @email=r.EMail, @Website=r.Website
from Prospect P inner join  Rol r on r.ID=P.Rol where r.ID=@ROLID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END 
 
set @Remarks = 'Converted from Lead# '+CONVERT(varchar(15), @prospectID)+'-'+ CONVERT(varchar(15), @ROLID)+ SPACE(1)+'--'+ CONVERT(varchar(25), GETDATE() )+  CHAR(13)+CHAR(10)  + @Remarks 

if(@custid=0)
begin
	exec @custid = spAddCustomer '','',0,@Name,@RolAddress,@RolCity,@RolState,@RolZip,@country, @Remarks,0,0,0,@contact,@phone,@Website,@email,@Cell,null
	IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END 
end 

exec @locid = spAddLocation  @Name,@Name, @Address ,0, @City,@State,@Zip,0,@Terr,@Remarks,@contact,@phone,@fax,@Cell,@email,@Website,@RolAddress,@RolCity, @RolState, @RolZip,null,@custid,null,'','','','','','','','', 0, 0 , ''
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

declare @locationROLID int 
select @locationROLID = Rol from Loc where Loc=@locid

update Phone set Rol=@locationROLID where Rol = @ROLID
update Documents set Screen='Location', ScreenID=@locid where Screen='SalesLead' and ScreenID=@prospectID
update tblEmailRol set Rol = @locationROLID where Rol=@ROLID
update Lead set RolType=4, Rol = @locationROLID where RolType=3 and Rol = @ROLID
update ToDo set Rol = @locationROLID where  Rol = @ROLID
update Done set Rol = @locationROLID where  Rol = @ROLID
update Estimate set RolID = @locationROLID where  RolID = @ROLID
update TicketO set LType=0, LID = @locid, [Owner]=@custid where  LID = @prospectID and LType = 1 and ISNULL([Owner],0)=0

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
delete from Rol where ID = @ROLID
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 
delete from Prospect where ID=@prospectID
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END


COMMIT TRANSACTION

end
