using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class State
    {
        public string Name;
        public string fDesc;
        public string Country;
        private DataSet _dsState;
        public string ConnConfig;
        public DataSet DsState
        {
            get { return _dsState; }
            set { _dsState = value; }
        }
    }
}
