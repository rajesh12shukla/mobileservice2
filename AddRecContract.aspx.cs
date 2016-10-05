using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Globalization;
using System.Web.UI.HtmlControls;

public partial class AddRecContract : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    Loc _objLoc = new Loc();
    BL_Customer objBL_Customer = new BL_Customer();

    JobT _objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    string defaultDate = "12/30/1899";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        //hdnCon.Value = Session["config"].ToString();
        txtUnit.Text = hdnUnit.Value;
        if (!IsPostBack)
        {
            rfvGLAcct.Enabled = false;
            FillRoute();
            FillServiceType();
            FillContractBill();
            BindCustomField();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;            

            if (Request.QueryString["uid"] != null)
            {
                pnlNext.Visible = true;

                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Contract";
                }
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.JobId = Convert.ToInt32(Request.QueryString["uid"]);                
                DataSet ds = new DataSet();
                ds = objBL_Contracts.GetContract(objProp_Contracts);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblContrName.Text = ds.Tables[0].Rows[0]["id"].ToString();
                    txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                    hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                    FillLocInfo();
                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["custom20"].ToString();
                    ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    txtBillStartDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BStart"]).ToShortDateString();
                    ddlBillFreq.SelectedValue = ds.Tables[0].Rows[0]["BCycle"].ToString();
                    txtBillAmt.Text = String.Format("{0:C}",Convert.ToDouble( ds.Tables[0].Rows[0]["bamt"]));
                    txtScheduleStartDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["sStart"]).ToShortDateString();
                    ddlSchFreq.SelectedValue = ds.Tables[0].Rows[0]["sCycle"].ToString();                    
                    chkWeekends.Checked = Convert.ToBoolean(Convert.ToInt32( ds.Tables[0].Rows[0]["swe"]));
                    txtDay.Text = ds.Tables[0].Rows[0]["sday"].ToString();
                    ddlDay.SelectedValue = ds.Tables[0].Rows[0]["sdate"].ToString();
                    txtsTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["stime"]).ToShortTimeString().Replace("12:00 AM", "");
                    chkCredit.Checked = Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0]["creditcard"]));
                    txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtBillHours.Text = ds.Tables[0].Rows[0]["hours"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                    ddlServiceType.SelectedValue = ds.Tables[0].Rows[0]["ctype"].ToString();
                    txtUnitExpiration.Text = ds.Tables[0].Rows[0]["ExpirationDate"].ToString();
                    txtNumFreq.Text = ds.Tables[0].Rows[0]["frequencies"].ToString();
                    ddlExpiration.SelectedValue = ds.Tables[0].Rows[0]["Expiration"].ToString();
                    ddlExpiration_SelectedIndexChanged(sender, e);
                    txtPO.Text = ds.Tables[0].Rows[0]["PO"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Billing"].ToString()))     // added by Mayuri 25th dec, 15
                    {                                                                          // Location billing details
                        ddlContractBill.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CustBilling"].ToString())) // Customer billing details
                    {
                        ddlBilling.SelectedValue = ds.Tables[0].Rows[0]["CustBilling"].ToString();
                    }

                    FillSpecifyLocation();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Central"].ToString()))
                    {
                        ddlSpecifiedLocation.SelectedValue = ds.Tables[0].Rows[0]["Central"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Chart"].ToString()))
                    {
                        hdnGLAcct.Value = ds.Tables[0].Rows[0]["Chart"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["GLAcct"].ToString()))
                    {
                        txtGLAcct.Text = ds.Tables[0].Rows[0]["GLAcct"].ToString();
                    }

                    ddlEscType.SelectedValue = ds.Tables[0].Rows[0]["BEscType"].ToString();
                    txtEscCycle.Text = ds.Tables[0].Rows[0]["BEscCycle"].ToString();
                    txtEscFactor.Text = ds.Tables[0].Rows[0]["BEscFact"].ToString();
                    txtEscdue.Text = ( ds.Tables[0].Rows[0]["EscLast"].ToString() != string.Empty) ? Convert.ToDateTime( ds.Tables[0].Rows[0]["EscLast"].ToString()).ToShortDateString() : "";

                    DataSet dsElev = new DataSet();
                    dsElev = objBL_Contracts.GetElevContract(objProp_Contracts);
                    if (dsElev.Tables[0].Rows.Count > 0)
                    {                        
                        foreach (GridViewRow gr in gvEquip.Rows)
                        {
                            Label lblID = (Label)gr.FindControl("lblID");
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
                            TextBox txtHours = (TextBox)gr.FindControl("txtHours");
                            Label lblname = (Label)gr.FindControl("lblUnit");

                            foreach (DataRow dr in dsElev.Tables[0].Rows)
                            {
                                if (lblID.Text == dr["elev"].ToString())
                                {
                                    if (txtUnit.Text != string.Empty)
                                    {
                                        txtUnit.Text = txtUnit.Text + ", " + lblname.Text;
                                    }
                                    else
                                    {
                                        txtUnit.Text = lblname.Text;
                                    }
                                    chkSelect.Checked = true;
                                    txtPrice.Text = String.Format("{0:C}", Convert.ToDouble((dr["price"] != DBNull.Value) ? dr["price"] : 0));
                                    txtHours.Text = dr["hours"].ToString();
                                }
                            }
                        }
                    }
                    txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                    txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                    txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                }
            }
            else
            {
                FillSpecifyLocation();
            }
            
        }
        Permission();
        
    }
    private void BindCustomField()
    {
        try
        {
            DataSet ds = new DataSet();
            
            _objJob.ConnConfig = Session["config"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                _objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString()); 
            }
            ds = objBL_Job.GetRecurringCustom(_objJob);

            gvCustom.DataSource = ds;
            gvCustom.DataBind();

            int jobTId = 0;
            if(ds.Tables[2].Rows.Count > 0)
            {
                jobTId = Convert.ToInt32(ds.Tables[2].Rows[0]["JobT"].ToString());
            }
            
            ViewState["JobT"] = jobTId;
            
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ((Label)gvCustom.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(ds.Tables[0].Rows.Count - 0);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        BindCustomItemGrid(ds);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    private void BindCustomItemGrid(DataSet dsCustom)
    {
        DataTable dtCust = dsCustom.Tables[0];
        DataTable dtValues = dsCustom.Tables[1];
        foreach (GridViewRow gr in gvCustom.Rows)
        {
            Label lblFormat = (Label)gr.FindControl("lblFormat");
            if (lblFormat.Text == "Dropdown")
            {
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlFormat");
                ddlCustomValue.Visible = true;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblValue = (Label)gr.FindControl("lblValue");

                DataTable dt = dtValues.Clone();
                //ItemID = " + Convert.ToInt32(lblID.Text) + " AND
                if(dtValues.Rows.Count > 0)
                {
                    DataRow[] result = dtValues.Select("Line = " + Convert.ToInt32(lblIndex.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }
                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                }
               
                ddlCustomValue.Items.Insert(0, (new ListItem("", "")));

                if (ddlCustomValue.Items.Contains(new ListItem(lblValue.Text, lblValue.Text)))
                {
                    ddlCustomValue.SelectedValue = lblValue.Text;
                }
                else
                {
                    ddlCustomValue.Items.Add(new ListItem(lblValue.Text, lblValue.Text));
                    ddlCustomValue.SelectedValue = lblValue.Text;
                }
            }
            else if (lblFormat.Text == "Checkbox")
            {
                CheckBox chkValue = (CheckBox)gr.FindControl("chkValue");
                chkValue.Visible = true;
            }
            else
            {
                TextBox txtValue = (TextBox)gr.FindControl("txtValue");
                txtValue.Visible = true;
            }
        }
    }
    private DataTable GetCustomTemplate()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("tblTabID", typeof(int));
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("Line", typeof(Int16));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Format", typeof(Int16));

            foreach (GridViewRow gr in gvCustom.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = Convert.ToInt32(((Label)gr.FindControl("lblID")).Text);
                dr["tblTabID"] = 0;
                dr["Label"] = ((Label)gr.FindControl("lblDesc")).Text;
                dr["Line"] = dt.Rows.Count + 1;
                if (((Label)gr.FindControl("lblFormat")).Text == "Dropdown")
                    dr["value"] = ((DropDownList)gr.FindControl("ddlFormat")).Text.Trim();
                else if(((Label)gr.FindControl("lblFormat")).Text == "Checkbox")
                {
                    CheckBox chk = ((CheckBox)gr.FindControl("chkValue"));
                    if (chk.Checked.Equals(true))
                    {
                        dr["value"] = '1';
                    }
                    else 
                    {
                        dr["value"] = '0';
                    }   
                }
                else
                    dr["value"] = ((TextBox)gr.FindControl("txtValue")).Text.Trim();
                dr["Format"] = ((Label)gr.FindControl("lblFormatID")).Text;
                dt.Rows.Add(dr);
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
            TextBox txtHours = (TextBox)gr.FindControl("txtHours");

            chkSelect.Attributes["onclick"] = "SelectRows('" + gvEquip.ClientID + "','" + txtUnit.ClientID + "','" + hdnUnit.ClientID + "'); CalculateAmount(); CalculateHours();";
            txtPrice.Attributes["onblur"] = "$('#" + txtPrice.ClientID + "').formatCurrency(); CalculateAmount();";
            txtHours.Attributes["onblur"] = "CalculateHours();";
        }
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cntractsMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkContract");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkContractsMenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderRecr");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("recurMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");
        
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");

        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    private void FillServiceType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getServiceType(objPropUser);

        ddlServiceType.DataSource = ds.Tables[0];
        ddlServiceType.DataTextField = "type";
        ddlServiceType.DataValueField = "type";
        ddlServiceType.DataBind();

        ddlServiceType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    public void GetJstatus()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        ds = objBL_Contracts.getJstatus(objProp_Contracts);
        ddlStatus.DataSource = ds.Tables[0];
        ddlStatus.DataTextField = "status";
        ddlStatus.DataValueField = "status";
        ddlStatus.DataBind();
    }

    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        objPropUser.InstallDate = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.Status = -1;
        ds = objBL_User.getElev(objPropUser);
        gvEquip.DataSource = ds.Tables[0];
        gvEquip.DataBind();
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

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlRoute.Items.Insert(1, new ListItem("Unassigned", "0"));
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ContrSrch"];
        Response.Redirect("addreccontract.aspx?uid=" + dt.Rows[0]["job"]);
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["ContrSrch"];
        //DataColumn[] keyColumns = new DataColumn[1];
        //keyColumns[0] = dt.Columns["job"];
        //dt.PrimaryKey = keyColumns;

        //DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        //int index = dt.Rows.IndexOf(d);

        //if (index > 0)
        //{
        //    Response.Redirect("addreccontract.aspx?uid=" + dt.Rows[index - 1]["job"]);
        //}
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ContrSrch"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["job"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addreccontract.aspx?uid=" + dt.Rows[index + 1]["job"]);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ContrSrch"];
        Response.Redirect("addreccontract.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["job"]);     

    }

    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ElevUnit", typeof(int));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Hours", typeof(double));

        foreach (GridViewRow gvr in gvEquip.Rows)
        {            
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
            
            if (chkSelect.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblUnit = (Label)gvr.FindControl("lblID");
                TextBox txtPrice = (TextBox)gvr.FindControl("txtPrice");
                TextBox txtHours = (TextBox)gvr.FindControl("txtHours");
                dr["ElevUnit"] = Convert.ToInt32(lblUnit.Text);
                if (txtPrice.Text.Trim() != string.Empty)
                {
                    dr["Price"] = double.Parse(txtPrice.Text, NumberStyles.Currency);
                }
                if (txtHours.Text.Trim() != string.Empty)
                {
                    dr["Hours"] = Convert.ToDouble(txtHours.Text);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
    private bool ValidateRecContract()
    {
        bool _isValid = true;
        int _status = 0;
        if (ddlContractBill.SelectedValue == "1")           // validate location level contract billing
        {
            if (string.IsNullOrEmpty(hdnLocId.Value))
                hdnLocId.Value = "0";

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
            objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            if (!objProp_Contracts.IsExistContract)
            {
                _status = 1;
                _isValid = false;
            }
            else
                _isValid = true;
        }
        if(_isValid.Equals(true))
        {
            if (ddlBilling.SelectedValue == "1")           // validate customer level billing
            {
                int _count = ddlSpecifiedLocation.Items.Count - 1;
                if (_count <= 0)
                {
                    _status = 2;
                    _isValid = false;
                }
                else
                {
                    _isValid = true;
                }
            }
        }
        if(_isValid.Equals(false))
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "validateRecContr", "validateRecContr('" + _status + "');", true);
        }   
        return _isValid;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidateRecContract())
            {
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.BAMT = double.Parse(txtBillAmt.Text, NumberStyles.Currency);
                objProp_Contracts.BCycle = Convert.ToInt32(ddlBillFreq.SelectedValue);
                objProp_Contracts.BStart = Convert.ToDateTime(txtBillStartDt.Text);
                objProp_Contracts.CreditCard = Convert.ToInt32(chkCredit.Checked);
                objProp_Contracts.Cycle = Convert.ToInt32(ddlSchFreq.SelectedValue);
                objProp_Contracts.Date = Convert.ToDateTime(txtScheduleStartDt.Text);
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
                objProp_Contracts.Owner = Convert.ToInt32(hdnPatientId.Value);
                objProp_Contracts.Remarks = txtRemarks.Text;
                objProp_Contracts.Sdate = Convert.ToInt32(ddlDay.SelectedValue);
                objProp_Contracts.Sday = Convert.ToInt32((txtDay.Text.Trim() != string.Empty) ? txtDay.Text : "0");
                objProp_Contracts.STime = Convert.ToDateTime(defaultDate + " " + txtsTime.Text);
                objProp_Contracts.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                objProp_Contracts.SStart = Convert.ToDateTime(txtScheduleStartDt.Text);
                objProp_Contracts.DtElevJob = GetElevData();
                objProp_Contracts.Route = ddlRoute.SelectedValue;
                objProp_Contracts.SWE = Convert.ToInt32(chkWeekends.Checked);
                objProp_Contracts.Hours = Convert.ToDouble(txtBillHours.Text);
                objProp_Contracts.Ctype = ddlServiceType.SelectedValue;
                objProp_Contracts.Description = txtDescription.Text;
                if (!string.IsNullOrEmpty(hdnGLAcct.Value))
                    objProp_Contracts.Chart = Convert.ToInt32(hdnGLAcct.Value);
                
                DateTime unitexp = System.DateTime.MinValue;
                if (DateTime.TryParse(txtUnitExpiration.Text.Trim(), out unitexp))
                    objProp_Contracts.ExpirationDate = Convert.ToDateTime(txtUnitExpiration.Text.Trim());
                if (ddlExpiration.SelectedValue != string.Empty)
                    objProp_Contracts.Expiration = Convert.ToInt16(ddlExpiration.SelectedValue);
                if (txtNumFreq.Text.Trim() != string.Empty)
                    objProp_Contracts.expirationfreq = Convert.ToInt16(txtNumFreq.Text.Trim());

                objProp_Contracts.EscalationType = Convert.ToInt16(ddlEscType.SelectedValue);
                objProp_Contracts.EscalationCycle = (txtEscCycle.Text!=string.Empty) ? Convert.ToInt32(txtEscCycle.Text) : 1;
                objProp_Contracts.EscalationFactor = (txtEscFactor.Text != string.Empty) ? Convert.ToDouble(txtEscFactor.Text) : 0;
                objProp_Contracts.EscalationLast = (txtEscdue.Text != string.Empty) ? Convert.ToDateTime(txtEscdue.Text) : DateTime.MinValue;

                if ((ddlSpecifiedLocation.Items.Count - 1) > 0)                                     // Customer billing details
                {                                                                                   // added by Mayuri 25th dec, 15
                    objProp_Contracts.CustBilling = Convert.ToInt16(ddlBilling.SelectedValue);
                    objProp_Contracts.Central = Convert.ToInt16(ddlSpecifiedLocation.SelectedValue);
                }
                else
                {
                    objPropUser.Billing = 0;
                }
                objProp_Contracts.JobTID = (int)ViewState["JobT"];
                DataTable dtCustom = GetCustomTemplate();
                dtCustom.Columns.Add("UpdateDate", typeof(DateTime));
                dtCustom.Columns.Add("Username", typeof(string));

                objProp_Contracts.DtCustom = dtCustom;
                if (!string.IsNullOrEmpty(txtBillRate.Text))
                {
                    objProp_Contracts.BillRate = Convert.ToDouble(txtBillRate.Text);
                }
                if (!string.IsNullOrEmpty(txtOt.Text))
                {
                    objProp_Contracts.RateOT = Convert.ToDouble(txtOt.Text);
                }
                if (!string.IsNullOrEmpty(txtNt.Text))
                {
                    objProp_Contracts.RateNT = Convert.ToDouble(txtNt.Text);
                }
                if (!string.IsNullOrEmpty(txtDt.Text))
                {
                    objProp_Contracts.RateDT = Convert.ToDouble(txtDt.Text);
                }
                if (!string.IsNullOrEmpty(txtTravel.Text))
                {
                    objProp_Contracts.RateTravel = Convert.ToDouble(txtTravel.Text);
                }
                if (!string.IsNullOrEmpty(txtMileage.Text))
                {
                    objProp_Contracts.Mileage = Convert.ToDouble(txtMileage.Text);
                }
                if(!string.IsNullOrEmpty(txtPO.Text))
                {
                    objProp_Contracts.PO = txtPO.Text;
                }
                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    //objProp_Contracts.ConnConfig = Session["config"].ToString();                  // added by Mayuri 25th dec,15
                    //objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
                    //if (objProp_Contracts.IsExistContract)
                    //{
                    //objProp_Contracts.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                    //}
                    //else
                    //{
                    //    objProp_Contracts.ContractBill = 0;
                    //}
                    objProp_Contracts.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue); // added by Mayuri 25th dec,15
                    objProp_Contracts.JobId = Convert.ToInt32(Request.QueryString["uid"].ToString());

                    objBL_Contracts.UpdateContract(objProp_Contracts);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Contract updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                }
                else
                {                                                                                 // location billing details
                    objProp_Contracts.ConnConfig = Session["config"].ToString();                  // added by Mayuri 25th dec,15
                    objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
                    if (objProp_Contracts.IsExistContract)
                    {
                        objProp_Contracts.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                    }
                    else
                    {
                        objProp_Contracts.ContractBill = 0;
                    }

                    objBL_Contracts.AddContract(objProp_Contracts);

                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Contract added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    ResetFormControlValues(this);
                    gvEquip.DataBind();
                }

                if (Request.QueryString["rt"] != null)
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();setTimeout(function(){window.close();},2000);}", true);

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
        Response.Redirect("reccontracts.aspx");
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillLocInfo();
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
            txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();
            //txtCity.Text = ds.Tables[0].Rows[0]["LocCity"].ToString();
            //ddlState.SelectedValue = ds.Tables[0].Rows[0]["locstate"].ToString();
            //txtZip.Text = ds.Tables[0].Rows[0]["locZip"].ToString();
            //txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            //txtMaincontact.Text = ds.Tables[0].Rows[0]["name"].ToString();
            //txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
            //txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
            hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
            ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["route"].ToString();
            txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
            txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
            txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
            txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
            txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
            txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
        }
        GetDataEquip();
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
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
    protected void ddlExpiration_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpiration.SelectedValue == "1")
        {
            txtUnitExpiration.Visible = true;
            txtNumFreq.Visible = false;
            RequiredFieldValidator4.Enabled = false;
            RequiredFieldValidator3.Enabled = true;
        }
        else if (ddlExpiration.SelectedValue == "2")
        {
            txtUnitExpiration.Visible = false;
            txtNumFreq.Visible = true;          
            RequiredFieldValidator4.Enabled = true;
            RequiredFieldValidator3.Enabled = false;
        }
        else
        {
            txtUnitExpiration.Visible = false;
            txtNumFreq.Visible = false;            
            RequiredFieldValidator4.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
        }
    }
    private void FillContractBill()
    {
        try
        {
            List<ContractBill> _lstBill = new List<ContractBill>();
            _lstBill = ContractBilling.GetAll();

            ddlContractBill.DataSource = _lstBill;
            ddlContractBill.DataValueField = "ID";
            ddlContractBill.DataTextField = "Name";
            ddlContractBill.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlContractBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlContractBill.SelectedValue =="1")
        {
            if (string.IsNullOrEmpty(hdnLocId.Value))
                hdnLocId.Value = "0";
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            if (!string.IsNullOrEmpty(txtLocation.Text))
            {
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
            }
            else
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);

            objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            if (!objProp_Contracts.IsExistContract)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
            } 
        }
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(txtCustomer.Text))
        {
            FillSpecifyLocation();
        }
    }
    private void FillSpecifyLocation()
    {
        try
        {
            _objLoc.ConnConfig = Session["config"].ToString();

            DataSet _dsLocation = new DataSet();
            if (string.IsNullOrEmpty(hdnPatientId.Value))
                hdnPatientId.Value = "0";
            _dsLocation = objBL_Customer.getAllLocationOnCustomer(_objLoc, Convert.ToInt32(hdnPatientId.Value));

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
    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlServiceType.SelectedValue != "")
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Type = ddlServiceType.SelectedValue;
            DataSet _ds = objBL_User.GetServiceTypeByType(objPropUser);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                txtGLAcct.Text = _ds.Tables[0].Rows[0]["GLAcct"].ToString();
                hdnGLAcct.Value = _ds.Tables[0].Rows[0]["Sacct"].ToString();
            }
            rfvGLAcct.ControlToValidate = "txtGLAcct";
            rfvGLAcct.Enabled = true;
        }
        else
        {
            rfvGLAcct.Enabled = false;
        }
    }
}
