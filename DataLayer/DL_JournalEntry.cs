using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_JournalEntry
    {
        public void DeleteGLA(Journal objJournal)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, " DELETE FROM GLA WHERE Batch = " + objJournal.BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTrans(Journal objJournal)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, " DELETE FROM Trans WHERE Batch = " + objJournal.BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTransByID(Transaction objTrans)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, " DELETE FROM Trans WHERE ID = " + objTrans.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataByRef(Journal objJournal)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT Ref, fDate, Internal, fDesc,     \n");
                if (objJournal.IsRecurring.Equals(true))
                {
                    varname.Append("            Frequency, (select CASE WHEN EXISTS(select 1 from GLARecurI where Job > 0 and Ref = '"+ objJournal.Ref +"') THEN 1 ELSE 0 END as bit) as IsJobSpec  \n");
                    varname.Append("            FROM GLARecur WHERE Ref = '" + objJournal.Ref + "'      \n");
                }
                else
                {
                    varname.Append("            Batch, (select CASE WHEN EXISTS(select 1 from Trans where VInt > 0 and Ref ='" + objJournal.Ref + "') THEN 1 ELSE 0 END as bit) as IsJobSpec       \n");
                    varname.Append("            FROM GLA WHERE Ref = '" + objJournal.Ref + "'           \n");
                }
                return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataByBatch(Journal objJournal)
        {
            try
            {
                return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT distinct t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, isnull(td.IsRecon,0) as IsRecon FROM Trans as t left join TransDeposits as td on t.Batch=td.Batch WHERE t.Batch = "+objJournal.BatchID+" AND (t.Type = 50 or t.Type = 30 or t.Type = 31)");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMaxTransID(Journal objJournal)
        {
            try
            {
                return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(ID),0)+1 AS MAXID FROM Trans");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransRef(Journal objJournal)
        {
            try
            {
                return objJournal.Ref = Convert.ToInt32(SqlHelper.ExecuteScalar(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(Ref),0)+1 AS MAXRef FROM GLA"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransBatch(Journal objJournal)
        {
            try
            {
                //return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans");
                return objJournal.BatchID = Convert.ToInt32(SqlHelper.ExecuteScalar(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllTransaction(Journal objJournal)
        {
            try
            {
                return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT Trans.ID AS TransID, Chart.Acct, Chart.fDesc as ChartDesc, Trans.fDesc AS TransDesc, Trans.Amount FROM Chart INNER JOIN Trans ON Trans.Acct = Chart.ID WHERE (Trans.Type = 50) AND (Trans.ID = " + objJournal.TransID + ")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddGLA(Journal objJournal)
        {
            try
            {
                string query = "INSERT INTO GLA (Ref,fDate,Internal,fDesc,Batch) VALUES (@Ref,@Date,@Internal,@Desc,@Batch)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objJournal.Ref));
                parameters.Add(new SqlParameter("@Date", objJournal.GLDate));
                parameters.Add(new SqlParameter("@Internal", objJournal.Internal));
                parameters.Add(new SqlParameter("@Desc", objJournal.GLDesc));
                parameters.Add(new SqlParameter("@Batch", objJournal.BatchID));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJournalTrans(Transaction objTrans)
        {
            var para = new SqlParameter[17];

            para[1] = new SqlParameter
            {
                ParameterName = "@Batch",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.BatchID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@fDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objTrans.TransDate
            };
            if (objTrans.Type != 0)
            {
                para[3] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTrans.Type
                };
            }
            para[4] = new SqlParameter
            {
                ParameterName = "@Line",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Line
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Ref",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Ref
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objTrans.TransDescription
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Float,
                Value = objTrans.Amount
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Acct",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Acct
            };
            if (objTrans.AcctSub != null)
            {
                para[9] = new SqlParameter
                {
                    ParameterName = "@AcctSub",
                    SqlDbType = SqlDbType.Int,
                    Value = objTrans.AcctSub
                };
            }
            para[10] = new SqlParameter
            {
                ParameterName = "@Sel",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Sel
            };
            if (objTrans.JobInt != 0)
            {
                para[12] = new SqlParameter
                {
                    ParameterName = "@VInt",
                    SqlDbType = SqlDbType.Int,
                    Value = objTrans.JobInt
                };
            }
            if (objTrans.PhaseDoub != 0.0)
            {
                para[13] = new SqlParameter
                {
                    ParameterName = "@VDoub",
                    SqlDbType = SqlDbType.Float,
                    Value = objTrans.PhaseDoub
                };
            }
            if (!string.IsNullOrEmpty(objTrans.strRef))
            {
                para[15] = new SqlParameter
                {
                    ParameterName = "@strRef",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTrans.strRef
                };
            }
            para[16] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            try
            {
                //SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                return Convert.ToInt32(para[16].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetDataByBatchRef(Transaction objTrans) // For journal entry
        //{
        //    try
        //    {
        //        return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName, t.TimeStamp, isnull(td.IsRecon,0) as IsRecon FROM Trans as t inner join Chart as c on t.Acct = c.ID left join TransDeposits as td on t.batch=td.Batch WHERE (t.Type = 50 or t.Type = 30 or t.Type = 31) AND t.Ref ="+objTrans.Ref+" AND t.Batch = "+objTrans.BatchID+" Order By t.Line");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void UpdateGLA(Journal objJournal)
        {
            try
            {

                string query = "UPDATE GLA SET fDate = @fDate, Internal = @Internal, fDesc = @fDesc WHERE Ref = @Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objJournal.Ref));
                parameters.Add(new SqlParameter("@fDate", objJournal.GLDate));
                parameters.Add(new SqlParameter("@Internal", objJournal.Internal));
                parameters.Add(new SqlParameter("@fDesc", objJournal.GLDesc));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateJournalTrans(Transaction objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDate = @fDate, fDesc = @fDesc, Amount = @Amount, Acct = @Acct, VInt = @VInt, VDoub=@VDoub WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objTrans.ID));
                parameters.Add(new SqlParameter("@fDate", objTrans.TransDate));
                parameters.Add(new SqlParameter("@fDesc", objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Amount", objTrans.Amount));
                parameters.Add(new SqlParameter("@Acct", objTrans.Acct));
                parameters.Add(new SqlParameter("@VInt", objTrans.JobInt));
                parameters.Add(new SqlParameter("@VDoub", objTrans.PhaseDoub));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetAllJE(Journal objJournal)
        //{
        //    try
        //    {
        //        return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT DISTINCT g.fDate, g.Ref, g.Internal, g.fDesc, g.Batch FROM GLA as g, Trans as t where (g.Batch = t.Batch) AND (g.Batch = t.Batch) AND (t.Type = 50) ORDER BY g.Ref DESC");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetJobsLoc(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, "spGetJobLocSearch", objTrans.SearchValue, objTrans.IsJob);
                //    return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT j.ID AS JID, j.fDesc AS fDesc, l.Tag AS Tag FROM Job as j, Loc as l where j.Loc=l.Loc order by j.Loc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLocByJobID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT j.ID AS ID, j.fDesc AS fDesc, l.Tag AS Tag FROM Job as j, Loc as l WHERE j.Loc=l.Loc and j.ID=" + objTrans.JobInt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllJEByDate(Journal objJournal)
        {
            try
            {
                //return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT DISTINCT g.fDate, g.Ref, g.Internal, g.fDesc, g.Batch FROM GLA as g, Trans as t where (g.Batch = t.Batch) AND (g.Batch = t.Batch) AND (t.Type = 50 or t.Type = 30 or t.Type = 31) AND (g.fDate >= '" + objJournal.StartDate + "') AND (g.fDate <= '" + objJournal.EndDate + "') ORDER BY g.Ref DESC");
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@startdate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = objJournal.StartDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@enddate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = objJournal.EndDate;
                return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, "spGetJournalAdj", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobDetailByID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT ID, fDesc, Loc FROM Job Where ID='" + objTrans.JobInt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPhaseByID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT ID, JobT, Job, Type, fDesc, Code, Actual, Line FROM JobTItem WHERE ID='" + objTrans.PhaseDoub + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateJournalTransAmount(Transaction objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc, fDate=@fDate, Amount = @Amount WHERE ID = @ID AND Type = @Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objTrans.ID));
                parameters.Add(new SqlParameter("@fDate", objTrans.TransDate));
                parameters.Add(new SqlParameter("@Amount", objTrans.Amount));
                parameters.Add(new SqlParameter("@Type", objTrans.Type));
                parameters.Add(new SqlParameter("@fDesc", objTrans.TransDescription));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentTransByBatchRef(Transaction objTrans)
        {
            try
            {
                //return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "DECLARE @Batch int SELECT @Batch=Batch FROM Trans WHERE ID=" + objTrans.ID + " SELECT Acct, AcctSub, Amount, Batch, EN, ID, Line, Ref, Sel, Status, Type, VDoub, VInt, fDate, fDesc, strRef FROM Trans WHERE Batch = @Batch AND Ref = " + objTrans.Ref);
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT Acct, Batch ,fDesc, Amount, EN, ID, Line, Ref, Sel, Status, Type, VDoub, VInt, fDate, strRef, AcctSub FROM Trans where batch=(select batch from Trans where ID=" + objTrans.ID+")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE ID=" + objTrans.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInvoiceTransDetails(Transaction _objTrans)
        {
            try
            {
                //string query = "UPDATE Invoice SET Batch=@Batch, TransID=@TransID WHERE Ref=@Ref";
                //List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@Ref", _objInvoices.Ref));
                //parameters.Add(new SqlParameter("@Batch", _objInvoices.Batch));
                //parameters.Add(new SqlParameter("@TransID", _objInvoices.TransID));
                string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objTrans.Ref));
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void UpdateBillTrans(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDate=@fDate, Amount=@Amount WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objTrans.ID));
                parameters.Add(new SqlParameter("@fDate", _objTrans.TransDate));
                parameters.Add(new SqlParameter("@Amount", _objTrans.Amount));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetOpenTrans(Transaction _objOpenTrans)
        //{
        //    try
        //    {
        //        return _objOpenTrans.DsTrans = SqlHelper.ExecuteDataset(_objOpenTrans.ConnConfig, CommandType.Text, "SELECT ID ,fDate ,Type ,Ref ,fDesc ,Amount ,Rec FROM OpenTrans");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void AddTransBankAdj(TransBankAdj _objTrans)
        {
            var para = new SqlParameter[5];

            para[1] = new SqlParameter
            {
                ParameterName = "@Batch",
                SqlDbType = SqlDbType.Int,
                Value = _objTrans.Batch
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Bank",
                SqlDbType = SqlDbType.Int,
                Value = _objTrans.Bank
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@IsRecon",
                SqlDbType = SqlDbType.Bit,
                Value = _objTrans.IsRecon
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Decimal,
                Value = _objTrans.Amount
            };
            try
            {
                SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.StoredProcedure, "spAddTransBankAdj", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransCheckRecon(TransBankAdj _objTrans)
        {
            try
            {
                string query = "UPDATE TransChecks SET IsRecon=@IsRecon WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objTrans.IsRecon));
                parameters.Add(new SqlParameter("@Batch", _objTrans.Batch));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransDepositRecon(TransBankAdj _objTrans)
        {
            try
            {
                string query = "UPDATE TransDeposits SET IsRecon=@IsRecon WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objTrans.IsRecon));
                parameters.Add(new SqlParameter("@Batch", _objTrans.Batch));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransSel(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Sel=@Sel WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objTrans.ID));
                parameters.Add(new SqlParameter("@Sel", _objTrans.Sel));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatchBank(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Type = 41 AND t.Batch = " + _objTrans.BatchID + " Order By t.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateClearItem(Transaction _objTrans)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objTrans.ConnConfig, "spUpdateClearItems", _objTrans.BatchID, _objTrans.Sel, _objTrans.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatchRef(Transaction _objTrans) // Transaction details
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, " SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Ref = " + _objTrans.Ref + " AND t.Batch = " + _objTrans.BatchID + " Order By t.Line");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, " SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Batch = " + _objTrans.BatchID + " Order By t.Line");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillAPTransByBatch(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, isnull(t.Sel,0) as Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Type = 40 AND t.Batch = " + _objTrans.BatchID + " Order By t.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransDateByBatch(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDate=@fDate, Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                parameters.Add(new SqlParameter("@fDate", _objTrans.TransDate));
                parameters.Add(new SqlParameter("@Ref", _objTrans.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBatchDetailsByID(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE ID=" + _objTrans.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheck(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc WHERE ID=@ID AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objTrans.ID));
                parameters.Add(new SqlParameter("@fDesc", _objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objTrans.Type));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheckByBatch(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Sel=@Sel WHERE Batch=@Batch AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                parameters.Add(new SqlParameter("@fDesc", _objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objTrans.Type));
                parameters.Add(new SqlParameter("@Sel", _objTrans.Sel));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatchType(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE Batch=" + _objTrans.BatchID +" AND Type="+_objTrans.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValidateByTimeStamp(Transaction _objTrans)
        {
            try
            {
                return _objTrans.IsAccessible = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objTrans.ConnConfig, "spCheckTimeStampByID", _objTrans.TableName, _objTrans.ID, _objTrans.TimeStamp));
                //return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT j.ID AS JID, j.fDesc AS fDesc, l.Tag AS Tag FROM Job as j, Loc as l where j.Loc=l.Loc order by j.Loc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteBillTrans(Transaction _objTrans)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, "Delete from Trans where batch="+_objTrans.BatchID+" and type=41");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTransDeposit(Transaction _objTrans)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, "Delete from TransDeposits where batch=" + _objTrans.BatchID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTransChecks(Transaction _objTrans)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, "Delete from TransChecks where batch=" + _objTrans.BatchID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransCheckNoByBatch(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                //parameters.Add(new SqlParameter("@fDate", _objTrans.TransDate));
                parameters.Add(new SqlParameter("@Ref", _objTrans.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPhaseByJobId(Transaction objTrans) // get phase expense type details
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobId",
                    SqlDbType = SqlDbType.Int,
                    Value = objTrans.JobInt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objTrans.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTrans.SearchValue
                };
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, "spGetPhaseByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransDataByBatch(Transaction objTrans)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("   SELECT t.Acct as AcctID, t.AcctSub,(case when(t.Amount > 0) then (t.amount) else 0.00 end) as Debit,(case when(t.Amount < 0) then (t.amount*-1) else 0.00 end) as Credit, \n");
                varname1.Append("       t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel,    \n");
                varname1.Append("       t.Status, t.Type, isnull(t.VDoub,0) as PhaseID, isnull(t.VInt,0) as JobID, t.fDate, t.fDesc as Description, t.strRef, c.Acct AS AcctNo, c.fDesc AS Account,     \n");
                varname1.Append("       t.TimeStamp, isnull(td.IsRecon,0) as IsRecon, l.Tag as Loc,     \n");
                varname1.Append("       j.fDesc as JobName, jobt.fDesc as Phase, jobt.Line as PhaseID   \n");
                varname1.Append("       FROM Trans as t                             \n");
                varname1.Append("           inner join Chart as c on t.Acct = c.ID  \n");
                varname1.Append("           left join TransDeposits as td on t.batch=td.Batch   \n");
                varname1.Append("           left join Job as j on j.ID = t.VInt     \n");
                varname1.Append("           left join Loc as l on l.Loc = j.Loc     \n");
                varname1.Append("           left join JobTItem as jobt on jobt.Line = t.VDoub and jobt.Job = t.VInt     \n");
                varname1.Append("           WHERE (t.Type = 50 or t.Type = 30 or t.Type = 31)                       \n");
                //varname1.Append("           AND t.Ref = '"+ objTrans.Ref +"' AND t.Batch = '"+ objTrans.BatchID +"' Order By t.Line     \n");
                varname1.Append("           AND t.Batch = '" + objTrans.BatchID + "' Order By t.Line     \n");
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPhaseByJobID(Transaction objTrans)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT jobt.ID, jobt.JobT, jobt.Job, jobt.Type, jobt.fDesc, jobt.Code, jobt.Actual, jobt.Line, l.Tag as LocName, j.fDesc as JobName     \n");
                varname.Append("            FROM JobTItem as jobt   \n");
                varname.Append("            inner join job as j on j.ID = jobt.Job  \n");
                varname.Append("            inner join loc as l on l.Loc = j.Loc    \n");
                if(objTrans.JobInt > 0)
                {
                    varname.Append("        WHERE jobt.Job = '"+ objTrans.JobInt+"'  \n");
                }
                else
                {
                    varname.Append("        WHERE jobt.Job <> 0 OR jobt.Job <> null       \n");
                }
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ProcessRecurJE(Journal objJournal)
        {
            SqlParameter paraRef = new SqlParameter();
            paraRef.ParameterName = "Ref";
            paraRef.SqlDbType = SqlDbType.Int;
            paraRef.Value = objJournal.Ref;

            SqlParameter paraReturn = new SqlParameter();
            paraReturn.ParameterName = "returnval";
            paraReturn.SqlDbType = SqlDbType.Int;
            paraReturn.Direction = ParameterDirection.ReturnValue;

            try
            {
                //SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.StoredProcedure, "spProcessRecurTransaction", paraRef, paraReturn);
                return Convert.ToInt32(paraReturn.Value);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
