using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class DL_AccountType
    {
        public DataSet GetType(AccountType _objAcType)
        {
            try
            {
                return _objAcType.DsType = SqlHelper.ExecuteDataset(_objAcType.ConnConfig, CommandType.Text, "SELECT c.* FROM (SELECT ROW_NUMBER() over (order by (select null)) - 1 as ID, Type FROM [CType]) as c ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTypeByAccount(AccountType _objAcType)
        {
            try
            {
                return _objAcType.DsType = SqlHelper.ExecuteDataset(_objAcType.ConnConfig, CommandType.Text, " SELECT * FROM SubCat Where CType = " + _objAcType.CType + " Order By SortOrder   ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubTypeByID(AccountType _objAcType)
        {
            try
            {
                return _objAcType.DsType = SqlHelper.ExecuteDataset(_objAcType.ConnConfig, CommandType.Text, "SELECT c.* FROM (SELECT ROW_NUMBER() over (order by (select null)) - 1 as ID, Type FROM [CType]) as c WHERE c.ID = " + _objAcType.CType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddSubAccount(AccountType _objAcType)
        {
            try
            {
                string query = "INSERT INTO SubCat (CType, SubType, SortOrder) VALUES (@Ctype, @SubType, @SortOrder)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ctype", _objAcType.CType));
                parameters.Add(new SqlParameter("@SubType", _objAcType.SubType));
                parameters.Add(new SqlParameter("@SortOrder", _objAcType.SortOrder));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objAcType.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllSubAccout(AccountType _objAcType)
        {
            try
            {
                return _objAcType.DsType = SqlHelper.ExecuteDataset(_objAcType.ConnConfig, CommandType.Text, "SELECT * FROM SubCat Order by CType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
