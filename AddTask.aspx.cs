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

public partial class AddTask : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["edit"] = 0;
            FillUsers();
            ddlAssigned.SelectedValue = Session["username"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                lnkNewEmail.Visible = true;
                lnkNewEmail.NavigateUrl = "email.aspx";
                GetTask();
                lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;
                if (Request.QueryString["fl"] != null)
                {
                    ViewState["edit"] = 0;
                    lblHeader.Text = "Add Follow-Up Task";
                    if (Request.QueryString["fl"].ToString() == "2")
                    {
                        string strScript = "noty({text: 'Please create the follow-up task now.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, theme : 'noty_theme_default',  closable : false});";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyload", strScript, true);
                    }
                }
                else
                {
                    ViewState["edit"] = 1;
                    lblHeader.Text = "Edit Task";
                    chkFollowUp.Visible = true;
                }
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
            }
            if (ViewState["edit"].ToString() == "0")
            {
                pnlSysInfo.Visible = false;
                menuLeads.Visible = false;
            }

            if (Request.QueryString["rol"] != null)
            {
                hdnId.Value = Request.QueryString["rol"].ToString();
                txtName.Text = Request.QueryString["name"].ToString();
                FillTasks(hdnId.Value);
                FillContact(Convert.ToInt32(hdnId.Value));
                BindEmails(GetMailsfromdb(-1, string.Empty));
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;
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

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkTasks");
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
    private void FillTasks(string name)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "t.rol";
        objProp_Customer.SearchValue = name;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;
        
        objProp_Customer.Mode = 1;
        ds = objBL_Customer.getTasks(objProp_Customer);
        gvTasksOpen.DataSource = ds.Tables[0];
        gvTasksOpen.DataBind();
        menuLeads.Items[0].Text = "Open Tasks(" + ds.Tables[0].Rows.Count + ")";

        objProp_Customer.Mode = 0;
        ds = objBL_Customer.getTasks(objProp_Customer);
        gvTasks.DataSource = ds.Tables[0];
        gvTasks.DataBind();
        menuLeads.Items[1].Text = "Task History(" + ds.Tables[0].Rows.Count + ")";
    }
    private void GetTask()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        ds = objBL_Customer.getTasksByID(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hdnId.Value = ds.Tables[0].Rows[0]["rol"].ToString();
            FillContact(Convert.ToInt32(hdnId.Value));
            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
            txtDesc.Text= ds.Tables[0].Rows[0]["remarks"].ToString();
            if (Request.QueryString["fl"] != null)
            {
                txtDesc.Text = "Follow-Up Task." + Environment.NewLine + ds.Tables[0].Rows[0]["remarks"].ToString(); 
            }
            txtCallDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["datedue"]).ToShortDateString();
            txtCallTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timedue"]).ToShortTimeString().Replace("12:00 AM", "");
            ddlAssigned.SelectedValue = ds.Tables[0].Rows[0]["fuser"].ToString();
            lblType.Text = ds.Tables[0].Rows[0]["contacttype"].ToString();
            FillTasks(ds.Tables[0].Rows[0]["rol"].ToString());
            BindEmails(GetMailsfromdb(-1, string.Empty));
            
            if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

            if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();
            
            if (ds.Tables[0].Rows[0]["statusID"].ToString() == "1" && Request.QueryString["fl"]==null)
            {
                ddlStatus.SelectedValue = "1";
                ddlStatus.Enabled = false;
                txtResol.Enabled = true;
                txtResol.Text = ds.Tables[0].Rows[0]["result"].ToString();
            }
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = Convert.ToInt32(hdnId.Value);
        objProp_Customer.DueDate = Convert.ToDateTime(txtCallDt.Text.Trim());
        objProp_Customer.TimeDue = Convert.ToDateTime("01/01/1900 " + txtCallTime.Text.Trim());
        objProp_Customer.Subject = txtSubject.Text.Trim();
        objProp_Customer.Remarks = txtDesc.Text.Trim();
        objProp_Customer.AssignedTo = ddlAssigned.SelectedItem.Text.Trim();
        objProp_Customer.Name = Session["Username"].ToString();
        objProp_Customer.Contact = "";
        objProp_Customer.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Customer.Resolution = txtResol.Text.Trim();
        objProp_Customer.LastUpdateUser = Session["username"].ToString();

        try
        {
            string strMsg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objProp_Customer.TaskID = 0;
                objProp_Customer.Mode = 0;
                objBL_Customer.AddTask(objProp_Customer);
                objGeneralFunctions.ResetFormControlValues(this);

                gvTasks.DataSource = null;
                gvTasks.DataBind();

                gvTasksOpen.DataSource = null;
                gvTasksOpen.DataBind();

                gvContacts.DataSource = null;
                gvContacts.DataBind();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.TaskID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.Mode = 1;
                objBL_Customer.AddTask(objProp_Customer);
                strMsg = "Updated";
                if (ddlStatus.SelectedValue == "1")
                {
                    ddlStatus.Enabled = false;
                }
                FillTasks(hdnId.Value);
                BindEmails(GetMailsfromdb(-1, string.Empty));
            }

            string strScript = string.Empty;
            if (chkFollowUp.Checked == true)
            {
                strScript += "CheckFollowup(" + Request.QueryString["uid"].ToString() + "," + chkFollowUp.ClientID + ");";
            }
            strScript += "noty({text: 'Task " + strMsg + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);

            FillRecentProspect();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("tasks.aspx");
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue == "1")
        {
            txtResol.Enabled = true;
        }
        else
        {
            txtResol.Enabled = false;
        }
    }
    private void FillUsers()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.getTaskUsers(objPropUser);

        ddlAssigned.DataSource = ds.Tables[0];
        ddlAssigned.DataTextField = "fuser";
        ddlAssigned.DataValueField = "fuser";
        ddlAssigned.DataBind();

        ddlAssigned.Items.Insert(0, new ListItem("--Select--", ""));
    }
    private void FillRecentProspect()
    {
        NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        masterSalesMaster.FillRecentProspect();
    }
    protected void btnFillTasks_Click(object sender, EventArgs e)
    {
        FillTasks(hdnId.Value);
        FillContact(Convert.ToInt32(hdnId.Value));
        BindEmails(GetMailsfromdb(-1, string.Empty));
        HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;
    }

    private DataSet GetMailsfromdb(int type, string OrderBy)
    {
        if (OrderBy == string.Empty)
            ViewState["sortexp"] = null;

        DataSet ds = null;
        if (hdnId.Value.Trim() != string.Empty)
        {
            objGeneral.OrderBy = OrderBy;
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.type = type;
            objGeneral.rol = Convert.ToInt32(hdnId.Value);
            objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
            ds = objBL_General.GetMails(objGeneral);
        }
        return ds;
    }

    private void BindEmails(DataSet ds)
    {
        if (ds != null)
        {
            gvmail.DataSource = ds.Tables[0];
            gvmail.DataBind();
            menuLeads.Items[3].Text = "Emails(" + ds.Tables[0].Rows.Count + ")";
            ViewState["newmail"] = ds.Tables[0].Rows.Count;
            ////lblNewEmail.Text = string.Empty;
            //lblEmailCount.Text = string.Empty;
            //panel9.Visible = false;
            hdnMailct.Value = ds.Tables[0].Rows.Count.ToString();
        }
        else
        {
            gvmail.DataBind();
            menuLeads.Items[2].Text = "Emails(0)";
        }
    }

    protected void lnkRefreshMails_Click(object sender, EventArgs e)
    {
        //if (Application["pop3"] == null)
        //{
        //    Application["pop3"] = 0;
        //}

        //if ((int)Application["pop3"] == 0)
        //{
        //    Application["pop3"] = 1;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        objGeneral.ConnConfig = Session["config"].ToString();
        //        ds = objBL_General.GetEmailAccounts(objGeneral);
        //        DataSet dsEmail = objBL_General.getCRMEmails(objGeneral);
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            ////Thread email = new Thread(delegate()
        //            ////   {
        //            ////try
        //            ////{

        //            string host = dr["inserver"].ToString();
        //            string user = dr["inusername"].ToString();
        //            string pass = dr["inpassword"].ToString();
        //            string port = dr["inport"].ToString();
        //            int Userid = Convert.ToInt32(dr["Userid"]);
        //            string LastFetch = dr["lastfetch"].ToString();
        //            //objGeneralFunctions.DownloadMailsIMAP(host, user, pass, port, Userid, Session["config"].ToString(), LastFetch, dsEmail);
                    
        //            //objGeneralFunctions.DownloadMails(host, user, pass, port, Userid, Session["config"].ToString());
        //            ////}
        //            ////catch(Exception ex)
        //            ////{
        //            ////    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
        //            ////}
        //            ////  });
        //            ////email.IsBackground = true;
        //            ////email.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        //    }
        //    finally
        //    {
        //        Application["pop3"] = 0;
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: 'Mail download in progress by another user. Please refresh to get downloaded mails.',  type : 'information', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout: 5000, theme : 'noty_theme_default',  closable : true});", true);
        //}

        BindEmails(GetMailsfromdb(-1, string.Empty));
    }
    #region Paging

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvmail.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvmail.PageIndex = ddlPages.SelectedIndex;

        FillGridPaged();
    }
    protected void gvmail_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvmail.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvmail.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvmail.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvmail.PageCount.ToString();
    }
    protected void Paginate(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvmail.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvmail.PageIndex = 0;
                break;
            case "prev":
                gvmail.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvmail.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvmail.PageIndex = gvmail.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridPaged();
    }
    protected void gvmail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Paginate(sender, e);
    }

    #endregion

    #region sorting
    private void FillGridPaged()
    {
        DataSet dt = new DataSet();
        string sort = string.Empty;
        if (ViewState["sortexp"] != null)
            sort = ViewState["sortexp"].ToString();

        dt = PageSortData(sort);
        BindEmails(dt);
    }
    protected void gvmail_Sorting(object sender, GridViewSortEventArgs e)
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
    private DataSet PageSortData(string sortExpression)
    {
        DataSet dt = new DataSet();
        dt = GetMailsfromdb(-1, sortExpression);
        //dt = (DataTable)Session["dtTicketList"];
        return dt;
    }
    private void SortGridView(string sortExpression, string direction)
    {
        ViewState["sortexp"] = sortExpression + direction;
        DataSet dt = PageSortData(sortExpression + direction);
        BindEmails(dt);
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
    #endregion

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //int newmail = 0;
        //if (ViewState["newmail"] != null)
        //{
        //    newmail = Convert.ToInt32(ViewState["newmail"].ToString());
        //}
        //DataSet ds = GetMailsfromdb(-1, string.Empty);

        //if (newmail != ds.Tables[0].Rows.Count)
        //{
        //    if (ViewState["newmail"] != null)
        //    {
        //        //lblNewEmail.Text = Convert.ToString(ds.Tables[0].Rows.Count - newmail) + " New Email(s)";
        //        lblEmailCount.Text = Convert.ToString(ds.Tables[0].Rows.Count - newmail) + " New Email(s)";
        //        panel9.Visible = true;
        //    }
        //}
        ////lblEmailCount.Text = Convert.ToString(newmail) + " New Email(s)";
        ////panel9.Visible = true;
    }

    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    UpdateProgress up = (UpdateProgress)Page.Master.Master.FindControl("UpdateProgress1");
    //    up.Visible = false;
    //}

    private void FillContact(int rol)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = rol;
        DataSet ds = new DataSet();
        ds = objBL_Customer.getContactByRolID(objProp_Customer);
        gvContacts.DataSource = ds.Tables[0];
        gvContacts.DataBind();
        menuLeads.Items[2].Text = "Contacts(" + ds.Tables[0].Rows.Count + ")";
    }
}
