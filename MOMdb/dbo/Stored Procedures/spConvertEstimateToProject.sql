CREATE PROCEDURE [dbo].[spConvertEstimateToProject]
@estimate int

as

declare 
@Job int, 
@loc int, 
@owner int, 
@Remarks varchar(max), 
@fdesc varchar(75),
@project int

select @project=e.Job, @loc= e.LocID, @owner=(select owner from loc where loc=e.locid), @Remarks= e.Remarks, @fdesc=e.fDesc from Estimate e 
where e.ID= @estimate

if(@loc <> 0)
begin
if (@project is null)
begin

BEGIN TRANSACTION

INSERT INTO job
(
Loc,
Owner,
fDate,
Status,
Remarks,
fDesc,
Rev,Mat,Labor,Cost,Profit,Ratio,Reg,OT,DT,TT,Hour,BRev,BMat,BLabor,BCost,BProfit,BRatio,BHour,Comm,BillRate,NT,Amount,
Type
)
values
(
@loc,
@owner,
GETDATE(),
0,
@Remarks,
@fdesc,
0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,
0
)

set @Job=SCOPE_IDENTITY()

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

UPDATE Estimate SET Job=@Job, Status=4 WHERE ID=@estimate

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
insert into JobTItem
(
--JobT,
Job,
Type,
fDesc,
Code,
Actual,
Budget,
Line,
[Percent],
Comm,
Modifier,
ETC,
ETCMod,
Labor
)
select 
--ID, 
@Job, 0, fDesc, Code, 0, Amount, Line,0, 0, 0, 0, 0, 0  
from EstimateI where Estimate =@estimate
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
  
COMMIT TRANSACTION

end 

else
BEGIN
	RAISERROR ('Estimate already converted to project !',16,1)
	RETURN
END

end
else
BEGIN
	RAISERROR ('Project cannot be created for Leads. Please convert the Lead to Customer!',16,1)
	RETURN
END

select @job