using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Vendor
    {
        private string _ConnConfig;
        private string _DBName;

        private DataSet _ds;
        private DataSet _dsID;
        private DataSet _dsIsExist;

        public int ID;
        public int Rol;
        public string Acct { get; set; }
        public string Type { get; set; }
        public Int16 Status { get; set; }
        public double Balance { get; set; }
        public double CLimit { get; set; }
        public Int16 Vendor1099 { get; set; }
        public string FID { get; set; }
        public int DA { get; set; }
        public string AcctNumber { get; set; }
        public Int16 Terms { get; set; }
        public double Disc { get; set; }
        public Int16 Days { get; set; }
        public Int16 InUse { get; set; }
        public string Remit { get; set; }
        public Int16 OnePer { get; set; }
        public string DBank { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }
        public string Custom6 { get; set; }
        public string Custom7 { get; set; }
        public DateTime Custom8 { get; set; }
        public DateTime Custom9 { get; set; }
        public DateTime Custom10 { get; set; }
        public string ShipVia { get; set; }
        public string QBVendorID { get; set; }
        private string _SearchValue;
        public bool IsExist { get; set; }
       
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
        public DataSet DsID
        {
            get { return _dsID; }
            set { _dsID = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public DataSet DsIsExist
        {
            get { return _dsIsExist; }
            set { _dsIsExist = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

    }
}
