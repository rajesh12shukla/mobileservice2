using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class ChartDetails
    {
        public int ID { get; set; }
        public string AccountType { get; set; }
        //public List<AcctDetails> AcctDetails { get; set; }
        public List<SubAcctDetails> LstSubAcct { get; set; }
        public SubAcctDetails SubAcct { get; set; }
        public double TotalBalance { get; set; }
    }
    public class AcctDetails
    {
        public int AcctTypeID { get; set; }
        public int AcctNum { get; set; }
        public string Account { get; set; }
        public double Balance { get; set; }
        public string SubAccount { get; set; }
    }
    public class SubAcctDetails
    {
        public int SubAcctID { get; set; }
        public int AcctNum { get; set; }
        public string SubAccount { get; set; }
        public double TotalBalance { get; set; }
        public List<AcctDetails> AcctDetails { get; set; }
    }
}
