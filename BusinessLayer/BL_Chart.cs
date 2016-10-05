using BusinessEntity;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class BL_Chart
    {
        DL_Chart _objDLChart = new DL_Chart();
        public int AddChart(Chart objChart)
        {
            return _objDLChart.AddChart(objChart);
        }
        //public DataSet getCOA(Chart objChart)
        //{
        //    return objDL_Chart.GetAllCOA(objChart);
        //}
        public void UpdateChart(Chart objChart)
        {
            _objDLChart.UpdateChart(objChart);
        }
        public DataSet GetChart(Chart objChart)
        {
            return _objDLChart.GetChart(objChart);
        }
        public void DeleteChart(Chart objChart)
        {
            _objDLChart.DeleteChart(objChart);
        }
        public DataSet GetAll(Chart objChart)
        {
            return _objDLChart.GetAllCOA(objChart);
        }        
        public DataSet GetAllStatus(Chart objChart)
        {
            return _objDLChart.GetStatus(objChart);
        }
        public bool IsExistAcct(Chart objChart)
        {
            return _objDLChart.IsExistAcct(objChart);
        }
        public bool IsExistAcctForEdit(Chart objChart)
        {
            return _objDLChart.IsExistAcctForEdit(objChart);
        }
        public DataSet GetAutoFillAccount(Chart objChart)
        {
            return _objDLChart.GetAutoFillAccount(objChart);
        }
        public DataSet GetAccountData(Chart objChart)
        {
            return _objDLChart.GetAccountData(objChart);
        }
        public DataSet GetUndepositeAcct(Chart objChart)
        {
            return _objDLChart.GetUndepositeAcct(objChart);
        }
        public DataSet GetAcctReceivable(Chart objChart)
        {
            return _objDLChart.GetAcctReceivable(objChart);
        }
        public DataSet GetBankAcct(Chart objChart)
        {
            return _objDLChart.GetBankAcct(objChart);
        }
        public DataSet GetSalesTaxAcct(Chart objChart)
        {
            return _objDLChart.GetSalesTaxAcct(objChart);
        }
        public DataSet GetAcctPayable(Chart objChart)
        {
            return _objDLChart.GetAcctPayable(objChart);
        }
        public void UpdateChartBalance(Chart objChart)
        {
            _objDLChart.UpdateChartBalance(objChart);
        }
        public DataSet GetChartByID(Chart objChart) //By Viral
        {
            return _objDLChart.GetChartByID(objChart);
        }
        public DataSet GetStock(Chart objChart)
        {
            return _objDLChart.GetStock(objChart);
        }
        public DataSet GetCurrentEarn(Chart objChart)
        {
            return _objDLChart.GetCurrentEarn(objChart);
        }
        public DataSet GetDistribution(Chart objChart)
        {
            return _objDLChart.GetDistribution(objChart);
        }
        public DataSet GetRetainedEarn(Chart objChart)
        {
            return _objDLChart.GetRetainedEarn(objChart);
        }
        public DataSet GetAllChartByDate(Chart objChart)
        {
            return _objDLChart.GetAllChartByDate(objChart);
        }
        public DataSet GetMinTransDate(Chart objChart)
        {
            return _objDLChart.GetMinTransDate(objChart);

        }
        public double GetSumOfRevenueByDate(Chart objChart)
        {
            return _objDLChart.GetSumOfRevenueByDate(objChart);
        }
        public double GetSumOfCostSalesByDate(Chart objChart)
        {
            return _objDLChart.GetSumOfCostSalesByDate(objChart);
        }
        public double GetSumOfExpenseByDate(Chart objChart)
        {
            return _objDLChart.GetSumOfExpenseByDate(objChart);
        }
        public bool IsChartBankAcct(Chart objChart)
        {
            return _objDLChart.IsChartBankAcct(objChart);
        }
        public void UpdateBankBalance(Chart objChart)
        {
            _objDLChart.UpdateBankBalance(objChart);
        }
        public DataSet GetAccountLedger(Chart objChart)
        {
            return _objDLChart.GetAccountLedger(objChart);
        }
        public void CalChartBalance(Chart objChart)
        {
            _objDLChart.CalChartBalance(objChart);
        }
        public DataSet GetBankCharge(Chart objChart)
        {
            return _objDLChart.GetBankCharge(objChart);
        }
        public DataSet GetPOAcct(Chart objChart)
        {
            return _objDLChart.GetPOAcct(objChart);
        }
        public DataSet GetChartByAcct(Chart objChart)
        {
            return _objDLChart.GetChartByAcct(objChart);
        }
        public double GetSumOfRevenueByAsOfDate(Chart objChart)
        {
            return _objDLChart.GetSumOfRevenueByAsOfDate(objChart);
        }
        public double GetSumOfCostSalesByAsOfDate(Chart objChart)
        {
            return _objDLChart.GetSumOfCostSalesByAsOfDate(objChart);
        }
        public double GetSumOfExpenseByAsOfDate(Chart objChart)
        {
            return _objDLChart.GetSumOfExpenseByAsOfDate(objChart);
        }
    }
}
