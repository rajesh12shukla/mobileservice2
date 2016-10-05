using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Net;
using Exchance;

public partial class AddEstimate : System.Web.UI.Page
{

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    protected DataTable dtBomType = new DataTable();
    protected DataTable dtBomItem = new DataTable();
    protected DataTable dtCurrency = new DataTable();
    JobT _objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    Wage _objWage = new Wage();
    BL_User objBL_User = new BL_User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        txtJobType.Enabled = false;
        //DropDownList ddlestimatecurrency = (DropDownList)gvBOM.HeaderRow.FindControl("ddlCurrencyEst");
        this.FillBomType();
        //this.FillBomTypeInitial();
        // FillBomType();            
        if (!IsPostBack)
        {
            SetDateField();
            CreateBOMTable();
            CreateMilestoneTable();
            // FillBucketDropdown();
            FillTemplateDropdown();
            // btnEdit.Visible = false;
            objProp_Customer.ConnConfig = Session["config"].ToString();
            ViewState["labor"] = objBL_Customer.GetEstimateLabor(objProp_Customer);
            ViewState["edit"] = 0;
            if (Request.QueryString["uid"] != null)
            {
                ViewState["edit"] = 1;
                GetTemplate(false);
            }
            else
            {
                SetExchange();
                // CreateTable();
            }
        }
        Permission();
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkEstimate");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        ul.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //lnkDelete.Visible = false;
        }
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            if (Sales == "N")
            {
                Response.Redirect("home.aspx");
            }
        }

    }

    private void SetDateField()
    {
        DateTime _now = DateTime.Now;
        var _startDate = new DateTime(_now.Year, _now.Month, _now.Day);
        TxtDate.Text = _startDate.ToShortDateString();
    }
    private void CreateMilestoneTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquistDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));

        DataRow dr = dt.NewRow();
        dr["Line"] = 0;
        dt.Rows.Add(dr);

        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();

    }
    protected void gvBOM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region

        //BOM Item list
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                DropDownList ddlCurrencyEst =
        (DropDownList)gvBOM.HeaderRow.FindControl("ddlCurrencyEst");
                DropDownList ddlBType = (e.Row.FindControl("ddlBType") as DropDownList);
                DropDownList ddlItem = (e.Row.FindControl("ddlItem") as DropDownList);
                Label lblBudgetExt = (e.Row.FindControl("lblBudgetExt") as Label);
                TextBox txtBudgetUnit = (e.Row.FindControl("txtBudgetUnit") as TextBox);
                TextBox txtQtyReq = (e.Row.FindControl("txtQtyReq") as TextBox);
                DropDownList ddlCurrencyEstbind = (e.Row.FindControl("ddlCurrencyEstbind") as DropDownList);
                if (ViewState["ddlCurrencyEst"] != null)
                    ddlCurrencyEstbind.Items.FindByValue(ViewState["ddlCurrencyEst"].ToString())
            .Selected = true;

                //List<ListItem> dtCurrencyItems = new List<ListItem>();
                //dtCurrencyItems.Add(new ListItem() { Text = "select", Value = "0" });
                //dtCurrencyItems.Add(new ListItem() { Text = "US", Value = "1" });
                //dtCurrencyItems.Add(new ListItem() { Text = "CDN", Value = "2" });
                //ddlCurrency.DataSource = dtCurrencyItems;

                //ddlCurrency.DataBind();


                double _budgetExt = 0.0;
                double _qtyReq = 0.0;
                if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
                {
                    FillInv(ddlItem);

                    if (!string.IsNullOrEmpty(txtQtyReq.Text))
                    {
                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                    }
                }
                else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
                {
                    FillWage(ddlItem);
                }
                else
                {
                    FillItems(ddlItem);
                }
                if (!string.IsNullOrEmpty(txtBudgetUnit.Text))
                {
                    if (!string.IsNullOrEmpty(txtQtyReq.Text))
                    {
                        if (_qtyReq.Equals(0))
                            _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                    }
                    _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
                }

                if (ViewState["TempBOM"] != null)
                {
                    DataTable _dtBom = (DataTable)ViewState["TempBOM"];
                    if (_dtBom.Rows.Count > 0)
                    {
                        foreach (var item in _dtBom.Rows)
                        {
                            var itemVal = _dtBom.Rows[0]["BItem"].ToString();
                            if (!string.IsNullOrEmpty(itemVal))
                            {
                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    itemVal = DataBinder.Eval(e.Row.DataItem, "BItem").ToString();
                                    ddlItem.SelectedValue = itemVal.ToString();
                                }
                            }
                        }
                    }
                }

                break;
        }


        #endregion
    }
    protected void gvBOM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        #region

        if (e.CommandName.Equals("AddBOMItem"))
        {
            int rowIndex = gvBOM.Rows.Count - 1;
            GridViewRow row = gvBOM.Rows[rowIndex];
            HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;
            HiddenField hdnLine = row.FindControl("hdnLine") as HiddenField;

            DataTable dt = GetBOMGridItems();

            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Line"] = 0;
                    dt.Rows.Add(dr);
                }
            }

            DataRow dr2 = dt.NewRow();
            dr2["Line"] = 0;
            dt.Rows.Add(dr2);

            ViewState["TempBOM"] = dt;
            gvBOM.DataSource = dt;
            gvBOM.DataBind();
        }

        #endregion
    }
    private DataTable GetBOMGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("jBudget", typeof(double));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("btype", typeof(int));
        dt.Columns.Add("BItem", typeof(string));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("ScrapFact", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));
        dt.Columns.Add("Actual", typeof(double));
        dt.Columns.Add("jPercent", typeof(double));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("currency", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("TotalPrice", typeof(double));

        /*   DataTable dt = new DataTable();
           dt.Columns.Add("Line", typeof(int));
           dt.Columns.Add("jcode", typeof(string));
           dt.Columns.Add("btype", typeof(int));
           dt.Columns.Add("BItem", typeof(string));
           dt.Columns.Add("fdesc", typeof(string));
           dt.Columns.Add("Vendor", typeof(string));
           dt.Columns.Add("currency", typeof(string));
           dt.Columns.Add("QtyReq", typeof(double));
           dt.Columns.Add("UM", typeof(string));
           dt.Columns.Add("BudgetUnit", typeof(double));
           dt.Columns.Add("BudgetExt", typeof(double));
           dt.Columns.Add("jPercent", typeof(double));
           dt.Columns.Add("Amount", typeof(double));
           dt.Columns.Add("TotalPrice", typeof(double));*/

        /* 
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));   
        dt.Columns.Add("jBudget", typeof(double));
        dt.Columns.Add("ScrapFact", typeof(string));
        dt.Columns.Add("Actual", typeof(double));
        */

        string strItems = hdnBOMItemJSON.Value.Trim();
        double _budgetExt = 0;
        double _qtyReq = 0;
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    _qtyReq = 0;
                    _budgetExt = 0;
                    if (dict["hdnLine"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();

                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["Vendor"] = dict["txtVendorEst"].ToString().Trim();
                    dr["currency"] = dict["ddlCurrencyEstbind"].ToString().Trim();
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    //dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    if (dict["ddlBType"].ToString() != "0")
                    {
                        dr["bType"] = Convert.ToInt32(dict["ddlBType"]);
                    }

                    if (!Convert.ToInt32(dict["ddlItem"]).Equals(0))
                    {
                        dr["BItem"] = Convert.ToInt32(dict["ddlItem"]);
                    }

                    if (dict["txtQtyReq"].ToString().Trim() != string.Empty)
                    {
                        dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
                    }
                    else
                    {
                        dr["QtyReq"] = 0;
                    }

                    if (dict["txtUM"].ToString().Trim() != string.Empty)
                    {
                        dr["UM"] = dict["txtUM"].ToString().Trim();
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
                    }
                    if (dict.ContainsKey("txtPercntge"))
                    {
                        if (dict["txtPercntge"].ToString().Trim() != string.Empty)
                        {
                            dr["jPercent"] = Convert.ToDouble(dict["txtPercntge"]);
                        }

                    }
                    else
                    {
                        if (dict["txtAmt"].ToString().Trim() != string.Empty)
                        {
                            dr["Amount"] = Convert.ToDouble(dict["txtAmt"]);
                        }
                    }
                    if (dict["hdnTotalPrice"].ToString().Trim() != string.Empty)
                    {
                        dr["TotalPrice"] = Convert.ToDouble(dict["hdnTotalPrice"]);
                    }

                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
                        }
                        _budgetExt = _qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
                        dr["BudgetExt"] = _budgetExt;
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    protected void ddlBType_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region comment

        DropDownList ddlBomType = (DropDownList)sender;
        GridViewRow gridrow = (GridViewRow)ddlBomType.NamingContainer;
        int rowIndex = gridrow.RowIndex;

        foreach (GridViewRow gr in gvBOM.Rows)
        {
            if (gr.RowIndex == rowIndex)
            {
                DropDownList ddlBType = (DropDownList)gr.FindControl("ddlBType");
                DropDownList ddlItem = (DropDownList)gr.FindControl("ddlItem");
                TextBox txtBudgetUnit = (TextBox)gr.FindControl("txtBudgetUnit");
                TextBox txtQtyReq = (TextBox)gr.FindControl("txtQtyReq");
                Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
                HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

                if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
                {
                    FillInv(ddlItem);
                }
                else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
                {
                    FillWage(ddlItem);
                }
                else
                {
                    FillItems(ddlItem);
                }
                double _budgetExt = 0;
                if (ddlBType.SelectedValue.Equals("1")) // if Materials
                {
                    double _qtyReq = 0.0;
                    //double _budgetUnit = 0.0;
                    //double _scrapFact = 0.0;


                    if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
                    {

                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                        _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
                        //_budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);

                    }
                    lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                    hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
                    {
                        _budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);
                    }
                    lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                    hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                }

            }
        }

        foreach (GridViewRow gr in gvBOM.Rows)
        {
            Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
            HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

            lblBudgetExt.Text = hdnBudgetExt.Value;
        }

        #endregion
    }
    private void FillInv(DropDownList ddlItem)
    {
        try
        {
            _objJob.ConnConfig = Session["config"].ToString();
            DataSet _dsInv = objBL_Job.GetInventoryItem(_objJob);
            dtBomItem = _dsInv.Tables[0];

            if (_dsInv.Tables[0].Rows.Count > 0)
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("Select Item", "0"));
                ddlItem.AppendDataBoundItems = true;
                ddlItem.DataSource = _dsInv;

                ddlItem.DataValueField = "value";
                ddlItem.DataTextField = "label";
                ddlItem.DataBind();
            }
            else
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillWage(DropDownList ddlItem)
    {
        try
        {
            _objWage.ConnConfig = Session["config"].ToString();
            DataSet _dsWage = objBL_User.GetAllWage(_objWage);
            dtBomItem = _dsWage.Tables[0];

            if (_dsWage.Tables[0].Rows.Count > 0)
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("Select Item", "0"));
                ddlItem.AppendDataBoundItems = true;
                ddlItem.DataSource = _dsWage;

                ddlItem.DataValueField = "value";
                ddlItem.DataTextField = "label";
                ddlItem.DataBind();
            }
            else
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillItems(DropDownList ddlItem)
    {
        ddlItem.Items.Clear();
        ddlItem.Items.Add(new ListItem("No data found", "0"));
        ddlItem.DataBind();
    }
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region comment

        DropDownList ddlItem = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlItem.NamingContainer;
        TextBox txtScope = (TextBox)row.FindControl("txtScope");
        DropDownList ddlBType = (DropDownList)row.FindControl("ddlBType");

        DataSet ds = new DataSet();
        if (ddlItem.SelectedValue != "0")
        {
            if (ddlBType.SelectedValue == "1" || ddlBType.SelectedValue == "2")       // select bom type Material
            {
                if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "1")
                {
                    _objJob.ConnConfig = Session["config"].ToString();
                    ds = objBL_Job.GetInventoryItem(_objJob);
                    dtBomItem = ds.Tables[0];
                }
                else if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "2")
                {
                    _objWage.ConnConfig = Session["config"].ToString();
                    ds = objBL_User.GetAllWage(_objWage);
                    dtBomItem = ds.Tables[0];
                }
                if (dtBomItem.Rows.Count > 0)
                {
                    DataRow dr = dtBomItem.Select("Value =" + ddlItem.SelectedValue).FirstOrDefault();
                    if (dr != null)
                    {
                        txtScope.Text = dr["fDesc"].ToString();
                    }
                    else
                    {
                        txtScope.Text = "";
                    }
                }
            }
        }
        #endregion
    }
    private void CreateBOMTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("jBudget", typeof(double));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("btype", typeof(int));
        dt.Columns.Add("BItem", typeof(string));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("ScrapFact", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));
        dt.Columns.Add("Actual", typeof(double));
        dt.Columns.Add("jPercent", typeof(double));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("currency", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("TotalPrice", typeof(double));

        /* 
         dt.Columns.Add("Line", typeof(int));
         dt.Columns.Add("jcode", typeof(string));
         dt.Columns.Add("btype", typeof(int));
         dt.Columns.Add("BItem", typeof(string));
         dt.Columns.Add("fdesc", typeof(string));
         dt.Columns.Add("Vendor", typeof(string));
         dt.Columns.Add("currency", typeof(string));
         dt.Columns.Add("QtyReq", typeof(double));
         dt.Columns.Add("UM", typeof(string));
         dt.Columns.Add("BudgetUnit", typeof(double));
         dt.Columns.Add("BudgetExt", typeof(double));
         dt.Columns.Add("jPercent", typeof(double));
         dt.Columns.Add("Amount", typeof(double));
         dt.Columns.Add("TotalPrice", typeof(double));*/

        DataRow dr = dt.NewRow();
        dr["Line"] = 0;
        dt.Rows.Add(dr);

        /*DataRow dr1 = dt.NewRow();
        dr1["Line"] = 0;
        dt.Rows.Add(dr1);*/

        gvBOM.DataSource = dt;
        gvBOM.DataBind();


        //this.BindCountryList(ddlCountry);
    }
    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(_objJob);

            DataRow dr = ds.Tables[0].NewRow();
            dr["ID"] = 0;
            dr["Type"] = "Select Type";
            ds.Tables[0].Rows.InsertAt(dr, 0);

            dtBomType = ds.Tables[0];

            dtCurrency = new DataTable();
            dtCurrency.Columns.Add("ID");
            dtCurrency.Columns.Add("Name");


            dtCurrency.Rows.Add("0", "select");
            dtCurrency.Rows.Add("1", "US");
            dtCurrency.Rows.Add("2", "CDN");


            //foreach (GridViewRow row in gvBOM.Rows)
            //{
            //    DropDownList ddlCurrencyList = (DropDownList)row.FindControl("ddlCurrencyEst");
            //    ddlCurrencyList.Items.Add(new ListItem("select", "0"));
            //    ddlCurrencyList.Items.Add(new ListItem("US", "1"));
            //    ddlCurrencyList.Items.Add(new ListItem("CDN", "2"));
            //}


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    
    protected void CurrencyChanged(object sender, EventArgs e)
    {
        DropDownList ddlCurrencyEst = (DropDownList)sender;
        ViewState["ddlCurrencyEst"] = ddlCurrencyEst.SelectedValue;
        //DropDownList ddlCurrencyEstbind = sender as DropDownList;
        //GridViewRow gvr = ddlCurrencyEst.NamingContainer as GridViewRow;
        //TextBox txtBudgetUnit = gvr.FindControl("txtBudgetUnit") as TextBox;
        //DropDownList ddlCurrencyEstbind = gvr.FindControl("ddlCurrencyEstbind") as DropDownList;
        //ddlCurrencyEstbind.SelectedValue = Convert.ToString(ViewState["ddlCurrencyEst"]);
        this.CreateBOMTable();
    }

    private void SetExchange()
    {

        if (Session["ExchangeRate"] == null)
        {
            //webservicex_Currency.CurrencyConvertor crn = new webservicex_Currency.CurrencyConvertor();
            //double rate = crn.ConversionRate(webservicex_Currency.Currency.CAD, webservicex_Currency.Currency.USD);

            ExchanceRate dsV = new ExchanceRate();
            dsV = parseWebXML(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            double USD = 0;
            double CAD = 0;
            foreach (DataRow dr in dsV.Tables[1].Rows)
            {
                if (dr["name"].ToString().Trim().ToUpper() == "USD")
                {
                    USD = Convert.ToDouble(dr["rate"]);
                }
                if (dr["name"].ToString().Trim().ToUpper() == "CAD")
                {
                    CAD = Convert.ToDouble(dr["rate"]);
                }
            }

            double rate = 0;
            rate = (1 / CAD) * USD;
            rate = Math.Round(rate, 2);

            Session["ExchangeRate"] = rate.ToString();
        }
        hdnExchangeRate.Value = Session["ExchangeRate"].ToString();
    }

    private void GetTemplate(bool edit)
    {
        if (Request.QueryString["uid"] != null)
        {
            Label13.Text = "Edit Estimate";

            // btnnewlab.Visible = false;
            ViewState["edit"] = 1;
            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            ds = objBL_Customer.getEstimateTemplateByID(objProp_Customer);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["job"].ToString() != "")
                {
                    trProj.Visible = true;
                    lnkProject.Text = "# " + ds.Tables[0].Rows[0]["job"].ToString();
                    lnkProject.NavigateUrl = "addproject.aspx?uid=" + ds.Tables[0].Rows[0]["job"].ToString();
                }
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtCont.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                hdnOwnerID.Value = ds.Tables[0].Rows[0]["locid"].ToString();
                hdnROLId.Value = ds.Tables[0].Rows[0]["rolid"].ToString();
                if (ds.Tables[4].Rows.Count > 0)
                {
                    ddlTemplate.SelectedValue = ds.Tables[4].Rows[0]["id"].ToString();
                }
                if (ds.Tables[0].Rows[0]["status"].ToString() == "4")
                {
                    ddlStatus.Items[4].Enabled = true;
                    ddlStatus.Items[5].Enabled = true;
                }
                ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                if (ddlStatus.SelectedValue == "4")
                    ddlStatus.Enabled = false;

                DataTable dtitems = ds.Tables[1].Copy();
                DataSet dsLabor = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                if (edit)
                {
                    SetExchange();
                    dsLabor = (DataSet)ViewState["labor"];
                    //gvTemplateItems.Enabled = true;
                    //gvTemplateItemsPercentage.Enabled = true;
                    //   ddlBucket.Enabled = true;
                    ddlTemplate.Enabled = true;
                    // btnEdit.Visible = false;
                }
                else
                {
                    hdnExchangeRate.Value = ds.Tables[0].Rows[0]["cadexchange"].ToString();
                    dsLabor = objBL_Customer.GetEstimateLaborForEstimate(objProp_Customer);
                    // gvTemplateItems.Enabled = false;
                    //gvTemplateItemsPercentage.Enabled = false;
                    //  ddlBucket.Enabled = false;
                    ddlTemplate.Enabled = false;
                    //btnEdit.Visible = true;
                }
                //DataSet dsLabor = (DataSet)ViewState["labor"];
                ////DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
                foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                {
                    dtitems.Columns.Add(drLabor["item"].ToString(), typeof(double));
                }

                foreach (DataRow drl in dtitems.Rows)
                {
                    foreach (DataRow drldata in ds.Tables[2].Rows)
                    {
                        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(drLabor["ID"].ToString()) == Convert.ToInt32(drldata["labourID"].ToString()) && Convert.ToUInt32(drl["line"].ToString()) == Convert.ToUInt32(drldata["line"].ToString()))
                                drl[drLabor["item"].ToString()] = drldata["amount"];
                        }
                    }
                }



                var drselect = from myRow in dtitems.AsEnumerable()
                               where myRow.Field<short>("Measure") != 3
                               select myRow;

                DataTable dtselect = new DataTable();
                if (drselect.Count() > 0)
                    dtselect = drselect.CopyToDataTable<DataRow>();

                var drselectPercent = from myRow in dtitems.AsEnumerable()
                                      where myRow.Field<short>("Measure") == 3
                                      select myRow;
                DataTable dtselectPercent = new DataTable();
                if (drselectPercent.Count() > 0)
                    dtselectPercent = drselectPercent.CopyToDataTable<DataRow>();

                //  gvTemplateItems.DataSource = dtselect;
                // gvTemplateItems.DataBind();

                //  AddColumns(dsLabor);
                // gvTemplateItems.DataBind();

                // gvTemplateItemsPercentage.DataSource = dtselectPercent;
                // gvTemplateItemsPercentage.DataBind();
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvMilestones.DataSource = ds.Tables[3];
                    gvMilestones.DataBind();
                    ViewState["TempMilestone"] = ds.Tables[3];
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    gvBOM.DataSource = ds.Tables[5];
                    gvBOM.DataBind();
                    ViewState["TempBom"] = ds.Tables[5];
                }
            }
        }
    }

    private void CreateTable()
    {
        //Session["esttempltable"] = null;

        DataTable dt = new DataTable();
        dt.Columns.Add("Scope", typeof(string));
        dt.Columns.Add("vendor", typeof(string));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("cost", typeof(string));
        dt.Columns.Add("currency", typeof(string));
        dt.Columns.Add("amount", typeof(double));

        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("measure", typeof(int));

        //objProp_Customer.ConnConfig = Session["config"].ToString();
        //DataSet dsLabor = new DataSet();

        ////if (Convert.ToInt16(ViewState["edit"]) == 1)
        ////{
        ////    objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        ////    dsLabor = objBL_Customer.GetEstimateLaborForEstimate(objProp_Customer);
        ////}
        ////else
        ////{
        //    dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
        ////}

        //ViewState["labor"] = dsLabor;

        //DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
        DataSet dsLabor = (DataSet)ViewState["labor"];
        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        {
            dt.Columns.Add(drLabor["item"].ToString(), typeof(double));
        }


        DataRow dr = dt.NewRow();
        dr["Scope"] = DBNull.Value;
        dr["vendor"] = DBNull.Value;
        //dr["Quantity"] = 0;
        //dr["cost"] = 0;
        dr["currency"] = DBNull.Value;
        //dr["amount"] = 0;
        //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //{
        //    dr[drLabor["item"].ToString()] = 0;
        //}
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Scope"] = DBNull.Value;
        dr1["vendor"] = DBNull.Value;
        //dr1["Quantity"] = 0;
        //dr1["cost"] = 0;
        dr1["currency"] = DBNull.Value;
        //dr1["amount"] = 0;
        //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //{
        //    dr1[drLabor["item"].ToString()] = 0;
        //}
        dt.Rows.Add(dr1);

        //Session["esttempltable"] = dt;
        //return dt;
        //BindGrid(dt);
        //BindGridPerc(dt);
    }

    private void BindGrid(DataTable dt)
    {
        // gvTemplateItems.DataSource = dt;
        // gvTemplateItems.DataBind();

        //   AddColumns(null);
        //gvTemplateItems.DataBind();
    }

    private void BindGridPerc(DataTable dt)
    {
        // gvTemplateItemsPercentage.DataSource = dt;
        //gvTemplateItemsPercentage.DataBind();

        //AddColumns();
        //gvTemplateItemsPercentage.DataBind();
    }

    private void AddColumns(DataSet dsLabor)
    {
        //    objProp_Customer.ConnConfig = Session["config"].ToString();
        //    //DataSet dsLabor = new DataSet();
        //    //dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
        //    if (dsLabor == null)
        //        dsLabor = (DataSet)ViewState["labor"];

        //    if (!IsPostBack)
        //    {
        //        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //        {
        //            TemplateField temp1 = new TemplateField();
        //            temp1.HeaderText = drLabor["item"].ToString() + ":" + drLabor["amount"].ToString();

        //            DynamicTemplateField dyntmp = new DynamicTemplateField(drLabor["item"].ToString(), gvTemplateItems.ClientID, gvTemplateItemsPercentage.ClientID);
        //            temp1.ItemTemplate = dyntmp;
        //            DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(drLabor["item"].ToString(), drLabor["amount"].ToString(), Convert.ToInt32(drLabor["ID"].ToString()));
        //            temp1.FooterTemplate = dyntmpfooter;

        //            gvTemplateItems.Columns.Add(temp1);
        //            //gvTemplateItems.Columns.Insert(10,temp1);
        //        }

        //        TemplateField tempLT = new TemplateField();
        //        tempLT.HeaderText = "Labor Total";
        //        DynamicTemplateFieldTotal dyntmpLB = new DynamicTemplateFieldTotal("LaborTotal");
        //        tempLT.ItemTemplate = dyntmpLB;
        //        DynamicTemplateFieldFooter dyntmpfooterLB = new DynamicTemplateFieldFooter("LaborTotal", "0", 0);
        //        tempLT.FooterTemplate = dyntmpfooterLB;
        //        gvTemplateItems.Columns.Add(tempLT);

        //        TemplateField tempGT = new TemplateField();
        //        tempGT.HeaderText = "Grand Total";
        //        DynamicTemplateFieldTotal dyntmpGT = new DynamicTemplateFieldTotal("GrandTotal");
        //        tempGT.ItemTemplate = dyntmpGT;
        //        DynamicTemplateFieldFooter dyntmpfooterGT = new DynamicTemplateFieldFooter("GrandTotal", "0", 0);
        //        tempGT.FooterTemplate = dyntmpfooterGT;
        //        gvTemplateItems.Columns.Add(tempGT);
        //    }
        //    else
        //    {
        //        List<TemplateField> strcol = new List<TemplateField>();
        //        List<DataRow> rowlab = new List<DataRow>();
        //        List<DataRow> rowlabgrid = new List<DataRow>();
        //        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //        {
        //            rowlab.Add(drLabor);
        //            int dtfields = 0;
        //            foreach (TemplateField tf in gvTemplateItems.Columns)
        //            {
        //                if (dtfields > 9)
        //                {
        //                    if (tf.HeaderText.Split(':')[0].ToString() == drLabor["item"].ToString()) // + " " + drLabor["amount"].ToString())
        //                    {
        //                        tf.HeaderText = drLabor["item"].ToString() + ":" + drLabor["amount"].ToString();
        //                        DynamicTemplateField dyntmp = new DynamicTemplateField(drLabor["item"].ToString(), gvTemplateItems.ClientID, gvTemplateItemsPercentage.ClientID);
        //                        tf.ItemTemplate = dyntmp;
        //                        DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(drLabor["item"].ToString(), drLabor["amount"].ToString(), Convert.ToInt32(drLabor["ID"].ToString()));
        //                        tf.FooterTemplate = dyntmpfooter;

        //                        strcol.Add(tf);
        //                        rowlabgrid.Add(drLabor);
        //                    }

        //                    if (tf.HeaderText == "Labor Total")
        //                    {
        //                        tf.HeaderText = "Labor Total";
        //                        DynamicTemplateFieldTotal dyntmp = new DynamicTemplateFieldTotal("LaborTotal");
        //                        tf.ItemTemplate = dyntmp;
        //                        DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter("LaborTotal", "0", 0);
        //                        tf.FooterTemplate = dyntmpfooter;

        //                        strcol.Add(tf);
        //                        rowlabgrid.Add(drLabor);
        //                    }

        //                    if (tf.HeaderText == "Grand Total")
        //                    {
        //                        tf.HeaderText = "Grand Total";
        //                        DynamicTemplateFieldTotal dyntmp = new DynamicTemplateFieldTotal("GrandTotal");
        //                        tf.ItemTemplate = dyntmp;
        //                        DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter("GrandTotal", "0", 0);
        //                        tf.FooterTemplate = dyntmpfooter;

        //                        strcol.Add(tf);
        //                        rowlabgrid.Add(drLabor);
        //                    }

        //                }
        //                dtfields++;
        //            }
        //        }

        //        int dtfields1 = 0;
        //        List<TemplateField> strcol1 = new List<TemplateField>();
        //        foreach (TemplateField tf in gvTemplateItems.Columns)
        //        {
        //            if (dtfields1 > 9)
        //            {
        //                strcol1.Add(tf);
        //            }
        //            dtfields1++;
        //        }

        //        foreach (TemplateField s in strcol)
        //        {
        //            strcol1.Remove(s);
        //        }

        //        foreach (TemplateField s in strcol1)
        //        {
        //            //gvTemplateItems.Columns.Remove(s);
        //        }

        //        foreach (DataRow d in rowlabgrid)
        //        {
        //            rowlab.Remove(d);
        //        }
        //        foreach (DataRow d in rowlab)
        //        {
        //            TemplateField temp1 = new TemplateField();
        //            temp1.HeaderText = d["item"].ToString() + ":" + d["amount"].ToString();

        //            DynamicTemplateField dyntmp = new DynamicTemplateField(d["item"].ToString(), gvTemplateItems.ClientID, gvTemplateItemsPercentage.ClientID);
        //            temp1.ItemTemplate = dyntmp;
        //            DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(d["item"].ToString(), d["amount"].ToString(), Convert.ToInt32(d["ID"].ToString()));
        //            temp1.FooterTemplate = dyntmpfooter;
        //            gvTemplateItems.Columns.Add(temp1);
        //            //gvTemplateItems.Columns.Insert(10, temp1);
        //        }
        //        //TemplateField tempLT = new TemplateField();
        //        //tempLT.HeaderText = "Labor Total";
        //        //DynamicTemplateFieldTotal dyntmpLB = new DynamicTemplateFieldTotal("LaborTotal");
        //        //tempLT.ItemTemplate = dyntmpLB;
        //        //DynamicTemplateFieldFooter dyntmpfooterLB = new DynamicTemplateFieldFooter("LaborTotal", "0", 0);
        //        //tempLT.FooterTemplate = dyntmpfooterLB;
        //        //gvTemplateItems.Columns.Add(tempLT);

        //        //TemplateField tempGT = new TemplateField();
        //        //tempGT.HeaderText = "Grand Total";
        //        //DynamicTemplateFieldTotal dyntmpGT = new DynamicTemplateFieldTotal("GrandTotal");
        //        //tempGT.ItemTemplate = dyntmpGT;
        //        //DynamicTemplateFieldFooter dyntmpfooterGT = new DynamicTemplateFieldFooter("GrandTotal", "0", 0);
        //        //tempGT.FooterTemplate = dyntmpfooterGT;
        //        //gvTemplateItems.Columns.Add(tempGT);
        //    }
    }

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.Name = txtName.Text.Trim();
            objProp_Customer.Description = txtREPdesc.Text.Trim();
            objProp_Customer.Remarks = txtREPremarks.Text.Trim();
            objProp_Customer.ROL = Convert.ToInt32(hdnROLId.Value);
            objProp_Customer.LocID = Convert.ToInt32(hdnOwnerID.Value);
            objProp_Customer.CADExchange = Convert.ToDouble(hdnExchangeRate.Value);
            //  objProp_Customer.IsItemEdited = Convert.ToInt16(gvTemplateItems.Enabled);
            objProp_Customer.Status = Convert.ToInt16(ddlStatus.SelectedValue);
            objProp_Customer.date =Convert.ToDateTime(TxtDate.Text);
            objProp_Customer.estimateno =Convert.ToInt32( TxtEstimateNo.Text);
            objProp_Customer.Contact = txtCont.Text;
            objProp_Customer.type = txtJobType.Text;

            DataTable dtBomEstimateItems = GetBOMGridItems();


            DataSet dtLaborEst = (DataSet)ViewState["labor"];
            objProp_Customer.dtLaborItemsEstimate = dtLaborEst.Tables[0];
            DataTable dtM = GetMilestoneGridItems();
            dtM.Columns.Remove("Department");

            int line = 1;

            if (ViewState["Line"] == null)
            {
                dtBomEstimateItems.AsEnumerable().ToList()
                            .ForEach(t => t["Line"] = line++);
                dtBomEstimateItems.AcceptChanges();

                dtM.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = line++);
                dtM.AcceptChanges();
            }
            else
            {
                line = (Int16)ViewState["Line"];
                line++;
                dtBomEstimateItems.Select("Line = 0")
                            .AsEnumerable().ToList()
                            .ForEach(t => t["Line"] = line++);
                dtBomEstimateItems.AcceptChanges();
                dtM.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = line++);
                dtM.AcceptChanges();
            }

            objProp_Customer.DtMilestone = dtM;
            objProp_Customer._dtBomEstimate = dtBomEstimateItems;
            if (ViewState["edit"].ToString() == "0")
            {
                //dt.Columns.RemoveAt(dt.Columns.Count - 2);
                objProp_Customer.Mode = 0;
                objBL_Customer.AddEstimate(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Estimate Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                IntializeControls();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.Mode = 1;
                objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objBL_Customer.AddEstimate(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Estimate Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter', theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("estimate.aspx");
    }

    private void IntializeControls()
    {
        GeneralFunctions objgn = new GeneralFunctions();
        objgn.ResetFormControlValues(this);
        //  CreateTable();
        //BindGrid();
    }

    private DataSet GetItemsfromGrid()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("Estimate", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("Cost", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Hours", typeof(double));
        dt.Columns.Add("Rate", typeof(double));
        dt.Columns.Add("Labor", typeof(double));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("Currency", typeof(string));

        dt.Columns.Add("measure", typeof(int));

        DataTable dtLaborItems = new DataTable();
        dtLaborItems.Columns.Add("Line", typeof(int));
        dtLaborItems.Columns.Add("LabourID", typeof(int));
        dtLaborItems.Columns.Add("TemplateID", typeof(int));
        dtLaborItems.Columns.Add("Amount", typeof(double));
        //ds.Tables.Add(dt);
        //ds.Tables.Add(dtLaborItems);

        DataSet ds1 = JSONToDatatableGridItems(hdnItemJSON.Value.Trim(), dt, dtLaborItems, 0, true);
        DataSet ds2 = JSONToDatatableGridItems(hdnItemJSONPerc.Value.Trim(), dt, dtLaborItems, ds1.Tables[0].Rows.Count, false);

        ds1.Tables[0].Merge(ds2.Tables[0]);
        ds1.Tables[1].Merge(ds2.Tables[1]);

        dt = ds1.Tables[0].Copy();
        dtLaborItems = ds1.Tables[1].Copy();

        ds.Tables.Add(dt);
        ds.Tables.Add(dtLaborItems);

        return ds;

        //string strItems = hdnItemJSON.Value.Trim();
        //if (strItems != string.Empty)
        //{
        //    objProp_Customer.ConnConfig = Session["config"].ToString();
        //    DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);

        //    JavaScriptSerializer sr = new JavaScriptSerializer();
        //    List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
        //    objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
        //    //List<EstimateItemData> objEstimateItemData = new List<EstimateItemData>();
        //    //objEstimateItemData = sr.Deserialize<List<EstimateItemData>>(strItems);
        //    int i = 0;
        //    //foreach (EstimateItemData eid in objEstimateItemData)
        //    foreach (Dictionary<object, object> dict in objEstimateItemData)
        //    {
        //        if (dict["txtScope"].ToString().Trim() != string.Empty)// || dict["txtUnitCost"].ToString().Trim() == string.Empty|| dict["txtQuan"].ToString().Trim() == string.Empty ||  dict["txtTotal"].ToString().Trim() == string.Empty
        //        {
        //            i++;
        //            DataRow dr = dt.NewRow();
        //            dr["Estimate"] = 0;
        //            dr["Line"] = i;
        //            dr["fDesc"] = dict["txtScope"].ToString().Trim();

        //            if (dict["txtQuan"].ToString().Trim() != string.Empty)
        //                dr["Quan"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

        //            if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
        //                dr["Cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

        //            if (dict["txtTotal"].ToString().Trim() != string.Empty)
        //                dr["Price"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

        //            dr["Hours"] = 0;
        //            dr["Rate"] = 0;
        //            dr["Labor"] = 0;

        //            if (dict["txtTotal"].ToString().Trim() != string.Empty)
        //                dr["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

        //            dr["STax"] = 0;
        //            dr["code"] = string.Empty;
        //            dr["Vendor"] = dict["txtVendor"].ToString().Trim();
        //            dr["Currency"] = dict["ddlCurrency"].ToString().Trim();
        //            dt.Rows.Add(dr);

        //            foreach (DataRow drlab in dsLabor.Tables[0].Rows)
        //            {
        //                if (dict["txt" + drlab["Item"]].ToString().Trim() != string.Empty)
        //                {
        //                    DataRow drLaborItems = dtLaborItems.NewRow();
        //                    drLaborItems["Line"] = i;
        //                    drLaborItems["LabourID"] = drlab["ID"];
        //                    drLaborItems["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drlab["Item"]].ToString(), "0"));
        //                    dtLaborItems.Rows.Add(drLaborItems);
        //                }
        //            }
        //        }
        //    }
        //}

        //return ds;
    }

    private DataSet JSONToDatatableGridItems(string strItems, DataTable dtnew, DataTable dtLaborItemsNew, int i, bool includeLaborItems)
    {
        DataTable dt = dtnew.Copy();
        DataTable dtLaborItems = dtLaborItemsNew.Copy();

        DataSet ds = new DataSet();
        //string strItems = hdnItemJSON.Value.Trim();
        if (strItems != string.Empty)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            //DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
            DataSet dsLabor = (DataSet)ViewState["labor"];

            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            //List<EstimateItemData> objEstimateItemData = new List<EstimateItemData>();
            //objEstimateItemData = sr.Deserialize<List<EstimateItemData>>(strItems);
            //int i = 0;
            //foreach (EstimateItemData eid in objEstimateItemData)
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() != string.Empty)// || dict["txtUnitCost"].ToString().Trim() == string.Empty|| dict["txtQuan"].ToString().Trim() == string.Empty ||  dict["txtTotal"].ToString().Trim() == string.Empty
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Estimate"] = 0;
                    dr["Line"] = i;
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();

                    if (dict["txtQuan"].ToString().Trim() != string.Empty)
                        dr["Quan"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

                    if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
                        dr["Cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

                    if (dict["txtTotal"].ToString().Trim() != string.Empty)
                        dr["Price"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                    dr["Hours"] = 0;
                    dr["Rate"] = 0;
                    dr["Labor"] = 0;

                    if (dict["txtTotal"].ToString().Trim() != string.Empty)
                        dr["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                    dr["STax"] = 0;
                    dr["code"] = string.Empty;
                    dr["Vendor"] = dict["txtVendor"].ToString().Trim();
                    dr["Currency"] = dict["ddlCurrency"].ToString().Trim();
                    dr["code"] = dict["txtCode"];
                    if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                        dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                    dt.Rows.Add(dr);

                    if (includeLaborItems)
                    {
                        foreach (DataRow drlab in dsLabor.Tables[0].Rows)
                        {
                            if (dict["txt" + drlab["Item"]].ToString().Trim() != string.Empty)
                            {
                                DataRow drLaborItems = dtLaborItems.NewRow();
                                drLaborItems["Line"] = i;
                                drLaborItems["LabourID"] = drlab["ID"];
                                drLaborItems["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drlab["Item"]].ToString(), "0"));
                                dtLaborItems.Rows.Add(drLaborItems);
                            }
                        }
                    }
                }
            }
        }

        ds.Tables.Add(dt);
        ds.Tables.Add(dtLaborItems);

        return ds;
    }

    private DataSet RestoreGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Scope", typeof(string));
        dt.Columns.Add("vendor", typeof(string));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("cost", typeof(string));
        dt.Columns.Add("currency", typeof(string));
        dt.Columns.Add("amount", typeof(double));

        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("measure", typeof(int));

        objProp_Customer.ConnConfig = Session["config"].ToString();
        //DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
        DataSet dsLabor = (DataSet)ViewState["labor"];
        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        {
            dt.Columns.Add(drLabor["item"].ToString(), typeof(double));
        }

        DataTable dtPerc = dt.Clone();
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        ds.Tables.Add(dtPerc);

        dt = JSONtoTableRestore(hdnItemJSON.Value.Trim(), dt, dsLabor, true);
        dtPerc = JSONtoTableRestore(hdnItemJSONPerc.Value.Trim(), dtPerc, dsLabor, false);

        return ds;

        //string strItems = hdnItemJSON.Value.Trim();
        //if (strItems != string.Empty)
        //{
        //    JavaScriptSerializer sr = new JavaScriptSerializer();
        //    List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
        //    objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
        //    int i = 0;
        //    foreach (Dictionary<object, object> dict in objEstimateItemData)
        //    {
        //        i++;
        //        DataRow dr = dt.NewRow();
        //        dr["Scope"] = dict["txtScope"];
        //        dr["vendor"] = dict["txtVendor"];

        //        if (dict["txtQuan"].ToString().Trim() != string.Empty)
        //            dr["Quantity"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

        //        if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
        //            dr["cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

        //        dr["currency"] = dict["ddlCurrency"];

        //        if (dict["txtTotal"].ToString().Trim() != string.Empty)
        //            dr["amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

        //        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //        {
        //            if (dict.ContainsKey("txt" + drLabor["item"].ToString()))
        //            {
        //                if (dict["txt" + drLabor["item"].ToString()].ToString().Trim() != string.Empty)
        //                    dr[drLabor["item"].ToString()] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drLabor["item"].ToString()].ToString(), "0"));
        //            }
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //}
        //return dt;
    }

    private DataTable JSONtoTableRestore(string strItems, DataTable dt, DataSet dsLabor, bool includeLaborItems)
    {
        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["Scope"] = dict["txtScope"];
                dr["vendor"] = dict["txtVendor"];
                dr["code"] = dict["txtCode"];
                if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                    dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                if (dict["txtQuan"].ToString().Trim() != string.Empty)
                    dr["Quantity"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

                if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
                    dr["cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

                dr["currency"] = dict["ddlCurrency"];

                if (dict["txtTotal"].ToString().Trim() != string.Empty)
                    dr["amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                if (includeLaborItems)
                {
                    foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                    {
                        if (dict.ContainsKey("txt" + drLabor["item"].ToString()))
                        {
                            if (dict["txt" + drLabor["item"].ToString()].ToString().Trim() != string.Empty)
                                dr[drLabor["item"].ToString()] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drLabor["item"].ToString()].ToString(), "0"));
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    //protected void gvTemplateItems_DataBound(object sender, EventArgs e)
    //{
    //    FillBucketDropdown();
    //    FillTemplateDropdown();
    //}
    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        try
        {
            string strItems = hdnMilestone.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnLine"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    dr["MilesName"] = dict["txtName"].ToString().Trim();
                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty)
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetMilestoneItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));

        try
        {
            string strItems = hdnMilestone.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Line"] = dict["hdnLine"].ToString().Trim();
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);
                    dr["MilesName"] = dict["txtName"].ToString().Trim();
                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty)
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    protected void gvMilestones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddMilestoneItem"))
        {
            int rowIndex = gvMilestones.Rows.Count - 1;
            GridViewRow row = gvMilestones.Rows[rowIndex];
            HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

            DataTable dt = GetMilestoneGridItems();
            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Line"] = 0;
                    dt.Rows.Add(dr);
                }
            }

            DataRow dr2 = dt.NewRow();
            dr2["Line"] = 0;
            dt.Rows.Add(dr2);

            ViewState["TempMilestone"] = dt;
            gvMilestones.DataSource = dt;
            gvMilestones.DataBind();

        }
    }
    /*
    protected void btnInsertBuck_Click(object sender, EventArgs e)
    {
        //DropDownList ddlBucket = gvTemplateItems.FooterRow.FindControl("ddlBucket") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.BucketID = Convert.ToInt32(ddlBucket.SelectedValue);
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getEstimateBucketItems(objProp_Customer);

        DataSet dsGrid = RestoreGrid();
        DataTable dt = dsGrid.Tables[0];
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["measure"].ToString() != "3")
            {
                DataRow drItems = dt.NewRow();
                drItems["Scope"] = dr["item"];
                drItems["vendor"] = dr["vendor"];
                drItems["Quantity"] = dr["unit"];
                drItems["cost"] = dr["cost"];
                drItems["currency"] = string.Empty;
                drItems["measure"] = dr["measure"];
                drItems["code"] = dr["code"];
                //drItems["amount"] = 0;
                //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                //{
                //    drItems[drLabor["item"].ToString()] = 0;
                //}
                dt.Rows.Add(drItems);
            }
        }
        // BindGrid(dt);

        if (dsGrid.Tables.Count > 1)
        {
            DataTable dtPerc = dsGrid.Tables[1];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["measure"].ToString() == "3")
                {
                    DataRow drItems = dtPerc.NewRow();
                    drItems["Scope"] = dr["item"];
                    drItems["vendor"] = dr["vendor"];
                    drItems["Quantity"] = dr["unit"];
                    drItems["cost"] = dr["cost"];
                    drItems["currency"] = string.Empty;
                    drItems["measure"] = dr["measure"];
                    drItems["code"] = dr["code"];
                    //drItems["amount"] = 0;
                    //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                    //{
                    //    drItems[drLabor["item"].ToString()] = 0;
                    //}
                    dtPerc.Rows.Add(drItems);
                }
            }
            BindGridPerc(dtPerc);
        }

        //DataTable dt = RestoreGrid();
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    DataRow drItems = dt.NewRow();
        //    drItems["Scope"] = dr["item"];
        //    drItems["vendor"] = dr["vendor"];
        //    //drItems["Quantity"] = 0;
        //    //drItems["cost"] = 0;
        //    drItems["currency"] = string.Empty;
        //    //drItems["amount"] = 0;
        //    //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //    //{
        //    //    drItems[drLabor["item"].ToString()] = 0;
        //    //}
        //    dt.Rows.Add(drItems);
        //}
        //BindGrid(dt);
    }
    */
    /*
    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        DataSet ds = RestoreGrid();
        FillBucketDropdown();
        // BindGrid(ds.Tables[0]);
        //if (ds.Tables.Count > 1)
        //    BindGridPerc(ds.Tables[1]);
        ModalPopupExtender1.Hide();
        ModalPopupExtender2.Hide();
    }
    */
    /*
    private void FillBucketDropdown()
    {
        //DropDownList ddlBucket = gvTemplateItems.FooterRow.FindControl("ddlBucket") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getEstimateBucket(objProp_Customer);
        ddlBucket.DataSource = ds.Tables[0];
        ddlBucket.DataTextField = "name";
        ddlBucket.DataValueField = "id";
        ddlBucket.DataBind();

        ddlBucket.Items.Insert(0, new ListItem("-- Select Bucket --", "0"));
        ddlBucket.Items.Insert(1, new ListItem("-- Add New --", "0"));
    }
    */
    private void FillTemplateDropdown()
    {
        //DropDownList ddlTemplate = gvTemplateItems.FooterRow.FindControl("ddlTemplate") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProjectTemplate(objProp_Customer);
        //ds = objBL_Customer.getEstimateTemplate(objProp_Customer);
        ddlTemplate.DataSource = ds.Tables[0];
        ddlTemplate.DataTextField = "fDesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();

        ddlTemplate.Items.Insert(0, new ListItem("-- Select Template --", "0"));
    }

    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlTemplate = gvTemplateItems.FooterRow.FindControl("ddlTemplate") as DropDownList;
        //DataTable dt = RestoreGrid();
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProjectJobID = Convert.ToInt32(ddlTemplate.SelectedValue);
        //ds = objBL_Customer.getEstimateTemplateByID(objProp_Customer);
        ds = objBL_Customer.getJobTemplateByID(objProp_Customer);
        if (ds.Tables.Count > 1)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                ds.Tables[1].Columns.Add("Vendor");
                ds.Tables[1].Columns.Add("currency");
                ds.Tables[1].Columns.Add("jPercent");
                ds.Tables[1].Columns.Add("Amount");
                ds.Tables[1].Columns.Add("TotalPrice");
                gvBOM.DataSource = ds.Tables[1];
                gvBOM.DataBind();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                gvMilestones.DataSource = ds.Tables[2];
                gvMilestones.DataBind();
                ViewState["TempMilestone"] = ds.Tables[2];
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                txtJobType.Text = ds.Tables[5].Rows[0]["Type"].ToString();

            }
            /*
            DataTable dtitems = ds.Tables[1].Copy();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            //DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
            DataSet dsLabor = (DataSet)ViewState["labor"];
            foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
            {
                dtitems.Columns.Add(drLabor["item"].ToString(), typeof(double));
            }
            foreach (DataRow drl in dtitems.Rows)
            {
                foreach (DataRow drldata in ds.Tables[2].Rows)
                {
                    foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(drLabor["ID"].ToString()) == Convert.ToInt32(drldata["labourID"].ToString()) && Convert.ToUInt32(drl["line"].ToString()) == Convert.ToUInt32(drldata["line"].ToString()))
                            drl[drLabor["item"].ToString()] = drldata["amount"];
                    }
                }
            }

            var drselect = from myRow in dtitems.AsEnumerable()
                           where myRow.Field<short>("Measure") != 3
                           select myRow;

            DataTable dtselect = drselect.CopyToDataTable<DataRow>();

            var drselectPercent = from myRow in dtitems.AsEnumerable()
                                  where myRow.Field<short>("Measure") == 3
                                  select myRow;
            DataTable dtselectPercent = new DataTable();
            if (drselectPercent.Count() > 0)
            {
                dtselectPercent = drselectPercent.CopyToDataTable<DataRow>();
            }

            //gvTemplateItems.DataSource = dtselect;
            //gvTemplateItems.DataBind();

            //gvTemplateItemsPercentage.DataSource = dtselectPercent;
            //gvTemplateItemsPercentage.DataBind();

            DataSet dsGrid = RestoreGrid();
            foreach (DataRow dr in dtselect.Rows)
            {
                dsGrid.Tables[0].ImportRow(dr);
            }
            foreach (DataRow dr in dtselectPercent.Rows)
            {
                dsGrid.Tables[1].ImportRow(dr);
            }
            BindGrid(dsGrid.Tables[0]);
            if (dsGrid.Tables.Count > 1)
                BindGridPerc(dsGrid.Tables[1]);
            //foreach (DataRow dr in dtitems.Rows)
            //{
            //    dt.ImportRow(dr);
            //}
            */
        }
        //BindGrid(dt);
    }
    /*
    protected void btnEdit_Click(object sender, EventArgs e)
    {

        GetTemplate(true);

    }
      */


    private ExchanceRate parseWebXML(string WebAddress)
    {
        XmlTextReader xmlReader;

        ExchanceRate dsVa = new ExchanceRate();

        DataRow newRowVa = null;

        try
        {
            //Read data from the XML-file over the interNET
            xmlReader = new XmlTextReader(WebAddress);
        }
        catch (WebException)
        {
            throw new WebException("Problem Getting ExchangeRates");
        }

        try
        {
            while (xmlReader.Read())
            {
                if (xmlReader.Name != "")
                {
                    //Check that there are node call gesmes:name
                    //if (xmlReader.Name == "gesmes:name")
                    //{
                    //    _author = xmlReader.ReadString();
                    //}

                    for (int i = 0; i < xmlReader.AttributeCount; i++)
                    {
                        //Check that there are node call Cube
                        if (xmlReader.Name == "Cube")
                        {
                            //Check that there are 1 attribut, then get the date
                            if (xmlReader.AttributeCount == 1)
                            {
                                xmlReader.MoveToAttribute("time");

                                DateTime tim = DateTime.Parse(xmlReader.Value);
                                newRowVa = null;
                                DataRow newRowCo = null;

                                newRowVa = dsVa.Exchance.NewRow();
                                newRowVa["Date"] = tim;
                                dsVa.Exchance.Rows.Add(newRowVa);

                                newRowCo = dsVa.Country.NewRow();
                                newRowCo["Initial"] = "EUR";
                                newRowCo["Name"] = "EUR";		// Find Country name from ISO code
                                newRowCo["Rate"] = 1.0;
                                dsVa.Country.Rows.Add(newRowCo);

                                newRowCo.SetParentRow(newRowVa);	// Make Key to subtable
                            }

                            //If the number of attributs are 2, so get the ExchangeRate-node
                            if (xmlReader.AttributeCount == 2)
                            {
                                xmlReader.MoveToAttribute("currency");
                                string cur = xmlReader.Value;

                                xmlReader.MoveToAttribute("rate");
                                decimal rat = decimal.Parse(xmlReader.Value.Replace(".", ",")); // I am using "," as a decimal symbol

                                DataRow newRowCo = null;

                                newRowCo = dsVa.Country.NewRow();
                                newRowCo["Initial"] = cur;
                                newRowCo["Name"] = cur;
                                newRowCo["Rate"] = rat;
                                dsVa.Country.Rows.Add(newRowCo);

                                newRowCo.SetParentRow(newRowVa);
                            }

                            xmlReader.MoveToNextAttribute();
                        }
                    }
                }
            }
        }
        catch (WebException)
        {
            throw new WebException("connections lost");
        }
        return dsVa;
    }
    
    protected void txtPercntge_TextChanged(object sender, EventArgs e)
    {
        TextBox txtPercntge = sender as TextBox;
        GridViewRow gvr = txtPercntge.NamingContainer as GridViewRow;
        TextBox txtAmt = gvr.FindControl("txtAmt") as TextBox;
        Label lblTotalPrice = gvr.FindControl("lblTotalPrice") as Label;
        //Label lblBudgetExt = gvr.FindControl("lblBudgetExt") as Label;
        
        TextBox txtBudgetUnit = gvr.FindControl("txtBudgetUnit") as TextBox;
        int totalamt = Convert.ToInt32((Convert.ToDecimal(txtBudgetUnit.Text) * Convert.ToInt32(txtPercntge.Text)) / 100);
        txtAmt.Text = Convert.ToString(totalamt);
        lblTotalPrice.Text = Convert.ToString(Convert.ToDecimal(txtBudgetUnit.Text) + totalamt);
        txtAmt.Enabled = false;

    }
    protected void txtAmt_TextChanged(object sender, EventArgs e)
    {
        TextBox txtAmt = sender as TextBox;
        GridViewRow gvr = txtAmt.NamingContainer as GridViewRow;
        TextBox txtPercntge = gvr.FindControl("txtPercntge") as TextBox;
        Label lblTotalPrice = gvr.FindControl("lblTotalPrice") as Label;

        TextBox txtBudgetUnit = gvr.FindControl("txtBudgetUnit") as TextBox;
        int percentage = Convert.ToInt32((Convert.ToInt32(txtAmt.Text) * 100) / Convert.ToDecimal(txtBudgetUnit.Text));
        txtPercntge.Text = Convert.ToString(percentage);
        lblTotalPrice.Text = Convert.ToString(Convert.ToDecimal(txtBudgetUnit.Text) + Convert.ToInt32(txtAmt.Text));
        txtPercntge.Enabled = false;
    }
}
//public class DynamicTemplateField : ITemplate
//{
//    string strTextBoxName;
//    public DynamicTemplateField(string name)
//    {
//        this.strTextBoxName = name;
//    }

