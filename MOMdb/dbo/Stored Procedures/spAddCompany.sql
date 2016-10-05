

CREATE PROCEDURE [dbo].[spAddCompany]

@CompanyName varchar(15),
@Address varchar(8000),
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@Tel varchar(22),
@fax varchar(20),
@Email varchar(50),
@Web varchar(50),
@MSM varchar(15),
@DSN varchar(100),
@DBname varchar(50),
@UserName varchar(50),	
@Password varchar(50),
@Contact varchar(50),
@Remarks varchar(200)

as
BEGIN TRANSACTION

if not exists (select 1 from Control where DBName=@DBname)
begin
insert into Control
(
Name,
Address,
City,
State,
Zip,
Phone,
Fax,
Email,
WebAddress,
msm,
Dsn,
DBname,
username,
password,
GeoLock,
Contact,
Remarks,
Custweb
)
values
(
@CompanyName,
@Address,
@City,
@State,
@Zip,
@Tel,
@fax,
@Email,
@Web,
@MSM,
@DSN,
@DBname,
@UserName,
@Password,
0,
@Contact,
@Remarks,
0
)
end
else
begin
RAISERROR ('Database name already exixts, please use different database name!', 16, 1)    
 RETURN
end
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END


INSERT [dbo].[tblUser] ([fUser], [Password], [Status], [Access], [fStart], [fEnd], [Since], [Last], [Remarks], [Owner], [Location], [Elevator], [Invoice], [Deposit], [Apply], [WriteOff], [ProcessC], [ProcessT], [Interest], [Collection], [ARViewer], [AROther], [Vendor], [Bill], [BillSelect], [BillPay], [PO], [APViewer], [APOther], [Chart], [GLAdj], [GLViewer], [IReg], [CReceipt], [PJournal], [YE], [Service], [Financial], [Item], [InvViewer], [InvAdj], [Job], [JobViewer], [JobTemplate], [JobClose], [JobResult], [Dispatch], [Ticket], [Resolve], [TestDate], [TC], [Human], [DispReport], [Violation], [UserS], [Control], [Bank], [BankRec], [BankViewer], [Manual], [Log], [Code], [STax], [Zone], [Territory], [Commodity], [Employee], [Crew], [PRProcess], [PRRemit], [PRRegister], [PRReport], [Diary], [TTD], [Document], [Phone], [ToDo], [Sales], [ToDoC], [EN], [Proposal], [Convert], [POLimit], [FU], [POApprove], [Tool], [Vehicle], [Estimates], [AwardEstimates], [BidResults], [Competitors], [JobHours], [Totals], [fDate], [PDA], [Tech], [MassResolvePDATickets], [ListsAdmin], [UserType]) VALUES (N'ADMIN', N'PASSWORD', 0, 2, CAST(0x00009A1600000000 AS DateTime), CAST(0x00009FCA00000000 AS DateTime), CAST(0x000092F400000000 AS DateTime), CAST(0x000092F400000000 AS DateTime), N'', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', 0, N'YYYYYY', 0, 0, N'YYYYYY', N'YYYYYY', CAST(0.00 AS Numeric(30, 2)), N'YYYYYY', 1, N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', NULL, 1, CAST(0x00009A5200000000 AS DateTime), NULL, NULL, 1, 0, NULL)


IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 INSERT INTO Inv 
            (Name, 
             fDesc, 
             Cat, 
             Balance, 
             Measure, 
             tax, 
             AllowZero, 
             inuse, 
             type, 
             sacct, 
             Remarks, Status, Price1)  
VALUES      ( 'Recurring', 
              'Recurring', 
              0, 
              0, 
              '', 
              0, 
              0, 
              0, 
              1, 
              10, 
              '', 0,0) 
              
INSERT INTO Inv 
            (Name, 
             fDesc, 
             Cat, 
             Balance, 
             Measure, 
             tax, 
             AllowZero, 
             inuse, 
             type, 
             sacct, 
             Remarks, Status, Price1) 
VALUES      ( 'Expenses', 
              'Expenses', 
              0, 
              0, 
              '', 
              0, 
              0, 
              0, 
              1, 
              10, 
              '', 0,0) 
              
