CREATE PROCEDURE [dbo].[spCreateInventory]
@Ids xml 

as
	begin
			DECLARE @success int
			set @success=0
			
				create table #tempitems
				(
					Name varchar(30),
				   fDesc varchar(255),
				   Part varchar(50),
				   Status smallint,
				   SAcct int,
				   Measure varchar(10),
				   Tax smallint,
				   Balance numeric(30,2),
				   Price1 numeric(30,4),
				   Price2 numeric(30,4),
				   Price3 numeric(30,4),
				   Price4 numeric(30,4),
				   Price5 numeric(30,4),
				   Remarks varchar(8000),
				   Cat smallint,
				   LVendor int,
				   LCost numeric(30,4),
				   AllowZero smallint,
				   Type smallint,
				   InUse smallint,
				   EN int,
				   Hand numeric(30,2),
				   Aisle varchar(15),
				   fOrder numeric(30,2),
				   Min numeric(30,2),
				   Shelf varchar(15),
				   Bin varchar(15),
				   Requ numeric(30,2),
				   Warehouse varchar(5),
				   Price6 numeric(30,4),
				   Committed numeric(30,4),
				   QBInvID varchar(100),
				   LastUpdateDate datetime default null ,
				   QBAccountID varchar(100),
				   Available numeric(30,4),
				   IssuedOpenJobs numeric(30,4),
				   Description2 varchar(255),
				   Description3 varchar(255),
				   Description4 varchar(255),
				   DateCreated datetime null,				  
				   Specification varchar(255),
				   Specification2 varchar(255),
				   Specification3 varchar(255),
				   Specification4 varchar(255),
				   Revision varchar(255),
				   LastRevisionDate datetime default null,
				   Eco varchar(100),
				   Drawing varchar(255),
				   Reference varchar(255),
				   Length varchar(255),
				   Width varchar(255),
				   Weight varchar(255),
				   InspectionRequired bit,
				   CoCRequired bit,
				   ShelfLife numeric(30,4),
				   SerializationRequired bit,
				   GLcogs varchar(100),
				   GLPurchases varchar(100),
				   ABCClass varchar(100),
				   OHValue numeric(30,4),
				   OOValue numeric(30,4),
				   OverIssueAllowance bit,
				   UnderIssueAllowance bit,
				   InventoryTurns numeric(30,4),
				   MOQ numeric(30,4),
				   MinInvQty numeric(30,4),
				   MaxInvQty numeric(30,4),
				   Commodity varchar(100),
				   LastReceiptDate datetime default null,				  
				   EAU decimal(30,4),
				   EOLDate datetime,
				   WarrantyPeriod INT,
				   PODueDate datetime default null,
				   DefaultReceivingLocation bit,
				   DefaultInspectionLocation bit,
				   LastSalePrice numeric(30,4),
				   AnnualSalesQty numeric(30,4),
				   AnnualSalesAmt numeric(30,4),
				   QtyAllocatedToSO numeric(30,4),
				   MaxDiscountPercentage numeric(30,4),
				 
				   Height varchar(255),
				   UnitCost numeric(30,4),
				   GLSales int,
				   EOQ numeric(30,4),
				   LeadTime int
				)

				insert into #tempitems
				select
			
					   a.b.value('Name[1]', 'varchar(30)'),
					   a.b.value('fDesc[1]','varchar(255)'),
					   a.b.value('Part[1]','varchar(50)'),
					   a.b.value('Status[1]','smallint'),
					   a.b.value('SAcct[1]','int'),
					   a.b.value('Measure[1]','varchar(10)'),
					   a.b.value('Tax[1]','smallint'),
					   a.b.value('Balance[1]','numeric(30,2)'),
					   a.b.value('Price1[1]','numeric(30,4)'),
					   a.b.value('Price2[1]','numeric(30,4)'),
					   a.b.value('Price3[1]','numeric(30,4)'),
					   a.b.value('Price4[1]','numeric(30,4)'),
					   a.b.value('Price5[1]','numeric(30,4)'),
					   a.b.value('Remarks[1]','varchar(8000)'),
					   a.b.value('Cat[1]','smallint'),
					   a.b.value('LVendor[1]','int'),
					   a.b.value('LCost[1]','numeric(30,4)'),
					   a.b.value('AllowZero[1]','smallint'),
					   0,
					   --a.b.value('Type[1]','smallint'),
					   a.b.value('InUse[1]','smallint'),
					   a.b.value('EN[1]','int'),
					   a.b.value('Hand[1]','numeric(30,2)'),
					   a.b.value('Aisle[1]','varchar(15)'),
					   a.b.value('fOrder[1]','numeric(30,2)'),
					   a.b.value('Min[1]','numeric(30,2)'),
					   a.b.value('Shelf[1]','varchar(15)'),
					   a.b.value('Bin[1]','varchar(15)'),
					   a.b.value('Requ[1]','numeric(30,2)'),
					   a.b.value('Warehouse[1]','varchar(5)'),
					   a.b.value('Price6[1]','numeric(30,4)'),
					   a.b.value('Committed[1]','numeric(30,4)'),
					   a.b.value('QBInvID[1]','varchar(100)'),
					case when a.b.value('LastUpdateDate[1]','datetime')='' then null else a.b.value('LastUpdateDate[1]','datetime') end,
					   a.b.value('QBAccountID[1]','varchar(100)'),
					   a.b.value('Available[1]','numeric(30,4)'),
					   a.b.value('IssuedOpenJobs[1]','numeric(30,4)'),
					   a.b.value('Description2[1]','varchar(255)'),
					   a.b.value('Description3[1]','varchar(255)'),
					   a.b.value('Description4[1]','varchar(255)'),
					     case when a.b.value('DateCreated[1]','datetime')='' then null else a.b.value('DateCreated[1]','datetime') end,
					  				  
					   a.b.value('Specification[1]','varchar(255)'),
					   a.b.value('Specification2[1]','varchar(255)'),
					   a.b.value('Specification3[1]','varchar(255)'),
					   a.b.value('Specification4[1]','varchar(255)'),
					   a.b.value('Revision[1]','varchar(255)'),
					   case when a.b.value('LastRevisionDate[1]','datetime')='' then null else a.b.value('LastRevisionDate[1]','datetime') end,					  
					   a.b.value('Eco[1]','varchar(100)'),
					   a.b.value('Drawing[1]','varchar(255)'),
					   a.b.value('Reference[1]','varchar(255)'),
					   a.b.value('Length[1]','varchar(255)'),
					   a.b.value('Width[1]','varchar(255)'),
					   a.b.value('Weight[1]','varchar(255)'),
					   a.b.value('InspectionRequired[1]','bit'),
					   a.b.value('CoCRequired[1]','bit'),
					   a.b.value('ShelfLife[1]','numeric(30,4)'),
					   a.b.value('SerializationRequired[1]','bit'),
					   a.b.value('GLcogs[1]','varchar(100)'),
					   a.b.value('GLPurchases[1]','varchar(100)'),
					   a.b.value('ABCClass[1]','varchar(100)'),
					   a.b.value('OHValue[1]','numeric(30,4)'),
					   a.b.value('OOValue[1]','numeric(30,4)'),
					   a.b.value('OverIssueAllowance[1]','bit'),
					   a.b.value('UnderIssueAllowance[1]','bit'),
					   a.b.value('InventoryTurns[1]','numeric(30,4)'),
					   a.b.value('MOQ[1]','numeric(30,4)'),
					   a.b.value('MinInvQty[1]','numeric(30,4)'),
					   a.b.value('MaxInvQty[1]','numeric(30,4)'),
					   a.b.value('Commodity[1]','varchar(100)'),
					   case when a.b.value('LastReceiptDate[1]','datetime')='' then null else a.b.value('LastReceiptDate[1]','datetime') end,					  			  
					   a.b.value('EAU[1]','decimal(30,4)'),
					   case when a.b.value('EOLDate[1]','datetime')='' then null else a.b.value('EOLDate[1]','datetime') end,	
					   -- case when a.b.value('WarrantyPeriod[1]','datetime')='' then null else a.b.value('WarrantyPeriod[1]','datetime') end,					   
					a.b.value('WarrantyPeriod[1]','INT'),					   
					    case when a.b.value('PODueDate[1]','datetime')='' then null else a.b.value('PODueDate[1]','datetime') end,
					  
					   a.b.value('DefaultReceivingLocation[1]','bit'),
					   a.b.value('DefaultInspectionLocation[1]','bit'),
					   a.b.value('LastSalePrice[1]','numeric(30,4)'),
					   a.b.value('AnnualSalesQty[1]','numeric(30,4)'),
					   a.b.value('AnnualSalesAmt[1]','numeric(30,4)'),
					   a.b.value('QtyAllocatedToSO[1]','numeric(30,4)'),
					   a.b.value('MaxDiscountPercentage[1]','numeric(30,4)'),					  
					   a.b.value('Height[1]','varchar(255)'),
					   a.b.value('UnitCost[1]','numeric(30,4)'),
					   a.b.value('GLSales[1]','int'),
					   a.b.value('EOQ[1]','numeric(30,4)'),
					   a.b.value('LeadTime[1]','int')

			from @Ids.nodes('Inventory/Item')a(b)

			

					if exists(select 1 from #tempitems)
				begin
		
			INSERT INTO [dbo].[Inv]
           ([Name]
           ,[fDesc]
           ,[Part]
           ,[Status]
           ,[SAcct]
           ,[Measure]
           ,[Tax]
           ,[Balance]
           ,[Price1]
           ,[Price2]
           ,[Price3]
           ,[Price4]
           ,[Price5]
           ,[Remarks]
           ,[Cat]
           ,[LVendor]
           ,[LCost]
           ,[AllowZero]
           ,[Type]
           ,[InUse]
           ,[EN]
           ,[Hand]
           ,[Aisle]
           ,[fOrder]
           ,[Min]
           ,[Shelf]
           ,[Bin]
           ,[Requ]
           ,[Warehouse]
           ,[Price6]
           ,[Committed]
           ,[QBInvID]
           ,[LastUpdateDate]
           ,[QBAccountID]
           ,[Available]
           ,[IssuedOpenJobs]
           ,[Description2]
           ,[Description3]
           ,[Description4]
           ,[DateCreated]           
           ,[Specification]
           ,[Specification2]
           ,[Specification3]
           ,[Specification4]
           ,[Revision]
           ,[LastRevisionDate]
           ,[Eco]
           ,[Drawing]
           ,[Reference]
           ,[Length]
           ,[Width]
           ,[Weight]
           ,[InspectionRequired]
           ,[CoCRequired]
           ,[ShelfLife]
           ,[SerializationRequired]
           ,[GLcogs]
           ,[GLPurchases]
           ,[ABCClass]
           ,[OHValue]
           ,[OOValue]
           ,[OverIssueAllowance]
           ,[UnderIssueAllowance]
           ,[InventoryTurns]
           ,[MOQ]
           ,[MinInvQty]
           ,[MaxInvQty]
           ,[Commodity]
           ,[LastReceiptDate]
           
           ,[EAU]
           ,[EOLDate]
           ,[WarrantyPeriod]
           ,[PODueDate]
           ,[DefaultReceivingLocation]
           ,[DefaultInspectionLocation]
           ,[LastSalePrice]
           ,[AnnualSalesQty]
           ,[AnnualSalesAmt]
           ,[QtyAllocatedToSO]
           ,[MaxDiscountPercentage]
          
           ,[Height]
           ,[UnitCost]
           ,[GLSales]
           ,[EOQ]
           ,[LeadTime]
		   )
			(select * from #tempitems)

			end

					set @success=@@IDENTITY

					drop table #tempitems
	

			

		select @success
			

	end
GO