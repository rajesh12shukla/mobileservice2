

CREATE PROCEDURE [dbo].[spAddLocation]
@Account varchar(15),
@LocName varchar(100),
@Address varchar(255),
@status smallint,
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@Route int,
@Terr int,
@Remarks text,



@ContactName varchar(50),
@Phone varchar(28),
@fax varchar(28),
@Cellular varchar(28),
@Email varchar(50),
@Website varchar(50),
@RolAddress varchar(255),
@RolCity varchar(50),
@RolState varchar(2),
@RolZip varchar(10),
@Type varchar(50),
@Owner int,
@Stax varchar(25),
@Lat varchar(50),
@Lng varchar(50),
@Custom1 varchar(50),
@Custom2 varchar(50),
@To varchar(250),
@CC varchar(250),
@ToInv varchar(250),
@CCInv varchar(250),
@CreditHold tinyint,
@DispAlert tinyint,
@CreditReason varchar(100),
@prospectID int,
@ContractBill tinyint,
@Terms int,
@ContactData As [dbo].[tblTypeContact] Readonly,
@BillRate numeric(30,2),
@OT numeric(30,2),
@NT numeric(30,2),
@DT numeric(30,2),
@Travel numeric(30,2),
@Mileage numeric(30,2)
--,
--@MAPAddress varchar(255)

as

declare @Rol int
declare @CustID int
declare @DucplicateAcctID int = 0
declare @DucplicateLocName int = 0

select @DucplicateAcctID = COUNT(1) from Loc where id =@Account 
select @DucplicateLocName = COUNT(1) from Loc where Tag =@LocName 


if(@DucplicateLocName=0)
begin

--if(@DucplicateAcctID=0)
--begin

BEGIN TRANSACTION
  
 if(@prospectID=0)  
 begin
 
	insert into Rol
	(

	City,
	State,
	Zip,
	Address,
	GeoLock,
	Remarks,
	Type,
	Contact,
	Name,
	Phone,
	Website,
	EMail,
	Cellular,
	Fax,
	Lat,
	Lng,
	LastUpdateDate
	)
	values
	(

	@RolCity,
	@RolState,
	@RolZip,
	@RolAddress,
	0,
	@Remarks,
	4,
	@ContactName,
	@LocName,
	@phone,
	@Website,
	@email,
	@Cellular,
	@fax,
	@Lat,
	@Lng,
	GETDATE()
	)
	set @Rol=SCOPE_IDENTITY()

end else
begin
	declare @ProspectROLID int
	select @ProspectROLID = Rol from Prospect where ID= @prospectID
	update Rol set Type = 4, LastUpdateDate=GETDATE() where ID= @ProspectROLID
	set @Rol = @ProspectROLID
end

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
  
--if not exists (select 1 from Loc where id =@Account)
--begin
insert into Loc
(
ID,
Tag,
Address,
City,
State,
Zip,
Rol,
Status,
Type,
Route,
Terr,
Owner,
STax,
Custom1,
Custom2,
Custom14,
Custom15,
Custom12,
Custom13,
Remarks,
DispAlert,
Credit,
CreditReason,
Prospect,
Billing,
defaultterms,
CreatedBy,
BillRate,
RateOT,
RateNT,
RateDT,
RateTravel,
RateMileage
--,
--MAPAddress
)
values
(
@Account,
@LocName,
@Address,
@City,
@State,
@Zip,
@Rol,
@status,
@Type,
@Route,
@Terr,
@Owner,
@Stax,
@Custom1,
@Custom2,
@To, 
@CC,
@ToInv,
@CCInv,
@Remarks,
@DispAlert,
@CreditHold,
@CreditReason,
@prospectID,
@ContractBill,
@Terms,
'MOM',
@BillRate,
@OT,
@NT,
@DT,
@Travel,
@Mileage
--,
--@MAPAddress
)
set @CustID=SCOPE_IDENTITY()
--end 
--else
--begin
-- RAISERROR ('Account # already exists, please use different Account # !', 16, 1)  
-- ROLLBACK TRANSACTION    
-- RETURN
--end
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 insert into Phone
 (
 Rol,
 fDesc,
 Phone,
 Fax, 
 Cell,
 Email
 )
 select @Rol,name,Phone,fax,cell,email from @ContactData
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 /******************convert prospect update data**********************/
 if(@prospectID<>0)
begin 

 --declare @ProspectROLID int
 --select @ProspectROLID = Rol from Prospect where ID= @prospectID
 
--update Phone set Rol=@Rol where Rol = @ProspectROLID
update Documents set Screen='Location', ScreenID=@CustID where Screen='SalesLead' and ScreenID=@prospectID
--update tblEmailRol set Rol = @Rol where Rol=@ProspectROLID
update Lead set RolType=4, Rol = @Rol where RolType=3 and Rol = @ProspectROLID
--update ToDo set Rol = @Rol where  Rol = @ProspectROLID
--update Done set Rol = @Rol where  Rol = @ProspectROLID
----update Estimate set RolID = @Rol where  RolID = @ProspectROLID
update TicketO set LType=0, LID = @CustID, [Owner]= @Owner where  LID = @prospectID and LType = 1 and ISNULL([Owner],0)=0

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
--delete from Rol where ID = @ProspectROLID
 
delete from Prospect where ID=@prospectID
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
end
 /********************/
 
 
 
 
 COMMIT TRANSACTION
 
--  end 
--else
--begin
-- RAISERROR ('Account # already exists, please use different Account # !', 16, 1)  
-- RETURN
--end
 
  end 
else
begin
 RAISERROR ('Location name already exists, please use different Location name !', 16, 1)  
 RETURN
end
 

 
 return(@CustID)
