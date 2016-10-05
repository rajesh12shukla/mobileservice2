using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

public partial class BillingCodes : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

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
            FillBillCodes();
            //FillDepartmentDDL();
        }
        Permission();
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("AcctMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("billingLink");
        //a.Style.Add("color", "#2382b2");        

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkbillcodeSmenu");
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
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvBillCodes.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvBillCodes.ClientID + "',event);";
            gr.Attributes["ondblclick"] = "document.getElementById('" + LinkButton8.ClientID + "').click();";
        }
        ClientScript.RegisterStartupScript(Page.GetType(), "key8", "SelectedRowStyle('" + gvBillCodes.ClientID + "');", true);
    }

    private void FillBillCodes()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = "1";
        ds = objBL_User.getBillCodes(objProp_User);
        BindGridDatatable(ds.Tables[0]);
    }

    private void BindGridDatatable(DataTable dt)
    {
        Session["BillCodeSrch"] = dt;
        gvBillCodes.DataSource = dt;
        gvBillCodes.DataBind();
    }

    private void FillDepartmentDDL()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objProp_User);

        ddlStatus.DataSource = ds.Tables[0];
        ddlStatus.DataTextField = "type";
        ddlStatus.DataValueField = "id";
        ddlStatus.DataBind();
    }

    protected void lnkAddBillingCodes_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
        ViewState["edit"] = 0;
        ResetFormControlValues(this);
        //TogglePopUp();
    }

    private void TogglePopUp()
    {
        string strScript = "openModelPopup()";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "BillingCode", strScript, true);
    }
    
    protected void lnkDeleteBillingCodes_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow di in gvBillCodes.Rows)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblBillID = (Label)di.FindControl("lblBillID");

                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.BillCode = Convert.ToInt32(lblBillID.Text);

                    objBL_User.DeleteBillingCode(objProp_User);
                    FillBillCodes();
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrDelbillcodes", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkEditBillingCodes_Click(object sender, EventArgs e)
    {
        ViewState["edit"] = 1;
        ResetFormControlValues(this);
        pnlBillCode.Visible = true;

        foreach (GridViewRow di in gvBillCodes.Rows)
        {
            CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            Label lblName = (Label)di.FindControl("lblId");
            Label lbldesc = (Label)di.FindControl("lbldesc");
            //Label lblCat = (Label)di.FindControl("lblCatID");
            Label lblStatus = (Label)di.FindControl("lblStatus");
            Label lblBalance = (Label)di.FindControl("lblBalance");
            Label lblMeasure = (Label)di.FindControl("lblMeasure");
            Label lblrem = (Label)di.FindControl("lblrem");
            Label lblStatusID = (Label)di.FindControl("lblStatusID");
            Label lblbillID = (Label)di.FindControl("lblbillid");
            Label lblAcctID = (Label)di.FindControl("lblAcctID");
            Label lblaccount = (Label)di.FindControl("lblaccount");

            if (chkSelect.Checked == true)
            {
                txtBillCode.Text = lblName.Text;
                txtBillDesc.Text = lbldesc.Text;
                ddlStatus.SelectedValue = lblStatusID.Text;
                txtBillBal.Text = lblBalance.Text;
                txtBillMeasure.Text = lblMeasure.Text;
                txtBillRemarks.Text = lblrem.Text;
                hdnBillID.Value = lblbillID.Text;
                hdnPatientId.Value = lblAcctID.Text;
                txtAccount.Text = lblaccount.Text;
                this.programmaticModalPopup.Show();
            }
        }
    }

    protected void lnkBillingCodesSave_Click(object sender, EventArgs e)
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.ContactName = txtBillCode.Text;
        objProp_User.SalesDescription = txtBillDesc.Text;
        objProp_User.CatStatus = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_User.Balance = Convert.ToDouble(txtBillBal.Text);
        objProp_User.Measure = txtBillMeasure.Text;
        objProp_User.Remarks = txtBillRemarks.Text;
        objProp_User.Type = "1";
        if (hdnPatientId.Value != string.Empty)
            objProp_User.sAcct = Convert.ToInt32(hdnPatientId.Value);
        else
            objProp_User.sAcct = 0;
        if (ViewState["edit"].ToString() == "0")
        {
            objBL_User.AddBillCode(objProp_User);
        }
        else if (ViewState["edit"].ToString() == "1")
        {
            objProp_User.BillCode = Convert.ToInt32(hdnBillID.Value);
            objBL_User.UpdateBillCode(objProp_User);
        }
        objProp_User.Type = null;
        FillBillCodes();
    }

    protected void lnkBillingCodesClose_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
        FillBillCodes();
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
                    //case "System.Web.UI.WebControls.CheckBox":
                    //    ((CheckBox)c).Checked = false;
                    //    break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }
    
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
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
        dt = (DataTable)Session["BillCodeSrch"];
        return dt;
    }

    #region Paging

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvBillCodes.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvBillCodes.PageIndex = ddlPages.SelectedIndex;

        FillGridPaged();
    }
    protected void gvBillCodes_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvBillCodes.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvBillCodes.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvBillCodes.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvBillCodes.PageCount.ToString();
    }
    protected void Paginate(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvBillCodes.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvBillCodes.PageIndex = 0;
                break;
            case "prev":
                gvBillCodes.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvBillCodes.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvBillCodes.PageIndex = gvBillCodes.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridPaged();
    }
    protected void gvBillCodes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Paginate(sender, e);
    }

    #endregion

    #region Sorting

    protected void gvBillCodes_Sorting(object sender, GridViewSortEventArgs e)
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
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortData();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindGridDatatable(dv.ToTable());
    }

    #endregion


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetAccounts(string prefixText, int count, string contextKey)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getAccountAuto(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Account"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }
}
