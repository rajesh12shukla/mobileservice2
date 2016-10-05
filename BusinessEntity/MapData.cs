using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessEntity
{
    public class MapData
    {
        private DataTable _LocData;
        private string _ConnConfig;
        private DataSet _Ds;
        private string _tech;
        private DateTime _Date;
        private double _Latitude;
        private double _Longitude;
        private int _TicketID;
        private int _Assigned;
        private int _LocID;
        private string _LocTag;
        private string _LocAddress;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Phone;
        private string _Cell;
        private string _Worker;
        private DateTime _CallDate;
        private DateTime _SchDate;
        private DateTime _EnrouteTime;
        private DateTime _OnsiteTime;
        private DateTime _ComplTime;
        private string _Category;
        private int _Unit;
        private string _Reason;
        private string _CustomerName;
        private int _CustID;
        private double _Resize;
        private double _EST;
        private int _ISTicketD;
        private string _CompDescription;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private double _RT;
        private double _OT;
        private double _NT;
        private double _TT;
        private double _DT;
        private double _Total;
        private int _Charge;
        private string _filterCharge;
        private string _Supervisor;
        private string _FilterReview;
        private int _Review;
        private string _Who;
        private byte[] _Signature;
        private string _Remarks;
        private int _Level;
        private int _Department;
        private int _Mobile;
        private string _Custom2;
        private string _Custom3;
        private int _Custom6;
        private int _Custom7;
        private string _Custom1;
        private string _Custom4;
        private string _Custom5;
        private string _Workorder;
        private string _SearchBy;
        private string _SearchValue;
        private int _Status;
        private int _WorkComplete;
        private double _ZoneExpense;
        private double _TollExpense;
        private double _MiscExpense;
        private int _MileStart;
        private int _MileEnd;
        private string _FileName;
        private string _FilePath;
        private int _DocType;
        private string _TempId;
        private string _Screen;
        private string _DocTypeMIME;
        private int _DocumentID;
        private int _Internet;
        private int _WorkID;
        private int _InvoiceID;
        private int _IsList;
        private string _ManualInvoiceID;
        private string _QBInvoiceID;
        private int _TimeTransfer;
        private string _Timesheet;
        private string _CreditReason;
        public int JobTemplateID { get; set; }

        public int WageID { get; set; }
        public int phase { get; set; }
        public string jobcode { get; set; }
        public string Recommendation { get; set; }
        public string CustomTick1 { get; set; }
        public string CustomTick2 { get; set; }
        public int CustomTick3 { get; set; }
        public int CustomTick4 { get; set; }
        public double CustomTick5 { get; set; }
        public int DefaultWorker { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public int RoleID { get; set; }

        public string LocIDs { get; set; }

        public string OrderBy { get; set; }

        public string MainContact { get; set; }

        public DataTable dtReview { get; set; }

        public DataTable dtTickets { get; set; }

        private string _LastUpdatedBy;
        public DataTable dtEquips { get; set; }
        public string LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { _LastUpdatedBy = value; }
        }

        private string _QBServiceID;

        public string QBServiceID
        {
            get { return _QBServiceID; }
            set { _QBServiceID = value; }
        }

        private string _QBPayrollID;

        public string QBPayrollID
        {
            get { return _QBPayrollID; }
            set { _QBPayrollID = value; }
        }

        private string _Subject;

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        private string _Body;

        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        private int _Mode;

        public int Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        private int _DocID;

        public int DocID
        {
            get { return _DocID; }
            set { _DocID = value; }
        }

        private int _IsRecurring;

        public int IsRecurring
        {
            get { return _IsRecurring; }
            set { _IsRecurring = value; }
        }

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
        private string _NonSuper;

        public string NonSuper
        {
            get { return _NonSuper; }
            set { _NonSuper = value; }
        }

        public string Timesheet
        {
            get { return _Timesheet; }
            set { _Timesheet = value; }
        }

        public int TimeTransfer
        {
            get { return _TimeTransfer; }
            set { _TimeTransfer = value; }
        }

        public string QBInvoiceID
        {
            get { return _QBInvoiceID; }
            set { _QBInvoiceID = value; }
        }

        public string ManualInvoiceID
        {
            get { return _ManualInvoiceID; }
            set { _ManualInvoiceID = value; }
        }

        public int IsList
        {
            get { return _IsList; }
            set { _IsList = value; }
        }

        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }

        public int WorkID
        {
            get { return _WorkID; }
            set { _WorkID = value; }
        }

        public int Internet
        {
            get { return _Internet; }
            set { _Internet = value; }
        }

        public int DocumentID
        {
            get { return _DocumentID; }
            set { _DocumentID = value; }
        }

        public string DocTypeMIME
        {
            get { return _DocTypeMIME; }
            set { _DocTypeMIME = value; }
        }

        public string Screen
        {
            get { return _Screen; }
            set { _Screen = value; }
        }

        public string TempId
        {
            get { return _TempId; }
            set { _TempId = value; }
        }

        public int DocType
        {
            get { return _DocType; }
            set { _DocType = value; }
        }

        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public int MileEnd
        {
            get { return _MileEnd; }
            set { _MileEnd = value; }
        }

        public int MileStart
        {
            get { return _MileStart; }
            set { _MileStart = value; }
        }

        public double MiscExpense
        {
            get { return _MiscExpense; }
            set { _MiscExpense = value; }
        }

        public double TollExpense
        {
            get { return _TollExpense; }
            set { _TollExpense = value; }
        }

        public double ZoneExpense
        {
            get { return _ZoneExpense; }
            set { _ZoneExpense = value; }
        }

        public int WorkComplete
        {
            get { return _WorkComplete; }
            set { _WorkComplete = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
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

        public string Workorder
        {
            get { return _Workorder; }
            set { _Workorder = value; }
        }

        public string Custom5
        {
            get { return _Custom5; }
            set { _Custom5 = value; }
        }

        public string Custom4
        {
            get { return _Custom4; }
            set { _Custom4 = value; }
        }

        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }

        public int Custom7
        {
            get { return _Custom7; }
            set { _Custom7 = value; }
        }

        public int Custom6
        {
            get { return _Custom6; }
            set { _Custom6 = value; }
        }

        public string Custom3
        {
            get { return _Custom3; }
            set { _Custom3 = value; }
        }

        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }

        public int Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; }
        }
        public int jobid { get; set; }

        public int projectID { get; set; }
        public int Department
        {
            get { return _Department; }
            set { _Department = value; }
        }

        public string Bremarks { get; set; }

        public int Level
        {
            get { return _Level; }
            set { _Level = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public byte[] Signature
        {
            get { return _Signature; }
            set { _Signature = value; }
        }

        public string Who
        {
            get { return _Who; }
            set { _Who = value; }
        }

        public int Review
        {
            get { return _Review; }
            set { _Review = value; }
        }

        public string FilterReview
        {
            get { return _FilterReview; }
            set { _FilterReview = value; }
        }

        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; }
        }

        public string FilterCharge
        {
            get { return _filterCharge; }
            set { _filterCharge = value; }
        }

        public int Charge
        {
            get { return _Charge; }
            set { _Charge = value; }
        }

        public double Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        public double DT
        {
            get { return _DT; }
            set { _DT = value; }
        }

        public double TT
        {
            get { return _TT; }
            set { _TT = value; }
        }

        public double NT
        {
            get { return _NT; }
            set { _NT = value; }
        }

        public double OT
        {
            get { return _OT; }
            set { _OT = value; }
        }

        public double RT
        {
            get { return _RT; }
            set { _RT = value; }
        }

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        public string CompDescription
        {
            get { return _CompDescription; }
            set { _CompDescription = value; }
        }

        public int ISTicketD
        {
            get { return _ISTicketD; }
            set { _ISTicketD = value; }
        }

        public double EST
        {
            get { return _EST; }
            set { _EST = value; }
        }

        public double Resize
        {
            get { return _Resize; }
            set { _Resize = value; }
        }

        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }

        public string Reason
        {
            get { return _Reason; }
            set { _Reason = value; }
        }

        public int Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public DateTime ComplTime
        {
            get { return _ComplTime; }
            set { _ComplTime = value; }
        }

        public DateTime OnsiteTime
        {
            get { return _OnsiteTime; }
            set { _OnsiteTime = value; }
        }

        public DateTime EnrouteTime
        {
            get { return _EnrouteTime; }
            set { _EnrouteTime = value; }
        }

        public DateTime SchDate
        {
            get { return _SchDate; }
            set { _SchDate = value; }
        }

        public DateTime CallDate
        {
            get { return _CallDate; }
            set { _CallDate = value; }
        }

        public string Worker
        {
            get { return _Worker; }
            set { _Worker = value; }
        }

        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
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

        public string LocAddress
        {
            get { return _LocAddress; }
            set { _LocAddress = value; }
        }

        public string LocTag
        {
            get { return _LocTag; }
            set { _LocTag = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        public int Assigned
        {
            get { return _Assigned; }
            set { _Assigned = value; }
        }

        public int TicketID
        {
            get { return _TicketID; }
            set { _TicketID = value; }
        }

        public int roleid { get; set; }
        public double Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public double Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        public string Tech
        {
            get { return _tech; }
            set { _tech = value; }
        }

        public DataSet Ds
        {
            get { return _Ds; }
            set { _Ds = value; }
        }

        public DataTable LocData
        {
            get { return _LocData; }
            set { _LocData = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public string fBy { get; set; }
    }
}
