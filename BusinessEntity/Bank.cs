using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Bank
    {
        public int ID;
        public string fDesc;
        public int Rol;
        public string NBranch;
        public string NAcct;
        public string NRoute;
        public int NextC;
        public int NextD;
        public int NextE;
        public double Rate;
        public double CLimit;
        public Int16 Warn;
        public double Recon;
        public double Balance;
        public int Status;
        public int InUse;
        public int GeoLock = 0;
        private DataSet _dsBank;
        private string _ConnConfig;
        public int Chart;
        public DateTime LastReconDate;
        public double ServiceCharge;
        public double InterestCharge;
        public int ServiceAcct;
        public int InterestAcct;
        public DateTime ServiceDate;
        public DateTime InterestDate;
        public DataTable _dtBank;
        public DataTable DtBank
        {
            get { return _dtBank; }
            set { _dtBank = value; }
        }
        public DataSet DsBank
        {
            get { return _dsBank; }
            set { _dsBank = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
}
