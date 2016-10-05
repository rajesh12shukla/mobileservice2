using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class BalanceSheet : System.Web.UI.Page
{
    #region Variables

    Chart objChart = new Chart();
    BL_Chart objBL_Chart = new BL_Chart();
    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    BL_User objBL_User = new BL_User();

    ChartDetails objChartDetail = new ChartDetails();
    AcctDetails objAcct = new AcctDetails();
    SubAcctDetails objSubAcct = new SubAcctDetails();
    #endregion

    #region Events

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                //_objChart.ConnConfig = Session["config"].ToString();

                //DataSet _dsChart = _objBLReport.GetTypeForBalanceSheet(_objChart);
                //if (_dsChart.Tables[0].Rows.Count > 0)
                //{
                //    this.PopulateTreeView(_dsChart.Tables[0], 0, null);
                //}
                //_objPropUser.ConnConfig = Session["config"].ToString();
                //_objPropUser.YE = _objBLReport.GetFiscalYearData(_objPropUser);

                //if(string.IsNullOrEmpty(_objPropUser.YE.ToString()))
                //{
                //    int year = DateTime.Now.Year;
                //    DateTime firstDay = new DateTime(year, 1, 1);
                //    txtStartDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
                //    txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                //}
                //else
                //{
                //    if (_objPropUser.YE.Equals(DateTime.Today.Month) || _objPropUser.YE.Equals(DateTime.Today.Month))
                //    {
                //        int year = DateTime.Now.Year;
                //        DateTime firstDay = new DateTime(year, 1, 1);
                //        txtStartDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
                //        txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                //    }
                //}

                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                //txtStartDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
                txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                GetBalanceSheetReport();
                Permission();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = "Balance Sheet Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Balance Sheet Report attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "BalanceSheet.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetBalanceSheetReport();
    }
    protected void rvBalanceSheet_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetBalanceSheetReport();
    }
    #endregion

    #region Custom Functions
    private void GetBalanceSheetReport()
    {
        try
        {
            objChart.ConnConfig = Session["config"].ToString();

            #region start-end date
            if (string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                    objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
                else
                    objChart.EndDate = DateTime.Now.Date;
            }
            else
            {
                //objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
                objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
            }
            #endregion

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #region calculate net
            objChart.ConnConfig = Session["config"].ToString();

            DataSet dsIncome = objBL_Report.GetIncomestatementBalance(objChart);
            double revenue = 0;
            double costsales = 0;
            double expense = 0;
            double net = 0;
            double gross = 0;
            if (dsIncome.Tables[0].Rows.Count > 0)
            {
                DataRow drRev = dsIncome.Tables[0].Select("Type = 3").SingleOrDefault();
                DataRow drSale = dsIncome.Tables[0].Select("Type = 4").SingleOrDefault();
                DataRow drExp = dsIncome.Tables[0].Select("Type = 5").SingleOrDefault();
                if (drRev != null)
                    revenue = Convert.ToDouble(drRev["NAmt"]);
                if (drSale != null)
                    costsales = Convert.ToDouble(drSale["NAmt"]);
                if (drExp != null)
                    expense = Convert.ToDouble(drExp["NAmt"]);
                
                gross = revenue - costsales;
                net = gross - expense;
            }
            #endregion

            DateTime end = Convert.ToDateTime(txtEndDate.Text);
            string asOfDate = "As of " + end.ToString("MMMM dd, yyyy");
            //double currAmount = net;
            objChart.ConnConfig = Session["config"].ToString();
            DataSet _dsCurrentEarn = objBL_Chart.GetCurrentEarn(objChart);
            int currAcct = Convert.ToInt32(_dsCurrentEarn.Tables[0].Rows[0]["ID"]);
            int valExpCol = 0;
            if (rdExpandAll.Checked.Equals(true))
            {
                valExpCol = 1;
            }
            string url = (Request.Url.Scheme + (Uri.SchemeDelimiter + (Request.Url.Authority + (Request.ApplicationPath + "/"))));
            ReportParameter rpExpColl = new ReportParameter("paramExpCllAll", valExpCol.ToString());
            ReportParameter rpUser = new ReportParameter("paramUsername", Session["Username"].ToString());
            ReportParameter rpAsOfDate = new ReportParameter("paramAsOfDate", asOfDate);
            ReportParameter rpCurrentEarnAcct = new ReportParameter("paramCurrentEarnAcct", currAcct.ToString());
            ReportParameter rpCurrentEarnAmount = new ReportParameter("paramCurrentEarnAmount", net.ToString());

            DataSet ds = objBL_Report.GetBalanceSheetDetails(objChart);
           
            ds.Tables[0].AsEnumerable().ToList()
                .ForEach(b => b["Url"] = (Request.Url.Scheme + (Uri.SchemeDelimiter + (Request.Url.Authority + (Request.ApplicationPath + "accountledger.aspx?id=" + b["Acct"].ToString())))));

            ds.Tables[0].AcceptChanges();

            DataSet ds1 = ds;
            rvBalanceSheet.LocalReport.DataSources.Clear();
            rvBalanceSheet.LocalReport.DataSources.Add(new ReportDataSource("dsAcctDetails", ds.Tables[0]));
            rvBalanceSheet.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            rvBalanceSheet.LocalReport.ReportPath = "Reports/BalanceSheet.rdlc";
            rvBalanceSheet.HyperlinkTarget = "_blank";
            rvBalanceSheet.LocalReport.EnableHyperlinks = true;
            rvBalanceSheet.LocalReport.EnableExternalImages = false;
            rvBalanceSheet.LocalReport.SetParameters(new ReportParameter[] { rpUser, rpExpColl, rpAsOfDate, rpCurrentEarnAcct, rpCurrentEarnAmount });
            //List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            //param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));
            //rvBalanceSheet.LocalReport.SetParameters(param1);
            //rvBalanceSheet.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            rvBalanceSheet.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //private double CalculateCurrentEarn()
    //{
    //    double currExp = 0.0, totalRev = 0.00, totalCostSale = 0.00, totalExp = 0.00;
    //    try
    //    {

    //        //DateTime _selectDate = Convert.ToDateTime(txtCloseOutDate.Text);
    //        //DateTime _fStart = _selectDate;
    //        //_fStart = new DateTime(_fStart.Year, _fStart.Month, 1);

    //        //DateTime _fEnd = _fStart.AddMonths(1);
    //        //_fEnd = _fEnd.AddDays(-1);

    //        //_objChart.ConnConfig = Session["config"].ToString();
    //        //_objChart.StartDate = _fEnd.AddDays(1).AddYears(-1);
    //        //_objChart.EndDate = _fEnd;
    //        objChart.ConnConfig = Session["config"].ToString();
    //        //objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //        objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);

    //        totalRev = objBL_Chart.GetSumOfRevenueByAsOfDate(objChart);
    //        totalCostSale = objBL_Chart.GetSumOfCostSalesByAsOfDate(objChart);
    //        totalExp = objBL_Chart.GetSumOfExpenseByAsOfDate(objChart);

    //        currExp = totalRev - totalCostSale - totalExp; //Current Earnings = Total Revenues - Total Cost of Sales - Total Expenses

    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }

    //    return currExp;
    //}
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("financialStatement");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkFinancialStatement");
        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkBalanceSheet");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
    }
    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = rvBalanceSheet.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }

    #endregion

    //private void SetBalanceSheetHeader()
    //{
    //    lblCompany.Text = Session["company"].ToString();

    //    if(!string.IsNullOrEmpty(txtEndDate.Text))
    //    {
    //        DateTime _endDate = Convert.ToDateTime(txtEndDate.Text);

    //        lblAsOfDate.Text = "As of " + _endDate.ToString("MMMM dd, yyyy");
    //    }

    //}

    #region Populate Tree View
    //private void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
    //{
    //    int i = 0;

    //    foreach (DataRow row in dtParent.Rows)
    //    {
    //        TreeNode child = new TreeNode
    //        {
    //            Text = row["fDesc"].ToString(),
    //            Value = row["ID"].ToString()
    //        };
    //        if (parentId == 0)
    //        {
    //            child.SelectAction = TreeNodeSelectAction.None;
    //            treBalanceSheet.Nodes.Add(child);

    //            _objChart.ConnConfig = Session["config"].ToString();
    //            if (string.IsNullOrEmpty(txtStartDate.Text.ToString()) && string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //            {
    //                int year = DateTime.Now.Year;
    //                DateTime firstDay = new DateTime(year, 1, 1);
    //                _objChart.StartDate = firstDay;
    //                _objChart.EndDate = DateTime.Now.Date;
    //            }
    //            else
    //            {
    //                _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //                _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //            }
    //            _objChart.Type = Convert.ToInt32(row["ID"]);
    //            DataSet _dsType = _objBLReport.GetDataForBalanceSheet(_objChart);

    //            #region Populate Sub Tree

    //            double _totalAmount = 0.00;
    //            foreach (DataRow subRow in _dsType.Tables[0].Rows)
    //            {
    //                StringBuilder nodeText = new StringBuilder();
    //                nodeText.Append(@"<div class='styleDiv1'>").Append(subRow["fDesc"].ToString()).Append("</div><div class='styleDiv2'>").Append(Convert.ToDouble(subRow["Balance"]).ToString("0.00", CultureInfo.InvariantCulture)).Append("</div>");

    //                TreeNode childRow = new TreeNode
    //                {
    //                    Text = nodeText.ToString(),
    //                    Value = subRow["Acct"].ToString()
    //                };
    //                _totalAmount = _totalAmount + Convert.ToDouble(subRow["Balance"]);

    //                childRow.SelectAction = TreeNodeSelectAction.None;
    //                child.ChildNodes.Add(childRow);

    //            }

    //            StringBuilder nodeTotal = new StringBuilder();
    //            nodeTotal.Append(@"<div class='styleDiv1'> Total ").Append(row["fDesc"].ToString()).Append("</div><div class='styleDiv2'>").Append(_totalAmount.ToString("0.00", CultureInfo.InvariantCulture)).Append("</div>");

    //            TreeNode totalRow = new TreeNode
    //            {
    //                //Text = "Total "+row["fDesc"].ToString()+" "+_totalAmount.ToString("0.00", CultureInfo.InvariantCulture),
    //                Text = nodeTotal.ToString(),
    //                Value = i.ToString()
    //            };

    //            totalRow.SelectAction = TreeNodeSelectAction.None;
    //            child.ChildNodes.Add(totalRow);

    //            #endregion
    //        }
    //        else
    //        {
    //            treeNode.ChildNodes.Add(child);
    //            treeNode.SelectAction = TreeNodeSelectAction.None;
    //        }

    //    }
    //}
    #endregion

    //private void BindBalanceSheet2()
    //{

    //    IList<ChartDetails> _lstInc = new List<ChartDetails>();
    //    _objChart.ConnConfig = Session["config"].ToString();
    //    DataSet _dsChart = _objBLReport.GetTypeForBalanceSheet(_objChart);


    //    //if (string.IsNullOrEmpty(txtStartDate.Text.ToString()) || string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //    //{
    //    //    if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()))
    //    //        _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //    //    else
    //    //    {
    //    //        int year = DateTime.Now.Year;
    //    //        DateTime firstDay = new DateTime(year, 1, 1);
    //    //        _objChart.StartDate = firstDay;
    //    //    }
    //    //    if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //    //        _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //    //    else
    //    //        _objChart.EndDate = DateTime.Now.Date;
    //    //}
    //    //else
    //    //{
    //    //    _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //    //    _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //    //}
    //    //if (_dsChart.Tables[0].Rows.Count > 0)
    //    //{
    //    //    foreach (DataRow row in _dsChart.Tables[0].Rows)
    //    //    {
    //    //        DataSet _dsSubCat = new DataSet();
    //    //        _objChart.Type = Convert.ToInt32(row["ID"]);
    //    //        _dsSubCat = _objBLReport.GetSubCategory(_objChart);


    //    //        foreach(DataRow rowSub in _dsSubCat.Tables[0].Rows)
    //    //        {
    //    //            _objSubAcct = new SubAcctDetails();
    //    //            _objSubAcct.AcctNum = Convert.ToInt32(rowSub["Acct"]);
    //    //            _objSubAcct.SubAccount = rowSub["Sub"].ToString();
    //    //        }
    //    //        double _totalBalance = 0.00;
    //    //        _objChartDetail = new ChartDetails();
    //    //        _objChartDetail.ID = Convert.ToInt32(row["ID"]);
    //    //        _objChartDetail.AccountType = row["fDesc"].ToString();
    //    //        DataSet _dsInc = new DataSet();


    //    //        _dsInc = _objBLReport.GetDataForBalanceSheet(_objChart);
    //    //        List<AcctDetails> _lstIncome = new List<AcctDetails>();

    //    //        foreach (DataRow rowInc in _dsInc.Tables[0].Rows)
    //    //        {
    //    //            _objAcct = new AcctDetails();
    //    //            _objAcct.AcctTypeID = Convert.ToInt32(row["ID"]);
    //    //            _objAcct.AcctNum = Convert.ToInt32(rowInc["Acct"]);
    //    //            _objAcct.Account = rowInc["fDesc"].ToString();
    //    //            _objAcct.Balance = Convert.ToDouble(rowInc["Balance"]);

    //    //            _totalBalance = Convert.ToDouble(_dsInc.Tables[0].AsEnumerable().Sum(r => r.Field<Decimal>("Balance")));

    //    //            _lstIncome.Add(_objAcct);
    //    //        }


    //    //        _objChartDetail.TotalBalance = _totalBalance;
    //    //        _objChartDetail.AcctDetails = _lstIncome;
    //    //        _lstInc.Add(_objChartDetail);
    //    //    }
    //    //}
    //    lstBalance.DataSource = ""; //_lstInc
    //    lstBalance.DataBind();

    //    SetBalanceSheetHeader();

    //}
    //private void BindBalanceSheet()
    //{
    //    try
    //    {
    //        IList<ChartDetails> _lstInc = new List<ChartDetails>();
    //        _objChart.ConnConfig = Session["config"].ToString();
    //        DataSet _dsChart = _objBLReport.GetTypeForBalanceSheet(_objChart);


    //        #region start-end date
    //        if (string.IsNullOrEmpty(txtStartDate.Text.ToString()) || string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //        {
    //            if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()))
    //                _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //            else
    //            {
    //                int year = DateTime.Now.Year;
    //                DateTime firstDay = new DateTime(year, 1, 1);
    //                _objChart.StartDate = firstDay;
    //            }
    //            if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
    //                _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //            else
    //                _objChart.EndDate = DateTime.Now.Date;
    //        }
    //        else
    //        {
    //            _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
    //            _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
    //        }
    //        #endregion

    //        double _totalBalance = 0.00;
    //        if (_dsChart.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow row in _dsChart.Tables[0].Rows)
    //            {

    //                _objChartDetail = new ChartDetails();

    //                DataSet _dsSubCat = new DataSet();
    //                _objChart.Type = Convert.ToInt32(row["ID"]);
    //                _dsSubCat = _objBLReport.GetSubCategory(_objChart);

    //                List<SubAcctDetails> _lstSubAcct = new List<SubAcctDetails>();

    //                //double _totalAmt = 0.00;

    //                #region sub account
    //                _objSubAcct = new SubAcctDetails();
    //                _objSubAcct.SubAccount = row["fDesc"].ToString();

    //                DataSet _dsOther = new DataSet();
    //                _dsOther = _objBLReport.GetOtherAcctDetails(_objChart);
    //                List<AcctDetails> _lstOtherAcct = new List<AcctDetails>();

    //                foreach (DataRow rowOther in _dsOther.Tables[0].Rows)
    //                {
    //                    _objAcct = new AcctDetails();
    //                    _objAcct.AcctTypeID = Convert.ToInt32(row["ID"]);
    //                    _objAcct.AcctNum = Convert.ToInt32(rowOther["Acct"]);
    //                    _objAcct.SubAccount = row["fDesc"].ToString();
    //                    _objAcct.Account = rowOther["fDesc"].ToString();
    //                    _objAcct.Balance = Convert.ToDouble(rowOther["Balance"]);

    //                    _totalBalance = Convert.ToDouble(_dsOther.Tables[0].AsEnumerable().Sum(r => r.Field<Decimal>("Balance")));
    //                    _lstOtherAcct.Add(_objAcct);
    //                }
    //                if(_dsOther.Tables[0].Rows.Count > 0)
    //                {
    //                    _objSubAcct.AcctDetails = _lstOtherAcct;
    //                    _objSubAcct.TotalBalance = _totalBalance;

    //                    _lstSubAcct.Add(_objSubAcct);
    //                }
    //                //_totalAmt = _totalAmt + _totalBalance;
    //                #endregion

    //                foreach (DataRow rowSub in _dsSubCat.Tables[0].Rows)
    //                {
    //                    _objSubAcct = new SubAcctDetails();

    //                    _objSubAcct.SubAccount = rowSub["Sub"].ToString();

    //                    DataSet _dsInc = new DataSet();
    //                    _objChart.Sub = _objSubAcct.SubAccount;
    //                    _dsInc = _objBLReport.GetAcctDetailsBySubCat(_objChart);
    //                    List<AcctDetails> _lstAcct = new List<AcctDetails>();

    //                    foreach(DataRow rowInc in _dsInc.Tables[0].Rows)
    //                    {
    //                        _objAcct = new AcctDetails();
    //                        _objAcct.AcctTypeID = Convert.ToInt32(row["ID"]);
    //                        _objAcct.AcctNum = Convert.ToInt32(rowInc["Acct"]);
    //                        _objAcct.SubAccount = rowSub["Sub"].ToString();
    //                        _objAcct.Account = rowInc["fDesc"].ToString();
    //                        _objAcct.Balance = Convert.ToDouble(rowInc["Balance"]);

    //                        _totalBalance = Convert.ToDouble(_dsInc.Tables[0].AsEnumerable().Sum(r => r.Field<Decimal>("Balance")));
    //                        _lstAcct.Add(_objAcct);
    //                    }
    //                    //_totalAmt = _totalAmt + _totalBalance;
    //                    _objSubAcct.AcctDetails = _lstAcct;
    //                    if(_dsInc.Tables[0].Rows.Count > 0)
    //                    {
    //                        _objSubAcct.TotalBalance = _totalBalance;
    //                    }

    //                    if (_dsInc.Tables[0].Rows.Count > 0)
    //                    {
    //                        _lstSubAcct.Add(_objSubAcct);
    //                    }
    //                }
    //                //_objChartDetail.TotalBalance = _totalAmt;
    //                _objChartDetail.TotalBalance = _lstSubAcct.Sum(x => x.TotalBalance);
    //                _objChartDetail.AccountType = row["fDesc"].ToString();
    //                _objChartDetail.LstSubAcct = _lstSubAcct;
    //                _lstInc.Add(_objChartDetail);
    //            }
    //        }
    //        lstBalance.DataSource = "";// _lstInc;
    //        lstBalance.DataBind();

    //        SetBalanceSheetHeader();
    //    }
    //    catch (Exception e)
    //    {
    //        throw;
    //    }
    //}
    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        GetBalanceSheetReport();
    }
}