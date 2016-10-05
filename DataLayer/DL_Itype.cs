using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Itype
    {

        public List<Itype> GetALLItype()
        {
            DataSet ds = null;
            List<Itype> itypes = new List<Itype>();
            try
            {
                string constring = string.Empty;
                if (HttpContext.Current.Session["config"] != null)
                {
                    constring = HttpContext.Current.Session["config"].ToString();
                }

                if (string.IsNullOrEmpty(constring))
                    return itypes;

                ds = SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, Itype.GET_ALL_ITYPE);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Itype objtype = new Itype();
                            objtype.ID = ds.Tables[0].Rows[i][Itype.tblMappingID] != DBNull.Value ? (int)ds.Tables[0].Rows[i][Itype.tblMappingID] : 0;
                            objtype.Type = ds.Tables[0].Rows[i][Itype.tblMappingType] != DBNull.Value ? (string)ds.Tables[0].Rows[i][Itype.tblMappingType] : "";

                            itypes.Add(objtype);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itypes;
        }
    }
}
