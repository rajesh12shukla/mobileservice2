using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Dep
    {
        public int Ref;
        public DateTime fDate;
        public int Bank;
        public string fDesc;
        public double Amount;
        public int TransID;
        public int EN;
        public int ReceivedPaymentID;
        public bool IsRecon;
        public int fDateYear;
        public DateTime StartDate;
        public DateTime EndDate;

        private DataSet _ds;
        private DataSet _dsId;
        private string _ConnConfig;
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
        public DataSet DsID
        {
            get { return _dsId; }
            set { _dsId = value; }
        }
    }
}
