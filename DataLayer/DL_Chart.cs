using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace DataLayer
{
    public class DL_Chart
    {
        public int AddChart(Chart objChart)
        {
            try
            {
                var para = new SqlParameter[32];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Acct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Acct
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@AcType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.AcType
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Sub",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Sub
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Sub2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Sub2
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Remarks
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Status
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@City",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.City
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@State",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.State
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@Zip",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Zip
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@Phone",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Phone
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "@Fax",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Fax
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "@Contact",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Contact
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "@Address",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Address
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.EMail
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@Website",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Website
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "@Country",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Country
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "@Cellular",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Cellular
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Type
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "@GeoLock",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.GeoLock
                };
                if (objChart.Since != DateTime.MinValue)
                {
                    para[20] = new SqlParameter
                    {
                        ParameterName = "@Since",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Since
                    };
                }
                if (objChart.Last != DateTime.MinValue)
                {
                    para[21] = new SqlParameter
                    {
                        ParameterName = "@Last",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Last
                    };
                }
                para[22] = new SqlParameter
                {
                    ParameterName = "@NBranch",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NBranch
                };
                para[23] = new SqlParameter
                {
                    ParameterName = "@NAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NAcct
                };
                para[24] = new SqlParameter
                {
                    ParameterName = "@NRoute",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NRoute
                };
                para[25] = new SqlParameter
                {
                    ParameterName = "@NextC",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextC
                };
                para[26] = new SqlParameter
                {
                    ParameterName = "@NextD",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextD
                };
                para[27] = new SqlParameter
                {
                    ParameterName = "@NextE",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextE
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@Rate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Rate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@CLimit",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.CLimit
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@Warn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Warn
                };
                para[31] = new SqlParameter
                {
                    ParameterName = "@Recon",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Recon
                };
                //para[32] = new SqlParameter
                //{
                //    ParameterName = "returnval",
                //    SqlDbType = SqlDbType.Int,
                //    Value = ParameterDirection.ReturnValue
                //};

                SqlHelper.ExecuteNonQuery(objChart.ConnConfig, CommandType.StoredProcedure, "spAddChart", para);
                return Convert.ToInt32(para[32].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllCOA(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" \n");
                varname.Append(" SELECT ID, Acct, fDesc, isnull(Balance,0) as Balance,              \n");
                varname.Append("       Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status,     \n");
                varname.Append("       (CASE isnull(Status,0) WHEN 0 THEN 'Active'                \n");
                varname.Append("       WHEN 1 THEN 'InActive'                           \n");
                varname.Append("       WHEN 2 THEN 'Hold' END) AS AcctStatus,               \n");
                varname.Append("       (CASE Type WHEN 0 THEN 'Asset' \n");
                varname.Append("        WHEN 1 THEN 'Liability'    \n");
                varname.Append("        WHEN 2 THEN 'Equity'        \n");
                varname.Append("        WHEN 3 THEN 'Revenue'       \n");
                varname.Append("        WHEN 4 THEN 'Cost'          \n");
                varname.Append("        WHEN 5 THEN 'Expense'       \n");
                varname.Append("        WHEN 6 THEN 'Bank'  WHEN 7 THEN 'Non-Posting' END) AS AcctType,        \n");
                varname.Append("       Sub2, DAT, Branch, CostCenter, AcctRoot,QBAccountID,LastUpdateDate FROM Chart Order by Acct ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateChart(Chart objChart)
        {
            try
            {
                var para = new SqlParameter[35];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Acct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Acct
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@AcType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.AcType
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Sub",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Sub
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Sub2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Sub2
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Remarks
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Status
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@City",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.City
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@State",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.State
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@Zip",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Zip
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@Phone",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Phone
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "@Fax",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Fax
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "@Contact",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Contact
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "@Address",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Address
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.EMail
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@Website",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Website
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "@Country",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Country
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "@Cellular",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Cellular
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Type
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "@GeoLock",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.GeoLock
                };
                if (objChart.Since != DateTime.MinValue)
                {
                    para[20] = new SqlParameter
                    {
                        ParameterName = "@Since",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Since
                    };
                }
                if (objChart.Last != DateTime.MinValue)
                {
                    para[21] = new SqlParameter
                    {
                        ParameterName = "@Last",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Last
                    };
                }
                para[22] = new SqlParameter
                {
                    ParameterName = "@NBranch",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NBranch
                };
                para[23] = new SqlParameter
                {
                    ParameterName = "@NAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NAcct
                };
                para[24] = new SqlParameter
                {
                    ParameterName = "@NRoute",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NRoute
                };
                para[25] = new SqlParameter
                {
                    ParameterName = "@NextC",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextC
                };
                para[26] = new SqlParameter
                {
                    ParameterName = "@NextD",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextD
                };
                para[27] = new SqlParameter
                {
                    ParameterName = "@NextE",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextE
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@Rate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Rate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@CLimit",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.CLimit
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@Warn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Warn
                };
                para[31] = new SqlParameter
                {
                    ParameterName = "@Recon",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Recon
                };
                para[32] = new SqlParameter
                {
                    ParameterName = "@Rol",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.Rol
                };
                para[33] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.Bank
                };
                para[34] = new SqlParameter
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.ID
                };

                SqlHelper.ExecuteNonQuery(objChart.ConnConfig, CommandType.StoredProcedure, "spUpdateChart", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChart(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT [ID],[Acct],[fDesc],[Balance],[Type],[Sub],[Remarks],[Control],[InUse],[Detail],[CAlias],[Status],[Sub2],[DAT],[Branch],[CostCenter],[AcctRoot],[QBAccountID],[LastUpdateDate],ISNULL(DefaultNo,0) AS DefaultNo FROM Chart WHERE ID = " + _objChart.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteChart(Chart _objChart)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objChart.ConnConfig, CommandType.Text, " DELETE FROM Chart WHERE ID = " + _objChart.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStatus(Chart _objChart)
        {
            try
            {
                return _objChart.DsStatus = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT  IDENTITY (INT, 0, 1) AS ID, Status INTO #tempStatus FROM [Status] SELECT * FROM #tempStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistAcct(Chart objChart)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Chart WHERE Acct= '" + objChart.Acct + "' )THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistAcctForEdit(Chart objChart)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Chart WHERE Acct= '" + objChart.Acct + "' AND ID <> "+ objChart.ID +" )THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAutoFillAccount(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGetAccountSearch", _objChart.SearchValue);
                //SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "SELECT ID, Acct, fDesc, Balance, Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status, Sub2, DAT, Branch, CostCenter, AcctRoot,QBAccountID,LastUpdateDate FROM Chart Order by Acct");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccountData(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGetChartSearch", _objChart.SearchIndex, _objChart.SearchBy, _objChart.Condition, _objChart.SearchAcctType, _objChart.Sub, _objChart.SearchStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankAcct(Chart _objChart)             // Cash in bank account
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1000%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAcctReceivable(Chart _objChart)       // Account Receivable
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1200%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUndepositeAcct(Chart _objChart)       // Undeposited Fund
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1100%' AND Status=0 ORDER BY ID");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAcctPayable(Chart _objChart)          // Account Payable
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D2000%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D2000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesTaxAcct(Chart _objChart)         // Sales tax
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D2100%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D2100' AND Status=0 ORDER BY ID "); //D2120
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRetainedEarn(Chart _objChart)         // Retained Earning
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3920%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3920' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCurrentEarn(Chart _objChart)          // Current Earning
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3130%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3130' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStock(Chart _objChart)                // Stock 
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3110%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3110' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDistribution(Chart _objChart)         // Distribution
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3140%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3140' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateChartBalance(Chart _objChart)
        {
            SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spUpdateChartBalance", _objChart.ID, _objChart.Amount);
        }
        public DataSet GetChartByID(Chart _objChart) // By Viral
        {
            try
            {
                StringBuilder varname11 = new StringBuilder();
                varname11.Append(" \n");
                varname11.Append("select c.ID, \n");
                varname11.Append("       t.Acct, \n");
                varname11.Append("       t.fDate, \n");
                varname11.Append("       t.Batch, \n");
                varname11.Append("       t.Ref, \n");
                varname11.Append("       (CASE t.Type WHEN 50 THEN '1'");
                varname11.Append("       WHEN 40 THEN '2' ");
                varname11.Append("       WHEN 41 THEN '2' ");
                varname11.Append("       WHEN 21 THEN '3' ");
                varname11.Append("       WHEN 20 THEN '3' ");
                varname11.Append("       WHEN 5 THEN '4' ");
                varname11.Append("       WHEN 6 THEN '4' ");
                varname11.Append("       WHEN 5 THEN '5' ");
                varname11.Append("        WHEN 6 THEN '5' ");
                varname11.Append("       WHEN 1 THEN '6' ");
                varname11.Append("       WHEN 2 THEN '6' ");
                varname11.Append("       WHEN 3 THEN '6' ");
                varname11.Append("       WHEN 40 THEN '8' ");
                varname11.Append("       WHEN 40 THEN '8' ");
                varname11.Append("       WHEN 98 THEN '9' ");
                varname11.Append("       WHEN 99 THEN '9' END) as Type, \n");
                varname11.Append("       t.Ref, \n");
                varname11.Append("       c.fDesc as ChartfDesc, \n");
                varname11.Append("       t.fDesc, \n");
                varname11.Append("         t.Amount, \n");
                varname11.Append("         c.Balance \n");
                varname11.Append("        FROM   Chart c \n");
                varname11.Append("        INNER JOIN Trans t \n");
                varname11.Append("        on c.ID=t.Acct \n");
                varname11.Append("        WHERE  c.ID=" + _objChart.ID + " Order by t.fDate");

                //return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "select c.ID,t.Acct,t.fDate,t.Batch,t.Ref,(CASE t.Type WHEN 50 THEN '1' WHEN 40 THEN '2' WHEN 41 THEN '2'  WHEN 21 THEN '3' WHEN 20 THEN '3' WHEN 5 THEN '4' WHEN 6 THEN '4' WHEN 5 THEN '5' WHEN 6 THEN '5' WHEN 1 THEN '6' WHEN 2 THEN '6' WHEN 3 THEN '6' WHEN 40 THEN '8' WHEN 41 THEN '8' end)As Type,t.Ref,c.fDesc as ChartfDesc,t.fDesc,t.Amount,c.Balance from Chart c join Trans t on c.ID=t.Acct where c.ID=" + objChart.ID);          
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllChartByDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" \n");
                varname.Append("select c.ID, \n");
                varname.Append("       t.Acct, \n");
                varname.Append("       t.fDate, \n");
                varname.Append("       t.Batch, \n");
                varname.Append("       t.Ref, \n");
                varname.Append("       (CASE t.Type WHEN 50 THEN '1'");
                varname.Append("       WHEN 40 THEN '2' ");
                varname.Append("       WHEN 41 THEN '2' ");
                varname.Append("       WHEN 21 THEN '3' ");
                varname.Append("       WHEN 20 THEN '3' ");
                varname.Append("       WHEN 5 THEN '4' ");
                varname.Append("       WHEN 6 THEN '4' ");
                varname.Append("       WHEN 5 THEN '5' ");
                varname.Append("       WHEN 6 THEN '5' ");
                varname.Append("       WHEN 1 THEN '6' ");
                varname.Append("       WHEN 2 THEN '6' ");
                varname.Append("       WHEN 3 THEN '6' ");
                varname.Append("       WHEN 40 THEN '8' ");
                varname.Append("       WHEN 41 THEN '8' ");
                varname.Append("       WHEN 98 THEN '9' ");
                varname.Append("       WHEN 99 THEN '9' ");
                varname.Append("       WHEN 30 THEN '7' ");
                varname.Append("       WHEN 31 THEN '7' ");
                varname.Append("       ELSE t.Type END) as Type, \n");
                varname.Append("       t.Ref, \n");
                varname.Append("       c.fDesc as ChartfDesc, \n");
                varname.Append("       t.fDesc, \n");
                varname.Append("         t.Amount, \n");
                varname.Append("         c.Balance \n");
                varname.Append("        FROM   Chart c \n");
                varname.Append("        INNER JOIN Trans t \n");
                varname.Append("        on c.ID=t.Acct \n");
                varname.Append("        WHERE c.ID=" + _objChart.ID + " AND (t.fDate >= '" + _objChart.StartDate + "') AND (t.fDate <= '" + _objChart.EndDate + "') ORDER BY t.fDate");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
                //return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "select c.ID,t.Acct,t.fDate,t.Batch,t.Ref,(CASE t.Type WHEN 50 THEN '1' WHEN 40 THEN '2' WHEN 41 THEN '2'  WHEN 21 THEN '3' WHEN 20 THEN '3' WHEN 5 THEN '4' WHEN 6 THEN '4' WHEN 5 THEN '5' WHEN 6 THEN '5' WHEN 1 THEN '6' WHEN 2 THEN '6' WHEN 3 THEN '6' WHEN 40 THEN '8' WHEN 41 THEN '8' end)As Type,t.Ref,c.fDesc as ChartfDesc,t.fDesc,t.Amount,c.Balance from Chart c join Trans t on c.ID=t.Acct where (t.fDate >= '" + objChart.StartDate + "') AND (t.fDate <= '" + objChart.EndDate + "') ORDER BY t.fDate");
                //return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT DISTINCT g.fDate, g.Ref, g.Internal, g.fDesc, g.Batch FROM GLA as g, Trans as t where (g.Batch = t.Batch) AND (g.Batch = t.Batch) AND (t.Type = 50) AND (g.fDate >= '" + objJournal.StartDate + "') AND (g.fDate <= '" + objJournal.EndDate + "') ORDER BY g.Ref DESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMinTransDate(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT Min(fDate) AS MinDate FROM Trans");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfRevenueByDate(Chart _objChart) //To get Revenue Total details
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("	ISNULL(SUM(t.Amount),0.00) \n");
                varname.Append("    FROM Trans as t, Chart as c  \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 3 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate > '" + _objChart.StartDate + "' AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfCostSalesByDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 4 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate > '" + _objChart.StartDate + "' AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfExpenseByDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 5 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate > '" + _objChart.StartDate + "' AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsChartBankAcct(Chart _objChart)
        {

            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  CAST( \n");
                varname.Append("    CASE WHEN EXISTS(SELECT * FROM Chart  \n");
                varname.Append("    WHERE ID = " + _objChart.ID + " AND Type = 6) \n");
                varname.Append("    THEN 1 \n");
                varname.Append("    ELSE 0 \n");
                varname.Append("    END \n");
                varname.Append("    AS BIT)  \n");
                return _objChart.IsBankAcct = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankBalance(Chart _objChart)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spUpdateBankBalance", _objChart.ID, _objChart.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccountLedger(Chart _objChart)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spAccountLedger", _objChart.ID, _objChart.StartDate, _objChart.EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CalChartBalance(Chart _objChart)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spCalChartBalance");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankCharge(Chart _objChart)             // Bank Charges
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D6000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOAcct(Chart _objChart)       // Purchase Order
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D9991' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChartByAcct(Chart objChart)
        {
            try
            {
                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "SELECT * FROM Chart WHERE Acct = '" + objChart.Acct + "' AND Type<>7 ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Inventory Implementation
        public List<Chart> GetChartByType(int type)
        {
            DataSet ds = null;
            List<Chart> chart = new List<Chart>();
            try
            {
                string constring = string.Empty;
                if (HttpContext.Current.Session["config"] != null)
                {
                    constring = HttpContext.Current.Session["config"].ToString();
                }

                if (string.IsNullOrEmpty(constring))
                    return chart;

                ds = SqlHelper.ExecuteDataset(constring, "spGetChartByType", type);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Chart objchart = new Chart();
                            objchart.ID = ds.Tables[0].Rows[i]["ID"] != DBNull.Value ? (int)ds.Tables[0].Rows[i]["ID"] : 0;
                            objchart.Acct = ds.Tables[0].Rows[i]["Acct"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Acct"] : "";
                            objchart.fDesc = ds.Tables[0].Rows[i]["fDesc"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["fDesc"] : "";

                            chart.Add(objchart);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chart;
        }
        public double GetSumOfRevenueByAsOfDate(Chart _objChart) //To get Revenue Total details
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("	ISNULL(SUM(t.Amount),0.00) \n");
                varname.Append("    FROM Trans as t, Chart as c  \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 3 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfCostSalesByAsOfDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 4 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfExpenseByAsOfDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 5 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
