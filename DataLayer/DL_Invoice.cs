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
    public class DL_Invoice
    {
        public DataSet GetInvByID(Inv _objInv)
        {
            try
            {
                return _objInv.Ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.Text, "SELECT ID,Name,fDesc,Part,Status,SAcct,Measure,Tax,Balance,Price1,Price2,Price3,Price4,Price5,Remarks,Cat,LVendor,LCost,AllowZero,Type,InUse,EN,Hand,Aisle,fOrder,Min,Shelf,Bin,Requ,Warehouse,Price6,Committed,QBInvID,LastUpdateDate,QBAccountID FROM Inv WHERE ID="+_objInv.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateInvoiceTransDetails(Invoices _objInvoices)
        //{
        //    try
        //    {
        //        //string query = "UPDATE Invoice SET Batch=@Batch, TransID=@TransID WHERE Ref=@Ref";
        //        //List<SqlParameter> parameters = new List<SqlParameter>();
        //        //parameters.Add(new SqlParameter("@Ref", _objInvoices.Ref));
        //        //parameters.Add(new SqlParameter("@Batch", _objInvoices.Batch));
        //        //parameters.Add(new SqlParameter("@TransID", _objInvoices.TransID));
        //        string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@Ref", _objInvoices.Ref));
        //        parameters.Add(new SqlParameter("@Batch", _objInvoices.Batch));
        //        int rowsAffected = SqlHelper.ExecuteNonQuery(_objInvoices.ConnConfig, CommandType.Text, query, parameters.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        
        public void DeleteTransInvoiceByRef(Transaction _objTrans)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, " DELETE FROM Trans WHERE Ref = " + _objTrans.Ref +" AND Batch = " + _objTrans.BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByID(Invoices _objInvoice)
        {
            try
            {
                return _objInvoice.Ds = SqlHelper.ExecuteDataset(_objInvoice.ConnConfig, CommandType.Text, "SELECT fDate,Ref,Batch,TransID FROM Invoice WHERE Ref="+_objInvoice.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARRevenue(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.Loc
                };
                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetARRevenue", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePayInvoice(Invoices objInvoice)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, "select * from PaymentDetails where InvoiceID = " + objInvoice.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAppliedDeposit(Invoices objInvoice)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, "select * from trans t inner join (select t.Ref, d.Status from DepApply d inner join trans t on t.ID = d.TransID where d.type = 0 and t.ref = '"+ objInvoice.Ref +"') d    on d.status = t.status and t.type = 6   ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
