CREATE PROCEDURE [dbo].[spAddContract]
@loc int,
@owner int,
@date datetime,
@Status int,
@Creditcard int,
@Remarks text,
@BStart datetime,
@Bcycle int,
@BAmt numeric(30,2),
@SStart datetime,
@Cycle int,
@SWE int,
@Stime datetime,
@Sday int,
@SDate int,
@ElevJobData As [dbo].[tblTypeJoinElevJob] Readonly,
@Route varchar(75),
@hours numeric(30,2),
@fdesc varchar(75),
@CType varchar(15),
@ExpirationDate datetime,
@ExpirationFreq smallint,
@Expiration smallint,
@ContractBill smallint,
@CustomerBill smallint,
@Central int,
@Chart int,
@JobT int,
@EscalationType smallint,
@EscalationCycle smallint,
@EscalationFactor numeric(30,2),
@EscalationLast datetime,
@CustomItems AS tblTypeCustomTabItem readonly,
@BillRate numeric(30,2),
@RateOT numeric(30,2),
@RateNT numeric(30,2),
@RateDT numeric(30,2),
@RateTravel numeric(30,2),
@Mileage numeric(30,2),
@PO varchar(25)
as

declare @Job int
DECLARE @tblCustomFieldsId int
DECLARE @tblTabID int
DECLARE @Label VARCHAR(50)
DECLARE @TabLine SMALLINT
DECLARE @Value VARCHAR(50)
DECLARE @Format VARCHAR(50)

BEGIN TRANSACTION



declare 
@ProjectTemplate int,
@projremark varchar(75),
@projname varchar(75),
--@templateitems tblTypeProjectItem,
@bomitems tblTypeBomItem,
@MilestonItem tblTypeMilestoneItem ,
@servicetype varchar(15),
@InvExp int,
@InvServ int,
@WageS int,
@GLInt int,
@Post tinyint ,
@Charges tinyint,
@JobClose tinyint,
@fInt tinyint,
@types int

select top 1 @ProjectTemplate = ID from JobT where Type = 0

select 
@projremark = Remarks + convert(varchar(max), @Remarks) , @projname = fDesc, @servicetype = CType, 
@InvExp = InvExp, @InvServ=InvServ ,@Wages=Wage,@GLInt=GLInt,
@Post=Post, @Charges=Charge, @JobClose=JobClose, @fInt=fInt,@types=[Type]
from JobT where ID = @ProjectTemplate
			
INSERT INTO @bomitems
select 
ji.JobT,
ji.Job,
b.JobTItemID,	
ji.[Type] ,	
fdesc ,	
ji.Code,	
ji.Budget,	
Line,	
b.[Type] ,	
b.Item,	
b.QtyRequired,	
UM ,	
b.ScrapFactor ,	
b.BudgetUnit ,
b.BudgetExt,	
ji.Actual ,	
ji.[Percent]
from BOM b
inner join jobtitem ji on ji.ID = b.JobTItemID
where ji.JobT = @ProjectTemplate and (ji.job=0 or ji.job is null)

INSERT INTO @MilestonItem
select 
[JobT] ,
[Job] ,
m.[JobTItemID],
ji.[Type],
[fdesc] ,
ji.[Code] ,
[Line],
m.[MilestoneName] ,
[RequiredBy] ,
0 ,
[ProjAcquistDate] ,
[ActAcquistDate] ,
[Comments] ,
m.[Type]  ,
[Amount] 
from Milestone m
inner join jobtitem ji on ji.ID = m.JobTItemID
where ji.JobT = @ProjectTemplate and (ji.job=0 or ji.job is null)

