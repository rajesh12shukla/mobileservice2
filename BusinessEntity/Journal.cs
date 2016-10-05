using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Journal
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _dsGLA;
        private DataSet _dsTrans;
        private DataSet _dsRecurTrans;
        private DataSet _dsRecurCount;
        private DataSet _dsRecurDate;

        public DateTime StartDate;
        public DateTime EndDate;

        public DateTime fDate;
        public string fDesc;
        public int MaxTransID;
        public int MaxTransBatch;
        public int MaxGLARef;
        public int TransID;

        public DateTime GLDate;
        public string GLDesc;
        public int BatchID;
        public int Ref;
        public string Internal;
        public bool IsRecurring;
        public bool IsJobSpec;
        public int Frequency;
        
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public DataSet DsGLA
        {
            get { return _dsGLA; }
            set { _dsGLA = value; }
        }
        public DataSet DsTrans
        {
            get { return _dsTrans; }
            set { _dsTrans = value; }
        }
        public DataSet DsRecurTrans
        {
            get { return _dsRecurTrans; }
            set { _dsRecurTrans = value; }
        }
        public DataSet DsRecurCount
        {
            get { return _dsRecurCount; }
            set { _dsRecurCount = value; }
        }
        public DataSet DsRecurDate
        {
            get { return _dsRecurDate; }
            set { _dsRecurDate = value; }
        }
    }
  
}
