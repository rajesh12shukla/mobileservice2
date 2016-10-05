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
    public class DL_GLARecur
    {
        public DataSet GetAllRecurrTrans(Journal objJournal)
        {
            try
            {
                return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT Ref ,fDate, Internal, fDesc, Frequency FROM GLARecur Order by Ref");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcessRecurrCount(Journal objJournal)
        {
            try
            {
                return objJournal.DsRecurCount = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT Count(*) AS CountRecur FROM GLARecur Where fDate <='" + System.DateTime.Now + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetTransDataByRef(Transaction objTrans)
        {
            try
            {
                //return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT r.ID, r.Ref, r.Line, r.fDesc, r.Amount, r.Acct, r.Job, r.Phase, c.fDesc AS AcctName, c.Acct AS AcctNo FROM GLARecurI AS r, Chart AS c WHERE r.Acct = c.ID AND r.Ref = " + objTrans.Ref + " Order by r.Line");  
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT r.ID, r.Ref, r.Line, r.fDesc as Description, r.Amount, r.Acct, r.Job as JobID, r.Phase as PhaseID, c.fDesc AS Account, c.Acct AS AcctNo,         \n");
                varname.Append("                (case when(r.Amount > 0) then (r.Amount) else 0.00 end) as Debit, (case when(r.Amount < 0) then (r.Amount*-1) else 0.00 end) as Credit,         \n");
                varname.Append("                j.fDesc as JobName, jobt.fDesc as Phase, 0 as IsRecon, l.Tag as Loc, 0 as Sel           \n");
                varname.Append("                FROM GLARecurI AS r             \n");
                varname.Append("                    left join Chart AS c on r.Acct = c.ID   \n");
                varname.Append("                    left join Job as j on j.ID = r.Job      \n");
                varname.Append("                    left join Loc as l on l.Loc = j.Loc     \n");
                varname.Append("                    left join JobtItem jobt on jobt.Line = r.Phase and jobt.Job = r.Job \n");
                varname.Append("                    WHERE r.Ref = '"+ objTrans.Ref +"' Order by r.Line     \n");
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteGLARecur(Journal objJournal)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, " DELETE FROM GLARecur WHERE Ref = " + objJournal.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteRecurTrans(Journal objJournal)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, " DELETE FROM GLARecurI WHERE Ref = " + objJournal.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteRecurTransByID(Transaction objTrans)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, " DELETE FROM GLARecurI WHERE ID = " + objTrans.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddRecur(Journal objJournal)
        {
            try
            {
                string query = "INSERT INTO GLARecur (Ref,fDate,Internal,fDesc,Frequency) VALUES (@Ref,@fDate,@Internal,@fDesc,@Frequency)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objJournal.Ref));
                parameters.Add(new SqlParameter("@fDate", objJournal.GLDate));
                parameters.Add(new SqlParameter("@Internal", objJournal.Internal));
                parameters.Add(new SqlParameter("@fDesc", objJournal.GLDesc));
                parameters.Add(new SqlParameter("@Frequency", objJournal.Frequency));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void AddRecurTrans(Transaction objTrans)
        {
            string query = "IF @ID IS NULL SELECT @ID=ISNULL(MAX(ID),0)+1 FROM GLARecurI ";
            query+= "SET IDENTITY_INSERT GLARecurI ON ";
            query+= "INSERT INTO GLARecurI (ID, Ref,Line,fDesc,Amount,Acct,Job,Phase) VALUES (@ID,@Ref,@Line,@fDesc,@Amount,@Acct,@Job,@Phase)";
            query += "SET IDENTITY_INSERT GLARecurI OFF ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ID", null));
            parameters.Add(new SqlParameter("@Ref", objTrans.Ref));
            parameters.Add(new SqlParameter("@Line", objTrans.Line));
            parameters.Add(new SqlParameter("@fDesc", objTrans.TransDescription));
            parameters.Add(new SqlParameter("@Amount", objTrans.Amount));
            parameters.Add(new SqlParameter("@Acct", objTrans.Acct));
            parameters.Add(new SqlParameter("@Job", objTrans.JobInt));
            parameters.Add(new SqlParameter("@Phase", Convert.ToInt32(objTrans.PhaseDoub)));
            int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
        }
        public void UpdateRecur(Journal objJournal)
        {
            try
            {
                string query = "UPDATE GLARecur SET fDate = @fDate, Internal = @Internal, fDesc = @fDesc,Frequency = @Frequency WHERE Ref = @Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objJournal.Ref));
                parameters.Add(new SqlParameter("@fDate", objJournal.GLDate));
                parameters.Add(new SqlParameter("@Internal", objJournal.Internal));
                parameters.Add(new SqlParameter("@fDesc", objJournal.GLDesc));
                parameters.Add(new SqlParameter("@Frequency", objJournal.Frequency));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRecurTrans(Transaction objTrans)
        {
            try
            {
                string query = "UPDATE GLARecurI SET fDesc = @fDesc, Amount = @Amount, Acct = @Acct, Job = @Job, Phase=@Phase WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objTrans.ID));
                parameters.Add(new SqlParameter("@fDesc", objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Amount", objTrans.Amount));
                parameters.Add(new SqlParameter("@Acct", objTrans.Acct));
                parameters.Add(new SqlParameter("@Job", objTrans.JobInt));
                parameters.Add(new SqlParameter("@Phase", objTrans.PhaseDoub));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxRecurRef(Journal objJournal)
        {
            try
            {
                return objJournal.Ref = Convert.ToInt32(SqlHelper.ExecuteScalar(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(Ref),0)+1 AS MAXRef FROM GLARecur"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcessTransByDate(Journal objJournal)
        {
            try
            {
                return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT Ref ,fDate, Internal, fDesc, Frequency, 0 as Batch FROM GLARecur Where fDate >= '" + objJournal.StartDate + "' AND fDate <='" + objJournal.EndDate + "' Order by Ref desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMinRecurDate(Journal objJournal)
        {
            try
            {
                return objJournal.DsRecurDate = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT Min(fDate) AS MinDate FROM GLARecur");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
