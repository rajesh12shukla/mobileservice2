using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class UnitOfMeasure
    {
        #region ::StoreProc::
        public static string GET_ALL_UNITOFMEASURE = "spGetUnitOfMeasure";

        #endregion

        #region ::Table Mapping Name::
        public static string tblMappingID = "ID";
        public static string tblMappingCode = "UnitOfMeasureCode";
        public static string tblMappingDesc = "UnitOfMeasureDesc";
        #endregion

        #region ::Private Property Variable Declaration::
        private int _ID;
        private string _Code;
        private string _Description;
        private string _ConnConfig;
        private DataSet _ds;
        #endregion

        #region::Public Property Declaration::

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

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
            }
        }
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        #endregion


    }
}
