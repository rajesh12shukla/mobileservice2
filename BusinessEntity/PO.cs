using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class PO
    {
        public int POID;
        public DateTime fDate;
        public string fDesc;
        public double Amount;
        public int Vendor;
        public Int16 Status;
        public DateTime Due;
        public string ShipVia;
        public Int16 Terms;
        public string FOB;
        public string ShipTo;
        public Int16 Approved;
        public string Custom1;
        public string Custom2;
        public string ApprovedBy;
        public int ReqBy;
        public string fBy;
        private DataSet _ds;
        private DataTable _POdt;
        private string _ConnConfig;
        public DateTime StartDate;
        public DateTime EndDate;
        public string POReasonCode;
        public string CourrierAcct;
        public string PORevision;
        public double Quan;
        public double SelectedQuan;
        public double BalanceQuan;
        public double ReceivedQuan;
        public bool IsClosed;

        public DataTable PODt
        {
            get { return _POdt; }
            set { _POdt = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int RID;
        public string Ref;
        public string WB;
        public string Comments;
        public double Balance;
        public double Selected;
        public Int16 Line;
        public int ReceivePOId;
    }
}
