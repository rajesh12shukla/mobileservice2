using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessEntity
{
    public class AccountType
    {
        private string _connConfig;
        public int ID { get; set; }
        public int CType { get; set; }
        public string SubType { get; set; }
        public int SortOrder { get; set; }
        private int _maxSortValue { get; set; }
        private DataSet _dsType;
        public string ConnConfig
        {
            get { return _connConfig; }
            set { _connConfig = value; }
        }
        public DataSet DsType
        {
            get { return _dsType; }
            set { _dsType = value; }
        }
        public int MaxSortValue 
        { 
            get { return _maxSortValue; }
            set { _maxSortValue = value; } 
        }
    }
}
