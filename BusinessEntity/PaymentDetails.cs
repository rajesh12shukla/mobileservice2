using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class PaymentDetails
    {
        public int ID;
        public int ReceivedPaymentID;
        public int TransID;
        public int Loc;
        public int Rol;
        public int InvoiceID;
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
