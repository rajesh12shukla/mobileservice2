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
using AjaxControlToolkit;
using System.IO;
using System.Data.Odbc;

public partial class AddLocation : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    BL_Invoice objBL_Invoice = new BL_Invoice();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            GetQBInt();
            objGeneral.ConnConfig = Session["config"].ToString();
            DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
            int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
            hdnSageIntegration.Value = intintegration.ToString();
            if (intintegration == 1)
            {
                btnSageID.Visible = true;
                //lblSageAddress.Visible = true;
            }
            else
            {
                btnSageID.Visible = false;
                //lblSageAddress.Visible = false;
            }
            DataSet dscstm = new DataSet();

            dscstm = GetCustomFields("Loc1");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lblCustom1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            }
            dscstm = GetCustomFields("Loc2");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lblCustom2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            }

            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
            DateTime lastDay = firstDay.AddDays(DaysinMonth);

            txtInvDtFrom.Text = firstDay.ToShortDateString();
            txtInvDtTo.Text = lastDay.ToShortDateString();

            tpHistory.Visible = false;
            tpInvoices.Visible = false;
            tpViewEquipment.Visible = false;
            tpProjects.Visible = false;
            FillTerms();
            GetCustomerAll();
            FillRoute();
            Fillterritory();
            FillLocationType();
            FillSalesTax();
            //FillWorker();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            Session["contacttableloc"] = null;
            CreateTable();
            FillCategory();
            FillDepartment();
            FillContractBill();
            HyperLink2.NavigateUrl = "addticket.aspx";
            CreateAlertTable();
            BindGrid();
            if (Request.QueryString["lid"] != null)
            {
                ddlCustomer.SelectedValue = Request.QueryString["lid"].ToString();
                txtCustomer.Text = ddlCustomer.SelectedItem.Text;
                hdnPatientId.Value = ddlCustomer.SelectedValue;
                ddlCustomer_SelectedIndexChanged(sender, e);
            }

            if (Request.QueryString["uid"] != null)
            {
                GetInvoices();
                pnlNext.Visible = true;
                tpHistory.Visible = true;
                tpInvoices.Visible = true;
                tpViewEquipment.Visible = true;
                tpProjects.Visible = true;
                pnlDoc.Visible = true;
                HyperLink2.NavigateUrl = "AddTicket.aspx?locid=" + Request.QueryString["uid"].ToString();
                lnkAddProject.NavigateUrl = "AddProject.aspx?locid=" + Request.QueryString["uid"].ToString();
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Location";
                    //btnSageID.Visible = false;
                    //if (intintegration == 1)
                    //    txtAcctno.Enabled = false;
                }

                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
                DataSet ds = new DataSet();
                ds = objBL_User.getLocationByID(objPropUser);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                    txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                    txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));

                    if (ViewState["qbint"].ToString() == "1")
                    {
                        //if (ds.Tables[0].Rows[0]["qblocid"].ToString() == string.Empty)
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
                    objPropUser.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[0]["owner"].ToString());
                    DataSet dsCust = new DataSet();
                    dsCust = objBL_User.getCustomerByID(objPropUser);

                    GetOpenCalls();
                    GetDataEquip();
                    FillProjects();
                    lblLocationName.Text = ds.Tables[0].Rows[0]["ID"].ToString();

                    txtAcctno.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                    hdnAcctID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                    txtLocName.Text = ds.Tables[0].Rows[0]["Tag"].ToString();
                    txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();

                    hdnCustomerAddress.Value = dsCust.Tables[0].Rows[0]["Address"].ToString();

                    txtCity.Text = ds.Tables[0].Rows[0]["LocCity"].ToString();

                    hdnCustomerCity.Value = dsCust.Tables[0].Rows[0]["City"].ToString();

                    ddlState.SelectedValue = ds.Tables[0].Rows[0]["locstate"].ToString();

                    hdnCustomerState.Value = dsCust.Tables[0].Rows[0]["state"].ToString();

                    txtZip.Text = ds.Tables[0].Rows[0]["locZip"].ToString();

                    hdnCustomerZipCode.Value = dsCust.Tables[0].Rows[0]["zip"].ToString();

                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["Route"].ToString();
                    ddlTerr.SelectedValue = ds.Tables[0].Rows[0]["terr"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtMaincontact.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                    txtBillAdd.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtBillCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                    ddlBillState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                    txtBillZip.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    ddlCustomer.SelectedValue = ds.Tables[0].Rows[0]["owner"].ToString();
                    txtCustomer.Text = ddlCustomer.SelectedItem.Text;
                    hdnPatientId.Value = ddlCustomer.SelectedValue;
                    //txtGoogleAddress.Text = ds.Tables[0].Rows[0]["MAPAddress"].ToString(); 
                    ViewState["rol"] = ds.Tables[0].Rows[0]["rol"].ToString();
                    lat.Value = ds.Tables[0].Rows[0]["Lat"].ToString();
                    lng.Value = ds.Tables[0].Rows[0]["Lng"].ToString();
                    txtCst1.Text = ds.Tables[0].Rows[0]["custom1"].ToString();
                    txtCst2.Text = ds.Tables[0].Rows[0]["custom2"].ToString();
                    txtEmailTo.Text = ds.Tables[0].Rows[0]["custom14"].ToString();
                    txtEmailCC.Text = ds.Tables[0].Rows[0]["custom15"].ToString();
                    txtEmailToInv.Text = ds.Tables[0].Rows[0]["custom12"].ToString();
                    txtEmailCCInv.Text = ds.Tables[0].Rows[0]["custom13"].ToString();
                    ddlLocStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    ddlSTax.SelectedValue = ds.Tables[0].Rows[0]["stax"].ToString();
                    txtCreditReason.Text = ds.Tables[0].Rows[0]["creditreason"].ToString();
                    chkDispAlert.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DispAlert"]);
                    chkCreditHold.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Credit"]);
                    ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["defaultterms"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Billing"].ToString())) //added by Mayuri 24th dec, 15
                    {
                        ddlContractBill.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                    }
                    GetDocuments();
                }
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        gvContacts.DataSource = ds.Tables[1];
                        gvContacts.DataBind();
                        Session["contacttableloc"] = ds.Tables[1];
                    }
                }
                if (Request.QueryString["tab"] != null)
                {
                    if (Request.QueryString["tab"] == "equip")
                    {
                        TabContainer1.ActiveTab = tpViewEquipment;
                    }
                    else if (Request.QueryString["tab"] == "inv")
                    {
                        TabContainer1.ActiveTab = tpInvoices;
                    }
                }
            }

            FillProspect();

        }
        if (Request.QueryString["o"] == null)
        {
            Permission();
        }
    }

    private void FillTerms()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getTerms(objPropUser);

        ddlTerms.DataSource = ds.Tables[0];
        ddlTerms.DataTextField = "name";
        ddlTerms.DataValueField = "id";
        ddlTerms.DataBind();

        ddlTerms.Items.Insert(0, new ListItem(":: Select ::", "0"));
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
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvOpenCalls.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblComp = (Label)gr.FindControl("lblComp");
            Label lblTicketId = (Label)gr.FindControl("lblTicketId");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "',event);";
            //gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
            gr.Attributes["ondblclick"] = "window.open('addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1','_blank');";
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvOpenCalls.ClientID + "');", true);

        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            gr.Attributes["ondblclick"] = "location.href='addequipment.aspx?uid=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString() + "'";
            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvEquip.ClientID + "',event);";
        }

        foreach (GridViewRow gr in gvInvoice.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            Label lblType = (Label)gr.FindControl("lblType");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            if (lblType.Text.Equals("AR Invoice"))
            {
                gr.Attributes["ondblclick"] = "location.href='addinvoice.aspx?uid=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString() + "'";
            }
            else
            {
                gr.Attributes["ondblclick"] = "location.href='addreceivepayment.aspx?id=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString() + "'";
            }
            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvInvoice.ClientID + "',event);";
        }

        ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvInvoice.ClientID + "');", true);
        ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvEquip.ClientID + "');", true);

        foreach (GridViewRow gr in gvContacts.Rows)
        {
            HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
            Label lblMail = (Label)gr.FindControl("lblEmail");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
            gr.Attributes["onclick"] = "SelectRowmail('" + hdnSelected.ClientID + "','" + gr.ClientID + "','" + lblMail.ClientID + "','" + chkSelect.ClientID + "','" + gvContacts.ClientID + "','" + lnkMail.ClientID + "');";

        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript12", "SelectedRowStyle('" + gvContacts.ClientID + "');", true);
    }

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

                lblLocationName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtAcctno.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtLocName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString(); ;
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                txtBillAdd.Text = ds.Tables[0].Rows[0]["billAddress"].ToString();
                txtBillCity.Text = ds.Tables[0].Rows[0]["billcity"].ToString();
                ddlBillState.SelectedValue = ds.Tables[0].Rows[0]["billstate"].ToString();
                txtBillZip.Text = ds.Tables[0].Rows[0]["billzip"].ToString();
                ddlCustomer.SelectedValue = Request.QueryString["customerid"].ToString();
                txtCustomer.Text = ddlCustomer.SelectedItem.Text;
                hdnPatientId.Value = ddlCustomer.SelectedValue;
                ddlLocStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
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
    private void GetCustomerAll()
    {
        DataSet ds = new DataSet();
        objPropUser.DBName = Session["dbname"].ToString();
        ds = objBL_User.getCustomers(objPropUser);

        ddlCustomer.DataSource = ds.Tables[0];
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "fDesc";
        ddlRoute.DataValueField = "id";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));

        ddlTerr.DataSource = ds.Tables[0];
        ddlTerr.DataTextField = "fDesc";
        ddlTerr.DataValueField = "id";
        ddlTerr.DataBind();

        ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "Name";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {

            if (ddlRoute.Items.Contains(new ListItem(ds.Tables[1].Rows[0][0].ToString())))
                ddlRoute.Items.FindByText(ds.Tables[1].Rows[0][0].ToString()).Selected = true;

        }

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));

    }
    private void FillLocationType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getlocationType(objPropUser);
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objPropUser);
        ddlTerr.DataSource = ds.Tables[0];
        ddlTerr.DataTextField = "Name";
        ddlTerr.DataValueField = "ID";
        ddlTerr.DataBind();

        ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillSalesTax()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getSTax(objPropUser);                   //change by dev on 14th march
        //objBL_User.getSalesTax(objPropUser);

        ddlSTax.DataSource = ds.Tables[0];
        ddlSTax.DataTextField = "Name";
        ddlSTax.DataValueField = "Name";
        ddlSTax.DataBind();
    }
    private void Permission()
    {

        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cstmMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnklocationsSmenu");
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
            lblHeader.Text = "Location";
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            btnSubmit.Visible = false;
            lnkAddnew.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            lnkContactSave.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }
    private void ClearControls()
    {
        ResetFormControlValues(this);
        lat.Value = string.Empty;
        lng.Value = string.Empty;
        CreateTable();
        gvContacts.DataBind();
    }
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);
        return ds;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=loc");
        }
        else
        {
            Response.Redirect("locations.aspx");
        }
    }
    private bool ValidateLocation()
    {
        bool _isValid = true;

        if (ddlContractBill.SelectedValue == "1")
        {
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"]);
            objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            if (!objProp_Contracts.IsExistContract)
            {
                _isValid = false;
            }
            else
                _isValid = true;
        }
        if (!_isValid)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
        }
        return _isValid;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateLocation())
        {
            Submit();
        }
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
            if (!string.IsNullOrEmpty(txtBillRate.Text))
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
            if (!string.IsNullOrEmpty(txtTravel.Text))
            {
                objPropUser.RateTravel = Convert.ToDouble(txtTravel.Text);
            }
            if (!string.IsNullOrEmpty(txtMileage.Text))
            {
                objPropUser.MileageRate = Convert.ToDouble(txtMileage.Text);
            }
            objPropUser.AccountNo = txtAcctno.Text;
            objPropUser.Locationname = txtLocName.Text;
            objPropUser.Address = txtAddress.Text;
            objPropUser.Status = Convert.ToInt16(ddlLocStatus.SelectedValue);
            objPropUser.City = txtCity.Text;
            objPropUser.State = ddlState.SelectedValue;
            objPropUser.Zip = txtZip.Text;
            if (ddlRoute.SelectedValue != string.Empty)
            {
                objPropUser.Route = Convert.ToInt32(ddlRoute.SelectedValue);
            }
            if (ddlTerr.SelectedValue != string.Empty)
            {
                objPropUser.Territory = Convert.ToInt32(ddlTerr.SelectedValue);
            }
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.MainContact = txtMaincontact.Text;
            objPropUser.Phone = txtPhoneCust.Text;
            objPropUser.Fax = txtFax.Text;
            objPropUser.Cell = txtCell.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Website = txtWebsite.Text;
            objPropUser.RolAddress = txtBillAdd.Text;
            objPropUser.RolCity = txtBillCity.Text;
            objPropUser.RolState = ddlBillState.SelectedValue;
            objPropUser.RolZip = txtBillZip.Text;
            objPropUser.Type = ddlType.SelectedValue;
            objPropUser.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
            objPropUser.MAPAddress = string.Empty;
            objPropUser.Stax = ddlSTax.SelectedValue;
            objPropUser.Lat = lat.Value.Trim();
            objPropUser.Lng = lng.Value.Trim();
            objPropUser.Custom1 = txtCst1.Text;
            objPropUser.Custom2 = txtCst2.Text;
            objPropUser.ToMail = txtEmailTo.Text.Trim();
            objPropUser.CCMail = txtEmailCC.Text.Trim();
            objPropUser.MailToInv = txtEmailToInv.Text.Trim();
            objPropUser.MailCCInv = txtEmailCCInv.Text.Trim();
            objPropUser.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            objPropUser.CreditHold = Convert.ToInt16(chkCreditHold.Checked);
            objPropUser.CreditReason = txtCreditReason.Text.Trim();
            objPropUser.TermsID = Convert.ToInt16(ddlTerms.SelectedValue);

            if (Session["contacttableloc"] != null)
            {
                objPropUser.ContactData = (DataTable)Session["contacttableloc"];
            }
            objPropUser.dtDocs = SaveDocInfo();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {

                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"]);
                objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
                if (objProp_Contracts.IsExistContract) //added by Mayuri 24th dec,15
                {
                    objPropUser.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                }
                else
                {
                    objPropUser.ContractBill = 0;
                }


                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                if (hdnAcctID.Value.Trim() != txtAcctno.Text.Trim())
                {
                    if (SageAlert() == 1)
                    {
                        return;
                    }
                }
                objBL_User.UpdateLocation(objPropUser);
                hdnAcctID.Value = txtAcctno.Text;
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Location updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                objPropUser.ContractBill = 0;
                if (Request.QueryString["prospectid"] != null)
                {
                    objPropUser.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
                }
                if (SageAlert() == 1)
                {
                    return;
                }
                objBL_User.AddLocation(objPropUser);


                ConvertProspectWizard();

                if (Request.QueryString["cpw"] == null)
                {
                    ViewState["mode"] = 0;

                    if (Request.QueryString["page"] != null)
                    {
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=loc");
                    }

                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Location added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                    ClearControls();
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
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));

        foreach (GridViewRow gr in gvDocuments.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
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
    private void ConvertProspectWizard()
    {
        if (Request.QueryString["cpw"] != null)
        {
            if (Request.QueryString["opid"] != null)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addopprt.aspx?cpw=1&uid=" + Request.QueryString["opid"].ToString() + "';", true);
            else if (Request.QueryString["ticketid"] != null)
            {
                string ticketid = Request.QueryString["ticketid"].ToString();
                string comp = Request.QueryString["comp"].ToString();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addticket.aspx?cpw=1&id=" + ticketid + "&comp=" + comp + "';", true);

            }
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addcustomer.aspx?uid=" + Request.QueryString["customerid"].ToString() + "';", true);
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

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvContacts.Rows)
        {
            DataTable dt = (DataTable)Session["contacttableloc"];
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

        DataTable dt = (DataTable)Session["contacttableloc"];

        gvContacts.DataSource = dt;
        gvContacts.DataBind();
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        TogglePopup();
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttableloc"];

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

        Session["contacttableloc"] = dt;

        gvContacts.DataSource = dt;
        gvContacts.DataBind();

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
            if (Session["contacttableloc"] != null)
            {
                objPropUser.ContactData = (DataTable)Session["contacttableloc"];
            }

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.RolId = Convert.ToInt32(ViewState["rol"].ToString());
                objPropUser.ConnConfig = Session["config"].ToString();
                objBL_User.UpdateCustomerContact(objPropUser);
                //RowSelect();
                //RowSelectContact();
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

        Session["contacttableloc"] = dt;
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
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript12", "SelectedRowStyle('" + gvContacts.ClientID + "');", true);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttableloc"];

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
        Session["contacttableloc"] = dt;
        gvContacts.DataSource = dt;
        gvContacts.DataBind();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
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

    protected void gvContacts_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //e.Row.ID = uniqueRowId.ToString();
        //++uniqueRowId;      
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locations"];
        }

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["loc"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            string url = "addlocation.aspx?uid=" + dt.Rows[index + 1]["loc"];
            if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locations"];
        }
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["loc"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            string url = "addlocation.aspx?uid=" + dt.Rows[index - 1]["loc"];
            if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url);

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locations"];
        }
        string url = "addlocation.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["loc"];
        if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
        }
        Response.Redirect(url);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locations"];
        }
        string url = "addlocation.aspx?uid=" + dt.Rows[0]["loc"];
        if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
        }
        Response.Redirect(url);

    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mode"]) != 1)
        {
            objPropUser.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
            objPropUser.DBName = Session["dbname"].ToString();
            DataSet dsCust = new DataSet();
            dsCust = objBL_User.getCustomerByID(objPropUser);

            if (dsCust.Tables[0].Rows.Count > 0)
            {
                txtAddress.Text = dsCust.Tables[0].Rows[0]["Address"].ToString();
                hdnCustomerAddress.Value = txtAddress.Text;

                txtCity.Text = dsCust.Tables[0].Rows[0]["City"].ToString();
                hdnCustomerCity.Value = txtCity.Text;

                ddlState.SelectedValue = dsCust.Tables[0].Rows[0]["State"].ToString();
                hdnCustomerState.Value = ddlState.SelectedValue;

                txtZip.Text = dsCust.Tables[0].Rows[0]["Zip"].ToString();
                hdnCustomerZipCode.Value = txtZip.Text;

                //txtRemarks.Text = dsCust.Tables[0].Rows[0]["Remarks"].ToString();
                txtMaincontact.Text = dsCust.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = dsCust.Tables[0].Rows[0]["phone"].ToString();
                txtWebsite.Text = dsCust.Tables[0].Rows[0]["website"].ToString();
                txtEmail.Text = dsCust.Tables[0].Rows[0]["email"].ToString();
                txtCell.Text = dsCust.Tables[0].Rows[0]["cellular"].ToString();
                //txtLocName.Text = txtCustomer.Text;

                if (dsCust.Tables.Count > 1)
                {
                    gvContacts.DataSource = dsCust.Tables[1];
                    gvContacts.DataBind();
                    Session["contacttableloc"] = dsCust.Tables[1];
                }
                txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["BillRate"].ToString()));
                txtOt.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateOT"].ToString()));
                txtNt.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateNT"].ToString()));
                txtDt.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateDT"].ToString()));
                txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateTravel"].ToString()));
                txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateMileage"].ToString()));
            }
        }
    }

    protected void showModalPopupServerOperatorButton_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
        iframe.Attributes["src"] = "";
        GetCustomerAll();
        if (Session["custidloc"] != null)
        {
            ddlCustomer.SelectedValue = Session["custidloc"].ToString();
            Session["custidloc"] = null;
            ddlCustomer_SelectedIndexChanged(sender, e);
            txtCustomer.Text = ddlCustomer.SelectedItem.Text;
            hdnPatientId.Value = ddlCustomer.SelectedValue;
        }
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        this.ModalPopupExtender1.Hide();
        iframeCustomer.Attributes["src"] = "";
        GetOpenCalls();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCustomers(string prefixText, int count, string contextKey)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        //objPropUser.DBName = HttpContext.Current.Session["dbname"].ToString();
        //objPropUser.SearchBy = "Name";
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getCustomerAuto(objPropUser);
        //ds = objBL_User.getCustomerSearch(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetOpenCalls();
    }

    protected void gvOpenCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
        //    Label lblComp = (Label)e.Row.FindControl("lblComp");
        //    Label lblTicketId = (Label)e.Row.FindControl("lblTicketId");

        //    e.Row.Attributes["onclick"] = "SelectRowChk('" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "');";
        //    e.Row.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
        //}
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
                //iframeCustomer.Attributes["src"] = "Printticketlocation.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"])+"&s="+ddlStatus.SelectedValue+"&sd="+txtfromDate.Text+"&ed="+txtToDate.Text+"&sn="+ddlStatus.SelectedItem.Text+"";
                //iframeCustomer.Attributes["width"] = "1024px";        
                //ModalPopupExtender1.Show();
            }
        }
        Response.Redirect("Printticketlocation.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text);
    }
    protected void lnkAddEQ_Click(object sender, EventArgs e)
    {
        string url = txtLocName.Text;
        Response.Redirect("addequipment.aspx?page=addlocation&lid=" + Request.QueryString["uid"].ToString() + "&locname=" + Server.UrlEncode(url));
    }
    protected void lnkDeleteEQ_Click(object sender, EventArgs e)
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
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccEq", "noty({text: 'Equipment deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetDataEquip();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrEq", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    protected void btnCopyEQ_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvEquip.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&t=c&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkEditEq_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvEquip.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
            }
        }
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
                    Response.Redirect("addinvoice.aspx?uid=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
                }
                else
                {
                    Response.Redirect("addreceivepayment.aspx?id=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
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
        Response.Redirect("addinvoice.aspx?page=addlocation&lid=" + Request.QueryString["uid"].ToString());
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

    #region Equipment
    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        //objPropUser.SearchBy = "e.loc";
        //objPropUser.SearchValue = Request.QueryString["uid"].ToString();
        objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objPropUser.InstallDate = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.Status = -1;

        ds = objBL_User.getElev(objPropUser);
        BindGridDatatable(ds.Tables[0]);
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
    private void BindGridDatatable(DataTable dt)
    {
        Session["ElevSrchLoc"] = dt;
        gvEquip.DataSource = dt;
        gvEquip.DataBind();
        CalculateTotal(dt);
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
        dt = (DataTable)Session["ElevSrchLoc"];
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

    private void GetOpenCalls()
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = Convert.ToInt32(Request.QueryString["uid"]);
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
        Session["dtTicketListLoc"] = dt;
    }
    private void FillOpencallsGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortDataOpencalls();
        FillCallHistory(dt);
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
        dt = (DataTable)Session["dtTicketListLoc"];
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

        //objProp_Contracts.SearchBy = "i.loc";
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
        objProp_Contracts.CustID = 0;
        objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = objBL_Invoice.GetARRevenue(objProp_Contracts);
        //ds = objBL_Contracts.GetInvoices(objProp_Contracts);
        BindInvoiceGridDatatable(ds.Tables[0]);

    }
    private void BindInvoiceGridDatatable(DataTable dt)
    {
        Session["InvoiceSrchLoc"] = dt;
        gvInvoice.DataSource = dt;
        gvInvoice.DataBind();

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

        if (dt.Rows.Count > 0)
        {
            Label lblTotalAmount = (Label)gvInvoice.FooterRow.FindControl("lblTotalAmount");
            lblTotalAmount.Text = string.Format("{0:c}", dt.Compute("sum(Amount)", string.Empty));
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
        dt = (DataTable)Session["InvoiceSrchLoc"];
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
        }
        else if (ddlSearchInv.SelectedValue == "i.Status")
        {
            ddlStatusInv.Visible = true;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = false;
        }
        else if (ddlSearchInv.SelectedValue == "i.fdate")
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = true;
        }
        else if (ddlSearchInv.SelectedValue == "l.loc")
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = false;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = false;
        }
        else
        {
            ddlStatusInv.Visible = false;
            txtSearchInv.Visible = true;
            ddlDepartment.Visible = false;
            txtInvDt.Visible = false;
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

            objMapData.Screen = "Location";
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
            objMapData.Screen = "Location";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }
        objMapData.TempId = "0";

        objMapData.Mode = 1;
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

    private int CheckSageID(string job)
    {
        string DSN = System.Web.Configuration.WebConfigurationManager.AppSettings["SageDSN"].Trim();
        string query = "Select job from master_jcm_job_1 where Job = ?";
        OdbcConnection odbccon = new OdbcConnection(DSN);
        if (odbccon.State != ConnectionState.Open)
        {
            odbccon.Open();
        }
        System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(query, odbccon);
        da.SelectCommand.Parameters.AddWithValue("@Job", job);
        DataTable dt = new DataTable();
        da.Fill(dt);
        odbccon.Close();
        int count = dt.Rows.Count;
        return count;
    }
    protected void btnSageID_Click(object sender, EventArgs e)
    {
        int i = 1;
        string str = "Account # Already Exists in Sage";
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

    private int SageAlert()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        int count = 0;
        if (intintegration == 1)
        {
            int i = 1;
            string str = "Account # Already Exists in Sage..";
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
    private void FillContractBill()
    {
        List<ContractBill> _lstBill = new List<ContractBill>();
        _lstBill = ContractBilling.GetAll();

        //ddlContractBill.Items.Add(new ListItem(":: Select ::", "Select"));
        //ddlContractBill.AppendDataBoundItems = true;

        ddlContractBill.DataSource = _lstBill;
        ddlContractBill.DataValueField = "ID";
        ddlContractBill.DataTextField = "Name";
        ddlContractBill.DataBind();
    }

    protected void ddlContractBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"]);
            objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            if (!objProp_Contracts.IsExistContract)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
            }
        }
        else
        {
            if (ddlContractBill.SelectedValue == "1")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
            }

        }

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


    private void FillProjects()
    {
        objProp_Customer.SearchBy = "j.loc";
        objProp_Customer.SearchValue = Request.QueryString["uid"].ToString();
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProject(objProp_Customer);
        gvProject.DataSource = ds.Tables[0];
        gvProject.DataBind();
        CalculateBalance(ds.Tables[0]);
    }
    private void CalculateBalance(DataTable dt)
    {
        double dblHours = 0;
        double dblBilled = 0;
        double dblTotalExp = 0;
        double dblNet = 0;
        double dblLabor = 0;
        double dblPercent = 0;
        double dblExp = 0;
        double dblMat = 0;
        double dblOrder = 0;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Hour"].ToString() != string.Empty)
                {
                    dblHours += Convert.ToDouble(dr["Hour"].ToString());
                }
                if (dr["TotalBilled"].ToString() != string.Empty)
                {
                    dblBilled += Convert.ToDouble(dr["TotalBilled"].ToString());
                }
                if (dr["TotalExp"].ToString() != string.Empty)
                {
                    dblTotalExp += Convert.ToDouble(dr["TotalExp"].ToString());
                }
                if (dr["net"].ToString() != string.Empty)
                {
                    dblNet += Convert.ToDouble(dr["net"].ToString());
                }
                if (dr["LaborExp"].ToString() != string.Empty)
                {
                    dblLabor += Convert.ToDouble(dr["LaborExp"].ToString());
                }
                if (dr["NetPercent"].ToString() != string.Empty)
                {
                    dblPercent += Convert.ToDouble(dr["NetPercent"]);
                }
                if (dr["Expenses"].ToString() != string.Empty)
                {
                    dblExp += Convert.ToDouble(dr["Expenses"]);
                }
                if (dr["MaterialExp"].ToString() != string.Empty)
                {
                    dblMat += Convert.ToDouble(dr["MaterialExp"]);
                }
                if (dr["TotalOnOrder"].ToString() != string.Empty)
                {
                    dblOrder += Convert.ToDouble(dr["TotalOnOrder"]);
                }
            }

            Label lblHourFooter = (Label)gvProject.FooterRow.FindControl("lblHourFooter");
            Label lblRevFooter = (Label)gvProject.FooterRow.FindControl("lblRevFooter");
            Label lblTotalExpFooter = (Label)gvProject.FooterRow.FindControl("lblTotalExpFooter");
            Label lblNetFooter = (Label)gvProject.FooterRow.FindControl("lblNetFooter");
            Label lblTotalLaborFooter = (Label)gvProject.FooterRow.FindControl("lblTotalLaborFooter");
            Label lblPercentFooter = (Label)gvProject.FooterRow.FindControl("lblPercentFooter");
            Label lblExpensesFooter = (Label)gvProject.FooterRow.FindControl("lblExpensesFooter");
            Label lblTotalMatFooter = (Label)gvProject.FooterRow.FindControl("lblTotalMatFooter");
            Label lblTotalOrderFooter = (Label)gvProject.FooterRow.FindControl("lblTotalOrderFooter");

            lblHourFooter.Text = string.Format("{0:n}", dblHours);
            lblRevFooter.Text = string.Format("{0:c}", dblBilled);
            lblTotalExpFooter.Text = string.Format("{0:c}", dblTotalExp);
            lblNetFooter.Text = string.Format("{0:c}", dblNet);
            //lblTotalLaborFooter.Text = string.Format("{0:c}", dblLabor);
            //lblExpensesFooter.Text = string.Format("{0:c}", dblExp);
            //lblPercentFooter.Text = string.Format("{0:n}", dblPercent);
            //lblTotalMatFooter.Text = string.Format("{0:c}", dblMat);
            //lblTotalOrderFooter.Text = string.Format("{0:c}", dblOrder);
        }
    }

    protected void lnkAddnewRow_Click(object sender, EventArgs e)
    {
        AddNewRow();
    }

    private DataTable CreateAlertTable()
    {
        Session["alerttable"] = null;

        DataTable dt = new DataTable();
        dt.Columns.Add("AlertID", typeof(int));
        dt.Columns.Add("AlertCode", typeof(string));
        dt.Columns.Add("AlertSubject", typeof(string));
        dt.Columns.Add("AlertMessage", typeof(string));

        DataRow dr = dt.NewRow();
        dr["AlertID"] = 0;
        dr["AlertCode"] = DBNull.Value;
        dr["AlertSubject"] = DBNull.Value;
        dr["AlertMessage"] = DBNull.Value;
        dt.Rows.Add(dr);

        Session["alerttable"] = dt;
        return dt;
    }
    private void AddNewRow()
    {
        GridData();

        DataTable dt = new DataTable();
        dt = (DataTable)Session["alerttable"];

        DataRow dr = dt.NewRow();
        dr["AlertID"] = 0;
        dr["AlertCode"] = DBNull.Value;
        dr["AlertSubject"] = DBNull.Value;
        dr["AlertMessage"] = DBNull.Value;
        dt.Rows.Add(dr);

        Session["alerttable"] = dt;

        BindGrid();
    }

    private void GridData()
    {
        DataTable dt = (DataTable)Session["alerttable"];

        DataTable dtDetails = dt.Clone();

        foreach (GridViewRow gr in gvAlerts.Rows)
        {
            TextBox txtCode = (TextBox)gr.FindControl("txtCode");
            TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
            TextBox lblMessage = (TextBox)gr.FindControl("lblMessage");
            Label lblID = (Label)gr.FindControl("lblID");

            DataRow dr = dtDetails.NewRow();
            dr["AlertCode"] = txtCode.Text.Trim();
            dr["AlertSubject"] = lblDesc.Text;
            dr["AlertMessage"] = lblMessage.Text;
            dr["AlertID"] = lblID.Text;
          
            dtDetails.Rows.Add(dr);
        }

        Session["alerttable"] = dtDetails;
    }

    private void BindGrid()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["alerttable"];

        gvAlerts.DataSource = dt;
        gvAlerts.DataBind();

        ((Label)gvAlerts.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(dt.Rows.Count);
    }
    protected void ibtnDeleteItem_Click(object sender, EventArgs e)
    {
        DeleteREPItem();
    }

    private void DeleteREPItem()
    {
        //GridData();

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["templtable"];

        //int count = 0;
        //foreach (GridViewRow gr in gvTemplateItems.Rows)
        //{
        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //    Label lblIndex = (Label)gr.FindControl("lblIndex");
        //    int index = Convert.ToInt32(lblIndex.Text) - 1;

        //    if (chkSelect.Checked == true)
        //    {
        //        dt.Rows.RemoveAt(index - count);
        //        count++;
        //    }
        //}

        //if (dt.Rows.Count == 0)
        //{
        //    DataRow dr = dt.NewRow();
        //    dr["Code"] = DBNull.Value;
        //    dr["EquipT"] = DBNull.Value;
        //    dr["Elev"] = 0;
        //    dr["fDesc"] = DBNull.Value;
        //    dr["Line"] = dt.Rows.Count;
        //    dr["Lastdate"] = DBNull.Value;
        //    dr["NextDateDue"] = DBNull.Value;
        //    dr["Frequency"] = -1;
        //    dt.Rows.Add(dr);
        //}

        //Session["templtable"] = dt;
        //BindGrid();
    }
}


