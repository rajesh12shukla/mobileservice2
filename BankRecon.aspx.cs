using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BankRecon : System.Web.UI.Page
{
    #region Variables

    Bank _objBank = new Bank();
    BL_BankAccount _objBLBank = new BL_BankAccount();

    Dep _objDep = new Dep();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    CD _objCD = new CD();
    BL_Bills _objBL_Bills = new BL_Bills();

    User _objPropUser = new User();
    Transaction _objTrans = new Transaction();
    BL_User _objBL_User = new BL_User();

    Journal _objJournal = new Journal();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    TransBankAdj _objTransBank = new TransBankAdj();
    BL_ReportsData _objBLReportsData = new BL_ReportsData();
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
                FillBank();
                SetBankRecon();

                DateTime _statementDate = SetStatementDate();

                BindCheckGrid(_statementDate);
                BindDepositGrid(_statementDate);

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvDeposit.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblId = (Label)gr.FindControl("lblId");

            AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvDeposit.ClientID + "',event);";
            gr.Attributes["onclick"] = "calculateDepositAmount();";

        }
        foreach (GridViewRow gr in gvCheck.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblId = (Label)gr.FindControl("lblId");

            AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvCheck.ClientID + "',event);";
            gr.Attributes["onclick"] = "calculateCheckAmount();";
        }
    }
    protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (IsValidDate())
            {
                if (!ddlBank.SelectedValue.Equals("0"))
                {
                    txtEndingBalance.ReadOnly = false;
                    _objBank.ConnConfig = Session["config"].ToString();
                    _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
                    DataSet _dsBankBal = _objBLBank.GetBankByID(_objBank);
                    if (_dsBankBal.Tables[0].Rows.Count > 0)
                    {
                        double _balance = Convert.ToDouble(_dsBankBal.Tables[0].Rows[0]["Recon"]);
                        lblBeginBalance.Text = _balance.ToString("0.00", CultureInfo.InvariantCulture);
                        hdnChartID.Value = _dsBankBal.Tables[0].Rows[0]["Chart"].ToString();
                        //hdnRolName.Value = _dsBankBal.
                        // lblDifference.Text = _balance.ToString("0.00", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        lblBeginBalance.Text = "0.00";
                        //  lblDifference.Text = "0.00";
                    }
                }
                else
                {
                    txtEndingBalance.ReadOnly = true;
                    lblBeginBalance.Text = "0.00";
                    //  lblDifference.Text = "0.00";
                }
                DateTime _statementDate = Convert.ToDateTime(txtStatementDate.Text);
                BindCheckGrid(_statementDate);
                BindDepositGrid(_statementDate);
                ScriptManager.RegisterStartupScript(this, GetType(), "calDifference", "calculateDifference();", true);
            }   
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void txtStatementDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtStatementDate = (TextBox)sender;
            if(!string.IsNullOrEmpty(txtStatementDate.Text))
            {
                if (IsValidDate())
                {
                    DateTime _statementDate = Convert.ToDateTime(txtStatementDate.Text);
                    BindCheckGrid(_statementDate);
                    BindDepositGrid(_statementDate);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkBtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidateForm())
            {
                int j = 0;
                double totalOutCheck = 0.00; double totalOutDep = 0.00;      // total outstanding check, deposit
                SetInitialRow();
                DataTable _dt = (DataTable)ViewState["OpenTrans"];
                DataTable dtBank = (DataTable)ViewState["BankRecon"];
                
                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
                _objBank.Balance = Convert.ToDouble(txtEndingBalance.Text);
                _objBank.LastReconDate = DateTime.Now;
                if (!string.IsNullOrEmpty(hdnServiceAcct.Value))
                {
                    _objBank.ServiceAcct = Convert.ToInt32(hdnServiceAcct.Value);
                }
                if (!string.IsNullOrEmpty(txtServiceChargeDate.Text))
                {
                    _objBank.ServiceDate = Convert.ToDateTime(txtServiceChargeDate.Text);
                }
                if (!string.IsNullOrEmpty(txtServiceChrgAmount.Text))
                {
                    _objBank.ServiceCharge = Convert.ToDouble(txtServiceChrgAmount.Text);
                }
                if (!string.IsNullOrEmpty(hdnInterestAcct.Value))
                {
                    _objBank.InterestAcct = Convert.ToInt32(hdnInterestAcct.Value);
                }
                if (!string.IsNullOrEmpty(txtInterestDate.Text))
                {
                    _objBank.InterestDate = Convert.ToDateTime(txtInterestDate.Text);
                }
                if (!string.IsNullOrEmpty(txtInterestAmount.Text))
                {
                    _objBank.InterestCharge = Convert.ToDouble(txtInterestAmount.Text);
                }
                
                #region Update Checks and Deposit
                
                foreach (GridViewRow gr in gvCheck.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    Label lblfDate = (Label)gr.FindControl("lblfDate");
                    Label lblfDesc = (Label)gr.FindControl("lblfDesc");
                    Label lblAmount = (Label)gr.FindControl("lblAmount");
                    Label lblRef = (Label)gr.FindControl("lblRef");
                    Label lblType = (Label)gr.FindControl("lblType");
                    HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                    if (chkSelect.Checked.Equals(true))
                    {
                        HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");
                        HiddenField hdnTypeNum = (HiddenField)gr.FindControl("hdnTypeNum");
                        HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                        DataRow drbank = null;
                        drbank = dtBank.NewRow();
                        drbank["ID"] = Convert.ToInt32(hdnID.Value);
                        drbank["fDate"] = Convert.ToDateTime(lblfDate.Text);
                        drbank["TypeNum"] = Convert.ToInt32(hdnTypeNum.Value);
                        drbank["Ref"] = lblRef.Text;
                        drbank["Amount"] = Convert.ToDouble(hdnAmount.Value);
                        drbank["Batch"] = Convert.ToInt32(hdnBatch.Value);
                        dtBank.Rows.Add(drbank);
                    }
                    else
                    {
                        DataRow dr = null;
                        dr = _dt.NewRow();
                        dr["RowID"] = j + 1;
                        dr["ID"] = 0;
                        dr["fDate"] = Convert.ToDateTime(lblfDate.Text);
                        dr["Type"] = "Check/Credit";
                        dr["Ref"] = lblRef.Text;
                        dr["fDesc"] = lblfDesc.Text;
                        
                        dr["Amount"] = (double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint) * -1);
                        totalOutCheck = totalOutCheck + double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                                  NumberStyles.AllowThousands |
                                                                                  NumberStyles.AllowDecimalPoint);
                        _dt.Rows.Add(dr);
                    }
                }

                foreach (GridViewRow gr in gvDeposit.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    Label lblfDate = (Label)gr.FindControl("lblfDate");
                    Label lblfDesc = (Label)gr.FindControl("lblfDesc");
                    Label lblAmount = (Label)gr.FindControl("lblAmount");
                    Label lblRef = (Label)gr.FindControl("lblRef");
                    Label lblType = (Label)gr.FindControl("lblType");
                    HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                    if (chkSelect.Checked.Equals(true))
                    {
                        HiddenField hdnBatch = (HiddenField)gr.FindControl("hdnBatch");
                        HiddenField hdnTypeNum = (HiddenField)gr.FindControl("hdnTypeNum");    
                        HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                            
                        DataRow drbank = null;
                        drbank = dtBank.NewRow();
                        drbank["ID"] = Convert.ToInt32(hdnID.Value);
                        drbank["fDate"] = Convert.ToDateTime(lblfDate.Text);
                        drbank["TypeNum"] = Convert.ToInt32(hdnTypeNum.Value);
                        drbank["Ref"] = lblRef.Text;
                        drbank["Amount"] = Convert.ToDouble(hdnAmount.Value);
                        drbank["Batch"] = Convert.ToInt32(hdnBatch.Value);
                        //drbank["TypeNum"] = hdnTypeNum.Value;
                        dtBank.Rows.Add(drbank);
                    }
                    else
                    {
                        DataRow dr = null;
                        dr = _dt.NewRow();
                        dr["RowID"] = j + 1;
                        dr["ID"] = 0;
                        dr["fDate"] = Convert.ToDateTime(lblfDate.Text);
                        dr["Type"] = "Deposit/Debit";
                        dr["Ref"] = lblRef.Text;
                        dr["fDesc"] = lblfDesc.Text;
                        dr["Amount"] = double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                              NumberStyles.AllowThousands |
                                                                              NumberStyles.AllowDecimalPoint);
                        totalOutDep = totalOutDep + double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                              NumberStyles.AllowThousands |
                                                                              NumberStyles.AllowDecimalPoint);
                        _dt.Rows.Add(dr);
                    }
                }
                _dt.DefaultView.Sort = "fDate ASC";
                ViewState["OpenTrans"] = _dt.DefaultView.ToTable();
                #endregion
                _objBank.DtBank = dtBank;
                _objBLBank.BankRecon(_objBank);

                GetBankReconReport(totalOutCheck, totalOutDep);
                
                ScriptManager.RegisterStartupScript(this, GetType(), "displayBankRecon", "displayBankRecon();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    
    #endregion

    #region Custom Functions
    //private void UpdateClearedItemDetails(int _batch, int _itemNum)
    //{
    //    try
    //    {
    //        if(_batch > 0)
    //        {
    //            _objTrans.ConnConfig = Session["config"].ToString();
    //            _objTrans.BatchID = _batch;
    //            if (_itemNum.Equals((int)CommonHelper.BankReconItem.Check))         //Check
    //                _objTrans.Type = 20;
    //            else if (_itemNum.Equals((int)CommonHelper.BankReconItem.Deposit))    //Deposit
    //                _objTrans.Type = 5;
    //            else                           //Bank Adj Journal
    //            {
    //                _objTrans.Type = 30;
    //            }
                
    //            _objTrans.Sel = 1;
    //            _objBLJournal.UpdateClearItem(_objTrans);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    //private void UpdateBankBalance()
    //{
    //    try
    //    {
    //        _objBank.ConnConfig = Session["config"].ToString();
    //        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

    //        _objChart.ConnConfig = Session["config"].ToString();
    //        _objChart.ID = _objBLBank.GetChartByBank(_objBank);
    //        _objChart.Amount = Convert.ToDouble(txtEndingBalance.Text);
    //        _objBLChart.UpdateBankBalance(_objChart);
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    //private void UpdateReconciliationBal()
    //{
    //    try
    //    {
    //        _objBank.ConnConfig = Session["config"].ToString();
    //        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
    //        _objBank.Recon = Convert.ToDouble(txtEndingBalance.Text);
    //        _objBank.LastReconDate = DateTime.Today;
    //        _objBLBank.UpdateBankRecon(_objBank);
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    //private void MakeBankAdj()
    //{
    //    try
    //    {
    //        _objJournal.ConnConfig = Session["config"].ToString();
    //        int _batch = _objBLJournal.GetMaxTransBatch(_objJournal);
    //        int _ref = _objBLJournal.GetMaxTransRef(_objJournal);
    //        int i = 0;
    //        bool _isBankTax = false;
    //        double _balanceAmt = 0.00;
    //        double _serviceChrg = Convert.ToDouble(txtServiceChrgAmount.Text);
    //        double _interestChrg = Convert.ToDouble(txtInterestAmount.Text);
    //        if (_serviceChrg > 0 && _interestChrg > 0)
    //        {
    //            _balanceAmt = _serviceChrg - _interestChrg;
    //        }

    //        if (_serviceChrg > 0 || _interestChrg > 0)
    //        {
    //            _objJournal.ConnConfig = Session["config"].ToString();
    //            _objJournal.Ref = _ref;
    //            _objJournal.BatchID = _batch;
    //            _objJournal.GLDate = DateTime.Today.Date;
    //            _objJournal.GLDesc = "Bank reconciliation " + DateTime.Today.ToString("MM/dd/yyyy");
    //            _objJournal.Internal = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();
    //            _objBLJournal.AddGLA(_objJournal);              // Add into GLA table

    //        }
    //        if (_serviceChrg > 0)
    //        {
    //            if (_balanceAmt > 0)
    //                _serviceChrg = _serviceChrg * -1;
    //            _objTrans = new Transaction();

    //            _objTrans.ConnConfig = Session["config"].ToString();
    //            _objTrans.BatchID = _batch;
    //            _objTrans.Ref = _ref;
    //            _objTrans.Sel = 0;
    //            _objTrans.Line = i;
    //            _objTrans.TransDate = Convert.ToDateTime(txtServiceChargeDate.Text);
    //            _objTrans.TransDescription = "Bank Service Charge";
    //            _objTrans.Acct = Convert.ToInt32(hdnServiceAcct.Value);
    //            _objTrans.Amount = _serviceChrg;
    //            _objTrans.Type = 31;
    //            i++;
    //            _isBankTax = true;
    //            _objBLJournal.AddJournalTrans(_objTrans);
    //        }
    //        if (_interestChrg > 0)
    //        {
    //            if (_balanceAmt < 0)
    //                _interestChrg = _interestChrg * -1;
    //            _objTrans = new Transaction();

    //            _objTrans.ConnConfig = Session["config"].ToString();
    //            _objTrans.BatchID = _batch;
    //            _objTrans.Ref = _ref;
    //            _objTrans.Sel = 0;
    //            _objTrans.Line = i;
    //            _objTrans.TransDate = Convert.ToDateTime(txtInterestDate.Text);
    //            _objTrans.TransDescription = "Interest";
    //            _objTrans.Acct = Convert.ToInt32(hdnInterestAcct.Value);
    //            _objTrans.Amount = _interestChrg;
    //            _objTrans.Type = 31;
    //            _isBankTax = true;
    //            _objBLJournal.AddJournalTrans(_objTrans);
    //        }
    //        if (_isBankTax)
    //        {
    //            _objTrans = new Transaction();

    //            _objTrans.ConnConfig = Session["config"].ToString();
    //            _objTrans.BatchID = _batch;
    //            _objTrans.Ref = _ref;
    //            _objTrans.Sel = 1;
    //            _objTrans.Line = i;
    //            _objTrans.TransDate = DateTime.Today.Date;
    //            _objTrans.TransDescription = "Bank reconciliation " + DateTime.Today.ToString("MM/dd/yyyy");
    //            _objTrans.Acct = Convert.ToInt32(hdnChartID.Value);
    //            _objTrans.Amount = _balanceAmt * -1;
    //            _objTrans.Type = 30;
    //            _objTrans.AcctSub = Convert.ToInt32(ddlBank.SelectedValue);
    //            //_objTrans.Status = "";
    //            _objBLJournal.AddJournalTrans(_objTrans);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    private bool ValidateForm()
    {
        bool _isValid = false;
        try
        {
            double _serviceChrg = Convert.ToDouble(txtServiceChrgAmount.Text);
            double _interestChrg = Convert.ToDouble(txtInterestAmount.Text);

            if(_serviceChrg > 0)
            {
                if(!string.IsNullOrEmpty(txtServiceChargeDate.Text) && (!string.IsNullOrEmpty(txtServiceAccount.Text)))
                {
                    _isValid = true;
                }
                else
                {
                    _isValid = false;
                }
            }
            else
            {
                _isValid = true;
            }
            if(_isValid.Equals(true))
            {
                if (_interestChrg > 0)
                {
                    if (!string.IsNullOrEmpty(txtInterestDate.Text) && (!string.IsNullOrEmpty(txtInterestAccount.Text)))
                    {
                        _isValid = true;
                    }
                    else
                    {
                        _isValid = false;
                    }
                }
                else
                {
                    _isValid = true;
                }
            }
        

            //double _differnce = Convert.ToDouble(lblDifference.Text);
        
            double _differnce = Convert.ToDouble(hdnDifference.Value);
            if(_isValid.Equals(false))
            {
                //string str = "Please fill Service charge/Interest Details.";
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);

                ScriptManager.RegisterStartupScript(this, GetType(), "ValidateBankRecon", "ValidateBankRecon(1,0);", true);
            }
            else if (!_differnce.Equals(0))
            {
                _isValid = false;
                if (_differnce < 0) _differnce = _differnce * -1;
                ScriptManager.RegisterStartupScript(this, GetType(), "ValidateBankRecon", "ValidateBankRecon(2,'" + _differnce.ToString("0.00", CultureInfo.InvariantCulture) + "');", true);
                //string str = "You bank reconciliation is off by $" + _differnce.ToString("0.00", CultureInfo.InvariantCulture) + ". Please correct any mistakes you may have made before proceeding.";
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
            }
            IsValidDate();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return _isValid;
    }
    private void GetBankReconReport(double _outstandingCheck=0.00, double _outstandingDep=0.00)
    {
        try
        {
            lnkBtnSave.Visible = false;
            _objPropUser.ConnConfig = Session["config"].ToString();
            DataSet _dsUser = _objBLReportsData.GetControlForReports(_objPropUser);     //Get loggedin user details

            _objBank.ConnConfig = Session["config"].ToString();
            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
            DataSet _dsBank = _objBLBank.GetBankRolByID(_objBank);    //Get Bank details

            DataTable _dtOpenTrans = (DataTable)ViewState["OpenTrans"];
            rvBankRecon.LocalReport.DataSources.Clear();
            rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsAddress", _dsUser.Tables[0]));
            rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsBank", _dsBank.Tables[0]));
            rvBankRecon.LocalReport.DataSources.Add(new ReportDataSource("dsOpenTrans", _dtOpenTrans));
            rvBankRecon.LocalReport.ReportPath = "Reports/BankRecon.rdlc";

            //double _totalOutstanding = 0.00;
            ReportParameter[] rptParams = new ReportParameter[]{
                new ReportParameter("paramUsername",Session["Username"].ToString()),
                new ReportParameter("paramStatementDate",txtStatementDate.Text),
                new ReportParameter("paramOutstandingCredits",_outstandingCheck.ToString("0.00", CultureInfo.InvariantCulture)),
                new ReportParameter("paramOutstandingDebits",_outstandingDep.ToString("0.00", CultureInfo.InvariantCulture)),
                new ReportParameter("paramBankName",ddlBank.SelectedItem.Text)
            };
            rvBankRecon.LocalReport.EnableExternalImages = false;
            rvBankRecon.LocalReport.SetParameters(rptParams);
   
            rvBankRecon.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void SetBankRecon()
    {
        try
        {
            int _userID = Convert.ToInt32(Session["userid"]);
            if(_userID > 0)
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.UserID = _userID;
            
                DataSet _ds = _objBL_User.GetUserAddress(_objPropUser);

            }
            txtEndingBalance.Text = "0.00";
            lblDifference.Text = "0.00";
            lblBeginBalance.Text = "0.00";
            txtServiceChrgAmount.Text = "0.00";
            txtInterestAmount.Text = "0.00";
            txtEndingBalance.ReadOnly = true;
            //lblDepositCount.Text = "0";
            //lblCheckCount.Text = "0";
            lblDepositAmount.Text = "0.00";
            lblCheckAmount.Text = "0.00";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private DateTime SetStatementDate()
    {
        DateTime _statementDate = DateTime.Now.AddMonths(-1);
        try
        {
            _statementDate = new DateTime(_statementDate.Year, _statementDate.Month, 1);
            _statementDate = _statementDate.AddMonths(1);
            _statementDate = _statementDate.AddDays(-1);
            txtStatementDate.Text = _statementDate.ToString("MM/dd/yyyy");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return _statementDate;
    }
    private void FillBank()
    {
        try
        {
            _objBank.ConnConfig = Session["config"].ToString();
            DataSet _dsBank = new DataSet();
            _dsBank = _objBLBank.GetAllBankNames(_objBank);

            ddlBank.Items.Add(new ListItem(":: Select ::", "0"));

            if(_dsBank.Tables[0].Rows.Count > 0)
            {
                ddlBank.AppendDataBoundItems = true;
                ddlBank.DataSource = _dsBank;

                ddlBank.DataValueField = "ID";
                ddlBank.DataTextField = "fDesc";
                ddlBank.DataBind();
            }
            else
            {
                ddlBank.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void BindCheckGrid(DateTime _statementDate)
    {
        try
        {
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.fDate = _statementDate;
            _objCD.fDateYear = _statementDate.Year;
            if(!ddlBank.SelectedValue.Equals("0"))
            {
                _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            }
            DataSet _ds = _objBL_Bills.GetChecksDetails(_objCD);

            gvCheck.DataSource = _ds;
            gvCheck.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void BindDepositGrid(DateTime _statementDate)
    {
        try
        {
            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.fDate = _statementDate;
            _objDep.fDateYear = _statementDate.Year;
            if (!ddlBank.SelectedValue.Equals("0"))
            {
                _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            }
            DataSet _ds = _objBL_Deposit.GetDepositDetails(_objDep);

            gvDeposit.DataSource = _ds;
            gvDeposit.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("fDate", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Type", typeof(string)));
        dt.Columns.Add(new DataColumn("Ref", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(double)));

        ViewState["OpenTrans"] = dt;

        DataTable dtbank = new DataTable();
        dtbank.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("fDate", typeof(DateTime)));
        dtbank.Columns.Add(new DataColumn("Type", typeof(string)));
        dtbank.Columns.Add(new DataColumn("Ref", typeof(string)));
        //dtbank.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dtbank.Columns.Add(new DataColumn("Amount", typeof(double)));
        dtbank.Columns.Add(new DataColumn("Batch", typeof(Int32)));
        dtbank.Columns.Add(new DataColumn("TypeNum", typeof(Int32)));
        
        ViewState["BankRecon"] = dtbank;
    }
    private bool IsValidDate()
    {
        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy", 
                "MM/dd/yyyy", "M/d/yyyy", 
                "M/d/yyyy", "M/d/yyyy", 
                "M/d/yyyy", "M/d/yyyy", 
                "MM/dd/yyyy", "M/dd/yyyy"};
        var dt = DateTime.TryParseExact(txtStatementDate.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);

        if (dt)
        {
            return true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }
    #endregion

}