INSERT INTO Inv 
            (Name, 
             fDesc, 
             Cat, 
             Balance, 
             Measure, 
             tax, 
             AllowZero, 
             inuse, 
             type, 
             sacct, 
             Remarks, Status, Price1) 
VALUES      ( 'Mileage', 
              'Mileage', 
              0, 
              0, 
              '', 
              0, 
              0, 
              0, 
              1, 
              10, 
              '', 0,0) 
              
INSERT INTO Inv 
            (Name, 
             fDesc, 
             Cat, 
             Balance, 
             Measure, 
             tax, 
             AllowZero, 
             inuse, 
             type, 
             sacct, 
             Remarks, Status, Price1) 
VALUES      ( 'Time Spent', 
              'Time Spent', 
              0, 
              0, 
              '', 
              0, 
              0, 
              0, 
              1, 
              10, 
              '', 0,0) 
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 INSERT INTO JobType 
            (
            Type,
            Remarks
            ) 
VALUES      ( 
			  'Recurring:R', 
              'Recurring'               
            ) 
            
SET IDENTITY_INSERT JobType ON            
            INSERT INTO JobType 
            (
            ID,
            Type,
            Remarks
            ) 
VALUES      ( 
				0,
			  'PM', 
              'PM'               
            ) 
SET IDENTITY_INSERT JobType OFF
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
  INSERT INTO Category 
            (
            Type,
            Remarks
            ) 
VALUES      ( 
			  'Recurring', 
              'Recurring'               
            ) 
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
INSERT INTO PType 
            (Type) 
VALUES      ('Mobile Service') 
 
 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
 
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'AP1099', N'0', NULL, 1)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APCLimit', N'$0.00', NULL, 2)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APDays', N'10', NULL, 3)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APDetail', N'0', NULL, 4)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APDisc', N'0.00', NULL, 5)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APItemQuan', N'1', NULL, 7)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APStatus', N'0', NULL, 8)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APTerms', N'3', NULL, 9)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APType', N'Cost of Sales', NULL, 10)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'APVoid', N'1', NULL, 11)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BankBalance', N'0', NULL, 13)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BonusGL', N'12', NULL, 14)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Branch', N'0', NULL, 15)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BranchNo', N'00', NULL, 16)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BudgetV1', N'Budget1', NULL, 17)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BudgetV2', N'Budget2', NULL, 18)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'BudgetV3', N'Budget3', NULL, 19)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'CollAging', N'30', NULL, 23)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Country', N'0', NULL, 24)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Customiz2', N'0', NULL, 26)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'CustType', N'General', NULL, 27)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DArea', N'720', NULL, 28)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc1', N'Discount 1', NULL, 29)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc2', N'Discount 2', NULL, 30)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc3', N'Discount 3', NULL, 31)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc4', N'Discount 4', NULL, 32)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc5', N'Discount 5', NULL, 33)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Disc6', N'Discount 6', NULL, 34)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DispNature', N'0', NULL, 35)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DispSource', N'None', NULL, 36)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DState', N'CO', NULL, 37)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ECurrent', N'31', NULL, 39)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev1', N'0', NULL, 40)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev10', N'Custom10', NULL, 41)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev11', N'Custom11', NULL, 42)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev12', N'Custom12', NULL, 43)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev13', N'Custom13', NULL, 44)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev14', N'Custom14', NULL, 45)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev15', N'Custom15', NULL, 46)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev2', N'Custom2', NULL, 47)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev3', N'Custom3', NULL, 48)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev4', N'Custom4', NULL, 49)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev5', N'Custom5', NULL, 50)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev6', N'Custom6', NULL, 51)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev7', N'Custom7', NULL, 52)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev8', N'Custom8', NULL, 53)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Elev9', N'Custom9', NULL, 54)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'EMail', N'info@automatedintegration.com', NULL, 55)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'EndTime', N'05:00 PM', NULL, 56)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'FederalID', N'11-111111', NULL, 57)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ForceAddress', N'False', NULL, 58)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ForcePhoneNumber', N'False', NULL, 59)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLCOST', N'40', NULL, 64)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLOU', N'9', NULL, 67)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLPR', N'7', NULL, 68)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLPRE', N'98', NULL, 70)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GLSALES', N'10', NULL, 71)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GSTGL', N'9', NULL, 74)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'GSTRate', N'0.00', NULL, 75)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'HolidayGL', N'12', NULL, 76)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvGL', N'0', NULL, 81)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvGL', N'0', NULL, 82)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvItemDes', N'0', NULL, 83)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvoiceVerify', N'', NULL, 84)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'InvTicket', N'2', NULL, 85)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job1', N'Custom1', NULL, 86)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job10', N'Custom10', NULL, 87)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job11', N'Custom11', NULL, 88)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job12', N'Custom12', NULL, 89)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job13', N'Custom13', NULL, 90)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job14', N'Custom14', NULL, 91)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job15', N'Custom15', NULL, 92)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job2', N'Custom2', NULL, 93)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job3', N'Custom3', NULL, 94)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job4', N'Custom4', NULL, 95)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job5', N'Custom5', NULL, 96)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job6', N'Custom6', NULL, 97)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job7', N'Custom7', NULL, 98)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job8', N'Custom8', NULL, 99)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job9', N'Custom9', NULL, 100)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastCalc', N'01/01/2003', NULL, 102)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastInt', N'0', NULL, 103)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastRun', N'4/01/2011', NULL, 104)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastRunT', N'5/1/2011', NULL, 105)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc1', N'Custom 1', NULL, 106)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc10', N'Custom 10', NULL, 107)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc2', N'Custom 2', NULL, 108)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc3', N'Custom 3', NULL, 109)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc4', N'Custom 4', NULL, 110)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc5', N'Custom 5', NULL, 111)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc6', N'Custom 6', NULL, 112)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc7', N'Custom 7', NULL, 113)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc8', N'Custom 8', NULL, 114)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc9', N'Custom 9', NULL, 115)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDRoute', N'2', NULL, 116)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDSTax', N'CO', NULL, 117)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDSType', N'STANDARD', NULL, 118)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDTerr', N'2', NULL, 119)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocDZone', N'1', NULL, 121)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LocType', N'Non-Contract', NULL, 122)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoginN', N'1', NULL, 123)

INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LoginY', N'1', NULL, 124)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolCallAssigned', N'', 0, 127)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolCallClosed', N'', 0, 128)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolCallUnassigned', N'', 0, 129)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolHotspots', N'', 0, 130)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolInternal', N'', 0, 131)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MapSymbolWorker', N'', 0, 132)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MaxCheck', N'15', NULL, 133)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MileRate', N'0.2800', NULL, 136)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ModuleN', N'0', NULL, 137)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ModuleY', N'0', NULL, 138)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextApply', N'34', NULL, 139)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextBatch', N'80', NULL, 140)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextDep', N'10', NULL, 141)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextInv', N'48', NULL, 142)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextJob', N'37', NULL, 143)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextQuote', N'4', NULL, 145)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'NextTicket', N'265', NULL, 146)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OpenCallOptionElapsed', N'False', NULL, 148)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OpenCallOptionPastDue', N'False', NULL, 149)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OpeningOption', N'OpeningThisWeek', NULL, 150)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionCallVoid', N'False', NULL, 151)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionEnableUser', N'', NULL, 152)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionIncludeHotspot', N'False', NULL, 153)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionMinimize', N'False', NULL, 154)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OptionTimeStampClose', N'False', NULL, 155)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Owner1', N'Custom1', NULL, 156)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Owner2', N'Custom2', NULL, 157)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PageA', N'0', NULL, 158)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PageT', N'0', NULL, 159)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PDAON', N'1', NULL, 160)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PMarkup', N'0', NULL, 161)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PMarkup', N'0', NULL, 162)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PO1', N'Custom1', NULL, 163)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PO2', N'Custom2', NULL, 164)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'POApprove', N'0', NULL, 165)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRAmount', N'2535.45', NULL, 167)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBFICA', N'-0.000000000000212', NULL, 168)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBFIT', N'0', NULL, 169)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBFUTA', N'0', NULL, 170)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBLocal', N'0', NULL, 171)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBMEDI', N'0', NULL, 172)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRBSIT', N'0', NULL, 173)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCFactor', N'10.80', NULL, 174)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCLapsed', N'1', NULL, 175)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCRange', N'2', NULL, 176)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCSTax', N'1', NULL, 177)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCycle', N'0', NULL, 178)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRCZero', N'0', NULL, 179)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price1', N'50', NULL, 180)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price2', N'100', NULL, 181)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price3', N'300', NULL, 182)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Price4', N'1000', NULL, 183)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD1', N'0', NULL, 184)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD2', N'0', NULL, 185)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD3', N'0', NULL, 186)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD4', N'0', NULL, 187)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PriceD5', N'0', NULL, 188)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRLast', N'2/20/2011', NULL, 189)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRLast2', N'2/26/2011', NULL, 190)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ProgramCustom', N'0', NULL, 191)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PromptForTime', N'False', NULL, 192)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRQuest', N'0', NULL, 193)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRFICA', N'2', NULL, 194)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRFIT', N'2', NULL, 195)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRFUTA', N'2', NULL, 196)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRLOCAL', N'0', NULL, 197)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRMEDI', N'2', NULL, 198)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'PRRSIT', N'2', NULL, 199)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Quote1', N'Custom1', NULL, 200)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Quote2', N'Custom2', NULL, 201)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ReconInt', N'21', NULL, 202)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ReconSC', N'21', NULL, 203)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRound', N'0', NULL, 204)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResroundA2', N'1', NULL, 205)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundAM', N'0', NULL, 206)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundOT', N'1', NULL, 207)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResroundP2', N'05:00 PM', NULL, 208)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundPM', N'05:00 PM', NULL, 209)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ResRoundTo', N'0', NULL, 210)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Return', N'0.00', NULL, 211)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleAutoSizeColumns', N'True', NULL, 212)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleAutoSizeRows', N'False', NULL, 213)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleConfirmBar', N'False', NULL, 214)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleFindSlotWeekend', N'False', NULL, 215)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleOpenView', N'OptionViewTag', NULL, 216)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleShowIcons', N'False', NULL, 217)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleTimeIncrement', N'30 Minutes', NULL, 218)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleTimeRange', N'Whole Day', NULL, 219)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ScheduleWarnConflict', N'False', NULL, 220)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ShowCType', N'0', NULL, 221)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ShowWorkerType', N'', NULL, 222)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ShowWorkerZone', N'', NULL, 223)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'StartTime', N'08:00 AM', NULL, 224)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'StateID', N'11-1111111', NULL, 225)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Terms', N'3', NULL, 226)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket1', N'PO#', NULL, 227)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket2', N'Custom2', NULL, 228)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket3', N'Custom3', NULL, 229)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket4', N'Custom4', NULL, 230)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket5', N'Custom5', NULL, 231)

INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vacation', N'0', NULL, 232)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'VacationGL', N'12', NULL, 233)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle1', N'Custom1', NULL, 234)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle2', N'Custom2', NULL, 235)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle3', N'Custom3', NULL, 236)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle4', N'Custom4', NULL, 237)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vehicle5', N'Custom5', NULL, 238)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor1', N'Custom1', NULL, 239)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor2', N'Custom2', NULL, 240)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor3', N'Custom3', NULL, 241)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor4', N'Custom4', NULL, 242)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor5', N'Custom5', NULL, 243)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'VerifyAddress', N'', NULL, 244)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'ViewRolodex', N'0', NULL, 245)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job16', N'Custom16', NULL, 246)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job17', N'Custom17', NULL, 247)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job18', N'Custom18', NULL, 248)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job19', N'Custom19', NULL, 249)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Job20', N'Custom20', NULL, 250)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc11', N'Custom11', NULL, 251)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc12', N'Custom12', NULL, 252)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc13', N'Custom13', NULL, 253)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc14', N'Custom14', NULL, 254)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Loc15', N'Custom15', NULL, 255)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'LastCost', N'0', NULL, 256)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'POGrid', N'0', NULL, 257)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OHPercent', N'3.5000', NULL, 258)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor6', N'Custom6', NULL, 260)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor7', N'Custom7', NULL, 261)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor8', N'Custom8', NULL, 262)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor9', N'Custom9', NULL, 263)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Vendor10', N'Custom10', NULL, 264)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect1', N'Custom1', NULL, 265)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect2', N'Custom2', NULL, 266)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect3', N'Custom3', NULL, 267)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect4', N'Custom4', NULL, 268)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Prospect5', N'Custom5', NULL, 269)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Estimate1', N'Custom1', NULL, 270)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Estimate2', N'Custom2', NULL, 271)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TKStatus', N'0', NULL, 272)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'OHCharge', N'10/31/2008', NULL, 273)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Quote3', N'Custom3', NULL, 274)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp1', N'Custom1', NULL, 275)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp2', N'Custom2', NULL, 276)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp3', N'Custom3', NULL, 277)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp4', N'Custom4', NULL, 278)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Emp5', N'Custom5', NULL, 279)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DBColor', N'14', NULL, 280)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'DBColor2', N'12', NULL, 281)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'JobEsc', N'3', NULL, 282)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket6', N'Diag Only', NULL, 283)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket7', N'BIO', NULL, 284)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket8', N'USA', NULL, 285)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket9', N'Custom9', NULL, 286)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'Ticket10', N'Custom10', NULL, 287)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'MultiTravel', N'0', NULL, 288)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'IntRate', N'0', NULL, 289)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'IntPer', N'0', NULL, 290)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'IntComp', N'0', NULL, 291)

INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst1', N'TicketCustom1', NULL, 292)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst2', N'TicketCustom2', NULL, 293)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst3', N'TicketCheckbox1', NULL, 294)
INSERT [dbo].[Custom] ([Name], [Label], [Number], [ID]) VALUES (N'TicketCst4', N'TicketCheckbox2', NULL, 295)

 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 

SET IDENTITY_INSERT [dbo].[tblTerms] ON 
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (0, N'Upon Receipt', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (1, N'Net 10 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (2, N'Net 15 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (3, N'Net 30 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (4, N'Net 45 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (5, N'Net 60 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (6, N'2%-10/Net 30 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (7, N'Net 90 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (8, N'Net 180 Days', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (9, N'COD', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (10, N'Net 120', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (11, N'Net 150', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (12, N'Net 210', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (13, N'Net 240', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (14, N'Net 270', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (15, N'Net 300', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (16, N'Net Due On 10th', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (17, N'Net Due', NULL, NULL)
INSERT [dbo].[tblTerms] ([ID], [Name], [QBTermsID], [LastUpdateDate]) VALUES (18, N'Credit Card', NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblTerms] OFF



 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END
 
 
SET IDENTITY_INSERT [dbo].[tblPages] ON
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (1, N'Ticketlist', N'ticketlistview.aspx', 0)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (2, N'Schedule Board', N'scheduler.aspx', 0)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (3, N'Map', N'map.aspx', 0)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (4, N'Route Builder', N'routebuilder.aspx', 0)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (5, N'Add/Edit Ticket', N'addticket.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (6, N'E-Timesheet', N'etimesheet.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (7, N'Equipments', N'equipments.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (8, N'Add/Edit Equipment', N'addequipment.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (9, N'Payment History', N'paymenthistory.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (10, N'Users', N'users.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (11, N'Add/Edit User', N'adduser.aspx', NULL)
INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (12, N'Setup', N'setup.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (13, N'Chart of Account', N'chartofaccount.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (14, N'Add/Edit COA', N'addcoa.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (15, N'Journal Entry', N'journalentry.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (16, N'Add/Edit Journal Entry', N'addjournalentry.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (17, N'Received Payment', N'receivepayment.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (18, N'Add/Edit Received Payment', N'addreceivepayment.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (19, N'Deposit', N'managedeposit.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (20, N'Add/Edit Deposit', N'adddeposit.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (21, N'Vendors', N'vendors.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (22, N'Add/Edit Vendors', N'addvendor.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (23, N'Bills', N'managebills.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (24, N'Add/Edit Bills', N'addbills.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (25, N'Write Checks', N'writechecks.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (26, N'Bank Reconciliation', N'bankrecon.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (27, N'Project', N'project.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (28, N'Add/Edit Project', N'addproject.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (29, N'Purchase Order', N'managepo.aspx', NULL)

INSERT [dbo].[tblPages] ([ID], [PageName], [URL], [Status]) VALUES (30, N'Add/Edit PO', N'addpo.aspx', NULL)


SET IDENTITY_INSERT [dbo].[tblPages] OFF


insert into tblPagePermissions 
([User],Page,Access) 
select u.ID as userid,
p.ID as pageid,
case  when p.ID = 1 or p.ID = 2 or p.ID = 5  then 
case  substring(isnull(dispatch,'NNNNNN'),1,1 )  when 'Y' then 1 else 0 end
when p.ID = 3 then 
case substring(isnull(ticket,'NNNNNN'),4,1 )  when 'Y' then 1 else 0 end
when p.ID = 7 or p.ID = 8 then
case substring(isnull(elevator,'NNNNNN'),1,1 )  when 'Y' then 1 else 0 end

when p.ID = 10 or p.ID = 11 then
case substring(isnull(UserS,'NNNNNN'),1,1 )  when 'Y' then 1 else 0 end

 end
from tbluser u
left outer join Emp e  on u.fUser=e.CallSign
inner join tblPages p on 1 = 1 



SET IDENTITY_INSERT [dbo].[Chart] ON 

INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (1, N'D1000', N'Cash in Bank', CAST(0.00 AS Numeric(30, 2)), 6, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1000')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (2, N'D1100', N'Undeposited Funds', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1100')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (3, N'D1200', N'Accounts Receivable', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1200')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (4, N'D2000', N'Accounts Payable', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D2000')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (5, N'D2100', N'Sales Tax Payable', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D2100')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (6, N'D3110', N'Stock', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3110')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (7, N'D3130', N'Current Earnings', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3130')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (8, N'D3140', N'Distribution', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3140')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (9, N'D3920', N'Retained Earnings', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3920')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (10, N'D6000', N'Bank Charges', CAST(0.00 AS Numeric(30, 2)), 5, N'', N'', 0, 1, 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D6000')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (11, N'D9991', N'Purchase Orders', CAST(0.00 AS Numeric(30, 2)), 7, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D9991')

SET IDENTITY_INSERT [dbo].[Chart] OFF




INSERT [dbo].[CType] ([Type]) VALUES (N'Asset')

INSERT [dbo].[CType] ([Type]) VALUES (N'Liability')

INSERT [dbo].[CType] ([Type]) VALUES (N'Equity')

INSERT [dbo].[CType] ([Type]) VALUES (N'Revenue')

INSERT [dbo].[CType] ([Type]) VALUES (N'Cost')

INSERT [dbo].[CType] ([Type]) VALUES (N'Expense')

INSERT [dbo].[CType] ([Type]) VALUES (N'Bank')

INSERT [dbo].[CType] ([Type]) VALUES (N'Non-Posting')

INSERT [dbo].[Status] ([Status]) VALUES (N'Active')

INSERT [dbo].[Status] ([Status]) VALUES (N'Inactive')

INSERT [dbo].[Status] ([Status]) VALUES (N'Hold')



INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AB', N'Alberta', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AK', N'Alaska', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AL', N'Alabama', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AR', N'Arkansas', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'AZ', N'Arizona', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'BC', N'British Columbia', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'CA', N'California', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'CO', N'Colorado', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'CT', N'Connecticut', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'DC', N'Washington DC', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'DE', N'Delaware', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'FL', N'Florida', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'GA', N'Georgia', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'HI', N'Hawaii', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'IA', N'Iowa', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ID', N'Idaho', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'IL', N'Illinois', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'IN', N'Indiana', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'KS', N'Kansas', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'KY', N'Kentucky', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'LA', N'Louisiana', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MA', N'Massachusetts', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MB', N'Manitoba', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MD', N'Maryland', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ME', N'Maine', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MI', N'Michigan', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MN', N'Minnesota', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MO', N'Missouri', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MS', N'Mississippi', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'MT', N'Montana', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NB', N'New Brunswick', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NC', N'North Carolina', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ND', N'North Dakota', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NE', N'Nebraska', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NF', N'Newfoundland', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NH', N'New Hampshire', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NJ', N'New Jersey', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NM', N'New Mexico', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NS', N'Nova Scotia', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NV', N'Nevada', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'NY', N'New York', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'OH', N'Ohio', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'OK', N'Oklahoma', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'ON', N'Ontario', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'OR', N'Oregon', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'PA', N'Pennsylvania', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'PE', N'Prince Edward Is', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'PQ', N'Quebec', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'RI', N'Road Island', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'SC', N'South Carolina', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'SD', N'South Dakota', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'SK', N'Saskatchewan', N'Canada')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'TN', N'Tennessee', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'TX', N'Texas', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'UT', N'Utah', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'VA', N'Virginia', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'VT', N'Vermont', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WA', N'Washington', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WI', N'Wisonsin', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WV', N'West Virginia', N'USA')
INSERT [dbo].[State] ([Name], [fDesc], [Country]) VALUES (N'WY', N'Wyoming', N'USA')
 
 ----- project tables setup -----------------------------------------------------------

 SET IDENTITY_INSERT [dbo].[BOMT] ON 


INSERT [dbo].[BOMT] ([ID], [Type]) VALUES (1, N'Materials')

INSERT [dbo].[BOMT] ([ID], [Type]) VALUES (2, N'Labor')

INSERT [dbo].[BOMT] ([ID], [Type]) VALUES (3, N'Sub-Contract')

INSERT [dbo].[BOMT] ([ID], [Type]) VALUES (4, N'Permit')

INSERT [dbo].[BOMT] ([ID], [Type]) VALUES (5, N'Consulting')

INSERT [dbo].[BOMT] ([ID], [Type]) VALUES (6, N'Equipment Rental')

SET IDENTITY_INSERT [dbo].[BOMT] OFF

SET IDENTITY_INSERT [dbo].[JobCode] ON 

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (1, N'100', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (2, N'200', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (3, N'300', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (4, N'400', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (5, N'500', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (6, N'600', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (7, N'700', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (8, N'800', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (9, N'900', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (10, N'1000', NULL)

INSERT [dbo].[JobCode] ([ID], [Code], [IsDefault]) VALUES (11, N'999', 1)

SET IDENTITY_INSERT [dbo].[JobCode] OFF

INSERT [dbo].[JStatus] ([Status]) VALUES (N'Open')

INSERT [dbo].[JStatus] ([Status]) VALUES (N'Closed')

INSERT [dbo].[JStatus] ([Status]) VALUES (N'Hold')

INSERT [dbo].[JStatus] ([Status]) VALUES (N'Completed')

SET IDENTITY_INSERT [dbo].[OrgDep] ON 


INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (1, N'Finance')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (2, N'Supply Chain')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (3, N'Engineering')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (4, N'Sales')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (5, N'Legal')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (6, N'Design')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (7, N'QA & Inspections')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (8, N'Customer')

INSERT [dbo].[OrgDep] ([ID], [Department]) VALUES (9, N'Vendor')

SET IDENTITY_INSERT [dbo].[OrgDep] OFF

INSERT [dbo].[Posting] ([Post], [ID]) VALUES (N'Real Time', 0)

INSERT [dbo].[Posting] ([Post], [ID]) VALUES (N'On Closing', 1)

INSERT [dbo].[Posting] ([Post], [ID]) VALUES (N'% of completion', 2)

SET IDENTITY_INSERT [dbo].[tblTabs] ON 


INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (1, 28, N'Header')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (2, 28, N'Attributes - General')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (3, 28, N'Attributes - GC Info')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (4, 28, N'Attributes - Equipment')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (5, 28, N'Finance - General')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (6, 28, N'Finance - Billing')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (7, 28, N'Finance - Budgets')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (8, 28, N'Ticketlist')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (9, 28, N'BOM')

INSERT [dbo].[tblTabs] ([ID], [tblPageID], [TabName]) VALUES (10, 28, N'Milestones')

SET IDENTITY_INSERT [dbo].[tblTabs] OFF

SET IDENTITY_INSERT [dbo].[UM] ON 


INSERT [dbo].[UM] ([ID], [fDesc]) VALUES (1, N'Each')

INSERT [dbo].[UM] ([ID], [fDesc]) VALUES (2, N'Hours')

SET IDENTITY_INSERT [dbo].[UM] OFF


  COMMIT TRANSACTION
