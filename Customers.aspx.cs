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
using QBFC12Lib;
using System.Web.Script.Serialization;
using System.Globalization;

public partial class Customers : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    private bool booSessionBegun;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        Permission();

        if (!IsPostBack)
        {
            //ShowQBSyncControls();
            FillCustomerType();
            GetDataAll();
            ConvertToJSON();
        }
        RowSelect();
    }

    private void ShowQBSyncControls()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
            {
                lnkSyncQB.Visible = true;
            }
            else
            {
                lnkSyncQB.Visible = false;
            }
        }
    }

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

    private List<CustomerReport> GetReportsName()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.Type = "Customer";
            dsGetReports = objBL_ReportsData.GetStockReports(objProp_User);
            //if (dsGetReports.Tables.Count > 0)
            for (int i = 0; i <= dsGetReports.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustomerReport = new CustomerReport();
                //drpReports.DataSource = dsGetReports.Tables[0];
                //drpReports.DataTextField = "ReportName";
                //drpReports.DataValueField = "Id";
                //drpReports.DataBind();

                //drpReports.Items.Insert(0, "Print");
                //drpReports.SelectedIndex = 0;

                objCustomerReport.ReportId = Convert.ToInt32(dsGetReports.Tables[0].Rows[i]["Id"]);
                objCustomerReport.ReportName = dsGetReports.Tables[0].Rows[i]["ReportName"].ToString();
                objCustomerReport.IsGlobal = Convert.ToBoolean(dsGetReports.Tables[0].Rows[i]["IsGlobal"]);

                lstCustomerReport.Add(objCustomerReport);
            }
        }
        catch (Exception ex)
        {
            //
        }
        return lstCustomerReport;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }

    private void FillCustomerType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomerType(objProp_User);
        ddlUserType.DataSource = ds.Tables[0];
        ddlUserType.DataTextField = "Type";
        ddlUserType.DataValueField = "Type";
        ddlUserType.DataBind();

        //ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void GetDataAll()
    {

        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        ds = objBL_User.getCustomers(objProp_User);

        if (ds.Tables.Count > 0)
        {
            //gvUsers.DataSource = ds.Tables[0];
            //gvUsers.DataBind();
            //CalculateBalance();
            //RowSelect();

            //Session["customers"] = ds.Tables[0];
            //Session["searchdata"] = ds.Tables[0];
            lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) Found.";
            BindGridDatatable(ds.Tables[0]);
        }
    }

    private void FillGrid()
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = getdata();
        BindGridDatatable(dt);
        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";
        //gvUsers.DataSource = dt;
        //gvUsers.DataBind();
        //CalculateBalance();
        //RowSelect();
    }

    private void FillGridPaged()
    {
        DataTable dt = new DataTable();

        dt = PageSortData();
        BindGridDatatable(dt);
        //gvUsers.DataSource = dt;
        //gvUsers.DataBind();
        //CalculateBalance();
        //RowSelect();
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortData();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindGridDatatable(dv.ToTable());
        //gvUsers.DataSource = dv;
        //gvUsers.DataBind();
        //CalculateBalance();
        //RowSelect();
    }

    private void BindGridDatatable(DataTable dt)
    {
        Session["customers"] = dt;
        Session["searchdata"] = dt;
        gvUsers.DataSource = dt;
        gvUsers.DataBind();
        CalculateBalance();
        RowSelect();

        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
            gvUsers.Columns[2].Visible = true;
        else
            gvUsers.Columns[2].Visible = false;
    }

    private void BindGridDatatable(DataView dv)
    {
        gvUsers.DataSource = dv;
        gvUsers.DataBind();
        CalculateBalance();
        RowSelect();

        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
            gvUsers.Columns[2].Visible = true;
        else
            gvUsers.Columns[2].Visible = false;
    }

    private DataTable getdata()
    {
        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        if (ddlSearch.SelectedIndex != 0)
        {
            objProp_User.SearchBy = ddlSearch.SelectedValue;
            objProp_User.SearchValue = txtSearch.Text;

            if (ddlSearch.SelectedValue == "o.Status")
            {
                objProp_User.SearchValue = rbStatus.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "o.type")
            {
                objProp_User.SearchValue = ddlUserType.SelectedValue;
            }

            ds = objBL_User.getCustomerSearch(objProp_User);
        }
        else
        {
            ds = objBL_User.getCustomers(objProp_User);
        }

        DataTable dt = ds.Tables[0];
        Session["searchdata"] = dt;
        return dt;
    }

    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["searchdata"];
        return dt;
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Paginate(sender, e);
    }

    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string gvrow = ((GridView)sender).DataKeys[e.Row.RowIndex].Value.ToString();
        //    HiddenField hdnSelected = (HiddenField)e.Row.FindControl("hdnSelected");
        //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");

        //    e.Row.Attributes["ondblclick"] = "location.href='addcustomer.aspx?uid=" + gvrow + "'";
        //    e.Row.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvUsers.ClientID + "');";

        //}
    }

    private void RowSelect()
    {
        foreach (GridViewRow gr in gvUsers.Rows)
        {
            Label lblUserID = (Label)gr.Cells[1].FindControl("lblId");
            HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["ondblclick"] = "location.href='addcustomer.aspx?uid=" + lblUserID.Text + "'";
            gr.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvUsers.ClientID + "',event);";

        }
        ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvUsers.ClientID + "');", true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvUsers.Rows)
        {

            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblUserID = (Label)di.Cells[1].FindControl("lblId");

            if (hdnSelected.Value == "1")
            {
                Response.Redirect("addcustomer.aspx?uid=" + lblUserID.Text);
            }
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvUsers.Rows)
        {
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblUserID = (Label)di.Cells[1].FindControl("lblId");

            if (hdnSelected.Value == "1")
            {
                Response.Redirect("addcustomer.aspx?uid=" + lblUserID.Text + "&t=c");
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvUsers.Rows)
        {
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblUserID = (Label)di.Cells[1].FindControl("lblId");

            if (hdnSelected.Value == "1")
            {
                DeleteCustomer(Convert.ToInt32(lblUserID.Text));
            }
        }
    }

    private void DeleteCustomer(int CustID)
    {
        objProp_User.CustomerID = CustID;
        objProp_User.ConnConfig = Session["config"].ToString();

        try
        {
            objBL_User.DeleteCustomer(objProp_User);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Customer deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            FillGrid();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addcustomer.aspx");
    }

    protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvUsers.PageIndex = e.NewPageIndex;
        FillGridPaged();
    }

    protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        GetDataAll();
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlSearch_SelectedIndexChanged(sender, e);
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSearch.SelectedIndex == 5)
        //{
        //    ddlUserType.Visible = true;
        //    txtSearch.Visible = false;
        //    rbStatus.Visible = false;
        //}
        //else 
        if (ddlSearch.SelectedIndex == 3)
        {
            rbStatus.Visible = true;
            txtSearch.Visible = false;
            ddlUserType.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 4)
        {
            rbStatus.Visible = false;
            txtSearch.Visible = false;
            ddlUserType.Visible = true;
        }
        else
        {
            rbStatus.Visible = false;
            txtSearch.Visible = true;
            ddlUserType.Visible = false;
        }
    }

    private void CalculateBalance()
    {
        try
        {
            DataTable dt = (DataTable)Session["searchdata"];
            double dblBalTotal = 0;
            int locs = 0;
            int equip = 0;
            int opencall = 0;

            if (dt.Rows.Count > 0)
            {
                //foreach (GridViewRow gr in gvUsers.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    //Label lblBalance = (Label)gr.FindControl("lblBalance");

                    //if (lblBalance.Text != string.Empty)
                    //{
                    //    dblBalTotal += Convert.ToDouble(lblBalance.Text);
                    //}
                    if (dr["balance"].ToString() != string.Empty)
                    {
                        dblBalTotal += Convert.ToDouble(dr["balance"].ToString());
                    }
                    if (dr["loc"].ToString() != string.Empty)
                    {
                        locs += Convert.ToInt32(dr["loc"].ToString());
                    }
                    if (dr["equip"].ToString() != string.Empty)
                    {
                        equip += Convert.ToInt32(dr["equip"].ToString());
                    }
                    if (dr["opencall"].ToString() != string.Empty)
                    {
                        opencall += Convert.ToInt32(dr["opencall"].ToString());
                    }
                }

                Label lblBalanceTotal = (Label)gvUsers.FooterRow.FindControl("lblBalanceTotal");
                Label lblLocTotal = (Label)gvUsers.FooterRow.FindControl("lblLocsTotal");
                Label lblEquipTotal = (Label)gvUsers.FooterRow.FindControl("lblequipTotal");
                Label lblopencallTotal = (Label)gvUsers.FooterRow.FindControl("lblopencallTotal");

                if(dblBalTotal < 0)   // change to display negative balance in red color with positive value by dev 3rd Feb,16
                {
                    lblBalanceTotal.ForeColor = System.Drawing.Color.Red;
                    lblBalanceTotal.Text = string.Format("{0:c}", dblBalTotal * -1);
                }
                else
                {
                    lblBalanceTotal.Text = string.Format("{0:c}", dblBalTotal);
                }
            
                lblLocTotal.Text = locs.ToString();
                lblEquipTotal.Text = equip.ToString();
                lblopencallTotal.Text = opencall.ToString();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void gvUsers_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvUsers.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvUsers.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvUsers.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvUsers.PageCount.ToString();
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvUsers.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvUsers.PageIndex = ddlPages.SelectedIndex;

        // a method to populate your grid
        FillGridPaged();
    }

    protected void Paginate(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvUsers.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvUsers.PageIndex = 0;
                break;
            case "prev":
                gvUsers.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvUsers.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvUsers.PageIndex = gvUsers.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridPaged();
    }

    private string IsNull(string input, string replacement)
    {
        string output = string.Empty;

        if (!string.IsNullOrEmpty(input))
        {
            output = input;
        }
        else
        {
            output = replacement;
        }

        return output;
    }

    private string SuffixSpace(string input)
    {
        string output = string.Empty;

        if (input != string.Empty)
        {
            output = input + " ";
        }

        return output;
    }

    //////////// QB Sync ///////////////
    #region QB Sync

    public void QBCustomerSync()
    {
        #region Table Schema
        DataTable dt = new DataTable();
        dt.Columns.Add("ListID", typeof(string));
        dt.Columns.Add("CustomerName", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MainContact", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Address", typeof(string));
        dt.Columns.Add("City", typeof(string));
        dt.Columns.Add("State", typeof(string));
        dt.Columns.Add("Zip", typeof(string));
        dt.Columns.Add("IsJob", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("ParentCustID", typeof(string));
        dt.Columns.Add("BillAddress", typeof(string));
        dt.Columns.Add("BillCity", typeof(string));
        dt.Columns.Add("BillState", typeof(string));
        dt.Columns.Add("BillZip", typeof(string));
        dt.Columns.Add("LastUpdateDate", typeof(DateTime));
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("LocType", typeof(string));
        dt.Columns.Add("Status", typeof(bool));

        #endregion

        QBSessionManager sessionManager = null;
        booSessionBegun = false;

        try
        {
            #region Connection to QB

            string path = "";
            DataSet dsC = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objProp_User);
            if (dsC.Tables[0].Rows.Count > 0)
            {
                path = dsC.Tables[0].Rows[0]["QBPath"].ToString();
            }

            IMsgSetRequest requestMsgSet;
            IMsgSetResponse responseMsgSet;
            //QBSessionManager sessionManager = null;
            //bool booSessionBegun = false;            
            sessionManager = new QBSessionManager();
            sessionManager.CommunicateOutOfProcess(true);
            sessionManager.QBAuthPreferences.PutIsReadOnly(false);
            sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
            sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);
            sessionManager.OpenConnection2("", "Mobile Service Manager", ENConnectionType.ctLocalQBD);
            sessionManager.BeginSession(path, ENOpenMode.omDontCare);

            //"C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Company Files\\Elevator Refurbishing Corp.qbw"
            //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\IdeavateSol.qbw", ENOpenMode.omDontCare);

            if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
            {
                throw new Exception("Auth Not Obeyed!!");
            }
            booSessionBegun = true;
            requestMsgSet = getLatestMsgSetRequest(sessionManager);
            requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;
            #endregion

            DateTime LastSycnDate = System.DateTime.MinValue;
            DataSet dsTime = new DataSet();
            //String FormatDate = string.Empty;
            objProp_User.ConnConfig = Session["config"].ToString(); ;
            dsTime = objBL_User.getControl(objProp_User);
            if (dsTime.Tables[0].Rows[0]["QBLastSync"] != DBNull.Value)
            {
                LastSycnDate = Convert.ToDateTime(dsTime.Tables[0].Rows[0]["QBLastSync"]);
                //FormatDate = LastSycnDate.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            #region Sync Customer type
            /// Import
            requestMsgSet.ClearRequests();
            ICustomerTypeQuery Custtype = requestMsgSet.AppendCustomerTypeQueryRq();
            if (LastSycnDate != System.DateTime.MinValue)
            {
                Custtype.ORListQuery.ListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responses = responseMsgSet.ResponseList.GetAt(0);
            ICustomerTypeRetList invoiceRets = responses.Detail as ICustomerTypeRetList;
            if (invoiceRets != null)
            {
                if (!(invoiceRets.Count == 0))
                {
                    int rowcount = invoiceRets.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        ICustomerTypeRet invoiceRet1 = invoiceRets.GetAt(ndx);

                        objProp_User.QBCustomerTypeID = invoiceRet1.ListID.GetValue();
                        objProp_User.CustomerType = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 50);
                        objProp_User.Remarks = invoiceRet1.FullName.GetValue();
                        objProp_User.ConnConfig = Session["config"].ToString();

                        objBL_User.AddQBCustomerType(objProp_User);
                    }
                }
            }

            /// Export
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet dsCustomertype = new DataSet();
            dsCustomertype = objBL_User.getMSMCustomertype(objProp_User);
            foreach (DataRow dr in dsCustomertype.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ICustomerTypeAdd CustomertypeReq = requestMsgSet.AppendCustomerTypeAddRq();
                CustomertypeReq.Name.SetValue(objGeneralFunctions.Truncate(dr["type"].ToString(), 31));

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ICustomerTypeRet customertype = (ICustomerTypeRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBCustomerTypeID = customertype.ListID.GetValue();
                    objProp_User.CustomerType = dr["type"].ToString();
                    objBL_User.UpdateQBcustomertypeID(objProp_User);
                }
            }

            #endregion

            #region Sync Location Type
            /// Import
            requestMsgSet.ClearRequests();
            IJobTypeQuery LocType = requestMsgSet.AppendJobTypeQueryRq();
            if (LastSycnDate != System.DateTime.MinValue)
            {
                LocType.ORListQuery.ListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responseJobType = responseMsgSet.ResponseList.GetAt(0);
            IJobTypeRetList invoiceRetJobType = responseJobType.Detail as IJobTypeRetList;
            if (invoiceRetJobType != null)
            {
                if (!(invoiceRetJobType.Count == 0))
                {
                    int rowcount = invoiceRetJobType.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        IJobTypeRet invoiceRet1 = invoiceRetJobType.GetAt(ndx);

                        objProp_User.QBCustomerTypeID = invoiceRet1.ListID.GetValue();
                        objProp_User.CustomerType = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 50);
                        objProp_User.Remarks = invoiceRet1.FullName.GetValue();
                        objProp_User.ConnConfig = Session["config"].ToString();

                        objBL_User.AddQBLocType(objProp_User);
                    }
                }
            }

            /// Export
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet dsJobtype = new DataSet();
            dsJobtype = objBL_User.getMSMLoctype(objProp_User);
            foreach (DataRow dr in dsJobtype.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                IJobTypeAdd JobtypeReq = requestMsgSet.AppendJobTypeAddRq();
                JobtypeReq.Name.SetValue(objGeneralFunctions.Truncate(dr["type"].ToString(), 31));

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    IJobTypeRet jobtype = (IJobTypeRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBCustomerTypeID = jobtype.ListID.GetValue();
                    objProp_User.CustomerType = dr["type"].ToString();
                    objBL_User.UpdateQBJobtypeID(objProp_User);
                }
            }

            #endregion

            #region Sync Sales Tax
            /// Import
            requestMsgSet.ClearRequests();
            ISalesTaxCodeQuery SalesTax = requestMsgSet.AppendSalesTaxCodeQueryRq();
            if (LastSycnDate != System.DateTime.MinValue)
            {
                SalesTax.ORListQuery.ListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responseSalesTax = responseMsgSet.ResponseList.GetAt(0);
            ISalesTaxCodeRetList SalesTaxRet = responseSalesTax.Detail as ISalesTaxCodeRetList;
            if (SalesTaxRet != null)
            {
                if (!(SalesTaxRet.Count == 0))
                {
                    int rowcount = SalesTaxRet.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        ISalesTaxCodeRet invoiceRet1 = SalesTaxRet.GetAt(ndx);

                        objProp_User.QBSalesTaxID = invoiceRet1.ListID.GetValue();
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objProp_User.SalesTax = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 25);
                        objProp_User.SalesDescription = objGeneralFunctions.Truncate(invoiceRet1.Desc.GetValue(), 75);
                        objProp_User.SalesRate = 0;
                        objProp_User.State = "";
                        objProp_User.Remarks = "";
                        objProp_User.IsTaxable = Convert.ToInt32(invoiceRet1.IsTaxable.GetValue());
                        objBL_User.AddQBSalesTax(objProp_User);
                    }
                }
            }

            /// Export
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getMSMSalesTax(objProp_User);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ISalesTaxCodeAdd SalestaxReq = requestMsgSet.AppendSalesTaxCodeAddRq();
                SalestaxReq.Name.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 3));
                SalestaxReq.Desc.SetValue(objGeneralFunctions.Truncate(dr["fdesc"].ToString(), 31));
                SalestaxReq.IsTaxable.SetValue(Convert.ToBoolean(dr["IStax"].ToString()));

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ISalesTaxCodeRet salestax = (ISalesTaxCodeRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBSalesTaxID = salestax.ListID.GetValue();
                    objProp_User.SalesTax = dr["name"].ToString();
                    objBL_User.UpdateQBsalestaxID(objProp_User);
                }
            }
            ////DataSet dsQBSalestax = new DataSet();
            ////dsQBSalestax = objBL_User.getQBSalesTax(objProp_User);
            ////foreach (DataRow dr in dsQBSalestax.Tables[0].Rows)
            ////{
            ////    requestMsgSet.ClearRequests();
            ////    ISalesTaxCodeQuery salestaxQ = requestMsgSet.AppendSalesTaxCodeQueryRq();
            ////    salestaxQ.ORListQuery.ListIDList.Add(dr["QBstaxID"].ToString());
            ////    responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            ////    IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
            ////    ISalesTaxCodeRetList salestaxRetList = thisResponse.Detail as ISalesTaxCodeRetList;
            ////    if (salestaxRetList != null)
            ////    {
            ////        if (!(salestaxRetList.Count == 0))
            ////        {
            ////            ISalesTaxCodeRet salestaxRet = salestaxRetList.GetAt(0);
            ////            string editSequence = salestaxRet.EditSequence.GetValue();
            ////            DateTime lastUpdateDateQB = salestaxRet.TimeModified.GetValue();

            ////            if (dr["LastUpdateDate"] != DBNull.Value)
            ////            {
            ////                if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
            ////                {
            ////                    requestMsgSet.ClearRequests();
            ////                    ISalesTaxCodeMod staxcodeMod = requestMsgSet.AppendSalesTaxCodeModRq();
            ////                    staxcodeMod.ListID.SetValue(dr["QBstaxID"].ToString());
            ////                    staxcodeMod.EditSequence.SetValue(editSequence);
            ////                    staxcodeMod.Name.SetValue(dr["name"].ToString());

            ////                    responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            ////                    IResponse thisResponse1 = responseMsgSet.ResponseList.GetAt(0);
            ////                }
            ////            }
            ////        }
            ////        else
            ////        {

            ////        }
            ////    }
            ////}
            #endregion

            #region Import data from QuickBooks

            requestMsgSet.ClearRequests();
            ICustomerQuery invoiceAdd = requestMsgSet.AppendCustomerQueryRq();
            //invoiceAdd.ORCustomerListQuery.CustomerListFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly);
            if (LastSycnDate != System.DateTime.MinValue)
            {
                invoiceAdd.ORCustomerListQuery.CustomerListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse response = responseMsgSet.ResponseList.GetAt(0);
            ICustomerRetList invoiceRet = response.Detail as ICustomerRetList;
            if (invoiceRet != null)
            {
                if (!(invoiceRet.Count == 0))
                {
                    int rowcount = invoiceRet.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        ICustomerRet invoiceRet1 = invoiceRet.GetAt(ndx);

                        DataRow dr = dt.NewRow();

                        if (invoiceRet1.IsActive != null)
                            if (invoiceRet1.IsActive.GetValue() == true)
                                dr["Status"] = false;
                            else
                                dr["Status"] = true;
                        else
                            dr["Status"] = 0;

                        if (invoiceRet1.TimeModified != null && invoiceRet1.TimeModified.ToString() != "")
                            dr["LastUpdateDate"] = invoiceRet1.TimeModified.GetValue();
                        else
                            dr["LastUpdateDate"] = string.Empty;

                        if (invoiceRet1.Name != null && invoiceRet1.Name.ToString() != "")
                            dr["CustomerName"] = invoiceRet1.Name.GetValue();
                        else
                            dr["CustomerName"] = string.Empty;

                        if (dr["CustomerName"] == string.Empty)
                        {
                            if (invoiceRet1.CompanyName != null && invoiceRet1.CompanyName.ToString() != "")
                                dr["CustomerName"] = invoiceRet1.CompanyName.GetValue();
                            else
                                dr["CustomerName"] = string.Empty;
                        }

                        //if (invoiceRet1.Notes != null && invoiceRet1.Notes.ToString() != "")
                        //    dr["Remarks"] = invoiceRet1.Notes.GetValue();
                        //else
                        //    dr["Remarks"] = string.Empty;

                        //if (invoiceRet1.Contact != null && invoiceRet1.Contact.ToString() != "")
                        //{
                        //    dr["MainContact"] = invoiceRet1.Contact.GetValue();
                        //}
                        //else
                        //{
                        //    dr["MainContact"] = string.Empty;
                        //}
                        string FirstName = string.Empty;
                        string MiddleName = string.Empty;
                        string LastName = string.Empty;
                        if (invoiceRet1.FirstName != null && invoiceRet1.FirstName.ToString() != "")
                        {
                            FirstName = invoiceRet1.FirstName.GetValue();
                        }
                        if (invoiceRet1.MiddleName != null && invoiceRet1.MiddleName.ToString() != "")
                        {
                            MiddleName = invoiceRet1.MiddleName.GetValue();
                        }
                        if (invoiceRet1.LastName != null && invoiceRet1.LastName.ToString() != "")
                        {
                            LastName = invoiceRet1.LastName.GetValue();
                        }

                        if (MiddleName.Trim() == string.Empty)
                        {
                            dr["MainContact"] = FirstName.Trim() + " " + LastName.Trim();
                        }
                        else
                        {
                            dr["MainContact"] = FirstName.Trim() + " " + MiddleName.Trim() + " " + LastName.Trim();
                        }


                        if (invoiceRet1.Phone != null && invoiceRet1.Phone.ToString() != "")
                            dr["Phone"] = invoiceRet1.Phone.GetValue();
                        else
                            dr["Phone"] = string.Empty;

                        if (invoiceRet1.Email != null && invoiceRet1.Email.ToString() != "")
                            dr["Email"] = invoiceRet1.Email.GetValue();
                        else
                            dr["Email"] = string.Empty;

                        //if (invoiceRet1.Mobile != null && invoiceRet1.Mobile.ToString() != "")
                        //    dr["Cell"] = invoiceRet1.Mobile.GetValue();
                        //else
                        //    dr["Cell"] = string.Empty;

                        if (invoiceRet1.ListID != null && invoiceRet1.ListID.ToString() != "")
                            dr["ListID"] = invoiceRet1.ListID.GetValue();
                        else
                            dr["ListID"] = string.Empty;

                        if (invoiceRet1.Sublevel != null && invoiceRet1.Sublevel.ToString() != "")
                            dr["IsJob"] = invoiceRet1.Sublevel.GetValue();
                        else
                            dr["IsJob"] = string.Empty;

                        if (invoiceRet1.Fax != null && invoiceRet1.Fax.ToString() != "")
                            dr["Fax"] = invoiceRet1.Fax.GetValue();
                        else
                            dr["Fax"] = string.Empty;

                        if (invoiceRet1.CustomerTypeRef != null)
                        {
                            if (invoiceRet1.CustomerTypeRef.FullName != null)
                                dr["type"] = invoiceRet1.CustomerTypeRef.FullName.GetValue();
                            else
                                dr["type"] = string.Empty;
                        }

                        if (invoiceRet1.JobTypeRef != null)
                        {
                            if (invoiceRet1.JobTypeRef.FullName != null)
                                dr["loctype"] = invoiceRet1.JobTypeRef.FullName.GetValue();
                            else
                                dr["loctype"] = string.Empty;
                        }

                        if (invoiceRet1.ParentRef != null)
                            dr["ParentCustID"] = invoiceRet1.ParentRef.ListID.GetValue();
                        else
                            dr["ParentCustID"] = string.Empty;


                        if (invoiceRet1.BillAddress != null)
                        {
                            string BillAdd1 = string.Empty;
                            string BillAdd2 = string.Empty;
                            string BillAdd3 = string.Empty;
                            string BillAdd4 = string.Empty;
                            string BillAdd5 = string.Empty;
                            string BillCity = string.Empty;
                            string BillState = string.Empty;
                            string BillZip = string.Empty;
                            string BillNotes = string.Empty;

                            if (invoiceRet1.BillAddress.Addr1 != null)
                                BillAdd1 = invoiceRet1.BillAddress.Addr1.GetValue();

                            if (invoiceRet1.BillAddress.Addr2 != null)
                                BillAdd2 = invoiceRet1.BillAddress.Addr2.GetValue();

                            if (invoiceRet1.BillAddress.Addr3 != null)
                                BillAdd3 = invoiceRet1.BillAddress.Addr3.GetValue();

                            //if (invoiceRet1.BillAddress.Addr4 != null)
                            //    BillAdd4 = invoiceRet1.BillAddress.Addr4.GetValue();

                            //if (invoiceRet1.BillAddress.Addr5 != null)
                            //    BillAdd5 = invoiceRet1.BillAddress.Addr5.GetValue();

                            if (invoiceRet1.BillAddress.City != null)
                                BillCity = invoiceRet1.BillAddress.City.GetValue();

                            if (invoiceRet1.BillAddress.State != null)
                                BillState = invoiceRet1.BillAddress.State.GetValue();

                            if (invoiceRet1.BillAddress.PostalCode != null)
                                BillZip = invoiceRet1.BillAddress.PostalCode.GetValue();

                            string BillAddress = SuffixSpace(BillAdd1) + Environment.NewLine + SuffixSpace(BillAdd2) + Environment.NewLine + SuffixSpace(BillAdd3);

                            //if (invoiceRet1.BillAddress.Note != null)
                            //    BillNotes = invoiceRet1.BillAddress.Note.GetValue();

                            dr["BillAddress"] = BillAddress.Trim();
                            dr["BillCity"] = BillCity.Trim();
                            dr["BillState"] = BillState.Trim();
                            dr["BillZip"] = BillZip.Trim();
                            dr["Remarks"] = BillNotes.Trim();
                        }

                        if (invoiceRet1.ShipAddress != null)
                        {
                            string ShipAdd1 = string.Empty;
                            string ShipAdd2 = string.Empty;
                            string ShipAdd3 = string.Empty;
                            string ShipAdd4 = string.Empty;
                            string ShipAdd5 = string.Empty;
                            string ShipCity = string.Empty;
                            string ShipState = string.Empty;
                            string ShipZip = string.Empty;
                            string ShipNotes = string.Empty;

                            if (invoiceRet1.ShipAddress.Addr1 != null)
                                ShipAdd1 = invoiceRet1.ShipAddress.Addr1.GetValue();

                            if (invoiceRet1.ShipAddress.Addr2 != null)
                                ShipAdd2 = invoiceRet1.ShipAddress.Addr2.GetValue();

                            if (invoiceRet1.ShipAddress.Addr3 != null)
                                ShipAdd3 = invoiceRet1.ShipAddress.Addr3.GetValue();

                            //if (invoiceRet1.ShipAddress.Addr4 != null)
                            //    ShipAdd4 = invoiceRet1.ShipAddress.Addr4.GetValue();

                            //if (invoiceRet1.ShipAddress.Addr5 != null)
                            //    ShipAdd5 = invoiceRet1.ShipAddress.Addr5.GetValue();

                            if (invoiceRet1.ShipAddress.City != null)
                                ShipCity = invoiceRet1.ShipAddress.City.GetValue();

                            if (invoiceRet1.ShipAddress.State != null)
                                ShipState = invoiceRet1.ShipAddress.State.GetValue();

                            if (invoiceRet1.ShipAddress.PostalCode != null)
                                ShipZip = invoiceRet1.ShipAddress.PostalCode.GetValue();

                            string ShipAddress = SuffixSpace(ShipAdd1) + Environment.NewLine + SuffixSpace(ShipAdd2) + Environment.NewLine + SuffixSpace(ShipAdd3);

                            //if (invoiceRet1.ShipAddress.Note != null)
                            //    ShipNotes = invoiceRet1.ShipAddress.Note.GetValue();

                            dr["Address"] = ShipAddress.Trim();
                            dr["City"] = ShipCity.Trim();
                            dr["State"] = ShipState.Trim();
                            dr["Zip"] = ShipZip.Trim();
                            if (ShipNotes.Trim() != string.Empty)
                            {
                                dr["Remarks"] = ShipNotes.Trim();
                            }
                        }

                        if (dr["Address"].ToString().Trim() == string.Empty)
                        {
                            dr["Address"] = dr["BillAddress"];
                            dr["City"] = dr["BillCity"];
                            dr["State"] = dr["BillState"];
                            dr["Zip"] = dr["BillZip"];
                        }

                        if (dr["Address"].ToString().Trim() == string.Empty)
                        {
                            dr["Address"] = dr["CustomerName"].ToString();
                        }

                        dt.Rows.Add(dr);

                        fCount++;
                    }

                    #region Import Customers from QB

                    var query = from row in dt.AsEnumerable()
                                where row.Field<string>("IsJob").Equals("0")
                                select row;

                    DataTable dtnew = dt.Clone();
                    foreach (var record in query)
                    {
                        DataRow drRow = dtnew.NewRow();
                        drRow = record;

                        dtnew.ImportRow(drRow);
                    }

                    foreach (DataRow dr in dtnew.Rows)
                    {
                        objProp_User.FirstName = dr["CustomerName"].ToString();
                        objProp_User.Remarks = dr["Remarks"].ToString();
                        objProp_User.MainContact = dr["MainContact"].ToString();
                        objProp_User.Phone = dr["Phone"].ToString();
                        objProp_User.Email = dr["Email"].ToString();
                        objProp_User.Cell = dr["Cell"].ToString();
                        objProp_User.QBCustomerID = dr["ListID"].ToString();
                        objProp_User.Address = dr["Address"].ToString();
                        objProp_User.City = dr["City"].ToString();
                        objProp_User.State = dr["State"].ToString();
                        objProp_User.Zip = dr["Zip"].ToString();
                        objProp_User.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                        objProp_User.Status = Convert.ToInt16(dr["Status"]);

                        objProp_User.Username = "";
                        objProp_User.Password = "";
                        objProp_User.Website = "";
                        objProp_User.Type = dr["type"].ToString();
                        objProp_User.Schedule = 0;
                        objProp_User.Mapping = 0;
                        objProp_User.Internet = 0;
                        objProp_User.ConnConfig = Session["config"].ToString();

                        objBL_User.AddCustomerQB(objProp_User);
                    }
                    #endregion


                    #region Import Locations from QB
                    var queryLocation = from row in dt.AsEnumerable()
                                        where row.Field<string>("IsJob").Equals("1")
                                        select row;

                    DataTable dtnewLoc = dt.Clone();
                    foreach (var record in queryLocation)
                    {
                        DataRow drRow = dtnewLoc.NewRow();
                        drRow = record;

                        dtnewLoc.ImportRow(drRow);
                    }

                    DataTable dtLoc = dtnewLoc.Clone();
                    foreach (DataRow drrow in dtnew.Rows)
                    {
                        int isHavingJob = 0;
                        foreach (DataRow drrow2 in dtnewLoc.Rows)
                        {
                            if (drrow["ListID"].ToString() == drrow2["ParentCustID"].ToString())
                            {
                                isHavingJob = 1;
                            }
                        }
                        if (isHavingJob == 0)
                        {
                            drrow["ParentCustID"] = drrow["listid"];
                            dtLoc.ImportRow(drrow);
                        }
                    }
                    foreach (DataRow dr in dtLoc.Rows)
                    {
                        dtnewLoc.ImportRow(dr);
                    }

                    foreach (DataRow dr in dtnewLoc.Rows)
                    {
                        objProp_User.AccountNo = dr["CustomerName"].ToString();
                        objProp_User.Locationname = dr["CustomerName"].ToString();
                        objProp_User.Address = dr["Address"].ToString();
                        objProp_User.Status = Convert.ToInt16(dr["Status"]);
                        objProp_User.City = dr["City"].ToString();
                        objProp_User.State = dr["State"].ToString();
                        objProp_User.Zip = dr["Zip"].ToString();
                        objProp_User.Remarks = dr["Remarks"].ToString();
                        objProp_User.MainContact = dr["MainContact"].ToString();
                        objProp_User.Phone = dr["Phone"].ToString();
                        objProp_User.Fax = dr["Fax"].ToString();
                        objProp_User.Cell = dr["Cell"].ToString();
                        objProp_User.Email = dr["Email"].ToString();
                        objProp_User.RolAddress = dr["BillAddress"].ToString();
                        objProp_User.RolCity = dr["BillCity"].ToString();
                        objProp_User.RolState = dr["BillState"].ToString();
                        objProp_User.RolZip = dr["BillZip"].ToString();
                        objProp_User.QBlocationID = dr["ListID"].ToString();
                        objProp_User.QBCustomerID = dr["ParentCustID"].ToString();
                        objProp_User.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                        objProp_User.Type = dr["loctype"].ToString();
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objBL_User.AddQBLocation(objProp_User);
                    }
                    #endregion
                }
            }

            #endregion

            #region Export data to Quickbooks

            #region Export/Add customers to QB
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet dsSalestax = new DataSet();
            dsSalestax = objBL_User.getMSMCustomers(objProp_User);
            foreach (DataRow dr in dsSalestax.Tables[0].Rows)
            {
                bool active;
                if (dr["Status"].ToString() == "1")
                {
                    active = false;
                }
                else
                {
                    active = true;
                }

                string firstname = string.Empty;
                string middlename = string.Empty;
                string lastname = string.Empty;
                if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                {
                    string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int contactLength = contact.Count();

                    if (contactLength > 0)
                    {
                        firstname = contact[0].Trim();
                    }
                    if (contactLength == 2)
                    {
                        lastname = contact[1].Trim();
                    }
                    if (contactLength > 2)
                    {
                        middlename = contact[1].Trim();
                        lastname = contact[2].Trim();
                    }
                }

                string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int intAddCount = strAddress.Count();
                string addr1 = string.Empty;
                string addr2 = string.Empty;
                string addr3 = string.Empty;
                if (intAddCount > 0)
                {
                    addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                }
                if (intAddCount > 1)
                {
                    addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                }
                if (intAddCount > 2)
                {
                    if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                    {
                        addr3 = strAddress[2].Trim();
                    }
                }
                if (intAddCount > 3)
                {
                    for (int i = 3; i < strAddress.Count(); i++)
                    {
                        addr3 += " " + strAddress[i].Trim();
                    }
                }
                addr3 = objGeneralFunctions.Truncate(addr3, 41);

                requestMsgSet.ClearRequests();
                ICustomerAdd customerReq = requestMsgSet.AppendCustomerAddRq();
                customerReq.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                customerReq.Name.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                customerReq.IsActive.SetValue(active);
                //customerReq.Notes.SetValue(dr["remarks"].ToString());
                //customerReq.Contact.SetValue(dr["contact"].ToString());
                customerReq.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                customerReq.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                customerReq.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                customerReq.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                customerReq.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                customerReq.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                customerReq.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                customerReq.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                customerReq.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                //customerReq.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                customerReq.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                customerReq.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                customerReq.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                customerReq.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                customerReq.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                customerReq.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                customerReq.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                customerReq.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                customerReq.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                customerReq.CustomerTypeRef.ListID.SetValue(dr["QBCustomertypeID"].ToString());

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ICustomerRet customer = (ICustomerRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBCustomerID = customer.ListID.GetValue();
                    objProp_User.CustomerID = Convert.ToInt32(dr["id"]);
                    objBL_User.UpdateQBCustomerID(objProp_User);
                }
            }
            #endregion

            #region Export/Update Customers to QB
            DataSet dsQB = new DataSet();
            dsQB = objBL_User.getQBCustomers(objProp_User);
            foreach (DataRow dr in dsQB.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ICustomerQuery CustQ = requestMsgSet.AppendCustomerQueryRq();
                CustQ.ORCustomerListQuery.ListIDList.Add(dr["QBCustomerID"].ToString());
                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
                if (customerRetList != null)
                {
                    if (!(customerRetList.Count == 0))
                    {
                        ICustomerRet customerRet = customerRetList.GetAt(0);
                        string editSequence = customerRet.EditSequence.GetValue();
                        DateTime lastUpdateDateQB = customerRet.TimeModified.GetValue();
                        bool active;
                        if (dr["Status"].ToString() == "1")
                        {
                            active = false;
                        }
                        else
                        {
                            active = true;
                        }
                        string firstname = string.Empty;
                        string middlename = string.Empty;
                        string lastname = string.Empty;
                        if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                        {
                            string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            int contactLength = contact.Count();

                            if (contactLength > 0)
                            {
                                firstname = contact[0].Trim();
                            }
                            if (contactLength == 2)
                            {
                                lastname = contact[1].Trim();
                            }
                            if (contactLength > 2)
                            {
                                middlename = contact[1].Trim();
                                lastname = contact[2].Trim();
                            }
                        }
                        string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        int intAddCount = strAddress.Count();
                        string addr1 = string.Empty;
                        string addr2 = string.Empty;
                        string addr3 = string.Empty;
                        if (intAddCount > 0)
                        {
                            addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                        }
                        if (intAddCount > 1)
                        {
                            addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                        }
                        if (intAddCount > 2)
                        {
                            if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                            {
                                addr3 = strAddress[2].Trim();
                            }
                        }
                        if (intAddCount > 3)
                        {
                            for (int i = 3; i < strAddress.Count(); i++)
                            {
                                addr3 += " " + strAddress[i].Trim();
                            }
                        }
                        addr3 = objGeneralFunctions.Truncate(addr3, 41);

                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                        {
                            requestMsgSet.ClearRequests();
                            ICustomerMod CustMod = requestMsgSet.AppendCustomerModRq();
                            CustMod.ListID.SetValue(dr["QBCustomerID"].ToString());
                            CustMod.EditSequence.SetValue(editSequence);
                            CustMod.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                            CustMod.Name.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                            CustMod.IsActive.SetValue(active);
                            //CustMod.Notes.SetValue(dr["remarks"].ToString());
                            //CustMod.Contact.SetValue(dr["contact"].ToString());
                            CustMod.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                            CustMod.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                            CustMod.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                            CustMod.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                            CustMod.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                            CustMod.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                            CustMod.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                            CustMod.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                            CustMod.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                            //CustMod.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                            CustMod.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                            CustMod.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                            CustMod.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                            CustMod.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                            CustMod.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                            CustMod.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                            CustMod.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                            CustMod.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                            CustMod.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                            CustMod.CustomerTypeRef.ListID.SetValue(dr["QBCustomertypeID"].ToString());

                            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                            IResponse thisResponse1 = responseMsgSet.ResponseList.GetAt(0);
                        }
                    }
                    else
                    {

                    }
                }
            }
            #endregion

            #region Export/Add Location to QB
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet dsLoc = new DataSet();
            dsLoc = objBL_User.getMSMLocation(objProp_User);
            foreach (DataRow dr in dsLoc.Tables[0].Rows)
            {
                bool active;
                if (dr["Status"].ToString() == "1")
                {
                    active = false;
                }
                else
                {
                    active = true;
                }
                string firstname = string.Empty;
                string middlename = string.Empty;
                string lastname = string.Empty;
                if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                {
                    string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int contactLength = contact.Count();

                    if (contactLength > 0)
                    {
                        firstname = contact[0].Trim();
                    }
                    if (contactLength == 2)
                    {
                        lastname = contact[1].Trim();
                    }
                    if (contactLength > 2)
                    {
                        middlename = contact[1].Trim();
                        lastname = contact[2].Trim();
                    }
                }

                string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int intAddCount = strAddress.Count();
                string addr1 = string.Empty;
                string addr2 = string.Empty;
                string addr3 = string.Empty;
                if (intAddCount > 0)
                {
                    addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                }
                if (intAddCount > 1)
                {
                    addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                }
                if (intAddCount > 2)
                {
                    if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                    {
                        addr3 = strAddress[2].Trim();
                    }
                }
                if (intAddCount > 3)
                {
                    for (int i = 3; i < strAddress.Count(); i++)
                    {
                        addr3 += " " + strAddress[i].Trim();
                    }
                }
                addr3 = objGeneralFunctions.Truncate(addr3, 41);

                string[] strShipAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int intShipAddCount = strShipAddress.Count();
                string Shipaddr1 = string.Empty;
                string Shipaddr2 = string.Empty;
                string Shipaddr3 = string.Empty;
                if (intShipAddCount > 0)
                {
                    Shipaddr1 = objGeneralFunctions.Truncate(strShipAddress[0].Trim(), 41);
                }
                if (intShipAddCount > 1)
                {
                    Shipaddr2 = objGeneralFunctions.Truncate(strShipAddress[1].Trim(), 41);
                }
                if (intShipAddCount > 2)
                {
                    if (!string.IsNullOrEmpty(strShipAddress[2].Trim()))
                    {
                        Shipaddr3 = strShipAddress[2].Trim();
                    }
                }
                if (intShipAddCount > 3)
                {
                    for (int i = 3; i < strShipAddress.Count(); i++)
                    {
                        Shipaddr3 += " " + strShipAddress[i].Trim();
                    }
                }
                Shipaddr3 = objGeneralFunctions.Truncate(Shipaddr3, 41);

                requestMsgSet.ClearRequests();
                ICustomerAdd customerReq = requestMsgSet.AppendCustomerAddRq();
                customerReq.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                customerReq.Name.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                customerReq.IsActive.SetValue(active);
                customerReq.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                customerReq.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                customerReq.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                //customerReq.Notes.SetValue(dr["remarks"].ToString());
                //customerReq.Contact.SetValue(dr["contact"].ToString());
                customerReq.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(Shipaddr1, 41));
                customerReq.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(Shipaddr2, 41));
                customerReq.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(Shipaddr3, 41));
                customerReq.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["shipcity"].ToString(), 21));
                customerReq.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["shipstate"].ToString(), 21));
                customerReq.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["shipzip"].ToString(), 13));
                // customerReq.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                customerReq.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                customerReq.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                customerReq.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                customerReq.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                customerReq.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                customerReq.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                customerReq.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                customerReq.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                customerReq.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                //customerReq.ParentRef.FullName.SetValue(dr["Name"].ToString());
                customerReq.ParentRef.ListID.SetValue(dr["qbcustomerid"].ToString());
                customerReq.JobTypeRef.ListID.SetValue(dr["QBlocTypeID"].ToString());

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ICustomerRet customer = (ICustomerRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBlocationID = customer.ListID.GetValue();
                    objProp_User.LocID = Convert.ToInt32(dr["id"]);
                    objBL_User.UpdateQBLocationID(objProp_User);
                }
            }
            #endregion

            #region Export/Update Location to QB
            DataSet dsQBLoc = new DataSet();
            dsQBLoc = objBL_User.getQBLocation(objProp_User);
            foreach (DataRow dr in dsQBLoc.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ICustomerQuery CustQ = requestMsgSet.AppendCustomerQueryRq();
                CustQ.ORCustomerListQuery.ListIDList.Add(dr["QBlocID"].ToString());
                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
                if (customerRetList != null)
                {
                    if (!(customerRetList.Count == 0))
                    {
                        ICustomerRet customerRet = customerRetList.GetAt(0);
                        string editSequence = customerRet.EditSequence.GetValue();
                        DateTime lastUpdateDateQB = customerRet.TimeModified.GetValue();

                        bool active;
                        if (dr["Status"].ToString() == "1")
                        {
                            active = false;
                        }
                        else
                        {
                            active = true;
                        }
                        string firstname = string.Empty;
                        string middlename = string.Empty;
                        string lastname = string.Empty;
                        if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                        {
                            string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            int contactLength = contact.Count();

                            if (contactLength > 0)
                            {
                                firstname = contact[0].Trim();
                            }
                            if (contactLength == 2)
                            {
                                lastname = contact[1].Trim();
                            }
                            if (contactLength > 2)
                            {
                                middlename = contact[1].Trim();
                                lastname = contact[2].Trim();
                            }
                        }

                        string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        int intAddCount = strAddress.Count();
                        string addr1 = string.Empty;
                        string addr2 = string.Empty;
                        string addr3 = string.Empty;
                        if (intAddCount > 0)
                        {
                            addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                        }
                        if (intAddCount > 1)
                        {
                            addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                        }
                        if (intAddCount > 2)
                        {
                            if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                            {
                                addr3 = strAddress[2].Trim();
                            }
                        }
                        if (intAddCount > 3)
                        {
                            for (int i = 3; i < strAddress.Count(); i++)
                            {
                                addr3 += " " + strAddress[i].Trim();
                            }
                        }
                        addr3 = objGeneralFunctions.Truncate(addr3, 41);

                        string[] strShipAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        int intShipAddCount = strShipAddress.Count();
                        string Shipaddr1 = string.Empty;
                        string Shipaddr2 = string.Empty;
                        string Shipaddr3 = string.Empty;
                        if (intShipAddCount > 0)
                        {
                            Shipaddr1 = objGeneralFunctions.Truncate(strShipAddress[0].Trim(), 41);
                        }
                        if (intShipAddCount > 1)
                        {
                            Shipaddr2 = objGeneralFunctions.Truncate(strShipAddress[1].Trim(), 41);
                        }
                        if (intShipAddCount > 2)
                        {
                            if (!string.IsNullOrEmpty(strShipAddress[2].Trim()))
                            {
                                Shipaddr3 = strShipAddress[2].Trim();
                            }
                        }
                        if (intShipAddCount > 3)
                        {
                            for (int i = 3; i < strShipAddress.Count(); i++)
                            {
                                Shipaddr3 += " " + strShipAddress[i].Trim();
                            }
                        }
                        Shipaddr3 = objGeneralFunctions.Truncate(Shipaddr3, 41);

                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                        {
                            requestMsgSet.ClearRequests();
                            ICustomerMod CustMod = requestMsgSet.AppendCustomerModRq();
                            CustMod.ListID.SetValue(dr["QBlocID"].ToString());
                            CustMod.EditSequence.SetValue(editSequence);
                            CustMod.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                            CustMod.Name.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                            CustMod.IsActive.SetValue(active);
                            CustMod.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                            CustMod.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                            CustMod.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                            //CustMod.Notes.SetValue(dr["remarks"].ToString());
                            //CustMod.Contact.SetValue(dr["contact"].ToString());
                            CustMod.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(Shipaddr1, 41));
                            CustMod.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(Shipaddr2, 41));
                            CustMod.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(Shipaddr3, 41));
                            CustMod.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["shipcity"].ToString(), 21));
                            CustMod.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["shipstate"].ToString(), 21));
                            CustMod.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["shipzip"].ToString(), 13));
                            // CustMod.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                            CustMod.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                            CustMod.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                            CustMod.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                            CustMod.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                            CustMod.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                            CustMod.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                            CustMod.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                            CustMod.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                            CustMod.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                            CustMod.JobTypeRef.ListID.SetValue(dr["QBlocTypeID"].ToString());
                            CustMod.ParentRef.ListID.SetValue(dr["qbcustomerid"].ToString());

                            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                            IResponse thisResponse1 = responseMsgSet.ResponseList.GetAt(0);
                        }
                    }
                    else
                    {

                    }
                }
            }
            #endregion

            #endregion

            objGeneral.ConnConfig = Session["config"].ToString();
            objBL_General.UpdateQBLastSync(objGeneral);

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Sync completed successfully!', dismissQueue: true, type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);//</br>Quickbooks not running. Quickbooks must be running with Admin account on server.
        }
        finally
        {
            #region Close QB Connection

            if (booSessionBegun)
            {
                sessionManager.EndSession();
            }
            booSessionBegun = false;
            sessionManager.CloseConnection();

            #endregion
        }
    }

    public IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager)
    {
        // Find and adapt to supported version of QuickBooks
        double supportedVersion = QBFCLatestVersion(sessionManager);

        short qbXMLMajorVer = 0;
        short qbXMLMinorVer = 0;

        if (supportedVersion >= 6.0)
        {
            qbXMLMajorVer = 6;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 5.0)
        {
            qbXMLMajorVer = 5;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 4.0)
        {
            qbXMLMajorVer = 4;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 3.0)
        {
            qbXMLMajorVer = 3;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 2.0)
        {
            qbXMLMajorVer = 2;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 1.1)
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 1;
        }
        else
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 0;
            //Response.Write("It seems that you are running QuickBooks 2002 Release 1. We strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements");
        }

        // Create the message set request object
        IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVer, qbXMLMinorVer);
        return requestMsgSet;
    }

    private double QBFCLatestVersion(QBSessionManager SessionManager)
    {
        // Use oldest version to ensure that this application work with any QuickBooks (US)
        IMsgSetRequest msgset = SessionManager.CreateMsgSetRequest("US", 1, 0);
        msgset.AppendHostQueryRq();
        IMsgSetResponse QueryResponse = SessionManager.DoRequests(msgset);


        IResponse response = QueryResponse.ResponseList.GetAt(0);

        // Please refer to QBFC Developers Guide for details on why 
        // "as" clause was used to link this derrived class to its base class
        IHostRet HostResponse = response.Detail as IHostRet;
        IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

        int i;
        double vers;
        double LastVers = 0;
        string svers = null;

        for (i = 0; i <= supportedVersions.Count - 1; i++)
        {
            svers = supportedVersions.GetAt(i);
            vers = Convert.ToDouble(svers);
            if (vers > LastVers)
            {
                LastVers = vers;
            }
        }
        return LastVers;
    }

    protected void lnkSyncQB_Click(object sender, EventArgs e)
    {
        QBCustomerSync();
        GetDataAll();
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlSearch_SelectedIndexChanged(sender, e);
    }

    //public void addCustomerToQB()
    //{
    //    QBSessionManager aSession = null;
    //    IMsgSetRequest requests;
    //    IMsgSetResponse responses;
    //    bool connected = false;
    //    aSession = new QBSessionManager();
    //    aSession.OpenConnection2("", "ADDCUST", ENConnectionType.ctLocalQBD);
    //    aSession.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\IdeavateSol.qbw", ENOpenMode.omDontCare);
    //    connected = true;
    //    requests = getLatestMsgSetRequest(aSession);
    //    requests.Attributes.OnError = ENRqOnError.roeStop;



    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();
    //    ds = objBL_User.getMSMCustomers(objProp_User);
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        requests.ClearRequests();
    //        ICustomerAdd customerReq = requests.AppendCustomerAddRq();
    //        customerReq.CompanyName.SetValue(dr["name"].ToString());
    //        customerReq.Name.SetValue(dr["name"].ToString());
    //        customerReq.Notes.SetValue(dr["remarks"].ToString());
    //        customerReq.Contact.SetValue(dr["contact"].ToString());
    //        customerReq.Phone.SetValue(dr["phone"].ToString());
    //        customerReq.Email.SetValue(dr["email"].ToString());
    //        customerReq.Fax.SetValue(dr["fax"].ToString());
    //        customerReq.BillAddress.Addr2.SetValue(dr["address"].ToString());
    //        customerReq.BillAddress.City.SetValue(dr["city"].ToString());
    //        customerReq.BillAddress.State.SetValue(dr["state"].ToString());
    //        customerReq.BillAddress.PostalCode.SetValue(dr["zip"].ToString());

    //        responses = aSession.DoRequests(requests);
    //        IResponse thisResponse = responses.ResponseList.GetAt(0);
    //        if (thisResponse.StatusCode == 0)
    //        {
    //            ICustomerRet customer = (ICustomerRet)thisResponse.Detail;
    //            objProp_User.ConnConfig = Session["config"].ToString();
    //            objProp_User.QBCustomerID = customer.ListID.GetValue();
    //            objProp_User.CustomerID = Convert.ToInt32(dr["id"]);
    //            objBL_User.UpdateQBCustomerID(objProp_User);
    //        }
    //    }



    //    DataSet dsQB = new DataSet();
    //    dsQB = objBL_User.getQBCustomers(objProp_User);
    //    foreach (DataRow dr in dsQB.Tables[0].Rows)
    //    {
    //        requests.ClearRequests();
    //        ICustomerQuery CustQ = requests.AppendCustomerQueryRq();
    //        CustQ.ORCustomerListQuery.ListIDList.Add(dr["QBCustomerID"].ToString());
    //        responses = aSession.DoRequests(requests);
    //        IResponse thisResponse = responses.ResponseList.GetAt(0);
    //        ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
    //        ICustomerRet customerRet = customerRetList.GetAt(0);
    //        string editSequence = customerRet.EditSequence.GetValue();
    //        DateTime lastUpdateDateQB = customerRet.TimeModified.GetValue();


    //        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
    //        {
    //            requests.ClearRequests();
    //            ICustomerMod CustMod = requests.AppendCustomerModRq();
    //            CustMod.ListID.SetValue(dr["QBCustomerID"].ToString());
    //            CustMod.EditSequence.SetValue(editSequence);
    //            CustMod.CompanyName.SetValue(dr["name"].ToString());
    //            CustMod.Name.SetValue(dr["name"].ToString());
    //            CustMod.Notes.SetValue(dr["remarks"].ToString());
    //            CustMod.Contact.SetValue(dr["contact"].ToString());
    //            CustMod.Phone.SetValue(dr["phone"].ToString());
    //            CustMod.Email.SetValue(dr["email"].ToString());
    //            CustMod.Fax.SetValue(dr["fax"].ToString());
    //            CustMod.BillAddress.Addr2.SetValue(dr["address"].ToString());
    //            CustMod.BillAddress.City.SetValue(dr["city"].ToString());
    //            CustMod.BillAddress.State.SetValue(dr["state"].ToString());
    //            CustMod.BillAddress.PostalCode.SetValue(dr["zip"].ToString());

    //            responses = aSession.DoRequests(requests);
    //            IResponse thisResponse1 = responses.ResponseList.GetAt(0);
    //        }
    //    }

    //    aSession.EndSession();
    //    connected = false;
    //    aSession.CloseConnection();
    //}			

    #endregion

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        ddlSearch_SelectedIndexChanged(sender, e);
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

    //protected void lnkPrint_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("CustomersReport.aspx", false);
    //}

    //protected void drpReports_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (drpReports.SelectedIndex != 0)
    //        {
    //            Response.Redirect("CustomersReport.aspx?reportId=" + drpReports.SelectedValue + "&reportName=" + drpReports.SelectedItem, false);
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
}
