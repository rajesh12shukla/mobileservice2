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
    public class BL_Report
    {
        DL_Report _objDLReport = new DL_Report();
        //public DataSet GetChartByAcctType(Chart _objChart)
        //{
        //    return _objDLReport.GetChartByAcctType(_objChart);
        //}
        public DataSet GetTypeForBalanceSheet(Chart _objChart)
        {
            return _objDLReport.GetTypeForBalanceSheet(_objChart);
        }
        public DataSet GetDataForBalanceSheet(Chart _objChart) //For Balance sheet
        {
            return _objDLReport.GetDataForBalanceSheet(_objChart);
        }
        public DataSet GetTypeForTrialBalance(Chart _objChart)
        {
            return _objDLReport.GetTypeForTrialBalance(_objChart);
        }
        public DataSet GetTypeForIncome(Chart _objChart)
        {
            return _objDLReport.GetTypeForIncome(_objChart);
        }
        public DataSet GetSubCategory(Chart _objChart)
        {
            return _objDLReport.GetSubCategory(_objChart);
        }
        public DataSet GetAcctDetailsBySubCat(Chart _objChart)
        {
            return _objDLReport.GetAcctDetailsBySubCat(_objChart);
        }
        public DataSet GetOtherAcctDetails(Chart _objChart)
        {
            return _objDLReport.GetOtherAcctDetails(_objChart);
        }
        public int GetFiscalYearData(User objPropUser)
        {
            return _objDLReport.GetFiscalYearData(objPropUser);
        }
        public DataSet GetBalanceSheetDetails(Chart _objChart)
        {
            return _objDLReport.GetBalanceSheetDetails(_objChart);
        }
        public DataSet GetIncomeStatementDetails(Chart _objChart)
        {
            return _objDLReport.GetIncomeStatementDetails(_objChart);
        }
        public DataSet GetTrialBalanceDetails(Chart _objChart)
        {
            return _objDLReport.GetTrialBalanceDetails(_objChart);
        }
        public DataSet GetPeriodClosedYear(User _objPropUser)
        {
            return _objDLReport.GetPeriodClosedYear(_objPropUser);
        }
        public DataSet GetIncomestatementBalance(Chart objChart)
        {
            return _objDLReport.GetIncomestatementBalance(objChart);
        }

        public DataSet GetPurchaseJournal(OpenAP _objOpenAp)
        {
            return _objDLReport.GetPurchaseJournal(_objOpenAp);
        }
    }
}
