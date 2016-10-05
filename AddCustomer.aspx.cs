using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.Odbc;

public partial class AddCustomer : System.Web.UI.Page
{
    Loc _objLoc = new Loc();
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    General objGeneral = new General();
    BL_General objBL_General = new BL_General();

    BL_Invoice objBL_Invoice = new BL_Invoice();

    private int uniqueRowId = 1;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    #region Page events

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvContacts.Rows)
        {
            HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
            Label lblMail = (Label)gr.FindControl("lblEmail");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
      
            gr.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
            gr.Attributes["onclick"] = "SelectRowmail('" + hdnSelected.ClientID + "','" + gr.ClientID + "','" + lblMail.ClientID + "','" + chkSelect.ClientID + "','" + gvContacts.ClientID + "','" + lnkMail.ClientID + "');";

        }

        foreach (GridViewRow gr in gvOpenCalls.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblComp = (Label)gr.FindControl("lblComp");
            Label lblTicketId = (Label)gr.FindControl("lblTicketId");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "',event);";
            //gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
            gr.Attributes["ondblclick"] = "window.open('addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1','_blank');";
        }

        foreach (GridViewRow gr in gvLoc.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblloc");
            HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["ondblclick"] = "location.href='addlocation.aspx?uid=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString() + "'";
            //gr.Attributes["ondblclick"] = "document.getElementById('"+ lnkEditLoc.ClientID +"').click()";
            gr.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvLoc.ClientID + "',event);";

        }
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["ondblclick"] = "location.href='addequipment.aspx?uid=" + lblID.Text + "&page=addcustomer&cuid=" + Request.QueryString["uid"].ToString() + "'";
            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvEquip.ClientID + "',event);";

        }
        foreach (GridViewRow gr in gvInvoice.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblType = (Label)gr.FindControl("lblType");

            if (lblType.Text.Equals("AR Invoice"))
            {
                gr.Attributes["ondblclick"] = "location.href='addinvoice.aspx?uid=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString() + "'";
            }
            else
            {
                gr.Attributes["ondblclick"] = "location.href='addreceivepayment.aspx?id=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString() + "'";
            }
            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvInvoice.ClientID + "',event);";
        }

        ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvInvoice.ClientID + "');", true);

        ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvEquip.ClientID + "');", true);

        ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvLoc.ClientID + "');", true);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvOpenCalls.ClientID + "');", true);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvContacts.ClientID + "');", true);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");

        }
        uniqueRowId = 1;
        if (!IsPostBack)
        {
            GetQBInt();
            FillSpecifyLocation();
            objGeneral.ConnConfig = Session["config"].ToString();
            DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
            int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
            hdnSageIntegration.Value = intintegration.ToString();
            if (intintegration == 1)
            {
                txtCName.MaxLength = 50;
                btnSageID.Visible = true;
                txtAcctno.Visible = true;
                lblSageID.Visible = true;
                //lblSageAddress.Visible = true;
            }
            else
            {
                btnSageID.Visible = false;
                txtAcctno.Visible = false;
                lblSageID.Visible = false;
                //lblSageAddress.Visible = false;
            }

            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
            DateTime lastDay = firstDay.AddDays(DaysinMonth);

            txtInvDtFrom.Text = firstDay.ToShortDateString();
            txtInvDtTo.Text = lastDay.ToShortDateString();

            FillCustomerType();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            Session["contacttable"] = null;
            if (Request.QueryString["o"] == null)
            {
                Session["locationdataCust"] = null;
            }
            CreateTable();
            tpViewEquipment.Visible = false;
            tpViewInvoicelinks.Visible = false;
            tpViewlocations.Visible = false;
            tpViewServiceHistory.Visible = false;

            #region Getdata
            if (Request.QueryString["uid"] == null)
                btnPrint.Visible = false;
            if (Request.QueryString["uid"] != null)
            {
                FillCategory();
                GetOpenCalls();
                GetDataEquip();
                GetInvoices();
                FillDepartment();
                tpViewEquipment.Visible = true;
                tpViewInvoicelinks.Visible = true;
                tpViewlocations.Visible = true;
                tpViewServiceHistory.Visible = true;
                pnlNext.Visible = true;
                pnlDoc.Visible = true;

                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Customer";
                    //btnSageID.Visible = false;
                    //if (intintegration == 1)
                    //    txtAcctno.Enabled = false;
                }

                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.DBName = Session["dbname"].ToString();
                DataSet ds = new DataSet();
                ds = objBL_User.getCustomerByID(objPropUser);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ViewState["userid"] = ds.Tables[0].Rows[0]["userid"].ToString();
                    //ViewState["rolid"] = ds.Tables[0].Rows[0]["rolid"].ToString();
                    //ViewState["empid"] = ds.Tables[0].Rows[0]["empid"].ToString(); 

                    if (ViewState["qbint"].ToString() == "1")
                    {
                        //if (ds.Tables[0].Rows[0]["qbcustomerid"].ToString() == string.Empty)
                        //{
                            Label5.Text = Request.QueryString["uid"];
                            Label5.Visible = true;
                            Label4.Visible = true;
                    }
                    else
                    {
                        Label5.Text = string.Empty;
                        Label4.Visible = false;
                        Label5.Visible = false;
                    }
                    //}
                    txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                    txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                    txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));

                    txtAcctno.Text = ds.Tables[0].Rows[0]["ownerid"].ToString();
                    hdnAcctID.Value = ds.Tables[0].Rows[0]["ownerid"].ToString();
                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    txtCName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblCustomerName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtPassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                    ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    txtUserName.Text = ds.Tables[0].Rows[0]["flogin"].ToString();
                    txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                    txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                    ddlUserType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    ViewState["rolid"] = ds.Tables[0].Rows[0]["rol"].ToString();
                    ddlCustStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Billing"].ToString()))
                    {
                        ddlBilling.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Central"].ToString()))
                    {
                        ddlSpecifiedLocation.SelectedValue = ds.Tables[0].Rows[0]["Central"].ToString();
                    }
                    
                    chkEquipments.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["CPEquipment"]);
                    chkGrpWO.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["groupbyWO"]);
                    chkOpenTicket.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["openticket"]);

                    if (ds.Tables[0].Rows[0]["internet"].ToString() == "1")
                    {
                        chkInternet.Checked = true;
                        pnlInternet.Visible = true;
                    }
                    if (ds.Tables[0].Rows[0]["ledger"].ToString() == "1")
                    {
                        chkScheduleBrd.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["ticketd"].ToString() == "1")
                    {
                        chkMap.Checked = true;
                    }
                  
                    GetDocuments();
                }
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        gvContacts.DataSource = ds.Tables[1];
                        gvContacts.DataBind();
                        //RowSelectContact();
                        Session["contacttable"] = ds.Tables[1];
                    }
                }
                if (ds.Tables.Count > 2)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        gvLoc.DataSource = ds.Tables[2];
                        gvLoc.DataBind();
                        Session["locationdataCust"] = ds.Tables[2];
                        CalculateBalance();

                        ddllocation.DataSource = ds.Tables[2];
                        ddllocation.DataTextField = "tag";
                        ddllocation.DataValueField = "loc";
                        ddllocation.DataBind();
                        ddllocation.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
                if (Request.QueryString["tab"] != null)
                {
                    if (Request.QueryString["tab"] == "loc")
                    {
                        TabContainer1.ActiveTab = tpViewlocations;
                    }
                    else if (Request.QueryString["tab"] == "equip")
                    {
                        TabContainer1.ActiveTab = tpViewEquipment;
                    }
                    else if (Request.QueryString["tab"] == "inv")
                    {
                        TabContainer1.ActiveTab = tpViewInvoicelinks;
                    }
                }

            }
        }
            #endregion

        //FillSpecifyLocation();

        /***********GetDataProspect****************/

        FillProspect();

        /***************/

        if (Request.QueryString["o"] == null)
        {
            Permission();
        }
        else
        {
            lnkClose.Visible = false;
        }
    }
    private void GetQBInt()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);

        ViewState["qbint"] = "0";
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
                ViewState["qbint"] = "1";
        }
    }
    private void FillSpecifyLocation()
    {
        try
        {
            _objLoc.ConnConfig = Session["config"].ToString();
            DataSet _dsLocation = new DataSet();
            _dsLocation = objBL_Customer.getAllLocationOnCustomer(_objLoc, Convert.ToInt32(Request.QueryString["uid"]));
            ddlSpecifiedLocation.Items.Clear();
            if (_dsLocation.Tables[0].Rows.Count > 0)
            {
                ddlSpecifiedLocation.Items.Add(new ListItem(":: Select ::", "0"));
                ddlSpecifiedLocation.AppendDataBoundItems = true;

                ddlSpecifiedLocation.DataSource = _dsLocation;
                ddlSpecifiedLocation.DataValueField = "Loc";
                ddlSpecifiedLocation.DataTextField = "Tag";
                ddlSpecifiedLocation.DataBind();
            }
            else
            {
                ddlSpecifiedLocation.Items.Add(new ListItem("No Locations Available", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region Functions
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cstmMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkCustomersSmenu");
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
            pnlSave.Visible = false;
            lnkAddnew.Visible = false;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
            lblHeader.Text = "Customer";
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //if (Request.QueryString["user"] == null)
            //{
            Response.Redirect("home.aspx");
            //btnSubmit.Visible = false;
            //lnkAddnew.Visible = false;
            //btnEdit.Visible = false;
            //btnDelete.Visible = false;
            //lnkContactSave.Visible = false;
            //}
            //else
            //{
            //    tpViewEquipment.Visible = false;
            //    tpViewInvoicelinks.Visible = false;
            //    tpViewServiceHistory.Visible = false;
            //    pnlContactsGrid.Visible = false;
            //    pnlNext.Visible = false;
            //    Panel3.Visible = false;
            //}
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
    private void ClearControls()
    {
        //txtAddress.Text = string.Empty;        
        //txtCity.Text = string.Empty;        
        //txtPassword.Text = string.Empty;
        //ddlState.SelectedIndex = -1;        
        //txtUserName.Text = string.Empty;
        //txtZip.Text = string.Empty;        
        //chkScheduleBrd.Checked = false;
        //txtCName.Text = string.Empty;
        //chkMap.Checked = false;
        //chkInternet.Checked = false;
        //pnlInternet.Visible = false;
        //txtRemarks.Text = string.Empty;
        ResetFormControlValues(this);
        CreateTable();
        gvContacts.DataBind();
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
    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
        }
        return Value;
    }
    #endregion

    #region Customers
    private void FillCustomerType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomerType(objPropUser);
        ddlUserType.DataSource = ds.Tables[0];
        ddlUserType.DataTextField = "Type";
        ddlUserType.DataValueField = "Type";
        ddlUserType.DataBind();

        //ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("customers.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
    private void Submit()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);

        if (txtRemarks.Text.Length > 500 && intintegration == 1)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrr", "noty({text: 'Error : Remarks text can not be greater than 500 characters.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }

        try
        {
            objPropUser.FirstName = txtCName.Text;
            objPropUser.Address = txtAddress.Text;
            objPropUser.City = txtCity.Text;
            objPropUser.Password = txtPassword.Text.Trim();
            objPropUser.State = ddlState.SelectedValue;
            objPropUser.Username = txtUserName.Text.Trim();
            objPropUser.Zip = txtZip.Text;
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.MainContact = txtMaincontact.Text;
            objPropUser.Phone = txtPhoneCust.Text;
            objPropUser.Website = txtWebsite.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Cell = txtCell.Text;
            objPropUser.Type = ddlUserType.SelectedValue;
            objPropUser.Status = Convert.ToInt16(ddlCustStatus.SelectedValue);
            objPropUser.AccountNo = txtAcctno.Text.Trim();
            if ((ddlSpecifiedLocation.Items.Count - 2) > 0)
            {
                objPropUser.Billing = Convert.ToInt16(ddlBilling.SelectedValue);
                objPropUser.Central = Convert.ToInt32(ddlSpecifiedLocation.SelectedValue);
            }
            else {
                objPropUser.Billing = 0;
            }

            if (chkScheduleBrd.Checked == true)
            {
                objPropUser.Schedule = 1;
            }
            else
            {
                objPropUser.Schedule = 0;
            }

            if (chkMap.Checked == true)
            {
                objPropUser.Mapping = 1;
            }
            else
            {
                objPropUser.Mapping = 0;
            }

            if (chkInternet.Checked == true)
            {
                objPropUser.Internet = 1;
            }
            else
            {
                objPropUser.Internet = 0;
            }

            objPropUser.EquipID = Convert.ToInt16(chkEquipments.Checked);
            objPropUser.grpbyWO = Convert.ToInt16(chkGrpWO.Checked);
            objPropUser.openticket = Convert.ToInt16(chkOpenTicket.Checked);

            if (Session["contacttable"] != null)
            {
                objPropUser.ContactData = (DataTable)Session["contacttable"];
            }

            if (Session["MSM"].ToString() == "TS")
                objPropUser.IsTSDatabase = 1;
            else
                objPropUser.IsTSDatabase = 0;

            if (ddlBilling.SelectedValue == "1")
            {
                var countSpecifiedLocationItems = ddlSpecifiedLocation.Items.Count - 1;
                if (countSpecifiedLocationItems == 0)
                {
                    // ClientScript.RegisterStartupScript(Page.GetType(),"ValidateSpecifyLocation","ValidateSpecifyLocation()", true);
                    divLabelMessage.Visible = true;
                     return;
                }
               
            }
            if(!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objPropUser.BillRate = Convert.ToDouble(txtBillRate.Text);
            }
            if (!string.IsNullOrEmpty(txtOt.Text))
            {
                objPropUser.RateOT = Convert.ToDouble(txtOt.Text);
            }
            if (!string.IsNullOrEmpty(txtNt.Text))
            {
                objPropUser.RateNT = Convert.ToDouble(txtNt.Text);
            }
            if (!string.IsNullOrEmpty(txtDt.Text))
            {
                objPropUser.RateDT = Convert.ToDouble(txtDt.Text);
            }
            if(!string.IsNullOrEmpty(txtTravel.Text))
            {
                objPropUser.RateTravel = Convert.ToDouble(txtTravel.Text);
            }
            if(!string.IsNullOrEmpty(txtMileage.Text))
            {
                objPropUser.MileageRate = Convert.ToDouble(txtMileage.Text);
            }
            objPropUser.dtDocs = SaveDocInfo();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                if (hdnAcctID.Value.Trim() != txtAcctno.Text.Trim())
                {
                    if (SageAlert() == 1)
                    {
                        return;
                    }
                }
                objBL_User.UpdateCustomer(objPropUser);
                hdnAcctID.Value = txtAcctno.Text;
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Customer updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                if (SageAlert() == 1)
                {
                    return;
                }
                objBL_User.AddCustomer(objPropUser);

                ConvertProspectWizard(objPropUser.CustomerID);

                ViewState["mode"] = 0;
                ViewState["custid"] = objPropUser.CustomerID;
                Session["custidloc"] = objPropUser.CustomerID;

                ClearControls();

                if (Request.QueryString["cpw"] == null)
                {
                    if (Request.QueryString["o"] == null)
                    {
                        this.programmaticModalPopup.Show();
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Customer added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
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

    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal",typeof(int));
        dt.Columns.Add("Remarks", typeof(string));

        foreach(GridViewRow gr in gvDocuments.Rows)
        {
            Label lblID= (Label)gr.FindControl("lblID");
            CheckBox chkPortal = (CheckBox)gr.FindControl("chkPortal");
            TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
            
            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = chkPortal.Checked;
            dr["Remarks"] = txtRemarks.Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    private void ConvertProspectWizard(int custID)
    {
        if (Request.QueryString["cpw"] != null)
        {
            string ProspectID = Request.QueryString["prospectid"].ToString();
            if (Request.QueryString["opid"] != null)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "&opid=" + Request.QueryString["opid"].ToString() + "';", true);
            else if (Request.QueryString["ticketid"] != null)
            {
                string ticketid = Request.QueryString["ticketid"].ToString();
                string comp = Request.QueryString["comp"].ToString();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "&Ticketid=" + ticketid + "&comp=" + comp + "';", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "';", true);
        }
    }
    protected void chkInternet_CheckedChanged(object sender, EventArgs e)
    {
        if (chkInternet.Checked == true)
        {
            pnlInternet.Visible = true;
        }
        else
        {
            pnlInternet.Visible = false;
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvContacts.Rows)
        {
            DataTable dt = (DataTable)Session["contacttable"];
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

            if (hdnSelected.Value == "1")
            {
                TogglePopup();

                DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

                txtContcName.Text = dr["Name"].ToString();
                txtContPhone.Text = dr["Phone"].ToString();
                txtContFax.Text = dr["Fax"].ToString();
                txtContCell.Text = dr["Cell"].ToString();
                txtContEmail.Text = dr["Email"].ToString();
                ViewState["editcon"] = 1;
                ViewState["index"] = lblindex.Text;
            }
        }
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        TogglePopup();

        DataTable dt = (DataTable)Session["contacttable"];

        gvContacts.DataSource = dt;
        gvContacts.DataBind();
        //RowSelectContact();
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        TogglePopup();
    }
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvOpenCalls.Rows)
        {
            CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            Label lblComp = (Label)di.FindControl("lblComp");
            Label lblTicketId = (Label)di.Cells[1].FindControl("lblTicketId");

            if (chkSelect.Checked == true)
            {
                iframeCustomer.Attributes["src"] = "Printticket.aspx?id=" + lblTicketId.Text + "&c=" + lblComp.Text + "&cl=0";
                //iframeCustomer.Attributes["width"] = "700px";  
                ModalPopupExtender1.Show();
                return;
            }
            else
            {
                //iframeCustomer.Attributes["src"] = "PrintticketCustomer.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text + "";
                //iframeCustomer.Attributes["width"] = "1024px";
                //ModalPopupExtender1.Show();
            }
        }
        Response.Redirect("PrintticketCustomer.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text);

    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttable"];

        DataRow dr = dt.NewRow();

        dr["Name"] = Truncate(txtContcName.Text, 50);
        dr["Phone"] = Truncate(txtContPhone.Text, 22);
        dr["Fax"] = Truncate(txtContFax.Text, 22);
        dr["Cell"] = Truncate(txtContCell.Text, 22);
        dr["Email"] = Truncate(txtContEmail.Text, 50);

        if (ViewState["editcon"].ToString() == "1")
        {
            dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            ViewState["editcon"] = 0;
        }
        else
        {
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        Session["contacttable"] = dt;

        gvContacts.DataSource = dt;
        gvContacts.DataBind();
        //RowSelectContact();

        ClearContact();
        TogglePopup();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
        }
    }
    private void SubmitContact()
    {
        try
        {
            if (Session["contacttable"] != null)
            {
                objPropUser.ContactData = (DataTable)Session["contacttable"];
            }

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.RolId = Convert.ToInt32(ViewState["rolid"].ToString());
                objPropUser.ConnConfig = Session["config"].ToString();
                objBL_User.UpdateCustomerContact(objPropUser);
                //lblMsg.Text = "Customer updated successfully.";                             
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    private void TogglePopup()
    {
        if (pnlOverlay.Visible == false)
        {
            pnlOverlay.Visible = true;
            pnlContact.Visible = true;
        }
        else
        {
            pnlOverlay.Visible = false;
            pnlContact.Visible = false;
        }
    }
    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ContactID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        //dt.Columns.Add("ShutdownAlert", typeof(bool));
        Session["contacttable"] = dt;
    }
    private void ClearContact()
    {
        txtContcName.Text = string.Empty;
        txtContPhone.Text = string.Empty;
        txtContFax.Text = string.Empty;
        txtContCell.Text = string.Empty;
        txtContEmail.Text = string.Empty;
    }
    protected void gvContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    HiddenField hdnSelected = (HiddenField)e.Row.FindControl("hdnSelected");
        //    Label lblMail = (Label)e.Row.FindControl("lblEmail");
        //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");

        //    e.Row.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
        //    e.Row.Attributes["onclick"] = "SelectRowmail('" + hdnSelected.ClientID + "','" + e.Row.ClientID + "','" + lblMail.ClientID + "','" + chkSelect.ClientID + "','" + gvContacts.ClientID + "','" + lnkMail.ClientID + "');";

        //}
    }
    private void RowSelectContact()
    {
        foreach (GridViewRow gr in gvContacts.Rows)
        {
            HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
            Label lblMail = (Label)gr.FindControl("lblEmail");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
            gr.Attributes["onclick"] = "SelectRowmail('" + hdnSelected.ClientID + "','" + gr.ClientID + "','" + lblMail.ClientID + "','" + chkSelect.ClientID + "','" + gvContacts.ClientID + "','" + lnkMail.ClientID + "');";

        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvContacts.ClientID + "');", true);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttable"];

        foreach (GridViewRow di in gvContacts.Rows)
        {
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

            if (hdnSelected.Value == "1")
            {
                dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));

            }
        }
        dt.AcceptChanges();
        Session["contacttable"] = dt;
        gvContacts.DataSource = dt;
        gvContacts.DataBind();
        //RowSelectContact();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
        }
    }
    protected void gvContacts_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.ID = uniqueRowId.ToString();
        ++uniqueRowId;
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[index + 1]["id"]);
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[index - 1]["id"]);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"]);
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[0]["id"]);
    }

    #endregion

    #region Locations

    private void FillLoc()
    {
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.getCustomerByID(objPropUser);

        if (ds.Tables.Count > 2)
        {
            if (ds.Tables[2].Rows.Count > 0)
            {
                gvLoc.DataSource = ds.Tables[2];
                gvLoc.DataBind();
                Session["locationdataCust"] = ds.Tables[2];
                CalculateBalance();
            }
        }
    }
    protected void lnkAddLoc_Click(object sender, EventArgs e)
    {
        Response.Redirect("addlocation.aspx?page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkEditLoc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvLoc.Rows)
        {
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblUserID = (Label)di.Cells[1].FindControl("lblloc");

            if (hdnSelected.Value == "1")
            {
                Response.Redirect("addlocation.aspx?uid=" + lblUserID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkCopyloc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvLoc.Rows)
        {
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblUserID = (Label)di.Cells[1].FindControl("lblloc");

            if (hdnSelected.Value == "1")
            {
                Response.Redirect("addlocation.aspx?uid=" + lblUserID.Text + "&t=c&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkDeleteLoc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvLoc.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.Cells[1].FindControl("lblloc");

            if (chkSelected.Checked == true)
            {
                DeleteLocation(Convert.ToInt32(lblUserID.Text));
            }
        }
    }
    private void DeleteLocation(int LocID)
    {
        objPropUser.LocID = LocID;
        objPropUser.ConnConfig = Session["config"].ToString();

        try
        {
            objBL_User.DeleteLocation(objPropUser);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Location deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            FillLoc();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    private void FillGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortData();
        gvLoc.DataSource = dt;
        gvLoc.DataBind();
        CalculateBalance();
    }

    protected void gvLoc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Paginate(sender, e);
    }
    protected void gvLoc_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string gvrow = ((GridView)sender).DataKeys[e.Row.RowIndex].Value.ToString();
            //HiddenField hdnSelected = (HiddenField)e.Row.FindControl("hdnSelected");
            //CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");

            e.Row.Attributes["ondblclick"] = "location.href='addlocation.aspx?uid=" + gvrow + "'";
            //e.Row.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvLoc.ClientID + "');";

        }
    }
    protected void gvLoc_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvLoc.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvLoc.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvLoc.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvLoc.PageCount.ToString();
    }
    protected void gvLoc_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }
    protected void Paginate(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvLoc.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvLoc.PageIndex = 0;
                break;
            case "prev":
                gvLoc.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvLoc.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvLoc.PageIndex = gvLoc.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridPaged();
    }
    private void CalculateBalance()
    {
        DataTable dt = (DataTable)Session["locationdataCust"];
        double dblBalTotal = 0;
        int equip = 0;

        foreach (DataRow dr in dt.Rows)
        {
            if (dr["balance"].ToString() != string.Empty)
            {
                dblBalTotal += Convert.ToDouble(dr["balance"].ToString());
            }
            if (dr["Elevs"].ToString() != string.Empty)
            {
                equip += Convert.ToInt32(dr["Elevs"].ToString());
            }
        }

        Label lblBalanceTotal = (Label)gvLoc.FooterRow.FindControl("lblBalanceTotal");
        Label lblEquipTotal = (Label)gvLoc.FooterRow.FindControl("lblequipTotal");

        lblBalanceTotal.Text = dblBalTotal.ToString();
        lblEquipTotal.Text = equip.ToString();
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortData();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        gvLoc.DataSource = dv.ToTable();
        gvLoc.DataBind();
        CalculateBalance();
    }
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["locationdataCust"];
        return dt;
    }
    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvLoc.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvLoc.PageIndex = ddlPages.SelectedIndex;

        // a method to populate your grid
        FillGridPaged();
    }

    #endregion

    #region Equipment

    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        //objPropUser.SearchBy = "e.owner";
        //objPropUser.SearchValue = Request.QueryString["uid"].ToString();
        objPropUser.InstallDate = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.Status = -1;

        ds = objBL_User.getElev(objPropUser);
        BindGridDatatable(ds.Tables[0]);
    }

    protected void lnkAddEquip_Click(object sender, EventArgs e)
    {
        Response.Redirect("addequipment.aspx?page=addcustomer&cuid=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkEditEquip_Click(object sender, EventArgs e)
    {

        foreach (GridViewRow di in gvEquip.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&page=addcustomer&cuid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkcopyEquip_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvEquip.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&t=c&page=addcustomer&cuid=" + Request.QueryString["uid"].ToString());
            }
        }

    }
    protected void lnkDeleteEquip_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvEquip.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                DeleteEquipment(Convert.ToInt32(lblUserID.Text));
            }
        }
    }
    private void DeleteEquipment(int EquipID)
    {
        objPropUser.EquipID = EquipID;
        objPropUser.ConnConfig = Session["config"].ToString();

        try
        {
            objBL_User.DeleteEquipment(objPropUser);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Equipment deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetDataEquip();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void BindGridDatatable(DataTable dt)
    {
        Session["ElevSrchCust"] = dt;
        gvEquip.DataSource = dt;
        gvEquip.DataBind();
        CalculateTotal(dt);
    }
    private void CalculateTotal(DataTable dt)
    {
        double total = 0.00;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["price"] != DBNull.Value)
            {
                total += Convert.ToDouble(dr["price"]);
            }
        }
        if (dt.Rows.Count > 0)
        {
            Label lblBalanceTotal = (Label)gvEquip.FooterRow.FindControl("lblTotalPrice");
            lblBalanceTotal.Text = string.Format("{0:0.00}", total);
        }
    }

    protected void gvEquip_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (EquipGridViewSortDirection == SortDirection.Ascending)
        {
            EquipGridViewSortDirection = SortDirection.Descending;
            SortEquipGridView(sortExpression, DESCENDING);
        }
        else
        {
            EquipGridViewSortDirection = SortDirection.Ascending;
            SortEquipGridView(sortExpression, ASCENDING);
        }
    }
    protected void gvEquip_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvEquip.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvEquip.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvEquip.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvEquip.PageCount.ToString();
    }
    protected void gvEquip_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateEquip(sender, e);
    }
    public SortDirection EquipGridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirectionEquip"] == null)
                ViewState["sortDirectionEquip"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirectionEquip"];
        }
        set { ViewState["sortDirectionEquip"] = value; }
    }
    private void SortEquipGridView(string sortExpression, string direction)
    {
        DataTable dt = EquipPageSortData();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindGridDatatable(dv.ToTable());
    }
    private DataTable EquipPageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["ElevSrchCust"];
        return dt;
    }
    protected void ddlPagesEquip_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvEquip.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvEquip.PageIndex = ddlPages.SelectedIndex;

        FillEquipGridPaged();
    }
    protected void PaginateEquip(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvEquip.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvEquip.PageIndex = 0;
                break;
            case "prev":
                gvEquip.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvEquip.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvEquip.PageIndex = gvEquip.PageCount;
                break;
        }

        // popultate the gridview control
        FillEquipGridPaged();
    }
    private void FillEquipGridPaged()
    {
        DataTable dt = new DataTable();

        dt = EquipPageSortData();
        BindGridDatatable(dt);
    }

    #endregion

    #region Call History

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objPropUser);
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(0, new ListItem("None", "None"));
    }
    private void GetOpenCalls()
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.CustID = Convert.ToInt32(Request.QueryString["uid"]);
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        if (txtfromDate.Text != string.Empty)
        {
            objMapData.StartDate = Convert.ToDateTime(txtfromDate.Text);
        }
        else
        {
            objMapData.StartDate = System.DateTime.MinValue;
        }

        if (txtToDate.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(txtToDate.Text);
        }
        else
        {
            objMapData.EndDate = System.DateTime.MinValue;
        }

        objMapData.SearchBy = ddlSearch.SelectedValue.Trim();

        if (ddlSearch.SelectedValue == "t.cat")
        {
            objMapData.SearchValue = ddlCategory.SelectedValue;
        }
        else
        {
            objMapData.SearchValue = txtSearch.Text;
        }
        objMapData.Department = -1;
        ds = objBL_MapData.getCallHistory(objMapData);

        FillCallHistory(ds.Tables[0]);
    }

    private void FillCallHistory(DataTable dt)
    {
        gvOpenCalls.DataSource = dt;
        gvOpenCalls.DataBind();
        Session["dtTicketListCust"] = dt;
    }
    private void FillOpencallsGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortDataOpencalls();
        FillCallHistory(dt);
    }
    protected void showModalPopupServerOperatorButton_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
    }
    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        this.ModalPopupExtender1.Hide();
        iframeCustomer.Attributes["src"] = "";
        GetOpenCalls();
    }
    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        Response.Redirect("addlocation.aspx?page=addcustomer&lid=" + ViewState["custid"].ToString());
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetOpenCalls();
    }
    protected void lnkEditTicket_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvOpenCalls.Rows)
        //{
        //    CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
        //    Label lblTicketId = (Label)di.FindControl("lblTicketId");
        //    Label lblComp = (Label)di.FindControl("lblComp");

        //    if (chkSelected.Checked == true)
        //    {
        //        Panel2.Attributes.Add("style", "display:none");
        //        //iframeCustomer.Attributes["width"] = "1330px";
        //        iframeCustomer.Attributes["src"] = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text;
        //        this.ModalPopupExtender1.Show();
        //    }
        //}
    }

    protected void gvOpenCalls_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (OpencallsGridViewSortDirection == SortDirection.Ascending)
        {
            OpencallsGridViewSortDirection = SortDirection.Descending;
            SortOpencallsGridView(sortExpression, DESCENDING);
        }
        else
        {
            OpencallsGridViewSortDirection = SortDirection.Ascending;
            SortOpencallsGridView(sortExpression, ASCENDING);
        }
    }

    private DataTable PageSortDataOpencalls()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtTicketListCust"];
        return dt;
    }

    private void SortOpencallsGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortDataOpencalls();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        FillCallHistory(dv.ToTable());
    }

    public SortDirection OpencallsGridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirectionOpencalls"] == null)
                ViewState["sortDirectionOpencalls"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirectionOpencalls"];
        }
        set { ViewState["sortDirectionOpencalls"] = value; }
    }

    protected void ddlPagesOpenCall_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvOpenCalls.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvOpenCalls.PageIndex = ddlPages.SelectedIndex;

        FillOpencallsGridPaged();
    }
    protected void gvOpenCalls_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvOpenCalls.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvOpenCalls.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvOpenCalls.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvOpenCalls.PageCount.ToString();
    }
    protected void PaginateOpencalls(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvOpenCalls.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvOpenCalls.PageIndex = 0;
                break;
            case "prev":
                gvOpenCalls.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvOpenCalls.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvOpenCalls.PageIndex = gvOpenCalls.PageCount;
                break;
        }

        // popultate the gridview control
        FillOpencallsGridPaged();
    }
    protected void gvOpenCalls_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateOpencalls(sender, e);
    }

    #endregion

    #region Invoices

    private void GetInvoices()
    {

        //DataSet ds = new DataSet();
        //objProp_Contracts.ConnConfig = Session["config"].ToString();
        //objProp_Contracts.SearchBy = "l.owner";
        //objProp_Contracts.SearchValue = Request.QueryString["uid"].ToString();

        //if (txtInvDtFrom.Text != string.Empty)
        //{
        //    objProp_Contracts.StartDate = Convert.ToDateTime(txtInvDtFrom.Text);
        //}
        //else
        //{
        //    objProp_Contracts.StartDate = System.DateTime.MinValue;
        //}

        //if (txtInvDtTo.Text != string.Empty)
        //{
        //    objProp_Contracts.EndDate = Convert.ToDateTime(txtInvDtTo.Text);
        //}
        //else
        //{
        //    objProp_Contracts.EndDate = System.DateTime.MinValue;
        //}

        //ds = objBL_Contracts.GetInvoices(objProp_Contracts);

        //BindInvoiceGridDatatable(ds.Tables[0]);


        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        objProp_Contracts.SearchBy = ddlSearchInv.SelectedValue.Trim();
        if (ddlSearchInv.SelectedValue == "i.Type")
        {
            objProp_Contracts.SearchValue = ddlDepartment.SelectedValue;
        }
        else if (ddlSearchInv.SelectedValue == "i.Status")
        {
            objProp_Contracts.SearchValue = ddlStatusInv.SelectedValue;
        }
        else if (ddlSearchInv.SelectedValue == "i.fdate")
        {
            objProp_Contracts.SearchValue = txtInvDt.Text;
        }
        else if (ddlSearchInv.SelectedValue == "l.loc")
        {
            objProp_Contracts.SearchValue = ddllocation.SelectedValue;
        }
        else
        {
            objProp_Contracts.SearchValue = txtSearchInv.Text;
        }

        if (txtInvDtFrom.Text != string.Empty)
        {
            objProp_Contracts.StartDate = Convert.ToDateTime(txtInvDtFrom.Text);
        }
        else
        {
            objProp_Contracts.StartDate = System.DateTime.MinValue;
        }

        if (txtInvDtTo.Text != string.Empty)
        {
            objProp_Contracts.EndDate = Convert.ToDateTime(txtInvDtTo.Text);
        }
        else
        {
            objProp_Contracts.EndDate = System.DateTime.MinValue;
        }

        objProp_Contracts.CustID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objProp_Contracts.Loc = 0;
        //ds = objBL_Contracts.GetInvoices(objProp_Contracts);
        ds = objBL_Invoice.GetARRevenue(objProp_Contracts);
        BindInvoiceGridDatatable(ds.Tables[0]);

    }
    protected void lnkEditInvoice_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvInvoice.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");
            Label lblType = (Label)di.FindControl("lblType");

            if (chkSelected.Checked == true)
            {
                if (lblType.Text.Equals("AR Invoice"))
                {
                    Response.Redirect("addinvoice.aspx?uid=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
                }
                else
                {
                    Response.Redirect("addreceivepayment.aspx?id=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
                }
            }
        }
    }
    protected void lnkCopyInvoice_Click(object sender, EventArgs e)
    {

    }
    protected void lnkDeleteInvoice_Click(object sender, EventArgs e)
    {

    }
    protected void lnkAddInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("addinvoice.aspx?page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetInvoices();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;
        ddlSearchInv_SelectedIndexChanged(sender, e);

        txtInvDtTo.Text = string.Empty;
        txtInvDtFrom.Text = string.Empty;
        GetInvoices();
    }
    protected void gvOpenCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            //Label lblComp = (Label)e.Row.FindControl("lblComp");
            //Label lblTicketId = (Label)e.Row.FindControl("lblTicketId");

            ////iframeCustomer.Attributes["src"] = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text;            

            //e.Row.Attributes["onclick"] = "SelectRowChk('" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "');";
            //e.Row.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
        }
    }
    private void RowSelect()
    {
        foreach (GridViewRow gr in gvOpenCalls.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblComp = (Label)gr.FindControl("lblComp");
            Label lblTicketId = (Label)gr.FindControl("lblTicketId");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "',event);";
            gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvOpenCalls.ClientID + "');", true);
    }

    private void BindInvoiceGridDatatable(DataTable dt)
    {
        Session["InvoiceSrchCust"] = dt;
        gvInvoice.DataSource = dt;
        gvInvoice.DataBind();

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

        if (dt.Rows.Count > 0)
        {
            Label lblTotalAmount = (Label)gvInvoice.FooterRow.FindControl("lblTotalAmount");
            lblTotalAmount.Text = string.Format("{0:c}", dt.Compute("sum(amount)", string.Empty));
        }
    }

    private void FillInvoiceGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortDataInvoice();
        BindInvoiceGridDatatable(dt);
    }

    private DataTable PageSortDataInvoice()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["InvoiceSrchCust"];
        return dt;
    }

    #region Paging

    protected void ddlPagesInvoice_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvInvoice.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvInvoice.PageIndex = ddlPages.SelectedIndex;

        FillInvoiceGridPaged();
    }
    protected void gvInvoice_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvInvoice.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvInvoice.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvInvoice.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvInvoice.PageCount.ToString();
    }
    protected void PaginateInvoice(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvInvoice.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvInvoice.PageIndex = 0;
                break;
            case "prev":
                gvInvoice.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvInvoice.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvInvoice.PageIndex = gvInvoice.PageCount;
                break;
        }

        // popultate the gridview control
        FillInvoiceGridPaged();
    }
    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateInvoice(sender, e);
    }

    #endregion

    #region Sorting

    protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirectionInvoice == SortDirection.Ascending)
        {
            GridViewSortDirectionInvoice = SortDirection.Descending;
            SortGridViewInvoice(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirectionInvoice = SortDirection.Ascending;
            SortGridViewInvoice(sortExpression, ASCENDING);
        }
    }
    public SortDirection GridViewSortDirectionInvoice
    {
        get
        {
            if (ViewState["sortDirectionInvoice"] == null)
                ViewState["sortDirectionInvoice"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirectionInvoice"];
        }
        set { ViewState["sortDirectionInvoice"] = value; }
    }
    private void SortGridViewInvoice(string sortExpression, string direction)
    {
        DataTable dt = PageSortDataInvoice();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindInvoiceGridDatatable(dv.ToTable());
    }

    #endregion

    #endregion

    private void FillProspect()
    {
        if (Request.QueryString["cpw"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
            DataSet ds = new DataSet();
            ds = objBL_Customer.getProspectByID(objProp_Customer);

            if (ds.Tables[0].Rows.Count > 0)
            {
                FileUpload1.Visible = false;
                lnkDeleteDoc.Visible = false;
                lnkAddnew.Visible = false;
                btnDelete.Visible = false;
                btnEdit.Visible = false;
                pnlDoc.Visible = true;
                btnSubmit.Text = "Next";
                lnkClose.Visible = false;

                txtCName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                lblCustomerName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                ddlCustStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                //ddlBilling.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                GetDocuments();

                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        gvContacts.DataSource = ds.Tables[1];
                        gvContacts.DataBind();
                    }
                }
            }
        }
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearch.SelectedValue == "t.cat")
        {
            ddlCategory.Visible = true;
            txtSearch.Visible = false;
        }
        else
        {
            ddlCategory.Visible = false;
            txtSearch.Visible = true;
        }
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;
        ddlSearchInv_SelectedIndexChanged(sender, e);
        txtInvDtTo.Text = string.Empty;
        txtInvDtFrom.Text = string.Empty;
    }
    protected void ddlSearchInv_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearchInv.SelectedValue == "i.Type")
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = true;
            txtInvDt.Visible = false;
            ddllocation.Visible = false;
        }
        else if (ddlSearchInv.SelectedValue == "i.Status")
        {
            ddlStatusInv.Visible = true;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = false;
            ddllocation.Visible = false;
        }
        else if (ddlSearchInv.SelectedValue == "i.fdate")
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = true;
            ddllocation.Visible = false;
        }
        else if (ddlSearchInv.SelectedValue == "l.loc")
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = false;
            ddllocation.Visible = true;
        }
        else
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = true;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = false;
            ddllocation.Visible = false;
        }
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new ListItem("Select", ""));
    }
    private void Locations()
    {
        DataSet ds = new DataSet();
        //objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = objBL_User.getLocationByCustomerID(objPropUser);

        ddllocation.DataSource = ds.Tables[0];
        ddllocation.DataTextField = "tag";
        ddllocation.DataValueField = "loc";
        ddllocation.DataBind();

        //ddllocation.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (FileUpload1.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
            }

            objMapData.Screen = "Customer";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            //objMapData.Subject = txtNoteSub.Text.Trim();
            //objMapData.Body = txtNoteBody.Text.Trim();
            //objMapData.Mode = Convert.ToInt16(ViewState["notesmode"]);
            //if (ViewState["notesmode"].ToString() == "1")
            //    objMapData.DocID = Convert.ToInt32(hdnNoteID.Value);
            //else
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();
            objBL_User.UpdateDocInfo(objPropUser);

            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetDocuments()
    {
        bool IsProspect = false;
        if (Request.QueryString["cpw"] != null)
            IsProspect = true;

        if (IsProspect)
        {
            objMapData.Screen = "SalesLead";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
        }
        else
        {
            objMapData.Screen = "Customer";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }

        objMapData.TempId = "0";


        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetDocuments(objMapData);
        gvDocuments.DataSource = ds.Tables[0];
        gvDocuments.DataBind();
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }
    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            }
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();
            objBL_User.UpdateDocInfo(objPropUser);

            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnSageID_Click(object sender, EventArgs e)
    {
        int i = 1;
        string str = "Customer ID Already Exists in Sage";
        if (txtAcctno.Text.Trim() != string.Empty)
        {
            if (ViewState["mode"].ToString() == "1")
            {
                if (hdnAcctID.Value.Trim() == txtAcctno.Text.Trim())
                {
                    return;
                }
            }
            try
            {
                i = CheckSageID(txtAcctno.Text.Trim());
                if (i == 0)
                {
                    str = "Account # Available!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
                }
            }
            catch (Exception ex)
            {
                string strex = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + strex + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',dismissQueue: true,    closable : true});", true);
            }
        }
    }

    private int CheckSageID(string job)
    {
        string DSN = System.Web.Configuration.WebConfigurationManager.AppSettings["SageDSN"].Trim();
        string query = "Select customer from master_arm_customer where customer = ?";
        OdbcConnection odbccon = new OdbcConnection(DSN);
        if (odbccon.State != ConnectionState.Open)
        {
            odbccon.Open();
        }
        System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(query, odbccon);
        da.SelectCommand.Parameters.AddWithValue("@customer", job);
        DataTable dt = new DataTable();
        da.Fill(dt);
        odbccon.Close();
        int count = dt.Rows.Count;
        return count;
    }
    private int SageAlert()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        int count = 0;
        if (intintegration == 1)
        {
            int i = 1;
            string str = "Customer ID Already Exists in Sage..";
            try
            {
                if (txtAcctno.Text.Trim() != string.Empty)
                    i = CheckSageID(txtAcctno.Text.Trim());

                if (i != 0)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',dismissQueue: true,  closable : true});", true);
                    count = 1;
                }
            }
            catch (Exception ex)
            {
                string strex = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + strex + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true, closable : true});", true);
                count = 1;
            }
        }
        return count;
    }

    public string ShowHoverText(object desc, object reason)
    {
        string result = string.Empty;
        result = "<B>Reason</B>: " + Convert.ToString(reason).Replace("\n", "<br/>");

        if (!string.IsNullOrEmpty(Convert.ToString(desc)))
            result += "<br/><br/><B>Resolution</B>: " + Convert.ToString(desc).Replace("\n", "<br/>");

        return result;
    }

    protected void gvOpenCalls_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            AjaxControlToolkit.HoverMenuExtender ajxHover = (AjaxControlToolkit.HoverMenuExtender)e.Row.FindControl("hmeRes");
            e.Row.ID = e.Row.RowIndex.ToString();
            ajxHover.TargetControlID = e.Row.ID;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("aragingreport.aspx?uid=" + Request.QueryString["uid"]);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}
