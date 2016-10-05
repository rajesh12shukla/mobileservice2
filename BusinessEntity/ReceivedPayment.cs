using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class ReceivedPayment
    {
        public int ID;
        public int Loc;
       
        public double Amount;
        public DateTime PaymentReceivedDate;
        public Int16 PaymentMethod;
        public string CheckNumber;
        public double AmountDue;
        public string fDesc;
        public int DepID;
        public int Status;
        private DataSet _ds;
        private DataSet _dsID;
        private DataTable _dtPay;
        private string _ConnConfig;
        public int Rol;
        public DateTime StartDate;
        public DateTime EndDate;
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
            get { return _dsID; }
            set { _dsID = value; }
        }
        public DataTable DtPay
        {
            get { return _dtPay; }
            set { _dtPay = value; }
        }

    }
}