//    public void InstantiateIn(Control container)
//    {
//        TextBox txt1 = new TextBox();
//        txt1.ID = "txt" + strTextBoxName;
//        txt1.Width = 50;
//        txt1.Text = "0";
//        txt1.Attributes.Add("onkeydown", "NumericValid(event);");
//        txt1.Attributes.Add("onblur", "calculateDynamic('" + strTextBoxName + "');");
//        txt1.DataBinding += new EventHandler(txt1_DataBinding);
//        container.Controls.Add(txt1);
//    }
//    private void txt1_DataBinding(object sender, EventArgs e)
//    {
//        TextBox target = (TextBox)sender;
//        GridViewRow container = (GridViewRow)target.NamingContainer;
//        target.Text = ((DataRowView)container.DataItem)[strTextBoxName].ToString();
//    }
//}
//public class DynamicTemplateFieldFooter : ITemplate
//{
//    string strTextBoxName;
//    string strvalue;
//    public DynamicTemplateFieldFooter(string name, string value)
//    {
//        this.strTextBoxName = name;
//        this.strvalue = value;
//    }

//    public void InstantiateIn(Control container)
//    {
//        TextBox txt1 = new TextBox();
//        txt1.ID = "txt" + strTextBoxName + "T";
//        txt1.Width = 50;
//        txt1.CssClass = "texttransparent";
//        txt1.Attributes.Add("onfocus", "this.blur();");
//        container.Controls.Add(txt1);

//        HiddenField hdn1 = new HiddenField();
//        hdn1.ID = "hdn" + strTextBoxName + "T";
//        hdn1.Value = strvalue;
//        container.Controls.Add(hdn1);
//    }
//}
