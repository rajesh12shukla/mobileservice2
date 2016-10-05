using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Transaction
    {
        public int ID;
        public int BatchID;
        public DateTime TransDate;
        public int Acct;
        public int? AcctSub;
        public int Type;
        public int Line;
        public int Ref;
        public string TransDescription;
        public double Amount;
        public string Status;
        public int JobInt;
        public double PhaseDoub;
        public string strRef;
        public Int16 Sel;
        public double UseTax;
        public int fDateYear;
        public byte[] TimeStamp;
        public bool IsAccessible;
        public bool IsUseTax;
        public int UseTaxGL;
        public string UtaxName;
        public bool IsJob;

        private string _ConnConfig;
        private DataSet _dsTrans;
        private string _SearchValue;
        private string _tableName;

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsTrans
        {
            get { return _dsTrans; }
            set { _dsTrans = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
    }
    public class TransactionModel
    {
        public TransactionModel(int _val, string _field)
        {
            FieldValue = _val;
            Field = _field;
        }

        public int FieldValue { get; set; }
        public string Field { get; set; }
    }
}

