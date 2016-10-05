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
    public class BL_Deposit
    {
        DL_Deposit objDL_Deposit = new DL_Deposit();
        public DataSet GetAllInvoices(Contracts objPropContracts)
        {
            return objDL_Deposit.GetAllInvoices(objPropContracts);
        }
        public DataSet GetInvoiceByLocID(Contracts objPropContracts)
        {
            return objDL_Deposit.GetInvoiceByLocID(objPropContracts);
        }
        public DataSet GetInvoiceByRef(Invoices objInvoice)
        {
            return objDL_Deposit.GetInvoiceByRef(objInvoice);
        }
        public void UpdateInvoice(Invoices objInv)
        {
            objDL_Deposit.UpdateInvoice(objInv);
        }
        //public int AddReceivedPayment(ReceivedPayment _objReceiPmt)
        //{
        //    return objDL_Deposit.AddReceivedPayment(_objReceiPmt);
        //}
        public void AddPaymentDetails(PaymentDetails _objPayment)
        {
            objDL_Deposit.AddPaymentDetails(_objPayment);
        }
        public DataSet GetTransByInvoiceID(Transaction _objTrans)
        {
            return objDL_Deposit.GetTransByInvoiceID(_objTrans);
        }
        public DataSet GetAllReceivePayment(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetAllReceivePayment(_objReceiPmt);
        }
        public DataSet GetReceivePaymentByID(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetReceivePaymentByID(_objReceiPmt);
        }
        public DataSet GetPaymentByReceivedID(PaymentDetails _objPayment)
        {
            return objDL_Deposit.GetPaymentByReceivedID(_objPayment);
        }
        public DataSet GetInvoiceByID(Contracts objPropContracts)
        {
            return objDL_Deposit.GetInvoiceByID(objPropContracts);
        }
        public DataSet GetReceivePaymentDetailsByID(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetReceivePaymentDetailsByID(_objReceiPmt);
        }
        public int AddDeposit(Dep _objDep)
        {
            return objDL_Deposit.AddDeposit(_objDep);
        }
        public void UpdateDeposit(Dep _objDep)
        {
            objDL_Deposit.UpdateDeposit(_objDep);
        }
        public void AddDepositDetails(DepositDetails _objDepDetails)
        {
            objDL_Deposit.AddDepositDetails(_objDepDetails);
        }
        public DataSet GetAllDeposits(Dep _objDep)
        {
            return objDL_Deposit.GetAllDeposits(_objDep);
        }
        public DataSet GetDepByID(Dep _objDep)
        {
            return objDL_Deposit.GetDepByID(_objDep);
        }
        public DataSet GetReceivedPaymentByDep(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetReceivedPaymentByDep(_objReceiPmt);
        }
        public DataSet GetAllReceivePaymentForDep(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetAllReceivePaymentForDep(_objReceiPmt);
        }
        public void AddOpenARDetails(OpenAR _objOpenAR)
        {
            objDL_Deposit.AddOpenARDetails(_objOpenAR);
        }
        public DataSet GetPaymentDetailsByReceivedID(PaymentDetails _objPayment)
        {
            return objDL_Deposit.GetPaymentDetailsByReceivedID(_objPayment);
        }
        public DataSet GetPaymentByReceivedBatch(PJ _objPj)
        {
            return objDL_Deposit.GetPaymentByReceivedBatch(_objPj);
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            return objDL_Deposit.GetTransByBatch(_objTrans);
        }
        public DataSet GetByReceivedPaymentByTransID(PaymentDetails _objPmtDetail)
        {
            return objDL_Deposit.GetByReceivedPaymentByTransID(_objPmtDetail);
        }
        public void UpdateDepRecon(Dep _objDep)
        {
            objDL_Deposit.UpdateDepRecon(_objDep);
        }
        public DataSet GetDepositDetails(Dep _objDep)
        {
            return objDL_Deposit.GetDepositDetails(_objDep);
        }
        public void DeletePayment(ReceivedPayment _objReceiPmt)
        {
            objDL_Deposit.DeletePayment(_objReceiPmt);
        }
        public void DeleteDeposit(Dep _objDep)
        {
            objDL_Deposit.DeleteDeposit(_objDep);
        }
        public void UpdateReceivedPayStatus(ReceivedPayment _objReceivePay)
        {
            objDL_Deposit.UpdateReceivedPayStatus(_objReceivePay);
        }
        public void UpdateReceivePayment(ReceivedPayment _objReceivePay)
        {
            objDL_Deposit.UpdateReceivePayment(_objReceivePay);
        }
        //public void UpdateReceivedPayment(ReceivedPayment _objReceiPmt)
        //{
        //    objDL_Deposit.UpdateReceivedPayment(_objReceiPmt);
        //}
        public DataSet GetInvoiceByCustomerID(Contracts objPropContracts)
        {
            return objDL_Deposit.GetInvoiceByCustomerID(objPropContracts);
        }
        public void UpdatePayment(ReceivedPayment _objReceivePay)
        {
            objDL_Deposit.UpdatePayment(_objReceivePay);
        }
        public DataSet GetPaymentCustomer(ReceivedPayment _objReceivePay)
        {
            return objDL_Deposit.GetPaymentCustomer(_objReceivePay);
        }
        public DataSet GetInvoicesByReceivedPay(PaymentDetails objPayment)
        {
            return objDL_Deposit.GetInvoicesByReceivedPay(objPayment);
        }
        public void AddReceivePayment(ReceivedPayment objReceivePay)
        {
            objDL_Deposit.AddReceivePayment(objReceivePay);
        }
        public void UpdateDepositTrans(Dep objDep)
        {
            objDL_Deposit.UpdateDepositTrans(objDep);
        }

        public DataSet GetAllReceivePaymentAjaxSearch(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetAllReceivePaymentAjaxSearch(_objReceiPmt);
        }
    }
}
