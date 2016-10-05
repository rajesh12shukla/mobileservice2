using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class OpenAP
    {
        public int Vendor;
        public DateTime fDate;
        public DateTime Due;
        public Int16 Type;
        public string fDesc;
        public double Original;
        public double Balance;
        public double Selected;
        public double Disc;
        public int PJID;
        public int TRID;
        public string Ref;
        public DateTime SearchDate;
        public Int16 SearchValue;

        private DataSet _ds;
        private string _ConnConfig;

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
