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
    public class BL_Bills{
    DL_Bills _objDL_Bills = new DL_Bills();
        public void AddOpenAP(OpenAP _objOpenAP)
        {
            _objDL_Bills.AddOpenAP(_objOpenAP);
        }
        public int AddPJ(PJ _objPJ)
        {
            return _objDL_Bills.AddPJ(_objPJ);
        }
        public int AddCD(CD _objCD)
        {
            return _objDL_Bills.AddCD(_objCD);
        }
        public void AddJobI(JobI _objJobI)
        {
            _objDL_Bills.AddJobI(_objJobI);
        }
        public DataSet GetAllPJDetails(PJ _objPJ)
        {
            return _objDL_Bills.GetAllPJDetails(_objPJ);
        }
        public DataSet GetPJDetailByID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJDetailByID(_objPJ);
        }
        public void DeleteJobI(JobI _objJobI)
        {
            _objDL_Bills.DeleteJobI(_objJobI);
        }
        public void UpdatePJ(PJ _objPJ)
        {
            _objDL_Bills.UpdatePJ(_objPJ);
        }
        public void UpdateOpenAP(OpenAP _objOpenAP)
        {
            _objDL_Bills.UpdateOpenAP(_objOpenAP);
        }
        public DataSet GetBillsByVendor(OpenAP _objOpenAP)
        {
            return _objDL_Bills.GetBillsByVendor(_objOpenAP);
        }
        public void AddPaid(Paid _objPaid)
        {
            _objDL_Bills.AddPaid(_objPaid);
        }
        public void UpdateOpenAPPayment(OpenAP _objOpenAP)
        {
            _objDL_Bills.UpdateOpenAPPayment(_objOpenAP);
        }
        public DataSet GetPJDetailByBatch(PJ _objPJ)
        {
            return _objDL_Bills.GetPJDetailByBatch(_objPJ);
        }
        public DataSet GetAllCD(CD _objCD)
        {
            return _objDL_Bills.GetAllCD(_objCD);
        }
        //public DataSet GetAllCDByBankDate(CD _objCD) //Commented by Mayuri 8th Dec
        //{
        //    return _objDL_Bills.GetAllCDByBankDate(_objCD);
        //}
        public void UpdateCDRecon(CD _objCD)
        {
            _objDL_Bills.UpdateCDRecon(_objCD);
        }
        public DataSet GetChecksDetails(CD _objCD)
        {
            return _objDL_Bills.GetChecksDetails(_objCD);
        }
        public DataSet GetCDByID(CD _objCD)
        {
            return _objDL_Bills.GetCDByID(_objCD);
        }
        public DataSet GetPaidDetailByID(Paid _objPaid)
        {
            return _objDL_Bills.GetPaidDetailByID(_objPaid);
        }
        public void UpdateCDDate(CD _objCD)
        {
            _objDL_Bills.UpdateCDDate(_objCD);
        }
        public void UpdateCDVoid(CD _objCD)
        {
            _objDL_Bills.UpdateCDVoid(_objCD);
        }
        public DataSet GetOpenAPByPJID(OpenAP _objOpenAP)
        {
            return _objDL_Bills.GetOpenAPByPJID(_objOpenAP);
        }
        public void DeleteOpenAPByPJID(OpenAP _objOpenAP)
        {
            _objDL_Bills.DeleteOpenAPByPJID(_objOpenAP);
        }
        public void UpdatePJOnVoidCheck(PJ _objPJ)
        {
            _objDL_Bills.UpdatePJOnVoidCheck(_objPJ);
        }
        public DataSet GetJobIByTransID(JobI _objJobI)
        {
            return _objDL_Bills.GetJobIByTransID(_objJobI);
        }
        public DataSet GetCDByTransID(CD _objCD)
        {
            return _objDL_Bills.GetCDByTransID(_objCD);
        }
        public DataSet GetPJByTransID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJByTransID(_objPJ);
        }
        public void UpdatePJClear(PJ _objPJ)
        {
            _objDL_Bills.UpdatePJClear(_objPJ);
        }
        public void DeleteCheckDetails(CD _objCD)
        {
            _objDL_Bills.DeleteCheckDetails(_objCD);
        }
        public DataSet GetPJByID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJByID(_objPJ);
        }
        public void AddPJItem(PJ _objPJ)
        {
            _objDL_Bills.AddPJItem(_objPJ);
        }
        public void DeletePJItem(PJ _objPJ)
        {
            _objDL_Bills.DeletePJItem(_objPJ);
        }
        public void DeleteAPBill(PJ _objPJ)
        {
            _objDL_Bills.DeleteAPBill(_objPJ);
        }
        public bool IsExistCheckNum(CD _objCD)
        {
            return _objDL_Bills.IsExistCheckNum(_objCD);
        }
        public bool IsExistCheckNumOnEdit(CD _objCD)
        {
            return _objDL_Bills.IsExistCheckNumOnEdit(_objCD);
        }
        public int GetBankID(CD _objCD)
        {
            return _objDL_Bills.GetBankID(_objCD);
        }
        public void UpdateCDCheckNo(CD _objCD)
        {
            _objDL_Bills.UpdateCDCheckNo(_objCD);
        }
        public DataSet GetBillsDetailsByDue(PJ _objPJ)
        {
            return _objDL_Bills.GetBillsDetailsByDue(_objPJ);
        }
        public DataSet GetAllPO(PO _objPO)
        {
            return _objDL_Bills.GetAllPO(_objPO);
        }
        public DataSet GetPOById(PO _objPO)
        {
            return _objDL_Bills.GetPOById(_objPO);
        }
        public void DeletePOById(PO _objPO)
        {
            _objDL_Bills.DeletePOById(_objPO);
        }
        public void AddPO(PO _objPO)
        {
            _objDL_Bills.AddPO(_objPO);
        }
        public void UpdatePO(PO _objPO)
        {
            _objDL_Bills.UpdatePO(_objPO);
        }
        public int GetMaxPOId(PO _objPO)
        {
            return _objDL_Bills.GetMaxPOId(_objPO);
        }
        public bool IsFirstPo(PO _objPO)
        {
            return _objDL_Bills.IsFirstPo(_objPO);
        }
        public void UpdatePOBalance(PO _objPO)
        {
            _objDL_Bills.UpdatePOBalance(_objPO);
        }
        public DataSet GetPOByVendor(PO _objPO)
        {
            return _objDL_Bills.GetPOByVendor(_objPO);
        }
        public DataSet GetPOItemByPO(PO _objPO)
        {
            return _objDL_Bills.GetPOItemByPO(_objPO);
        }
        public void UpdatePOStatusById(PO _objPO)
        {
            _objDL_Bills.UpdatePOStatusById(_objPO);
        }
        public DataSet GetAddPOTerms(PO _objPO)
        {
            return _objDL_Bills.GetAddPOTerms(_objPO);
        }
        public bool IsBillExistForInsert(PJ _objPJ)
        {
            return _objDL_Bills.IsBillExistForInsert(_objPJ);
        }
        public bool IsBillExistForEdit(PJ _objPJ)
        {
            return _objDL_Bills.IsBillExistForEdit(_objPJ);
        }
        public int GetMaxReceivePOId(PO _objPO)
        {
            return _objDL_Bills.GetMaxReceivePOId(_objPO);
        }
        public void AddReceivePO(PO _objPO)
        {
            _objDL_Bills.AddReceivePO(_objPO);
        }
        public void UpdatePOStatus(PO _objPO)
        {
            _objDL_Bills.UpdatePOStatus(_objPO);
        }
        public void UpdatePOItemBalance(PO _objPO)
        {
            _objDL_Bills.UpdatePOItemBalance(_objPO);
        }
        public void AddReceivePOItem(PO _objPO)
        {
            _objDL_Bills.AddReceivePOItem(_objPO);
        }
        public DataSet GetAllReceivePO(PO _objPO)
        {
            return _objDL_Bills.GetAllReceivePO(_objPO);
        }
        public DataSet GetReceivePoById(PO _objPO)
        {
            return _objDL_Bills.GetReceivePoById(_objPO);
        }
        public DataSet GetListReceivePO(PO _objPO)
        {
            return _objDL_Bills.GetListReceivePO(_objPO);
        }
        public DataSet GetAllPOByDue(PO _objPO)
        {
            return _objDL_Bills.GetAllPOByDue(_objPO);
        }
        public void UpdatePOItemQuan(PO _objPO)
        {
            _objDL_Bills.UpdatePOItemQuan(_objPO);
        }
        public bool IsClosedPO(PO _objPO)
        {
            return _objDL_Bills.IsClosedPO(_objPO);
        }
        public bool IsExistRPOForInsert(PO objPO)
        {
            return _objDL_Bills.IsExistRPOForInsert(objPO);
        }
        public DataSet GetPOList(PO objPO)
        {
            return _objDL_Bills.GetPOList(objPO);
        }
        public DataSet GetReceivePOList(PO objPO)
        {
            return _objDL_Bills.GetReceivePOList(objPO);
        }
        public DataSet GetPOReceivePOById(PO objPO)
        {
            return _objDL_Bills.GetPOReceivePOById(objPO);
        }
        public int AddBills(PJ objPJ)
        {
            return _objDL_Bills.AddBills(objPJ);
        }
        public void UpdateBills(PJ objPJ)
        {
            _objDL_Bills.UpdateBills(objPJ);
        }
        public void UpdateReceivePOStatus(PO objPO)
        {
            _objDL_Bills.UpdateReceivePOStatus(objPO);
        }
        public bool IsExistPO(PO objPO)
        {
            return _objDL_Bills.IsExistPO(objPO);
        }

        public DataSet GetAllPOAjaxSearch(PO _objPO)
        {
            return _objDL_Bills.GetAllPOAjaxSearch(_objPO);
        }
        public DataSet GetPOItemInfoAjaxSearch(PO _objPO)
        {
            return _objDL_Bills.GetPOItemInfoAjaxSearch(_objPO);
        }
        public DataSet GetAPExpenses(Vendor objVendor)
        {
            return _objDL_Bills.GetAPExpenses(objVendor);
        }
        public void UpdatePODue(PO objPO)
        {
            _objDL_Bills.UpdatePODue(objPO);
        }

        //Rahil's Implementation
        public DataSet GetBankCD(CD _objCD)
        {
            return _objDL_Bills.GetAllBankCD(_objCD);
        }
        public DataSet GetCheckByPaidBill(PJ objPJ)
        {
            return _objDL_Bills.GetCheckByPaidBill(objPJ);
        }
        public DataSet GetBillTransDetails(PJ objPJ)
        {
            return _objDL_Bills.GetBillTransDetails(objPJ);
        }
    }
}
