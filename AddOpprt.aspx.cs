using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

public partial class AddOpprt : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
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
            lnkSpecific.Visible = false;
            lnkAllMail.Visible = false;
            //lnkNewEmail.Visible = false;

            ViewState["edit"] = 0;
            FillUsers();
            FillStages();
            ViewState["rolapp"] = 0;
            //SalesMaster masterSalesMaster = (SalesMaster)Page.Master;
            //masterSalesMaster.FillPendingRec();

            if (Request.QueryString["uid"] != null)
            {
                hdnUID.Value = Request.QueryString["uid"].ToString();
                lnkSpecific.Visible = true;
                lnkAllMail.Visible = true;
                lnkNewEmail.Visible = true;


                ViewState["edit"] = 1;
                lblHeader.Text = "Edit Opportunity";
                GetOppertunity();
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                lnkNewEmail.NavigateUrl = "email.aspx?op=" + Request.QueryString["uid"].ToString() + "&rol=" + hdnId.Value;
            }
            if (ViewState["edit"].ToString() == "0")
            {
                HideControls();
            }
            if (Request.QueryString["rol"] != null)
            {
                ViewState["rolapp"] = Request.QueryString["rol"];
                hdnId.Value = Request.QueryString["rol"].ToString();
                txtName.Text = Request.QueryString["name"].ToString();
                FillTasks(hdnId.Value);
                FillContact(Convert.ToInt32(hdnId.Value));
                GetDocuments(Convert.ToInt32(Request.QueryString["owner"].ToString()));
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;
            }

            NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
            masterSalesMaster.FillPendingRec(Convert.ToInt32(ViewState["rolapp"]));
        }
        Permission();
    }
    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*
        if (ddlStage.SelectedValue == "r.name" || ddlStage.SelectedValue == "l.fdesc")
        {
            txtSearch.Visible = true;
            ddlProbab.Visible = false;
            ddlStatus.Visible = false;
            ddlAssigned.Visible = false;
        }
         */
    }

    private void HideControls()
    {
        //pnlNotes.Visible = false;
        pnlSysInfo.Visible = false;
        menuLeads.Visible = false;
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkOpportunities");
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
    private void FillStages()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getStages(objProp_Customer);

        ddlStage.DataSource = ds.Tables[0];
        ddlStage.DataTextField = "Description";
        ddlStage.DataValueField = "ID";
        ddlStage.DataBind();
        ddlStage.Items.Insert(0, new ListItem("--Select--", ""));
    }
    private void GetOppertunity()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.OpportunityID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        ds = objBL_Customer.getOpportunityByID(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["rolapp"] = ds.Tables[0].Rows[0]["rol"].ToString();
            hdnId.Value = ds.Tables[0].Rows[0]["rol"].ToString();
            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtOppName.Text = ds.Tables[0].Rows[0]["fDesc"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            if (ds.Tables[0].Rows[0]["closedate"] != DBNull.Value)
                txtCloseDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["closedate"]).ToShortDateString();
            ddlProbab.SelectedValue = ds.Tables[0].Rows[0]["Probability"].ToString();
            ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
            lblType.Text = ds.Tables[0].Rows[0]["contacttype"].ToString();
            hdnOwnerID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
            txtNextStep.Text = ds.Tables[0].Rows[0]["nextstep"].ToString();
            txtDesc.Text = ds.Tables[0].Rows[0]["desc"].ToString();
            ddlAssigned.SelectedValue = ds.Tables[0].Rows[0]["fuser"].ToString().ToUpper();
            chkClosed.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["closed"]);

            if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

            if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();

            if (ds.Tables[0].Rows[0]["revenue"] != DBNull.Value)
            {
                txtAmount.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["revenue"]));
            }
            ddlSource.SelectedValue = ds.Tables[0].Rows[0]["source"].ToString();
            FillTasks(ds.Tables[0].Rows[0]["rol"].ToString());
            FillContact(Convert.ToInt32(ds.Tables[0].Rows[0]["rol"].ToString()));
            GetDocuments(Convert.ToInt32(ds.Tables[0].Rows[0]["ownerid"].ToString()));
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (lblType.Text.Trim() == "Lead")
        {
            if (chkClosed.Checked == true)
            {
                if (pnlCustomer.Visible == false)
                {
                    pnlCustomer.Visible = true;
                    uc_CustomerSearch1._txtCustomer.Focus();
                    return;
                }
            }
            else
            {
                pnlCustomer.Visible = false;
                uc_CustomerSearch1._hdnCustID.Value = string.Empty;
            }
        }
        else
        {
            pnlCustomer.Visible = false;
            uc_CustomerSearch1._hdnCustID.Value = string.Empty;
        }

        objProp_Customer.Name = txtOppName.Text.Trim();
        objProp_Customer.ROL = Convert.ToInt32(hdnId.Value);
        objProp_Customer.Probability = Convert.ToInt32(ddlProbab.SelectedValue);
        objProp_Customer.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Customer.Remarks = txtRemarks.Text.Trim();
        objProp_Customer.EndDate = txtCloseDate.Text.Trim();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        if (uc_CustomerSearch1._hdnCustID.Value.Trim() != string.Empty)
            objProp_Customer.ProspectID = Convert.ToInt32(uc_CustomerSearch1._hdnCustID.Value);//0;//Convert.ToInt32(hdnOwnerID.Value);
        else
            objProp_Customer.ProspectID = 0;
        objProp_Customer.NextStep = string.Empty;
        objProp_Customer.Fuser = ddlAssigned.SelectedValue;
        objProp_Customer.LastUpdateUser = Session["username"].ToString();
        objProp_Customer.Close = Convert.ToInt16(chkClosed.Checked);
        objProp_Customer.ticketID = 0;

        string strDesc = string.Empty;
        if (txtNextStep.Text.Trim() != string.Empty)
        {
            strDesc = System.DateTime.Now.ToString() + "      " + Session["username"].ToString() + Environment.NewLine + "Next Step : " + txtNextStep.Text.Trim() + Environment.NewLine + txtDesc.Text.Trim();
        }
        else
        {
            strDesc = txtDesc.Text.Trim();
        }
        objProp_Customer.Description = strDesc;
        objProp_Customer.Source = ddlSource.SelectedValue;
        if (txtAmount.Text.Trim() != string.Empty)
            objProp_Customer.Amount = double.Parse(txtAmount.Text.Trim(), NumberStyles.Currency);

        try
        {
            int OpprtID = 0;
            string strMsg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objProp_Customer.OpportunityID = 0;
                objProp_Customer.Mode = 0;
                OpprtID = objBL_Customer.AddOpportunity(objProp_Customer);

                ConvertProspectWizard(OpprtID);

                objGeneralFunctions.ResetFormControlValues(this);

                gvContacts.DataSource = null;
                gvContacts.DataBind();

                gvDocuments.DataSource = null;
                gvDocuments.DataBind();

                gvTasks.DataSource = null;
                gvTasks.DataBind();

                gvTasksCompleted.DataSource = null;
                gvTasksCompleted.DataBind();

                pnlCustomer.Visible = false;
                uc_CustomerSearch1._hdnCustID.Value = string.Empty;
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.OpportunityID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.Mode = 1;
                OpprtID = objBL_Customer.AddOpportunity(objProp_Customer);

                ConvertProspectWizard(OpprtID);

                strMsg = "Updated";
                GetOppertunity();
                pnlCustomer.Visible = false;
            }

            string strScript = string.Empty;
            strScript += "noty({text: 'Opportunity " + strMsg + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);

            FillRecentProspect();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void ConvertProspectWizard(int OpprtID)
    {
        string ProspectID = hdnOwnerID.Value.Trim();
        if (lblType.Text.Trim() == "Lead" && chkClosed.Checked == true && uc_CustomerSearch1._hdnCustID.Value == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Opportunity Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "&opid=" + OpprtID + "';", true);
        }
        else if (lblType.Text.Trim() == "Lead" && chkClosed.Checked == true && uc_CustomerSearch1._hdnCustID.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Opportunity Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "&opid=" + OpprtID + "';", true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("opportunity.aspx");
    }
    protected void btnFillTasks_Click(object sender, EventArgs e)
    {
        FillTasks(hdnId.Value);
        FillContact(Convert.ToInt32(hdnId.Value));
        GetDocuments(Convert.ToInt32(hdnOwnerID.Value));

        HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;

        ViewState["rolapp"] = hdnId.Value;
        NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        masterSalesMaster.FillPendingRec(Convert.ToInt32(ViewState["rolapp"]));
    }
    private void FillRecentProspect()
    {
        NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        masterSalesMaster.FillRecentProspect();
        masterSalesMaster.FillPendingRec(Convert.ToInt32(ViewState["rolapp"]));
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
        gvTasks.DataSource = ds.Tables[0];
        gvTasks.DataBind();
        menuLeads.Items[1].Text = "Open Tasks(" + ds.Tables[0].Rows.Count + ")";

        objProp_Customer.Mode = 0;
        ds = objBL_Customer.getTasks(objProp_Customer);
        gvTasksCompleted.DataSource = ds.Tables[0];
        gvTasksCompleted.DataBind();
        menuLeads.Items[2].Text = "Task History(" + ds.Tables[0].Rows.Count + ")";

        if (ViewState["edit"].ToString() == "0")
        {
            BindEmails(GetMailsfromdb(-1, string.Empty));
            hdnMailType.Value = "-1";
        }
        else
        {
            BindEmails(GetMailsfromdb(-2, string.Empty));
            hdnMailType.Value = "-2";
        }
    }

    private void FillContact(int rol)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = rol;
        DataSet ds = new DataSet();
        ds = objBL_Customer.getContactByRolID(objProp_Customer);
        gvContacts.DataSource = ds.Tables[0];
        gvContacts.DataBind();
        menuLeads.Items[3].Text = "Contacts(" + ds.Tables[0].Rows.Count + ")";
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

        //ddlAssigned.Items.Insert(0, new ListItem("--Select--", ""));
        ddlAssigned.Items.Insert(0, new ListItem("None", ""));
    }
    private void GetDocuments(int Leadid)
    {
        if (lblType.Text == "Lead")
            objMapData.Screen = "SalesLead";
        else
            objMapData.Screen = "Location";

        objMapData.TempId = "0";
        objMapData.TicketID = Leadid;

        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetDocuments(objMapData);
        gvDocuments.DataSource = ds.Tables[0];
        gvDocuments.DataBind();

        menuLeads.Items[5].Text = "Notes and Attachments(" + ds.Tables[0].Rows.Count + ")";
    }
    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    private DataSet GetMailsfromdb(int type, string OrderBy)
    {
        if (OrderBy == string.Empty)
            ViewState["sortexp"] = null;

        DataSet ds = null;
        lnkAllMail.BackColor = System.Drawing.Color.DarkGray;
        lnkSpecific.BackColor = System.Drawing.Color.Transparent;
        if (hdnId.Value.Trim() != string.Empty)
        {
            objGeneral.OrderBy = OrderBy;
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.type = type;
            objGeneral.rol = Convert.ToInt32(hdnId.Value);
            objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
            if (type == -2)
            {
                objGeneral.RegID = "[OP-" + Request.QueryString["uid"].ToString() + "]";
                objGeneral.rol = 0;
                lnkAllMail.BackColor = System.Drawing.Color.Transparent;
                lnkSpecific.BackColor = System.Drawing.Color.DarkGray;
            }

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
            menuLeads.Items[4].Text = "Emails(" + ds.Tables[0].Rows.Count + ")";
            ////lblNewEmail.Text = string.Empty;
            //lblEmailCount.Text = string.Empty;
            //panel10.Visible = false;
            hdnMailct.Value = ds.Tables[0].Rows.Count.ToString();
        }
        else
        {
            gvmail.DataBind();
            menuLeads.Items[4].Text = "Emails(0)";
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

        if (hdnMailType.Value == "-1")
            BindEmails(GetMailsfromdb(-1, string.Empty));
        else if (hdnMailType.Value == "-2")
            BindEmails(GetMailsfromdb(-2, string.Empty));
    }
    protected void lnkAllMail_Click(object sender, EventArgs e)
    {
        BindEmails(GetMailsfromdb(-1, string.Empty));
        hdnMailType.Value = "-1";
    }
    protected void lnkSpecific_Click(object sender, EventArgs e)
    {
        BindEmails(GetMailsfromdb(-2, string.Empty));
        hdnMailType.Value = "-2";
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
        if (hdnMailType.Value == "-1")
            dt = GetMailsfromdb(-1, sortExpression);
        else if (hdnMailType.Value == "-2")
            dt = GetMailsfromdb(-2, sortExpression);

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
        //        panel10.Visible = true;
        //    }
        //}
        ////lblEmailCount.Text = Convert.ToString(newmail) + " New Email(s)";
        ////panel10.Visible = true;
    }
    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    UpdateProgress up = (UpdateProgress)Page.Master.Master.FindControl("UpdateProgress1");
    //    up.Visible = false;
    //}
    protected void chkClosed_CheckedChanged(object sender, EventArgs e)
    {
        pnlCustomer.Visible = chkClosed.Checked;
    }
}
