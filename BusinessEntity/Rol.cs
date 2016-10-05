using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Rol
    {
        public int ID;
        public string Name;
        public string City;
        public string State;
        public string Zip;
        public string Phone;
        public string Fax;
        public string Contact;
        public string Remarks;
        public int Type;
        public int fLong;
        public int Latt;
        public int GeoLock;
        public DateTime Since;
        public DateTime Last;
        public string Address;
        public int EN;
        public string EMail;
        public string Website;
        public string Cellular;
        public string Category;
        public string Position;
        public string Country;
        public string lat;
        public string lng;
        public DateTime LastUpdateDate;
        public string ConnConfig;

        public Bank objBank;
        private DataSet _dsRol;
        private DataSet _dsID;
      
        public DataSet DsRol
        {
            get { return _dsRol; }
            set { _dsRol = value; }
        }
        public DataSet DsID
        {
            get { return _dsID; }
            set { _dsID = value; }
        }
       
    }
}
