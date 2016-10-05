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
using ZXing;

public partial class AddEquipment : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objPropCustomer = new Customer();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        //hdnCon.Value = Session["config"].ToString();

        if (!IsPostBack)
        {
            userpermissions();
            FillEquipCategory();
            FillEquiptype();
            FillServiceType();
            FillRepTemplate();
            getCustomTemplate();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;

            if (Request.QueryString["lid"] != null && Request.QueryString["locname"] != null)
            {
                hdnLocId.Value = Request.QueryString["lid"].ToString();
                txtLocation.Text = Request.QueryString["locname"].ToString();
            }

            if (Request.QueryString["cuid"] != null)
            {
                hdnPatientId.Value = Request.QueryString["cuid"].ToString();
            }

            if (Request.QueryString["uid"] != null)
            {
                
                pnlNext.Visible = true;

                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                    lblHeader.Text = "";
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Equipment";
                }
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
                DataSet ds = new DataSet();
                ds = objBL_User.getequipByID(objPropUser);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
                    DateTime lastDay = firstDay.AddDays(DaysinMonth);
                    txtfromDate.Text = firstDay.ToShortDateString();
                    txtToDate.Text = lastDay.ToShortDateString();

                    tpnlREPH.Visible = true;
                    lblEquipName.Text = ds.Tables[0].Rows[0]["Unit"].ToString();
                    txtLocation.Text = ds.Tables[0].Rows[0]["location"].ToString();
                    hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                    txtEquipID.Text = ds.Tables[0].Rows[0]["unit"].ToString();
                    txtDesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    ddlServiceType.SelectedValue = ds.Tables[0].Rows[0]["cat"].ToString();
                    txtManuf.Text = ds.Tables[0].Rows[0]["manuf"].ToString();
                    txtSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                    txtUnique.Text = ds.Tables[0].Rows[0]["state"].ToString();
                    txtPrice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                    ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["category"].ToString();
                    ddlCustTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();
                    hdnSelectedVal.Value = ds.Tables[0].Rows[0]["template"].ToString();
                    if (ds.Tables[0].Rows[0]["install"] != DBNull.Value)
                    {
                        txtInstalled.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["install"]).ToShortDateString();
                    }
                    if (ds.Tables[0].Rows[0]["last"] != DBNull.Value)
                    {
                        txtLast.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["last"]).ToShortDateString();
                    }
                    if (ds.Tables[0].Rows[0]["since"] != DBNull.Value)
                    {
                        txtSince.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["since"]).ToShortDateString();
                    }
                    rbStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    pnlQR.Visible = true;
                    //imgQR.ImageUrl = "QRhandler.ashx?id=" + HttpUtility.UrlEncode(ds.Tables[0].Rows[0]["locationID"].ToString()) + ":" + HttpUtility.UrlEncode(ds.Tables[0].Rows[0]["unit"].ToString()) + ":" + ds.Tables[0].Rows[0]["loc"].ToString() + ":" + ds.Tables[0].Rows[0]["unitid"].ToString();
                    string strQRString = ds.Tables[0].Rows[0]["locationID"].ToString() + ":" + ds.Tables[0].Rows[0]["unit"].ToString() + ":" + ds.Tables[0].Rows[0]["loc"].ToString() + ":" + ds.Tables[0].Rows[0]["unitid"].ToString();
                    imgQR.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(GenerateQR(strQRString));

                    gvTemplateItems.DataSource = ds.Tables[1];
                    gvTemplateItems.DataBind();

                    gvCtemplItems.DataSource = ds.Tables[2];
                    gvCtemplItems.DataBind();

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        //ViewState["customvalues"] = ds.Tables[3];
                        binditemgrid(ds.Tables[3]);
                    }

                    fillREPHistory();
                    HyperLink2.NavigateUrl = "AddTicket.aspx?locid=" + hdnLocId.Value + "&unitid=" + Request.QueryString["uid"].ToString() + "&unit=" + lblEquipName.Text;
                }
                //gvSelectTemplate.DataSource = ds.Tables[1];
                //gvSelectTemplate.DataBind();
            }
        }
        Permission();
    }
    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    foreach (GridViewRow gr in gvRepDetails.Rows)
    //    {
    //        if (Session["type"].ToString() == "c")
    //        {
    //            gr.Attributes["ondblclick"] = "window.open('Printticket.aspx?id=" + lblTicketId.Text + "&c=" + lblComp.Text + "&pop=1','_blank');";
    //        }
    //        else
    //        {
    //            gr.Attributes["ondblclick"] = "window.open('addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1','_blank');";
    //        }
    //    }
    //}
    private byte[] GenerateQR(string QR)
    {
        byte[] QRbytes = null;
        var qrValue = QR;
        if (qrValue.ToString().Trim() != string.Empty)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 200,
                    Width = 200,
                    Margin = 1
                }
            };

            using (var bitmap = barcodeWriter.Write(qrValue))
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                QRbytes = stream.GetBuffer();
            }
        }
        return QRbytes;
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cstmMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEquipmentsSmenu");
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
            //Response.Redirect("home.aspx");
            //pnlSave.Visible = false;
            lblHeader.Text = "Equipment";
            btnSubmit.Visible = false;
            pnlEquipments.Enabled = false;
            tpCustom.Visible = false;
            tpREP.Visible = false;
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //Response.Redirect("home.aspx");
            //btnSubmit.Visible = false;
            pnlEquipments.Enabled = false;
        }

        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            //Response.Redirect("home.aspx");
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
                objPropUser.PageName = "addequipment.aspx";
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

    private void fillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
        objPropUser.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "template")
        {
            objPropUser.SearchValue = ddlTemplate.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "rd.Code")
        {
            objPropUser.SearchValue = txtCodeSearch.Text.Trim();
        }
        else if (ddlSearch.SelectedValue == "eti.frequency")
        {
            objPropUser.SearchValue = ddlFreq.SelectedValue;
        }
        else
        {
            objPropUser.SearchValue = txtSearch.Text.Trim();
        }
        objPropUser.Status = Convert.ToInt16(ddlDates.SelectedValue);
        objPropUser.StartDate = txtfromDate.Text.Trim();
        objPropUser.EndDate = txtToDate.Text.Trim();
        if (Session["type"].ToString() == "c")
        {
            objPropUser.Cust = 1;
        }
        else
        {
            objPropUser.Cust = 0;
        }
        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        FillREPDetails(ds.Tables[0]);
    }

    protected void getCustomTemplate()
    {
        objPropCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getCustomTemplate(objPropCustomer);
        ddlCustTemplate.DataSource = ds.Tables[0];
        ddlCustTemplate.DataTextField = "Fdesc";
        ddlCustTemplate.DataValueField = "ID";
        ddlCustTemplate.DataBind();
        ddlCustTemplate.Items.Insert(0, new ListItem("--Select--", "0"));
    }
    private void FillEquipCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquipmentCategory(objPropUser);
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "edesc";
        ddlCategory.DataValueField = "edesc";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("None", "None"));
        ddlCategory.Items.Add(new ListItem("New", "New"));
        ddlCategory.Items.Add(new ListItem("Refurbished", "Refurbished"));
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearch.SelectedValue == "template")
        {
            txtSearch.Visible = false;
            ddlTemplate.Visible = true;
            txtCodeSearch.Visible = false;
            ddlFreq.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "rd.Code")
        {
            txtSearch.Visible = false;
            ddlTemplate.Visible = false;
            txtCodeSearch.Visible = true;
            ddlFreq.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "eti.frequency")
        {
            txtSearch.Visible = false;
            ddlTemplate.Visible = false;
            txtCodeSearch.Visible = false;
            ddlFreq.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            ddlTemplate.Visible = false;
            txtCodeSearch.Visible = false;
            ddlFreq.Visible = false;
        }
    }

    private void FillREPDetails(DataTable dt)
    {
        Session["dtREPDetail"] = dt;
        gvRepDetails.DataSource = dt;
        gvRepDetails.DataBind();
        lblRecordCountHist.Text = dt.Rows.Count + " record(s) found.";
    }

    private DataTable GetREPDatafromSession()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtREPDetail"];
        return dt;
    }

    private void FillGridfromSession()
    {
        DataTable dt = new DataTable();
        dt = GetREPDatafromSession();
        FillREPDetails(dt);
    }

    #region Sorting REP Details

    protected void gvRepDetails_Sorting(object sender, GridViewSortEventArgs e)
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
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = GetREPDatafromSession();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        FillREPDetails(dv.ToTable());
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

    #region Paging REP Details

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvRepDetails.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvRepDetails.PageIndex = ddlPages.SelectedIndex;

        FillGridfromSession();
    }
    protected void gvRepDetails_DataBound(object sender, EventArgs e)
    {
        #region add pages to dropdown
        GridViewRow gvrPager = gvRepDetails.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvRepDetails.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvRepDetails.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvRepDetails.PageCount.ToString();
        #endregion
    }
    protected void gvRepDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvRepDetails.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvRepDetails.PageIndex = 0;
                break;
            case "prev":
                gvRepDetails.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvRepDetails.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvRepDetails.PageIndex = gvRepDetails.PageCount;
                break;
        }

        // popultate the gridview control
        FillGridfromSession();
    }

    #endregion

    private void FillEquiptype()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getEquiptype(objPropUser);

        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "edesc";
        ddlType.DataValueField = "edesc";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem("None", "None"));
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
        ddlServiceType.Items.Insert(0, new ListItem("None", "None"));
    }

    private void FillRepTemplate()
    {
        DataSet ds = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getRepTemplateName(objPropCustomer);
        gvSelectTemplate.DataSource = ds;
        gvSelectTemplate.DataBind();

        ddlRepTemp.DataSource = ds;
        ddlRepTemp.DataTextField = "fdesc";
        ddlRepTemp.DataValueField = "id";
        ddlRepTemp.DataBind();

        ddlRepTemp.Items.Insert(0, new ListItem("--Select--", ""));

        ddlTemplate.DataSource = ds;
        ddlTemplate.DataTextField = "fdesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();

        ddlTemplate.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        string url = "addequipment.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"];
        string screenid = string.Empty;
        if (Request.QueryString["cuid"] != null)
        {
            screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
        }
        else if (Request.QueryString["lid"] != null)
        {
            screenid = "&lid=" + Request.QueryString["lid"].ToString();
        }
        if (Request.QueryString["page"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + screenid;
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
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        string url = "addequipment.aspx?uid=" + dt.Rows[0]["id"];
        string screenid = string.Empty;
        if (Request.QueryString["cuid"] != null)
        {
            screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
        }
        else if (Request.QueryString["lid"] != null)
        {
            screenid = "&lid=" + Request.QueryString["lid"].ToString();
        }
        if (Request.QueryString["page"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + screenid;
        }
        Response.Redirect(url);

    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            string url = "addequipment.aspx?uid=" + dt.Rows[index - 1]["id"];
            string screenid = string.Empty;
            if (Request.QueryString["cuid"] != null)
            {
                screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
            }
            else if (Request.QueryString["lid"] != null)
            {
                screenid = "&lid=" + Request.QueryString["lid"].ToString();
            }
            if (Request.QueryString["page"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + screenid;
            }
            Response.Redirect(url);
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            string url = "addequipment.aspx?uid=" + dt.Rows[index + 1]["id"];
            string screenid = string.Empty;
            if (Request.QueryString["cuid"] != null)
            {
                screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
            }
            else if (Request.QueryString["lid"] != null)
            {
                screenid = "&lid=" + Request.QueryString["lid"].ToString();
            }
            if (Request.QueryString["page"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + screenid;
            }
            Response.Redirect(url);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            string uid = string.Empty;
            if (Request.QueryString["cuid"] != null)
            {
                uid = Request.QueryString["cuid"].ToString();
            }
            else
            {
                uid = Request.QueryString["lid"].ToString();
            }
            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + uid + "&tab=equip");
        }
        else
        {
            Response.Redirect("equipments.aspx");
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
            objPropUser.Unit = txtEquipID.Text;
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.Type = ddlType.SelectedValue;
            objPropUser.Cat = ddlServiceType.SelectedValue;
            objPropUser.Manufacturer = txtManuf.Text;
            objPropUser.Serial = txtSerial.Text;
            objPropUser.UniqueID = txtUnique.Text;
            objPropUser.Category = ddlCategory.SelectedValue;
            objPropUser.Description = txtDesc.Text;

            if (txtSince.Text.Trim() == string.Empty)
            {
                objPropUser.InstallDateTime = System.DateTime.MinValue;
            }
            else
            {
                objPropUser.InstallDateTime = Convert.ToDateTime(txtSince.Text);
            }

            if (txtLast.Text.Trim() == string.Empty)
            {
                objPropUser.LastServiceDate = System.DateTime.MinValue;
            }
            else
            {
                objPropUser.LastServiceDate = Convert.ToDateTime(txtLast.Text);
            }

            if (txtInstalled.Text.Trim() == string.Empty)
            {
                objPropUser.InstallDateimport = System.DateTime.MinValue;
            }
            else
            {
                objPropUser.InstallDateimport = Convert.ToDateTime(txtInstalled.Text);
            }

            if (txtPrice.Text.Trim() == string.Empty)
            {
                objPropUser.EquipPrice = Convert.ToDouble("0.00");
            }
            else
            {
                objPropUser.EquipPrice = Convert.ToDouble(txtPrice.Text);
            }
            objPropUser.Status = Convert.ToInt32(rbStatus.SelectedValue);
            objPropUser.Remarks = txtRemarks.Text;

            //CreateTable();
            //DataTable dt = (DataTable)Session["templtableEquipment"];
            DataTable dt = CreateTableFromGrid();
            dt.Columns.Remove("Name");
            objPropUser.DtItems = dt;

            DataTable dtCustom = CreateCustomTemplate();
            objPropUser.dtcustom = dtCustom;
            objPropUser.CustomTemplateID = Convert.ToInt32(ddlCustTemplate.SelectedValue);

            objPropUser.ConnConfig = Session["config"].ToString();

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                if (Session["MSM"].ToString() == "TS")
                {
                    objPropUser.ItemsOnly = 1;
                }
                objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                objBL_User.UpdateEquipment(objPropUser);
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Equipment updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                objBL_User.AddEquipment(objPropUser);

                if (Request.QueryString["page"] != null)
                {
                    string uid = string.Empty;
                    if (Request.QueryString["cuid"] != null)
                    {
                        uid = Request.QueryString["cuid"].ToString();
                    }
                    else
                    {
                        uid = Request.QueryString["lid"].ToString();
                    }
                    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + uid + "&tab=equip");
                }

                ResetFormControlValues(this);

                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Equipment added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            //hdnSaved.Value = "1";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void ddlFreq_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFrequency = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddlFrequency.Parent.Parent;

        TextBox txtLastDate = (TextBox)gvRow.FindControl("txtLdate");
        TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");


        if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate.Text != "")
        {
            string[] arr = txtLastDate.Text.Split('/');
            DateTime dt = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

            txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        }
        else
        {
            txtDueDate.Text = "";
        }

    }

    protected void txtLdate_TextChanged(object sender, EventArgs e)
    {

        TextBox txtLastDate = (TextBox)sender;

        GridViewRow gvRow = (GridViewRow)txtLastDate.Parent.Parent;
        DropDownList ddlFrequency = (DropDownList)gvRow.FindControl("ddlFreq");
        TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");

        if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate.Text != "")
        {
            string[] arr = txtLastDate.Text.Split('/');
            DateTime dt = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

            txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        }
        else
        {
            txtDueDate.Text = "";
        }
    }

    protected DateTime calculateNextDueDate(DateTime dt, int frequencyIndex)
    {
        switch (frequencyIndex)
        {
            case 0: dt = dt.AddDays(1); break;
            case 1: dt = dt.AddDays(7); break;
            case 2: dt = dt.AddDays(14); break;
            case 3: dt = dt.AddMonths(1); break;
            case 4: dt = dt.AddMonths(2); break;
            case 5: dt = dt.AddMonths(3); break;
            case 6: dt = dt.AddMonths(6); break;
            case 7: dt = dt.AddYears(1); break;
            case 8: dt = dt; break;
            case 9: dt = dt.AddMonths(4); break;
            case 10: dt = dt.AddYears(2); break;
            case 11: dt = dt.AddYears(3); break;
            case 12: dt = dt.AddYears(5); break;
            case 13: dt = dt.AddYears(7); break;
            default:
                //default stuff
                break;
        }
        return dt;
    }

    protected void btnAddNewItem_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
        pnlREPT.Visible = true;
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
    }

    private DataTable CreateTableFromGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("EquipT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("Lastdate", typeof(DateTime));
        dt.Columns.Add("NextDateDue", typeof(DateTime));
        dt.Columns.Add("Frequency", typeof(int));
        dt.Columns.Add("Section", typeof(string));

        foreach (GridViewRow gr in gvTemplateItems.Rows)
        {
            //if (((TextBox)gr.FindControl("lblDesc")).Text.Trim() != string.Empty)
            //{
            DataRow dr = dt.NewRow();
            dr["Code"] = ((Label)gr.FindControl("lblCode")).Text.Trim();
            dr["Name"] = ((Label)gr.FindControl("lblName")).Text.Trim();
            dr["EquipT"] = ((Label)gr.FindControl("lblEquipT")).Text.Trim();
            dr["Elev"] = 0;
            dr["fDesc"] = ((TextBox)gr.FindControl("lblDesc")).Text.Trim();
            dr["Line"] = dt.Rows.Count;
            if (((TextBox)gr.FindControl("txtLdate")).Text.Trim() == string.Empty)
            {
                dr["Lastdate"] = DBNull.Value;
            }
            else
            {
                dr["Lastdate"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtLdate")).Text.Trim()).ToShortDateString();
            }

            if (((TextBox)gr.FindControl("txtDuedate")).Text.Trim() == string.Empty)
            {
                dr["NextDateDue"] = DBNull.Value;
            }
            else
            {
                dr["NextDateDue"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtDuedate")).Text.Trim()).ToShortDateString();
            }

            dr["Frequency"] = ((DropDownList)gr.FindControl("ddlFreq")).SelectedItem.Value;
            dr["Section"] = ((TextBox)gr.FindControl("txtSection")).Text.Trim();
            dt.Rows.Add(dr);
            //}
        }

        //Session["templtableEquipment"] = dt;
        return dt;
    }

    private DataTable CreateCustomTemplate()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ElevT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("value", typeof(string));
        dt.Columns.Add("Format", typeof(string));

        foreach (GridViewRow gr in gvCtemplItems.Rows)
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = Convert.ToInt32(((Label)gr.FindControl("lblID")).Text);
            dr["ElevT"] = 0;
            dr["Elev"] = 0;
            dr["fDesc"] = ((Label)gr.FindControl("lblDesc")).Text;
            dr["Line"] = dt.Rows.Count + 1;
            if (((Label)gr.FindControl("lblFormat")).Text == "Dropdown")
                dr["value"] = ((DropDownList)gr.FindControl("ddlFormat")).Text.Trim();
            else
                dr["value"] = ((TextBox)gr.FindControl("lblValue")).Text.Trim();
            dr["Format"] = ((Label)gr.FindControl("lblFormat")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private void AppendTemplateItemstoGrid(int TemplateID, string Startdate, bool Unique)
    {
        DataTable dtItems = CreateTableFromGrid();
        //DataTable dt = (DataTable)Session["templtableEquipment"];
        //Session["templtableEquipment"] = null;

        DataSet dsNewItems = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        int Elev = 0;
        objPropCustomer.TemplateID = TemplateID;
        dsNewItems = objBL_Customer.getTemplateItemByID(objPropCustomer);

        foreach (DataRow dr in dsNewItems.Tables[0].Rows)
        {
            //DataRow[] drSelect = dtItems.Select("equipt=" + Convert.ToInt32(dr["EquipT"]) + " and fdesc='" + dr["fDesc"].ToString() + "'");
            int count = 0;

            //if (Unique)
            //{
            //    DataRow[] drSelect = dtItems.Select("Code='" + dr["Code"].ToString() + "'");
            //    count = drSelect.Count();
            //}

            if (count == 0)
            {
                DataRow drNew = dtItems.NewRow();
                drNew["Code"] = dr["Code"].ToString();
                drNew["Name"] = dr["Name"].ToString();
                drNew["EquipT"] = dr["EquipT"].ToString();
                drNew["Elev"] = Elev;
                drNew["fDesc"] = dr["fDesc"].ToString();
                drNew["Line"] = dtItems.Rows.Count;
                if (Startdate != string.Empty)
                {
                    DateTime dtst = new DateTime();
                    if (DateTime.TryParse(Startdate, out dtst))
                    {
                        drNew["Lastdate"] = dtst;
                        if (Convert.ToInt32(dr["Frequency"].ToString()) > -1)
                            drNew["NextDateDue"] = calculateNextDueDate(dtst, Convert.ToInt32(dr["Frequency"].ToString()));
                    }
                    else
                    {
                        drNew["Lastdate"] = DBNull.Value;
                        drNew["NextDateDue"] = DBNull.Value;
                    }
                }
                else
                {
                    drNew["Lastdate"] = DBNull.Value;
                    drNew["NextDateDue"] = DBNull.Value;
                }

                drNew["Frequency"] = dr["Frequency"].ToString();
                drNew["Section"] = dr["Section"].ToString();
                dtItems.Rows.InsertAt(drNew, 0);
            }
        }

        //Session["templtableEquipment"] = dt;
        gvTemplateItems.DataSource = dtItems;
        gvTemplateItems.DataBind();
    }

    protected void cbRepTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lnk.Parent.Parent;
        Label lblRepTempID = (Label)gvRow.FindControl("lblRepTempId");
        TextBox txtStartDate = (TextBox)gvRow.FindControl("txtStartDate");

        AppendTemplateItemstoGrid(Convert.ToInt32(lblRepTempID.Text), txtStartDate.Text.Trim(), true);
    }

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        ////CreateTable();
        ////DataTable dtSession = (DataTable)Session["templtableEquipment"];        


        //int exists = CheckDuplicateCode();

        //if (exists == 1)
        //{
        //    txtCode.Focus();
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAlertCode", "alert('Code already exists, please enter different code.')", true);
        //    return;
        //}   

        //DataTable dt = CreateTableFromGrid();

        //DataRow dr = dt.NewRow();
        //dr["Code"] = txtCode.Text.Trim();
        //dr["Name"] = ddlRepTemp.SelectedItem.Text;
        //dr["EquipT"] = ddlRepTemp.SelectedItem.Value;
        //dr["Elev"] = 0;
        //dr["fDesc"] = DBNull.Value;
        //dr["Line"] = dt.Rows.Count;
        //dr["Lastdate"] = DBNull.Value;
        //dr["NextDateDue"] = DBNull.Value;
        //dr["Frequency"] = -1;
        //dt.Rows.InsertAt(dr,0);
        //gvTemplateItems.DataSource = dt;
        //gvTemplateItems.DataBind();
        ////Session["templtableEquipment"] = dtSession;

        AppendTemplateItemstoGrid(Convert.ToInt32(ddlRepTemp.SelectedValue), txtLastDate.Text.Trim(), false);
        //this.programmaticModalPopup.Hide();
    }

    //private int CheckDuplicateCode()
    //{
    //      int exists = 0;
    //      //if (addnew == 1)
    //      //{
    //          foreach (GridViewRow gr in gvTemplateItems.Rows)
    //          {
    //              Label txtCodeField = (Label)gr.FindControl("lblCode");

    //              if (!txtCodeField.Text.Trim().Equals(string.Empty))
    //              {
    //                  if (txtCodeField.Text.Trim().Equals(txtCode.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
    //                  {
    //                      exists = 1;
    //                  }
    //              }
    //          }

    //          if (exists == 0)
    //          {
    //              DataSet ds = new DataSet();
    //              objPropCustomer.ConnConfig = Session["config"].ToString();
    //              objPropCustomer.TemplateID = 0;// Convert.ToInt32(ddlRepTemp.SelectedValue);
    //              objPropCustomer.SearchValue = string.Empty;
    //              ds = objBL_Customer.getTemplateItemCodes(objPropCustomer);

    //              foreach (DataRow dr in ds.Tables[0].Rows)
    //              {
    //                  if (dr["code"].ToString().Trim().Equals(txtCode.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
    //                  {
    //                      exists = 1;
    //                  }
    //              }
    //          }
    //      //}
    //      //else
    //      //{
    //      //    DataSet ds = new DataSet();
    //      //    objPropCustomer.ConnConfig = Session["config"].ToString();
    //      //    objPropCustomer.TemplateID = 0;
    //      //    objPropCustomer.EquipID=Convert.ToInt32()
    //      //    ds = objBL_Customer.getTemplateItemCodes(objPropCustomer);

    //      //    foreach (DataRow dr in ds.Tables[0].Rows)
    //      //    {
    //      //        foreach (GridViewRow gr in gvTemplateItems.Rows)
    //      //        {
    //      //            Label txtCodeField = (Label)gr.FindControl("lblCode");
    //      //            if (dr["code"].ToString().Trim().Equals(txtCodeField.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
    //      //            {
    //      //                exists = 1;
    //      //            }
    //      //        }
    //      //    }
    //      //}

    //    return exists;     
    //}

    protected void btnDeleteItem_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["templtableEquipment"];
        DataTable dt = CreateTableFromGrid();
        int index = 0;
        foreach (GridViewRow gr in gvTemplateItems.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                dt.Rows.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
        //Session["templtableEquipment"] = dt;
        gvTemplateItems.DataSource = dt;
        gvTemplateItems.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        fillREPHistory();
    }

    protected void lnkclear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = -1;
        txtSearch.Text = string.Empty;
        txtCodeSearch.Text = string.Empty;
        ddlTemplate.SelectedIndex = -1;
        ddlFreq.SelectedIndex = -1;
        ddlDates.SelectedIndex = -1;
        txtfromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtSearch.Visible = true;
        txtCodeSearch.Visible = false;
        ddlFreq.Visible = false;
        ddlTemplate.Visible = false;
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        fillREPHistory();
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

    protected void gvTemplateItems_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection(SortDireaction);
        DataTable dt = CreateTableFromGrid();
        if (dt != null)
        {
            dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            gvTemplateItems.DataSource = dt;
            gvTemplateItems.DataBind();
            SortDireaction = _sortDirection;
        }
    }

    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";
        }
        else
        {
            _sortDirection = "ASC";
        }
    }

    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }
    private string _sortDirection;

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCodes(string prefixText, int count, string contextKey)
    {
        Customer objPropCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        DataSet ds = new DataSet();
        objPropCustomer.SearchValue = prefixText;
        objPropCustomer.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropCustomer.TemplateID = 0;
        ds = objBL_Customer.getTemplateItemCodes(objPropCustomer);

        DataTable dt = ds.Tables[0];

        List<string> lstItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["code"].ToString(), row["code"].ToString());
            lstItems.Add(dbValues);
        }

        return lstItems.ToArray();
    }

    protected void ddlCustTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        objPropCustomer.TemplateID = Convert.ToInt32(ddlCustTemplate.Text);
        objPropCustomer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getCustTemplateItemByID(objPropCustomer);
        gvCtemplItems.DataSource = ds;
        gvCtemplItems.DataBind();
        hdnSelectedVal.Value = ddlCustTemplate.SelectedValue;
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                //gvCtemplItems.DataSource = ds;
                //gvCtemplItems.DataBind();
                ((Label)gvCtemplItems.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(ds.Tables[0].Rows.Count - 0);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    //ViewState["customvalues"] = ds.Tables[1];
                    binditemgrid(ds.Tables[1]);
                }
            }
        }
    }

    private void binditemgrid(DataTable dtValues)
    {
        foreach (GridViewRow gr in gvCtemplItems.Rows)
        {
            Label lblFormat = (Label)gr.FindControl("lblFormat");
            if (lblFormat.Text == "Dropdown")
            {

                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlFormat");
                ddlCustomValue.Visible = true;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblValueh = (Label)gr.FindControl("lblValueh");

                DataTable dt = dtValues.Clone();
                DataRow[] result = dtValues.Select("ItemID = " + Convert.ToInt32(lblID.Text) + " AND Line = " + Convert.ToInt32(lblIndex.Text) + "");
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
                ddlCustomValue.Items.Insert(0, (new ListItem("", "")));

                if (ddlCustomValue.Items.Contains(new ListItem(lblValueh.Text, lblValueh.Text)))
                {
                    ddlCustomValue.SelectedValue = lblValueh.Text;
                }
                else
                {
                    ddlCustomValue.Items.Add(new ListItem(lblValueh.Text, lblValueh.Text));
                    ddlCustomValue.SelectedValue = lblValueh.Text;
                }
            }
        }
    }

    protected void gvCtemplItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFormat = (Label)e.Row.FindControl("lblFormat");
            if (lblFormat.Text == "Dropdown")
            {
                DropDownList ddlCustomValue = (DropDownList)e.Row.FindControl("ddlFormat");
                ddlCustomValue.Visible = true;
                Label lblID = (Label)e.Row.FindControl("lblID");
                Label lblIndex = (Label)e.Row.FindControl("lblIndex");

                if (ViewState["customvalues"] != null)
                {
                    DataTable dtValues = (DataTable)ViewState["customvalues"];
                    DataTable dt = dtValues.Clone();
                    DataRow[] result = dtValues.Select("ItemID = " + Convert.ToInt32(lblID.Text) + " AND Line = " + Convert.ToInt32(lblIndex.Text) + "");
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
                    ddlCustomValue.Items.Insert(0, (new ListItem("", "")));
                }
            }
        }
    }

    public string setLinkStyle(string internet)
    {
        string str = "cursor:pointer";
        if (Session["type"].ToString() == "c")
        {
            if (internet == "0")
                str = "color:black";
        }
        return str;
    }
}
