using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class AddCOA : System.Web.UI.Page
{
    #region "Variables"

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    AccountType _objAcType = new AccountType();
    BL_AccountType _objBLAcType = new BL_AccountType();

    Bank _objBank = new Bank();
    Rol _objRol = new Rol();
    BL_BankAccount _objBLBank = new BL_BankAccount();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    #endregion

    #region "events"

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx", false);
            }
            string _connectionString = Session["config"].ToString();

            if (!IsPostBack)
            {
                userpermissions();
                _objChart.ConnConfig = _connectionString;
                pnlBankAccount2.Visible = false;
                FillAccountType();
                FillStatus();

                FillState();        //for Bank account panel
                txtBal.ReadOnly = true;
                txtReconciled.ReadOnly = true;
                txtBal.Text = "0.00";
                txtReconciled.Text = "0.00";
                txtRate.Text = "0.00";
                pnlBankAccount.Visible = false;
                pnlBankInfo.Visible = false;
                //pnlBankAccount2.Visible = false;
                if (Request.QueryString["c"] != null)       //Copy COA
                {
                    if (Request.QueryString["id"] != null)
                    {
                        SetDataForEdit();
                        lblAcctType.Text = ddlType.SelectedItem.Text;
                        lblAcctNum.Text = txtAcctNum.Text;
                        lblAcctName.Text = txtAcName.Text;
                    }
                }
                else if (Request.QueryString["id"] != null)  //Edit COA
                {
                    SetDataForEdit();
                    lblAcctType.Text = ddlType.SelectedItem.Text;
                    lblAcctNum.Text = txtAcctNum.Text;
                    lblAcctName.Text = txtAcName.Text;
                    pnlNext.Visible = true;
                }
                else                                        //Add COA   
                {
                    //lblAddEditUser.Text = "Add New Account ";
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Add New Account ";
                    FillSubAccount();
                    //ddlSubAcCategory.Enabled = false;

                }
                txtBal.Enabled = false;
                Permission();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("financeMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkFinanceMgr");
        //a.Style.Add("color", "#2382b2"); 

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkCOA");
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
            Response.Redirect("home.aspx", false);
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx", false);
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx", false);
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

    #region Close COA
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("chartofaccount.aspx", false);
    }
    #endregion

    #region Save COA
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.Acct = txtAcctNum.Text;
                _objChart.fDesc = txtAcName.Text;
                _objChart.AcType = Convert.ToInt32(ddlType.SelectedValue);

                if (ddlSubAcCategory.SelectedValue != " Select Sub Category ")
                    _objChart.Sub = ddlSubAcCategory.SelectedItem.Text;
                else
                    _objChart.Sub = "";

                _objChart.Sub2 = "";
                _objChart.Remarks = txtDescription.Text;
                _objChart.InUse = 1;
                _objChart.Detail = 0;
                _objChart.Status = Convert.ToInt32(ddlStatus.SelectedIndex);
                //_objChart.ConnConfig = Session["config"].ToString();
                //_objChart.Balance = Convert.ToDouble(0);
                //_objRol.Name = txtAcName.Text;
                _objChart.Contact = txtContact.Text;
                _objChart.Address = txtAddress.Text;
                _objChart.Phone = txtPhone.Text;
                _objChart.Fax = txtFax.Text;
                _objChart.City = txtCity.Text;
                _objChart.Cellular = txtCellular.Text;
                //_objRol.State = txtState.Text;
                if (!ddlState.SelectedValue.Equals("Select State"))
                {
                    _objChart.State = ddlState.SelectedValue;
                }
                _objChart.Zip = txtZip.Text;
                _objChart.EMail = txtEmail.Text;
                _objChart.Country = txtCountry.Text;
                _objChart.Website = txtWebsite.Text;
                //_objChart.Type = 2; // Bank Type number

                //_objBank.fDesc = txtAcName.Text;
                _objChart.NBranch = txtBranch.Text;
                _objChart.NAcct = txtAcct.Text;
                _objChart.NRoute = txtRoute.Text;
                if (!string.IsNullOrEmpty(txtCreditLimit.Text))
                    _objChart.CLimit = Convert.ToDouble(txtCreditLimit.Text);

                if (!string.IsNullOrEmpty(txtNCheck.Text))
                    _objChart.NextC = Convert.ToInt32(txtNCheck.Text);
                if (!string.IsNullOrEmpty(txtNDeposit.Text))
                    _objChart.NextD = Convert.ToInt32(txtNDeposit.Text);
                if (!string.IsNullOrEmpty(txtNEPay.Text))
                    _objChart.NextE = Convert.ToInt32(txtNEPay.Text);
                if (!string.IsNullOrEmpty(txtRate.Text))
                    _objChart.Rate = Convert.ToDouble(txtRate.Text);
                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    _objChart.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                if (chkWarn.Checked == true)
                    _objChart.Warn = 1;
                else
                    _objChart.Warn = 0;


                if (Request.QueryString["c"] != null)   // COPY
                {
                    #region "COPY"
                    if (Request.QueryString["id"] != null)
                    {

                        _objBLChart.AddChart(_objChart);

                        Response.Redirect("chartofaccount.aspx", false);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "AddedCoa();", true);
                    }
                    #endregion
                }
                else if (Request.QueryString["id"] != null)   // EDIT
                {
                    _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
                    if (ddlType.SelectedItem.Text == "Bank")
                    {
                        if (!string.IsNullOrEmpty(hdnRol.Value))
                        {
                            _objChart.Rol = Convert.ToInt32(hdnRol.Value);
                        }
                        if (!string.IsNullOrEmpty(hdnBank.Value))
                        {
                            _objChart.Bank = Convert.ToInt32(hdnBank.Value);
                        }
                    }
                    //UpdateChart();
                    _objBLChart.UpdateChart(_objChart);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Chart of Account Updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    
                    if (ddlType.SelectedItem.Text.Equals("Bank"))
                    {
                        pnlBankAccount.Visible = true;
                        pnlBankInfo.Visible = true;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                    }
                }
                else   // ADD
                {
                    _objBLChart.AddChart(_objChart);
                    ResetFormControlValues(this);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Chart of Account Added Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                pnlBankAccount.Visible = false;
                pnlBankAccount2.Visible = false;
                pnlBankInfo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    #endregion
    private void SetBankData(DataRow _drBank, DataRow _drRol)
    {
        try
        {
            hdnRol.Value = _drRol["ID"].ToString();
            txtAcName.Text = _drRol["Name"].ToString();
            txtContact.Text = _drRol["Contact"].ToString();
            txtAddress.Text = _drRol["Address"].ToString();
            txtPhone.Text = _drRol["Phone"].ToString();
            txtFax.Text = _drRol["Fax"].ToString();
            txtCity.Text = _drRol["City"].ToString();
            txtCellular.Text = _drRol["Cellular"].ToString();
            ddlState.SelectedValue = _drRol["State"].ToString();
            txtZip.Text = _drRol["Zip"].ToString();
            txtEmail.Text = _drRol["EMail"].ToString();
            txtCountry.Text = _drRol["Country"].ToString();
            txtWebsite.Text = _drRol["Website"].ToString();

            hdnBank.Value = _drBank["ID"].ToString();
            txtAcName.Text = _drBank["fDesc"].ToString();
            txtBranch.Text = _drBank["NBranch"].ToString();
            txtAcct.Text = _drBank["NAcct"].ToString();
            txtRoute.Text = _drBank["NRoute"].ToString();
            txtCreditLimit.Text = _drBank["CLimit"].ToString();

            if (_drBank["NextC"] != null)
                txtNCheck.Text = _drBank["NextC"].ToString();
            if (_drBank["NextD"] != null)
                txtNDeposit.Text = _drBank["NextD"].ToString();
            if (_drBank["NextE"] != null)
                txtNEPay.Text = _drBank["NextE"].ToString();

            txtRate.Text = _drBank["Rate"].ToString();
            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                ddlStatus.SelectedValue = _drBank["Status"].ToString();
            if (Convert.ToInt16(_drBank["Warn"]).Equals(1))
                chkWarn.Checked = true;
            else
                chkWarn.Checked = false;
            if (Request.QueryString["c"] != null)
            {
                if (Request.QueryString["c"].ToString().Equals("1"))
                {
                    txtCreditLimit.Text = "";
                    txtNCheck.Text = "";
                    txtNDeposit.Text = "";
                    txtNEPay.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlSubAcCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubAcCategory.SelectedValue == "0")
        {
            ddlSubAcCategory.SelectedIndex = -1;
            mpeAddSubAccount.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "SetSubCategory", "SetSubCategoryData();", true);
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubAccount();
            lblAcctType.Text = ddlType.SelectedItem.Text;
            if (ddlType.SelectedItem.Text.Equals("Bank"))
            {
                pnlBankAccount.Visible = true;
                pnlBankInfo.Visible = true;
                pnlBankAccount2.Visible = true;
                chkWarn.Checked = true;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize Textbox", "reSizeTextbox();", true);

                #region "EDIT COA"
                if (Request.QueryString["id"] != null)  // In case, if bank details are previously added by user then one can easy 
                {                                       // fetch those details from database by changing his/her Account type to "Bank" again.
                    DataSet _dsIsExist = new DataSet();
                    _objBank.ConnConfig = Session["config"].ToString();
                    _objBank.Chart = Convert.ToInt32(Request.QueryString["id"]);
                    _dsIsExist = _objBLBank.IsExistBankAcct(_objBank);  //check if user have previously added Bank details.
                    int count = 0;
                    if (_dsIsExist.Tables[0].Columns.Contains("CBANK"))
                    {
                        count = Convert.ToInt32(_dsIsExist.Tables[0].Rows[0]["CBANK"]);
                        if (count > 0)
                        {
                            _objBank.ConnConfig = Session["config"].ToString();
                            _objBank.Chart = Convert.ToInt32(Request.QueryString["id"]);
                            DataSet _dsBank = new DataSet();
                            _dsBank = _objBLBank.GetBankByChart(_objBank);      // Get Bank details by Chart ID
                            DataRow _drBank = _dsBank.Tables[0].Rows[0];
                            if (_dsBank != null)
                            {
                                DataSet _dsRol = new DataSet();
                                _objRol.ConnConfig = Session["config"].ToString();
                                _objRol.ID = Convert.ToInt32(_dsBank.Tables[0].Rows[0]["Rol"]);
                                _dsRol = _objBLBank.GetRolByID(_objRol);        // Get Rol details by Rol ID
                                DataRow _drRol = _dsRol.Tables[0].Rows[0];
                                SetBankData(_drBank, _drRol);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                pnlBankAccount.Visible = false;
                pnlBankAccount2.Visible = false;
                pnlBankInfo.Visible = false;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('false');", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    #region Add Sub Account Detail

    protected void lbtnSubAcctSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            GetSubAcctData();
            _objBLAcType.AddSubAccount(_objAcType);

            FillSubAccount();
            ddlSubAcCategory.Items.FindByText(txtSubAcct.Text).Selected = true;
            txtSubAcct.Text = "";
            mpeAddSubAccount.Hide();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            //dt.PrimaryKey = keyColumns;

            DataRow d = dt.Select("ID=" + Request.QueryString["id"].ToString()).FirstOrDefault();
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addcoa.aspx?id=" + dt.Rows[index - 1]["ID"], false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            //dt.PrimaryKey = keyColumns;

            DataRow d = dt.Select("ID="+Request.QueryString["id"].ToString()).FirstOrDefault();
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addcoa.aspx?id=" + dt.Rows[index + 1]["ID"], false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Chart"];
            Response.Redirect("addcoa.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"], false);
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
            DataTable dt = (DataTable)Session["Chart"];
            Response.Redirect("addcoa.aspx?id=" + dt.Rows[0]["ID"], false);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void cvAccountNum_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            bool IsExists = false;
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.Acct = txtAcctNum.Text;

            //int _count = Convert.ToInt32(_dsIsAcctExit.Tables[0].Rows[0]["CountAcct"]);

            if (Request.QueryString["id"] != null)
            {
                _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
                IsExists = _objBLChart.IsExistAcctForEdit(_objChart);
            }
            else
            {
                IsExists = _objBLChart.IsExistAcct(_objChart);
            }
            if (IsExists)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Custom Functions
    private void FillStatus()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = _objBLChart.GetAllStatus(_objChart);
            ddlStatus.DataSource = ds;
            ddlStatus.DataBind();
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillAccountType()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            DataSet _dsType = new DataSet();
            _dsType = _objBLAcType.GetAllType(_objAcType);
            ddlType.DataSource = _dsType;
            ddlType.DataBind();
            ddlType.DataValueField = "ID";
            ddlType.DataTextField = "Type";
            ddlType.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillSubAccount()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);

            DataSet _dsSubType = new DataSet();
            _dsSubType = _objBLAcType.GetTypeByAccount(_objAcType);

            if (_dsSubType != null)
            {
                ddlSubAcCategory.Items.Clear();

                //_dsSubType.Tables[0] = _dsSubType.Tables[0].DefaultView.ToTable();
                if (_dsSubType.Tables.Count > 0)
                {
                    ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                    ddlSubAcCategory.Items.Add(new ListItem(" < Add New > ", "0"));
                    ddlSubAcCategory.AppendDataBoundItems = true;

                    ddlSubAcCategory.DataSource = _dsSubType;

                    ddlSubAcCategory.DataValueField = "ID";
                    ddlSubAcCategory.DataTextField = "SubType";

                    ddlSubAcCategory.DataBind();

                }
                else
                {
                    ddlSubAcCategory.Items.Insert(0, new ListItem(" No Sub Category Available ", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void ResetFormControlValues(Control parent)
    {
        try
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
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
  
    #region Fetch Sub Account
    private void GetSubAcctData()
    {
        try
        {
            GetMaxSortValue();
            _objAcType.ConnConfig = Session["config"].ToString();
            _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);
            _objAcType.SubType = txtSubAcct.Text;
            _objAcType.SortOrder = _objAcType.MaxSortValue + 1;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Set data for Edit
    public void SetDataForEdit()
    {
        try
        {
            if (Request.QueryString["c"] != null) //COPY
            {
                if (Request.QueryString["c"] != "1")
                {
                    lblHeader.Text = "Add Account";
                }
                else
                    lblHeader.Text = "Edit Account";
            }
            else
                lblHeader.Text = "Edit Account";

            DataSet _dsChart = new DataSet();

            _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
            _dsChart = _objBLChart.GetChart(_objChart);

            ddlType.SelectedValue = _dsChart.Tables[0].Rows[0]["Type"].ToString();
            txtAcName.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
            FillSubAccount();
            if (!string.IsNullOrEmpty(_dsChart.Tables[0].Rows[0]["Sub"].ToString()))
            {
                ddlSubAcCategory.Items.FindByText(_dsChart.Tables[0].Rows[0]["Sub"].ToString()).Selected = true;
            }

            txtDescription.Text = _dsChart.Tables[0].Rows[0]["Remarks"].ToString();
            ddlStatus.SelectedValue = _dsChart.Tables[0].Rows[0]["Status"].ToString();
            if (Request.QueryString["c"] == null)
            {
                if (Request.QueryString["c"] != "1")
                {
                    txtAcctNum.Text = _dsChart.Tables[0].Rows[0]["Acct"].ToString();
                    txtBal.Text = _dsChart.Tables[0].Rows[0]["Balance"].ToString();
                }
            }

            if (ddlType.SelectedItem.Text == "Bank")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                pnlBankAccount.Visible = true;
                pnlBankInfo.Visible = true;
                pnlBankAccount2.Visible = true;
                //_objChart.ID
                DataSet _dsBank = new DataSet();
                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.Chart = _objChart.ID;
                _dsBank = _objBLBank.GetBankByChart(_objBank);
                if(_dsBank.Tables[0].Rows.Count > 0)
                {
                    DataRow _drBank = _dsBank.Tables[0].Rows[0];
                    if (_dsBank != null)
                    {
                        DataSet _dsRol = new DataSet();
                        _objRol.ConnConfig = Session["config"].ToString();
                        _objRol.ID = Convert.ToInt32(_dsBank.Tables[0].Rows[0]["Rol"]);
                        _dsRol = _objBLBank.GetRolByID(_objRol);
                        DataRow _drRol = _dsRol.Tables[0].Rows[0];
                        SetBankData(_drBank, _drRol);
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
    private void GetMaxSortValue()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            DataSet _dsAcType = new DataSet();
            _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);
            _dsAcType = _objBLAcType.GetTypeByAccount(_objAcType);
            _objAcType.MaxSortValue = 0;
            if (_dsAcType != null)
            {
                var strr = _dsAcType.Tables[0].AsEnumerable().Max(m => m["SortOrder"]);
                _objAcType.MaxSortValue = Convert.ToInt32(strr);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillState()
    {
        try
        {

            DataSet _dsState = new DataSet();
            State _objState = new State();

            _objState.ConnConfig = Session["config"].ToString();

            _dsState = _objBLBank.GetStates(_objState);

            ddlState.Items.Add(new ListItem("Select State"));
            ddlState.AppendDataBoundItems = true;

            ddlState.DataSource = _dsState;
            ddlState.DataValueField = "Name";
            ddlState.DataTextField = "fDesc";
            ddlState.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
                _objPropUser.PageName = "addcoa.aspx";
                DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                }
                else
                {
                    Response.Redirect("home.aspx", false);
                }
            }
        }
    }

    #endregion
   
}