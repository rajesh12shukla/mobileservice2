using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    /// <summary>
    /// This class is representing stages of sales and opportunities 
    /// </summary>
    public class Stage
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Description of stages
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// No. of Stages Count
        /// </summary>
        public int Count { get; set; }

    }
}
