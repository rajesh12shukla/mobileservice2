using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CashflowStatement : System.Web.UI.Page
{
    #region "Variables"
    Chart _objChart = new Chart();
    BL_Report _objBLReport = new BL_Report();

    #endregion
    
    #region "events"

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {

            SetNetIncomeDetails();
        }
    }
    #endregion

    private void SetNetIncomeDetails()
    {
        double _incomeTotal = 0.00; double _expenseTotal = 0.00; double totalAmount = 0.00;
        _objChart.ConnConfig = Session["config"].ToString();
        DataSet _dsChart = _objBLReport.GetTypeForIncome(_objChart);

        if (string.IsNullOrEmpty(txtStartDate.Text.ToString()) && string.IsNullOrEmpty(txtEndDate.Text.ToString()))
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            _objChart.StartDate = firstDay;
            _objChart.EndDate = DateTime.Now.Date;
        }
        else
        {
            _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
            _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        foreach (DataRow row in _dsChart.Tables[0].Rows)
        {
            double _totalBalance = 0.00;
            DataSet _dsInc = new DataSet();
            _objChart.Type = Convert.ToInt32(row["ID"]);

            _dsInc = _objBLReport.GetDataForBalanceSheet(_objChart);
            foreach (DataRow rowInc in _dsInc.Tables[0].Rows)
            {
                _totalBalance = Convert.ToDouble(_dsInc.Tables[0].AsEnumerable().Sum(r => r.Field<Decimal>("Balance")));

                if (Convert.ToInt32(row["ID"]).Equals(3))
                {
                    _incomeTotal = _totalBalance;
                }
                else if (Convert.ToInt32(row["ID"]).Equals(5))
                {
                    _expenseTotal = _totalBalance;
                }
            }
        }

        if (_incomeTotal < 0)
            _incomeTotal = _incomeTotal * -1;
        if (_expenseTotal < 0)
            _expenseTotal = _expenseTotal * -1;
        totalAmount = _incomeTotal - _expenseTotal;
        if (totalAmount > 0)
        {
            lblNet.Text = "Net Income";
            lblNetAmount.Text = totalAmount.ToString();
        }
    }
    #endregion
}