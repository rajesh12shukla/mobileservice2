using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Paid
    {
        public int PITR;
        public DateTime fDate;
        public Int16 Type;
        public Int16 Line;
        public string fDesc;
        public double Original;
        public double Balance;
        public double Disc;
        public double Paid1;
        public int TRID;
        public string Ref;
        public string ConnConfig;

        private DataSet _ds;
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

    }
}
