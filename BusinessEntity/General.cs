using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessEntity
{
    public class General
    {
        private string _RegID;
        private string _DeviceID;
        private DataSet _ds;
        private string _ConnConfig;
        private string _EmpID;
        private string _FunctionName;
        private string _CustomName;
        private string _CustomLabel;
        private string _CodeCategory;
        private int _CodeType;
        private string _Code;
        private string _CodeDesc;
        private string _Category;
        private int _DiagnosticType;
        private string _Remarks;
        private int _GPSInterval;
        private int _IsRunning;
        private int _IsGPSEnabled;
        private string _ServiceName;
        private string _Error;
        private string _QBapi;
        private string _QBRequestID;
        private string _QBStatusCode;
        private string _QBStatusSeverity;
        public int userid { get; set; }
        public string From { get; set; }
        public string to { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string subject { get; set; }
        public DateTime? sentdate { get; set; }
        public string date { get; set; }
        public int Attachments { get; set; }
        public string msgid { get; set; }
        public long uid { get; set; }
        public Guid GUID { get; set; }
        public int mailid { get; set; }
        public int type { get; set; }
        public int rol { get; set; }
        public string AccountID { get; set; }
        public string OrderBy { get; set; }       
        private string _QBStatusMessage;
        public string TextQuery { get; set; }

        public string QBStatusMessage
        {
            get { return _QBStatusMessage; }
            set { _QBStatusMessage = value; }
        }

        public string QBStatusSeverity
        {
            get { return _QBStatusSeverity; }
            set { _QBStatusSeverity = value; }
        }

        public string QBStatusCode
        {
            get { return _QBStatusCode; }
            set { _QBStatusCode = value; }
        }

        public string QBRequestID
        {
            get { return _QBRequestID; }
            set { _QBRequestID = value; }
        }

        public string QBapi
        {
            get { return _QBapi; }
            set { _QBapi = value; }
        }

        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }

        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }
        }
      
        public int IsGPSEnabled
        {
            get { return _IsGPSEnabled; }
            set { _IsGPSEnabled = value; }
        }

        public int IsRunning
        {
            get { return _IsRunning; }
            set { _IsRunning = value; }
        }

        public int GPSInterval
        {
            get { return _GPSInterval; }
            set { _GPSInterval = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
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

        public string CodeDesc
        {
            get { return _CodeDesc; }
            set { _CodeDesc = value; }
        }

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public int CodeType
        {
            get { return _CodeType; }
            set { _CodeType = value; }
        }

        public string CodeCategory
        {
            get { return _CodeCategory; }
            set { _CodeCategory = value; }
        }

        public string CustomLabel
        {
            get { return _CustomLabel; }
            set { _CustomLabel = value; }
        }

        public string CustomName
        {
            get { return _CustomName; }
            set { _CustomName = value; }
        }

        public string FunctionName
        {
            get { return _FunctionName; }
            set { _FunctionName = value; }
        }

        public string EmpID
        {
            get { return _EmpID; }
            set { _EmpID = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }

        public string RegID
        {
            get { return _RegID; }
            set { _RegID = value; }
        }
    }
}
