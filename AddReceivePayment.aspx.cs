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

public partial class AddReceivePayment : System.Web.UI.Page
{
    #region Variables
    private const string asc = " ASC";
    private const string desc = " DESC";

    Customer objCustomer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();
    Contracts objPropContracts = new Contracts();

    Chart objChart = new Chart();
    BL_Chart objBL_Chart = new BL_Chart();

    Invoices objInvoice = new Invoices();

    Transaction objTrans = new Transaction();
    ReceivedPayment objReceivePay = new ReceivedPayment();
    PaymentDetails objPayment = new PaymentDetails();

    BL_Deposit objBL_Deposit = new BL_Deposit();

    Journal objJournal = new Journal();
    BL_JournalEntry objBL_Journal = new BL_JournalEntry();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

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
                txtStatus.Enabled = false;
                txtStatus.Text = "Open";
                userpermissions();
                divSuccess.Visible = false;
                //FillCustomer();
                FillPayment();
                SetUndepositedFund();
                if (Request.QueryString["id"] != null) // edit received payment
                {
                    lblHeader.Text = "Edit Receive Payment";
                    SetDataForEdit();
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
                    txtCustomer.ReadOnly = true;
                    txtLocation.ReadOnly = true;
                    rfvCustomer.Enabled = false;
                    if (ddlPayment.SelectedItem.Text == "Cash")
                    {
                        lblCheck.Text = "Reference";
                        lblAmount.Text = "Amount received";
                    }
                    pnlNext.Visible = true;
                }
                else //  add received payment
                {
                    txtMemo.Text = "Received payment";
                    lblCustomerBalance.Text = "$0.00";
                    txtAmount.Text = "$0.00";
                    txtStatus.Text = "Open";

                    DataSet ds = new DataSet();
                    
                    objProp_Contracts.ConnConfig = Session["config"].ToString();
                    objProp_Contracts.Rol = 0;
                    ds = objBL_Deposit.GetInvoiceByCustomerID(objProp_Contracts);

                    SetGridView(ds.Tables[0]);
                }
                Permission();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("financeMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkFinanceMgr");
        //a.Style.Add("color", "#2382b2"); 

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkReceivePayment");
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
    protected void Page_PreRender(Object o, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvInvoice.ClientID + "');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    #endregion
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                bool IsValid = ValidateGrid();
                if (IsValid)
                {
                    if (Request.QueryString["id"] != null) // edit received payment
                    {
                        bool Flag = (bool)ViewState["FlagPeriodClose"];
                        if (!Flag)
                        {
                            IsValid = false;
                        }
                    }
                    else
                    {
                        GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                        bool Flag = (bool)ViewState["FlagPeriodClose"];
                        if (!Flag)
                        {
                            IsValid = false;
                        }
                    }
                    if (IsValid)
                    {
                        double totalPay = 0.00;
                        double totalDue = 0.00;

                        CreateDatatable();
                        DataTable dt = (DataTable)ViewState["ReceivPay"];

                        DataTable dtReceive = dt.Clone();
                        objReceivePay.ConnConfig = Session["config"].ToString();
                        objReceivePay.ID = Convert.ToInt32(Request.QueryString["id"]);
                        objReceivePay.Rol = Convert.ToInt32(hdnCustID.Value);
                        if (!string.IsNullOrEmpty(hdnLocID.Value))
                        {
                            objReceivePay.Loc = Convert.ToInt32(hdnLocID.Value);
                        }
                        else
                            objReceivePay.Loc = 0;

                        objReceivePay.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                        objReceivePay.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                        objReceivePay.CheckNumber = txtCheck.Text;
                        objReceivePay.fDesc = txtMemo.Text;

                        foreach (GridViewRow gr in gvInvoice.Rows)
                        {
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                            if (chkSelect.Checked.Equals(true))
                            {
                                HiddenField hdnPaymentID = (HiddenField)gr.FindControl("hdnPaymentID");
                                HiddenField hdnTransID = (HiddenField)gr.FindControl("hdnTransID");
                                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                                Label lblOrigAmount = (Label)gr.FindControl("lblOrigAmount");
                                Label lblDueAmount = (Label)gr.FindControl("lblDueAmount");
                                TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
                                HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");

                                double pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);
                                double orig = double.Parse(lblOrigAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);
                                double due = double.Parse(lblDueAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);
                                double prevDue = double.Parse(hdnPrevDue.Value);    // actual due amount = Previous due amount


                                due = double.Parse(lblDueAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);      // calculated due amount

                                totalPay = +(totalPay + pay);
                                totalDue = totalDue + prevDue;

                                #region invoice status

                                DataRow dr = dtReceive.NewRow();
                                dr["InvoiceID"] = Convert.ToInt32(hdnID.Value);

                                if (pay < due)
                                {
                                    dr["Status"] = 3;           // partial payment : invoice
                                }
                                else if (prevDue.Equals(pay))
                                {
                                    dr["Status"] = 1;           // paid : invoice
                                }
                                else
                                {
                                    if (pay < prevDue)
                                    {
                                        dr["Status"] = 3;
                                    }
                                    //dr["Status"] = 0;           // open : invoice
                                }

                                dr["PayAmount"] = pay;

                                #endregion

                                dtReceive.Rows.Add(dr);
                            }
                            else
                            {
                                HiddenField hdnCheck = (HiddenField)gr.FindControl("hdnCheck");
                                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                                if (Convert.ToBoolean(hdnCheck.Value).Equals(true))
                                {
                                    objInvoice.ConnConfig = Session["config"].ToString();
                                    objInvoice.Ref = Convert.ToInt32(hdnID.Value);
                                    objInvoice.Status = 0;
                                    objBL_Deposit.UpdateInvoice(objInvoice);
                                }
                            }
                        }
                        objReceivePay.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);
                        objReceivePay.AmountDue = totalDue - totalPay;
                        objReceivePay.DtPay = dtReceive;

                        if (Request.QueryString["id"] != null)
                        {
                            objBL_Deposit.UpdateReceivePayment(objReceivePay);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Received payment Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        }
                        else
                        {
                            objBL_Deposit.AddReceivePayment(objReceivePay);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Received payment Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                            ResetForm();

                        }
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
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=inv");
        }
        else
        {
            Response.Redirect("receivepayment.aspx");
        }
    }
    protected void ddlPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayment.SelectedItem.Text == "Check")
        {
            lblCheck.Text = "Check";
            lblAmount.Text = "Check amount";
        }
        else
        {
            lblCheck.Text = "Reference";
            lblAmount.Text = "Amount received";
        }
        //updPnlAmount.Update();
        //updPnlCheck.Update();
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[0]["ID"]);
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
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[index - 1]["ID"]);
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
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[index + 1]["ID"]);
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
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            string sortExpression = e.SortExpression;

            if (GvSortDirection == SortDirection.Ascending)
            {
                GvSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, desc);
            }
            else
            {
                GvSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, asc);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        try
        {
            objPayment.ConnConfig = Session["config"].ToString();
            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objPayment.Loc = Convert.ToInt32(hdnLocID.Value);
            }
            else if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                objPayment.Rol = Convert.ToInt32(hdnCustID.Value);
            }
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            DataSet dsloc = new DataSet();
            dsloc = objBL_User.getLocationByID(objPropUser);

            if (dsloc.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = dsloc.Tables[0].Rows[0]["tag"].ToString();
                txtCustomer.Text = dsloc.Tables[0].Rows[0]["custname"].ToString();
                hdnCustID.Value = dsloc.Tables[0].Rows[0]["owner"].ToString();
            }

            DataSet ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);

            SetGridView(ds.Tables[0]);
            lblCustomerBalance.Text = string.Format("{0:c}", ds.Tables[1].Rows[0]["Balance"]);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                if (Convert.ToInt32(hdnCustID.Value) > 0)
                {
                    DataSet ds = new DataSet();
                    objPayment.ConnConfig = Session["config"].ToString();
                    objPayment.Rol = Convert.ToInt32(hdnCustID.Value);
                    ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);

                    SetGridView(ds.Tables[0]);
                    DataTable dt = ds.Tables[1];
                    lblCustomerBalance.Text = string.Format("{0:c}", dt.Rows[0]["Balance"]);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Custom Functions

    private void CreateDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("InvoiceID", typeof(int));
        dt.Columns.Add("Status", typeof(Int16));
        dt.Columns.Add("PayAmount", typeof(double));

        DataRow dr = dt.NewRow();
        dr["InvoiceID"] = DBNull.Value;
        dr["Status"] = DBNull.Value;
        dr["PayAmount"] = 0;

        dt.Rows.Add(dr);
        ViewState["ReceivPay"] = dt;
    }
    private bool ValidateGrid()
    {
        bool IsValid = true;
        try
        {
            int count = 0;
            foreach (GridViewRow gr in gvInvoice.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
                if (chkSelect.Checked.Equals(true))
                {
                    if (!string.IsNullOrEmpty(txtPAmount.Text))
                    {
                        double pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
                        if (!Convert.ToDouble(pay).Equals(0))
                        {
                            count++;
                        }
                    }
                }
            }
            if (count.Equals(0))
            {
                IsValid = false;
            }
            if(!IsValid)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'You cannot record a blank transaction. Fill in the appropriate fields and try again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return IsValid;
    }
    private void ResetForm()
    {
        ResetFormControlValues(this);

        //objPropContracts.ConnConfig = Session["config"].ToString();
        //objPropContracts.Rol = 0;

        objPayment.ConnConfig = Session["config"].ToString();
        objPayment.Rol = 0;
        DataSet ds = new DataSet();
        //ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.Rol = 0;
        ds = objBL_Deposit.GetInvoiceByCustomerID(objProp_Contracts);

        SetGridView(ds.Tables[0]);
        txtStatus.Enabled = false;
        txtStatus.Text = "Open";
        lblCheck.Text = "Check";
        lblCustomerBalance.Text = "$0.00";
        txtAmount.Text = "$0.00";
        txtMemo.Text = "Received payment";
    }
    private void SetUndepositedFund()
    {
        try
        {
            DataSet _dsAcct = new DataSet();
            objChart.ConnConfig = Session["config"].ToString();
            _dsAcct = objBL_Chart.GetUndepositeAcct(objChart);
            if (_dsAcct.Tables[0].Columns.Contains("fDesc"))
            {
                lblDepositTo.Text = _dsAcct.Tables[0].Rows[0]["Acct"].ToString() + " - " + _dsAcct.Tables[0].Rows[0]["fDesc"].ToString();
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
    private void FillPayment()
    {
        ddlPayment.Items.Add(new ListItem("Check", "0"));
        ddlPayment.Items.Add(new ListItem("Cash", "1"));
        ddlPayment.Items.Add(new ListItem("Wire Transfer", "2"));
        ddlPayment.Items.Add(new ListItem("ACH", "3"));
        ddlPayment.Items.Add(new ListItem("Credit Card", "4"));
    }
    private void SetGridView(DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("OrigAmount"))
                    {
                        gvInvoice.DataSource = dt;
                        gvInvoice.DataBind();
                        ViewState["Invoices"] = dt;

                        if (gvInvoice.FooterRow != null)
                        {
                            Label lblTotalOrigAmount = gvInvoice.FooterRow.FindControl("lblTotalOrigAmount") as Label;
                            lblTotalOrigAmount.Text = string.Format("{0:c}", dt.Compute("sum(OrigAmount)", string.Empty));

                            Label lblTotalSalesTax = gvInvoice.FooterRow.FindControl("lblTotalSalesTax") as Label;
                            lblTotalSalesTax.Text = string.Format("{0:c}", dt.Compute("sum(STax)", string.Empty));

                            Label lblTotalPretaxAmount = gvInvoice.FooterRow.FindControl("lblTotalPretaxAmount") as Label;
                            lblTotalPretaxAmount.Text = string.Format("{0:c}", dt.Compute("sum(Amount)", string.Empty));

                            Label lblTotalDueAmount = gvInvoice.FooterRow.FindControl("lblTotalDueAmount") as Label;
                            lblTotalDueAmount.Text = string.Format("{0:c}", dt.Compute("sum(DueAmount)", string.Empty));

                            Label lblTotalPayAmount = gvInvoice.FooterRow.FindControl("lblTotalPayAmount") as Label;
                            lblTotalPayAmount.Text = string.Format("{0:c}", dt.Compute("sum(paymentAmt)", string.Empty));

                        }
                        if (Request.QueryString["id"] != null) // edit received payment
                        {
                            CheckAllCheckbox();
                        }
                    }
                }
                else
                {
                    gvInvoice.DataSource = dt;
                    gvInvoice.DataBind();
                    ViewState["Invoices"] = dt;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void CheckAllCheckbox()
    {
        foreach (GridViewRow row in gvInvoice.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            HiddenField hdnPaymentID = (HiddenField)row.FindControl("hdnPaymentID");
            if (Convert.ToInt32(hdnPaymentID.Value) > 0)
            {
                chkSelect.Checked = true;
            }
        }
    }
    private void SetDataForEdit()
    {
        try
        {
            lblReceiveRef.Visible = true;
            lblReceiveID.Visible = true;

            // lblHeader.Text = "Edit Receive Payment";
            DataSet dsReceive = new DataSet();
            objReceivePay.ConnConfig = Session["config"].ToString();
            objReceivePay.ID = Convert.ToInt32(Request.QueryString["id"]);

            dsReceive = objBL_Deposit.GetReceivePaymentByID(objReceivePay);
            if (dsReceive.Tables[0].Columns.Contains("ID"))
            {
                lblReceiveID.Text = dsReceive.Tables[0].Rows[0]["ID"].ToString();

                if (Convert.ToInt16(dsReceive.Tables[0].Rows[0]["Status"]).Equals(1))
                {
                    btnSubmit.Visible = false;
                    txtStatus.Text = "Deposited";
                }
                else
                {
                    txtStatus.Text = "Open";
                }

                txtCustomer.Text = dsReceive.Tables[0].Rows[0]["RolName"].ToString();
                txtLocation.Text = dsReceive.Tables[0].Rows[0]["Tag"].ToString();
                hdnCustID.Value = dsReceive.Tables[0].Rows[0]["Owner"].ToString();
                hdnLocID.Value = dsReceive.Tables[0].Rows[0]["Loc"].ToString();

                txtAmount.Text = string.Format("{0:c}", Convert.ToDouble(dsReceive.Tables[0].Rows[0]["Amount"]));
                txtDate.Text = Convert.ToDateTime(dsReceive.Tables[0].Rows[0]["PaymentReceivedDate"]).ToString("MM/dd/yyyy");
                txtCheck.Text = dsReceive.Tables[0].Rows[0]["CheckNumber"].ToString();
                ddlPayment.SelectedValue = dsReceive.Tables[0].Rows[0]["PaymentMethod"].ToString();
                txtMemo.Text = dsReceive.Tables[0].Rows[0]["fDesc"].ToString();

                //txtMemo.Text = _dsReceiPmt.Tables[0].Rows[0]["PaymentMethod"].ToString();
                DataSet dsPayment = new DataSet();
                objPayment.ConnConfig = Session["config"].ToString();
                objPayment.ReceivedPaymentID = Convert.ToInt32(dsReceive.Tables[0].Rows[0]["ID"]);
                objPayment.Rol = Convert.ToInt32(hdnCustID.Value);
                objPayment.Loc = Convert.ToInt32(hdnLocID.Value);
                DataSet ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);
                if(ds.Tables[1].Rows.Count > 0)
                {
                    lblCustomerBalance.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[1].Rows[0]["Balance"]));
                }
                SetGridView(ds.Tables[0]);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetPeriodDetails(DateTime transDate)
    {
        bool Flag = CommonHelper.GetPeriodDetails(transDate);
        ViewState["FlagPeriodClose"] = Flag;
        if (!Flag)
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
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                objPropUser.PageName = "addreceivepayment.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objPropUser);
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
    
    public SortDirection GvSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    private void FillGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortData();

        BindGridDatatable(dt);
    }
    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        try
        {
            GetSelectedInvoice();
            if (ViewState["Invoices"] != null)
            {
                dt = (DataTable)ViewState["Invoices"];
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private void BindGridDatatable(DataTable dt)
    {
        try
        {
            ViewState["Invoices"] = dt;
            gvInvoice.DataSource = dt;
            gvInvoice.DataBind();
            CheckAllCheckbox();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortData();

        DataView dvInvoice = new DataView(dt);
        dvInvoice.Sort = sortExpression + direction;

        BindGridDatatable(dvInvoice.ToTable());
    }
    private void GetSelectedInvoice()
    {
        DataTable dtInvoice = new DataTable();
        dtInvoice = (DataTable)ViewState["Invoices"];

        foreach (GridViewRow row in gvInvoice.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            TextBox txtPAmount = (TextBox)row.FindControl("txtPAmount");
            HiddenField hdnPrevDue = (HiddenField)row.FindControl("hdnPrevDue");
            HiddenField hdnID = (HiddenField)row.FindControl("hdnID");
            double pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            double due = double.Parse(hdnPrevDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            int tref = Convert.ToInt32(hdnID.Value);

            if (!pay.Equals(0))
            {
                chkSelect.Checked = true;
                DataRow[] drInv = dtInvoice.Select("Ref = " + tref);

                foreach(var dr in drInv)
                {
                    dr["paymentAmt"] = Convert.ToDouble(pay);
                    dr["DueAmount"] = due - pay;
                }
            }
        }
        ViewState["Invoices"] = dtInvoice;
    }
    #endregion


    protected void cvAmount_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            double total = 0;
            foreach(GridViewRow gr in gvInvoice.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if(chkSelect.Checked.Equals(true))
                {
                    TextBox txtPayAmt = (TextBox)gr.FindControl("txtPAmount");
                    if(!string.IsNullOrEmpty(txtPayAmt.Text))
                    {
                        total += double.Parse(txtPayAmt.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
                    }
                }
            }
          
            if(total.Equals(double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint)))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}