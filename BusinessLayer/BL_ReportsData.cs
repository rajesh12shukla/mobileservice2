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
    public class BL_ReportsData
    {
        DL_ReportsData objDL_Reports = new DL_ReportsData();

        public DataSet getCustomerDetails(User objPropUser)
        {
            return objDL_Reports.GetCustomerDetails(objPropUser);
        }

        public DataSet getCustomerDetailsTest(User objPropUser)
        {
            return objDL_Reports.GetCustomerDetailsTest(objPropUser);
        }

        public DataSet GetAccountSummaryListingDetail(User objPropUser)
        {
            return objDL_Reports.GetAccSummaryDetail(objPropUser);
        }

        public DataSet InsertCustomerReport(CustomerReport objCustReport)
        {
            return objDL_Reports.InsertCustomerReport(objCustReport);
        }

        public void DeleteCustomerReport(CustomerReport objCustReport)
        {
            objDL_Reports.DeleteCustomerReport(objCustReport);
        }

        public void UpdateCustomerReport(CustomerReport objCustReport)
        {
            objDL_Reports.UpdateCustomerReport(objCustReport);
        }

        public DataSet GetReports(User objPropUser)
        {
            return objDL_Reports.GetReports(objPropUser);
        }

        public DataSet GetStockReports(User objPropUser)
        {
            return objDL_Reports.GetStockReports(objPropUser);
        }

        public DataSet GetReportColByRepId(CustomerReport objCustReport)
        {
            return objDL_Reports.GetReportColByRepId(objCustReport);
        }

        public DataSet GetReportFiltersByRepId(CustomerReport objCustReport)
        {
            return objDL_Reports.GetReportFiltersByRepId(objCustReport);
        }

        public DataSet GetOwners(string query, User objPropUser)
        {
            return objDL_Reports.GetOwners(query, objPropUser);
        }

        public DataSet GetReportDetailById(CustomerReport objCustReport)
        {
            return objDL_Reports.GetReportDetailById(objCustReport);
        }
        public bool CheckExistingReport(CustomerReport objCustReport, string reportAction)
        {
            return objDL_Reports.CheckExistingReport(objCustReport, reportAction);
        }

        public bool IsStockReportExist(CustomerReport objCustReport, string reportAction)
        {
            return objDL_Reports.IsStockReportExist(objCustReport, reportAction);
        }

        public bool CheckForDelete(CustomerReport objCustReport)
        {
            return objDL_Reports.CheckForDelete(objCustReport);
        }


        public DataSet GetControlForReports(User objPropUser)
        {
            return objDL_Reports.GetControlForReports(objPropUser);
        }

        public DataSet GetCustomerType(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerType(objCustReport);
        }

        public DataSet GetGroupedCustomersLocation(User objPropUser)
        {
            return objDL_Reports.GetGroupedCustomersLocation(objPropUser);
        }

        public DataSet GetCustomerName(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerName(objCustReport);
        }

        public DataSet GetCustomerAddress(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerAddress(objCustReport);
        }

        public DataSet GetCustomerCity(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerCity(objCustReport);
        }

        public DataSet GetHeaderFooterDetail(CustomerReport objCustReport)
        {
            return objDL_Reports.GetHeaderFooterDetail(objCustReport);
        }

        public DataSet GetColumnWidthByReportId(CustomerReport objCustReport)
        {
            return objDL_Reports.GetColumnWidthByReportId(objCustReport);
        }

        public void UpdateCustomerReportResizedWidth(CustomerReport objCustReport)
        {
            objDL_Reports.UpdateCustomerReportResizedWidth(objCustReport);
        }

        public DataSet GetCustReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetCustReportFiltersValue(objPropUser);
        }
    }
}
