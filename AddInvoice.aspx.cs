using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using Microsoft.SqlServer.Management.Smo;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Globalization;

public partial class AddInvoice : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objCustomer = new Customer();

    Owner _objOwner = new Owner();

    bool success;
    protected DataTable dtBillingCodeData = new DataTable();
    protected DataTable dtProjectCodeData = new DataTable();

    GeneralFunctions objGeneral = new GeneralFunctions();

    Inv _objInv = new Inv();
    Transaction _objTrans = new Transaction();

    Journal _objJournal = new Journal();
    BL_JournalEntry _objBL_Journal = new BL_JournalEntry();

    Invoices _objInvoices = new Invoices();
    BL_Invoice objBL_Invoice = new BL_Invoice();

    Chart _objChart = new Chart();
    BL_Chart _objBL_Chart = new BL_Chart();

    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
    int _batch = 0;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //string url_path_current = HttpContext.Current.Request.Url.ToString();
        //if (url_path_current.StartsWith("https:") == true)
        //{
        //    HttpContext.Current.Response.Redirect("http" + url_path_current.Remove(0, 5), false);
        //} 
        divSuccess.Visible = false;
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        FillBillCodes();
        FillProjectCodes();
        if (!IsPostBack)
        {
            imgVoid.Visible = false;
            imgPaid.Visible = false;
            divSuccess.Visible = false;
            ddlStatus.Enabled = true;
            IntializeData();
            FillDepartment();
            FillWorker(0);
            FillTerms();
            LoadData();
            GetPeriodDetails(Convert.ToDateTime(txtInvoiceDate.Text));
            getControl();
            LoadTicketData();
        }

        if (Request.QueryString["o"] == null)
        {
            Permission();
        }

        if (!string.IsNullOrEmpty(hdnFocus.Value))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyFocus", "document.getElementById('" + hdnFocus.Value + "').focus();", true);
        }
    }

    private void IntializeData()
    {
        ViewState["mode"] = 0;
        ViewState["editcon"] = 0;
        ViewState["invoicetable"] = null;

        //DataTable dt = new DataTable();
        //dt.Columns.Add("billtype");
        //dt.Columns.Add("id");
        //DataRow dr = dt.NewRow();
        //dr["ID"] = 0;
        //dr["billtype"] = "-Select-";
        //dt.Rows.Add(dr);
        //dtProjectCodeData = dt;

        CreateTable();
        BindGrid();
        txtInvoiceDate.Text = System.DateTime.Now.ToShortDateString();
    }

    private void getControl()
    {
        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        if (dsC.Tables[0].Rows.Count > 0)
        {
            lblCompNme.Text = dsC.Tables[0].Rows[0]["Name"].ToString();
            lblCompAddress.Text = dsC.Tables[0].Rows[0]["Address"].ToString();
            lblCompCity.Text = dsC.Tables[0].Rows[0]["city"].ToString();
            lblCompState.Text = dsC.Tables[0].Rows[0]["state"].ToString();
            lblCompZip.Text = dsC.Tables[0].Rows[0]["Zip"].ToString();
            lblCompphone.Text = dsC.Tables[0].Rows[0]["phone"].ToString();
        }
    }

    private void LoadData()
    {
        if (Request.QueryString["uid"] != null)
        {
            ddlStatus.Enabled = true;
            //ddlStatus.SelectedValue = "1";
            lblInv.Visible = true;
            lblInvoiceName.Visible = true;

            pnlNext.Visible = true;
            ViewState["mode"] = 1;
            lblHeader.Text = "Edit Invoice";

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
            DataSet ds = new DataSet();
            ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblInvoiceName.Text = ds.Tables[0].Rows[0]["ref"].ToString();
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["custom1"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
                txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["billto"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                txtPO.Text = ds.Tables[0].Rows[0]["po"].ToString();
                //ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["terms"].ToString();
                hdnTaxRegion.Value = ds.Tables[0].Rows[0]["taxregion"].ToString();
                hdnProjectId.Value = ds.Tables[0].Rows[0]["job"].ToString();
                if(!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["job"].ToString()))
                {
                    ddlDepartment.Enabled = false;
                }
                FillProjectCodes();
                txtProject.Text = ds.Tables[0].Rows[0]["job"].ToString();
                txtStaxrate.Text = ds.Tables[0].Rows[0]["taxregion"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ddate"].ToString()))
                {
                    txtDueDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ddate"]).ToShortDateString();
                }
                ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                txtInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fdate"]).ToShortDateString();
                if (ds.Tables[0].Rows[0]["mech"].ToString()!= string.Empty)
                {
                    FillWorker(Convert.ToInt32(ds.Tables[0].Rows[0]["mech"].ToString()));
                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["mech"].ToString();
                }
                hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                FillLocInfo();
                ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["terms"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["taxregion"].ToString()))
                {
                    objPropUser.Stax = ds.Tables[0].Rows[0]["taxregion"].ToString();
                    DataSet dsStax = new DataSet();
                    dsStax = objBL_User.getSalesTaxByID(objPropUser);

                    if (dsStax.Tables[0].Rows.Count > 0)
                    {
                        hdnStax.Value = dsStax.Tables[0].Rows[0]["rate"].ToString();
                        txtStaxrate.Text = ds.Tables[0].Rows[0]["taxregion"].ToString() + " - " + hdnStax.Value + " %";
                    }
                }
                if (ds.Tables[0].Rows[0]["status"].ToString().Equals("2"))
                {
                    imgVoid.Visible = true;
                    btnSubmit.Visible = false;
                }
                else if(ds.Tables[0].Rows[0]["status"].ToString().Equals("1"))
                {
                    imgPaid.Visible = true;
                    btnSubmit.Visible = false;
                }

            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable _dtInvItems = ds.Tables[1];
                ViewState["invoicetable"] = _dtInvItems;
                BindGrid();
            }
            string _returnValues = calculateTotal();
            if (!string.IsNullOrEmpty(_returnValues))
            {
                var _arrreturnValues = _returnValues.Split('|');

                hdnTotalAmount.Value = _arrreturnValues[0].ToString(); // change by Mayuri 19th dec, 15
            }
        }
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("AcctMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("billingLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkInvoicesSmenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderBill");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("billMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            pnlSave.Visible = false;
            lblHeader.Text = "Invoice";
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //btnSubmit.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvInvoices.Rows)
        {
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
            TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
            TextBox lblPricePer = (TextBox)gr.FindControl("lblPricePer");
            TextBox lblSalesTax = (TextBox)gr.FindControl("lblSalesTax");
            TextBox lblDescription = (TextBox)gr.FindControl("lblDescription");
            DropDownList ddlBillingCode = (DropDownList)gr.FindControl("ddlBillingCode");
            RequiredFieldValidator rfvQuantity = (RequiredFieldValidator)gr.FindControl("rfvQuantity");
            RequiredFieldValidator rfvBillCode = (RequiredFieldValidator)gr.FindControl("rfvBillCode");
            RequiredFieldValidator rfvPricePer = (RequiredFieldValidator)gr.FindControl("rfvPricePer");

            lblQuantity.Attributes["onkeyup"] = "document.getElementById('" + hdnFocus.ClientID + "').value='" + lblQuantity.ClientID + "'; CalculateGridAmount(); CheckAddRow('" + gvInvoices.ClientID + "','" + Convert.ToInt32(lblIndex.Text) + "','" + lnkAddnew.ClientID + "'); ";
            lblPricePer.Attributes["onkeyup"] = "document.getElementById('" + hdnFocus.ClientID + "').value='" + lblPricePer.ClientID + "'; CalculateGridAmount(); CheckAddRow('" + gvInvoices.ClientID + "','" + Convert.ToInt32(lblIndex.Text) + "','" + lnkAddnew.ClientID + "'); ";
            lblSalesTax.Attributes["onkeyup"] = "document.getElementById('" + hdnFocus.ClientID + "').value='" + lblSalesTax.ClientID + "'; CalculateGridAmount(); CheckAddRow('" + gvInvoices.ClientID + "','" + Convert.ToInt32(lblIndex.Text) + "','" + lnkAddnew.ClientID + "'); ";
            chkTaxable.Attributes["onclick"] = "CalculateGridAmount();";
            ddlBillingCode.Attributes["onchange"] = "BillCodeChanged('" + ddlBillingCode.ClientID + "','" + lblDescription.ClientID + "','" + lblPricePer.ClientID + "'); CalculateGridAmount();";
            //ValidatorEnable(document.getElementById('" + rfvQuantity.ClientID + "'),true);  ValidatorEnable(document.getElementById('" + rfvBillCode.ClientID + "'),true);  ValidatorEnable(document.getElementById('" + rfvPricePer.ClientID + "'),true);
        }
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "keyCalculate", "CalculateGridAmount();", true);
        foreach (GridViewRow gr in gvProject.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblID");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectRowFill('" + gvProject.ClientID + "','" + lblID.ClientID + "','" + lblname.ClientID + "','" + hdnProjectId.ClientID + "','" + txtProject.ClientID + "','divproject');  document.getElementById('" + btnGetCode.ClientID + "').click();";
        }

    }

    private void FillWorker(int Worker)
    {
        ddlRoute.Items.Clear();
        ddlRoute.SelectedIndex = -1;
        ddlRoute.SelectedValue = null;
        ddlRoute.ClearSelection();
        DataSet ds = new DataSet();
        objPropUser.WorkId = Worker;
        objPropUser.Status = 0;
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "fDesc";
        ddlRoute.DataValueField = "id";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", "0"));
    }

    private void FillBillCodes()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getBillCodes(objPropUser);

        DataRow dr = ds.Tables[0].NewRow();
        ds.Tables[0].Rows.InsertAt(dr, 0);

        dtBillingCodeData = ds.Tables[0];

        List<Dictionary<object, object>> dictionary = new List<Dictionary<object, object>>();
        JavaScriptSerializer sr = new JavaScriptSerializer();

        if (ds.Tables[0].Rows.Count > 0)
        {
            dictionary = objGeneral.RowsToDictionary(ds.Tables[0]);

            string str = sr.Serialize(dictionary);

            hdnBillCodeJSON.Value = str;
        }

        //ddlBillingCode.DataSource = ds.Tables[0];
        //ddlBillingCode.DataTextField = "Name";
        //ddlBillingCode.DataValueField = "ID";
        //ddlBillingCode.DataBind();
    }

    private void FillProjectCodes()
    {
        if (hdnProjectId.Value != string.Empty)
        {
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(hdnProjectId.Value);
            objProp_Customer.Type = "0";
            DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].NewRow();
                //dr["ID"] = 0;
                dr["Line"] = 0;
                dr["billtype"] = "-Select-";
                ds.Tables[1].Rows.InsertAt(dr, 0);
                dtProjectCodeData = ds.Tables[1];
                if (ds.Tables[0].Rows[0]["type"].ToString() != string.Empty && ds.Tables[0].Rows[0]["type"].ToString() != "0")
                {
                    ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("billtype");
                dt.Columns.Add("Line");
                //dt.Columns.Add("ID");
                DataRow dr = dt.NewRow();
                //dr["ID"] = 0;
                dr["Line"] = 0;
                dr["billtype"] = "-Select-";
                dt.Rows.Add(dr);
                dtProjectCodeData = dt;
            }
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("billtype");
            dt.Columns.Add("Line");
            //dt.Columns.Add("ID");
            DataRow dr = dt.NewRow();
            //dr["ID"] = 0;
            dr["Line"] = 0;
            dr["billtype"] = "-Select-";
            dt.Rows.Add(dr);
            dtProjectCodeData = dt;
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

        ddlDepartment.Items.Insert(0, new ListItem(":: Select ::", ""));
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

        ddlTerms.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Ref", typeof(int));
        dt.Columns.Add("line", typeof(int));
        dt.Columns.Add("Acct", typeof(int));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobItem", typeof(int));
        dt.Columns.Add("TransID", typeof(int));
        dt.Columns.Add("Measure", typeof(string));
        dt.Columns.Add("Disc", typeof(double));
        dt.Columns.Add("billcode", typeof(string));
        dt.Columns.Add("PriceQuant", typeof(double));
        dt.Columns.Add("StaxAmt", typeof(double));
        dt.Columns.Add("Code", typeof(int));

        DataRow dr = dt.NewRow();
        dr["acct"] = DBNull.Value;
        dr["line"] = dt.Rows.Count;
        dr["stax"] = 0;
        dr["Code"] = 0;

        DataRow dr1 = dt.NewRow();
        dr1["acct"] = DBNull.Value;
        dr1["line"] = dt.Rows.Count;
        dr1["stax"] = 0;
        dr1["Code"] = 0;

        dt.Rows.Add(dr);
        dt.Rows.Add(dr1);

        ViewState["invoicetable"] = dt;
    }

    public void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillLocInfo();
        }
    }

    private void FillLocInfo()
    {
        if (hdnLocId.Value == "")
        {
            return;
        }
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        DataSet ds = new DataSet();
        ds = objBL_User.getLocationByID(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
            hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
            txtStaxrate.Text = ds.Tables[0].Rows[0]["stax"].ToString();
            hdnTaxRegion.Value = ds.Tables[0].Rows[0]["stax"].ToString();
            ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["defaultterms"].ToString();
            UpdateDueByTerms();
            GetDataProject();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["stax"].ToString()))
            {
                objPropUser.Stax = ds.Tables[0].Rows[0]["stax"].ToString();
                DataSet dsStax = new DataSet();
                dsStax = objBL_User.getSalesTaxByID(objPropUser);
                if (dsStax.Tables[0].Rows.Count > 0)
                {
                    hdnStax.Value = dsStax.Tables[0].Rows[0]["rate"].ToString();
                    txtStaxrate.Text = ds.Tables[0].Rows[0]["stax"].ToString() + " - " + dsStax.Tables[0].Rows[0]["rate"].ToString() + " %";
                }
            }
        }
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        txtProject.Text = string.Empty;
        hdnProjectId.Value = string.Empty;
        FillLocInfo();
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["InvoiceSrch"];
        Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[0]["ref"]);
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["InvoiceSrch"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ref"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[index - 1]["ref"]);
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["InvoiceSrch"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ref"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[index + 1]["ref"]);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["InvoiceSrch"];
        Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ref"]);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mode"]) != 1)
        {
            GetPeriodDetails(Convert.ToDateTime(txtInvoiceDate.Text));
        }
        if ((bool)ViewState["FlagPeriodClose"])
        {
            Submit();
            if (Request.QueryString["o"] != null)
            {
                if (success == true)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyClose", "setTimeout('window.close();', 3000);", true);
                }
            }
        }
        //Response.Redirect(Request.RawUrl);
    }

    private void Submit()
    {
        GridData(0);
        DataTable dtInvoice1 = new DataTable();
        dtInvoice1 = (DataTable)ViewState["invoicetable"];
        CalculateTotals(dtInvoice1);
        DataTable dtInvoiceCopy = dtInvoice1.Copy();
        DataTable dtInvoiceTrans = dtInvoice1.Copy();
        dtInvoiceCopy.Columns.Remove("priceQuant");
        dtInvoiceCopy.Columns.Remove("billcode");

        try
        {

        double TotalPretaxAmount = 0;
        double TotalSalesTaxrate = 0;
        double PricePerTotal = 0;

        if (gvInvoices.Rows.Count > 0)
        {
            Label lblPricePerTotal = (Label)gvInvoices.FooterRow.FindControl("lblPricePerTotal");
            Label lblTotalPretaxAmt = (Label)gvInvoices.FooterRow.FindControl("lblPretaxAmountTotal");
            Label lblTotalSalesTax = (Label)gvInvoices.FooterRow.FindControl("lblSalesTaxTotal");
            //Label lblTotalInvoice = (Label)gvInvoices.FooterRow.FindControl("lblAmountTotal");

            PricePerTotal = Convert.ToDouble(lblPricePerTotal.Text.Replace('$', '0'));
            TotalPretaxAmount = Convert.ToDouble(lblTotalPretaxAmt.Text.Replace('$', '0'));
            TotalSalesTaxrate = Convert.ToDouble(lblTotalSalesTax.Text);
        }

        objProp_Contracts.Total = PricePerTotal;
        objProp_Contracts.Amount = TotalPretaxAmount;
        objProp_Contracts.Staxtotal = TotalSalesTaxrate;
        if (hdnStax.Value != string.Empty)
        {
            objProp_Contracts.Taxrate = Convert.ToDouble(hdnStax.Value);
        }
        else
        {
            objProp_Contracts.Taxrate = 0;
        }

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.DtRecContr = dtInvoiceCopy;
        objProp_Contracts.Date = Convert.ToDateTime(txtInvoiceDate.Text);
        objProp_Contracts.Remarks = txtRemarks.Text;
        objProp_Contracts.TaxRegion = hdnTaxRegion.Value;
        objProp_Contracts.Taxfactor = 100;
        objProp_Contracts.Taxable = Convert.ToInt32(0);
        objProp_Contracts.Type = Convert.ToInt32(ddlDepartment.SelectedValue);
        objProp_Contracts.JobId = 0;
        if (hdnProjectId.Value != "")
        {
            objProp_Contracts.JobId = Convert.ToInt32(hdnProjectId.Value);
        }
        objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
        objProp_Contracts.Terms = Convert.ToInt32(ddlTerms.SelectedValue);
        objProp_Contracts.PO = txtPO.Text;
        objProp_Contracts.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Contracts.Batch = 0;
        objProp_Contracts.Remarks = txtRemarks.Text;
        objProp_Contracts.Gtax = 0.00;
        objProp_Contracts.Mech = Convert.ToInt32(ddlRoute.SelectedValue);
        objProp_Contracts.Taxrate2 = 0;
        objProp_Contracts.TaxRegion2 = string.Empty;
        objProp_Contracts.BillTo = txtAddress.Text;
        objProp_Contracts.Idate = Convert.ToDateTime(txtInvoiceDate.Text); //txtInvoiceDate.Text
        objProp_Contracts.DueDate = Convert.ToDateTime(txtDueDate.Text);

        objProp_Contracts.Fuser = Session["Username"].ToString();
        objProp_Contracts.StaxI = 1;
        objProp_Contracts.InvoiceIDCustom = txtInvoiceNo.Text;

        if (Request.QueryString["tickid"] != null)
        {
            if (Request.QueryString["tickid"] != string.Empty)
            {
                //objProp_Contracts.TicketID = Convert.ToInt32(Request.QueryString["tickid"]);
                objProp_Contracts.Tickets = ViewState["tickets"].ToString().Trim();
            }
        }

        if (Convert.ToInt32(ViewState["mode"]) == 1)
        {
            objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
            objBL_Contracts.UpdateInvoice(objProp_Contracts);

            LoadData();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Invoice Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            success = true;
            hdnFocus.Value = string.Empty;
        }
        else
        {
            objProp_Contracts.InvoiceID = objBL_Contracts.CreateInvoice(objProp_Contracts);

            ResetFormControlValues(this);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Invoice Created Successfully! </br> <b> Invoice# : " + objProp_Contracts.InvoiceID.ToString() + "</b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            ViewState["mode"] = 0;
            success = true;
            hdnFocus.Value = string.Empty;
            IntializeData();
        }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '"+ str +"',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            success = false;
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

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=inv");
        }
        else
        {
            Response.Redirect("invoices.aspx?fil=1");
        }
    }

    protected void lnkCustSave_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)ViewState["invoicetable"];

        //DataRow dr = dt.NewRow();

        //dr["Ref"] = 0;
        //dr["line"] = dt.Rows.Count;
        //dr["Acct"] = ddlBillingCode.SelectedValue;
        //dr["Quan"] =Convert.ToDouble( txtQuantity.Text);
        //dr["fDesc"] = txtRemarks0.Text;
        //dr["PriceQuant"] = Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtpricePer.Text);
        //dr["Amount"] = Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtpricePer.Text) * Convert.ToDouble(txtSalesTax.Text);
        //dr["STax"] =Convert.ToDouble(txtSalesTax.Text);
        //dr["Job"] = 0;
        //dr["JobItem"] = 0;
        //dr["TransID"] = 0;
        //dr["Measure"] = string.Empty;
        //dr["Disc"] = 0;
        //dr["Price"] = Convert.ToDouble(txtpricePer.Text);
        //dr["billcode"] = ddlBillingCode.SelectedItem.Text;

        //if (ViewState["editcon"].ToString() == "1")
        //{
        //    dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
        //    dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
        //    ViewState["editcon"] = 0;
        //}
        //else
        //{
        //    dt.Rows.Add(dr);
        //}

        //dt.AcceptChanges();

        //ViewState["invoicetable"] = dt;

        //BindGrid();

        ////ClearContact();
        ////TogglePopup();

        ////if (ViewState["mode"].ToString() == "1")
        ////{
        ////SubmitContact();
        ////}
    }

    private void ClearDetails()
    {
        //txtQuantity.Text = string.Empty;
        //ddlBillingCode.SelectedIndex = 0;
        //txtRemarks0.Text = string.Empty;
        //txtpricePer.Text = string.Empty;
        //chkTaxable.Checked = false;
        //txtPretax.Text = string.Empty;
        //txtpricePer.Text = string.Empty;
        //txtAmount.Text = string.Empty;
        //txtSalesTax.Text = string.Empty;
    }

    private void BindGrid()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["invoicetable"];

        gvInvoices.DataSource = dt;
        gvInvoices.DataBind();

        CalculateTotals(dt);
    }

    private void CalculateTotals(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            Label lblPricePerTotal = (Label)gvInvoices.FooterRow.FindControl("lblPricePerTotal");
            Label lblTotalPretaxAmt = (Label)gvInvoices.FooterRow.FindControl("lblPretaxAmountTotal");
            Label lblTotalSalesTax = (Label)gvInvoices.FooterRow.FindControl("lblSalesTaxTotal");
            Label lblTotalInvoice = (Label)gvInvoices.FooterRow.FindControl("lblAmountTotal");

            double TotalPricePer = 0;
            double TotalPretaxAmt = 0;
            double TotalSalesTax = 0;
            double TotalInvoice = 0;

            double PricePer = 0;
            double PretaxAmt = 0;
            double SalesTax = 0;
            double Invoice = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Price"] != DBNull.Value)
                {
                    PricePer = Convert.ToDouble(dr["Price"]);
                    TotalPricePer += PricePer;
                }

                if (dr["PriceQuant"] != DBNull.Value)
                {
                    PretaxAmt = Convert.ToDouble(dr["PriceQuant"]);
                    TotalPretaxAmt += PretaxAmt;
                }

                if (dr["staxAmt"] != DBNull.Value)
                {
                    SalesTax = Convert.ToDouble(dr["staxAmt"]);
                    TotalSalesTax += SalesTax;
                }

                if (dr["amount"] != DBNull.Value)
                {
                    Invoice = Convert.ToDouble(dr["amount"]);
                    TotalInvoice += Invoice;
                }
            }

            lblPricePerTotal.Text = string.Format("{0:n}", TotalPricePer);
            lblTotalPretaxAmt.Text = string.Format("{0:n}", TotalPretaxAmt);
            lblTotalSalesTax.Text = string.Format("{0:n}", TotalSalesTax);
            lblTotalInvoice.Text = string.Format("{0:n}", TotalInvoice);
        }
    }

    protected void lnkCancelCust_Click(object sender, EventArgs e)
    {
        //programmaticModalPopup.Hide();
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        AddNew(0);

    }
    private void AddNew(int resetJobCode)
    {
        GridData(resetJobCode);

        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["invoicetable"];

        DataRow dr = dt.NewRow();
        
        if (ViewState["InvServ"] != null)
        {
            if (ViewState["InvServ"].ToString() != string.Empty && ViewState["InvServ"].ToString() != "0")
            {
                dr["acct"] = ViewState["InvServ"].ToString();
            }
        }
        else
        {
            dr["acct"] = DBNull.Value;
        }
        if (ViewState["BillRate"] != null)
        {
            dr["Price"] = ViewState["BillRate"].ToString();
        }
        dr["Quan"] = 1;
        dr["line"] = dt.Rows.Count;
        dr["stax"] = 0;
        dr["code"] = 0;
        dt.Rows.Add(dr);

        ViewState["invoicetable"] = dt;

        BindGrid();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        GridData(0);

        DataTable dt = (DataTable)ViewState["invoicetable"];
        int count = 0;
        for (int i = 0; i < gvInvoices.Rows.Count; i++)
        {
            CheckBox chkSelect = (CheckBox)gvInvoices.Rows[i].FindControl("chkSelect");
            Label lblindex = (Label)gvInvoices.Rows[i].FindControl("lblindex");

            if (chkSelect.Checked == true)
            {
                if (count == 0)
                {
                    dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));
                    count = 1;
                }
            }
        }
        dt.AcceptChanges();
        ViewState["invoicetable"] = dt;
        BindGrid();
        hdnFocus.Value = string.Empty;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvInvoices.Rows)
        //{
        //    DataTable dt = (DataTable)ViewState["invoicetable"];
        //    CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
        //    Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

        //    if (chkSelect.Checked== true)
        //    {
        //        programmaticModalPopup.Show();

        //        DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

        //        txtQuantity.Text = dr["Quan"].ToString();
        //        ddlBillingCode.SelectedValue = dr["Acct"].ToString();
        //        txtRemarks0.Text = dr["fDesc"].ToString();
        //        txtpricePer.Text = dr["Price"].ToString();
        //        //chkTaxable.Checked= dr["Email"].ToString();
        //        txtPretax.Text = dr["PriceQuant"].ToString();
        //        txtSalesTax.Text = dr["STax"].ToString();
        //        txtAmount.Text = dr["Amount"].ToString();
        //        ViewState["editcon"] = 1;
        //        ViewState["index"] = lblindex.Text;
        //    }
        //}
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        //btnSubmit_Click(sender, e);
        Submit();

        if (success == true)
        {
            if (Request.QueryString["o"] == null)
            {
                Response.Redirect("Printinvoice.aspx?uid=" + objProp_Contracts.InvoiceID, true);
            }
            else
            {
                Response.Redirect("Printinvoice.aspx?uid=" + objProp_Contracts.InvoiceID + "&o=1", true);
            }
        }
    }

    private void GridData(int resetJobCode)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["invoicetable"];

            DataTable dtDetails = dt.Clone();

            foreach (GridViewRow gr in gvInvoices.Rows)
            {
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
                DropDownList ddlBillingCode = (DropDownList)gr.FindControl("ddlBillingCode");
                TextBox lblDescription = (TextBox)gr.FindControl("lblDescription");
                TextBox lblPricePer = (TextBox)gr.FindControl("lblPricePer");
                TextBox lblSalesTax = (TextBox)gr.FindControl("lblSalesTax");
                CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
                DropDownList ddlProjectCode = (DropDownList)gr.FindControl("ddlProjectCode");

                if (lblQuantity.Text.Trim() != string.Empty && lblPricePer.Text.Trim() != string.Empty)
                {
                    DataRow dr = dtDetails.NewRow();

                    dr["Ref"] = 0;
                    dr["line"] = lblIndex.Text;
                    if (ddlBillingCode.SelectedIndex != 0)
                    {
                        dr["Acct"] = ddlBillingCode.SelectedValue;
                    }
                    if (lblQuantity.Text != "")
                    {
                        dr["Quan"] = Convert.ToDouble(lblQuantity.Text);
                    }

                    dr["fDesc"] = lblDescription.Text;
                    if (lblQuantity.Text != "" && lblPricePer.Text != "")
                    {
                        dr["PriceQuant"] = Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text);
                    }
                    else
                    {
                        dr["PriceQuant"] = 0.00;
                    }

                    if (lblQuantity.Text != "" && lblPricePer.Text != "" && hdnStax.Value != "")
                    {
                        if (chkTaxable.Checked == true)
                        {
                            dr["STaxAmt"] = (((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnStax.Value)) / 100);
                        }
                        else
                        {
                            dr["STaxAmt"] = 0.00;
                        }
                    }
                    else
                    {
                        dr["STaxAmt"] = 0.00;
                    }

                    if (lblQuantity.Text != "" && lblPricePer.Text != "" && hdnStax.Value != "")
                    {
                        ////dr["Amount"] = Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text) * Convert.ToDouble(lblSalesTax.Text);
                        //dr["Amount"] = (Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) + (((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnStax.Value)) / 100);
                        dr["Amount"] = Convert.ToDouble(dr["PriceQuant"]) + Convert.ToDouble(dr["STaxAmt"]);
                    }
                    else
                    {
                        dr["Amount"] = 0.00;
                    }

                    dr["STax"] = Convert.ToInt32(chkTaxable.Checked);
                    dr["Job"] = 0;
                    dr["JobItem"] = 0;
                    dr["TransID"] = 0;
                    dr["Measure"] = string.Empty;
                    dr["Disc"] = 0;
                    if (lblPricePer.Text != "")
                    {
                        dr["Price"] = Convert.ToDouble(lblPricePer.Text);
                    }
                    else
                    {
                        dr["Price"] = 0.00;
                    }

                    dr["billcode"] = ddlBillingCode.SelectedItem.Text;

                    if (resetJobCode == 1)
                        dr["Code"] = 0;
                    else
                        dr["Code"] = ddlProjectCode.SelectedValue;

                    dtDetails.Rows.Add(dr);
                }
            }

            ViewState["invoicetable"] = dtDetails;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    protected void ddlBillingCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridData(0);
        DropDownList ddlBillingCode = (DropDownList)sender;
        GridViewRow grdrDropDownRow = ((GridViewRow)ddlBillingCode.Parent.Parent);
        TextBox lblDescription = (TextBox)grdrDropDownRow.FindControl("lblDescription");
        TextBox lblPricePer = (TextBox)grdrDropDownRow.FindControl("lblPricePer");

        bool IsBillRate = false;
        DataSet dsJob = objBL_User.GetJobBillRatesById(objPropUser);
        if (dsJob.Tables[0].Rows.Count > 0)
        {
            if(Convert.ToDouble(dsJob.Tables[0].Rows[0]["BillRate"].ToString()) > 0)
            {
                IsBillRate = true;
            }
        }
        if (ddlBillingCode.SelectedValue != string.Empty)
        {
            objPropUser.BillCode = Convert.ToInt32(ddlBillingCode.SelectedValue);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getBillCodesByID(objPropUser);

            lblDescription.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
            if(IsBillRate)
            {
                lblPricePer.Text = ds.Tables[0].Rows[0]["BillRate"].ToString();
            }
            else
            {
                lblPricePer.Text = ds.Tables[0].Rows[0]["Price1"].ToString();
            }
        }
        else
        {
            lblDescription.Text = string.Empty;
            lblPricePer.Text = "0.00";
        }
        GridData(0);
        BindGrid();
    }

    protected void ddlTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateDueByTerms();
    }
    protected void lnkMakePayment_Click(object sender, EventArgs e)
    {
        //btnSubmit_Click(sender, e);

        Submit();

        if (success == true)
        {
            if (Request.QueryString["o"] == null)
                Response.Redirect("payment.aspx?uid=" + objProp_Contracts.InvoiceID + "&amt=" + objProp_Contracts.Total, true);
            else
                Response.Redirect("payment.aspx?uid=" + objProp_Contracts.InvoiceID + "&amt=" + objProp_Contracts.Total + "&o=1", true);
        }
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        AddOtherTickets();
    }

    private DataSet BuildLineItems(double totalTime, double Expenses, int mileage)
    {
        //int count = 0;
        //int countTT = 0;        
        //int countMil = 0;
        string line = string.Empty;
        if (Expenses != 0)
        {
            //count += 1;  
            line = "'expenses'";
        }
        if (mileage != 0)
        {
            //countMil = count;
            //count += 1;
            if (line != string.Empty)
            {
                line += ",";
            }
            line += "'mileage'";
        }
        if (totalTime != 0)
        {
            //countTT = count;        
            if (line != string.Empty)
            {
                line += ",";
            }
            line += "'Time Spent'";
        }

        if (line.Trim() == string.Empty)
        {
            line = "'Time Spent'";
        }

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.TicketLineItems = line;
        DataSet dsLineItem = objBL_Contracts.GetBillcodesforticket(objProp_Contracts);

        //if (dsLineItem.Tables[0].Rows.Count > 0)
        //{
        //    if (Expenses != 0)
        //    {
        //        dsLineItem.Tables[0].Rows[0]["Quan"] = 1;
        //        dsLineItem.Tables[0].Rows[0]["price"] = Expenses;
        //    }
        //    if (mileage != 0)
        //    {
        //        dsLineItem.Tables[0].Rows[countMil]["Quan"] = mileage;
        //    }
        //    if (totalTime != 0)
        //    {
        //        dsLineItem.Tables[0].Rows[countTT]["Quan"] = totalTime;
        //    }
        //}

        foreach (DataRow dr in dsLineItem.Tables[0].Rows)
        {
            if (Expenses != 0)
            {
                if (string.Equals(dr["billcode"].ToString(), "expenses", StringComparison.InvariantCultureIgnoreCase))
                {
                    dr["Quan"] = 1;
                    dr["price"] = Expenses;
                }
            }
            if (mileage != 0)
            {
                if (string.Equals(dr["billcode"].ToString(), "mileage", StringComparison.InvariantCultureIgnoreCase))
                    dr["Quan"] = mileage;
            }
            if (totalTime != 0)
            {
                if (string.Equals(dr["billcode"].ToString(), "Time Spent", StringComparison.InvariantCultureIgnoreCase))
                    dr["Quan"] = totalTime;
            }
        }

        return dsLineItem;
    }

    private void AddOtherTicketsAlert()
    {
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["tickid"].ToString());
        DataSet dsOtherTickets = new DataSet();
        dsOtherTickets = objBL_MapData.GetInvoiceTicketByWorkorder(objMapData);

        if (dsOtherTickets.Tables[0].Rows.Count > 1)
        {
            programmaticModalPopup.Show();
            if (dsOtherTickets.Tables[0].Rows.Count == 2)
                lblCount.Text = "There exists 1 more chargeable ticket for the same workorder. Do you like to include it with this invoice? ";
            else
                lblCount.Text = "There exists " + Convert.ToString(dsOtherTickets.Tables[0].Rows.Count - 1) + " more chargeable tickets for the same workorder. Do you like to include them with this invoice? ";

            ViewState["othertickets"] = dsOtherTickets.Tables[0];
        }
    }

    private void AddOtherTickets()
    {
        double totalTime = 0;
        double Expenses = 0;
        int mileage = 0;
        int Count = 0;
        string strRemarks = "Combined invoice for Ticket# ";
        string strTickets = string.Empty;

        DataTable dsOtherTickets = (DataTable)ViewState["othertickets"];
        foreach (DataRow dr in dsOtherTickets.Rows)
        {
            Count++;
            totalTime += Convert.ToDouble(dr["total"]);
            Expenses += Convert.ToDouble(dr["Expenses"]);
            mileage += Convert.ToInt32(dr["mileage"]);

            if (Count == 1)
            {
                strRemarks += dr["ID"].ToString();
                strTickets += dr["ID"].ToString();
            }
            else if (Count == dsOtherTickets.Rows.Count)
            {
                strRemarks += " and " + dr["ID"].ToString();
                strTickets += "," + dr["ID"].ToString();
            }
            else
            {
                strRemarks += ", " + dr["ID"].ToString();
                strTickets += "," + dr["ID"].ToString();
            }
        }

        DataTable dtLineItem = BuildLineItems(totalTime, Expenses, mileage).Tables[0];

        txtRemarks.Text = strRemarks;
        ViewState["tickets"] = strTickets;
        ViewState["invoicetable"] = dtLineItem;
        BindGrid();
        ViewState["othertickets"] = null;
    }

    private void LoadTicketData()
    {
        if (Request.QueryString["o"] != null)
        {
            lnkClose.Attributes["onclick"] = "window.close(); return;";
            pnlNext.Visible = false;
            if (Request.QueryString["tickid"] != null)
            {
                DataSet ds = new DataSet();
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.ISTicketD = 1;
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["tickid"].ToString());
                ds = objBL_MapData.GetTicketByID(objMapData);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["invoice"] != DBNull.Value)
                    {
                        if (ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != string.Empty && ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != "0")
                        {
                            Response.Redirect("addinvoice.aspx?o=1&uid=" + ds.Tables[0].Rows[0]["invoice"].ToString());
                        }
                    }
                    double totalTime = Convert.ToDouble(ds.Tables[0].Rows[0]["total"]);
                    double Expenses = Convert.ToDouble(ds.Tables[0].Rows[0]["othere"]) + Convert.ToDouble(ds.Tables[0].Rows[0]["toll"]) + Convert.ToDouble(ds.Tables[0].Rows[0]["zone"]);
                    int mileage = Convert.ToInt32(ds.Tables[0].Rows[0]["emile"]) - Convert.ToInt32(ds.Tables[0].Rows[0]["Smile"]);

                    txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                    hdnLocId.Value = ds.Tables[0].Rows[0]["lid"].ToString();
                    hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                    ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    FillWorker(Convert.ToInt32(ds.Tables[0].Rows[0]["fwork"].ToString()));
                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["fwork"].ToString();
                    txtRemarks.Text = "Invoice for Ticket# " + Request.QueryString["tickid"].ToString() + Environment.NewLine;
                    txtRemarks.Text += "Reason for service: " + Environment.NewLine + ds.Tables[0].Rows[0]["fdesc"].ToString() + Environment.NewLine;
                    txtRemarks.Text += "Work complete desc.: " + Environment.NewLine + ds.Tables[0].Rows[0]["descres"].ToString();
                    txtDueDate.Text = System.DateTime.Now.Date.ToShortDateString();

                    hdnProjectId.Value = ds.Tables[0].Rows[0]["job"].ToString();
                    FillProjectCodes();
                    txtProject.Text = ds.Tables[0].Rows[0]["job"].ToString();

                    FillLocInfo();

                    DataSet dsLineItem = BuildLineItems(totalTime, Expenses, mileage);

                    ViewState["invoicetable"] = dsLineItem.Tables[0];
                    ViewState["tickets"] = Request.QueryString["tickid"].ToString();
                    BindGrid();

                    AddOtherTicketsAlert();
                }
            }
        }
    }

    private void GetDataProject()
    {
        DataSet ds = new DataSet();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.LocID = Convert.ToInt32(hdnLocId.Value);

        ds = objBL_Customer.getJobEstimate(objCustomer);
        gvProject.DataSource = ds.Tables[0];
        gvProject.DataBind();
    }
    private string calculateTotal()     // added by Mayuri 7th Dec,15
    {
        double _totalAmount = 0.00;
        double _taxAmt = 0.00;
        double _pretaxAmount = 0.00;
        foreach (GridViewRow gr in gvInvoices.Rows)
        {
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
            DropDownList ddlBillingCode = (DropDownList)gr.FindControl("ddlBillingCode");
            TextBox lblDescription = (TextBox)gr.FindControl("lblDescription");
            TextBox lblPricePer = (TextBox)gr.FindControl("lblPricePer");
            CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
            Label lblPretaxAmount = (Label)gr.FindControl("lblPretaxAmount");

            if (lblQuantity.Text.Trim() != string.Empty && lblPricePer.Text.Trim() != string.Empty)
            {
                if (hdnStax.Value != "")
                {
                    if (chkTaxable.Checked == true)
                    {
                        _pretaxAmount = _pretaxAmount + Convert.ToDouble(lblPretaxAmount.Text);
                        _taxAmt = (((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnStax.Value)) / 100);
                    }
                }
                _totalAmount = _totalAmount + (Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) + Convert.ToDouble(_taxAmt);


            }
        }
        return _totalAmount + "|" + _pretaxAmount;
    }
    
    private void GetPeriodDetails(DateTime _invDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_invDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
        }
    }
    protected void btnGetCode_Click(object sender, EventArgs e)
    {
        FillProjectCodes();
        FillProjectData();
        ddlDepartment.Enabled = false;
        AddNew(1);
    }
    private void FillProjectData()
    {
        try
        {
            if (txtProject.Text != string.Empty)
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.ProjectJobID = Convert.ToInt32(txtProject.Text);
                objCustomer.Type = string.Empty;
                DataSet ds = objBL_Customer.getJobProjectByJobID(objCustomer);
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.JobId = Convert.ToInt32(hdnProjectId.Value);
                DataSet dsJob = objBL_User.GetJobBillRatesById(objPropUser);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtPO.Text = dr["PO"].ToString();
                    if (!string.IsNullOrEmpty(dr["Type"].ToString()))
                    {
                        ddlDepartment.SelectedValue = dr["Type"].ToString();
                    }
                    
                    ViewState["InvServ"] = dr["InvServ"].ToString();
                    double billrate = 0;
                    if(dsJob.Tables[0].Rows.Count > 0)
                    {
                        billrate = Convert.ToDouble(dsJob.Tables[0].Rows[0]["BillRate"]);
                    }
                    ViewState["BillRate"] = billrate;
                    if(billrate > 0)
                    {
                        hdnBillRate.Value = billrate.ToString();
                    }
                    DataTable dtInv = (DataTable)ViewState["invoicetable"];
                    if(!string.IsNullOrEmpty(dr["InvServ"].ToString()) && dr["InvServ"].ToString() != "0")
                    {   
                        foreach(DataRow drInv in dtInv.Rows)
                        {
                            drInv["acct"] = dr["InvServ"].ToString();
                        }
                    }
                    if(billrate > 0)
                    {
                        dtInv.AsEnumerable().ToList().
                            ForEach(t => t["Price"] = billrate);
                    }
                    ViewState["invoicetable"] = dtInv;
                    BindGrid();
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlProjectCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlProjectCode = (DropDownList)sender;
            GridViewRow gridrow = (GridViewRow)ddlProjectCode.NamingContainer;
            int rowIndex = gridrow.RowIndex;

            if (!ddlProjectCode.SelectedValue.Equals("0"))
            {
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = Convert.ToInt32(hdnProjectId.Value);
                objJob.Line = Convert.ToInt16(ddlProjectCode.SelectedValue);
                DataSet ds = objBL_Job.GetRevenueJobItemsByJob(objJob);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    TextBox lblDescription = (TextBox)gridrow.FindControl("lblDescription");
                    TextBox lblPricePer = (TextBox)gridrow.FindControl("lblPricePer");
                    Label lblPretaxAmount = (Label)gridrow.FindControl("lblPretaxAmount");
                    TextBox lblSalesTax = (TextBox)gridrow.FindControl("lblSalesTax");
                    Label lblAmount = (Label)gridrow.FindControl("lblAmount");

                    lblDescription.Text = dr["fDesc"].ToString();
                    lblPricePer.Text = Convert.ToDouble(dr["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
                    lblSalesTax.Text = "0.00";
                    lblPretaxAmount.Text = Convert.ToDouble(dr["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
                    lblAmount.Text = Convert.ToDouble(dr["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
                }
            }
            GridData(0);
            DataTable dt = (DataTable)ViewState["invoicetable"];
            CalculateTotals(dt);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateDueByTerms()
    {
        if (ddlTerms.SelectedValue == "0")
        {
            txtDueDate.Text = txtInvoiceDate.Text;
        }
        else if (ddlTerms.SelectedValue == "1")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(10).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "2")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(15).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "3")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(30).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "4")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(45).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "5")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(60).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "6")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(30).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "7")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(90).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "8")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(180).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "9")
        {
            txtDueDate.Text = txtInvoiceDate.Text;
        }
        else if (ddlTerms.SelectedValue == "10") //120 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(120).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "11") //150 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(150).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "12") //210 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(210).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "13") //240 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(240).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "14") //270 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(270).ToShortDateString();
        }
        else if(ddlTerms.SelectedValue == "15") //300 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(300).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "16") //net due on 10th
        {
            txtDueDate.Text = "";
        }
        else if (ddlTerms.SelectedValue == "17") //net due
        {
            txtDueDate.Text = "";
        }
        else if (ddlTerms.SelectedValue == "18") //Credit card
        {
            txtDueDate.Text = "";
        }
    }
    protected void imgPaid_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            _objInvoices.ConnConfig = Session["config"].ToString();
            _objInvoices.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            DataSet ds = objBL_Invoice.GetReceivePayInvoice(_objInvoices);
            if(ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    imgPaid.OnClientClick = "window.open('addreceivepayment.aspx?id=" + dr["ReceivedPaymentID"].ToString() + "');";
                }
            }
            else
            {
                ds = objBL_Invoice.GetAppliedDeposit(_objInvoices);
                if(ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        imgPaid.OnClientClick = "window.open('adddeposit.aspx?id=" + dr["Ref"].ToString() + "');";
                    }
                }
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}
