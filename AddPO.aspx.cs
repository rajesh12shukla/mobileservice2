using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddPO : System.Web.UI.Page
{
    #region Variable
    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    PO _objPO = new PO();
    BL_Bills _objBLBills = new BL_Bills();
 
    BL_Vendor _objBLVendor = new BL_Vendor();
    Vendor _objVendor = new Vendor();

    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
    
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
                divSuccess.Visible = false;
                txtCreatedBy.Text = Session["User"].ToString();
                FillUserAddress();
                //lnkPrint.Visible = false;
                //lstitem.Visible = false;
                lnk_PO.Visible = false;
                lnk_Madden.Visible = false;
                //lstitem.Style.Add("display", "none");
                txtPO.Enabled = false;
                if (Request.QueryString["id"] != null)
                {
                    //lstitem.Style.Add("display", "block");
                    //lstitem.Visible = true;
                    lblPO.Visible = true;
                    lblPOId.Visible = true;

                    //lnkPrint.Visible = true;
                    lnk_PO.Visible = true;
                    lnk_Madden.Visible = true;
                    lblHeader.Text = "Edit Purchase Order";
                    _objPO.ConnConfig = Session["config"].ToString();
                    _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
                    DataSet ds = _objBLBills.GetPOById(_objPO);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        lblPOId.Text = dr["PO"].ToString();
                        txtPO.Text = dr["PO"].ToString();
                        txtVendor.Text = dr["VendorName"].ToString();
                        hdnVendorID.Value = dr["Vendor"].ToString();
                        //txtAddress.Text = dr["Address"].ToString() + Environment.NewLine + dr["city"].ToString() + ", "+ dr["State"].ToString() + ", "+ dr["zip"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
                        ddlTerms.SelectedValue = dr["PaymentTerms"].ToString();
                        txtFOB.Text = dr["FOB"].ToString();
                        if (!string.IsNullOrEmpty(dr["StatusName"].ToString()))
                        {
                            txtStatus.Text = dr["StatusName"].ToString();
                            txtStatus.Enabled = false;
                        }
                        
                        if (Convert.ToInt16(dr["Status"]).Equals(1))
                        {
                            btnSubmit.Visible = false;
                        }
                        txtShipVia.Text = dr["ShipVia"].ToString();
                        //txtFreight.Text = dr["Freight"].ToString();
                        txtCreatedBy.Text = dr["fBy"].ToString();
                        //txtCustom1.Text = dr["Custom1"].ToString();
                        //txtCustom2.Text = dr["Custom2"].ToString();
                        lblTotalAmount.Text = dr["Amount"].ToString();
                        hdnTotal.Value = dr["Amount"].ToString();
                        if (Convert.ToInt16(dr["Approved"]).Equals(1))
                        {
                            chkApproved.Checked = true;
                        }
                        txtDesc.Text = dr["fDesc"].ToString();
                        txtShipTo.Text = dr["ShipTo"].ToString();
                        txtPoRevision.Text = dr["PORevision"].ToString();
                        txtPOCode.Text = dr["POReasonCode"].ToString();
                        txtCourrierAcct.Text = dr["CourrierAcct"].ToString();
                        gvGLItems.DataSource = ds.Tables[1];
                        gvGLItems.DataBind();
                    }
                    pnlNext.Visible = true;
                }
                else
                {
                    SetPOForm();
                }
                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
                }
                FillTerms();
                FillBomType();
                DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
                if(dsTerm.Tables[0].Rows.Count > 0)
                {
                    lblTC.Text = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString().Replace("\n", "<br />");
                }
                txtQty.Text = "0.00";
                txtBudgetUnit.Text = "0.00";
                lblBudgetExt.Text = "0.00";
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
        foreach (GridViewRow gr in gvGLItems.Rows)
        {
            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");

            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + gvGLItems.ClientID + "',event);";
        }
       
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvJournal.ClientID + "');", true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool _flag = false;
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
            _flag = (bool)ViewState["FlagPeriodClose"];

            if (_flag)
            {
                DataTable dtPO = GetPOItem();

                if(dtPO.Rows.Count > 0)
                {
                    if (dtPO.Rows.Count > 0)
                    {
                        dtPO.Columns.Remove("RowID");
                        dtPO.Columns.Remove("AcctNo");
                        dtPO.Columns.Remove("Loc");
                        dtPO.Columns.Remove("JobName");
                        dtPO.Columns.Remove("Phase");
                        //dtPO.Columns.Remove("TypeID");
                    }
                    _objPO.PODt = dtPO;
                    _objPO.POID = Convert.ToInt32(txtPO.Text);
                    _objPO.ConnConfig = Session["config"].ToString();
                    _objPO.fDate = Convert.ToDateTime(txtDate.Text);
                    _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                    _objPO.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                    _objPO.FOB = txtFOB.Text;

                    _objPO.ShipVia = txtShipVia.Text;
                    _objPO.ShipTo = txtAddress.Text;
                    _objPO.ReqBy = 0;
                    _objPO.fBy = txtCreatedBy.Text;
                    if (chkApproved.Checked.Equals(true))
                    {
                        _objPO.Approved = 1;
                    }
                    else
                    {
                        _objPO.Approved = 0;
                    }
                    _objPO.fDesc = txtDesc.Text;
                    _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
                    _objPO.Status = 0;
                    _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                    _objPO.ShipVia = txtShipVia.Text;
                    _objPO.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                    _objPO.FOB = txtFOB.Text;
                    _objPO.ShipTo = txtShipTo.Text;
                    //_objPO.Custom1 = txtCustom1.Text;
                    //_objPO.Custom2 = txtCustom2.Text;
                    _objPO.ApprovedBy = "";
                    _objPO.ReqBy = Convert.ToInt32(0);
                    _objPO.CourrierAcct = txtCourrierAcct.Text;
                    _objPO.POReasonCode = txtPOCode.Text;
                    _objPO.PORevision = txtPoRevision.Text;

                    double totalAmount = 0;
                    foreach (GridViewRow gr in gvGLItems.Rows)
                    {
                        TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                        if (!string.IsNullOrEmpty(txtGvAmount.Text))
                        {
                            totalAmount += Convert.ToDouble(txtGvAmount.Text);
                        }
                    }
                    _objPO.Amount = totalAmount;
                    
                    if (Request.QueryString["id"] != null)
                    {
                        _objBLBills.UpdatePO(_objPO);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'PO Updated Successfully! </br> <b> PO# : " + _objPO.POID + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        _objPO.Status = 0;
                        _objBLBills.AddPO(_objPO);
                        ResetFormControlValues(this);
                        SetPOForm();
                        //Response.Redirect(Request.RawUrl, false);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'PO Created Successfully! </br> <b> PO# : " + _objPO.POID + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }   
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'You must have at least one item on the purchase order.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManagePO.aspx");
    }
    protected void gvGLItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (gvGLItems.Rows.Count > 0)
            //{
            //    gvGLItems.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
            int rowIndex = Convert.ToInt32(e.CommandArgument);
           
            if (e.CommandName == "DeleteTransaction")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvGLItems.Rows[index];

                HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                TextBox txtGvAmount = (TextBox)row.FindControl("txtGvAmount");
                TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                TextBox txtGvPrice = (TextBox)row.FindControl("txtGvPrice");
                TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                HiddenField hdnJobID = (HiddenField)row.FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                TextBox txtGvDue = (TextBox)row.FindControl("txtGvDue");
                HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");

                hdnAcctID.Value = "0";
                txtGvAcctNo.Text = "";
                txtGvDesc.Text = "";
                txtGvPrice.Text = "";
                txtGvQuan.Text = "";
                txtGvAmount.Text = "";
                txtGvLoc.Text = "";
                txtGvJob.Text = "";
                txtGvPhase.Text = "";
                hdnPID.Value = "0";
                hdnJobID.Value = "0";
                txtGvDue.Text = "";
                hdnTypeId.Value = "0";
                txtGvItem.Text = "";

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
            int rowIndex = gvGLItems.Rows.Count - 1;
            
            DataTable dt = GetPOGridItems();
            DataRow dr = null;
            for (int i = 0; i < 4; i++)
            {
                dr = dt.NewRow();
                dr["RowID"] = i + 1;
                dr["Line"] = i + 1;
                dr["ID"] = 0;
                dr["AcctID"] = 0;
                dr["AcctNo"] = string.Empty;
                dr["fDesc"] = string.Empty;
                dr["Loc"] = string.Empty;
                dr["JobName"] = string.Empty;
                dr["JobID"] = 0;
                dr["Phase"] = string.Empty;
                dr["PhaseID"] = 0;
                dr["TypeID"] = DBNull.Value;
                dr["Quan"] = string.Empty;
                dr["Price"] = string.Empty;
                dr["Amount"] = string.Empty;
                dr["Freight"] = 0.00;
                dr["Rquan"] = 0.00;
                dr["Due"] = DBNull.Value;

                
                dt.Rows.Add(dr);
                rowIndex++;
            }

            gvGLItems.DataSource = dt;
            gvGLItems.DataBind();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnSelectVendor_Click(object sender, EventArgs e)
    {
        FillAddress();
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PO"];
            Response.Redirect("addpo.aspx?id=" + dt.Rows[0]["PO"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PO"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["PO"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addpo.aspx?id=" + dt.Rows[index - 1]["PO"]);
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
            DataTable dt = (DataTable)Session["PO"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["PO"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addpo.aspx?id=" + dt.Rows[index + 1]["PO"]);
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
            DataTable dt = (DataTable)Session["PO"];
            Response.Redirect("addpo.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["PO"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void lnkPrint_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("PrintPO.aspx?id=" + Request.QueryString["id"].ToString(), true);
    //}

  
    #endregion

    #region Custom Functions
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
    private void FillAddress()
    {
        if (!string.IsNullOrEmpty(hdnVendorID.Value))
        {
            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = Convert.ToInt32(hdnVendorID.Value);
            DataSet ds = new DataSet();
            ds = _objBLVendor.GetVendorRolDetails(_objVendor);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
                txtShipVia.Text = ds.Tables[0].Rows[0]["ShipVia"].ToString();
            }
            else
            {
                txtAddress.Text = "";
            }
        }
    }
    private void FillUserAddress()
    {
        DataSet dsC = new DataSet();
        _objPropUser.ConnConfig = Session["config"].ToString();
        dsC = _objBLUser.getControl(_objPropUser);

        string address;
        address = dsC.Tables[0].Rows[0]["Address"].ToString() + ", " + Environment.NewLine;
        address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
        txtShipTo.Text = address;
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
                _objPropUser.PageName = "addpo.aspx";
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
    private void SetInitialRow()    //Initialization of Datatable.
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
  
            dt.Columns.Add(new DataColumn("RowID", typeof(string)));
            dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
            dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
            dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
            dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
            dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
            dt.Columns.Add(new DataColumn("Quan", typeof(string)));
            dt.Columns.Add(new DataColumn("Price", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Loc", typeof(string)));
            dt.Columns.Add(new DataColumn("JobName", typeof(string)));
            dt.Columns.Add(new DataColumn("JobID", typeof(Int32)));    // Job
            dt.Columns.Add(new DataColumn("Phase", typeof(string)));
            dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
            dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
            dt.Columns.Add(new DataColumn("Freight", typeof(double)));  // Freight
            dt.Columns.Add(new DataColumn("Rquan", typeof(double)));    // Rquan
            dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
            dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
            dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
            dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));

            int rowIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                dr = dt.NewRow();
                dr["RowID"] = i + 1;
                dr["Line"] = i + 1;
                dr["ID"] = 0;
                dr["AcctID"] = 0;
                dr["AcctNo"] = string.Empty;
                dr["fDesc"] = string.Empty;
                dr["Loc"] = string.Empty;
                dr["JobName"] = string.Empty;
                dr["JobID"] = 0;
                dr["Phase"] = string.Empty;
                dr["PhaseID"] = 0;
                dr["TypeID"] = DBNull.Value;
                dr["Quan"] = string.Empty;
                dr["Price"] = string.Empty;
                dr["Amount"] = string.Empty;
                dr["Freight"] = 0.00;
                dr["Rquan"] = 0.00;
                dr["Due"] = DBNull.Value;
                dr["ItemDesc"] = string.Empty;
                dt.Rows.Add(dr);
                rowIndex++;
            }

            ViewState["Transactions"] = dt;

            gvGLItems.DataSource = dt;
            gvGLItems.DataBind();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void SetPOForm()
    {
        try
        {
            _objPO.ConnConfig = Session["config"].ToString();
            txtDesc.Text = "Purchase order";
            bool firstPo = _objBLBills.IsFirstPo(_objPO);
            if (firstPo.Equals(false))
            {
                txtPO.Enabled = true;
            }

            int PoId = _objBLBills.GetMaxPOId(_objPO);
            txtPO.Text = PoId.ToString();

            SetInitialRow();
            lblTotalAmount.Text = "0.00";
            txtQty.Text = "0.00";
            txtBudgetUnit.Text = "0.00";
            lblBudgetExt.Text = "0.00";
            FillUserAddress();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "GetClientUTC();", true);
            //txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //txtDueDate.Text = DateTime.Now.AddDays(30).ToString("MM/dd/yyyy");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();

            ds = _objBLUser.getTerms(_objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable GetPOGridItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));    // Job
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        dt.Columns.Add(new DataColumn("Freight", typeof(double)));  // Freight
        dt.Columns.Add(new DataColumn("Rquan", typeof(double)));    // Rquan
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
        dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
        dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));

        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnIndex"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (!(dict["hdnTID"].ToString().Trim() == string.Empty))
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnTID"].ToString());
                    }
                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }
                    if (!(dict["hdnAcctID"].ToString().Trim() == string.Empty))
                    {
                        dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    }
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    dr["Quan"] = dict["txtGvQuan"].ToString();
                    dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    dr["Amount"] = dict["txtGvAmount"].ToString().Trim();
                    if (!(dict["txtGvLoc"].ToString().Trim() == string.Empty))
                    {
                        dr["Loc"] = dict["txtGvLoc"].ToString().Trim();
                    }
                    if (!(dict["txtGvJob"].ToString().Trim() == string.Empty))
                    {
                        dr["JobName"] = dict["txtGvJob"].ToString().Trim();
                    }
                    if (!(dict["hdnJobID"].ToString().Trim() == string.Empty))
                    {
                        dr["JobID"] = Convert.ToInt32(dict["hdnJobID"]);
                    }
                    if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                    {
                        dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                    }
                    if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                    {
                        dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                    }
                    if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                    {
                        dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                    }
                    dr["Freight"] = 0.00;
                    if (!(dict["txtGvDue"].ToString().Trim() == string.Empty))
                    {
                        dr["Due"] = Convert.ToDateTime(dict["txtGvDue"].ToString());
                    }
                    else
                    {
                        dr["Due"] = DBNull.Value;
                    }
                    if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    {
                        dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    }
                    if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    {
                        dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    }

                    dt.Rows.Add(dr);
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetPOItem()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));    // Job
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));    
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        dt.Columns.Add(new DataColumn("Freight", typeof(double)));  // Freight
        dt.Columns.Add(new DataColumn("Rquan", typeof(double)));    // Rquan
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
        dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
        dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));

        try
        {        
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtGvAcctNo"].ToString().Trim() == string.Empty)
                    {
                        //return dt;
                        continue;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (!(dict["hdnTID"].ToString().Trim() == string.Empty))
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnTID"].ToString());
                    }
                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }
                    dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    dr["Quan"] = dict["txtGvQuan"].ToString();
                    dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    dr["Amount"] = dict["txtGvAmount"].ToString().Trim();
                    if (!(dict["txtGvLoc"].ToString().Trim() == string.Empty))
                    {
                        dr["Loc"] = dict["txtGvLoc"].ToString().Trim();
                    }
                    if (!(dict["txtGvJob"].ToString().Trim() == string.Empty))
                    {
                        dr["JobName"] = dict["txtGvJob"].ToString().Trim();
                    }
                    if (!(dict["hdnJobID"].ToString().Trim() == string.Empty))
                    {
                        dr["JobID"] = Convert.ToInt32(dict["hdnJobID"]);
                    }
                    if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                    {
                        dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                    }
                    if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                    {
                        dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                    }
                    if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                    {
                        dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                    }
                    dr["Freight"] = 0.00;
                    if(!(dict["txtGvDue"].ToString().Trim() == string.Empty))
                    {
                        dr["Due"] = Convert.ToDateTime(dict["txtGvDue"].ToString());
                    }
                    else
                    {
                        dr["Due"] = DBNull.Value;
                    }
                    if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    {
                        dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    }
                    if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    {
                        dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    }
                    
                    dt.Rows.Add(dr);
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(objJob);

            DataRow dr = ds.Tables[0].NewRow();
            if(ds.Tables[0].Rows.Count > 0)
            {
                dr["ID"] = 0;
                dr["Type"] = "Select Type";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }
            else
            {
                dr["ID"] = 0;
                dr["Type"] = "No data found";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }
            ddlBomType.DataSource = ds.Tables[0];
            ddlBomType.DataTextField = "Type";
            ddlBomType.DataValueField = "ID";
            ddlBomType.DataBind();
        }
        catch(Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCode(string prefixText, int count, string contextKey)
    {
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();
       
        DataSet ds = new DataSet();
        //objJob.SearchValue = prefixText
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_Job.GetJobCode(objJob);
        DataTable dt = ds.Tables[0];

        List<string> txtValue = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }
        return txtValue.ToArray();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetUM(string prefixText, int count)
    {
        JobT _objJob = new JobT();
        BL_Job _objBLJob = new BL_Job();

        DataSet ds = new DataSet();
        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = _objBLJob.GetAllUM(_objJob);
        DataTable dt = ds.Tables[0];

        List<string> txtValue = new List<string>();
        String dbValues;
        if (dt.Rows.Count.Equals(0))
        {
            DataRow dr = dt.NewRow();
            dr["value"] = 0;
            dr["label"] = "No Record Found!";
            dt.Rows.InsertAt(dr, 0);
        }

        foreach(DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }

        return txtValue.ToArray();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetItems(string prefixText, int count, string contextKey)
    {
        Wage objWage = new Wage();
        BL_User objBL_User = new BL_User();
        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        List<string> txtValue = new List<string>();
        DataTable dtval = new DataTable();
        //objJob.SearchValue = prefixText
        objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        if(contextKey == null)
            contextKey = string.Empty;

        if (contextKey.Equals("1"))  // on select of Materials, fill inv data
        {
            DataSet ds = objBL_Job.GetAllInvDetails(objJob);
            dtval = ds.Tables[0];
        }
        else if (contextKey.Equals("2"))   // on select of Labor, fill wage data
        {
            DataSet ds = objBL_User.GetAllWage(objWage);
            dtval = ds.Tables[0];
        }
        else
        {
            dtval.Columns.Add("label", typeof(string));
            dtval.Columns.Add("value", typeof(int));

            DataRow drval = dtval.NewRow();
            drval["value"] = 0;
            drval["label"] = "No data found";
            dtval.Rows.Add(drval);
        }

        DataTable dt = dtval;
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["label"].ToString(), row["value"].ToString());
            txtValue.Add(dbValues);
        }
        return txtValue.ToArray();
    }
    protected void lbtnItemSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            var item = txtItem.Text;
            var type = ddlBomType.SelectedItem.Text;
            objJob.ConnConfig = Session["config"].ToString();
            objJob.Job = Convert.ToInt32(hdnJobId.Value);
            objJob.Code = txtOpSeq.Text;
            objJob.Type = Convert.ToInt16(ddlBomType.SelectedValue);
            objJob.ItemID = Convert.ToInt32(hdnItemID.Value);
            objJob.fDesc = txtJobDesc.Text;
            objJob.QtyReq = Convert.ToDouble(txtQty.Text);
            objJob.UM = txtUM.Text;
            objJob.BudgetUnit = Convert.ToDouble(txtBudgetUnit.Text);
            objJob.BudgetExt = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtBudgetUnit.Text);
            objJob.ScrapFact = 0;
            objJob.Line = objBL_Job.AddBOMItem(objJob);
            //addedItem(item, itemId, phaseId, typeId, type, fdesc)
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "addedItem('" + item + "', '" + objJob.ItemID + "', '" + objJob.Line + "', '" + objJob.Type + "', '" + type + "', '" + objJob.fDesc + "');", true);
        }
        catch(Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnk_PO_Click(object sender, EventArgs e)
    {
        Response.Redirect("PrintPO.aspx?id=" + Request.QueryString["id"].ToString(), true);
    }
    protected void lnk_Madden_Click(object sender, EventArgs e)
    {
        Response.Redirect("PrintMaddenPO.aspx?id=" + Request.QueryString["id"].ToString(), true);
    }
}