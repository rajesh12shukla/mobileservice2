using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class AddDeposit : System.Web.UI.Page
{
    #region Variables
    ReceivedPayment _objReceivePay = new ReceivedPayment();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    Chart _objChart = new Chart();
    BL_Chart _objBL_Chart = new BL_Chart();
    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();

    Contracts _objPropContracts = new Contracts();
    Dep _objDep = new Dep();
    PaymentDetails _objPayment = new PaymentDetails();

    Journal _objJournal = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    DepositDetails _objDepDetails = new DepositDetails();
    OpenAR _objOpenAR = new OpenAR();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    #endregion

    #region Events
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
                imgCleared.Visible = false;
                userpermissions();
                divSuccess.Visible = false;
                _objDep.ConnConfig = Session["config"].ToString();
                //DataSet _ds = _objBL_Deposit.GetAllDeposits(_objDep);
                //ViewState["Deposit"] = _ds.Tables[0];

                lblDepositTotal.Text = string.Format("{0:c}", 0);
                FillBank();

                SetDataToDepositTo();
                //DataSet _dsAcct = new DataSet();
                //_objChart.ConnConfig = Session["config"].ToString();
                //_dsAcct = _objBL_Chart.GetBankAcct(_objChart);
                //if(_dsAcct.Tables[0].Columns.Contains("ID"))
                //{
                //    txtDepositTo.Text = _dsAcct.Tables[0].Rows[0]["fDesc"].ToString();
                //    txtDepositTo.ReadOnly = true;
                //}
                if (Request.QueryString["id"] != null)   // EDIT
                {
                    lblRef.Visible = true;
                    lblRefId.Visible = true;
                    lblRefId.Text = Request.QueryString["id"];
                    lblHeader.Text = "Edit Deposit";
                    _objDep.Ref = Convert.ToInt32(Request.QueryString["id"]);
                    DataSet _dsDep = _objBL_Deposit.GetDepByID(_objDep);
                    if (_dsDep.Tables[0].Columns.Contains("Ref"))
                    {
                        SetDeposit(_dsDep.Tables[0]);
                        gvDeposit.Columns[4].Visible = false;
                        
                        if(Convert.ToInt16(_dsDep.Tables[0].Rows[0]["IsRecon"]).Equals(1))
                        {
                            imgCleared.Visible = true;
                            btnSubmit.Visible = false;
                        }
                    }
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
                }
                else
                {
                    txtDate.Text = DateTime.Now.ToShortDateString();
                    txtMemo.Text = "Deposit";
                    lblCustomerPayment.Text = "Select customer payments that you have received and list amounts to deposit below.";
                    BindReceivedPaymentGrid();
                }
                Permission();   
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void Page_PreRender(Object o, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gr in gvReceivePayment.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
                //gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvReceivePayment.ClientID + "',event);";

                //gr.Attributes["ondblclick"] = "";
                //"window.open('addreceivepayment.aspx?id=" + lblId.Text + "');";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvReceivePayment.ClientID + "');", true);
            

            foreach (GridViewRow gr in gvDeposit.Rows)
            {
                TextBox txtDescription = (TextBox)gr.FindControl("txtDescription");
                gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtDescription.ClientID + "','" + gvDeposit.ClientID + "',event);";
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;

            DataSet _dsDep = new DataSet();
            foreach(GridViewRow gr in gvReceivePayment.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                if(chkSelect.Checked.Equals(true))
                {
                    _objReceivePay.ConnConfig = Session["config"].ToString();
                    _objReceivePay.ID = Convert.ToInt32(hdnID.Value);
                    DataSet _dsRecev = new DataSet();
                    _dsRecev = _objBL_Deposit.GetReceivePaymentDetailsByID(_objReceivePay);
                    if (count > 0)
                    {
                        DataTable _dt = new DataTable();
                        _dt = _dsRecev.Tables[0];
                        _dsDep.Merge(_dsRecev, true);
                    }
                    else
                    {
                        _dsDep = _dsRecev;
                    }
                    count++;
                }
            }
            if (count > 0)
            {
                if (_dsDep.Tables[0].Columns.Contains("Loc"))
                {

                    gvDeposit.DataSource = _dsDep;
                    gvDeposit.DataBind();
                    double totalAmount = Convert.ToDouble(_dsDep.Tables[0].Compute("SUM(Amount)", string.Empty));
                    lblDepositTotal.Text = string.Format("{0:c}", totalAmount);
                }
            }
            else
            {
                gvDeposit.DataSource = null;
                gvDeposit.DataBind();
                lblDepositTotal.Text = string.Format("{0:c}", 0);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidDate())
            {
                bool _flag = false;
                if (Request.QueryString["id"] != null)   // EDIT
                {
                    _flag = (bool)ViewState["FlagPeriodClose"];
                    if (_flag)
                    {
                        #region EDIT

                        EditDepositData();
                        //Response.Redirect("managedeposit.aspx", false);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Updated Successfully! <BR/>Ref# " + Request.QueryString["id"] + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        #endregion
                    }
                }
                else //ADD
                {
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                    _flag = (bool)ViewState["FlagPeriodClose"];

                    if (_flag)
                    {
                        #region ADD

                        int depId = AddDepositData();
                        ResetForm();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + depId + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["Ref"];
            dt.PrimaryKey = keyColumns;

            if (Request.QueryString["id"] != null)
            {
                DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                int index = dt.Rows.IndexOf(d);

                if (index > 0)
                {
                    Response.Redirect("adddeposit.aspx?id=" + dt.Rows[index - 1]["Ref"]);
                }
            }
            else
            {
                DataRow d = dt.Rows.Find(dt.Rows.Count);
                int index = dt.Rows.IndexOf(d);

                Response.Redirect("adddeposit.aspx?id=" + dt.Rows[index]["Ref"]);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["Ref"];
            dt.PrimaryKey = keyColumns;

            if (Request.QueryString["id"] != null)
            {
                DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                int index = dt.Rows.IndexOf(d);
                int c = dt.Rows.Count - 1;
                if (index < c)
                {
                    Response.Redirect("adddeposit.aspx?id=" + dt.Rows[index + 1]["Ref"]);
                }
            }
            else
            {
                Response.Redirect("adddeposit.aspx");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            Response.Redirect("adddeposit.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["Ref"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            Response.Redirect("adddeposit.aspx?id=" + dt.Rows[0]["Ref"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("managedeposit.aspx");
    }
    #endregion

    #region Custom Functions
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("financeMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkFinanceMgr");
        //a.Style.Add("color", "#2382b2"); 

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkDeposit");
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
    private void SetDataToDepositTo()
    {
        try
        {
            DataSet _dsAcct = new DataSet();
            _objChart.ConnConfig = Session["config"].ToString();
            _dsAcct = _objBL_Chart.GetBankAcct(_objChart);
            if (_dsAcct.Tables[0].Columns.Contains("ID"))
            {
                txtDepositTo.Text = _dsAcct.Tables[0].Rows[0]["fDesc"].ToString();
                txtDepositTo.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void SetDeposit(DataTable dt)
    {
        try
        {
            if(dt.Rows.Count > 0)
            {
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["fDate"]).ToString("MM/dd/yyyy");
                txtMemo.Text = dt.Rows[0]["fDesc"].ToString();
                lblDepositTotal.Text = string.Format("{0:c}", Convert.ToDouble(dt.Rows[0]["Amount"].ToString()));
                ddlBank.SelectedValue = Convert.ToInt32(dt.Rows[0]["Bank"]).ToString();
            }

            _objReceivePay.ConnConfig = Session["config"].ToString();
            _objReceivePay.DepID = Convert.ToInt32(Request.QueryString["id"]);
            DataSet _dsDep = _objBL_Deposit.GetReceivedPaymentByDep(_objReceivePay);
            gvDeposit.DataSource = _dsDep;
            gvDeposit.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void BindReceivedPaymentGrid()
    {
        try 
        { 
            DataSet _ds = new DataSet();
            _objReceivePay.ConnConfig = Session["config"].ToString();
            _ds = _objBL_Deposit.GetAllReceivePaymentForDep(_objReceivePay);
            gvReceivePayment.DataSource = _ds;
            gvReceivePayment.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void FillPaymentMethod(DropDownList ddlPaymentMethod)
    {
        ddlPaymentMethod.Items.Add(new ListItem("Check", "0"));
        ddlPaymentMethod.Items.Add(new ListItem("Cash", "1"));
    }
    private void FillBank()
    {
        try
        {
            _objBank.ConnConfig = Session["config"].ToString();
            DataSet _dsBank = new DataSet();
            _dsBank = _objBL_Bank.GetAllBankNames(_objBank);

            if (_dsBank.Tables[0].Rows.Count > 0)
            {
                ddlBank.Items.Add(new ListItem(":: Select ::", "0"));
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
    private void ResetForm()
    {
        try
        {
            ResetFormControlValues(this);
            
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtMemo.Text = "Deposit";
            lblDepositTotal.Text = string.Format("{0:c}", 0);
            SetDataToDepositTo();
            BindReceivedPaymentGrid();
            gvDeposit.DataSource = null;
            gvDeposit.DataBind();
            divSuccess.Visible = false;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
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
    private bool ValidateGrid() 
    {
        bool _isValid = true;

        try
        {
            foreach(GridViewRow grDep in gvDeposit.Rows)
            {
                TextBox txtDescription = (TextBox)grDep.FindControl("txtDescription");
                if (string.IsNullOrEmpty(txtDescription.Text))
                {
                    _isValid = false;
                }
            }
            //for (int i = 0; i < gvDeposit.Rows.Count; i++)
            //{
            //    TextBox txtDescription = (TextBox)gvDeposit.Rows[i].Cells[1].FindControl("txtDescription");
            //    if(string.IsNullOrEmpty(txtDescription.Text))
            //    {
            //        _isValid = false;
            //    }
            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return _isValid;
    }
    private void EditDepositData()
    {
        try
        { 
            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _objDep.fDate = Convert.ToDateTime(txtDate.Text);
            _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objDep.fDesc = txtMemo.Text;
            
            _objDep.Amount = double.Parse(lblDepositTotal.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            _objBL_Deposit.UpdateDeposit(_objDep);
            foreach (GridViewRow di in gvDeposit.Rows)
            {
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
                _objReceivePay.ConnConfig = Session["config"].ToString();
                _objReceivePay.ID = Convert.ToInt32(hdnID.Value);
                _objReceivePay.Status = 1;
                _objBL_Deposit.UpdateReceivedPayStatus(_objReceivePay);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        } 
       
    }
    private int AddDepositData()
    {
        int dep = 0;
        try
        {
            #region add deposit
            DataSet _dsMaxTrans = new DataSet();
            _objJournal.ConnConfig = Session["config"].ToString();
            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.fDate = Convert.ToDateTime(txtDate.Text);
            _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objDep.fDesc = txtMemo.Text;
            _objDep.Amount = double.Parse(lblDepositTotal.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            _objDep.IsRecon = false;
            dep = _objBL_Deposit.AddDeposit(_objDep);
            
            #endregion

            int _batch = _objBLJournal.GetMaxTransBatch(_objJournal);

            _objChart.ConnConfig = Session["config"].ToString();
            DataSet _dsUndepAcct = new DataSet();   //undeposited fun
            _dsUndepAcct = _objBL_Chart.GetUndepositeAcct(_objChart);

            DataSet _dsBank = new DataSet();        // cash in bank
            _dsBank = _objBL_Chart.GetBankAcct(_objChart);
            int transId = 0;
            int n = 0;
            double totalAmount = 0.00;
            foreach (GridViewRow gr in gvDeposit.Rows)
            {
                //CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                //if(chkSelect.Checked.Equals(true))
                //{
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                Label lblAmount = (Label)gr.FindControl("lblAmount");
                TextBox txtDescription = (TextBox)gr.FindControl("txtDescription");

                _objDepDetails.ConnConfig = Session["config"].ToString();
                _objDepDetails.DepID = dep;
                _objDepDetails.ReceivedPaymentID = Convert.ToInt32(hdnID.Value);
                _objBL_Deposit.AddDepositDetails(_objDepDetails);

                _objReceivePay.ConnConfig = Session["config"].ToString();
                _objReceivePay.ID = _objDepDetails.ReceivedPaymentID;
                _objReceivePay.Status = 1;
                _objBL_Deposit.UpdateReceivedPayStatus(_objReceivePay);

                #region add transaction
                    
                
                if (_dsBank.Tables[0].Columns.Contains("ID"))
                {
                    _objJournal = new Journal(); //Generate Batch ID
                    _objJournal.ConnConfig = Session["config"].ToString();
                    double _payAmount = double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint);
                    totalAmount = totalAmount +_payAmount;

                    //Undeposited Funds         Credit 
                    _objTrans = new Transaction();
                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.BatchID = _batch;
                    _objTrans.Acct = Convert.ToInt32(_dsUndepAcct.Tables[0].Rows[0]["ID"]);
                    _objTrans.Type = 6;
                    _objTrans.TransDate = Convert.ToDateTime(txtDate.Text);
                    _objTrans.Amount = (_payAmount * -1);
                    _objTrans.Line = n++;
                    _objTrans.Ref = dep; //deposit ref#
                    _objTrans.TransDescription = (string.IsNullOrEmpty(txtDescription.Text)) ? "Payment" : txtDescription.Text;
                    _objTrans.Sel = 0;
                    transId = _objBLJournal.AddJournalTrans(_objTrans);
                    UpdateChartBalance();
                }
                
                #endregion

                DataSet _dsPayment = new DataSet();
                _objPayment.ConnConfig = Session["config"].ToString();
                _objPayment.ReceivedPaymentID = Convert.ToInt32(hdnID.Value);
                _dsPayment = _objBL_Deposit.GetPaymentDetailsByReceivedID(_objPayment);
                if (_dsPayment.Tables[0].Columns.Contains("ID"))
                {
                    #region display invoices
                    //int count = 0;
                    foreach (DataRow dr in _dsPayment.Tables[0].Rows)
                    {
                        DataSet _dsInvoice = new DataSet();
                    
                        _objOpenAR.ConnConfig = Session["config"].ToString();
                        _objOpenAR.Ref = dep;
                        _objOpenAR.fDesc = txtMemo.Text;
                        //_objOpenAR.TransID = Convert.ToInt32(dr["TransID"]);          //past used to add Received payment TransID
                        _objOpenAR.TransID = transId;                                   //as per ts, TransID should be of deposit transaction
                        _objOpenAR.InvoiceID = Convert.ToInt32(dr["InvoiceID"]);
                        _objOpenAR.Original = Convert.ToDouble(dr["TotalAmount"]);                                     // ts used to store amount in negative.
                        _objOpenAR.Balance = Convert.ToDouble(dr["TotalAmount"]) - Convert.ToDouble(dr["PaidAmount"]); // ts is doing this while apply payment, while in mom if payment is first received then we can show balance in deposit.
                        _objOpenAR.Selected = Convert.ToDouble(dr["PaidAmount"]);                                      // ts is storing received amount in openAR.Original
                        _objOpenAR.fDate = Convert.ToDateTime(dr["InvoiceDate"]);
                        _objOpenAR.Due = Convert.ToDateTime(dr["DueDate"]);
                        _objOpenAR.Type = 1;
                        _objOpenAR.Loc = Convert.ToInt32(dr["Loc"]);
                        _objBL_Deposit.AddOpenARDetails(_objOpenAR);
                        //count++;
                    }
                    #endregion
                }
            }
            //Cash in Bank                 Debit
            _objTrans = new Transaction();
            _objTrans.ConnConfig = Session["config"].ToString();
            _objTrans.BatchID = _batch;
            _objTrans.Acct = Convert.ToInt32(_dsBank.Tables[0].Rows[0]["ID"]);
            _objTrans.Type = 5;
            _objTrans.TransDate = Convert.ToDateTime(txtDate.Text);
            _objTrans.Amount = totalAmount; //Convert.ToDouble(lblAmount.Text);
            _objTrans.Line = n;
            _objTrans.Ref = dep; //deposit ref#
            _objTrans.TransDescription = txtMemo.Text;
            _objTrans.Sel = 0;
            _objTrans.AcctSub = Convert.ToInt32(ddlBank.SelectedValue);
            transId = _objBLJournal.AddJournalTrans(_objTrans);
            UpdateChartBalance();

            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.Ref = dep;
            _objDep.TransID = transId;
            _objBL_Deposit.UpdateDepositTrans(_objDep);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return dep;
    }
    protected void gvDeposit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                GridViewRow row = e.Row;

                //DropDownList ddlPaymentMethod = (e.Row.FindControl("ddlPaymentMethod") as DropDownList);
                //FillPaymentMethod(ddlPaymentMethod);

                //GridViewRow row = gvDeposit.Rows[index];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = chkSelect.ID;
                trigger.EventName = "CheckedChanged";
                uPnlDeposit.Triggers.Add(trigger);
                break;

        }
    }
    private void UpdateChartBalance()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.ID = _objTrans.Acct;
            _objChart.Amount = _objTrans.Amount;
            _objBL_Chart.UpdateChartBalance(_objChart);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        } 
    }
    private void GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
        }
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                _objPropUser.PageName = "adddeposit.aspx";
                DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
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
    private bool IsValidDate()
    {
        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy", 
                "MM/dd/yyyy", "M/d/yyyy", 
                "M/d/yyyy", "M/d/yyyy", 
                "M/d/yyyy", "M/d/yyyy", 
                "MM/dd/yyyy", "M/dd/yyyy"};
        var dt = DateTime.TryParseExact(txtDate.Text.ToString(), formats,
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