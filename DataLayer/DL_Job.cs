using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web;

namespace DataLayer
{
    public class DL_Job
    {
        public DataSet GetAllJobType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID,Type,Count,Color,Remarks,IsDefault FROM JobType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID,Type,Count,Color,Remarks,IsDefault FROM JobType Where ID <> 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetContractType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT Type, fDesc, MatCharge, Reg, OT,DT, WReg, WOT, WDT, WCharge, HReg, HOT, HDT, HCharge, Count, Remarks, Serv, LTest, NT, Travel, fOver, WNT, HNT, NonContract, Free, FGL, En FROM LType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvService(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label FROM Inv WHERE Type = 1 AND Status = 0 ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPosting(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Post FROM Posting");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCode(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID as value, Code as label FROM JobCode");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllUM(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID as value, fDesc as label FROM UM");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllInvDetails(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label FROM Inv WHERE Status = 0 ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Department, ID as value, Department as label FROM OrgDep ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobStatus(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, " SELECT IDENTITY (INT, 0, 1) AS ID, Status INTO #tempJStatus FROM [JStatus] SELECT * FROM #tempJStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobTFinanceByID(JobT _objJob)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT j.ID, j.fDesc,   \n");
                varname.Append("         j.InvExp, c.fDesc as InvExpName, j.InvServ, i.Name AS InvServiceName, j.Wage, p.fDesc as WageName,     \n");
                varname.Append("         j.GLInt, j.CType, j.Post, j.Charge, j.fInt, j.JobClose,\n");
                varname.Append("         (SELECT c.fdesc from JobT j left join Chart c on j.GLInt = c.ID where j.ID=" + _objJob.ID + " ) as GLName \n");
                varname.Append("    FROM JobT j LEFT JOIN   \n");
                varname.Append("        PRWage p on j.Wage = p.ID LEFT JOIN     \n");
                varname.Append("        Inv i on j.InvServ = i.ID LEFT JOIN     \n");
                varname.Append("        Chart c on j.InvExp = c.ID              \n");
                varname.Append("        	WHERE j.id=" + _objJob.ID + "           \n");
                //varname.Append("     \n");
                //varname.Append("    SELECT j.JobTID, t.* \n");
                //varname.Append("        FROM tblCustomJobT j INNER JOIN tblCustomFields t \n");
                //varname.Append("        ON t.ID = j.tblCustomFieldsID WHERE j.JobTID = " + _objJob.ID + "  \n");
                //varname.Append("     \n");
                //varname.Append("    SELECT j.JobTID, t.* \n");
                //varname.Append("        FROM tblCustomJobT j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID\n");
                //varname.Append("        RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID WHERE j.JobTID = " + _objJob.ID + "  \n");
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBomType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Type FROM BOMT ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTabByPageUrl(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT t.ID, t.tblPageID, t.TabName FROM tblTabs t INNER JOIN tblPages p ON p.ID = t.tblPageID WHERE p.URL = '" + _objJob.PageUrl + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRecurringCustom(JobT objJob)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "JobId";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objJob.Job;

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetRecurCustomFields", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetRecurringJobCustom(JobT _objJob)
        //{
        //    StringBuilder QueryText = new StringBuilder();
        //    QueryText.Append("        SELECT jt.ID AS JobT, t.* ,     \n");
        //    QueryText.Append("         		(CASE t.Format WHEN 1 THEN 'Currency'    \n");
        //    QueryText.Append("                  WHEN 2 THEN 'Date'                \n");
        //    QueryText.Append("        		    WHEN 3 THEN 'Text'              \n");
        //    QueryText.Append("     		        WHEN 4 THEN 'Dropdown'                    \n");
        //    QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl     \n");
        //    QueryText.Append("       	        ,j.Value as Value    \n");
        //    QueryText.Append("       	 FROM tblCustomJob j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID    \n");
        //    QueryText.Append("           INNER JOIN Job jt ON jt.ID = j.JobID   \n");
        //    QueryText.Append("           WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)  \n");
        //    QueryText.Append("              \n");
        //    QueryText.Append("      SELECT jt.ID AS JobT, t.*, tc.Label, tc.Format, tc.tblTabID,  \n");
        //    QueryText.Append("      		(CASE tc.Format WHEN 1 THEN 'Currency'  \n");
        //    QueryText.Append("                  WHEN 2 THEN 'Date'    \n");
        //    QueryText.Append("      	        WHEN 3 THEN 'Text'    \n");
        //    QueryText.Append("     	            WHEN 4 THEN 'Dropdown'   \n");
        //    QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl   \n");
        //    QueryText.Append("     	    FROM tblCustomJob j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID   \n");
        //    QueryText.Append("          RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID  \n");
        //    QueryText.Append("          INNER JOIN Job jt ON jt.ID = j.JobID               \n");
        //    QueryText.Append("          WHERE jt.ID = " + _objJob.ID + " AND (tc.IsDeleted is null OR tc.IsDeleted = 0)      \n");
        //    try
        //    {
        //        return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public bool IsExistRecurrJobT(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("      SELECT CAST(Count(*) AS BIT) FROM JobT WHERE TYPE = 0 AND Status = 0 \n");
                if (!_objJob.ID.Equals(0))
                {
                    QueryText.Append("   AND ID!='" + _objJob.ID + "'       \n");
                }
                return _objJob.IsExistRecurr = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.Text, QueryText.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectTemplateCustomFields(JobT objJob)
        {
            try
            {
                var param = new SqlParameter[2];

                param[0] = new SqlParameter
                {
                    ParameterName = "jobt",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.ID
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetProjectTemplateCustomFields", param);

                //StringBuilder QueryText = new StringBuilder();
                //QueryText.Append("        SELECT jt.ID AS JobT,jt.type, t.* ,     \n");
                //QueryText.Append("         		(CASE t.Format WHEN 1 THEN 'Currency'    \n");
                //QueryText.Append("                  WHEN 2 THEN 'Date'                \n");
                //QueryText.Append("        		    WHEN 3 THEN 'Text'              \n");
                //QueryText.Append("     		        WHEN 4 THEN 'Dropdown'                    \n");
                //QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl     \n");
                //QueryText.Append("       	        ,j.Value as Value    \n");
                //QueryText.Append("       	 FROM tblCustomJobT j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID    \n");
                //QueryText.Append("           INNER JOIN JobT jt ON jt.ID = j.JobTID   \n");
                //QueryText.Append("           WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)   \n");
                //QueryText.Append("              \n");
                //QueryText.Append("      SELECT jt.ID AS JobT, t.*, tc.Label, tc.Format, tc.tblTabID,  \n");
                //QueryText.Append("      		(CASE tc.Format WHEN 1 THEN 'Currency'  \n");
                //QueryText.Append("                  WHEN 2 THEN 'Date'    \n");
                //QueryText.Append("      	        WHEN 3 THEN 'Text'    \n");
                //QueryText.Append("     	            WHEN 4 THEN 'Dropdown'   \n");
                //QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl   \n");
                //QueryText.Append("     	    FROM tblCustomJobT j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID   \n");
                //QueryText.Append("          RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID  \n");
                //QueryText.Append("          INNER JOIN JobT jt ON jt.ID = j.JobTID               \n");
                //QueryText.Append("          WHERE jt.ID = " + _objJob.ID + " AND (tc.IsDeleted is null OR tc.IsDeleted = 0)     \n");
                //return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectCustomTab(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("        SELECT t.tblTabID \n");
                QueryText.Append("         		FROM tblCustomJobT j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID      \n");
                QueryText.Append("              INNER JOIN JobT jt ON jt.ID = j.JobTID                   \n");
                QueryText.Append("        		WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)               \n");
                QueryText.Append("     		    GROUP by tblTabID                   \n");
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistProjectTempByType(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("      SELECT CAST(Count(*) AS BIT) FROM JobT ");
                QueryText.Append("      WHERE Type = '" + _objJob.Type + "' AND ID = '" + _objJob.ID + "' ");
                return _objJob.IsExist = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.Text, QueryText.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobTById(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT * FROM JobT WHERE ID='" + _objJob.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataByUM(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT ID as value, fDesc as label FROM UM WHERE fDesc = '" + objJob.UM + "'    ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostByJob(JobT objJob)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "job";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objJob.Job;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostByJob", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTypeItemByExpCode(JobT objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                //QueryText.Append("     SELECT '" + objJob.Code + "' as Code , b.Type as Item, 1 as Type, 'cost' as TypeName,      \n");
                QueryText.Append("      SELECT '" + objJob.Code + "' as Code, b.type as ItemTypeId, '" + objJob.Type + "' as Type,   \n");
                QueryText.Append("          bt.Type as Item,        \n");
                QueryText.Append("          'Cost' as TypeName,     \n");
                QueryText.Append("      isnull(sum(j.Budget),0) as TotalBudget, isnull(sum(j.Actual),0) as TotalActual, isnull(sum(j.Budget),0) - isnull(sum(j.Actual),0) as TotalVariance,   \n");
                QueryText.Append("      0.00 as TotalCommited, 0.00 as TotalOutstand      \n");
                QueryText.Append("      FROM JobTItem j         \n");
                QueryText.Append("      INNER JOIN Bom b ON j.ID = b.JobTItemID     \n");
                QueryText.Append("          LEFT JOIN BOMT bt on b.Type = bt.ID     \n");
                QueryText.Append("          WHERE j.Type = 1 and j.Job = '" + objJob.Job + "'        \n");
                QueryText.Append("          and j.code = '" + objJob.Code + "'      \n");
                QueryText.Append("          GROUP by b.Type, bt.Type    \n");
                QueryText.Append("          ORDER BY bt.Type");
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistExpJobItemByJob(JobT objJob)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objJob.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM JobI as ji INNER JOIN JobTItem as j ON j.Job = ji.Job AND j.Line = ji.Phase INNER JOIN Trans as t ON ji.TransID = t.ID WHERE ji.Job = '" + objJob.Job + "' AND ji.Phase = '" + objJob.Phase + "' and j.Type = 1 and t.Type = 41)THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistRevJobItemByJob(JobT objJob)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objJob.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM JobI as ji INNER JOIN JobTItem as j ON j.Job = ji.Job AND j.Line = ji.Phase INNER JOIN Trans as t ON ji.TransID = t.ID WHERE ji.Job = '" + objJob.Job + "' AND ji.Phase = '" + objJob.Phase + "' and j.Type = 0 and (t.Type = 2 or t.Type = 4))THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRevenueJobItemsByJob(JobT objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("      SELECT Top 1 *                                \n");
                QueryText.Append("          FROM JobTItem as jobt               \n");
                QueryText.Append("          INNER JOIN Milestone as m           \n");
                QueryText.Append("              ON jobt.ID = m.JobTItemID       \n");
                QueryText.Append("              WHERE jobt.Type = 0 AND jobt.Line='" + objJob.Line + "' AND jobt.Job = '" + objJob.Job + "'    \n");

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int16 AddBOMItem(JobT objJob)
        {
            try
            {
                var param = new SqlParameter[12];

                param[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "fdesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.fDesc
                };
                param[2] = new SqlParameter
                {
                    ParameterName = "code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Code
                };
                param[3] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                param[4] = new SqlParameter
                {
                    ParameterName = "item",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.ItemID
                };
                param[5] = new SqlParameter
                {
                    ParameterName = "qty",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.QtyReq
                };
                param[6] = new SqlParameter
                {
                    ParameterName = "um",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.UM
                };
                param[7] = new SqlParameter
                {
                    ParameterName = "scrapfactor",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.ScrapFact
                };
                param[8] = new SqlParameter
                {
                    ParameterName = "budgetunit",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.BudgetUnit
                };
                param[9] = new SqlParameter
                {
                    ParameterName = "budgetext",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.BudgetExt
                };
                param[10] = new SqlParameter
                {
                    ParameterName = "IsDefault",
                    SqlDbType = SqlDbType.Bit,
                    Value = objJob.IsDefault
                };
                param[11] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spAddBOMItem", param);
                objJob.Line = Convert.ToInt16(param[10].Value);
                return objJob.Line;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostCodeByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostCodeByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryItem(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label, fDesc FROM Inv WHERE Status = 0 AND (Type = 0 OR Type = 2) ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostTypeByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Code
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostTypeByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostInvoicesByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Code
                };
                para[3] = new SqlParameter          // Bom.Type or Milestone.Type Id
                {
                    ParameterName = "typeId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.TypeId
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostInvoicesByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBOMTByTypeName(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT top 1 ID AS Value, Type AS Label FROM BOMT WHERE Type like '%" + objJob.TypeName + "%'   ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPhaseExpByJobType(JobT objJob) // get phase expense type details
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.SearchValue
                };
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, "spGetPhaseExpByJobType", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostTicketsByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Code
                };
                para[2] = new SqlParameter          // Bom.Type 
                {
                    ParameterName = "typeId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.TypeId
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostTicketsByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProfitLossValues(JobT objJob)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "job";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objJob.Job;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostAmountByJob", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllJobTypeForSearch(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllJobTypeForAjaxSearch(int type)
        {


            DataSet ds = new DataSet();


            try
            {

                ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["config"].ToString(), "spGetProjectDetails", type);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }
    }
}
