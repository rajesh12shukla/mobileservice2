using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Loc
    {
        public int LocID;
        public int Owner;
        public string ID;
        public string Tag;
        public string Address;
        public string City;
        public string State;
        public Int16 Elevs;
        public Int16 Status;
        public double Balance;
        public int Rol;
        public string STax;
        public int Job;
        public string Remarks;
        public string Type;
        public Int16 Billing;
        public string Country;
        private DataSet _dsLoc;
        private string _ConnConfig;
        public DataSet DsLoc
        {
            get { return _dsLoc; }
            set { _dsLoc = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
}
