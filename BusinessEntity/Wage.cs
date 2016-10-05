using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Wage
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _ds;
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
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public Int16 Field { get; set; }
        public double Reg { get; set; }
        public double OT1 { get; set; }
        public double OT2 { get; set; }
        public double TT { get; set; }
        public Int16 FIT { get; set; }
        public Int16 FICA { get; set; }
        public Int16 MEDI { get; set; }
        public Int16 FUTA { get; set; }
        public Int16 SIT { get; set; }
        public Int16 Vac { get; set; }
        public Int16 WC { get; set; }
        public Int16 Uni { get; set; }
        public int GL { get; set; }
        public double NT { get; set; }
        public int MileageGL { get; set; }
        public int ReimGL { get; set; }
        public int ZoneGL { get; set; }
        public Int16 Globe { get; set; }
        public Int16 Status { get; set; }
        public double CReg { get; set; }
        public double COT { get; set; }
        public double CDT { get; set; }
        public double CNT { get; set; }
        public double CTT { get; set; }
        public string Remarks { get; set; }
        public int RegGL { get; set; }
        public int OTGL { get; set; }
        public int NTGL { get; set; }
        public int DTGL { get; set; }
        public int TTGL { get; set; }
    }
}
