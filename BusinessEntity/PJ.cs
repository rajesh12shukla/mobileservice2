using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class PJ
    {
        public int ID;
        public DateTime fDate;
        public DateTime PostDate;
        public string Ref;
        public string fDesc;
        public double Amount;
        public int Vendor;
        public Int16 Status;
        public int Batch;
        public Int16 Terms;
        public int PO;
        public int TRID;
        public Int16 Spec;
        public DateTime IDate;
        public double UseTax;
        public double Disc;
        public string Custom1;
        public string Custom2;
        public int ReqBy;
        public string VoidR;
        public string UtaxName;
        public int GL;
        private DataSet _ds;
        private string _ConnConfig;
        public DateTime StartDate;
        public DateTime EndDate;
        public Int16 SearchValue;
        public DateTime SearchDate;
        public DataTable _dt;
        public int ReceivePo;
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
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

    }
}
