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


namespace DataLayer
{
    public class DL_UnitOfMeasure
    {

        public List<UnitOfMeasure> GetALLUnitOfMeasure()
        {
            DataSet ds = null;
            List<UnitOfMeasure> uoms = new List<UnitOfMeasure>();
            try
            {
                string constring = string.Empty;
                if (HttpContext.Current.Session["config"] != null)
                {
                    constring = HttpContext.Current.Session["config"].ToString();
                }

                if (string.IsNullOrEmpty(constring))
                    return uoms;

                ds = SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, UnitOfMeasure.GET_ALL_UNITOFMEASURE);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            UnitOfMeasure objuom = new UnitOfMeasure();
                            objuom.ID = ds.Tables[0].Rows[i][UnitOfMeasure.tblMappingID] != DBNull.Value ? (int)ds.Tables[0].Rows[i][UnitOfMeasure.tblMappingID] : 0;
                            objuom.Code = ds.Tables[0].Rows[i][UnitOfMeasure.tblMappingCode] != DBNull.Value ? (string)ds.Tables[0].Rows[i][UnitOfMeasure.tblMappingCode] : "";
                            objuom.Description = ds.Tables[0].Rows[i][UnitOfMeasure.tblMappingDesc] != DBNull.Value ? (string)ds.Tables[0].Rows[i][UnitOfMeasure.tblMappingDesc] : "";

                            uoms.Add(objuom);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uoms;
        }
    }
}