exec @Job = spAddProject  
			@job =0,
			@owner=null ,
			@loc=@loc ,
			@fdesc=@fdesc ,
			@status=@Status ,
			@type=0 ,
			@Remarks= @projremark, 
			@ctype =@servicetype,
			@ProjCreationDate=NULL ,
			@PO =@PO,
			@SO =null,
			@Certified = 0,
			@Custom1 =null,
			@Custom2 =null,
			@Custom3 =null,
			@Custom4 =null,
			@Custom5 =null,
			@template =@ProjectTemplate,
			@RolName=null ,
			@city =null,
			@state =null,
			@zip =null,
			@country =null,
			@phone =null,
			@cellular =null,
			@fax =null,
			@contact =null,
			@email =null,
			@rolRemarks =null,
			@rolType =null,
			@InvExp =@InvExp,
			@InvServ =@InvServ,
			@Wage =@Wages,
			@GLInt =@GLInt,
			@jobtCType =null,
			@Post =@Post,
			@Charge =@Charges,
			@JobClose =@JobClose,
			@fInt =@fInt,
			--@Items=@templateitems ,
			--@TeamItems =null,			
			@BomItem = @bomitems ,
			@MilestonItem = @MilestonItem,
		
			@BillRate=@BillRate,
			@RateOT=@RateOT,
			@RateNT=@RateNT,
			@RateDT=@RateDT,
			@RateTravel=@RateTravel,
			@Mileage=@Mileage

update job set 

Custom20 = @Route, 
CreditCard = @Creditcard

where ID = @Job			

--INSERT INTO job
--(
--Loc,
--Owner,
--fDate,
--Status,
--CreditCard,
--Remarks,
--Rev,Mat,Labor,Cost,Profit,Ratio,Reg,OT,DT,TT,Hour,BRev,BMat,BLabor,BCost,BProfit,BRatio,BHour,Comm,BillRate,NT,Amount,
--Custom20,
--Type,
--fDesc,
--CType,
--Template,
--LastUpdateDate
--)
--values
--(
--@loc,
--@owner,
--@date,
--@Status,
--@Creditcard,
--@Remarks,
--0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,
--@route,
--0,
--@fdesc,
--@CType,
--(select top 1 ID from JobT where Type = 0),
--GETDATE()
--)

--set @Job=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

insert into Contract
(
Job,
BStart,
BCycle,
BAmt,
SStart,
SCycle,
SWE,
STime,
SDay,
SDate,
Loc,
Owner,
Hours,
Status,
ExpirationDate,
Frequencies,
Expiration,
Chart,
BEscType,
BEscCycle,
BEscFact,
EscLast
)
values
(
@Job,
@BStart,
@Bcycle,
@BAmt,
@SStart,
@Cycle,
@SWE,
@Stime,
@Sday,
@SDate,
@loc,
@owner,
@hours,
@Status,
case @Expiration when 1 then @ExpirationDate else NULL end ,
case @Expiration when 2 then @ExpirationFreq else NULL end ,
@Expiration,
@Chart,
@EscalationType,
@EscalationCycle,
@EscalationFactor,
@EscalationLast
)

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 insert into tbljoinElevJob
 (
	Job, elev,price,Hours
 )
 select @Job,Elevunit,price,hours from @ElevJobData
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 update Loc set Maint=1, Billing=@ContractBill where Loc=@loc  -- change by Mayuri 25th dec,15 for billing details in Loc, owner table

 UPDATE [Owner] SET Billing=@CustomerBill, Central=@Central WHERE ID=@owner

 --------------------------------------------- update custom data of recurring contract -----------------------------------------
 
	DECLARE db_cursor CURSOR FOR 

	SELECT [ID], [tblTabID], [Label], [Line], [Value], [Format] FROM @CustomItems

	OPEN db_cursor
	FETCH NEXT FROM db_cursor INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format 

	WHILE @@FETCH_STATUS = 0
	BEGIN  	

		INSERT INTO [dbo].[tblCustomJobT]
           ([JobTID]
		   ,[JobID]
           ,[tblCustomFieldsID]
           ,[Value])
		 VALUES
			   (@JobT
			   ,@Job
			   ,@tblCustomFieldsId
			   ,@Value)

	FETCH NEXT FROM db_cursor INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format 
	END  

	CLOSE db_cursor 
	DEALLOCATE db_cursor


 COMMIT TRANSACTION
GO

