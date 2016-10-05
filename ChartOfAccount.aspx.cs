using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using System.Globalization;


public partial class ChartOfAccount : System.Web.UI.Page
{
    #region Variables

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    AccountType _objAcType = new AccountType();
    BL_AccountType _objBLAcType = new BL_AccountType();

    private const string _asc = " ASC";
    private const string _desc = " DESC";

    #endregion

    #region Events
    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (GridViewRow gr in gvChartOfAccount.Rows)
        //{
        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //    Label lblId = (Label)gr.FindControl("lblId");

        //    AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
        //    bool _editFinance = (bool)Session["EditFinance"];
        //    if (_editFinance.Equals(false))
        //    {
        //        gr.Attributes["ondblclick"] = "window.open('permissionissue.aspx');";
        //    }
        //    else
        //    {
        //        gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvChartOfAccount.ClientID + "',event);";
        //        gr.Attributes["ondblclick"] = "window.open('addcoa.aspx?id=" + lblId.Text + "');";
        //    }
        //}
        foreach (GridViewRow gr in gvChartOfAccount.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblId = (Label)gr.FindControl("lblId");

            //gr.Cells[6].Attributes["ondblclick"] = "window.open('accountledger.aspx?id=" + lblId.Text + "');";
            gr.Cells[6].Attributes["ondblclick"] = "window.location.assign('accountledger.aspx?id=" + lblId.Text + "');";
            gr.Cells[7].Attributes["ondblclick"] = "window.location.assign('accountledger.aspx?id=" + lblId.Text + "');";
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvChartOfAccount.ClientID + "');", true);
    }
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
                txtSearch.Visible = false;
                ddlBalanceCondition.Visible = false;
                BindChart();
                SetTotal();
                FillStatus();
                FillAccountType();
                FillSubAccount();
           
            }
            Permission();
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
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addcoa.aspx", false);   
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            //bool _editFinance = (bool)Session["EditFinance"];
            foreach (GridViewRow di in gvChartOfAccount.Rows)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblID = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {    
                    Response.Redirect("addcoa.aspx?id=" + lblID.Text, false);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow di in gvChartOfAccount.Rows)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblID = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {
                    _objChart.ID = Convert.ToInt32(lblID.Text);

                    //check if balance is zero
                    _objChart.ConnConfig = Session["config"].ToString();

                    DataSet _dsChartDetail = new DataSet();
                    _dsChartDetail = _objBLChart.GetChart(_objChart);
                    double _balance = Convert.ToDouble(_dsChartDetail.Tables[0].Rows[0]["Balance"].ToString());
                    int _defaultAcct = Convert.ToInt32(_dsChartDetail.Tables[0].Rows[0]["DefaultNo"].ToString());
                    if (!_balance.Equals(0))
                    {
                    
                        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "notifyDelete();", true);
                    }
                    else
                    {
                        if (_defaultAcct.Equals(0))
                        {
                            _objBLChart.DeleteChart(_objChart);

                            BindChart();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "defaultNotifyDelete();", true);
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
    protected void gvChartOfAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvChartOfAccount.PageIndex = e.NewPageIndex;

            if (Session["Chart"] != null)
            {
                //gvChartOfAccount.DataSource = (DataSet)Session["ChartResult"];
                gvChartOfAccount.DataSource = Session["Chart"];
                gvChartOfAccount.DataBind();
            }
            else
            {
                BindChart();
                SetTotal();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvChartOfAccount_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            string _sortExpression = e.SortExpression;

            if (GvSortDirection == SortDirection.Ascending)
            {
                GvSortDirection = SortDirection.Descending;
                SortGridView(_sortExpression, _desc);
                SetTotal();
            }
            else
            {
                GvSortDirection = SortDirection.Ascending;
                SortGridView(_sortExpression, _asc);
                SetTotal();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvChartOfAccount_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {

            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    //if (gvChartOfAccount.Rows.Count > 0)
                    //{
                    //    gvChartOfAccount.HeaderRow.TableSection = TableRowSection.TableHeader;
                    //}

                    break;
                case DataControlRowType.Footer:
                    Label lblGrandTotalDebit = default(Label);
                    lblGrandTotalDebit = (Label)e.Row.FindControl("lblGrandTotalDebit");

                    Label lblGrandTotalCredit = default(Label);
                    lblGrandTotalCredit = (Label)e.Row.FindControl("lblGrandTotalCredit");

                    lblGrandTotalDebit.Text = string.Format("{0:c}", Convert.ToDouble(ViewState["GrandTotalDebit"]));
                    lblGrandTotalCredit.Text =  string.Format("{0:c}", Convert.ToDouble(ViewState["GrandTotalCredit"]));

                    break;
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void gvChartOfAccount_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            Paginate(sender, e);
            switch (e.CommandName)
            {

                case "LACTIVE":

                    break; // TODO: might not be correct. Was : Exit Select

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow di in gvChartOfAccount.Rows)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblID = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {
                    Response.Redirect("addcoa.aspx?id=" + lblID.Text + "&c=1");
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Chart _objC = new Chart();
            if (ddlSearch.SelectedIndex != 0)
            {
                _objC.SearchIndex = Convert.ToInt32(ddlSearch.SelectedIndex);
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    switch (ddlSearch.SelectedIndex)
                    {
                        case 1:
                            _objC.SearchBy = txtSearch.Text;
                            break;
                        case 2:
                            _objC.SearchBy = txtSearch.Text;
                            break;
                        case 3:
                            if (ddlBalanceCondition.SelectedIndex != 0)
                            {
                                _objC.SearchBy = txtSearch.Text;
                                _objC.Condition = ddlBalanceCondition.SelectedItem.Text;
                            }
                            break;
                    }
                }
            }
            if (ddlStatus.SelectedValue != " Select Status ")
                _objC.SearchStatus = Convert.ToInt32(ddlStatus.SelectedValue);
            if (ddlType.SelectedValue != " Select Account Type ")
                _objC.SearchAcctType = Convert.ToInt32(ddlType.SelectedValue);
            if (ddlSubAcCategory.SelectedValue != " Select Sub Category ")
                _objC.Sub = ddlSubAcCategory.SelectedItem.Text;

            _objC.ConnConfig = Session["config"].ToString();

            DataSet _dsChart = new DataSet();
            _dsChart = _objBLChart.GetAccountData(_objC);
           
            if (_dsChart != null && _dsChart.Tables.Count > 0)
            {
                //Session["ChartResult"] = _dsChart;
                Session["Chart"] = _dsChart.Tables[0];
                gvChartOfAccount.DataSource = _dsChart;
                gvChartOfAccount.DataBind();
            }
            lblRecordCount.Text = _dsChart.Tables[0].Rows.Count.ToString() + " Record(s) Found.";
            SetTotal();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvrPager = gvChartOfAccount.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

            gvChartOfAccount.PageIndex = ddlPages.SelectedIndex;

            // a method to populate your grid
            FillGridPaged();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvChartOfAccount_DataBound(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvrPager = gvChartOfAccount.BottomPagerRow;

            if (gvrPager == null) return;

            // get your controls from the gridview
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

            if (ddlPages != null)
            {
                // populate pager
                for (int i = 0; i < gvChartOfAccount.PageCount; i++)
                {

                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());

                    if (i == gvChartOfAccount.PageIndex)
                        lstItem.Selected = true;

                    ddlPages.Items.Add(lstItem);
                }
            }

            // populate page count
            if (lblPageCount != null)
                lblPageCount.Text = gvChartOfAccount.PageCount.ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void Paginate(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvChartOfAccount.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvChartOfAccount.PageIndex = 0;
                break;
            case "prev":
                gvChartOfAccount.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvChartOfAccount.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvChartOfAccount.PageIndex = gvChartOfAccount.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridPaged();
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlBalanceCondition.SelectedIndex = 0;
        if (ddlSearch.SelectedIndex == 0)
        {
            txtSearch.Visible = false;
            ddlBalanceCondition.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 1)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 2)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 3)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = true;
        }
    }
  
    #endregion

    #region Custom Functions
    private void FillGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortData();
        BindGridDatatable(dt);
        SetTotal();
        //gvUsers.DataSource = dt;
        //gvUsers.DataBind();       
    }
    private void BindChart()
    {
        try
        {
            DataSet dsChart = new DataSet();
            _objChart.ConnConfig = Session["config"].ToString();

            dsChart = _objBLChart.GetAll(_objChart);

            double _debitVal = 0;
            double _creditVal = 0;

            if (ViewState["GrandTotalDebit"] == null || ViewState["GrandTotalCredit"] == null)
            {
                if (dsChart.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsChart.Tables[0].Rows)
                    {
                        if (!Convert.ToInt16(dr["Type"]).Equals(7))
                        {
                            if (Convert.ToDouble(dr["Balance"]) < 0)
                                _creditVal = _creditVal + (Convert.ToDouble(dr["Balance"]) * -1);
                            else
                                _debitVal = _debitVal + Convert.ToDouble(dr["Balance"]);
                        }
                    }
                }

                ViewState["GrandTotalDebit"] = _debitVal;
                ViewState["GrandTotalCredit"] = _creditVal;
            }
         
            Session["Chart"] = dsChart.Tables[0];
            lblRecordCount.Text = dsChart.Tables[0].Rows.Count.ToString() + " Record(s) Found.";
            gvChartOfAccount.DataSource = dsChart;
            gvChartOfAccount.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", false);
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
    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        if (Session["Chart"] != null)
        {
            dt = (DataTable)Session["Chart"];
        }
        else
        {
            DataSet _dsChart = new DataSet();
            _objChart.ConnConfig = Session["config"].ToString();
            _dsChart = _objBLChart.GetAll(_objChart);
            Session["Chart"] = _dsChart.Tables[0];
            dt = _dsChart.Tables[0];
        }
        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";
        return dt;
    }
    private void SetTotal()
    {
        try
        {
            if(gvChartOfAccount.Rows.Count > 0)
            {
                double _debitAmount = 0.00, _creditAmount = 0.00;
                foreach (GridViewRow gr in gvChartOfAccount.Rows)
                {
                    HiddenField hdnTypeId = gr.FindControl("hdnTypeId") as HiddenField;
                    Label lblDebit = gr.FindControl("lblDebit") as Label;
                    Label lblCredit = gr.FindControl("lblCredit") as Label;

                    if (!Convert.ToInt16(hdnTypeId.Value).Equals(7))
                    {
                        if (!string.IsNullOrEmpty(lblDebit.Text))
                        {
                            _debitAmount = _debitAmount + double.Parse(lblDebit.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                        NumberStyles.AllowThousands |
                                        NumberStyles.AllowDecimalPoint);
                        }
                        if (!string.IsNullOrEmpty(lblCredit.Text))
                        {
                            _creditAmount = _creditAmount + double.Parse(lblCredit.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                        NumberStyles.AllowThousands |
                                        NumberStyles.AllowDecimalPoint);
                        }
                    }
                }
                if (gvChartOfAccount.FooterRow != null)
                {
                    Label lblTotalDebit = gvChartOfAccount.FooterRow.FindControl("lblTotalDebit") as Label;
                    Label lblTotalCredit = gvChartOfAccount.FooterRow.FindControl("lblTotalCredit") as Label;
                    
                    lblTotalDebit.Text = string.Format("{0:c}", _debitAmount);
                    lblTotalCredit.Text = string.Format("{0:c}", _creditAmount);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        //DataSet _dsCharts = new DataSet();
        //_objChart.ConnConfig = Session["config"].ToString();
        //_dsCharts = _objBLChart.GetAll(_objChart);

        DataTable dt = PageSortData();

        DataView _dvChart = new DataView(dt);
        //DataView _dvChart = new DataView(_dsCharts.Tables[0]);
        _dvChart.Sort = sortExpression + direction;

        BindGridDatatable(_dvChart.ToTable());
    }
    private void BindGridDatatable(DataTable dt)
    {
        Session["Chart"] = dt;
        gvChartOfAccount.DataSource = dt;
        gvChartOfAccount.DataBind();
        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";
    }
   
    #region Fill Status
    private void FillStatus()
    {
        try
        {
            DataSet _dsStatus = new DataSet();
            _dsStatus = _objBLChart.GetAllStatus(_objChart);
            if (_dsStatus.Tables.Count > 0)
            {
                ddlStatus.Items.Add(new ListItem(" Select Status "));
                ddlStatus.AppendDataBoundItems = true;
                ddlStatus.DataSource = _dsStatus;
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Status";
                ddlStatus.DataBind();
            }
            else
            {
                ddlStatus.Items.Insert(0, new ListItem(" No Status Available ", "0"));
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Fill Account Type
    private void FillAccountType()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            DataSet _dsType = new DataSet();
            _dsType = _objBLAcType.GetAllType(_objAcType);

            if (_dsType.Tables.Count > 0)
            {
                ddlType.Items.Add(new ListItem(" Select Account Type "));
                ddlType.AppendDataBoundItems = true;
                ddlType.DataSource = _dsType;
                ddlType.DataValueField = "ID";
                ddlType.DataTextField = "Type";
                ddlType.DataBind();
            }
            else
            {
                ddlType.Items.Insert(0, new ListItem(" No Account Type Available ", "0"));
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Fill Sub Account
    private void FillSubAccount()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            // _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);

            DataSet _dsSubType = new DataSet();
            _dsSubType = _objBLAcType.GetAllSubAccout(_objAcType);

            if (_dsSubType != null)
            {
                // ddlSubAcCategory.Items.Clear();

                //_dsSubType.Tables[0] = _dsSubType.Tables[0].DefaultView.ToTable();
                if (_dsSubType.Tables.Count > 0)
                {
                    ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                    //ddlSubAcCategory.Items.Add(new ListItem(" < Add New > ", "0"));
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
    #endregion

    #endregion

   
}