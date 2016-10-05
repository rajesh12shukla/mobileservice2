CREATE PROCEDURE [dbo].[spAddCustomer]
@UserName	nvarchar(50),	
@Password	nvarchar(50),
@status smallint,
@FName varchar(75),
@Address varchar(8000),
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@country varchar(50),
@Remarks varchar(8000),
@Mapping int,
@Schedule int ,
@Internet int,
@contact varchar(50),
@phone varchar(28),
@Website varchar(50),
@email varchar(50),
@Cell varchar(28),
@Type varchar(50),
@Equipment smallint,
@SageID varchar(50),
@Billing smallint,
@grpbywo smallint,
@openticket smallint,
@ContactData As [dbo].[tblTypeContact] Readonly,
@BillRate numeric(30,2),
@OT numeric(30,2),
@NT numeric(30,2),
@DT numeric(30,2),
@Travel numeric(30,2),
@Mileage numeric(30,2)

as

declare @Rol int
declare @work int
declare @CustID int
declare @DucplicateCustName int = 0
declare @DucplicateSageID int = 0
select @DucplicateCustName = COUNT(1) from Rol r inner join Owner o on o.Rol=r.ID where Name =@FName 
--select @DucplicateSageID = COUNT(1) from Owner where SageID = @SageID and ltrim(rtrim(isnull(@SageID,''))) <> ''

if(@DucplicateCustName=0)
begin

BEGIN TRANSACTION
  
insert into Rol
(
Name,
City,
State,
Zip,
Address,
GeoLock,
Remarks,
Type,
Country,
Contact,
Phone,
Website,
EMail,
Cellular,
LastUpdateDate
)
values
(
@FName,
@City,
@State,
@Zip,
@Address,
0,
@Remarks,
0,
@country,
@contact,
@phone,
@Website,
@email,
@Cell,
GETDATE()
)
set @Rol=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 

if not exists (select 1 from Owner where fLogin =@UserName and @UserName <>'' union select 1 from tblUser where fUser=@UserName and @UserName <>'')
begin
insert into Owner
(
Balance,
fLogin,
Password,
Status,
Ledger,
TicketD,
Internet,
Rol,
Billing,
Type,
CPEquipment,
--SageID,
OwnerID,
CreatedBy,
GroupbyWO,
openticket,
BillRate,
RateOT,
RateNT,
RateDT,
RateTravel,
RateMileage
)
values
(
'0.00',
@UserName,
@Password,
@status,
@Schedule,
@Mapping,
@Internet,
@Rol,
@Billing,
@Type,
@Equipment,
@SageID,
'MOM',
@grpbywo,
@openticket,
@BillRate,
@OT,
@NT,
@DT,
@Travel,
@Mileage
)
set @CustID=SCOPE_IDENTITY()
end 
else
begin
 RAISERROR ('Username already exists, please use different username!', 16, 1)  
 ROLLBACK TRANSACTION    
 RETURN
end



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
 COMMIT TRANSACTION
 
   end 
else
begin
 RAISERROR ('Customer name already exists, please use different Customer name !', 16, 1)  
 RETURN
end
 
 
 return (@CustID)
