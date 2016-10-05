using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class WebMethodHeader
    {
        public int TotalCount;
        public bool HasError;

        public List<string> ErrorMessages = new List<string>();
        public List<string> SuccessMessages = new List<string>();
        public WebMethodHeader()
        {
            Init(0, false);
        }

        private void Init(int totalcount,bool hasError)
        {
            TotalCount = totalcount;
            HasError = hasError;
        }

    }
}
