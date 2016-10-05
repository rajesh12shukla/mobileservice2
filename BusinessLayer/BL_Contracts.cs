using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DataLayer;
using BusinessEntity;
using System.Data;

namespace BusinessLayer
{
    public class BL_Contracts
    {
        DL_Contracts objDL_Contracts = new DL_Contracts();

        public DataSet getContractsData(Contracts objPropContracts)
        {
            return objDL_Contracts.getContractsData(objPropContracts);
        }

        public DataSet GetContract(Contracts objPropContracts)
        {
            return objDL_Contracts.GetContract(objPropContracts);
        }

        public DataSet GetElevContract(Contracts objPropContracts)
        {
            return objDL_Contracts.GetElevContract(objPropContracts);
        }

        public DataSet getJstatus(Contracts objPropContracts)
        {
            return objDL_Contracts.getJstatus(objPropContracts);
        }

        public void AddContract(Contracts objPropContracts)
        {
            objDL_Contracts.AddContract(objPropContracts);
        }

        public void AddContractTemp(Contracts objPropContracts)
        {
            objDL_Contracts.AddContractTemp(objPropContracts);
        }

        public void UpdateContract(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateContract(objPropContracts);
        }

        public void DeleteContract(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteContract(objPropContracts);
        }

        public DataSet AddRecurringTickets(Contracts objPropContracts)
        {
            return objDL_Contracts.AddRecurringTickets(objPropContracts);
        }

        public DataSet GetLastProcessDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetLastProcessDate(objPropContracts);
        }

        public DataSet GetInvoiceLastProcessDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceLastProcessDate(objPropContracts);
        }

        public void CreateRecurringTickets(Contracts objPropContracts)
        {
            objDL_Contracts.CreateRecurringTickets(objPropContracts);
        }

        public DataSet CreateRecurringInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.CreateRecurringInvoices(objPropContracts);
        }

        public DataSet GetBillingFieldByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillingFieldByLoc(objPropContracts);
        }

        public int CreateInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.CreateInvoice(objPropContracts);
        }

        public void CreateQBInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.CreateQBInvoice(objPropContracts);
        }

        public void CreateQBInvoiceMapping(Contracts objPropContracts)
        {
            objDL_Contracts.CreateQBInvoiceMapping(objPropContracts);
        }

        public void UpdateInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateInvoice(objPropContracts);
        }

        public DataSet GetInvoicesByID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesByID(objPropContracts);
        }

        public DataSet GetInvoicesAmount(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesAmount(objPropContracts);
        }

        public DataSet GetInvoicesStatus(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesStatus(objPropContracts);
        }

        public DataSet GetBillcodesforticket(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillcodesforticket(objPropContracts);
        }

        public DataSet GetBillcodesforQBChargeableticket(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillcodesforQBChargeableticket(objPropContracts);
        }

        public DataSet GetRecurringInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetRecurringInvoices(objPropContracts);
        }

        public DataSet GetInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoices(objPropContracts);
        }

        public DataSet GetAPInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetAPInvoices(objPropContracts);
        }

        public DataSet GetJobCostItems(Contracts objPropContracts)
        {
            return objDL_Contracts.GetJobCostItems(objPropContracts);
        }

        public void DeleteInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteInvoice(objPropContracts);
        }

        public void DeleteInvoiceByListID(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteInvoiceByListID(objPropContracts);
        }

        public object AddPayment(Contracts objPropContracts)
        {
            return objDL_Contracts.AddPayment(objPropContracts);
        }

        public DataSet GetPaymentHistory(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPaymentHistory(objPropContracts);
        }

        public DataSet getPaymentGatewayInfo(Contracts objPropContracts)
        {
            return objDL_Contracts.getPaymentGatewayInfo(objPropContracts);
        }

        public void AddMerchant(Contracts objPropContracts)
        {
            objDL_Contracts.AddMerchant(objPropContracts);
        }

        public void DeleteMerchant(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteMerchant(objPropContracts);
        }

        public int GetMaxQBInvoiceID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetMaxQBInvoiceID(objPropContracts);
        }

        public DataSet GetBillcodesforTimeSheet(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillcodesforTimeSheet(objPropContracts);
        }
        public DataSet GetPayrollforTimeSheet(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPayrollforTimeSheet(objPropContracts);
        }
        public DataSet GetPayrollByAccount(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPayrollByAccount(objPropContracts);
        }
        public DataSet getCustomerAddress(Contracts objPropContracts)
        {
            return objDL_Contracts.getCustomerAddress(objPropContracts);
        }
        public DataSet GetInvoicesByBatch(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesByBatch(objPropContracts);
        }
        public void UpdateCustomerBalance(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateCustomerBalance(objPropContracts);
        }
        public bool IsExistContractByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.IsExistContractByLoc(objPropContracts);
        }
        public DataSet GetLastProcessDateOfInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.GetLastProcessDateOfInvoice(objPropContracts);
        }
        public DataSet GetInvoicesDetailsByID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesDetailsByID(objPropContracts);
        }
        public DataSet GetEmailDetailByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.GetEmailDetailByLoc(objPropContracts);
        }
        public DataSet GetInvoicesByRef(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesByRef(objPropContracts);
        }
        public DataSet GetInvoiceItemByRef(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceItemByRef(objPropContracts);
        }
        public void UpdateVoidInvoiceDetails(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateVoidInvoiceDetails(objPropContracts);
        }
        public DataSet GetARInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetARInvoices(objPropContracts);
        }

        public void UpdateExpirationDate(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateExpirationDate(objPropContracts);
        }
    }
}
