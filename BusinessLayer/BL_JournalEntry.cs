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
    public class BL_JournalEntry
    {
        DL_JournalEntry _objDLJournal = new DL_JournalEntry();
        public void DeleteGLA(Journal objJournal)
        {
            _objDLJournal.DeleteGLA(objJournal);
        }
        public void DeleteTrans(Journal objJournal)
        {
            _objDLJournal.DeleteTrans(objJournal);
        }
        public void DeleteTransByID(Transaction objTrans)
        {
            _objDLJournal.DeleteTransByID(objTrans);
        }
        public DataSet GetDataByRef(Journal objJournal)
        {
            return _objDLJournal.GetDataByRef(objJournal);
        }
        public DataSet GetDataByBatch(Journal objJournal)
        {
            return _objDLJournal.GetDataByBatch(objJournal);
        }
        public DataSet GetMaxTransID(Journal objJournal)
        {
            return _objDLJournal.GetMaxTransID(objJournal);
        }
        public int GetMaxTransRef(Journal objJournal)
        {
            return _objDLJournal.GetMaxTransRef(objJournal);
        }
        public int GetMaxTransBatch(Journal objJournal)
        {
            return _objDLJournal.GetMaxTransBatch(objJournal);
        }
        public void AddGLA(Journal objJournal)
        {
            _objDLJournal.AddGLA(objJournal);
        }
        public int AddJournalTrans(Transaction objTrans)
        {
            return _objDLJournal.AddJournalTrans(objTrans);
        }
        //public DataSet GetDataByBatchRef(Transaction objTrans)
        //{
        //    return _objDLJournal.GetDataByBatchRef(objTrans);
        //}
        public void UpdateGLA(Journal objJournal)
        {
            _objDLJournal.UpdateGLA(objJournal);
        }
        public void UpdateJournalTrans(Transaction objTrans)
        {
            _objDLJournal.UpdateJournalTrans(objTrans);
        }
        //public DataSet GetAllJE(Journal objJournal)
        //{
        //    return _objDLJournal.GetAllJE(objJournal);
        //}
        public DataSet GetJobsLoc(Transaction objTrans)
        {
            return _objDLJournal.GetJobsLoc(objTrans);
        }
        public DataSet GetLocByJobID(Transaction objTrans)
        {
            return _objDLJournal.GetLocByJobID(objTrans);
        }
        public DataSet GetAllJEByDate(Journal objJournal)
        {
            return _objDLJournal.GetAllJEByDate(objJournal);
        }
        public DataSet GetJobDetailByID(Transaction objTrans)
        {
            return _objDLJournal.GetJobDetailByID(objTrans);
        }
        public DataSet GetPhaseByID(Transaction objTrans)
        {
            return _objDLJournal.GetPhaseByID(objTrans);
        }
        public void UpdateJournalTransAmount(Transaction objTrans)
        {
            _objDLJournal.UpdateJournalTransAmount(objTrans);
        }
        public DataSet GetPaymentTransByBatchRef(Transaction objTrans)
        {
            return _objDLJournal.GetPaymentTransByBatchRef(objTrans);
        }
        public DataSet GetTransByID(Transaction objTrans)
        {
            return _objDLJournal.GetTransByID(objTrans);
        }
        public void UpdateInvoiceTransDetails(Transaction _objTrans)
        {
            _objDLJournal.UpdateInvoiceTransDetails(_objTrans);
        }
        public void UpdateBillTrans(Transaction _objTrans)
        {
            _objDLJournal.UpdateBillTrans(_objTrans);
        }
        //public DataSet GetOpenTrans(Transaction _objOpenTrans)
        //{
        //    return _objDLJournal.GetOpenTrans(_objOpenTrans);
        //}
        public void AddTransBankAdj(TransBankAdj _objTrans)
        {
            _objDLJournal.AddTransBankAdj(_objTrans);
        }
        public void UpdateTransCheckRecon(TransBankAdj _objTrans)
        {
            _objDLJournal.UpdateTransCheckRecon(_objTrans);
        }
        public void UpdateTransDepositRecon(TransBankAdj _objTrans)
        {
            _objDLJournal.UpdateTransDepositRecon(_objTrans);
        }
        public void UpdateTransSel(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransSel(_objTrans);
        }
        public void UpdateClearItem(Transaction _objTrans)
        {
            _objDLJournal.UpdateClearItem(_objTrans);
        }
        public DataSet GetTransByBatchRef(Transaction _objTrans)
        {
            return _objDLJournal.GetTransByBatchRef(_objTrans);
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            return _objDLJournal.GetTransByBatch(_objTrans);
        }
        public DataSet GetBillAPTransByBatch(Transaction _objTrans)
        {
            return _objDLJournal.GetBillAPTransByBatch(_objTrans);
        }
        public void UpdateTransDateByBatch(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransDateByBatch(_objTrans);
        }
        public void UpdateTransVoidCheck(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransVoidCheck(_objTrans);
        }
        public void UpdateTransVoidCheckByBatch(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransVoidCheckByBatch(_objTrans);
        }
        public DataSet GetTransByBatchType(Transaction _objTrans)
        {
            return _objDLJournal.GetTransByBatchType(_objTrans);
        }
        public bool ValidateByTimeStamp(Transaction _objTrans)
        {
            return _objDLJournal.ValidateByTimeStamp(_objTrans);
        }
        public void DeleteBillTrans(Transaction _objTrans)
        {
            _objDLJournal.DeleteBillTrans(_objTrans);
        }
        public void DeleteTransDeposit(Transaction _objTrans)
        {
            _objDLJournal.DeleteTransDeposit(_objTrans);
        }
        public void DeleteTransChecks(Transaction _objTrans)
        {
            _objDLJournal.DeleteTransChecks(_objTrans);
        }
        public void UpdateTransCheckNoByBatch(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransCheckNoByBatch(_objTrans);
        }
        public DataSet GetPhaseByJobId(Transaction objTrans)
        {
            return _objDLJournal.GetPhaseByJobId(objTrans);
        }
        //public DataSet GetPhaseExpByJobType(Transaction objTrans)
        //{
        //    return _objDLJournal.GetPhaseExpByJobType(objTrans);
        //}
        public DataSet GetTransDataByBatch(Transaction objTrans)
        {
            return _objDLJournal.GetTransDataByBatch(objTrans);
        }
        public DataSet GetAllPhaseByJobID(Transaction objTrans)
        {
            return _objDLJournal.GetAllPhaseByJobID(objTrans);
        }
        public int ProcessRecurJE(Journal objJournal)
        {
            return _objDLJournal.ProcessRecurJE(objJournal);
        }
    }
}
