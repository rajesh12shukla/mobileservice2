using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace BusinessEntity
{

    public class Inventory
    {
        #region ::StoreProc::
        public static string GET_ALL_INVENTORY = "spGetInventory";
        public static string GET_ALL_INVENTORY_BY_ID = "spGetInventoryDetailsById";
        public static string GET_ALL_ITEM_QUANTITY = "spGetItemQuantity";
        public static string GET_ALL_ITEM_PURCHASE_ORDER = "spGetItemPurchaseOrder";
        public static string GET_DELETE_INVENTORY_BULK = "spDeleteInventoryInBulk";
        public static string CREATE_INVENTORY_XML = "spCreateInventory";
        public static string UPDATE_INVENTORY_XML = "spUpdateInventory";
        public static string CREATE_INVENTORY_MANUFACTURER_INFORMATION = "spCreateInventoryManufacturerInformation";
        public static string DELETE_INVENTORY_MANUFACTURER_INFORMATION = "spDeleteInventoryManufacturerInformation";
        public static string UPDATE_INVENTORY_MANUFACTURER_INFORMATION = "spUpdateInventoryManufacturerInformation";
        public static string GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID = "spReadInventoryManufacturerInformationByInventoryId";
        public static string GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID_APPROVEDVENDOR = "spReadInveManufacturerInfoByInvIdAndApprovedVendor";

        #endregion

        #region ::Enum::
        #endregion


        #region ::Private Property Variable Declaration::

        private int _ID;
        private string _Name;
        private string _fDesc;
        private string _Part;
        private int _Status;
        private int _SAcct;
        private string _Measure;
        private int _Tax;
        private decimal _Balance;
        private decimal _Price1;
        private decimal _Price2;
        private decimal _Price3;
        private decimal _Price4;
        private decimal _Price5;
        private string _Remarks;
        private int _Cat;
        private int _LVendor;
        private decimal _LCost;
        private int _AllowZero;
        private int _Type;
        private int _InUse;
        private int _EN;
        private decimal _Hand;
        private string _Aisle;
        private decimal _fOrder;
        private decimal _Min;
        private string _Shelf;
        private string _Bin;
        private decimal _Requ;
        private string _Warehouse;
        private decimal _Price6;
        private decimal _Committed;
        private string _QBInvID;
        private DateTime? _LastUpdateDate;
        private string _QBAccountID;
        private decimal _Available;
        private decimal _IssuedOpenJobs;
        private string _Description2;
        private string _Description3;
        private string _Description4;
        private DateTime? _DateCreated;
        private string _Class;
        private string _Specification;
        private string _Specification2;
        private string _Specification3;
        private string _Specification4;
        private string _Revision;
        private DateTime? _LastRevisionDate;
        private string _Eco;
        private string _Drawing;
        private string _Reference;
        private string _Length;
        private string _Width;
        private string _Weight;
        private string _Height;
        private bool _InspectionRequired;
        private bool _CoCRequired;
        private decimal _ShelfLife;
        private bool _SerializationRequired;
        private string _GLcogs;
        private string _GLSales;
        private string _GLPurchases;
        private string _ABCClass;
        private decimal _OHValue;
        private decimal _OOValue;
        private bool _OverIssueAllowance;
        private bool _UnderIssueAllowance;
        private decimal _InventoryTurns;
        private decimal _MOQ;
        private decimal _EOQ;
        private decimal _MinInvQty;
        private decimal _MaxInvQty;
        private string _Commodity;
        private DateTime? _LastReceiptDate;
        private string _MPN;
        private string _ApprovedManufacturer;
        private string _ApprovedVendor;
        private decimal _EAU;
        private DateTime? _EOLDate;
        private int? _WarrantyPeriod;
        private DateTime? _PODueDate;
        private bool _DefaultReceivingLocation;
        private bool _DefaultInspectionLocation;
        private decimal _LastSalePrice;
        private decimal _AnnualSalesQty;
        private decimal _AnnualSalesAmt;
        private decimal _QtyAllocatedToSO;
        private decimal _MaxDiscountPercentage;
        private decimal _UnitCost;
        private int _LeadTime;
        private DateTime? _DateLastPurchase;

        #endregion

        #region::Public Property Declaration::

        public DataSet Ds;
        public string ConnConfig;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string fDesc { get { return _fDesc; } set { _fDesc = value; } }
        public string Part { get { return _Part; } set { _Part = value; } }
        public int Status { get { return _Status; } set { _Status = value; } }

        public int SAcct { get { return _SAcct; } set { _SAcct = value; } }

        public string Measure { get { return _Measure; } set { _Measure = value; } }
        public int Tax { get { return _Tax; } set { _Tax = value; } }
        public decimal Balance { get { return _Balance; } set { _Balance = value; } }

        public decimal Price1 { get { return _Price1; } set { _Price1 = value; } }
        public decimal Price2 { get { return _Price2; } set { _Price2 = value; } }
        public decimal Price3 { get { return _Price3; } set { _Price3 = value; } }
        public decimal Price4 { get { return _Price4; } set { _Price4 = value; } }
        public decimal Price5 { get { return _Price5; } set { _Price5 = value; } }
        public string Remarks { get { return _Remarks; } set { _Remarks = value; } }
        public int Cat { get { return _Cat; } set { _Cat = value; } }
        public int LVendor { get { return _LVendor; } set { _LVendor = value; } }
        public decimal LCost { get { return _LCost; } set { _LCost = value; } }
        public int AllowZero { get { return _AllowZero; } set { _AllowZero = value; } }
        //public int Type { get { return _Type; } set { _Type = value; } }
        public int InUse { get { return _InUse; } set { _InUse = value; } }
        public int EN { get { return _EN; } set { _EN = value; } }
        public decimal Hand { get { return _Hand; } set { _Hand = value; } }
        public string Aisle { get { return _Aisle; } set { _Aisle = value; } }
        public decimal fOrder { get { return _fOrder; } set { _fOrder = value; } }
        public decimal Min { get { return _Min; } set { _Min = value; } }
        public string Shelf { get { return _Shelf; } set { _Shelf = value; } }
        public string Bin { get { return _Bin; } set { _Bin = value; } }
        public decimal Requ { get { return _Requ; } set { _Requ = value; } }
        public string Warehouse { get { return _Warehouse; } set { _Warehouse = value; } }
        public decimal Price6 { get { return _Price6; } set { _Price6 = value; } }
        public decimal Committed { get { return _Committed; } set { _Committed = value; } }
        public string QBInvID { get { return _QBInvID; } set { _QBInvID = value; } }
        public DateTime? LastUpdateDate { get { return _LastUpdateDate; } set { _LastUpdateDate = value; } }
        public string QBAccountID { get { return _QBAccountID; } set { _QBAccountID = value; } }
        public decimal Available { get { return _Available; } set { _Available = value; } }
        public decimal IssuedOpenJobs { get { return _IssuedOpenJobs; } set { _IssuedOpenJobs = value; } }
        public string Description2 { get { return _Description2; } set { _Description2 = value; } }
        public string Description3 { get { return _Description3; } set { _Description3 = value; } }
        public string Description4 { get { return _Description4; } set { _Description4 = value; } }
        public DateTime? DateCreated { get { return _DateCreated; } set { _DateCreated = value; } }
        public string Class { get { return _Class; } set { _Class = value; } }
        public string Specification { get { return _Specification; } set { _Specification = value; } }
        public string Specification2 { get { return _Specification2; } set { _Specification2 = value; } }
        public string Specification3 { get { return _Specification3; } set { _Specification3 = value; } }
        public string Specification4 { get { return _Specification4; } set { _Specification4 = value; } }
        public string Revision { get { return _Revision; } set { _Revision = value; } }
        public DateTime? LastRevisionDate { get { return _LastRevisionDate; } set { _LastRevisionDate = value; } }
        public string Eco { get { return _Eco; } set { _Eco = value; } }
        public string Drawing { get { return _Drawing; } set { _Drawing = value; } }
        public string Reference { get { return _Reference; } set { _Reference = value; } }
        public string Length { get { return _Length; } set { _Length = value; } }
        public string Width { get { return _Width; } set { _Width = value; } }
        public string Weight { get { return _Weight; } set { _Weight = value; } }
        public string Height { get { return _Height; } set { _Height = value; } }
        public bool InspectionRequired { get { return _InspectionRequired; } set { _InspectionRequired = value; } }
        public bool CoCRequired { get { return _CoCRequired; } set { _CoCRequired = value; } }
        public decimal ShelfLife { get { return _ShelfLife; } set { _ShelfLife = value; } }
        public bool SerializationRequired { get { return _SerializationRequired; } set { _SerializationRequired = value; } }
        public string GLSales { get { return _GLSales; } set { _GLSales = value; } }
        public string GLcogs { get { return _GLcogs; } set { _GLcogs = value; } }
        public string GLPurchases { get { return _GLPurchases; } set { _GLPurchases = value; } }
        public string ABCClass { get { return _ABCClass; } set { _ABCClass = value; } }
        public decimal OHValue { get { return _OHValue; } set { _OHValue = value; } }
        public decimal OOValue { get { return _OOValue; } set { _OOValue = value; } }
        public bool OverIssueAllowance { get { return _OverIssueAllowance; } set { _OverIssueAllowance = value; } }
        public bool UnderIssueAllowance { get { return _UnderIssueAllowance; } set { _UnderIssueAllowance = value; } }
        public decimal InventoryTurns { get { return _InventoryTurns; } set { _InventoryTurns = value; } }
        public decimal MOQ { get { return _MOQ; } set { _MOQ = value; } }

        public decimal EOQ { get { return _EOQ; } set { _EOQ = value; } }
        public decimal MinInvQty { get { return _MinInvQty; } set { _MinInvQty = value; } }
        public decimal MaxInvQty { get { return _MaxInvQty; } set { _MaxInvQty = value; } }
        public string Commodity { get { return _Commodity; } set { _Commodity = value; } }
        public DateTime? LastReceiptDate { get { return _LastReceiptDate; } set { _LastReceiptDate = value; } }
        public string MPN { get { return _MPN; } set { _MPN = value; } }
        public string ApprovedManufacturer { get { return _ApprovedManufacturer; } set { _ApprovedManufacturer = value; } }
        public string ApprovedVendor { get { return _ApprovedVendor; } set { _ApprovedVendor = value; } }
        public decimal EAU { get { return _EAU; } set { _EAU = value; } }
        public DateTime? EOLDate { get { return _EOLDate; } set { _EOLDate = value; } }
        public int? WarrantyPeriod { get { return _WarrantyPeriod; } set { _WarrantyPeriod = value; } }
        public DateTime? PODueDate { get { return _PODueDate; } set { _PODueDate = value; } }
        public bool DefaultReceivingLocation { get { return _DefaultReceivingLocation; } set { _DefaultReceivingLocation = value; } }
        public bool DefaultInspectionLocation { get { return _DefaultInspectionLocation; } set { _DefaultInspectionLocation = value; } }
        public decimal LastSalePrice { get { return _LastSalePrice; } set { _LastSalePrice = value; } }
        public decimal AnnualSalesQty { get { return _AnnualSalesQty; } set { _AnnualSalesQty = value; } }
        public decimal AnnualSalesAmt { get { return _AnnualSalesAmt; } set { _AnnualSalesAmt = value; } }
        public decimal QtyAllocatedToSO { get { return _QtyAllocatedToSO; } set { _QtyAllocatedToSO = value; } }
        public decimal MaxDiscountPercentage { get { return _MaxDiscountPercentage; } set { _MaxDiscountPercentage = value; } }
        public decimal UnitCost { get { return _UnitCost; } set { _UnitCost = value; } }
        public int LeadTime { get { return _LeadTime; } set { _LeadTime = value; } }
        public DateTime? DateLastPurchase { get { return _DateLastPurchase; } set { _DateLastPurchase = value; } }
        public InventoryManufacturerInformation[] ApprovedVendors
        {
            get;
            set;
        }
        #endregion


    }

    public class InventoryManufacturerInformation
    {
        #region ::Private Property Variable Declaration::
        private int _ID;
        private int _InventoryID;
        private string _MPN;
        private string _ApprovedManufacturer;
        private string _ApprovedVendor;
        public string _ApprovedVendorId;
        public string _ApprovedVendorEmail;
        #endregion

        #region::Public Property Declaration::
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public int InventoryID
        {
            get
            {
                return _InventoryID;
            }
            set
            {
                _InventoryID = value;
            }
        }
        public string MPN
        {
            get
            {
                return _MPN;
            }
            set
            {
                _MPN = value;
            }
        }
        public string ApprovedManufacturer
        {
            get
            {
                return _ApprovedManufacturer;
            }
            set
            {
                _ApprovedManufacturer = value;
            }
        }
        public string ApprovedVendor
        {
            get
            {
                return _ApprovedVendor;
            }
            set
            {
                _ApprovedVendor = value;
            }
        }
        public string ApprovedVendorId
        {
            get
            {
                return _ApprovedVendorId;
            }
            set
            {
                _ApprovedVendorId = value;
            }
        }

        public string ApprovedVendorEmail
        {
            get
            {
                return _ApprovedVendorEmail;
            }
            set
            {
                _ApprovedVendorEmail = value;
            }
        }
        #endregion
    }


    public sealed class InventorySearchConditions
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string MappingColumn { get; set; }
    }

    public class Itype : Inventory
    {

        #region ::StoreProc::
        public static string GET_ALL_ITYPE = "spGetItype";

        #endregion

        #region ::Table Mapping Name::
        public static string tblMappingID = "ID";
        public static string tblMappingType = "Type";

        #endregion


        private int _ID;
        private string _Type;
        private int _Count;
        private string _Remarks;


        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string Type
        {

            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        //private int _Count;
        //private string _Remarks;


    }

    public class RequestQuoteVendorJSON
    {
        public List<RequestQuoteManufacutrerJSON> ManufacturerInfo { get; set; }

        public string Email { get; set; }
    }

    public class RequestQuoteManufacutrerJSON
    {
        public int Vendorid { get; set; }
        public string MPN { get; set; }
        public string Manufacturer { get; set; }
        public int InventoryManufacturerInformationId { get; set; }

    }
}
