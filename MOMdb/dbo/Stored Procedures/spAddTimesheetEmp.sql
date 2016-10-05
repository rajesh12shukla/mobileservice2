CREATE proc [dbo].[spAddTimesheetEmp]
@StartDate datetime,
@EndDate datetime,
@Processed smallint,
@EmpData AS [dbo].[tblTypeTimesheetEmpl] Readonly,
@TicketData AS [dbo].[tblTypeTimesheetTickets] Readonly

as
declare @timesheetID int
if not exists(select 1 from tblTimesheet where StartDate =@StartDate and EndDate=@EndDate)
begin
	insert into tblTimesheet (StartDate, EndDate)values(@StartDate,@EndDate)
	set @timesheetID=SCOPE_IDENTITY()

	insert into tblTimesheetEmp
	(
	[TimesheetID],
		[EmpID],
		[Pay],
		[PayMethod],
		[Reg],
		[OT],
		[DT],
		[TT],
		[NT],
		[Holiday],
		[Vacation],
		[SickTime],
		[Zone],
		[Reimb],
		[Mileage],
		[Bonus],
		[Extra],
		[Total],
		[Misc],
		[Toll],FixedHours, Salary, MileRate, HourRate, DollarAmount,Custom
	)
	select @timesheetID,
		[EmpID],
		[Pay],
		[PayMethod],
		[Reg],
		[OT],
		[DT],
		[TT],
		[NT],
		[Holiday],
		[Vacation],
		[SickTime],
		[Zone],
		[Reimb],
		[Mileage],
		[Bonus],
		[Extra],
		[Total],
		[Misc],
		[Toll],FixedHours, Salary, MileRate, HourRate,DollarAmount,Custom
		 from @EmpData

	--update TicketD set 
	--TimesheetID=@timesheetID
	--where ID in (select TicketID from @TicketData)
end
else
begin 
	select @timesheetID = ID from tblTimesheet where StartDate =@StartDate and EndDate=@EndDate

	--delete from tblTimesheetEmp where timesheetid= @timesheetID
	update tblTimesheetEmp set
		[Pay]=ED.[Pay],
		[PayMethod]=ED.[PayMethod],
		[Reg]=ED.[Reg],
		[OT]=ED.[OT],
		[DT]=ED.[DT],
		[TT]=ED.[TT],
		[NT]=ED.[NT],
		[Holiday]=ED.[Holiday],
		[Vacation]=ED.[Vacation],
		[SickTime]=ED.[SickTime],
		[Zone]=ED.[Zone],
		[Reimb]=ED.[Reimb],
		[Mileage]=ED.[Mileage],
		[Bonus]=ED.[Bonus],
		[Extra]=ED.[Extra],
		[Total]=ED.[Total],
		[Misc]=ED.[Misc],
		[Toll]=ED.[Toll]
		,FixedHours=ED.FixedHours, Salary=ED.Salary, MileRate=ED.MileRate, HourRate=ED.HourRate,
		DollarAmount=ED.DollarAmount,
		Custom=ED.Custom
	from tblTimesheetEmp TSE inner join @EmpData ED on TSE.TimesheetID=@timesheetID and TSE.EmpID=ED.EmpID

	insert into tblTimesheetEmp
	(
		[TimesheetID],
		[EmpID],
		[Pay],
		[PayMethod],
		[Reg],
		[OT],
		[DT],
		[TT],
		[NT],
		[Holiday],
		[Vacation],
		[SickTime],
		[Zone],
		[Reimb],
		[Mileage],
		[Bonus],
		Extra,
		[Total],
		[Misc],
		[Toll],FixedHours, Salary, MileRate, HourRate,DollarAmount,Custom
	)
	select @timesheetID,
		[EmpID],
		[Pay],
		[PayMethod],
		[Reg],
		[OT],
		[DT],
		[TT],
		[NT],
		[Holiday],
		[Vacation],
		[SickTime],
		[Zone],
		[Reimb],
		[Mileage],
		[Bonus],
		Extra,
		[Total],
		[Misc],
		[Toll],FixedHours, Salary, MileRate, HourRate, DollarAmount,Custom
	from @EmpData where EmpID not in (select EmpID from tblTimesheetEmp where TimesheetID = @timesheetID)
		
	--update TicketD set 
	--TimesheetID=@timesheetID
	--where ID in (select TicketID from @TicketData) 
end

update TicketD set 
	TimesheetID=@timesheetID,
	Reg=td.Reg,
	OT =td.OT,
	NT=td.NT,
	DT=td.DT,
	TT=td.TT,
	Zone=td.Zone,
	Mileage= td.Mileage,
	[OtherE]=td.[Misc],
	[Toll]=td.[Toll],
	Custom2=CONVERT(varchar(50), td.Extra),
	HourlyRate=td.HourlyRate	,
	CustomTick5=td.Custom,
	CustomTick3=td.CustomTick3,
	CustomTick2=td.CustomTick2,
	CustomTick1=td.CustomTick1
	from TicketD d 
	inner join @TicketData td on td.TicketID=d.ID

update tblTimesheetEmp set
	Reg1=td.Reg,
	OT1 =td.OT,
	NT1=td.NT,
	DT1=td.DT,
	TT1=td.TT,
	Zone1=td.Zone,
	Mileage1= td.Mileage,
	Misc1=td.[Misc],
	Toll1=td.[Toll],
	Extra1=td.Extra,
	HourRate1=td.HourlyRate
	from tblTimesheetEmp d 
	inner join @TicketData td on td.Empid=d.EmpID and td.TicketID=0
	
--if(@Processed=1)
--begin

--update tbltimesheet set processed = 1 where StartDate =@StartDate and EndDate=@EndDate

--update TicketD set 
--	ClearPR=1
--	from TicketD d 
--	inner join @TicketData td on td.TicketID=d.ID

--end
