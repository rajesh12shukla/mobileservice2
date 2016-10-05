using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class GenerateCheck
    {
        public int CheckNum;
        public DateTime CheckDate;
        public Double TotalAmount;
        public string TotalAmountWords;
        public string VendorName;
        public string VendorAddress;
        public DataTable dtOpenAP;
    }
}
