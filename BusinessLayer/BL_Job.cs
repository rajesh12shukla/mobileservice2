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
    public class BL_Job
    {
        DL_Job objDL_Job = new DL_Job();

        public DataSet GetAllJobType(JobT _objJobT)
        {
            return objDL_Job.GetAllJobType(_objJobT);
        }
        public DataSet GetJobType(JobT _objJobT)
        {
            return objDL_Job.GetJobType(_objJobT);
        }
        public DataSet GetContractType(JobT _objJob)
        {
            return objDL_Job.GetContractType(_objJob);
        }
        public DataSet GetInvService(JobT _objJob)
        {
            return objDL_Job.GetInvService(_objJob);
        }
        public DataSet GetPosting(JobT _objJob)
        {
            return objDL_Job.GetPosting(_objJob);
        }
        public DataSet GetJobCode(JobT _objJob)
        {
            return objDL_Job.GetJobCode(_objJob);
        }
        public DataSet GetAllUM(JobT _objJob)
        {
            return objDL_Job.GetAllUM(_objJob);
        }
        public DataSet GetAllInvDetails(JobT _objJob)
        {
            return objDL_Job.GetAllInvDetails(_objJob);
        }
        public DataSet GetServiceType(JobT _objJob)
        {
            return objDL_Job.GetServiceType(_objJob);
        }
        public DataSet GetJobStatus(JobT _objJob)
        {
            return objDL_Job.GetJobStatus(_objJob);
        }
        public DataSet GetJobTFinanceByID(JobT _objJob)
        {
            return objDL_Job.GetJobTFinanceByID(_objJob);
        }
        public DataSet GetBomType(JobT _objJob)
        {
            return objDL_Job.GetBomType(_objJob);
        }
        public DataSet GetTabByPageUrl(JobT _objJob)
        {
            return objDL_Job.GetTabByPageUrl(_objJob);
        }
        public DataSet GetRecurringCustom(JobT _objJob)
        {
            return objDL_Job.GetRecurringCustom(_objJob);
        }
        //public DataSet GetRecurringJobCustom(JobT _objJob)
        //{
        //    return objDL_Job.GetRecurringJobCustom(_objJob);
        //}
        public bool IsExistRecurrJobT(JobT _objJob)
        {
            return objDL_Job.IsExistRecurrJobT(_objJob);
        }
        public DataSet GetProjectTemplateCustomFields(JobT objJob)
        {
            return objDL_Job.GetProjectTemplateCustomFields(objJob);
        }
        public DataSet GetProjectCustomTab(JobT _objJob)
        {
            return objDL_Job.GetProjectCustomTab(_objJob);
        }
        public bool IsExistProjectTempByType(JobT _objJob)
        {
            return objDL_Job.IsExistProjectTempByType(_objJob);
        }
        public DataSet GetJobTById(JobT _objJob)
        {
            return objDL_Job.GetJobTById(_objJob);
        }
        public DataSet GetDataByUM(JobT objJob)
        {
            return objDL_Job.GetDataByUM(objJob);
        }
        public DataSet GetJobCostByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostByJob(objJob);
        }
        public DataSet GetTypeItemByExpCode(JobT objJob)
        {
            return objDL_Job.GetTypeItemByExpCode(objJob);
        }
        public bool IsExistExpJobItemByJob(JobT objJob)
        {
            return objDL_Job.IsExistExpJobItemByJob(objJob);
        }
        public bool IsExistRevJobItemByJob(JobT objJob)
        {
            return objDL_Job.IsExistRevJobItemByJob(objJob);
        }
        public DataSet GetRevenueJobItemsByJob(JobT objJob)
        {
            return objDL_Job.GetRevenueJobItemsByJob(objJob);
        }
        public Int16 AddBOMItem(JobT objJob)
        {
            return objDL_Job.AddBOMItem(objJob);
        }
        public DataSet GetInventoryItem(JobT objJob)
        {
            return objDL_Job.GetInventoryItem(objJob);
        }
        public DataSet GetJobCostCodeByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostCodeByJob(objJob);
        }
        public DataSet GetJobCostTypeByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostTypeByJob(objJob);
        }
        public DataSet GetJobCostInvoicesByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostInvoicesByJob(objJob);
        }
        public DataSet GetPhaseExpByJobType(JobT objJob)
        {
            return objDL_Job.GetPhaseExpByJobType(objJob);
        }
        public DataSet GetBOMTByTypeName(JobT objJob)
        {
            return objDL_Job.GetBOMTByTypeName(objJob);
        }
        public DataSet GetJobCostTicketsByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostTicketsByJob(objJob);
        }
        public DataSet GetProfitLossValues(JobT objJob)
        {
            return objDL_Job.GetProfitLossValues(objJob);
        }
        //public DataSet GetInventoryByName(JobT objJob)
        //{
        //    return objDL_Job.GetInventoryByName(objJob);
        //}

        public DataSet GetAllJobTypeForSearch(JobT _objJob)
        {
            return objDL_Job.GetAllJobTypeForSearch(_objJob);
        }

        public DataSet GetAllJobTypeForAjaxSearch(int type)
        {
            return objDL_Job.GetAllJobTypeForAjaxSearch(type);
        }
    }
}