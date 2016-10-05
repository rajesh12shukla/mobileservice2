

CREATE PROCEDURE [dbo].[spAddSageLocation]
@Account varchar(15),
@LocName varchar(100),
@Type varchar(15),
@SageOwner VARCHAR(100),
@status smallint,
@Remarks text,
@ContactName varchar(50),
@Phone varchar(28),
@fax varchar(28),
@Cellular varchar(28),
@Email varchar(50),
@Website varchar(50),
@Address varchar(255),
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@RolAddress varchar(255),
@RolCity varchar(50),
@RolState varchar(2),
@RolZip varchar(10),
@SageKeyID   VARCHAR(100),
@LastUpdateDate DATETIME,
@SageCustomer varchar(100)
--@ContactData As [dbo].[tblTypeContact] Readonly

as

declare @Rol int
declare @locID int = convert(int,@SageKeyID)
declare @OwnerID int = 0
declare @DucplicateLocName int = 0
select @DucplicateLocName = COUNT(1) from Loc where Tag =@LocName 
select top 1 @OwnerID = ID from owner where sageid = @SageCustomer

BEGIN TRANSACTION

--if(@DucplicateLocName=0)
--begin

if(@SageKeyID = '0')
BEGIN
    if not exists(select 1 from Loc where SageID = @Account)
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
	--EMail,
	Cellular,
	Fax	
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
	--@email,
	@Cellular,
	@fax	
	)
	set @Rol=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
--select top 1 @OwnerID = ID from owner where sageid = @SageCustomer
 
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
	--Type,
	Owner,
	Remarks,
	SageID,
	Custom14	
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
	0,
	----@status,
	----@Type,
	--(select top 1 ID from owner where sageid = @SageCustomer),
	@OwnerID,
	--convert(int,@SageOwner),
	@Remarks,
	@Account,
	@Email
	)
	set @locID=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
END
END
 
ELSE
BEGIN
 
 select @Rol=Rol from Loc where Loc=  convert(int,@SageKeyID)
 --SageID=@Account and ltrim(rtrim(ISNULL(@Account ,''))) <> ''
  
  DECLARE @lastup INT =0
  
update Rol set
@lastup = 1,
Name=@LocName,
City=@RolCity,
State=@RolState,
Zip=@RolZip,
Address=@RolAddress,
----Remarks=@Remarks,
Contact=@ContactName,
Phone=@phone
----Website=@Website,
----EMail=@email,
--Cellular=@Cellular,
--Fax=@fax
where id =@Rol
 AND ISNULL( LastUpdateDate, '01/01/1900' ) < @LastUpdateDate

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

  IF( @lastup = 1 )
BEGIN
update Loc set

ID=@Account,
Tag=@LocName,
Address=@Address,
City=@City,
State=@State,
Zip=@Zip,
----Type=@Type,
----Status=@status,
--Owner=(select top 1 ID from owner where sageid = @SageCustomer),
--Owner= convert(int,@SageOwner),
Owner=@OwnerID,
Remarks=@Remarks,
SageID=@Account,
Custom14=@Email
where Loc= convert(int,@SageKeyID)
--SageID = @Account  and ltrim(rtrim(ISNULL(@Account ,''))) <> ''
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
END
END 


--end 
COMMIT TRANSACTION

 select @locID as locid ,@OwnerID as OwnerID 
--return @locID



 
