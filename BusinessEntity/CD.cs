using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class CD
    {
        public int ID;
        public DateTime fDate;
        public int Ref;
        public string fDesc;
        public double Amount;
        public int Bank;
        public Int16 Type;
        public Int16 Status;
        public int TransID;
        public int Vendor;
        public string French;
        public string Memo;
        public string VoidR;
        public Int16 ACH;
        public int fDateYear;
        public bool IsRecon;
        public DateTime StartDate;
        public DateTime EndDate;

        private DataSet _ds;
        private string _ConnConfig;
        public bool IsExistCheckNo;

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
    }
}
