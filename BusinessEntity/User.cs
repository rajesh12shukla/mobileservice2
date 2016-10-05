using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessEntity
{
    public class User
    {
        private string _Username;
        private string _Password;        
        private int _UserID;
        private int _PDA;
        private int _Field;
        private int _Status;
        private string _FirstName;
        private string _LastNAme;
        private string _MiddleName;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Tele;
        private string _Cell;
        private string _Email;
        private DateTime _DtHired;
        private DateTime _DtFired;
        private DataSet _dsUserAuthorization;
        private string _CreateTicket;
        private string _WorkDate;
        private string _LocationRemarks;
        private string _ServiceHist;
        private string _PurchaseOrd;
        private string _Expenses;
        private string _ProgFunctions;
        private string _AccessUser;
        private int _RolId;
        private int _WorkId;
        private int _EmpId;
        private string _SearchBy;
        private string _SearchValue;
        private string _Remarks;
        private int _Schedule;
        private int _Mapping;
        private int _TypeID;
        private string _Country;
        private int _CustomerID;
        private DataTable _ContactData;
        private int _Internet;
        private string _MainContact;
        private string _Phone;
        private string _Website;
        private string _Type;
        private string _AccountNo;
        private string _Locationname;
        private int _Route;
        private int _Territory;
        private string _RolAddress;
        private string _RolCity;
        private string _RolState;
        private string _RolZip;
        private string _Fax;
        private int _LocID;
        private string _ConnConfig;
        private string _MSM;
        private string _DSN;
        private string _DBName;
        private string _Script;
        private int ctrlID;
        private string _DeviceID;
        private DateTime _Edate;
        private string _FieldEmp;
        private string _Pager;
        private string _ContactName;
        private string _Supervisor;
        private string _CustomerType;
        private int _Salesperson;
        private string _Reg;
        private string _MAPAddress;
        private string _EquipType;
        private string _ServiceType;
        private string _Manufacturer;
        private string _ServiceDate;
        private string _InstallDate;
        private string _Price;
        private string _Unit;
        private string _Cat;
        private string _Serial;
        private string _UniqueID;
        private DateTime _InstallDateTime;
        private DateTime _LastServiceDate;
        private double _EquipPrice;
        private int _EquipID;
        private string _UserLic;
        private int _UserLicID;
        private string _SalesTax;
        private string _SalesDescription;
        private double _SalesRate;
        private int _CatStatus;
        private double _Balance;
        private string _Measure;
        private int _BillCode;
        private string _Stax;
        private int _IsSuper;
        private string _Lat;
        private string _Lng;
        private string _WarehouseID;
        private string _WarehouseName;
        private byte[] _Logo;
        private string _Custom1;
        private string _Custom2;
        private string _ToMail;
        private string _CCMail;
        private int _JobtypeID;
        private string _MailToInv;
        private string _MailCCInv;
        private string _QBCustomerID;
        private string _QBlocationID;
        private DateTime _LastUpdateDate;
        private string _QBCustomerTypeID;
        private string _QBSalesTaxID;
        private int _CustWeb;
        private string _WorkOrder;
        private string _QBPath;
        private int _Default;
        private string _Category;
        private int _DiagnosticType;
        private int _DepartmentID;
        private string _Lang;
        private int _multiLang;
        private DateTime _InstallDateimport;
        private string _Description;
        private int _QBInteg;
        private string _MerchantID;
        private string _LoginID;
        private string _PaymentUser;
        private string _PaymentPass;
        private int _MerchantInfoId;
        private int _IsTaxable;
        private string _IsTS;
        private string _QBJobtypeID;
        private string _QBInvID;
        private string _QBTermsID;
        private int _TermsID;
        private string _QBAccountID;
        private int _AccountID;
        private string _QBEmployeeID;
        private int _DefaultWorker;
        private int _EmailMS;
        private int _QBFirstSync;
        private DataTable _dtItems;
        private DataTable _dtItemsDeleted;
        private string _Dispatch;
        private int _REPtemplateID;
        public int _Mode;
        private int _ItemsOnly;
        private string _Code;
        private int _YE;
        private int _FChart;
        private int _FGLAdj;
        private int _FDeposit;
        private int _FCustomerPayment;
        private int _FinanStatement;

        private int _addChart;
        private int _editChart;
        private int _viewChart;
        private int _addGLAdj;
        private int _editGLAdj;
        private int _viewGLAdj;
        private int _addDeposit;
        private int _editDeposit;
        private int _viewDeposit;
        private int _addCustomerPayment;
        private int _editCustomerPayment;
        private int _viewCustomerPayment;

        private int _APVendor;
        private int _APBill;
        private int _APBillSelect;
        private int _APBillPay;

        private DateTime _fStart;
        private DateTime _fEnd;
        private DataSet _ds;
        private Int16 _contractBill;        //added by Mayuri 24th dec,15
        private string _gstReg;
        private Int16 _stype;
        private string _PSTReg;
        private int _PageID;
        public double BillRate;
        public double RateOT;
        public double RateNT;
        public double RateDT;
        public double RateTravel;
        public double RateMileage;
        public int JobId;
        public int PageID
        {
            get { return _PageID; }
            set { _PageID = value; }
        }
       
        private string _TermsConditions;
        public string TermsConditions
        {
            get { return _TermsConditions; }
            set { _TermsConditions = value; }
        }
        public DataTable dtDocs { get; set; }

        public int WageID { get; set; }
        public string WageName { get; set; }
        public bool Chargeable { get; set; }
        public Int16 sType
        {
            get { return _stype; }
            set { _stype = value; }
        }
        public string PSTReg
        {
            get { return _PSTReg; }
            set { _PSTReg = value; }
        }
        public string GSTReg
        {
            get { return _gstReg; }
            set { _gstReg = value; }
        }
        public Int16 ContractBill
        {
            get { return _contractBill; }
            set { _contractBill = value; }
        }

        private int _billing; //added by komal 12-23-2015
        private int _Central; //added by komal 12-23-2015

        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public int APVendor
        {
            get { return _APVendor; }
            set { _APVendor = value; }
        }
        public int APBill
        {
            get { return _APBill; }
            set { _APBill = value; }
        }
        public int APBillSelect
        {
            get { return _APBillSelect; }
            set { _APBillSelect = value; }
        }
        public int APBillPay
        {
            get { return _APBillPay; }
            set { _APBillPay = value; }
        }

        public DateTime FStart
        {
            get { return _fStart; }
            set { _fStart = value; }
        }
        public DateTime FEnd
        {
            get { return _fEnd; }
            set { _fEnd = value; }
        }
        public int FChart 
        {
            get { return _FChart; }
            set { _FChart = value; }
        }
        public int FGLAdj
        {
            get { return _FGLAdj; }
            set { _FGLAdj = value; }
        }
        public int FinanStatement
        {
            get { return _FinanStatement; }
            set { _FinanStatement = value; }
        }
        public int AddChart
        {
            get { return _addChart; }
            set { _addChart = value; }
        }
        public int EditChart
        {
            get { return _editChart; }
            set { _editChart = value; }
        }
        public int ViewChart
        {
            get { return _viewChart; }
            set { _viewChart = value; }
        }
        public int AddGLAdj
        {
            get { return _addGLAdj; }
            set { _addGLAdj = value; }
        }
        public int EditGLAdj
        {
            get { return _editGLAdj; }
            set { _editGLAdj = value; }
        }
        public int ViewGLAdj
        {
            get { return _viewGLAdj; }
            set { _viewGLAdj = value; }
        }
        public int FDeposit
        {
            get { return _FDeposit; }
            set { _FDeposit = value; }
        }
        public int AddDeposit
        {
            get { return _addDeposit; }
            set { _addDeposit = value; }
        }
        public int EditDeposit
        {
            get { return _editDeposit; }
            set { _editDeposit = value; }
        }
        public int ViewDeposit
        {
            get { return _viewDeposit; }
            set { _viewDeposit = value; }
        }
        public int FCustomerPayment
        {
            get { return _FCustomerPayment; }
            set { _FCustomerPayment = value; }
        }
        public int AddCustomerPayment
        {
            get { return _addCustomerPayment; }
            set { _addCustomerPayment = value; }
        }
        public int EditCustomerPayment
        {
            get { return _editCustomerPayment; }
            set { _editCustomerPayment = value; }
        }
        public int ViewCustomerPayment
        {
            get { return _viewCustomerPayment; }
            set { _viewCustomerPayment = value; }
        }
        public int saved { get; set; }

        public int unsaved { get; set; }
        public DataTable dtTicketData { get; set; }
        public int PayMethod { get; set; }
        public double PHours { get; set; }
        public double Salary { get; set; }
        public string Department { get; set; }
        public int RoleID { get; set; }
        public int TransferTimeSheet { get; set; }
        public int TransferInvoice { get; set; }
        public string QbserviceItemlabor { get; set; }
        public string QBserviceItemExp { get; set; }
        public DataTable EmpData { get; set; }
        public string MOMUSer { get; set; }
        public string  MOMPASS { get; set; }
        public DateTime Startdt { get; set; }
        public DateTime Enddt { get; set; }
        public string SageLocID { get; set; }
        public string SageCustID { get; set; }

        public DataTable dtGroupdata { get; set; }

        public int MassReview { get; set; }

        public int ProspectID { get; set; }

        public int IsTSDatabase { get; set; }

        public string InServer { get; set; }

        public string InUsername { get; set; }

        public string InPassword { get; set; }

        public int InPort { get; set; }

        public string OutServer { get; set; }

        public string OutUsername { get; set; }

        public string OutPassword { get; set; }

        public int OutPort { get; set; }

        public bool SSL { get; set; }

        public int EmailAccount { get; set; }

        public double HourlyRate { get; set; }

        public int EmpMaintenance { get; set; }

        public int Timestampfix { get; set; }

        public string EmpRefID { get; set; }

        public int PayPeriod { get; set; }

        public double MileageRate { get; set; }

        private string _QBWageID;

        public int AddEquip{ get; set; }

        public int EditEquip { get; set; }

        public int CustomTemplateID { get; set; }

        public DataTable dtcustom { get; set; }

        public DataTable dtCustomValues { get; set; }

        public int InvID { get; set; }
        public string QBWageID
        {
            get { return _QBWageID; }
            set { _QBWageID = value; }
        }

        private double _SalesAmount;

        public double SalesAmount
        {
            get { return _SalesAmount; }
            set { _SalesAmount = value; }
        }

        private double _AnnualAmount;

        public double AnnualAmount
        {
            get { return _AnnualAmount; }
            set { _AnnualAmount = value; }
        }

        private int _Month;

        public int Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        private int _SalesMgr;

        public int SalesMgr
        {
            get { return _SalesMgr; }
            set { _SalesMgr = value; }
        }

        private string _StartDate;

        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private string _EndDate;

        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private string _CreditReason;

        public string CreditReason
        {
            get { return _CreditReason; }
            set { _CreditReason = value; }
        }
        private int _CreditHold;

        public int CreditHold
        {
            get { return _CreditHold; }
            set { _CreditHold = value; }
        }

        private int _DispAlert;

        public int DispAlert
        {
            get { return _DispAlert; }
            set { _DispAlert = value; }
        }
        
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public int ItemsOnly
        {
            get { return _ItemsOnly; }
            set { _ItemsOnly = value; }
        }

        public int Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public int REPtemplateID
        {
            get { return _REPtemplateID; }
            set { _REPtemplateID = value; }
        }

        public string Dispatch
        {
            get { return _Dispatch; }
            set { _Dispatch = value; }
        }

        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }

        public DataTable DtItemsDeleted
        {
            get { return _dtItemsDeleted; }
            set { _dtItemsDeleted = value; }
        }

        public int QBFirstSync
        {
            get { return _QBFirstSync; }
            set { _QBFirstSync = value; }
        }

        public int EmailMS
        {
            get { return _EmailMS; }
            set { _EmailMS = value; }
        }

        public int DefaultWorker
        {
            get { return _DefaultWorker; }
            set { _DefaultWorker = value; }
        }

        public string QBEmployeeID
        {
            get { return _QBEmployeeID; }
            set { _QBEmployeeID = value; }
        }

        public int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        public string QBAccountID
        {
            get { return _QBAccountID; }
            set { _QBAccountID = value; }
        }

        public int TermsID
        {
            get { return _TermsID; }
            set { _TermsID = value; }
        }

        public string QBTermsID
        {
            get { return _QBTermsID; }
            set { _QBTermsID = value; }
        }

        public string QBInvID
        {
            get { return _QBInvID; }
            set { _QBInvID = value; }
        }
        
        public string QBJobtypeID
        {
            get { return _QBJobtypeID; }
            set { _QBJobtypeID = value; }
        }

        public string IsTS
        {
            get { return _IsTS; }
            set { _IsTS = value; }
        }

        public int IsTaxable
        {
            get { return _IsTaxable; }
            set { _IsTaxable = value; }
        }

        public int MerchantInfoId
        {
            get { return _MerchantInfoId; }
            set { _MerchantInfoId = value; }
        }

        public DataTable dtPageData { get; set; }

        public string PaymentPass
        {
            get { return _PaymentPass; }
            set { _PaymentPass = value; }
        }

        public string PaymentUser
        {
            get { return _PaymentUser; }
            set { _PaymentUser = value; }
        }

        public string LoginID
        {
            get { return _LoginID; }
            set { _LoginID = value; }
        }

        public string MerchantID
        {
            get { return _MerchantID; }
            set { _MerchantID = value; }
        }

        public int QBInteg
        {
            get { return _QBInteg; }
            set { _QBInteg = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public DateTime InstallDateimport
        {
            get { return _InstallDateimport; }
            set { _InstallDateimport = value; }
        }

        public int MultiLang
        {
            get { return _multiLang; }
            set { _multiLang = value; }
        }

        public string Lang
        {
            get { return _Lang; }
            set { _Lang = value; }
        }

        public int DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }

        public int DiagnosticType
        {
            get { return _DiagnosticType; }
            set { _DiagnosticType = value; }
        }


        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public int Default
        {
            get { return _Default; }
            set { _Default = value; }
        }

        public string QBPath
        {
            get { return _QBPath; }
            set { _QBPath = value; }
        }

        public string WorkOrder
        {
            get { return _WorkOrder; }
            set { _WorkOrder = value; }
        }

        public int CustWeb
        {
            get { return _CustWeb; }
            set { _CustWeb = value; }
        }

        public string QBSalesTaxID
        {
            get { return _QBSalesTaxID; }
            set { _QBSalesTaxID = value; }
        }

        public string QBCustomerTypeID
        {
            get { return _QBCustomerTypeID; }
            set { _QBCustomerTypeID = value; }
        }

        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { _LastUpdateDate = value; }
        }

        public string QBlocationID
        {
            get { return _QBlocationID; }
            set { _QBlocationID = value; }
        }

        public string QBCustomerID
        {
            get { return _QBCustomerID; }
            set { _QBCustomerID = value; }
        }

        public string MailCCInv
        {
            get { return _MailCCInv; }
            set { _MailCCInv = value; }
        }

        public string MailToInv
        {
            get { return _MailToInv; }
            set { _MailToInv = value; }
        }

        public int JobtypeID
        {
            get { return _JobtypeID; }
            set { _JobtypeID = value; }
        }

        public string CCMail
        {
            get { return _CCMail; }
            set { _CCMail = value; }
        }

        public string ToMail
        {
            get { return _ToMail; }
            set { _ToMail = value; }
        }

        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }

        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }

        public byte[] Logo
        {
            get { return _Logo; }
            set { _Logo = value; }
        }

        public string WarehouseName
        {
            get { return _WarehouseName; }
            set { _WarehouseName = value; }
        }

        public string WarehouseID
        {
            get { return _WarehouseID; }
            set { _WarehouseID = value; }
        }

        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }

        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        public int IsSuper
        {
            get { return _IsSuper; }
            set { _IsSuper = value; }
        }

        public string Stax
        {
            get { return _Stax; }
            set { _Stax = value; }
        }

        public int BillCode
        {
            get { return _BillCode; }
            set { _BillCode = value; }
        }

        public string Measure
        {
            get { return _Measure; }
            set { _Measure = value; }
        }

        public double Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }

        public int CatStatus
        {
            get { return _CatStatus; }
            set { _CatStatus = value; }
        }
                
        public double SalesRate
        {
            get { return _SalesRate; }
            set { _SalesRate = value; }
        }

        public string SalesDescription
        {
            get { return _SalesDescription; }
            set { _SalesDescription = value; }
        }

        public string SalesTax
        {
            get { return _SalesTax; }
            set { _SalesTax = value; }
        }

        public int UserLicID
        {
            get { return _UserLicID; }
            set { _UserLicID = value; }
        }

        public string UserLic
        {
            get { return _UserLic; }
            set { _UserLic = value; }
        }

        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }

        public double EquipPrice
        {
            get { return _EquipPrice; }
            set { _EquipPrice = value; }
        }

        public DateTime LastServiceDate
        {
            get { return _LastServiceDate; }
            set { _LastServiceDate = value; }
        }

        public DateTime InstallDateTime
        {
            get { return _InstallDateTime; }
            set { _InstallDateTime = value; }
        }

        public string UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }

        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }

        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }

        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        public string Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        public string PriceString { get; set; }

        public string InstallDate
        {
            get { return _InstallDate; }
            set { _InstallDate = value; }
        }

        public string InstallDateString { get; set; }

        public string ServiceDate
        {
            get { return _ServiceDate; }
            set { _ServiceDate = value; }
        }

        public string ServiceDateString { get; set; }

        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }

        public string ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }

        public string EquipType
        {
            get { return _EquipType; }
            set { _EquipType = value; }
        }

        public string MAPAddress
        {
            get { return _MAPAddress; }
            set { _MAPAddress = value; }
        }

        public string Reg
        {
            get { return _Reg; }
            set { _Reg = value; }
        }

        public int Salesperson
        {
            get { return _Salesperson; }
            set { _Salesperson = value; }
        }

        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }

        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; }
        }

        public string ContactName
        {
            get { return _ContactName; }
            set { _ContactName = value; }
        }

        public string Pager
        {
            get { return _Pager; }
            set { _Pager = value; }
        }
       
        public string FieldEmp
        {
            get { return _FieldEmp; }
            set { _FieldEmp = value; }
        }

        public DateTime Edate
        {
            get { return _Edate; }
            set { _Edate = value; }
        }

        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }

        public int CtrlID
        {
            get { return ctrlID; }
            set { ctrlID = value; }
        }

        public string Script
        {
            get { return _Script; }
            set { _Script = value; }
        }

        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }

        public string DSN
        {
            get { return _DSN; }
            set { _DSN = value; }
        }

        public string MSM
        {
            get { return _MSM; }
            set { _MSM = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
             

        public string RolZip
        {
            get { return _RolZip; }
            set { _RolZip = value; }
        }

        public string RolState
        {
            get { return _RolState; }
            set { _RolState = value; }
        }

        public string RolCity
        {
            get { return _RolCity; }
            set { _RolCity = value; }
        }

        public string RolAddress
        {
            get { return _RolAddress; }
            set { _RolAddress = value; }
        }
        
        public int Territory
        {
            get { return _Territory; }
            set { _Territory = value; }
        }

        public int Route
        {
            get { return _Route; }
            set { _Route = value; }
        }

        public string Locationname
        {
            get { return _Locationname; }
            set { _Locationname = value; }
        }

        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int sAcct { get; set; }
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }

        public int Internet
        {
            get { return _Internet; }
            set { _Internet = value; }
        }
        public string QBAccountNumber { get; set; }
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        public int Cust { get; set; }

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        public int TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

        public int Mapping
        {
            get { return _Mapping; }
            set { _Mapping = value; }
        }

        public int Schedule
        {
            get { return _Schedule; }
            set { _Schedule = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }

        public int EmpId
        {
            get { return _EmpId; }
            set { _EmpId = value; }
        }

        public int WorkId
        {
            get { return _WorkId; }
            set { _WorkId = value; }
        }

        public int RolId
        {
            get { return _RolId; }
            set { _RolId = value; }
        }

        public string AccessUser
        {
            get { return _AccessUser; }
            set { _AccessUser = value; }
        }

        public string ProgFunctions
        {
            get { return _ProgFunctions; }
            set { _ProgFunctions = value; }
        }

        public string Expenses
        {
            get { return _Expenses; }
            set { _Expenses = value; }
        }

        public string PurchaseOrd
        {
            get { return _PurchaseOrd; }
            set { _PurchaseOrd = value; }
        }

        public string ServiceHist
        {
            get { return _ServiceHist; }
            set { _ServiceHist = value; }
        }

        public string LocationRemarks
        {
            get { return _LocationRemarks; }
            set { _LocationRemarks = value; }
        }

        public string WorkDate
        {
            get { return _WorkDate; }
            set { _WorkDate = value; }
        }

        public string CreateTicket
        {
            get { return _CreateTicket; }
            set { _CreateTicket = value; }
        }

        public DateTime DtFired
        {
            get { return _DtFired; }
            set { _DtFired = value; }
        }

        public DateTime DtHired
        {
            get { return _DtHired; }
            set { _DtHired = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        public string Tele
        {
            get { return _Tele; }
            set { _Tele = value; }
        }

        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }

        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }

        public string LastNAme
        {
            get { return _LastNAme; }
            set { _LastNAme = value; }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public int Field
        {
            get { return _Field; }
            set { _Field = value; }
        }


        public int PDA
        {
            get { return _PDA; }
            set { _PDA = value; }
        }

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public int Billing
        {
            get { return _billing; }
            set { _billing = value; }
        }
         public int Central
        {
            get { return _Central; }
            set { _Central = value; }
        }
        
        public string PageName { get; set; }
        public int grpbyWO { get; set; }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
              

        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public int YE 
        { 
            get { return _YE; } 
            set { _YE = value; } 
        }

        public int GLAccount { get; set; }
        public int UType { get; set; }

        public short openticket { get; set; }
    }

}
