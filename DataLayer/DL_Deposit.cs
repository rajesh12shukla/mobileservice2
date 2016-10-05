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
    public class DL_Deposit
    {
        public DataSet GetAllInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.ref is not null \n");



            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByLocID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS PrevDueAmount, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,    \n");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                i.loc, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append(" FROM   Invoice i \n");
            else
                varname1.Append(" FROM   MS_Invoice i  \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Loc='" + objPropContracts.Loc + "' AND i.Status != 1 AND i.Status != 2  \n");

            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append(" SELECT Loc, isnull(Balance,0) as Balance FROM Loc WHERE Loc =" + objPropContracts.Loc + " \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByRef(Invoices _objInv)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.Text, "SELECT fDate,Ref,fDesc,Amount,STax,Total,TaxRegion,TaxRate,TaxFactor,Taxable,Type,Job,Loc,Terms,PO,Status,Batch,Remarks,TransID,GTax,Mech,Pricing,TaxRegion2,TaxRate2,BillToOpt,BillTo,Custom1,Custom2,IDate,fUser,Custom3,QBInvoiceID,LastUpdateDate FROM Invoice WHERE Ref=" + _objInv.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInvoice(Invoices _objInv)
        {
            try
            {
                string query = "UPDATE Invoice SET Status = @Status WHERE Ref = @Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objInv.Ref));
                parameters.Add(new SqlParameter("@Status", _objInv.Status));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objInv.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public int AddReceivedPayment(ReceivedPayment _objReceiPmt)
        //{
        //    try
        //    {
        //        string query = "DECLARE @ID int ";
        //        query += " SET IDENTITY_INSERT [ReceivedPayment] ON ";
        //        query += " SELECT @ID=isnull(max(ID),0) +1 FROM ReceivedPayment ";
        //        query += " INSERT INTO ReceivedPayment(ID,Owner,Loc,Amount,PaymentReceivedDate,PaymentMethod,CheckNumber,AmountDue,fDesc,Status)VALUES(@ID,@Owner,@Loc,@Amount,@PaymentReceivedDate,@PaymentMethod,@CheckNumber,@AmountDue,@fDesc,@Status) ";
        //        query += " SET IDENTITY_INSERT [ReceivedPayment] OFF ";
        //        query += " SELECT @ID ";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@Loc", _objReceiPmt.Loc));
        //        parameters.Add(new SqlParameter("@Owner", _objReceiPmt.Rol));
        //        parameters.Add(new SqlParameter("@Amount", _objReceiPmt.Amount));
        //        parameters.Add(new SqlParameter("@PaymentReceivedDate", _objReceiPmt.PaymentReceivedDate));
        //        parameters.Add(new SqlParameter("@PaymentMethod", _objReceiPmt.PaymentMethod));
        //        parameters.Add(new SqlParameter("@CheckNumber", _objReceiPmt.CheckNumber));
        //        parameters.Add(new SqlParameter("@AmountDue", _objReceiPmt.AmountDue));
        //        parameters.Add(new SqlParameter("@fDesc", _objReceiPmt.fDesc));
        //        parameters.Add(new SqlParameter("@Status", _objReceiPmt.Status));
        //        return _objReceiPmt.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(_objReceiPmt.ConnConfig, CommandType.Text, query, parameters.ToArray()));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void AddPaymentDetails(PaymentDetails _objPayment)
        {
            try
            {
                string query = "INSERT INTO PaymentDetails(ReceivedPaymentID,TransID,InvoiceID)VALUES(@ReceivedPaymentID,@TransID,@InvoiceID)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReceivedPaymentID", _objPayment.ReceivedPaymentID));
                parameters.Add(new SqlParameter("@TransID", _objPayment.TransID));
                parameters.Add(new SqlParameter("@InvoiceID", _objPayment.InvoiceID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPayment.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByInvoiceID(Transaction _objTrans)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT Acct, AcctSub, Amount, Batch, EN, ID, Line, Ref, Sel, Status, Type, VDoub, VInt, fDate, fDesc, strRef FROM Trans WHERE Type=1 AND Ref=" + _objTrans.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllReceivePayment(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT rp.ID,isnull(l.Owner,0) as Owner,isnull(ro.Name,'') AS customerName,rp.Loc,l.Tag,rp.Amount,rp.PaymentReceivedDate,rp.fDesc, \n");
                varname.Append("(CASE rp.PaymentMethod  \n");
                varname.Append(" WHEN 0 THEN 'Check' \n");
                varname.Append(" WHEN 1 THEN 'Cash' END) AS PaymentMethod, \n");
                varname.Append(" (case isnull(rp.Status,0) WHEN 0 then 'Open' WHEN 1 then 'Deposited' END) as StatusName,  \n");
                varname.Append(" rp.CheckNumber,rp.AmountDue,isnull(rp.Status,0) as Status FROM ReceivedPayment rp \n");
                //varname.Append(" left outer join Loc l on l.Loc=rp.Loc  \n");
                //varname.Append(" left outer join Rol r on r.ID=l.Rol \n");
                varname.Append(" LEFT JOIN owner o ON rp.Owner = o.ID \n");
                varname.Append(" LEFT JOIN rol ro ON o.Rol = ro.ID  \n");
                varname.Append(" LEFT JOIN Loc l ON rp.loc = l.loc \n");
                varname.Append(" WHERE (rp.PaymentReceivedDate >= '" + _objReceiPmt.StartDate + "') AND (rp.PaymentReceivedDate <= '" + _objReceiPmt.EndDate + "')");
                //varname.Append(" WHERE NOT EXISTS (SELECT * FROM DepositDetails dep WHERE dep.ReceivedPaymentID = rp.ID) \n");
                varname.Append(" ORDER BY rp.ID \n");
                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePaymentByID(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT r.ID,isnull(r.Owner,0) as Owner,isnull(ro.Name,'') As RolName,r.Loc,isnull(l.Tag,'') as Tag,r.Amount,r.PaymentReceivedDate,  \n");
                varname.Append("            r.PaymentMethod,r.CheckNumber,r.AmountDue,r.fDesc, isnull(r.Status,0) as Status  \n");
                varname.Append("            FROM ReceivedPayment r      \n");
                varname.Append("            LEFT JOIN owner o ON r.Owner = o.ID     \n");
                varname.Append("            LEFT JOIN rol ro ON o.Rol = ro.ID      \n");
                varname.Append("            LEFT JOIN Loc l ON r.loc = l.loc     \n");
                varname.Append("                 WHERE r.ID=" + _objReceiPmt.ID);
                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentByReceivedID(PaymentDetails _objPayment)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPayment.ConnConfig, CommandType.Text, "SELECT ID,ReceivedPaymentID,TransID,InvoiceID FROM PaymentDetails WHERE ReceivedPaymentID=" + _objPayment.ReceivedPaymentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,                     \n");
            varname1.Append("                fDate,                     \n");
            varname1.Append("                l.ID,                      \n");
            varname1.Append("                l.Tag,                     \n");
            varname1.Append("                i.Amount,                  \n");
            varname1.Append("                i.STax,                    \n");
            varname1.Append("                i.Total,                   \n");
            varname1.Append("                i.Status AS StatusID,              \n");
            varname1.Append("                i.custom1 as manualInv,            \n");
            varname1.Append("                " + objPropContracts.TransID + " AS TransID,                   \n");
            varname1.Append("                " + objPropContracts.PaymentID + " AS PaymentID,               \n");
            //varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                (isnull((Select tra.Amount FROM Trans as tra WHERE tra.Type = 98 AND tra.ID=" + objPropContracts.TransID + "),0.00) +                          \n");
            varname1.Append("                isnull((select (i.Total -sum(t.amount)) from (select distinct t.Amount from PaymentDetails p, Trans t where p.InvoiceID = t.ref and t.type = 98 and p.InvoiceID = " + objPropContracts.Ref + ") t), i.Total)) AS PrevDueAmount,     \n");
            varname1.Append("                isnull((Select tra.Amount FROM Trans as tra WHERE tra.Type = 98 AND tra.ID=" + objPropContracts.TransID + "),0.00) AS paymentAmt,              \n");
            //varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 AND tr.Ref=i.Ref),i.Total) AS DueAmount,");
            //varname1.Append("                isnull((select (i.Total - sum(t.Amount)) from PaymentDetails p inner join Trans t on p.InvoiceID = t.ref where p.InvoiceID = i.Ref and t.type = 98), i.Total)  AS DueAmount,");
            //varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 6 AND tr.ID=" + objPropContracts.TransID + "),i.Total) AS DueAmount,");
            varname1.Append("                isnull((select (i.Total -sum(t.amount)) from (select distinct t.Amount from PaymentDetails p, Trans t where p.InvoiceID = t.ref and t.type = 98 and p.InvoiceID = " + objPropContracts.Ref + ") t), i.Total)  AS DueAmount,  \n");
            varname1.Append("                isnull(i.Total,0) AS OrigAmount,       \n");
            varname1.Append("                (CASE i.status                         \n");
            varname1.Append("                  WHEN 0 THEN 'Open'           \n");
            varname1.Append("                  WHEN 1 THEN 'Paid'           \n");
            varname1.Append("                  WHEN 2 THEN 'Voided'         \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending'      \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card'    \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid'         \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status,      \n");
            varname1.Append("                i.PO,                                      \n");
            varname1.Append("                r.Name                  AS customername,   \n");
            varname1.Append("                i.loc,                                     \n");
            varname1.Append("                (SELECT Type                               \n");
            varname1.Append("                 FROM   JobType jt                         \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type,           \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i                           \n");
            else
                varname1.Append("FROM   MS_Invoice i                        \n");
            varname1.Append("       INNER JOIN Loc l                        \n");
            varname1.Append("               ON l.Loc = i.Loc                \n");
            varname1.Append("       INNER JOIN owner o                      \n");
            varname1.Append("               ON o.id = l.owner               \n");
            varname1.Append("       INNER JOIN rol r                        \n");
            varname1.Append("               ON o.rol = r.id                 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip    \n");
            varname1.Append("               ON i.ref = ip.ref where i.Ref=" + objPropContracts.Ref + "      \n");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePaymentDetailsByID(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT rp.ID,rp.Owner,ro.ID as Rol,isnull(rp.Loc,0) as Loc,ro.Name AS customerName,l.Tag,rp.Amount,rp.PaymentReceivedDate,rp.fDesc,  \n");
                varname.Append("(CASE rp.PaymentMethod  \n");
                varname.Append(" WHEN 0 THEN 'Check' \n");
                varname.Append(" WHEN 1 THEN 'Cash' END) AS PaymentMethod \n");
                varname.Append(" ,rp.CheckNumber,rp.AmountDue FROM ReceivedPayment rp \n");
                varname.Append("     left outer join Owner o on o.ID = rp.Owner \n");
                //varname.Append("     left outer join Loc lo on lo.Owner = o.ID \n");
                varname.Append("     left outer join Rol ro on ro.ID = o.Rol \n");
                varname.Append("     left outer join Loc l on l.Loc=rp.Loc  \n");
                varname.Append("     WHERE rp.ID=" + _objReceiPmt.ID + "  ORDER BY rp.ID ");
                //varname.Append(" left outer join Loc l on l.Loc=rp.Loc  \n");
                //varname.Append(" left outer join Rol r on r.ID=l.Rol WHERE rp.ID=" + _objReceiPmt.ID + " ORDER BY rp.ID \n");

                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
                //return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, "SELECT ID,Loc,Amount,PaymentReceivedDate,PaymentMethod,CheckNumber,AmountDue,fDesc FROM ReceivedPayment WHERE ID=" + _objReceiPmt.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddDeposit(Dep _objDep)
        {
            try
            {
                string query = "DECLARE @Ref INT; SELECT @Ref=ISNULL(MAX(Ref),0)+1 FROM Dep; INSERT INTO Dep(Ref,fDate,Bank,fDesc,Amount,TransID)VALUES(@Ref,@fDate,@Bank,@fDesc,@Amount,@TransID); SELECT @Ref AS DepID;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                //parameters.Add(new SqlParameter("@ReceivedPaymentID", _objDep.ReceivedPaymentID));
                parameters.Add(new SqlParameter("@fDate", _objDep.fDate));
                parameters.Add(new SqlParameter("@Bank", _objDep.Bank));
                parameters.Add(new SqlParameter("@fDesc", _objDep.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objDep.Amount));
                parameters.Add(new SqlParameter("@TransID", _objDep.TransID));
                _objDep.DsID = SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
                return Convert.ToInt32(_objDep.DsID.Tables[0].Rows[0]["DepID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDeposit(Dep _objDep)
        {
            try
            {
                string query = "UPDATE Dep SET fDate = @fDate, Bank = @Bank, fDesc = @fDesc, Amount = @Amount WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                parameters.Add(new SqlParameter("@fDate", _objDep.fDate));
                parameters.Add(new SqlParameter("@Bank", _objDep.Bank));
                parameters.Add(new SqlParameter("@fDesc", _objDep.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objDep.Amount));
                //parameters.Add(new SqlParameter("@TransID", _objDep.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddDepositDetails(DepositDetails _objDepDetails)
        {
            try
            {
                string query = "INSERT INTO DepositDetails(DepID,ReceivedPaymentID)VALUES(@DepID,@ReceivedPaymentID);";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                parameters.Add(new SqlParameter("@DepID", _objDepDetails.DepID));
                parameters.Add(new SqlParameter("@ReceivedPaymentID", _objDepDetails.ReceivedPaymentID));

                SqlHelper.ExecuteDataset(_objDepDetails.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllDeposits(Dep _objDep)
        {
            try
            {
                string query = "SELECT 0 as Batch, d.Ref,d.fDate,d.Bank,d.fDesc,isnull(d.Amount,0) as Amount,d.TransID,b.fDesc As BankName, isnull(d.IsRecon,0) as IsRecon, (case when (isnull(d.IsRecon,0) = 0) then 'Open' else 'Reconciled' end) as Status FROM Dep d, Bank b WHERE d.Bank=b.ID ";
                if ((_objDep.StartDate != DateTime.MinValue) && (_objDep.EndDate != DateTime.MinValue))
                {
                    query += "  AND (d.fDate >= '" + _objDep.StartDate + "') AND (d.fDate <= '" + _objDep.EndDate + "') ";
                }
                query += "      ORDER BY Ref";
                return SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDepByID(Dep _objDep)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, "SELECT Ref,fDate,Bank,fDesc,Amount,TransID,EN, isnull(IsRecon,0) as IsRecon FROM Dep WHERE Ref=" + _objDep.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivedPaymentByDep(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT rp.ID,rp.Owner,r.ID as Rol,r.Name AS customerName,isnull(rp.Loc,0) as Loc,isnull(l.Tag,'') as Tag ,rp.Amount,rp.PaymentReceivedDate,rp.fDesc,  \n");
                varname.Append("(CASE rp.PaymentMethod  \n");
                varname.Append(" WHEN 0 THEN 'Check' \n");
                varname.Append(" WHEN 1 THEN 'Cash' END) AS PaymentMethod \n");
                varname.Append(" ,rp.CheckNumber,rp.AmountDue FROM ReceivedPayment rp \n");
                varname.Append(" left outer join DepositDetails dep on dep.DepID = " + _objReceiPmt.DepID + " \n");
                varname.Append(" left outer join Owner o on o.ID = rp.Owner  \n");
                varname.Append(" left outer join Rol r on r.ID = o.Rol  \n");
                varname.Append(" left outer join Loc l on l.Loc=rp.Loc  \n");
                //varname.Append(" left outer join Rol r on r.ID=l.Rol \n");
                varname.Append(" WHERE rp.ID = dep.ReceivedPaymentID \n");
                varname.Append(" ORDER BY rp.ID \n");

                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
                //return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, "SELECT ID,Loc,Amount,PaymentReceivedDate,PaymentMethod,CheckNumber,AmountDue,fDesc FROM ReceivedPayment WHERE ID=" + _objReceiPmt.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllReceivePaymentForDep(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT rp.owner, rp.ID,    \n");
                varname.Append("		o.Rol,    \n");
                varname.Append("		r.Name AS customerName,     \n");
                varname.Append("        isnull(rp.loc,0) as loc, \n");
                varname.Append("        isnull(lo.Tag,'') as Tag,  \n");
                varname.Append("        rp.Amount,rp.PaymentReceivedDate,rp.fDesc,  \n");
                varname.Append("        (CASE rp.PaymentMethod      \n");
                varname.Append("         WHEN 0 THEN 'Check'    \n");
                varname.Append("         WHEN 1 THEN 'Cash' END) AS PaymentMethod   \n");
                varname.Append("         ,rp.CheckNumber,rp.AmountDue FROM ReceivedPayment rp   \n");
                varname.Append("         LEFT JOIN owner o ON o.ID =rp.Owner     \n");
                //varname.Append("         LEFT JOIN Loc l ON l.Owner = o.ID      \n");
                varname.Append("         LEFT JOIN Rol r on r.ID = o.Rol        \n");
                varname.Append("         LEFT JOIN Loc lo ON lo.Loc = rp.Loc \n");
                varname.Append("         WHERE NOT EXISTS (SELECT * FROM DepositDetails dep WHERE dep.ReceivedPaymentID = rp.ID)    \n");
                varname.Append("         ORDER BY rp.ID    \n");

                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddOpenARDetails(OpenAR _objOpenAR)
        {
            try
            {
                string query = "INSERT INTO OpenAR(Loc,fDate,Due,Type,Ref,fDesc,Original,Balance,Selected,TransID,InvoiceID) VALUES(@Loc,@fDate,@Due,@Type,@Ref,@fDesc,@Original,@Balance,@Selected,@TransID,@InvoiceID)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objOpenAR.Ref));
                parameters.Add(new SqlParameter("@fDate", _objOpenAR.fDate));
                parameters.Add(new SqlParameter("@Loc", _objOpenAR.Loc));
                //parameters.Add(new SqlParameter("@fDate", _objOpenAR.fDate));
                parameters.Add(new SqlParameter("@Due", _objOpenAR.Due));
                parameters.Add(new SqlParameter("@Type", _objOpenAR.Type));
                parameters.Add(new SqlParameter("@fDesc", _objOpenAR.fDesc));
                parameters.Add(new SqlParameter("@Original", _objOpenAR.Original));
                parameters.Add(new SqlParameter("@Balance", _objOpenAR.Balance));
                parameters.Add(new SqlParameter("@Selected", _objOpenAR.Selected));
                parameters.Add(new SqlParameter("@TransID", _objOpenAR.TransID));
                parameters.Add(new SqlParameter("@InvoiceID", _objOpenAR.InvoiceID));

                SqlHelper.ExecuteDataset(_objOpenAR.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentDetailsByReceivedID(PaymentDetails _objPayment) // with Payment details from Trans table, Invoice table and PaymentDetails table 
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPayment.ConnConfig, CommandType.Text, "SELECT p.ID,p.ReceivedPaymentID,p.TransID,p.InvoiceID,t.Amount As PaidAmount,i.Total AS TotalAmount, i.fDate As InvoiceDate,i.DDate As DueDate, i.Loc FROM PaymentDetails p, Trans t, Invoice i WHERE p.TransID=t.ID AND p.InvoiceID=i.Ref AND p.ReceivedPaymentID=" + _objPayment.ReceivedPaymentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentByReceivedBatch(PJ _objPj)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPj.ConnConfig, CommandType.Text, "SELECT P.TransID,T.ID, FROM PaymentDetails P JOIN Trans T on P.TransID=t.ID WHERE t.Batch=" + _objPj.Batch);
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
                return SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct FROM Trans WHERE Batch=" + _objTrans.BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetByReceivedPaymentByTransID(PaymentDetails _objPmtDetail)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPmtDetail.ConnConfig, CommandType.Text, "SELECT R.ID,R.Loc,R.Amount,R.PaymentReceivedDate,R.PaymentMethod,R.CheckNumber,R.AmountDue,R.fDesc,P.ReceivedPaymentID FROM ReceivedPayment R Join PaymentDetails P on R.ID=P.ReceivedPaymentID WHERE P.TransID=" + _objPmtDetail.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDepRecon(Dep _objDep)
        {
            try
            {
                string query = "UPDATE Dep SET IsRecon=@IsRecon WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objDep.IsRecon));
                parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepositDetails(Dep _objDep)
        {
            try
            {
                var para = new SqlParameter[4];

                para[1] = new SqlParameter
                {
                    ParameterName = "@Year",
                    SqlDbType = SqlDbType.Int,
                    Value = _objDep.fDateYear
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objDep.Bank
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objDep.fDate
                };

                return _objDep.Ds = SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.StoredProcedure, "spGetDepositDetailsByBank", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePayment(ReceivedPayment _objReceiPmt)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objReceiPmt.ConnConfig, "spDeleteReceivedPayment", _objReceiPmt.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteDeposit(Dep _objDep)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, "spDeleteDeposit", _objDep.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivedPayStatus(ReceivedPayment _objReceivePay)
        {
            try
            {
                string query = "UPDATE ReceivedPayment SET Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Status", _objReceivePay.Status));
                parameters.Add(new SqlParameter("@ID", _objReceivePay.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objReceivePay.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePayment(ReceivedPayment _objReceivePay)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[9];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePay";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = _objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "id";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _objReceivePay.ID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _objReceivePay.Loc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "amount";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = _objReceivePay.Amount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "dueAmount";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = _objReceivePay.AmountDue;

                para[5] = new SqlParameter();
                para[5].ParameterName = "payDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = _objReceivePay.PaymentReceivedDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "payMethod";
                para[6].SqlDbType = SqlDbType.SmallInt;
                para[6].Value = _objReceivePay.PaymentMethod;

                para[7] = new SqlParameter();
                para[7].ParameterName = "checknum";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _objReceivePay.CheckNumber;

                para[8] = new SqlParameter();
                para[8].ParameterName = "fDesc";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = _objReceivePay.fDesc;

                SqlHelper.ExecuteNonQuery(_objReceivePay.ConnConfig, CommandType.StoredProcedure, "spUpdateReceivePay", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByCustID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i  \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Loc='" + objPropContracts.Loc + "' AND i.Status != 1 AND i.Status != 2  \n");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByCustomerID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS PrevDueAmount, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                i.loc, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i  \n");
            varname1.Append("       LEFT JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       LEFT JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       LEFT JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Status != 1 AND i.Status != 2 AND o.ID= " + objPropContracts.Rol + "\n");

            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT ID, isnull(Balance,0) as Balance FROM Owner WHERE ID =" + objPropContracts.Rol + " \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePayment(ReceivedPayment _objReceivePay)
        {
            try
            {
                string query = "UPDATE ReceivedPayment SET Owner = @Owner WHERE ID=" + _objReceivePay.ID;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Owner", _objReceivePay.Rol));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objReceivePay.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentID(ReceivedPayment _objReceivePay)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append(" \n");
                varname1.Append("SELECT ID, isnull(Balance,0) as Balance FROM Owner WHERE ID =" + _objReceivePay.Loc + " \n");

                return SqlHelper.ExecuteDataset(_objReceivePay.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentCustomer(ReceivedPayment _objReceivePay)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append(" \n");
                varname1.Append("SELECT Owner FROM Loc WHERE Loc=" + _objReceivePay.Loc + " \n");

                return SqlHelper.ExecuteDataset(_objReceivePay.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UpdateReceivedPayment(ReceivedPayment _objReceiPmt)
        //{
        //    try
        //    {
        //        string query = "UPDATE ReceivedPayment SET Loc = @Loc,  Amount = @Amount, PaymentReceivedDate = @PaymentReceivedDate, PaymentMethod = @PaymentMethod, CheckNumber = @CheckNumber, AmountDue = @AmountDue, fDesc = @fDesc WHERE ID=@ID";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@ID", _objReceiPmt.ID));
        //        parameters.Add(new SqlParameter("@Loc", _objReceiPmt.Loc));
        //        //parameters.Add(new SqlParameter("@Rol", _objReceiPmt.Rol));
        //        parameters.Add(new SqlParameter("@Amount", _objReceiPmt.Amount));
        //        parameters.Add(new SqlParameter("@PaymentReceivedDate", _objReceiPmt.PaymentReceivedDate));
        //        parameters.Add(new SqlParameter("@PaymentMethod", _objReceiPmt.PaymentMethod));
        //        parameters.Add(new SqlParameter("@CheckNumber", _objReceiPmt.CheckNumber));
        //        parameters.Add(new SqlParameter("@AmountDue", _objReceiPmt.AmountDue));
        //        parameters.Add(new SqlParameter("@fDesc", _objReceiPmt.fDesc));
        //        int rowsAffected = SqlHelper.ExecuteNonQuery(_objReceiPmt.ConnConfig, CommandType.Text, query, parameters.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetInvoicesByReceivedPay(PaymentDetails objPayment)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePayId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPayment.ReceivedPaymentID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "owner";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPayment.Rol;

                para[2] = new SqlParameter();
                para[2].ParameterName = "loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPayment.Loc;

                return SqlHelper.ExecuteDataset(objPayment.ConnConfig, CommandType.StoredProcedure, "spGetInvoicesByReceivedPay", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddReceivePayment(ReceivedPayment objReceivePay)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[9];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePay";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "loc";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objReceivePay.Loc;

                para[2] = new SqlParameter();
                para[2].ParameterName = "owner";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objReceivePay.Rol;

                para[3] = new SqlParameter();
                para[3].ParameterName = "amount";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = objReceivePay.Amount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "dueAmount";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = objReceivePay.AmountDue;

                para[5] = new SqlParameter();
                para[5].ParameterName = "payDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = objReceivePay.PaymentReceivedDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "payMethod";
                para[6].SqlDbType = SqlDbType.SmallInt;
                para[6].Value = objReceivePay.PaymentMethod;

                para[7] = new SqlParameter();
                para[7].ParameterName = "checknum";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = objReceivePay.CheckNumber;

                para[8] = new SqlParameter();
                para[8].ParameterName = "fDesc";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = objReceivePay.fDesc;

                SqlHelper.ExecuteNonQuery(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spAddReceivePay", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDepositTrans(Dep objDep)
        {
            try
            {
                string query = "UPDATE Dep SET TransID = @TransID WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objDep.Ref));
                parameters.Add(new SqlParameter("@TransID", objDep.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllReceivePaymentAjaxSearch(ReceivedPayment _objReceiPmt)
        {
            DataSet ds = new DataSet();
            try
            {

                if (HttpContext.Current.Session["ReceivedPayment"] != null)
                {
                    DataTable dtpo = ((DataTable)HttpContext.Current.Session["ReceivedPayment"]).Copy();

                    ds.Tables.Add(dtpo);

                    //ds = ((DataSet)HttpContext.Current.Session["PO"]);

                }
                else
                {
                    _objReceiPmt.ConnConfig = HttpContext.Current.Session["config"].ToString();


                    StringBuilder varname = new StringBuilder();
                    varname.Append("SELECT rp.ID,isnull(l.Owner,0) as Owner,isnull(ro.Name,'') AS customerName,rp.Loc,l.Tag,rp.Amount,rp.PaymentReceivedDate,rp.fDesc, \n");
                    varname.Append("(CASE rp.PaymentMethod  \n");
                    varname.Append(" WHEN 0 THEN 'Check' \n");
                    varname.Append(" WHEN 1 THEN 'Cash'  \n");
                    varname.Append(" WHEN 2 THEN 'Wire Transfer'  \n");
                    varname.Append(" WHEN 3 THEN 'ACH'  \n");
                    varname.Append(" WHEN 4 THEN 'Credit Card'  \n");
                    varname.Append("        END) AS PaymentMethod,");
                    varname.Append(" (case isnull(rp.Status,0) WHEN 0 then 'Open' WHEN 1 then 'Deposited' END) as StatusName,  \n");
                    varname.Append(" rp.CheckNumber,rp.AmountDue,isnull(rp.Status,0) as Status FROM ReceivedPayment rp \n");
                    //varname.Append(" left outer join Loc l on l.Loc=rp.Loc  \n");
                    //varname.Append(" left outer join Rol r on r.ID=l.Rol \n");
                    varname.Append(" LEFT JOIN owner o ON rp.Owner = o.ID \n");
                    varname.Append(" LEFT JOIN rol ro ON o.Rol = ro.ID  \n");
                    varname.Append(" LEFT JOIN Loc l ON rp.loc = l.loc \n");
                    //varname.Append(" WHERE (rp.PaymentReceivedDate >= '" + _objReceiPmt.StartDate + "') AND (rp.PaymentReceivedDate <= '" + _objReceiPmt.EndDate + "')");
                    //varname.Append(" WHERE NOT EXISTS (SELECT * FROM DepositDetails dep WHERE dep.ReceivedPaymentID = rp.ID) \n");
                    varname.Append(" ORDER BY rp.ID \n");


                    ds = SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());

                }

                _objReceiPmt.Ds = ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }
    }
}
