using BusinessEntity;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Invoice
    {
        DL_Invoice _objDLInvoice = new DL_Invoice();

        public DataSet GetInvByID(Inv _objInv)
        {
            return _objDLInvoice.GetInvByID(_objInv);
        }
        //public void UpdateInvoiceTransDetails(Invoices _objInvoices)
        //{
        //    _objDLInvoice.UpdateInvoiceTransDetails(_objInvoices);
        //}
        public void DeleteTransInvoiceByRef(Transaction _objTrans)
        {
            _objDLInvoice.DeleteTransInvoiceByRef(_objTrans);
        }
        public DataSet GetInvoiceByID(Invoices _objInvoice)
        {
            return _objDLInvoice.GetInvoiceByID(_objInvoice);
        }
        public DataSet GetARRevenue(Contracts objContract)
        {
            return _objDLInvoice.GetARRevenue(objContract);
        }
        public DataSet GetReceivePayInvoice(Invoices objInvoice)
        {
            return _objDLInvoice.GetReceivePayInvoice(objInvoice);
        }
        public DataSet GetAppliedDeposit(Invoices objInvoice)
        {
            return _objDLInvoice.GetAppliedDeposit(objInvoice);
        }
    }
}
