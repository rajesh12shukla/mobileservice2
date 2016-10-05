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
    public class DL_Bills
    {
        public void AddOpenAP(OpenAP _objOpenAP)
        {
            try
            {
                string query = "INSERT INTO OpenAP(Vendor,fDate,Due,Type,fDesc,Original,Balance,Selected,Disc,PJID,TRID,Ref)"
                + "VALUES(@Vendor,@fDate,@Due,@Type,@fDesc,@Original,@Balance,@Selected,@Disc,@PJID,@TRID,@Ref)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Vendor", _objOpenAP.Vendor));
                parameters.Add(new SqlParameter("@fDate", _objOpenAP.fDate));
                parameters.Add(new SqlParameter("@Due", _objOpenAP.Due));
                parameters.Add(new SqlParameter("@Type", _objOpenAP.Type));
                parameters.Add(new SqlParameter("@fDesc", _objOpenAP.fDesc));
                parameters.Add(new SqlParameter("@Original", _objOpenAP.Original));
                parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Selected", _objOpenAP.Selected));
                parameters.Add(new SqlParameter("@Disc", _objOpenAP.Disc));
                parameters.Add(new SqlParameter("@PJID", _objOpenAP.PJID));
                parameters.Add(new SqlParameter("@TRID", _objOpenAP.TRID));
                parameters.Add(new SqlParameter("@Ref", _objOpenAP.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddPJ(PJ _objPJ)
        {
            try
            {
                string query = "DECLARE @ID INT; SELECT @ID=ISNULL(MAX(ID),0)+1 FROM PJ; "
                + "INSERT INTO PJ(ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR, ReceivePO)"
                + "VALUES (@ID, @fDate,@Ref, @fDesc,@Amount, @Vendor,@Status, @Batch,@Terms, @PO,@TRID,@Spec,@IDate,@UseTax,@Disc,@Custom1,@Custom2,@ReqBy,@VoidR,@ReceivePO) SELECT @ID AS PJID;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                parameters.Add(new SqlParameter("@fDate", _objPJ.fDate));
                parameters.Add(new SqlParameter("@Ref", _objPJ.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objPJ.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objPJ.Amount));
                parameters.Add(new SqlParameter("@Vendor", _objPJ.Vendor));
                parameters.Add(new SqlParameter("@Status", _objPJ.Status));
                parameters.Add(new SqlParameter("@Batch", _objPJ.Batch));
                parameters.Add(new SqlParameter("@Terms", _objPJ.Terms));
                parameters.Add(new SqlParameter("@PO", _objPJ.PO));
                parameters.Add(new SqlParameter("@TRID", _objPJ.TRID));
                parameters.Add(new SqlParameter("@Spec", _objPJ.Spec));
                parameters.Add(new SqlParameter("@IDate", _objPJ.IDate));
                parameters.Add(new SqlParameter("@UseTax", _objPJ.UseTax));
                parameters.Add(new SqlParameter("@Disc", _objPJ.Disc));
                parameters.Add(new SqlParameter("@Custom1", _objPJ.Custom1));
                parameters.Add(new SqlParameter("@Custom2", _objPJ.Custom2));
                parameters.Add(new SqlParameter("@ReqBy", _objPJ.ReqBy));
                parameters.Add(new SqlParameter("@VoidR", _objPJ.VoidR));
                parameters.Add(new SqlParameter("@ReceivePO", _objPJ.ReceivePo));
                DataSet _dsID = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());

                return Convert.ToInt32(_dsID.Tables[0].Rows[0]["PJID"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddCD(CD _objCD)
        {
            try
            {
                string query = "DECLARE @ID INT; SELECT @ID=ISNULL(MAX(ID),0)+1 FROM CD; "
                    + "INSERT INTO CD(ID,fDate,Ref,fDesc,Amount,Bank,Type,Status,TransID,Vendor,French,Memo,VoidR,ACH)"
                + "VALUES (@ID,@fDate,@Ref,@fDesc,@Amount,@Bank,@Type,@Status,@TransID,@Vendor,@French,@Memo,@VoidR,@ACH) SELECT @ID; ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@ID", _objCD.ID));
                parameters.Add(new SqlParameter("@fDate", _objCD.fDate));
                parameters.Add(new SqlParameter("@Ref", _objCD.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objCD.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objCD.Amount));
                parameters.Add(new SqlParameter("@Bank", _objCD.Bank));
                parameters.Add(new SqlParameter("@Type", _objCD.Type));
                parameters.Add(new SqlParameter("@Status", _objCD.Status));
                parameters.Add(new SqlParameter("@TransID", _objCD.TransID));
                parameters.Add(new SqlParameter("@Vendor", _objCD.Vendor));
                parameters.Add(new SqlParameter("@French", _objCD.French));
                parameters.Add(new SqlParameter("@Memo", _objCD.Memo));
                parameters.Add(new SqlParameter("@VoidR", _objCD.VoidR));
                parameters.Add(new SqlParameter("@ACH", _objCD.ACH));

                //int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
                return _objCD.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddJobI(JobI _objJobI)
        {
            try
            {
                string query = "INSERT INTO JobI(Job,Phase,fDate,Ref,fDesc,Amount,TransID,Type,UseTax,APTicket)"
                + " VALUES(@Job, @Phase,@fDate, @Ref,@fDesc,@Amount, @TransID,@Type, @UseTax,@APTicket)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Job", _objJobI.Job));
                parameters.Add(new SqlParameter("@Phase", _objJobI.Phase));
                parameters.Add(new SqlParameter("@fDate", _objJobI.fDate));
                parameters.Add(new SqlParameter("@Ref", _objJobI.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objJobI.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objJobI.Amount));
                parameters.Add(new SqlParameter("@TransID", _objJobI.TransID));
                parameters.Add(new SqlParameter("@Type", _objJobI.Type));
                //parameters.Add(new SqlParameter("@Labor", _objJobI.Labor));
                //parameters.Add(new SqlParameter("@Billed", _objJobI.Billed));
                //parameters.Add(new SqlParameter("@Invoice", _objJobI.Invoice));
                parameters.Add(new SqlParameter("@UseTax", _objJobI.UseTax));
                parameters.Add(new SqlParameter("@APTicket", _objJobI.vAPTicket));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objJobI.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPaid(Paid _objPaid)
        {
            try
            {
                string query = "INSERT INTO Paid(PITR,fDate,Type,Line,fDesc,Original,Balance,Disc,Paid,TRID,Ref)"
                + " VALUES(@PITR, @fDate, @Type, @Line, @fDesc, @Original, @Balance, @Disc ,@Paid ,@TRID ,@Ref)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PITR", _objPaid.PITR));
                parameters.Add(new SqlParameter("@fDate", _objPaid.fDate));
                parameters.Add(new SqlParameter("@Type", _objPaid.Type));
                parameters.Add(new SqlParameter("@Line", _objPaid.Line));
                parameters.Add(new SqlParameter("@fDesc", _objPaid.fDesc));
                parameters.Add(new SqlParameter("@Original", _objPaid.Original));
                parameters.Add(new SqlParameter("@Balance", _objPaid.Balance));
                parameters.Add(new SqlParameter("@Disc", _objPaid.Disc));
                parameters.Add(new SqlParameter("@Paid", _objPaid.Paid1));
                parameters.Add(new SqlParameter("@TRID", _objPaid.TRID));
                parameters.Add(new SqlParameter("@Ref", _objPaid.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPaid.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPJDetails(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate,p.Ref,p.fDesc,isnull(p.Amount,0) as Amount,p.Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open' ");
                varname.Append("                            WHEN 1 THEN 'Closed' ");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,isnull(p.PO,0) as PO,p.TRID,p.Spec,p.IDate,isnull(p.UseTax,0) as UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName, o.Due");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                varname.Append("    WHERE (p.fDate >= '" + _objPJ.StartDate + "') AND (p.fDate <= '" + _objPJ.EndDate + "') ");
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = " + _objPJ.Vendor);
                }
                if (_objPJ.SearchValue.Equals(1))
                {
                    varname.Append("       AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("       AND o.Due<='" + DateTime.Now.ToShortDateString() + "'");
                }
                else if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("      AND o.Due <= '" + _objPJ.SearchDate + "'");
                }

                varname.Append("    ORDER BY p.ID");
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateOpenAP(OpenAP _objOpenAP)
        {
            try
            {
                string query = "UPDATE OpenAP"
                + " SET Vendor = @Vendor, fDate = @fDate, Due = @Due, fDesc = @fDesc, Original = @Original, Balance = @Balance, Disc = @Disc, Ref = @Ref WHERE PJID = @PJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PJID", _objOpenAP.PJID));
                parameters.Add(new SqlParameter("@Vendor", _objOpenAP.Vendor));
                parameters.Add(new SqlParameter("@fDate", _objOpenAP.fDate));
                parameters.Add(new SqlParameter("@Due", _objOpenAP.Due));
                parameters.Add(new SqlParameter("@fDesc", _objOpenAP.fDesc));
                parameters.Add(new SqlParameter("@Original", _objOpenAP.Original));
                parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Disc", _objOpenAP.Disc));
                parameters.Add(new SqlParameter("@Ref", _objOpenAP.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePJ(PJ _objPJ)
        {
            try
            {
                string query = "UPDATE PJ "
                + "SET fDate = @fDate, Ref = @Ref, fDesc = @fDesc,Amount = @Amount, Vendor = @Vendor, Terms = @Terms,PO = @PO, IDate = @IDate, UseTax = @UseTax, Disc = @Disc, Custom1 = @Custom1, Custom2 = @Custom2, Spec = @Spec WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                parameters.Add(new SqlParameter("@fDate", _objPJ.fDate));
                parameters.Add(new SqlParameter("@fDesc", _objPJ.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objPJ.Amount));
                parameters.Add(new SqlParameter("@Vendor", _objPJ.Vendor));
                parameters.Add(new SqlParameter("@Terms", _objPJ.Terms));
                parameters.Add(new SqlParameter("@PO", _objPJ.PO));
                parameters.Add(new SqlParameter("@IDate", _objPJ.IDate));
                parameters.Add(new SqlParameter("@UseTax", _objPJ.UseTax));
                parameters.Add(new SqlParameter("@Disc", _objPJ.Disc));
                parameters.Add(new SqlParameter("@Ref", _objPJ.Ref));
                parameters.Add(new SqlParameter("@Custom1", _objPJ.Custom1));
                parameters.Add(new SqlParameter("@Custom2", _objPJ.Custom2));
                parameters.Add(new SqlParameter("@Spec", _objPJ.Spec));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJDetailByID(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,v.ID AS VendorID,r.Name AS VendorName,p.ReceivePO,  \n");
                varname.Append("       isnull((select sum(Amount) from PJ where PO = p.po),0) as ReceivedAmount, \n");
                varname.Append("       isnull((select Amount from PO where PO = p.po),0) as POAmount          \n");
                varname.Append("       FROM PJ AS p, Vendor AS v, Rol AS r  \n");
                varname.Append("       WHERE p.Vendor=v.ID AND v.Rol=r.ID AND p.ID=" + _objPJ.ID + "  \n");
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobIByTransID(JobI _objJobI)
        {
            try
            {
                return _objJobI.Ds = SqlHelper.ExecuteDataset(_objJobI.ConnConfig, CommandType.Text, "SELECT Job,Phase,fDate,Ref,fDesc,Amount,TransID,Type,Labor,Billed,Invoice,UseTax,APTicket FROM JobI WHERE TransID =" + _objJobI.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteJobI(JobI _objJobI)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objJobI.ConnConfig, CommandType.Text, " DELETE FROM JobI WHERE TransID = " + _objJobI.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillsByVendor(OpenAP _objOpenAP)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT DISTINCT o.Vendor, \n");
                varname1.Append("       o.fDate, \n");
                varname1.Append("       o.Due, \n");
                varname1.Append("       o.Type, \n");
                varname1.Append("       o.fDesc, \n");
                varname1.Append("       o.Original, \n");
                varname1.Append("       o.Balance, \n");
                varname1.Append("       o.Selected, \n");
                varname1.Append("       o.Disc, \n");
                varname1.Append("       o.PJID, \n");
                varname1.Append("       o.TRID, \n");
                varname1.Append("       o.Disc, \n");
                varname1.Append("       o.Ref, \n");
                varname1.Append("       p.Status, p.Spec, (CASE p.Spec WHEN 0 THEN 'Input Only' ");
                varname1.Append("       WHEN 1 THEN 'Hold - No Invoices' ");
                varname1.Append("       WHEN 2 THEN 'Hold - No Materials' ");
                varname1.Append("       WHEN 3 THEN 'Hold - Other' ");
                varname1.Append("       WHEN 4 THEN 'Verified' ");
                varname1.Append("       WHEN 5 THEN 'Selected' END) as StatusName, \n");
                varname1.Append("      '0.00' AS Payment, ");
                varname1.Append("      p.fDesc AS billDesc ");
                varname1.Append("      FROM OpenAP o, PJ p \n");
                // varname1.Append("      , Paid pa \n");
                //varname1.Append("      WHERE NOT EXISTS (SELECT * FROM Paid pa WHERE pa.TRID = o.TRID) AND p.ID=o.PJID AND o.Balance<>0 AND o.Original<>o.Selected AND o.Vendor=" + _objOpenAP.Vendor);
                varname1.Append("       WHERE p.ID=o.PJID \n");
                //varname1.Append("      AND pa.TRID = o.TRID ");
                varname1.Append("      AND o.Balance<>0 AND o.Original<>o.Selected  \n");
                varname1.Append("      AND o.Vendor=" + _objOpenAP.Vendor);
                //varname1.Append("       AND p.Status = 0 ");          // commented by dev on 9th of April, 2016
                if (_objOpenAP.SearchValue.Equals(1))
                {
                    varname1.Append("      AND o.Due <='" + DateTime.Now.ToShortDateString() + "'");
                }
                if (_objOpenAP.SearchValue.Equals(2))
                {
                    varname1.Append("      AND o.Due <='" + _objOpenAP.SearchDate + "'");
                }

                return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, varname1.ToString());
                //return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT o.Vendor,o.fDate,o.Due,o.Type,o.fDesc,o.Original,o.Balance,o.Selected,o.Disc,o.PJID,o.TRID,o.Ref,p.Status,(CASE p.Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END) as StatusName,'0.00' AS Disc, '0.00' AS Payment FROM OpenAP o, PJ p WHERE p.ID=o.PJID AND o.Vendor=" + _objOpenAP.Vendor);
                //return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT o.Vendor,o.fDate,o.Due,o.Type,o.fDesc,o.Original,o.Balance,o.Selected,o.Disc,o.PJID,o.TRID,o.Ref,p.Status,(CASE p.Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END) as StatusName,'0.00' AS Disc, '0.00' AS Payment FROM OpenAP o, PJ p WHERE (SELECT sum(pa.Paid) FROM Paid AS pa WHERE pa.TRID = o.TRID) p.ID=o.PJID AND o.Vendor=" + _objOpenAP.Vendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateOpenAPPayment(OpenAP _objOpenAP)
        {
            try
            {
                string query = "UPDATE OpenAP"
                + " SET Selected = @Selected, Balance = @Balance WHERE PJID = @PJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PJID", _objOpenAP.PJID));
                parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Selected", _objOpenAP.Selected));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJDetailByBatch(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR, ReceivePO FROM PJ WHERE Batch=" + _objPJ.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllCD(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[3];

                para[1] = new SqlParameter
                {
                    ParameterName = "sdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "edate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.EndDate
                };

                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetCheckDetails", para);
                //return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT c.ID, c.fDate, c.Ref, c.fDesc, c.Amount, c.Bank, c.Type, c.Status, c.TransID, c.Vendor, c.French, c.Memo, c.VoidR, c.ACH, r.Name AS VendorName , b.fDesc AS BankName, t.Batch, isnull(t.Sel,0) as Sel FROM CD AS c, Bank AS b, Vendor AS v, Rol AS r, Trans AS t WHERE c.Bank = b.ID AND c.Vendor=v.ID AND v.Rol=r.ID AND c.Status=0 AND c.TransID=t.ID order by fDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCDByRef(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT fDesc,Bank,Memo,fDate,Ref,ACH FROM CD WHERE Ref=" + _objCD.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDRecon(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET IsRecon=@IsRecon WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objCD.IsRecon));
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChecksDetails(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[4];

                para[1] = new SqlParameter
                {
                    ParameterName = "@Year",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.fDateYear
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.Bank
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.fDate
                };
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetCheckDetailsByBank", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCDByID(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount FROM CD as c, Bank as b, Trans as t WHERE c.Bank=b.ID AND c.TransID=t.ID AND c.ID=" + _objCD.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaidDetailByID(Paid _objPaid)
        {
            try
            {
                return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.Text, "SELECT pj.ID AS PJID,p.PITR,p.fDate,p.Type,p.Line,p.fDesc,p.Original,p.Balance,p.Disc,p.Paid,p.TRID,p.Ref,t.Batch FROM Paid AS p INNER JOIN Trans AS t ON p.TRID=t.ID INNER JOIN PJ as pj ON pj.Batch = t.Batch WHERE p.PITR=" + _objPaid.PITR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDDate(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET fDate=@fDate, Ref=@Ref WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                parameters.Add(new SqlParameter("@fDate", _objCD.fDate));
                parameters.Add(new SqlParameter("@Ref", _objCD.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDVoid(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET fDesc=@fDesc, Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objCD.fDesc));
                parameters.Add(new SqlParameter("@Status", _objCD.Status));
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByPaidTrans(Paid _objPaid)
        {
            try
            {
                return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.Text, "SELECT p.PITR,p.fDate,p.Type,p.Line,p.fDesc,p.Original,p.Balance,p.Disc,p.Paid,p.TRID,p.Ref,t.Batch FROM Paid AS p, Trans AS t WHERE p.TRID = t.ID PITR=" + _objPaid.PITR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOpenAPByPJID(OpenAP _objOpenAP)
        {
            try
            {
                return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT Vendor, fDate, Due, Type, fDesc, Original, Balance, Selected, Disc, PJID, TRID, Ref FROM OpenAP WHERE PJID = " + _objOpenAP.PJID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteOpenAPByPJID(OpenAP _objOpenAP)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, " DELETE FROM OpenAP WHERE PJID = " + _objOpenAP.PJID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePJOnVoidCheck(PJ _objPJ)
        {
            try
            {
                string query = "UPDATE PJ SET fDesc=@fDesc, Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objPJ.fDesc));
                parameters.Add(new SqlParameter("@Status", _objPJ.Status));
                parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCDByTransID(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT ID,fDesc,Bank,Memo,fDate,Ref,ACH,TransID FROM CD WHERE TransID=" + _objCD.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJByTransID(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, " SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE TRID=" + _objPJ.TRID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePJClear(PJ _objPJ)
        {
            try
            {
                string query = "UPDATE PJ SET Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Status", _objPJ.Status));
                parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCheckDetails(CD _objCD)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objCD.ConnConfig, "spDeleteCheckDetails", _objCD.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJByID(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, " SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPJItem(PJ _objPJ)
        {
            try
            {
                string query = "INSERT INTO PJItem(TRID,Stax,Amount,UseTax)"
               + " VALUES(@TRID, @Stax, @Amount, @UseTax)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@TRID", _objPJ.TRID));
                parameters.Add(new SqlParameter("@Stax", _objPJ.UtaxName));
                parameters.Add(new SqlParameter("@Amount", _objPJ.Amount));
                parameters.Add(new SqlParameter("@UseTax", _objPJ.UseTax));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePJItem(PJ _objPJ)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, " DELETE FROM PJItem WHERE TRID = " + _objPJ.TRID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteAPBill(PJ _objPJ)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, "spDeleteAPBill", _objPJ.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistCheckNum(CD _objCD)
        {
            try
            {
                return _objCD.IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM CD WHERE Ref= " + _objCD.Ref + " AND Bank=" + _objCD.Bank + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistCheckNumOnEdit(CD _objCD)
        {
            try
            {
                return _objCD.IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM CD WHERE Ref= " + _objCD.Ref + " AND Bank=" + _objCD.Bank + " AND ID <> " + _objCD.ID + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetBankID(CD _objCD)
        {
            try
            {
                return _objCD.Bank = Convert.ToInt32(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, "SELECT Bank FROM CD WHERE ID=" + _objCD.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDCheckNo(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET Ref=@Ref WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                //parameters.Add(new SqlParameter("@fDate", _objCD.fDate));
                parameters.Add(new SqlParameter("@Ref", _objCD.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillsDetailsByDue(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate,o.Due, p.Ref,p.fDesc,o.Balance As Total,p.Vendor As VendorID,r.Name AS Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open'                      \n");
                varname.Append("                            WHEN 1 THEN 'Closed'                    \n");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR \n");
                varname.Append("    ,isnull(DATEDIFF(day,o.Due,GETDATE()),0) AS DueIn,p.Amount    \n");
                varname.Append("    ,CASE WHEN (isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 7) \n");
                varname.Append("        THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    ELSE 0          \n");
                varname.Append("    END as SevenDay     \n");
                varname.Append("    ,CASE WHEN (isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 30)   \n");
                varname.Append("    	THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    	ELSE 0      \n");
                varname.Append("     END as ThirtyDay   \n");
                varname.Append("     ,CASE WHEN (isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 60)   \n");
                varname.Append("    	THEN   \n");
                varname.Append("    	o.Balance  \n");
                varname.Append("    	ELSE 0   \n");
                varname.Append("     END as SixtyDay	   \n");
                varname.Append("     ,CASE WHEN (isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 61)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as SixtyOneDay  \n");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 7) , o.Balance, 0)) as SevenDay     ");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 30) , o.Balance, 0)) as ThirtyDay   ");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 60) , o.Balance, 0)) as SixtyDay   ");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 61) , o.Balance , 0)) as SixtyOneDay  ");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                //varname.Append("    WHERE (p.fDate >= '" + _objPJ.StartDate + "') AND (p.fDate <= '" + _objPJ.EndDate + "')     \n");
                //varname.Append("      AND p.Status = 0 ");
                varname.Append("     WHERE  o.Balance<>0 AND o.Original<>o.Selected     \n");

                if (_objPJ.SearchValue.Equals(1))
                {
                    varname.Append("      AND o.Due <='" + DateTime.Now.ToShortDateString() + "'    \n");
                }
                else if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Due <='" + _objPJ.SearchDate + "'           \n");
                }
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("      AND p.Vendor = " + _objPJ.Vendor);
                }

                varname.Append("    ORDER BY p.ID");
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPO(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select p.*,                                                 \n");
                varname.Append("        r.Name as VendorName,                                   \n");
                varname.Append("         (CASE isnull(p.Status,0) WHEN 0 THEN 'Open'            \n");
                varname.Append("            WHEN 1 THEN 'Closed'                                \n");
                varname.Append("            WHEN 2 THEN 'Void'                                  \n");
                varname.Append("            WHEN 3 THEN 'Partial-Quantity'                      \n");
                varname.Append("            WHEN 4 THEN 'Partial-Amount'                        \n");
                varname.Append("            WHEN 5 THEN 'Closed At Receive PO' END) AS StatusName,     \n");
                varname.Append("           isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( poitem.Job as nvarchar)     \n");
                varname.Append("            FROM poitem where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Projectnumber,     \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( Loc.ID as nvarchar)     \n");
                varname.Append("            FROM POItem inner join Job on POItem.Job=Job.ID     \n");
                varname.Append("            inner join Loc on Job.Loc=Loc.Loc where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Location,     \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( poitem.fDesc as nvarchar)     \n");
                varname.Append("             FROM poitem where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Part    \n");
                varname.Append("        FROM PO as p                                            \n");
                varname.Append("            		left join Vendor as v on p.Vendor = v.ID    \n");
                varname.Append("                    left join Rol as r on v.Rol = r.ID          \n");
                varname.Append("                    order by p.PO       \n");


                //varname.Append("    select p.po, p.Amount as poamt,pj.amount, case when pj.Amount is null then 'Open'   \n");
                //varname.Append("    			when (pj.amount = p.Amount) or (pj.amount > p.Amount) then 'Closed'     \n");
                //varname.Append("    			when pj.amount < p.Amount then 'Partial-Amount' end as Status           \n");
                //varname.Append("    , p.* from po as p left join pj as pj on p.PO = pj.PO                               \n");
                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOById(PO _objPO)
        {
            try
            {
                //StringBuilder varname = new StringBuilder();
                //varname.Append("    SELECT p.*, r.Name AS VendorName, r.Address, r.City, r.State, r.Zip,          \n");
                //varname.Append("    (CASE p.Status WHEN 0 THEN 'Open'                            \n");
                //varname.Append("                   WHEN 1 THEN 'Closed'                                \n");
                //varname.Append("                   WHEN 2 THEN 'Void'                                  \n");
                //varname.Append("                   WHEN 3 THEN 'Partially Paid' END) AS StatusName     \n");
                //varname.Append("     FROM PO AS p, Vendor AS v, Rol AS r WHERE p.Vendor = v.ID AND v.Rol = r.ID AND p.PO='"+ _objPO.POID +"' \n");
                //varname.Append("     SELECT 0 as RowID, p.PO as ID, p.Line, p.Quan, p.fDesc as Account, p.Price, p.Amount, p.Job as JobID, j.fdesc as JobName, p.Phase as PhaseID, jt.fdesc as Phase, p.Inv,   \n");
                //varname.Append("        p.GL as AcctID, p.Freight, p.Rquan, p.Billed, p.Ticket, r.Name as Loc, c.Acct as AcctNo            \n");
                //varname.Append("        FROM POItem as p LEFT JOIN Job as j ON p.Job=j.ID                       \n");
                //varname.Append("        LEFT JOIN JobTItem as jt ON jt.Job = j.ID                               \n");
                //varname.Append("        LEFT JOIN Chart as c ON c.ID = p.GL                 \n");
                //varname.Append("        LEFT JOIN Loc as l ON l.Loc = j.Loc                 \n");
                //varname.Append("        LEFT JOIN Rol as r ON l.Rol = r.ID   WHERE p.PO='"+ _objPO.POID +"'               \n");
                //varname.Append("        ORDER BY Line       \n");
                //return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());  

                SqlParameter param = new SqlParameter();
                param.ParameterName = "PO";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objPO.POID;


                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPOById", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePOById(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("    DELETE FROM PO WHERE PO = '"+_objPO.POID+"' ");
                varname.Append("    DELETE FROM POItem WHERE PO = '" + _objPO.POID + "' ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[21];

                para[0] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "VendorId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Status
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "ShipVia",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipVia
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Terms",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Terms
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "FOB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.FOB
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "ShipTo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipTo
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Approved",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Approved
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "ApprovedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ApprovedBy
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "ReqBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.ReqBy
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "fBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fBy
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "POReasonCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.POReasonCode
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "CourrierAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.CourrierAcct
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "PORevision",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.PORevision
                };
                para[20] = new SqlParameter
                {
                    ParameterName = "POItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPO.PODt
                };

                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spAddPO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[21];

                para[0] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "VendorId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Status
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "ShipVia",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipVia
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Terms",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Terms
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "FOB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.FOB
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "ShipTo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipTo
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Approved",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Approved
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "ApprovedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ApprovedBy
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "ReqBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.ReqBy
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "fBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fBy
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "POReasonCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.POReasonCode
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "CourrierAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.CourrierAcct
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "PORevision",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.PORevision
                };
                para[20] = new SqlParameter
                {
                    ParameterName = "POItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPO.PODt
                };

                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spUpdatePO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxPOId(PO _objPO)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, "SELECT isnull(max(PO),0) +1 as PO FROM PO"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsFirstPo(PO _objPO)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PO)THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOBalance(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE Chart  \n");
                varname.Append("    SET Balance = ISNULL (p.Balance , 0)    \n");
                varname.Append("          FROM Chart c LEFT JOIN            \n");
                varname.Append("            (SELECT Sum(Amount) AS Balance  \n");
                varname.Append("                FROM PO) p                      \n");
                varname.Append("                ON c.DefaultNo = 'D9991' AND Status = 0   \n");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOByVendor(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT p.*, r.Name as VendorName, r.Address +', '+ CHAR(13)+ r.City +', '+ r.State+', '+ r.Zip as Address                 \n");
                varname.Append("            FROM PO as p INNER JOIN Vendor as v         \n");
                varname.Append("                ON p.Vendor = v.ID                      \n");
                varname.Append("                INNER JOIN Rol as r ON v.Rol = r.ID         \n");
                varname.Append("                WHERE p.Vendor = '" + _objPO.Vendor + "' AND (p.Status=0 OR p.Status=3 OR p.Status=4)  \n");
                varname.Append("        SELECT  \n");
                varname.Append("            poi.PO, poi.Line, poi.Quan, poi.fDesc, poi.Price, poi.Amount, poi.Job, poi.Phase, poi.due,   \n");
                varname.Append("            poi.Amount as Ordered,                      \n");
                varname.Append("            poi.Selected as PrvIn,                      \n");
                varname.Append("            poi.Balance as Outstanding,                 \n");
                varname.Append("            0.00 as Received,                           \n");
                varname.Append("            poi.Quan as OrderedQuan,            \n");
                varname.Append("            poi.SelectedQuan as PrvInQuan,      \n");
                varname.Append("            poi.BalanceQuan as OutstandQuan,    \n");
                varname.Append("            0.00 as ReceivedQuan,               \n");
                varname.Append("            poi.Inv, poi.GL, poi.Freight, poi.Rquan, poi.Billed, poi.Ticket         \n");
                varname.Append("            FROM POItem as poi LEFT JOIN PO as p ON poi.PO = p.PO                   \n");
                varname.Append("                WHERE poi.PO = (select top 1 po from PO WHERE Vendor = '" + _objPO.Vendor + "' order by PO)   \n");
                varname.Append("                ORDER by poi.line       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOItemByPO(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select poi.PO, poi.Line, poi.Quan, poi.fDesc, poi.Price, poi.Amount, poi.Job, poi.Phase, poi.due,    \n");
                varname.Append("    poi.Amount as Ordered,         \n");
                varname.Append("    poi.Selected as PrvIn,                      \n");
                varname.Append("    poi.Balance as Outstanding,                \n");
                varname.Append("    0.00 as Received,                   \n");
                varname.Append("    poi.Quan as OrderedQuan,            \n");
                varname.Append("    poi.SelectedQuan as PrvInQuan,      \n");
                varname.Append("    poi.BalanceQuan as OutstandQuan,    \n");
                varname.Append("    0.00 as ReceivedQuan,               \n");
                varname.Append("    poi.Inv, poi.GL, poi.Freight, poi.Rquan, poi.Billed, poi.Ticket    \n");
                varname.Append("    FROM POItem as poi LEFT JOIN PO as p ON poi.PO = p.PO   \n");
                varname.Append("        WHERE poi.PO = '" + _objPO.POID + "' AND (poi.Balance <> '0' AND poi.BalanceQuan <> '0')   \n");
                varname.Append("        ORDER by poi.line                       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOStatusById(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE PO SET Amount = '" + _objPO.Amount + "', fDesc = '" + _objPO.fDesc + "', Status = '" + _objPO.Status + "' WHERE PO ='" + _objPO.POID + "' ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAddPOTerms(PO _objPO)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, " SELECT t.* FROM [T&C] AS t INNER JOIN tblPages AS p ON t.tblPageID = p.ID WHERE p.PageName='Add/Edit PO' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsBillExistForInsert(PJ _objPJ)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJ WHERE Ref='" + _objPJ.Ref + "' AND Vendor='" + _objPJ.Vendor + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsBillExistForEdit(PJ _objPJ)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJ WHERE Ref='" + _objPJ.Ref + "' AND Vendor='" + _objPJ.Vendor + "' AND ID<>'" + _objPJ.ID + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxReceivePOId(PO _objPO)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, "SELECT isnull(max(ID),0) +1 as ID FROM ReceivePO"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddReceivePO(PO _objPO)
        {
            try
            {
                string query = "SET IDENTITY_INSERT [ReceivePO] ON ";
                query += " INSERT INTO ReceivePO(ID, PO, Ref, WB, Comments, Amount, fDate) ";
                query += " VALUES(@ID, @PO, @Ref, @WB, @Comments, @Amount, @fDate) ";
                query += " SET IDENTITY_INSERT [ReceivePO] OFF ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objPO.RID));
                parameters.Add(new SqlParameter("@PO", _objPO.POID));
                parameters.Add(new SqlParameter("@Ref", _objPO.Ref));
                parameters.Add(new SqlParameter("@WB", _objPO.WB));
                parameters.Add(new SqlParameter("@Comments", _objPO.Comments));
                parameters.Add(new SqlParameter("@Amount", _objPO.Amount));
                parameters.Add(new SqlParameter("@fDate", _objPO.fDate));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOStatus(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE PO SET Status = '" + _objPO.Status + "' WHERE PO ='" + _objPO.POID + "' ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOItemBalance(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET Selected='" + _objPO.Selected + "', Balance='" + _objPO.Balance + "' WHERE PO='" + _objPO.POID + "' AND Line='" + _objPO.Line + "'  ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddReceivePOItem(PO _objPO)
        {
            try
            {

                string query = " INSERT INTO RPOItem(ReceivePO, POLine, Quan, Amount) ";
                query += " VALUES (@ReceivePO, @POLine, @Quan, @Amount) ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReceivePO", _objPO.ReceivePOId));
                parameters.Add(new SqlParameter("@POLine", _objPO.Line));
                parameters.Add(new SqlParameter("@Quan", _objPO.Quan));
                parameters.Add(new SqlParameter("@Amount", _objPO.Amount));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllReceivePO(PO _objPO)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, " SELECT * FROM ReceivePO as r INNER JOIN PO as p ON r.PO=p.PO where r.ID = '" + _objPO.RID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePoById(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(r.Status,0) as Status, p.Due, p.ShipVia,           \n");
                varname.Append("         p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,    \n");
                varname.Append("         p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate, \n");
                varname.Append("         ro.Address +', '+ CHAR(13)+ ro.City +', '+ ro.State+', '+ ro.Zip as Address    \n");
                varname.Append("         FROM ReceivePO as r                             \n");
                varname.Append("         INNER JOIN PO as p ON r.PO=p.PO                 \n");
                varname.Append("         INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                varname.Append("         INNER JOIN Rol as ro ON v.Rol = ro.ID           \n");
                varname.Append("         WHERE r.ID = '" + _objPO.RID + "'      \n");
                varname.Append("    SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,    \n");
                varname.Append("         p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv, \n");
                varname.Append("         p.Amount as Ordered,                       \n");
                varname.Append("         p.Selected as PrvIn,                       \n");
                varname.Append("         p.Balance as Outstanding,                  \n");
                varname.Append("         rp.Amount as Received,                     \n");
                varname.Append("         p.Quan as OrderedQuan,                     \n");
                varname.Append("         p.SelectedQuan as PrvInQuan,               \n");
                varname.Append("         p.BalanceQuan as OutstandQuan,             \n");
                varname.Append("         isnull(rp.Quan,0) as ReceivedQuan,                   \n");
                varname.Append("         rp.POLine,                                 \n");
                varname.Append("         rp.ReceivePO                               \n");
                varname.Append("         FROM ReceivePO AS r          \n");
                varname.Append("        RIGHT JOIN RPOItem AS rp on rp.ReceivePO = r.ID                 \n");
                varname.Append("        INNER JOIN POItem AS p ON p.Line = rp.POLine                    \n");
                varname.Append("        WHERE r.ID = '" + _objPO.RID + "' and p.PO = r.PO                   \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetListReceivePO(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(p.Status,0) as Status, p.Due, p.ShipVia,           \n");
                varname.Append("        case when isnull(r.Status,0) = 0 then 'Open' when isnull(r.status,0) = 1 then 'Closed' else '' End as StatusName,               \n");
                varname.Append("        p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,             \n");
                varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate   \n");
                varname.Append("        FROM ReceivePO as r                             \n");
                varname.Append("        INNER JOIN PO as p ON r.PO=p.PO                 \n");
                varname.Append("        INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                varname.Append("        INNER JOIN Rol as ro ON v.Rol = ro.ID    ORDER BY r.ID       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPOByDue(PO _objPO)
        {
            try
            {
                //StringBuilder varname = new StringBuilder();
                //varname.Append("    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, p.Status, p.Due, p.ShipVia,           \n");
                //varname.Append("        p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,         \n");
                //varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate   \n");
                //varname.Append("        FROM ReceivePO as r                             \n");
                //varname.Append("        INNER JOIN PO as p ON r.PO=p.PO                 \n");
                //varname.Append("        INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                //varname.Append("        INNER JOIN Rol as ro ON v.Rol = ro.ID           \n");
                //varname.Append("        WHERE (p.Status=0 OR p.Status=3 OR p.Status=4)  ORDER BY p.Due \n");
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT p.*, r.Name as VendorName, r.Address +', '+ CHAR(13)+ r.City +', '+ r.State+', '+ r.Zip as Address                \n");
                varname.Append("            FROM PO as p INNER JOIN Vendor as v         \n");
                varname.Append("                ON p.Vendor = v.ID                      \n");
                varname.Append("                INNER JOIN Rol as r ON v.Rol = r.ID             \n");
                varname.Append("                WHERE (p.Status=0 OR p.Status=3 OR p.Status=4)  \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOItemQuan(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET SelectedQuan ='" + _objPO.SelectedQuan + "', BalanceQuan ='" + _objPO.BalanceQuan + "' WHERE PO='" + _objPO.POID + "' AND Line='" + _objPO.Line + "'  ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsClosedPO(PO _objPO)
        {
            StringBuilder varname = new StringBuilder();
            varname.Append("    SELECT CAST(CASE WHEN EXISTS(SELECT sum(selected) as total FROM POItem poi right join PO p ON p.PO = poi.PO WHERE p.Amount = (Select sum(amount) from POItem where po = '" + _objPO.POID + "') and poi.PO = '" + _objPO.POID + "') THEN 1  ELSE 0  END AS BIT) ");
            return _objPO.IsClosed = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, varname.ToString()));
        }
        public bool IsExistRPOForInsert(PO objPO)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM ReceivePO WHERE Ref='" + objPO.Ref + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOList(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT p.PO, p.Vendor, r.Name AS VendorName, p.Status, p.Amount, isnull((select sum(Amount) from PJ where PO = p.po),0) as ReceivedAmount \n");
                varname.Append(" FROM PO AS p INNER JOIN Vendor AS v ON p.Vendor = v.ID \n");
                varname.Append("    INNER JOIN Rol AS r ON r.ID = v.Rol                 \n");
                varname.Append("    WHERE p.Vendor <> 0 AND p.Status <> 1            \n");
                if (objPO.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = '" + objPO.Vendor + "'   \n");
                }
                if (objPO.POID > 0)
                {
                    varname.Append("    AND p.PO like '%" + objPO.POID + "%'    \n");
                }
                varname.Append("    ORDER BY p.PO           \n");
                return SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePOList(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT ID, CAST(ID as varchar(40)) as Value, Amount as ReceivedAmount         \n");
                varname.Append("    FROM ReceivePO                  \n");
                varname.Append("        WHERE isnull(Status,0) <> 1     \n");
                if (objPO.POID > 0)
                {
                    varname.Append("    AND PO like '%" + objPO.POID + "%'   \n");
                }
                varname.Append("        ORDER BY ID     \n");
                return SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOReceivePOById(PO objPO)  // Get to fill in bill screen
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "RID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objPO.RID;

                return objPO.Ds = SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.StoredProcedure, "spGetReceivePoById", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddBills(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[15];

                para[0] = new SqlParameter
                {
                    ParameterName = "GLItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Vendor
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.PostDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.IDate
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Terms
                };
                if (objPJ.PO > 0)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.PO
                    };
                }
                if (objPJ.ReceivePo > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.ReceivePo
                    };
                }
                para[10] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Status
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.Disc
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spAddBills", para);
                return Convert.ToInt32(para[14].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBills(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[17];

                para[0] = new SqlParameter
                {
                    ParameterName = "GLItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.ID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Vendor
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.PostDate
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.IDate
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Terms
                };
                if (objPJ.PO > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.PO
                    };
                }
                if (objPJ.ReceivePo > 0)
                {
                    para[10] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.ReceivePo
                    };
                }
                para[11] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Status
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.Disc
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom1
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom2
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Batch
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "TransId",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.TRID
                };
                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spUpdateBills", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePOStatus(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE ReceivePO SET Status = '" + objPO.Status + "' WHERE ID = '" + objPO.RID + "'     ");
                SqlHelper.ExecuteNonQuery(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPO(PO objPO)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT PO FROM PO WHERE PO = '" + objPO.POID + "') THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllPOAjaxSearch(PO _objPO)
        {
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["PO"] != null)
                {
                    DataTable dtpo = ((DataTable)HttpContext.Current.Session["PO"]).Copy();

                    ds.Tables.Add(dtpo);

                    //ds = ((DataSet)HttpContext.Current.Session["PO"]);

                }
                else
                {
                    _objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();

                    ds = GetAllPO(_objPO);

                }

                _objPO.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetPOItemInfoAjaxSearch(PO _objPO)
        {
            DataSet ds = new DataSet();
            try
            {
                _objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPoitemInfo");



                _objPO.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetAPExpenses(Vendor objVendor)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "vendor";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objVendor.ID;

                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.StoredProcedure, "spGetAPExpenses", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePODue(PO objPO)
        {
            try
            {
                string query = "UPDATE PO SET Due=@Due WHERE PO=@PO";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Due", objPO.Due));
                parameters.Add(new SqlParameter("@PO", objPO.POID));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objPO.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RAHIL IMPLEMENTATION
        public DataSet GetAllBankCD(CD _objCD)
        {
            try
            {
                //string sql = "Select CD.Ref ";
                string sql = "Select CASE ";
                sql = sql + " WHEN LEN(CD.Ref) = 1 ";
                sql = sql + " THEN '00000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 2 ";
                sql = sql + " THEN '0000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 3 ";
                sql = sql + " THEN '000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 4 ";
                sql = sql + " THEN '00000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 5 ";
                sql = sql + " THEN '0000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 6 ";
                sql = sql + " THEN '000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 7 ";
                sql = sql + " THEN '00' +  CAST(CD.Ref AS VARCHAR(9)) ";
                sql = sql + " WHEN LEN(CD.Ref) = 8 ";
                sql = sql + " THEN '0' +  CAST(CD.Ref AS VARCHAR(9))	 ";
                sql = sql + " ELSE '000000000'  End As Ref ";
                sql = sql + " , Rol.Name, Rol.Address,Rol.State,Rol.City,Rol.Zip, Bank.NBranch, Bank.NAcct, Bank.NRoute ";
                sql = sql + " from CD, Bank, Rol where CD.Bank = Bank.ID and Bank.Rol=Rol.ID and Rol.Type = 2 and CD.Ref = '" + _objCD.Ref + "'";
                return SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCheckByPaidBill(PJ objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select p.PITR from trans t inner join paid p on p.TRID = t.ID       \n");
                varname.Append("        where t.Batch = '"+ objPJ.Batch +"' and t.Type = 40             \n");

                return SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillTransDetails(PJ objPJ)
        {
            try
            {
                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, "spGetBillTransactions", objPJ.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}