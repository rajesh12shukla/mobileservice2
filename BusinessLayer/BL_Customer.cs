/*#cc01:11-09-2016*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataLayer;
using BusinessEntity;

namespace BusinessLayer
{
    public class BL_Customer
    {
        DL_Customer objDL_Customer = new DL_Customer();

        public DataSet getUserAuthorization(Customer objPropCustomer)
        {
            return objDL_Customer.getCustomers(objPropCustomer);
        }

        public DataSet getProspectByID(Customer objPropCustomer)
        {
            return objDL_Customer.getProspectByID(objPropCustomer);
        }

        public DataSet getProspect(Customer objPropCustomer)
        {
            return objDL_Customer.getProspect(objPropCustomer);
        }

        public DataSet getTasks(Customer objPropCustomer)
        {
            return objDL_Customer.getTasks(objPropCustomer);
        }

        public DataSet getTasksByID(Customer objPropCustomer)
        {
            return objDL_Customer.getTasksByID(objPropCustomer);
        }

        public DataSet getOpportunityByID(Customer objPropCustomer)
        {
            return objDL_Customer.getOpportunityByID(objPropCustomer);
        }

        public DataSet getRecentProspect(Customer objPropCustomer)
        {
            return objDL_Customer.getRecentProspect(objPropCustomer);
        }

        public DataSet getProspectType(Customer objPropCustomer)
        {
            return objDL_Customer.getProspectType(objPropCustomer);
        }

        public DataSet getStages(Customer objPropCustomer)
        {
            return objDL_Customer.getStages(objPropCustomer);
        }

        public DataSet getRepTemplateName(Customer objPropCustomer)
        {
            return objDL_Customer.getRepTemplateName(objPropCustomer);
        }


        public DataSet getRepTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getRepTemplate(objPropCustomer);
        }

        public DataSet getCustomTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getCustomTemplate(objPropCustomer);
        }

        public DataSet getTemplateItemByID(Customer objPropCustomer)
        {
            return objDL_Customer.getTemplateItemByID(objPropCustomer);
        }

        public DataSet getCustTemplateItemByID(Customer objPropCustomer)
        {
            return objDL_Customer.getCustTemplateItemByID(objPropCustomer);
        }

        public DataSet getCustomValues(Customer objPropCustomer)
        {
            return objDL_Customer.getCustomValues(objPropCustomer);
        }

        public DataSet getTemplateItemCodes(Customer objPropCustomer)
        {
            return objDL_Customer.getTemplateItemCodes(objPropCustomer);
        }

        public DataSet getTemplateItemByMultipleID(Customer objPropCustomer, string id)
        {
            return objDL_Customer.getTemplateItemByMultipleID(objPropCustomer, id);
        }

        public DataSet getTemplateItemByElevAndEquipT(Customer objPropCustomer, string EquipId, string Elev)
        {
            return objDL_Customer.getTemplateItemByElevAndEquipT(objPropCustomer, EquipId, Elev);
        }

        public void AddProspect(Customer objPropCustomer)
        {
            objDL_Customer.AddProspect(objPropCustomer);
        }

        public void UpdateProspect(Customer objPropCustomer)
        {
            objDL_Customer.UpdateProspect(objPropCustomer);
        }

        public void DeleteProspect(Customer objPropCustomer)
        {
            objDL_Customer.DeleteProspect(objPropCustomer);
        }

        public void AddProspectType(Customer objPropCustomer)
        {
            objDL_Customer.AddProspectType(objPropCustomer);
        }

        public void AddStages(Customer objPropCustomer)
        {
            objDL_Customer.UpdateStages(objPropCustomer);
        }

        public DataSet getLocCoordinates(Customer objPropCustomer)
        {
            return objDL_Customer.getLocCoordinates(objPropCustomer);
        }

        public DataSet GetWorkerCalculations(Customer objPropCustomer)
        {
            return objDL_Customer.GetWorkerCalculations(objPropCustomer);
        }

        public DataSet getWorkerMonthly(Customer objPropCustomer)
        {
            return objDL_Customer.getWorkerMonthly(objPropCustomer);
        }

        public int AddRouteTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.AddRouteTemplate(objPropCustomer);
        }

        public void UpdateLocRoute(Customer objPropCustomer)
        {
            objDL_Customer.UpdateLocRoute(objPropCustomer);
        }

        public DataSet getRouteTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getRouteTemplate(objPropCustomer);
        }

        public DataSet getTemplateByID(Customer objPropCustomer)
        {
            return objDL_Customer.getTemplateByID(objPropCustomer);
        }

        public DataSet getWorkers(Customer objPropCustomer)
        {
            return objDL_Customer.getWorkers(objPropCustomer);
        }

        public void AddTask(Customer objPropCustomer)
        {
            objDL_Customer.AddTask(objPropCustomer);
        }

        public DataSet getOpportunity(Customer objPropCustomer)
        {
            return objDL_Customer.getOpportunity(objPropCustomer);
        }

        public int AddOpportunity(Customer objPropCustomer)
        {
            return objDL_Customer.AddOpportunity(objPropCustomer);
        }

        public void DeleteOpportunity(Customer objPropCustomer)
        {
            objDL_Customer.DeleteOpportunity(objPropCustomer);
        }
        public void DeleteStages(Customer objPropCustomer)
        {
            objDL_Customer.UpdateStages(objPropCustomer);
        }       
        public void DeleteTask(Customer objPropCustomer)
        {
            objDL_Customer.DeleteTask(objPropCustomer);
        }

        public DataSet getContactByRolID(Customer objPropCustomer)
        {
            return objDL_Customer.getContactByRolID(objPropCustomer);
        }

        public DataSet getSalesDashboard(Customer objPropCustomer)
        {
            return objDL_Customer.getSalesDashboard(objPropCustomer);
        }

        public DataSet getLocationRole(Customer objPropCustomer)
        {
            return objDL_Customer.getLocationRole(objPropCustomer);
        }

        public DataSet getLocationByRoleID(Customer objPropCustomer)
        {
            return objDL_Customer.getLocationByRoleID(objPropCustomer);
        }

        public void AddLocationRole(Customer objPropCustomer)
        {
            objDL_Customer.AddLocationRole(objPropCustomer);
        }

        public void UpdateLocationRole(Customer objPropCustomer)
        {
            objDL_Customer.UpdateLocationRole(objPropCustomer);
        }

        public void DeleteLocationRole(Customer objPropCustomer)
        {
            objDL_Customer.DeleteLocationRole(objPropCustomer);
        }

        public DataSet GetEstimateLabor(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateLabor(objPropCustomer);
        }

        public DataSet GetEstimateLaborForEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateLaborForEstimate(objPropCustomer);
        }

        public void AddEstimateTemplate(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimateTemplate(objPropCustomer);
        }

        public DataSet getEstimateTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateTemplate(objPropCustomer);
        }
        public DataSet getEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimate(objPropCustomer);
        }
        public DataSet getJobProjectByJobID(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProjectByJobID(objPropCustomer);
        }
        public DataSet getJobTemplateByID(Customer objPropCustomer)
        {
            return objDL_Customer.getJobTemplateByID(objPropCustomer);
        }
        public DataSet getJobProject(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProject(objPropCustomer);
        }
        public DataSet getJobProjectTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProjectTemplate(objPropCustomer);
        }
        public DataSet getJobProjectTemp(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProjectTemp(objPropCustomer);
        }
        public DataSet getWage(Customer objPropCustomer)
        {
            return objDL_Customer.getWage(objPropCustomer);
        }
        public DataSet getEstimateTemplateByID(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateTemplateByID(objPropCustomer);
        }
        public DataSet getEstimateBucket(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateBucket(objPropCustomer);
        }
        public DataSet getEstimateBucketItems(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateBucketItems(objPropCustomer);
        }
        public void AddEstimateBucket(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimateBucket(objPropCustomer);
        }
        public DataSet getEstimateBucketByID(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateBucketByID(objPropCustomer);
        }
        public DataSet getJobEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.getJobEstimate(objPropCustomer);
        }
        public void AddEstimateLabor(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimateLabor(objPropCustomer);
        }
        public void AddEstimate(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimate(objPropCustomer);
        }
        public DataSet GetCustomerBomT(Customer objPropCustomer)
        {
            return objDL_Customer.GetBomt(objPropCustomer);
        }
        public void AddCustomBomValues(Customer objPropCustomer)
        {
            objDL_Customer.AddCustomBomT(objPropCustomer);
        }
        public int AddProject(Customer objPropCustomer)
        {
            return objDL_Customer.AddProject(objPropCustomer);
        }
        public int AddProjectTemplate(JobT _objJob)
        {
            return objDL_Customer.AddProjectTemplate(_objJob);
        }
        public int ConvertEstimateToProject(Customer objPropCustomer)
        {
            return objDL_Customer.ConvertEstimateToProject(objPropCustomer);
        }
        public DataSet getAllCustomers(Loc objLoc)
        {
            return objDL_Customer.getAllCustomers(objLoc);
        }
        
        public void DeleteProject(Customer objPropCustomer)
        {
            objDL_Customer.DeleteProject(objPropCustomer);
        }
        public void UpdateCustomerBalance(Owner _objOwner)
        {
            objDL_Customer.UpdateCustomerBalance(_objOwner);
        }
        public DataSet GetOwnerByID(Owner _objOwner)
        {
            return objDL_Customer.GetOwnerByID(_objOwner);
        }
        public DataSet GetOwnerByLoc(Owner _objOwner)
        {
            return objDL_Customer.GetOwnerByLoc(_objOwner);
        }
        public DataSet getAllLocationOnCustomer(Loc objLoc, int _ownerId)
        {
            return objDL_Customer.getAllLocationOnCustomer(objLoc, _ownerId);
        }
        public void DeleteProjectTemplate(JobT _objJob)
        {
            objDL_Customer.DeleteProjectTemplate(_objJob);
        }
        public double GetCustomerBalanceByID(Customer objPropCustomer)
        {
            return objDL_Customer.GetCustomerBalanceByID(objPropCustomer);
        }
        public double GetLocBalanceByID(Customer objPropCustomer)
        {
            return objDL_Customer.GetLocBalanceByID(objPropCustomer);
        }
        public int UpdateProjectTemplate(JobT _objJob)
        {
            return objDL_Customer.UpdateProjectTemplate(_objJob);
        }
        public void UpdateTemplateStatus(JobT _objJob)
        {
            objDL_Customer.UpdateTemplateStatus(_objJob);
        }
        
    }
}
