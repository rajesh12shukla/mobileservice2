using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class JobI
    {
        public int Job;
        public Int16 Phase;
        public DateTime fDate;
        public string Ref;
        public string fDesc;
        public double Amount;
        public int TransID;
        public Int16 Type;
        public Int16 Labor;
        public int Billed;
        public int Invoice;
        public int UseTax;
        public int vAPTicket;
        private DataSet _ds;
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
    }
}
