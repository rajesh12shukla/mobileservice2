using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class JournalEntry : System.Web.UI.Page
{

    #region Variables
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    Journal _objJournal = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    BL_GLARecur _objBLGLARecur = new BL_GLARecur();
    GeneralFunctions objGeneral = new GeneralFunctions();

    Bank _objBank = new Bank();
    BL_BankAccount _objBLBank = new BL_BankAccount();

    TransBankAdj _objTransBank = new TransBankAdj();

    User _objPropUser = new User();
    BL_User objBLUser = new BL_User();
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
                userpermissions();
                divSuccess.Visible = false;
                txtProof.Text = "0";
                FillFrequency();
                imgCleared.Visible = false;
                //txtTransDate.Attributes.Add("readonly", "readonly");
                txtProof.Attributes.Add("readonly", "readonly");

                gvJournal.Columns[6].Visible = false;
                gvJournal.Columns[7].Visible = false;
                gvJournal.Columns[8].Visible = false;

                if (Request.QueryString["id"] != null)
                {
                    #region Update

                    List<string> _lstDeletedTrans = new List<string>();
                    ViewState["DeletedTrans"] = _lstDeletedTrans;
                    if (Request.QueryString["r"] != null)
                    {
                        if (Request.QueryString["r"].ToString() == "1")
                        {
                            lblHeader.Text = "Update Recurring Entry";
                            lblFrequency.Visible = true;
                            ddlFrequency.Visible = true;
                            hdnIsRecurr.Value = "true";

                            SetForUpdate(Convert.ToInt32(Request.QueryString["id"]), true);
                            chkIsRecurr.Checked = true;
                            chkIsRecurr.Attributes.Add("onclick", "return false;");
                        }
                    }
                    else
                    {
                        //lblAddEditUser.Text = "Update Journal Entry ";
                        lblHeader.Text = "Update Journal Entry";
                        lblFrequency.Visible = false;
                        ddlFrequency.Visible = false;
                        hdnIsRecurr.Value = "false";

                        //Update Journal entry
                        SetForUpdate(Convert.ToInt32(Request.QueryString["id"]));
                        chkIsRecurr.Attributes.Add("onclick", "return false;");

                    }
                    //SetAccountName();
                    #endregion

                    //DateTime _transDate = Convert.ToDateTime(txtTransDate.Text);
                    GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text));
                }
                else
                {
                    #region Add

                    lblHeader.Text = "Add Journal Entry";
                    lblFrequency.Visible = false;
                    ddlFrequency.Visible = false;
                    hdnIsRecurr.Value = "false";
                    SetInitialRow();

                    _objJournal.ConnConfig = Session["config"].ToString();
                    DataSet _dsTransId = new DataSet();
                    _dsTransId = _objBLJournal.GetMaxTransID(_objJournal);

                    hdnTransID.Value = _dsTransId.Tables[0].Rows[0]["MAXID"].ToString();
                    txtEntryNo.Text = _dsTransId.Tables[0].Rows[0]["MAXID"].ToString();

                    #endregion
                }
                Permission();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;
        if(!_flag)
        {
            divSuccess.Visible = true;
        }
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("financeMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkFinanceMgr");
        //a.Style.Add("color", "#2382b2"); 

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkJournalEntry");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }

    #endregion

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvJournal.Rows)
        {
            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");

            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + gvJournal.ClientID + "',event);";

        }
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvJournal.ClientID + "');", true);
    }

    #region Save Data
    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] != null)
            {
                bool _flag = (bool)ViewState["FlagPeriodClose"];
                if (_flag)
                {
                    //Update Journal entry
                    if (ValidateGridView())
                    {
                        int tref = Convert.ToInt32(Request.QueryString["id"].ToString());
                        List<Transaction> _lstTrans = GetTransactions();
                        UpdateJournal(_lstTrans);
                        _objChart.ConnConfig = Session["config"].ToString();
                        _objBLChart.CalChartBalance(_objChart);
                        //Response.Redirect("journalentry.aspx");
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'JE Updated Successfully! <BR/> Ref # " + tref.ToString() + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
            else
            {
                if (ValidateGridView())
                {
                    GetPeriodDetails(Convert.ToDateTime(txtTransDate.Text));
                    bool _flag = (bool)ViewState["FlagPeriodClose"];
                    int tref = 0;
                    if (_flag)
                    {
                        List<Transaction> _lstTrans = GetTransactions();
                        if (chkIsRecurr.Checked.Equals(true))
                        {
                            tref = AddRecurEntry(_lstTrans);
                        }
                        else
                        {
                            tref = AddJournalEntry(_lstTrans);
                            _objChart.ConnConfig = Session["config"].ToString();
                            _objBLChart.CalChartBalance(_objChart);
                        }
                        ResetFormControlValues(this);
                        SetNewJournalEntry();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'JE Added Successfully! <BR/> Ref # " + tref.ToString() + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Add New lines
    protected void lbtnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
            int rowIndex = gvJournal.Rows.Count - 1;
            Label lblId = gvJournal.Rows[rowIndex].Cells[0].FindControl("lblId") as Label;
            rowIndex = Convert.ToInt32(lblId.Text);
            SetGridViewData();
            DataTable dt = (DataTable)ViewState["Transactions"];

            DataRow dr = null;
            for (int i = 0; i < 8; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                rowIndex++;
            }

            gvJournal.DataSource = dt;
            gvJournal.DataBind();
            //SetAccountName();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    protected void gvJournal_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    GridViewRow row = e.Row;
                    TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                    txtGvJob.ReadOnly = true;

                    TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                    txtGvAcctNo.ReadOnly = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvJournal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (gvJournal.Rows.Count > 0)
            {
                gvJournal.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            //First fetch your textboxes and labeles 

            if (e.CommandName == "DeleteTransaction")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvJournal.Rows[index];

                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "ClearRow('" + row.ClientID + "');", true);

                if (Request.QueryString["id"] != null)
                {
                    DataTable _dtTrans = (DataTable)ViewState["Transactions"];

                    Label lblTID = (Label)row.FindControl("lblTID");
                    if (!Convert.ToInt32(lblTID.Text).Equals(0))
                    {
                        if (_dtTrans.Rows.Count > 0)
                        {
                            DataRow _drTrans = _dtTrans.Select("ID = " + lblTID.Text).SingleOrDefault();
                            if (_drTrans != null)
                            {
                                if (Convert.ToInt32(_drTrans["ID"]) != 0)
                                {
                                    List<string> _lstDeletedTrans = (List<string>)ViewState["DeletedTrans"];
                                    _lstDeletedTrans.Add(_drTrans["ID"].ToString());
                                    ViewState["DeletedTrans"] = _lstDeletedTrans;
                                }
                            }
                        }
                    }
                    lblTID.Text = "0";
                
                }

                HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                TextBox txtGvAcctName = (TextBox)row.FindControl("txtGvAcctName");
                TextBox txtGvtransDes = (TextBox)row.FindControl("txtGvtransDes");
                TextBox txtGvDebit = (TextBox)row.FindControl("txtGvDebit");
                TextBox txtGvCredit = (TextBox)row.FindControl("txtGvCredit");
                TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");

                hdnAcctID.Value = "0";
                txtGvAcctNo.Text = "";
                txtGvAcctName.Text = "";
                txtGvtransDes.Text = "";
                txtGvDebit.Text = "";
                txtGvCredit.Text = "";
                txtGvLoc.Text = "";
                txtGvJob.Text = "";
                txtGvPhase.Text = "";
                hdnPID.Value = "0";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CheckBalanceProof", "BalanceProof();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("journalentry.aspx");
    }
    protected void chkJobSpecific_CheckedChanged(object sender, EventArgs e)
    {
        if (chkJobSpecific.Checked == true)
        {
            gvJournal.Columns[6].Visible = true;
            gvJournal.Columns[7].Visible = true;
            gvJournal.Columns[8].Visible = true;
        }
        else
        {
            gvJournal.Columns[6].Visible = false;
            gvJournal.Columns[7].Visible = false;
            gvJournal.Columns[8].Visible = false;
        }
    }
    protected void txtGvCredit_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtGvCredit = (TextBox)sender;
            double defaultVal;
            bool isFloat = Double.TryParse(txtGvCredit.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out defaultVal);

            if (isFloat)
            {
                txtGvCredit.Text = Convert.ToDouble(txtGvCredit.Text).ToString("0.00", CultureInfo.InvariantCulture);
                if (!Convert.ToDouble(txtGvCredit.Text).Equals(0))
                {
                    GridViewRow gridrow = (GridViewRow)txtGvCredit.NamingContainer;
                    int rowIndex = gridrow.RowIndex + 1;
                    foreach (GridViewRow row in gvJournal.Rows)
                    {
                        if (row.RowIndex == rowIndex)
                        {
                            TextBox nxtTxtGvCredit = (TextBox)row.FindControl("txtGvCredit");
                            TextBox nxtTxtGvDebit = (TextBox)row.FindControl("txtGvDebit");
                            if (!Convert.ToDouble(txtProof.Text).Equals(0))
                            {
                                if (string.IsNullOrEmpty(nxtTxtGvCredit.Text) && string.IsNullOrEmpty(nxtTxtGvDebit.Text))
                                {
                                    if (hdnIsPositive.Value.Equals("false"))
                                    {
                                        nxtTxtGvCredit.Text = "0.00";
                                        nxtTxtGvDebit.Text = txtProof.Text;
                                    }
                                    else
                                    {
                                        nxtTxtGvCredit.Text = txtProof.Text;
                                        nxtTxtGvDebit.Text = "0.00";
                                    }
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "BalanceProof", "BalanceProof();", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void txtGvDebit_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtGvDebit = (TextBox)sender;
            double defaultVal;
            bool isFloat = Double.TryParse(txtGvDebit.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out defaultVal);

            if (isFloat)
            {
                txtGvDebit.Text = Convert.ToDouble(txtGvDebit.Text).ToString("0.00", CultureInfo.InvariantCulture);
                if (!Convert.ToDouble(txtGvDebit.Text).Equals(0))
                {
                    GridViewRow gridrow = (GridViewRow)txtGvDebit.NamingContainer;
                    int rowIndex = gridrow.RowIndex + 1;
                    foreach (GridViewRow row in gvJournal.Rows)
                    {
                        if (row.RowIndex == rowIndex)
                        {
                            TextBox nxtTxtGvCredit = (TextBox)row.FindControl("txtGvCredit");
                            TextBox nxtTxtGvDebit = (TextBox)row.FindControl("txtGvDebit");
                            if (!Convert.ToDouble(txtProof.Text).Equals(0))
                            {
                                if (string.IsNullOrEmpty(nxtTxtGvCredit.Text) && string.IsNullOrEmpty(nxtTxtGvDebit.Text))
                                {
                                    if (hdnIsPositive.Value.Equals("false"))
                                    {
                                        nxtTxtGvCredit.Text = "0.00";
                                        nxtTxtGvDebit.Text = txtProof.Text;
                                    }
                                    else
                                    {
                                        nxtTxtGvCredit.Text = txtProof.Text;
                                        nxtTxtGvDebit.Text = "0.00";
                                    }
                                }
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "BalanceProof", "BalanceProof();", true);
                }
            }
            else
                txtGvDebit.Text = "0.00";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void chkIsRecurr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsRecurr.Checked.Equals(true))
        {
            lblFrequency.Visible = true;
            ddlFrequency.Visible = true;

            ScriptManager.RegisterStartupScript(this, GetType(), "clearEntry", "clearEntry();", true);
        }
        else
        {
            lblFrequency.Visible = false;
            ddlFrequency.Visible = false;

            //txtEntryNo.Text = hdnTransID.Value;
            ScriptManager.RegisterStartupScript(this, GetType(), "clearEntry", "clearEntry();", true);
        }
    }
    #endregion

    #region Custom Functions

    #region Fill Frequency

    private void FillFrequency()
    {
        try
        {
            List<Frequency> _lstFrequency = new List<Frequency>();
            _lstFrequency = FrequencyHelper.GetAll();
            ddlFrequency.DataSource = _lstFrequency;
            ddlFrequency.DataValueField = "ID";
            ddlFrequency.DataTextField = "Name";
            ddlFrequency.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Set Initial Row
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Account", typeof(string)));
        dt.Columns.Add(new DataColumn("Description", typeof(string)));
        dt.Columns.Add(new DataColumn("Debit", typeof(string)));
        dt.Columns.Add(new DataColumn("Credit", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("TimeStamp", typeof(string)));    //change on 23rd dec, 15

        int rowIndex = 0;
        for (int i = 0; i < 8; i++)
        {
            dr = dt.NewRow();
            dr["RowID"] = i + 1;
            dr["ID"] = 0;
            dr["AcctID"] = 0;
            dr["AcctNo"] = string.Empty;
            dr["Account"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["Debit"] = string.Empty;
            dr["Credit"] = string.Empty;
            dr["Loc"] = string.Empty;
            dr["JobName"] = string.Empty;
            dr["JobID"] = string.Empty;
            dr["Phase"] = string.Empty;
            dr["PhaseID"] = 0;
            dr["TimeStamp"] = string.Empty;
            dt.Rows.Add(dr);
            rowIndex++;
        }

        ViewState["Transactions"] = dt;
        gvJournal.DataSource = dt;
        gvJournal.DataBind();
    }
    #endregion
    private bool ValidateGridView()
    {
        bool _isValid = true;
        try
        {
            int _filledRows = 0;
        
            string _validationStr = "";
            for (int i = 0; i < gvJournal.Rows.Count; i++)
            {
                List<TransactionModel> _lstValues = new List<TransactionModel>();
                HiddenField hdnAcctID = (HiddenField)gvJournal.Rows[i].Cells[1].FindControl("hdnAcctID");
                TextBox txtGvAcctName = (TextBox)gvJournal.Rows[i].Cells[1].FindControl("txtGvAcctName");
                TextBox txtGvtransDes = (TextBox)gvJournal.Rows[i].Cells[2].FindControl("txtGvtransDes");
                TextBox txtGvDebit = (TextBox)gvJournal.Rows[i].Cells[3].FindControl("txtGvDebit");
                TextBox txtGvCredit = (TextBox)gvJournal.Rows[i].Cells[4].FindControl("txtGvCredit");

                HiddenField hdnJobID = (HiddenField)gvJournal.Rows[i].Cells[3].FindControl("hdnJobID");
                //TextBox txtGvJob = (TextBox)gvJournal.Rows[i].Cells[3].FindControl("txtGvJob");
                TextBox txtGvPhase = (TextBox)gvJournal.Rows[i].Cells[4].FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)gvJournal.Rows[i].Cells[4].FindControl("hdnPID");
                bool _isJobSpec = false;
                if (!string.IsNullOrEmpty(hdnAcctID.Value) && !string.IsNullOrEmpty(txtGvtransDes.Text) && !string.IsNullOrEmpty(txtGvDebit.Text) && !string.IsNullOrEmpty(txtGvCredit.Text))
                {
                    //if(!hdnAcctID.Value.Equals("0"))
                    if (chkJobSpecific.Checked == true)
                    {
                        _isJobSpec = true;
                        if (!string.IsNullOrEmpty(hdnJobID.Value) && !string.IsNullOrEmpty(hdnPID.Value))
                            _filledRows++;
                    }
                    else
                    {
                        _filledRows++;
                    }
                }
                _lstValues.Add(new TransactionModel(0, txtGvAcctName.Text));
                _lstValues.Add(new TransactionModel(1, hdnAcctID.Value));
                _lstValues.Add(new TransactionModel(2, txtGvtransDes.Text));
                _lstValues.Add(new TransactionModel(3, txtGvDebit.Text));
                _lstValues.Add(new TransactionModel(4, txtGvCredit.Text));
                _lstValues.Add(new TransactionModel(5, hdnJobID.Value));
                _lstValues.Add(new TransactionModel(6, txtGvPhase.Text));
                _lstValues.Add(new TransactionModel(7, hdnPID.Value));
                bool _isEntered = false;
                foreach (var lst in _lstValues)
                {
                    switch (lst.FieldValue)
                    {
                        case 0:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;
                            break;
                        //case 1:
                        //    if (!lst.Field.Length.Equals(0))
                        //    {
                        //        //if(!lst.Field.Equals("0"))
                        //        //{ _isEntered = true; }
                        //        _isEntered = true; 
                        //    }
                        //    break;
                        case 2:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;
                            break;
                        case 3:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;

                            break;
                        case 4:
                            if (!lst.Field.Length.Equals(0))
                                _isEntered = true;
                            break;
                        case 5:
                            if (_isJobSpec.Equals(true))
                            {
                                if (!lst.Field.Length.Equals(0))
                                    _isEntered = true;
                            }
                            break;
                        case 6:
                            if (_isJobSpec.Equals(true))
                            {
                                if (!lst.Field.Length.Equals(0))
                                    _isEntered = true;
                            }
                            break;
                    }

                    if (lst.FieldValue.Equals(4))
                    {
                        if (_isEntered)
                        {
                            #region validate

                            foreach (var l in _lstValues)
                            {
                                switch (l.FieldValue)
                                {
                                    //case 0:
                                    //    if (l.Field.Equals(" "))
                                    //    {
                                    //        _isValid = false;
                                    //        _validationStr = "Please select account name.";
                                    //    }
                                    //    break;
                                    case 1:
                                        if (!l.Field.Length.Equals(0))
                                        {
                                            if (l.Field.Equals("0"))
                                            {
                                                _isValid = false;
                                                _validationStr = "Please select valid account name.";
                                            }
                                        }
                                        break;
                                    case 2:
                                        if (l.Field.Length.Equals(0))
                                        {
                                            _isValid = false;
                                            _validationStr = "Please enter transaction memo.";
                                        }
                                        break;
                                    case 3:
                                        if (l.Field.Length.Equals(0))
                                        {
                                            _isValid = false;
                                            _validationStr = "Please enter debit/credit amount.";
                                        }
                                        break;
                                    case 4:
                                        if (l.Field.Length.Equals(0))
                                        {
                                            _isValid = false;
                                            _validationStr = "Please enter debit/credit amount.";
                                        }
                                        break;
                                    case 5:
                                        if (_isJobSpec.Equals(true))
                                        {
                                            if (l.Field.Length.Equals(0))
                                            {
                                                _isValid = false;
                                                _validationStr = "Please select job.";
                                            }
                                        }
                                        break;
                                    //case 6:
                                    //    if (_isJobSpec.Equals(true))
                                    //    {
                                    //        if (l.Field.Length.Equals(0))
                                    //        {
                                    //            _isValid = false;
                                    //            _validationStr = "Please select phase.";
                                    //        }
                                    //    }
                                    //    break;
                                    case 7:
                                        if (!l.Field.Length.Equals(0))
                                        {
                                            if (l.Field.Equals("0"))
                                            {
                                                _isValid = false;
                                                _validationStr = "Please select valid phase.";
                                            }
                                        }
                                        break;
                                }
                                if (_isValid.Equals(false))
                                    break;
                            }
                            #endregion
                        }
                    }
                    if (_isValid.Equals(false)) break;
                }
                //if (_isValid.Equals(false)) break;

            }
            if (_filledRows < 2)
            {
                _isValid = false;
                _validationStr = "You must fill out atleast 2 lines.";
            }
            if (!string.IsNullOrEmpty(_validationStr))
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + _validationStr + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _isValid;
    }
    private List<Transaction> GetTransactions()
    {
        List<Transaction> _lstTrans = new List<Transaction>();

        try
        {
            foreach (GridViewRow gr in gvJournal.Rows)
            {
                HiddenField hdnAcctID = (HiddenField)gr.FindControl("hdnAcctID");
                TextBox txtGvAcctName = (TextBox)gr.FindControl("txtGvAcctName");
                TextBox txtGvtransDes = (TextBox)gr.FindControl("txtGvtransDes");
                TextBox txtGvDebit = (TextBox)gr.FindControl("txtGvDebit");
                TextBox txtGvCredit = (TextBox)gr.FindControl("txtGvCredit");
                Label lblTID = (Label)gr.FindControl("lblTID");
                Label lblTimeStamp = (Label)gr.FindControl("lblTimeStamp");

                HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
                TextBox txtGvJob = (TextBox)gr.FindControl("txtGvJob");
                TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)gr.FindControl("hdnPID");

                //String _acctName = ddlGvAcctName.SelectedItem.Text;
                //String _acct = ddlGvAcctName.SelectedValue;
                String _acctName = txtGvAcctName.Text;
                String _acct = hdnAcctID.Value;
                String _description = txtGvtransDes.Text;
                String _debitAmt = txtGvDebit.Text;
                String _creditAmt = txtGvCredit.Text;
                String _jobName = txtGvJob.Text;
                String _jobID = hdnJobID.Value;
                String _phase = txtGvPhase.Text;
                String _phaseId = hdnPID.Value;

                if (!string.IsNullOrEmpty(_acct) && !string.IsNullOrEmpty(_description) && !string.IsNullOrEmpty(_debitAmt) && !string.IsNullOrEmpty(_creditAmt))
                {
                    Transaction _objT = new Transaction();
                    if (!string.IsNullOrEmpty(lblTID.Text))
                    {
                        _objT.ID = Convert.ToInt32(lblTID.Text);
                    }
                    _objT.TransDescription = _description;
                    _objT.Acct = Convert.ToInt32(_acct);
                    if (Convert.ToDouble(_debitAmt).Equals(0))
                    {
                        _objT.Amount = Convert.ToDouble(_creditAmt) * -1;
                    }
                    else
                    {
                        _objT.Amount = Convert.ToDouble(_debitAmt);
                    }
                    if (chkJobSpecific.Checked.Equals(true))
                    {
                        _objT.JobInt = Convert.ToInt32(_jobID);
                        _objT.PhaseDoub = Convert.ToDouble(_phaseId);
                    }
                    if (chkIsRecurr.Checked.Equals(false))           // change on 23rd dec, 15
                    {
                        //   _objT.TimeStamp = Convert.FromBase64String(hdnTimeStamp.Value);
                        _objT.TimeStamp = Encoding.UTF8.GetBytes(lblTimeStamp.Text);

                    }                        //_objT.TimeStamp = Convert.FromBase64String(lblTimeStamp.Text);
                    _lstTrans.Add(_objT);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _lstTrans;
    }
    private bool CheckIsBankJournal(List<Transaction> _lstTrans)
    {
        bool _isBankAcct = false;
        try
        {
            foreach(var _lst in _lstTrans)
            {
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.ID = _lst.Acct;
                _isBankAcct = _objBLChart.IsChartBankAcct(_objChart);
                if (_isBankAcct)
                    break;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _isBankAcct;
    }

    #region Add JE
    private int AddJournalEntry(List<Transaction> _lstTrans)
    {   
        _objJournal.Ref=0;
        try
        {
            _objJournal.ConnConfig = Session["config"].ToString();
            _objJournal.Ref = _objBLJournal.GetMaxTransRef(_objJournal);
            _objJournal.BatchID = _objBLJournal.GetMaxTransBatch(_objJournal);
            _objJournal.GLDate = Convert.ToDateTime(txtTransDate.Text);
            _objJournal.GLDesc = txtDescription.Text;
            _objJournal.Internal = txtEntryNo.Text;
            _objBLJournal.AddGLA(_objJournal);              // Add into GLA table
       
            int i = 0;

            bool _isBankJournal = CheckIsBankJournal(_lstTrans); // Check is this bank journal adjustment or journal entry.
            bool _isBankAcct = false;
            foreach (var _lst in _lstTrans)
            {
                _objTrans = new Transaction();

                _objTrans.ConnConfig = Session["config"].ToString();
                _objTrans.BatchID = _objJournal.BatchID;
                _objTrans.Ref = _objJournal.Ref;
                _objTrans.TransDate = _objJournal.GLDate;
                _objTrans.Line = i;
                _objTrans.TransDescription = _lst.TransDescription;
                _objTrans.Acct = _lst.Acct;
                _objTrans.Amount = _lst.Amount;
                _objTrans.Sel = 0;
                if (chkJobSpecific.Checked == true)     // In case of job specific
                {
                    _objTrans.JobInt = _lst.JobInt;
                    _objTrans.PhaseDoub = _lst.PhaseDoub;
                }

                #region Bank Adj
                if (_isBankJournal)                     // If Journal entry is Bank adjustment then Type is 30, 31
                {
                    _objChart.ID = _lst.Acct;
                    _isBankAcct = _objBLChart.IsChartBankAcct(_objChart);
                    if(_isBankAcct)     //If it is Bank account then it is type = 30
                    {
                        _objTrans.Type = 30;
                        _objBank.ConnConfig = Session["config"].ToString();
                        _objBank.Chart = _lst.Acct;
                        _objTrans.AcctSub = _objBLBank.GetBankIDByChart(_objBank);
                    }
                    else
                    {
                        _objTrans.Type = 31;
                    }
                }
                #endregion
         
                _objBLJournal.AddJournalTrans(_objTrans);   //Add Journal Entry

                //#region Update Chart Balance
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.ID = _lst.Acct;
                _objChart.Amount = _lst.Amount;
                //_objBLChart.UpdateChartBalance(_objChart);
                //#endregion

                #region Update Bank Balance
                if (_isBankJournal) //Current JE is Bank adjustment then update bank balance
                {
                    if (_isBankAcct)            //Check is current account is bank account then update it's balance
                    {
                        _objChart.ConnConfig = Session["config"].ToString();
                        _objChart.ID = _lst.Acct;
                        _objChart.Amount = _lst.Amount;
                        _objChart.Batch = _objTrans.BatchID;
                        _objBLChart.UpdateBankBalance(_objChart);

                        InsertBankAdjDetails(_objChart);
                    }
                }
                #endregion
           
                i++;
                
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _objJournal.Ref;
    }
    private int AddRecurEntry(List<Transaction> _lstTrans)
    {
        _objJournal.Ref = 0;
        try
        {
            _objJournal.ConnConfig = Session["config"].ToString();
            _objJournal.Ref = _objBLGLARecur.GetMaxRecurRef(_objJournal);
            _objJournal.GLDate = Convert.ToDateTime(txtTransDate.Text);
            _objJournal.GLDesc = txtDescription.Text;
            _objJournal.Internal = txtEntryNo.Text;
            _objJournal.Frequency = Convert.ToInt32(ddlFrequency.SelectedValue);
            _objBLGLARecur.AddRecur(_objJournal);           // Add into GLARecur table

            int i = 0;
            foreach (var _lst in _lstTrans)
            {
                _objTrans = new Transaction();

                _objTrans.ConnConfig = Session["config"].ToString();
                _objTrans.BatchID = _objJournal.BatchID;
                _objTrans.Ref = _objJournal.Ref;
                _objTrans.TransDate = _objJournal.GLDate;
                _objTrans.Line = i;
                _objTrans.TransDescription = _lst.TransDescription;
                _objTrans.Acct = _lst.Acct;
                _objTrans.Amount = _lst.Amount;
                _objTrans.Sel = 0;
                if (chkJobSpecific.Checked == true)     // In case of job specific
                {
                    _objTrans.JobInt = _lst.JobInt;
                    _objTrans.PhaseDoub = _lst.PhaseDoub;
                }
                _objBLGLARecur.AddRecurTrans(_objTrans);
                i++;
                
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _objJournal.Ref;
    }
    
    #endregion

    #region Update JE

    #region Set Data for Update JE
    private void SetForUpdate(int tRef, bool IsRecur = false)
    {
        try
        {
            _objTrans.ConnConfig = Session["config"].ToString();
            _objJournal.ConnConfig = Session["config"].ToString();
            DataSet dsGLA = new DataSet();
            _objJournal.Ref = tRef;
            _objJournal.IsRecurring = IsRecur;
            
            dsGLA = _objBLJournal.GetDataByRef(_objJournal);
            DataRow drGLA = dsGLA.Tables[0].Rows[0];
            _objJournal.IsJobSpec = Convert.ToBoolean(drGLA["IsJobSpec"]);
            if(_objJournal.IsRecurring.Equals(false))
            {
                hdnBatchID.Value = drGLA["Batch"].ToString();
            }

            txtTransDate.Text = Convert.ToDateTime(drGLA["fDate"]).ToShortDateString();
            txtEntryNo.Text = drGLA["Internal"].ToString();
            txtDescription.Text = drGLA["fDesc"].ToString();
            txtProof.Text = "0";
            if (IsRecur)
            {
                ddlFrequency.Items.FindByValue(drGLA["Frequency"].ToString()).Selected = true;
            }

            DataSet dsTrans = new DataSet();
            _objTrans.Ref = tRef;
            _objTrans.BatchID = Convert.ToInt32(drGLA["Batch"].ToString());
            if (IsRecur.Equals(true))
            {
                dsTrans = _objBLGLARecur.GetTransDataByRef(_objTrans);
            }
            else
            {
                _objTrans.BatchID = Convert.ToInt32(dsGLA.Tables[0].Rows[0]["Batch"]);
                dsTrans = _objBLJournal.GetTransDataByBatch(_objTrans);

                if(dsTrans.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsTrans.Tables[0].Select("Type = 30 AND Sel = 1").SingleOrDefault();
                    if(dr != null)
                    {
                        imgCleared.Visible = true;
                        btnSaveNew.Visible = false;
                    }
                }
            }

            //SetDatatableData(dsTrans, IsRecur);
            DataTable dt = dsTrans.Tables[0];
            if(dsTrans.Tables[0].Rows.Count < 8)
            {
                DataRow dr = null;

                for (int i = dt.Rows.Count; i < 8; i++)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            ViewState["Transactions"] = dt;

            gvJournal.DataSource = dt;
            gvJournal.DataBind();
            if (_objJournal.IsJobSpec)
            {
                chkJobSpecific.Checked = true;
                gvJournal.Columns[6].Visible = true;
                gvJournal.Columns[7].Visible = true;
                gvJournal.Columns[8].Visible = true;
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Set data GridView during Edit
    //private void SetDatatableData(DataSet _dsTrans, bool _isRecurr)
    //{
    //    try
    //    {
    //        //#region Initialize Datatable
    //        //DataTable _dt = new DataTable();
    //        //DataRow dr = null;
    //        //_dt.Columns.Add(new DataColumn("RowID", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
    //        //_dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
    //        //_dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("Account", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("Description", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("Debit", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("Credit", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("Loc", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("JobID", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("JobName", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("Phase", typeof(string)));
    //        //_dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
    //        //_dt.Columns.Add(new DataColumn("TimeStamp", typeof(string)));   //change on 23rd dec, 15
    //        //int rowIndex = 0;
    //        //for (int i = 0; i < 8; i++)
    //        //{
    //        //    dr = _dt.NewRow();
    //        //    dr["RowID"] = i + 1;
    //        //    dr["ID"] = 0;
    //        //    dr["AcctID"] = 0;
    //        //    dr["AcctNo"] = string.Empty;
    //        //    dr["Account"] = string.Empty;
    //        //    dr["Description"] = string.Empty;
    //        //    dr["Debit"] = string.Empty;
    //        //    dr["Credit"] = string.Empty;
    //        //    dr["Loc"] = string.Empty;
    //        //    dr["JobID"] = string.Empty;
    //        //    dr["JobName"] = string.Empty;
    //        //    dr["Phase"] = string.Empty;
    //        //    dr["PhaseID"] = 0;
    //        //    dr["TimeStamp"] = null;                             //change on 23rd dec, 15
    //        //    _dt.Rows.Add(dr);
    //        //    rowIndex++;
    //        //}
    //        //#endregion
    //        ////DataTable _dt = (DataTable)ViewState["Transactions"];
    //        //DataTable _dtTrans = _dsTrans.Tables[0];
    //        //bool _isJobSpecific = false;
            
    //        //if (_dtTrans.Rows.Count > 0)
    //        //{
    //        //    for (int i = 0; i < _dtTrans.Rows.Count; i++)
    //        //    {

    //        //        //_dt.Rows[i]["RowID"]
    //        //        _dt.Rows[i]["ID"] = _dtTrans.Rows[i]["ID"];
    //        //        _dt.Rows[i]["AcctNo"] = _dtTrans.Rows[i]["AcctNo"];
    //        //        _dt.Rows[i]["AcctID"] = _dtTrans.Rows[i]["Acct"];
    //        //        _dt.Rows[i]["Account"] = _dtTrans.Rows[i]["AcctName"];
    //        //        _dt.Rows[i]["Description"] = _dtTrans.Rows[i]["fDesc"];
    //        //        if (Convert.ToDouble(_dtTrans.Rows[i]["Amount"]) == 0)
    //        //        {
    //        //            _dt.Rows[i]["Debit"] = Convert.ToDouble(_dtTrans.Rows[i]["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
    //        //            _dt.Rows[i]["Credit"] = Convert.ToDouble(_dtTrans.Rows[i]["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
    //        //        }
    //        //        else if (Convert.ToDouble(_dtTrans.Rows[i]["Amount"]) < 0)
    //        //        {
    //        //            _dt.Rows[i]["Debit"] = "0.00";
    //        //            _dt.Rows[i]["Credit"] = (Convert.ToDouble(_dtTrans.Rows[i]["Amount"]) * -1).ToString("0.00", CultureInfo.InvariantCulture);
    //        //        }
    //        //        else
    //        //        {
    //        //            _dt.Rows[i]["Debit"] = Convert.ToDouble(_dtTrans.Rows[i]["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
    //        //            _dt.Rows[i]["Credit"] = "0.00";
    //        //        }
    //        //        if (_isRecurr.Equals(true))
    //        //        {
    //        //            #region Job specific Recurring
    //        //            if ((_dtTrans.Rows[i]["Job"] != null) && (_dtTrans.Rows[i]["Phase"] != null))
    //        //            {
    //        //                if (!Convert.ToInt32(_dtTrans.Rows[i]["Job"]).Equals(0) && !Convert.ToDouble(_dtTrans.Rows[i]["Phase"]).Equals(0.00))
    //        //                {
    //        //                    _isJobSpecific = true;

    //        //                    _objTrans.ConnConfig = Session["config"].ToString();
    //        //                    _objTrans.JobInt = Convert.ToInt32(_dtTrans.Rows[i]["Job"]);
    //        //                    DataSet _dsJob = new DataSet();
    //        //                    _dsJob = _objBLJournal.GetLocByJobID(_objTrans);

    //        //                    if (_dsJob.Tables[0].Rows.Count > 0)
    //        //                    {
    //        //                        _dt.Rows[i]["Loc"] = _dsJob.Tables[0].Rows[0]["Tag"];
    //        //                    }
    //        //                    _dt.Rows[i]["JobID"] = _dtTrans.Rows[i]["Job"].ToString();

    //        //                    DataSet _dsJobDesc = new DataSet();     //Get Job name by JobId from "Job"
    //        //                    _dsJobDesc = _objBLJournal.GetJobDetailByID(_objTrans);
    //        //                    if (_dsJobDesc.Tables[0].Rows.Count > 0)
    //        //                    {
    //        //                        _dt.Rows[i]["JobName"] = _dsJobDesc.Tables[0].Rows[0]["fDesc"].ToString();
    //        //                    }

    //        //                    DataSet _dsPhase = new DataSet();       //Get Phase details from "JobTItem"
    //        //                    _objTrans.PhaseDoub = Convert.ToInt32(_dtTrans.Rows[i]["Phase"]);
    //        //                    _objTrans.ConnConfig = Session["config"].ToString();
    //        //                    _dsPhase = _objBLJournal.GetPhaseByID(_objTrans);
    //        //                    if (_dsPhase.Tables[0].Columns.Contains("fDesc"))
    //        //                    {
    //        //                        _dt.Rows[i]["Phase"] = _dsPhase.Tables[0].Rows[0]["fDesc"].ToString();
    //        //                    }

    //        //                    _dt.Rows[i]["PhaseID"] = _dtTrans.Rows[i]["Phase"].ToString();
    //        //                }
    //        //            }
    //        //            #endregion
    //        //        }
    //        //        else
    //        //        {
    //        //            #region Job specific Journal
    //        //            if ((!string.IsNullOrEmpty(_dtTrans.Rows[i]["VInt"].ToString())) && (!string.IsNullOrEmpty(_dtTrans.Rows[i]["VDoub"].ToString())))
    //        //            {
    //        //                if (!Convert.ToInt32(_dtTrans.Rows[i]["VInt"]).Equals(0) && !Convert.ToDouble(_dtTrans.Rows[i]["VDoub"]).Equals(0.00))
    //        //                {
    //        //                    _isJobSpecific = true;

    //        //                    _objTrans.ConnConfig = Session["config"].ToString();
    //        //                    _objTrans.JobInt = Convert.ToInt32(_dtTrans.Rows[i]["VInt"]);
    //        //                    DataSet _dsJob = new DataSet();
    //        //                    _dsJob = _objBLJournal.GetLocByJobID(_objTrans);

    //        //                    if (_dsJob.Tables[0].Rows.Count > 0)
    //        //                    {
    //        //                        _dt.Rows[i]["Loc"] = _dsJob.Tables[0].Rows[0]["Tag"];
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This can not find location.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //        //                    }

    //        //                    _dt.Rows[i]["JobID"] = _dtTrans.Rows[i]["VInt"].ToString();

    //        //                    _objTrans.JobInt = Convert.ToInt32(_dtTrans.Rows[i]["VInt"]);
    //        //                    DataSet _dsJobDesc = new DataSet();         //Get Job name by JobId from "Job"
    //        //                    _dsJobDesc = _objBLJournal.GetJobDetailByID(_objTrans);
    //        //                    if (_dsJobDesc.Tables[0].Rows.Count > 0)
    //        //                    {
    //        //                        _dt.Rows[i]["JobName"] = _dsJobDesc.Tables[0].Rows[0]["fDesc"].ToString();
    //        //                    }

    //        //                    DataSet _dsPhase = new DataSet();           //Get Phase details from "JobTItem"
    //        //                    _objTrans.PhaseDoub = Convert.ToInt32(_dtTrans.Rows[i]["VDoub"]);
    //        //                    _objTrans.ConnConfig = Session["config"].ToString();
    //        //                    _dsPhase = _objBLJournal.GetPhaseByID(_objTrans);
    //        //                    if (_dsPhase.Tables[0].Rows.Count > 0)
    //        //                    {
    //        //                        _dt.Rows[i]["Phase"] = _dsPhase.Tables[0].Rows[0]["fDesc"].ToString();
    //        //                    }
    //        //                    //if (_dsPhase.Tables[0].Columns.Contains("fDesc"))
    //        //                    //{
    //        //                    //    _dt.Rows[i]["Phase"] = _dsPhase.Tables[0].Rows[0]["fDesc"].ToString();
    //        //                    //}

    //        //                    _dt.Rows[i]["PhaseID"] = Convert.ToInt32(_dtTrans.Rows[i]["VDoub"]).ToString();
    //        //                }
    //        //            }
    //        //            #endregion
    //        //        }
    //        //        if(_isRecurr.Equals(false))
    //        //        {
    //        //            if(Convert.ToInt32(_dtTrans.Rows[i]["Type"]).Equals(30))
    //        //            {
    //        //                if(Convert.ToInt16(_dtTrans.Rows[i]["Sel"]).Equals(1))          // if check is cleared
    //        //                {
    //        //                    btnSaveNew.Visible = false;
    //        //                    imgCleared.Visible = true;
    //        //                }
    //        //                if(Convert.ToBoolean(_dtTrans.Rows[i]["IsRecon"].Equals(true))) // if deposit is cleared
    //        //                {
    //        //                    btnSaveNew.Visible = false;
    //        //                    imgCleared.Visible = true;
    //        //                }
    //        //            }

    //        //            //_dt.Rows[i]["TimeStamp"] = System.Text.Encoding.UTF8.GetString(_dtTrans.Rows[i]["TimeStamp"]);     // change on 23rd dec,15
    //        //            byte[] _timeStamp = (byte[])_dtTrans.Rows[i]["TimeStamp"];
    //        //            //_dt.Rows[i]["TimeStamp"] = Encoding.UTF8.GetString(_timeStamp);   // change on 23rd dec,15
    //        //            _dt.Rows[i]["TimeStamp"] = Convert.ToBase64String(_timeStamp);
    //        //                                                                                                               // set timestamp into hiddenfield
    //        //        }
    //        //    }
    //        //}
           
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }

    //}
    #endregion
  
    private void UpdateJournal(List<Transaction> _lstTrans)
    {
        try
        {
            #region Update GLA
            _objJournal.ConnConfig = Session["config"].ToString();
            _objJournal.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _objJournal.GLDate = Convert.ToDateTime(txtTransDate.Text);
            _objJournal.Internal = txtEntryNo.Text;
            _objJournal.GLDesc = txtDescription.Text;
            _objJournal.IsRecurring = Convert.ToBoolean(hdnIsRecurr.Value);
            if (_objJournal.IsRecurring) // Update Recurring Entry
            {
                _objJournal.Frequency = Convert.ToInt32(ddlFrequency.SelectedValue);
                _objBLGLARecur.UpdateRecur(_objJournal);
            }
            else                        // Update Journal Entry
            {
                _objJournal.BatchID = Convert.ToInt32(hdnBatchID.Value);
                _objBLJournal.UpdateGLA(_objJournal);
            }
            #endregion

            #region Delete Transactions
            List<string> _lstDeletedTrans = (List<string>)ViewState["DeletedTrans"];
            DataTable dt = (DataTable)ViewState["Transactions"];

            foreach (var id in _lstDeletedTrans)
            {
                _objTrans.ConnConfig = Session["config"].ToString();
                _objTrans.ID = Convert.ToInt32(id);
                if (_objJournal.IsRecurring) // Delete Recurring transaction Entry
                {
                    _objBLGLARecur.DeleteRecurTransByID(_objTrans);
                }
                else                         // Delete existing transaction entry
                {
                    DataSet _dsTr = _objBLJournal.GetTransByID(_objTrans);
                
                    #region Delete Transaction balance From Chart
                                                                                    // added by Mayuri 10th dec, 15
                    foreach (DataRow _dr in _dsTr.Tables[0].Rows)
                    {
                         double _amount = Convert.ToDouble(_dr["Amount"]);

                         _objChart.ConnConfig = Session["config"].ToString();
                         _objChart.ID = Convert.ToInt32(_dr["Acct"]);
                         _amount = _amount * -1;

                         _objChart.Amount = _amount;
                        //_objBLChart.UpdateChartBalance(_objChart);

                        if(Convert.ToInt32(_dr["Type"]).Equals(30))     //delete balance from bank table
                        {
                            _objChart.ConnConfig = Session["config"].ToString();
                            _objChart.ID = Convert.ToInt32(_dr["Acct"]);

                            _objChart.Amount = _amount;
                            _objBLChart.UpdateBankBalance(_objChart);
                        }
                    }

                    #endregion
                
                    _objBLJournal.DeleteTransByID(_objTrans);

                }
            }
            #endregion

            #region Add/Update Transaction
            int i = 0;
            bool _isBankJournal = CheckIsBankJournal(_lstTrans); // Check is this bank journal adjustment or journal entry.
            bool _isAcct = false;
            foreach (var _lst in _lstTrans)
            {
                _objTrans = new Transaction();
       
                //if (!string.IsNullOrEmpty(dt.Rows[i]["ID"].ToString()))

                    if (!_lst.ID.Equals(0))
                    {
                        //if(_lst.TimeStamp.Equals(_obj))
                        #region Update transaction
                        #region Bank Adj
                        if (_isBankJournal)                     // If Journal entry is Bank adjustment then Type is 30, 31
                        {
                            _objChart.ID = _lst.Acct;
                            _isAcct = _objBLChart.IsChartBankAcct(_objChart);
                            if (_isAcct)     //If it is Bank account then it is type = 30
                            {
                                _objTrans.Type = 30;
                            }
                            else
                            {
                                _objTrans.Type = 31;
                            }
                        }
                        #endregion

                        _objTrans.ConnConfig = Session["config"].ToString();

                        _objTrans.ID = _lst.ID;
                        _objTrans.Acct = _lst.Acct;
                        _objTrans.TransDate = _objJournal.GLDate;

                        _objTrans.TransDescription = _lst.TransDescription;
                        _objTrans.Amount = _lst.Amount;
                        if (chkJobSpecific.Checked == true)
                        {
                            _objTrans.JobInt = _lst.JobInt;
                            _objTrans.PhaseDoub = _lst.PhaseDoub;
                        }

                        if (_objJournal.IsRecurring) // Update Recurring Entry
                        {
                            _objBLGLARecur.UpdateRecurTrans(_objTrans);
                        }
                        else                        // Update Journal Entry
                        {
                            Transaction _objTr = new Transaction();
                            _objTr.ConnConfig =  Session["config"].ToString();
                            _objTr.ID = _lst.ID;
                            _objTr.TableName = ((CommonHelper.Tables) 0).ToString();
                            _objTr.TimeStamp = _lst.TimeStamp;

                            bool _isAccess = _objBLJournal.ValidateByTimeStamp(_objTr);

                            DataSet _dsTrans = _objBLJournal.GetTransByID(_objTrans);
                            if(_isAccess || !_isAccess) //Check time stamp for each transaction
                            {
                                _objBLJournal.UpdateJournalTrans(_objTrans);
                            }
                        
                                double _previousAmount = Convert.ToDouble(_dsTrans.Tables[0].Rows[0]["Amount"]);
                                int _previousAcct = Convert.ToInt32(_dsTrans.Tables[0].Rows[0]["Acct"]);
                        
                                //#region Delete previous Transaction Balance
                                //// added by Mayuri 10th dec, 15
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = _previousAcct;
                                _previousAmount = _previousAmount * -1;

                                _objChart.Amount = _previousAmount;
                                //_objBLChart.UpdateChartBalance(_objChart);

                                //#endregion

                                //#region Update chart balance of updated transaction
                                //// added by Mayuri 10th dec, 15
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = _lst.Acct;
                                _objChart.Amount = _lst.Amount;
                                //_objBLChart.UpdateChartBalance(_objChart);

                                //#endregion

                                #region Delete previous Bank adj Balance

                                int _previousType = Convert.ToInt32(_dsTrans.Tables[0].Rows[0]["Type"]);
                                if (_previousType.Equals(30))
                                {
                                    _objChart.ConnConfig = Session["config"].ToString();
                                    _objChart.ID = _previousAcct;
                                    _objChart.Amount = _previousAmount;
                                    _objBLChart.UpdateBankBalance(_objChart);
                                }

                                #endregion

                                #region Update previous Bank adj Balance

                                if (_objTrans.Type.Equals(30))
                                {
                                    _objChart.ConnConfig = Session["config"].ToString();
                                    _objChart.ID = _lst.Acct;
                                    _objChart.Amount = _lst.Amount;
                                    _objBLChart.UpdateBankBalance(_objChart);
                                }

                                #endregion
                        
                        

                            //#region Update Chart Balance
                            //double _previousAmount = Convert.ToDouble(_dsTrans.Tables[0].Rows[0]["Amount"]);      //Commented by Mayuri 10th Dec,15
                            //if (!_previousAmount.Equals(_objTrans.Amount))
                            //{
                            //    double _diffBal = 0.00;
                            //    if (_previousAmount < 0 & _objTrans.Amount < 0)
                            //    {
                            //        _diffBal = (_previousAmount * -1) - (_objTrans.Amount * -1);
                            //        if (_diffBal > 0) _diffBal = _diffBal * -1;
                            //    }
                            //    else if (_previousAmount > 0 & _objTrans.Amount > 0)
                            //    {
                            //        _diffBal = _previousAmount - _objTrans.Amount;
                            //        if (_diffBal < 0) _diffBal = _diffBal * -1;
                            //    }
                            //    else  // In case of Amount changes to Debit to Credit OR Credit to Debit
                            //    {
                            //        if (_objTrans.Amount < 0)
                            //        {
                            //            _diffBal = _previousAmount + (_objTrans.Amount * -1);
                            //            _diffBal = _diffBal * -1;
                            //        }
                            //        else if (_previousAmount < 0)
                            //        {
                            //            _diffBal = (_previousAmount * -1) + _objTrans.Amount;
                            //        }
                            //    }

                            //    _objChart.ConnConfig = Session["config"].ToString();
                            //    _objChart.ID = _lst.Acct;
                            //    _objChart.Amount = _diffBal;
                            //    _objBLChart.UpdateChartBalance(_objChart);
                            //}
                            //#endregion
                        
                        }
                        #endregion
                    }
                    else
                    {
                        #region Add new transaction
                        _objTrans = new Transaction();
                        #region Bank Adj
                        if (_isBankJournal)                     // If Journal entry is Bank adjustment then Type is 30, 31
                        {
                            _objChart.ID = _lst.Acct;
                            _isAcct = _objBLChart.IsChartBankAcct(_objChart);
                            if (_isAcct)     //If it is Bank account then it is type = 30
                            {
                                _objTrans.Type = 30;
                            }
                            else
                            {
                                _objTrans.Type = 31;
                            }
                        }
                        #endregion

                        _objTrans.ConnConfig = Session["config"].ToString();

                        _objTrans.Ref = _objJournal.Ref;
                        _objTrans.TransDate = _objJournal.GLDate;
                        _objTrans.Line = i;
                        _objTrans.TransDescription = _lst.TransDescription;
                        _objTrans.Acct = _lst.Acct;
                        _objTrans.Amount = _lst.Amount;
                        if (chkJobSpecific.Checked == true)
                        {
                            _objTrans.JobInt = _lst.JobInt;
                            _objTrans.PhaseDoub = _lst.PhaseDoub;
                        }
                        //_objTrans.AcctSub = null;
                        //_objTrans.Status = null;
                        //_objTrans.JobInt = null;
                        //_objTrans.PhaseDoub = null;

                        if (_objJournal.IsRecurring) // Update Recurring Entry
                        {
                            _objBLGLARecur.AddRecurTrans(_objTrans);
                        }
                        else                       // Update Journal Entry
                        {
                            _objTrans.BatchID = _objJournal.BatchID;
                            _objBLJournal.AddJournalTrans(_objTrans);

                            //#region Update Chart Balance
                            _objChart.ConnConfig = Session["config"].ToString();
                            _objChart.ID = _lst.Acct;
                            _objChart.Amount = _lst.Amount;
                            //_objBLChart.UpdateChartBalance(_objChart);
                            //#endregion

                            #region Update Bank Balance
                            if (_isBankJournal) //Bank Adjustment then update bank balance
                            {
                                if (_isAcct)            //Check is current account is bank account then update it's balance
                                {
                                    _objChart.ConnConfig = Session["config"].ToString();
                                    _objChart.ID = _lst.Acct;
                                    _objChart.Amount = _lst.Amount;
                                    _objBLChart.UpdateBankBalance(_objChart);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                i++;
            }
            #endregion
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    private void InsertBankAdjDetails(Chart _objChart)
    {
        try
        {
            _objBank.ConnConfig = Session["config"].ToString();
            _objBank.Chart = _objChart.ID;
            _objBank.ID = _objBLBank.GetBankIDByChart(_objBank);

            _objTransBank.ConnConfig = Session["config"].ToString();
            _objTransBank.Bank = _objBank.ID;
            _objTransBank.Batch = _objChart.Batch;
            _objTransBank.Amount = _objChart.Amount;
            _objBLJournal.AddTransBankAdj(_objTransBank);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Set Added Data to DataTable
    private void SetGridViewData()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Transactions"];
            
            for (int i = 0; i < gvJournal.Rows.Count; i++)
            {
                HiddenField hdnAcctID = (HiddenField)gvJournal.Rows[i].Cells[1].FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)gvJournal.Rows[i].Cells[1].FindControl("txtGvAcctNo");
                TextBox txtGvAcctName = (TextBox)gvJournal.Rows[i].Cells[1].FindControl("txtGvAcctName");
                TextBox txtGvtransDes = (TextBox)gvJournal.Rows[i].Cells[2].FindControl("txtGvtransDes");
                TextBox txtGvDebit = (TextBox)gvJournal.Rows[i].Cells[3].FindControl("txtGvDebit");
                TextBox txtGvCredit = (TextBox)gvJournal.Rows[i].Cells[4].FindControl("txtGvCredit");
                Label lblTID = (Label)gvJournal.Rows[i].Cells[4].FindControl("lblTID");

                TextBox txtGvLoc = (TextBox)gvJournal.Rows[i].Cells[3].FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)gvJournal.Rows[i].Cells[3].FindControl("txtGvJob");
                HiddenField hdnJobID = (HiddenField)gvJournal.Rows[i].Cells[4].FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)gvJournal.Rows[i].Cells[4].FindControl("txtGvPhase");

                HiddenField hdnPID = (HiddenField)gvJournal.Rows[i].Cells[4].FindControl("hdnPID");

                dt.Rows[i]["AcctNo"] = txtGvAcctNo.Text;
                if (!(string.IsNullOrEmpty(hdnAcctID.Value)))
                {
                    dt.Rows[i]["AcctID"] = hdnAcctID.Value;
                }
                dt.Rows[i]["Account"] = txtGvAcctName.Text;
                dt.Rows[i]["Description"] = txtGvtransDes.Text;
                if(!(string.IsNullOrEmpty(txtGvDebit.Text)))
                {
                    dt.Rows[i]["Debit"] = txtGvDebit.Text;
                }
                if (!(string.IsNullOrEmpty(txtGvCredit.Text)))
                {
                    dt.Rows[i]["Credit"] = txtGvCredit.Text;
                }
                
                dt.Rows[i]["Loc"] = txtGvLoc.Text;
                if (!string.IsNullOrEmpty(hdnJobID.Value))
                {
                    dt.Rows[i]["JobID"] = hdnJobID.Value;
                    dt.Rows[i]["JobName"] = txtGvJob.Text;
                }
                if (!string.IsNullOrEmpty(hdnPID.Value))
                {
                    dt.Rows[i]["Phase"] = txtGvPhase.Text;
                    dt.Rows[i]["PhaseID"] = hdnPID.Value;
                }    
            }
            ViewState["Transactions"] = dt;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region Clear
    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }
    #endregion

    #region Set New GE
    private void SetNewJournalEntry()
    {
        try
        {
            SetInitialRow();
            //ScriptManager.RegisterStartupScript(this, GetType(), "ClearGridView", "ClearGridView();", true);
            _objJournal.ConnConfig = Session["config"].ToString();
            DataSet _dsTransId = new DataSet();
            _dsTransId = _objBLJournal.GetMaxTransID(_objJournal);
            hdnTransID.Value = _dsTransId.Tables[0].Rows[0]["MAXID"].ToString();
            txtEntryNo.Text = _dsTransId.Tables[0].Rows[0]["MAXID"].ToString();
            gvJournal.Columns[6].Visible = false;
            gvJournal.Columns[7].Visible = false;
            gvJournal.Columns[8].Visible = false;
            lblFrequency.Visible = false;
            ddlFrequency.Visible = false;
            //lblErrorMsg.Text = "";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                _objPropUser.PageName = "addjournalentry.aspx";
                DataSet dspage = objBLUser.getScreensByUser(_objPropUser);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }
    }
    #endregion

    //#region Fill Account Name
    //private void FillAccountName(DropDownList ddlGvAcctName)
    //{

    //    DataSet _dsAccount = new DataSet();
    //    _dsAccount = _objBLChart.GetAll(_objChart);

    //    if (_dsAccount != null)
    //    {
    //        ddlGvAcctName.Items.Clear();

    //        if (_dsAccount.Tables.Count > 0)
    //        {
    //            //ddlGvAcctName.Items.Add(new ListItem(" Select Account "));
    //            ddlGvAcctName.Items.Add(new ListItem(" "));
    //            ddlGvAcctName.Items.Add(new ListItem(" < Add New > ", "0"));
    //            ddlGvAcctName.AppendDataBoundItems = true;

    //            ddlGvAcctName.DataSource = _dsAccount;
    //            ddlGvAcctName.DataValueField = "ID";
    //            ddlGvAcctName.DataTextField = "fDesc";

    //            ddlGvAcctName.DataBind();

    //        }
    //        else
    //        {
    //            ddlGvAcctName.Items.Insert(0, new ListItem(" No Account Available ", "0"));
    //        }
    //    }
    //}
    //#endregion

}
