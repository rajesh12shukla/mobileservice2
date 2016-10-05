using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessEntity
{
    public class Customer
    {
        public int ProjectJobID { get; set; }
        private DataSet _DsCustomer;
        private string _ConnConfig;
        private int _ProspectID;
        private string _Name;
        private string _City;
       
        private string _State;
        private string _Zip;
        private string _Phone;
        private string _Cellular;
        private string _Contact;
        private string _Remarks;
        private string _Type;
        private int _Status;
        private string _Email;
        private int _Mode;
        private int _LocID;
        private string _LocIDs;
        private int _Worker;
        private string _RouteSequence;
        private int _TemplateID;
        private DataTable _dtWorkerData;
        private string _SearchValue;
        private string _Center;
        private string _Radius;
        private string _Overlay;
        private string _PolygonCoord;
        public int WageID { get; set; }
        public string LocationRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CustomerID { get; set; }
        public int RoleID { get; set; }
        public int ticketID { get; set; }
        public DataTable dtItems { get; set; }
        public DataTable dtLaborItems { get; set; }
        public DataTable dtLaborItemsEstimate { get; set; }
        private int _Close;
        public int BucketID { get; set; }
        public double CADExchange { get; set; }
        public int IsItemEdited { get; set; }
        public int ItemID { get; set; }
        public Int16 JobType { get; set; }
        public double Balance { get; set; }
        public int Ctype { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public string PO { get; set; }
        public string SO { get; set; }
        public Int16 Certified { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public DateTime Custom5 { get; set; }
        public string ctypeName { get; set; }
        public int InvExp { get; set; }
        public int InvServ { get; set; }
        public int Wage { get; set; }
        public int GLInt { get; set; }
        public string JobTempCtype { get; set; }
        public Int16 Post { get; set; }
        public Int16 Charge { get; set; }
        public Int16 JobClose { get; set; }
        public Int16 fInt { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double Mileage { get; set; }
        public DataTable dtCustom { get; set; }
        public DataTable _dtBOM { get; set; }
        public DataTable _dtCustom { get; set; }
        public DataTable _dtBomEstimate { get; set; }
        public Stage Stage { get; set; }
        public string _strlabel { get; set; }
        public Int32 _intTab{get;set;}
        public decimal _decPercentage{get;set;}
        public decimal _decPerAmount{get;set;}
        public string _strcontact { get; set; }
        public DateTime _dtdate { get; set; }
        public int _intestimateno { get; set; }
        public string _strtype { get; set; }
        //milestone
        public DataTable _dtmilestone { get; set; }


        public string type
        {
            get { return _strtype; }
            set { _strtype = value; }
        }
        public string contact
        {
            get { return _strcontact; }
            set { _strcontact = value; }
        }

        public DateTime date
        {
            get { return _dtdate; }
            set { _dtdate = value; }
        }

        public int estimateno
        {
            get { return _intestimateno; }
            set { _intestimateno = value; }
        }
        public DataTable milestone
        {
            get { return _dtmilestone; }
            set { _dtmilestone = value; }
        }

        public string label
        {
            get { return _strlabel; }
            set { _strlabel = value; }
        }
        public Int32 Tab
        {
            get { return _intTab; }
            set { _intTab = value; }
        }
        public decimal Percentage
        {
            get { return _decPercentage; }
            set { _decPercentage = value; }
        }
        public decimal PerAmount
        {
            get { return _decPerAmount; }
            set { _decPerAmount = value; }
        }
        public DataTable DtBomEstimate
        {
            get { return _dtBomEstimate; }
            set { _dtBomEstimate = value; }
        }
        public DataTable DtCustom
        {
            get { return _dtCustom; }
            set { _dtCustom = value; }
        }
        public DataTable DtBOM
        {
            get { return _dtBOM; }
            set { _dtBOM = value; }
        }
        public int Close
        {
            get { return _Close; }
            set { _Close = value; }
        }

        private string _Fuser;

        public string Fuser
        {
            get { return _Fuser; }
            set { _Fuser = value; }
        }

        private string _LastUpdateUser;

        public string LastUpdateUser
        {
            get { return _LastUpdateUser; }
            set { _LastUpdateUser = value; }
        }

        private string _NextStep;

        public string NextStep
        {
            get { return _NextStep; }
            set { _NextStep = value; }
        }

        private string _Source;

        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private double _Amount;

        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }


        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private int _OpportunityID;

        public int OpportunityID
        {
            get { return _OpportunityID; }
            set { _OpportunityID = value; }
        }

        private int _Probability;

        public int Probability
        {
            get { return _Probability; }
            set { _Probability = value; }
        }


        private string _Resolution;

        public string Resolution
        {
            get { return _Resolution; }
            set { _Resolution = value; }
        }

        private int _TaskID;
        
        public int TaskID
        {
            get { return _TaskID; }
            set { _TaskID = value; }
        }
        private string _AssignedTo;

        public string AssignedTo
        {
            get { return _AssignedTo; }
            set { _AssignedTo = value; }
        }
        private string _Subject;

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        private DateTime _TimeDue;

        public DateTime TimeDue
        {
            get { return _TimeDue; }
            set { _TimeDue = value; }
        }
        private DateTime _DueDate;

        public DateTime DueDate
        {
            get { return _DueDate; }
            set { _DueDate = value; }
        }
        private int _ROL;

        public int ROL
        {
            get { return _ROL; }
            set { _ROL = value; }
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

        private string _Lat;

        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        private string _Lng;

        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }

        private string _Website;

        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }

        private string _Fax;

        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }

        private DataTable _ContactData;

        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }

        private string _BillPhone;

        public string BillPhone
        {
            get { return _BillPhone; }
            set { _BillPhone = value; }
        }


        private string _BillZip;

        public string BillZip
        {
            get { return _BillZip; }
            set { _BillZip = value; }
        }

        private string _BillState;

        public string BillState
        {
            get { return _BillState; }
            set { _BillState = value; }
        }

        private string _BillCity;

        public string BillCity
        {
            get { return _BillCity; }
            set { _BillCity = value; }
        }

        private string _Billaddress;

        public string Billaddress
        {
            get { return _Billaddress; }
            set { _Billaddress = value; }
        }

        private int _Terr;

        public int Terr
        {
            get { return _Terr; }
            set { _Terr = value; }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }
        private string _SearchBy;

        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }

        private bool _EquipID;

        public bool EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }

        private bool _NullAddressOnly;

        public bool NullAddressOnly
        {
            get { return _NullAddressOnly; }
            set { _NullAddressOnly = value; }
        }

        public string PolygonCoord
        {
            get { return _PolygonCoord; }
            set { _PolygonCoord = value; }
        }

        public string Overlay
        {
            get { return _Overlay; }
            set { _Overlay = value; }
        }

        public string Radius
        {
            get { return _Radius; }
            set { _Radius = value; }
        }

        public string Center
        {
            get { return _Center; }
            set { _Center = value; }
        }

        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

        public DataTable dtWorkerData
        {
            get { return _dtWorkerData; }
            set { _dtWorkerData = value; }
        }  
  
        private DataTable _dtTemplateData;

        public DataTable DtTemplateData
        {
            get { return _dtTemplateData; }
            set { _dtTemplateData = value; }
        }      

        public int TemplateID
        {
            get { return _TemplateID; }
            set { _TemplateID = value; }
        }
       
        public string RouteSequence
        {
            get { return _RouteSequence; }
            set { _RouteSequence = value; }
        }

        public int Worker
        {
            get { return _Worker; }
            set { _Worker = value; }
        }

        public string LocIDs
        {
            get { return _LocIDs; }
            set { _LocIDs = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        public int Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public string Contact
        {
            get { return _Contact; }
            set { _Contact = value; }
        }

        public string Cellular
        {
            get { return _Cellular; }
            set { _Cellular = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
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
        
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public int ProspectID
        {
            get { return _ProspectID; }
            set { _ProspectID = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        private string _DBName;
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        private string _Country;
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        private int _RolType;
        public int RolType
        {
            get { return _RolType; }
            set { _RolType = value; }
        }
        private string _RolName;
        public string RolName
        {
            get { return _RolName; }
            set { _RolName = value; }
        }
        private string _RolRemarks;
        public string RolRemarks
        {
            get { return _RolRemarks; }
            set { _RolRemarks = value; }
        }
        private DataTable _dtTeam;
        public DataTable DtTeam
        {
            get { return _dtTeam; }
            set { _dtTeam = value; }
        }
        private DataTable _dtMilestone;
        public DataTable DtMilestone
        {
            get { return _dtMilestone; }
            set { _dtMilestone = value; }
        }
    }
}
