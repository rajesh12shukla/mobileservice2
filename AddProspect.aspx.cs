using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
//using OpenPop.Mime;
//using OpenPop.Mime.Header;
//using OpenPop.Pop3;
//using OpenPop.Pop3.Exceptions;
//using OpenPop.Common.Logging;
//using Message = OpenPop.Mime.Message;

public partial class AddProspect : System.Web.UI.Page
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
            //Response.Redirect("timeout.htm");
            //return;
        }
        if (!IsPostBack)
        {
            ViewState["convert"] = "0";
            ViewState["edit"] = 0;
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            ViewState["notesmode"] = 0;
            Session["contacttablelead"] = null;
            CreateTable();
            GetProspectType();
            FillSalesPerson();
            NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
            masterSalesMaster.FillPendingLeads();

            if (Request.QueryString["uid"] != null)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    lnkConvert.Visible = true;
                }
                FillProspectScreen(Request.QueryString["uid"].ToString());
                lnkNewEmail.Visible = true;
                lnkNewEmail.NavigateUrl = "email.aspx?to=" + txtEmail.Text.Trim() + "&rol=" + hdnRol.Value;
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + ViewState["rol"].ToString() + "&name=" + txtProspectName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + ViewState["rol"].ToString() + "&name=" + txtProspectName.Text;
                lnkAddopp.NavigateUrl = "AddOpprt.aspx?rol=" + ViewState["rol"].ToString() + "&owner=" + Request.QueryString["uid"].ToString() + "&name=" + txtProspectName.Text;
            }
            if (ViewState["edit"].ToString() == "0")
            {
                HideControls();
            }
        }
        Permission();
    }

    protected void lnkConvert_Click(object sender, EventArgs e)
    {
        pnlCustomer.Visible = true;
        uc_CustomerSearch1._txtCustomer.Focus();
        lnkConvert.Visible = false;
        lnkSave.Text = "Next";
        ViewState["convert"] = "1";
    }

    private void ConvertProspectWizard()
    {
        if (ViewState["convert"].ToString() == "1")
        {
            string ProspectID = Request.QueryString["uid"].ToString();
            if (uc_CustomerSearch1._hdnCustID.Value == string.Empty)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "';", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "';", true);
        }
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkProspect");
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

    private void HideControls()
    {
        pnlOpenTasks.Visible = false;
        pnlTaskH.Visible = false;
        pnlOpp.Visible = false;
        pnlEmail.Visible = false;
        pnlnotes.Visible = false;
        pnlSysInfo.Visible = false;
        menuLeads.Visible = false;
    }

    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ContactID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Email", typeof(string));

        Session["contacttablelead"] = dt;
    }

    private void FillProspectScreen(string ID)
    {
        lblHeader.Text = "Edit Lead";
        ViewState["edit"] = 1;
        objGeneralFunctions.ResetFormControlValues(this);
        txtProspectName.Focus();

        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProspectID = Convert.ToInt32(ID);
        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectByID(objProp_Customer);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtProspectName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
            ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
            ddlType.Text = ds.Tables[0].Rows[0]["type"].ToString();
            ddlSalesperson.SelectedValue = ds.Tables[0].Rows[0]["terr"].ToString();

            txtAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
            txtCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
            ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
            txtZip.Text = ds.Tables[0].Rows[0]["zip"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
            lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
            lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();

            txtBillAddress.Text = ds.Tables[0].Rows[0]["billaddress"].ToString();
            txtBillCity.Text = ds.Tables[0].Rows[0]["billcity"].ToString();
            ddlBillState.SelectedValue = ds.Tables[0].Rows[0]["billstate"].ToString();
            txtBillZip.Text = ds.Tables[0].Rows[0]["billzip"].ToString();
            txtBillPhone.Text = ds.Tables[0].Rows[0]["billphone"].ToString();

            txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
            txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
            txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
            txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
            ViewState["rol"] = ds.Tables[0].Rows[0]["rol"].ToString();
            hdnRol.Value = ds.Tables[0].Rows[0]["rol"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            txtSource.Text = ds.Tables[0].Rows[0]["source"].ToString();

            if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

            if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    FillContacts(ds.Tables[1]);
                }
            }
           
            FillTasks(ds.Tables[0].Rows[0]["rol"].ToString());
            FillOpportunity(ds.Tables[0].Rows[0]["rol"].ToString());
            GetDocuments();
            //GetMailsfromdb(-1);
            BindEmails(GetMailsfromdb(-1, string.Empty));
        }
    }

    public DataSet GetMailsfromdb(int type, string OrderBy)
    {
        if (OrderBy == string.Empty)
            ViewState["sortexp"] = null;

        DataSet ds = null;
        if (ViewState["rol"] != null)
        {
            if (ViewState["rol"].ToString().Trim() != string.Empty)
            {
                objGeneral.OrderBy = OrderBy;
                objGeneral.ConnConfig = Session["config"].ToString();
                objGeneral.type = type;
                objGeneral.rol = Convert.ToInt32(ViewState["rol"]);
                objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
                ds = objBL_General.GetMails(objGeneral);
            }
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
            ViewState["newmail"] = ds.Tables[0].Rows.Count;
            ////lblNewEmail.Text = string.Empty;
            //lblEmailCount.Text = string.Empty;
            //panel9.Visible = false;
            hdnMailct.Value = ds.Tables[0].Rows.Count.ToString();
        }
        else
        {
            gvmail.DataBind();
            menuLeads.Items[4].Text = "Emails(0)";
        }
    }

    private void GetProspectType()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectType(objProp_Customer);
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "type";
        ddlType.DataValueField = "type";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    private void FillSalesPerson()
    {

        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objPropUser);

        ddlSalesperson.DataSource = ds.Tables[0];
        ddlSalesperson.DataTextField = "name";
        ddlSalesperson.DataValueField = "id";
        ddlSalesperson.DataBind();

        ddlSalesperson.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.Name = txtProspectName.Text.Trim();
        objProp_Customer.Address = txtAddress.Text.Trim();
        objProp_Customer.City = txtCity.Text.Trim();
        objProp_Customer.State = ddlState.SelectedValue;
        objProp_Customer.Zip = txtZip.Text.Trim();
        objProp_Customer.Phone = txtPhone.Text.Trim();
        objProp_Customer.Cellular = txtCell.Text.Trim();
        objProp_Customer.Contact = txtMaincontact.Text.Trim();
        objProp_Customer.Type = ddlType.SelectedValue;
        objProp_Customer.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Customer.Email = txtEmail.Text.Trim();
        objProp_Customer.CustomerName = txtCustomer.Text.Trim();
        if (ddlSalesperson.SelectedValue != string.Empty)
            objProp_Customer.Terr = Convert.ToInt32(ddlSalesperson.SelectedValue);
        objProp_Customer.Billaddress = txtBillAddress.Text.Trim();
        objProp_Customer.BillCity = txtBillCity.Text.Trim();
        objProp_Customer.BillState = ddlBillState.SelectedValue;
        objProp_Customer.BillZip = txtBillZip.Text.Trim();
        objProp_Customer.BillPhone = txtBillPhone.Text.Trim();
        objProp_Customer.Fax = txtFax.Text.Trim();
        objProp_Customer.Website = txtWebsite.Text.Trim();
        objProp_Customer.Lat = lat.Value;
        objProp_Customer.Lng = lng.Value;
        objProp_Customer.Remarks = txtRemarks.Text.Trim();
        objProp_Customer.LastUpdateUser = Session["username"].ToString();
        objProp_Customer.Source = txtSource.Text.Trim();

        if (Session["contacttablelead"] != null)
        {
            objProp_Customer.ContactData = (DataTable)Session["contacttablelead"];
        }

        try
        {          
            string strMsg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objBL_Customer.AddProspect(objProp_Customer);
                objGeneralFunctions.ResetFormControlValues(this);
                gvContacts.DataSource = null;
                gvContacts.DataBind();
                Session["contacttablelead"] = null;
                CreateTable();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objBL_Customer.UpdateProspect(objProp_Customer);
                ConvertProspectWizard();
                FillContacts(FillContactByROL(Convert.ToInt32(ViewState["rol"])).Tables[0]);
                UpdatePanel1.Update();
                strMsg = "Updated";
                if (txtCustomer.Text == string.Empty)
                    txtCustomer.Text = txtProspectName.Text;
            }

            string strScript = string.Empty;
            strScript += "noty({text: 'Lead " + strMsg + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);

            FillRecentProspect();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillRecentProspect()
    {
        NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        masterSalesMaster.FillRecentProspect();
        masterSalesMaster.FillPendingLeads();
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        ModalPopup.Show();
        pnlContact.Visible = true;
        pnlAttach.Visible = false;
    }

    protected void lnkCancelContact_Click(object sender, EventArgs e)
    {
        ModalPopup.Hide();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvContacts.Rows)
        {
            DataTable dt = (DataTable)Session["contacttablelead"];
            CheckBox chkSelectCon = (CheckBox)di.FindControl("chkSelectCon");
            Label lblindex = (Label)di.FindControl("lblindex");

            if (chkSelectCon.Checked == true)
            {
                ModalPopup.Show();
                pnlContact.Visible = true;
                pnlAttach.Visible = false;

                DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

                txtContcName.Text = dr["Name"].ToString();
                txtContPhone.Text = dr["Phone"].ToString();
                txtContFax.Text = dr["Fax"].ToString();
                txtContCell.Text = dr["Cell"].ToString();
                txtContEmail.Text = dr["Email"].ToString();
                ViewState["editcon"] = 1;
                ViewState["index"] = lblindex.Text;
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttablelead"];

        foreach (GridViewRow di in gvContacts.Rows)
        {
            CheckBox chkSelectCon = (CheckBox)di.FindControl("chkSelectCon");
            Label lblindex = (Label)di.FindControl("lblindex");

            if (chkSelectCon.Checked == true)
            {
                dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));
            }
        }

        dt.AcceptChanges();
        FillContacts(dt);

    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttablelead"];

        DataRow dr = dt.NewRow();

        dr["Name"] = objGeneralFunctions.Truncate(txtContcName.Text, 50);
        dr["Phone"] = objGeneralFunctions.Truncate(txtContPhone.Text, 22);
        dr["Fax"] = objGeneralFunctions.Truncate(txtContFax.Text, 22);
        dr["Cell"] = objGeneralFunctions.Truncate(txtContCell.Text, 22);
        dr["Email"] = objGeneralFunctions.Truncate(txtContEmail.Text, 50);

        if (ViewState["editcon"].ToString() == "1")
        {
            dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            ViewState["editcon"] = 0;
        }
        else
        {
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        FillContacts(dt);

        objGeneralFunctions.ResetFormControlValues(pnlContact);
        ModalPopup.Hide();

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("prospects.aspx");
    }

    private void FillContacts(DataTable dt)
    {
        Session["contacttablelead"] = dt;

        gvContacts.DataSource = dt;
        gvContacts.DataBind();
        menuLeads.Items[0].Text = "Contacts(" + dt.Rows.Count + ")";
        
    }

    private DataSet FillContactByROL(int rol)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = rol;
        DataSet ds = new DataSet();
        return ds = objBL_Customer.getContactByRolID(objProp_Customer);
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
    }

    private void FillOpportunity(string name)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "l.rol";
        objProp_Customer.SearchValue = name;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;

        ds = objBL_Customer.getOpportunity(objProp_Customer);
        gvOpportunity.DataSource = ds.Tables[0];
        gvOpportunity.DataBind();
        menuLeads.Items[3].Text = "Opportunities(" + ds.Tables[0].Rows.Count + ")";

    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {

        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (FileUpload1.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
            }

            objMapData.Screen = "SalesLead";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            objMapData.Subject = txtNoteSub.Text.Trim();
            objMapData.Body = txtNoteBody.Text.Trim();
            objMapData.Mode = Convert.ToInt16(ViewState["notesmode"]);
            if (ViewState["notesmode"].ToString() == "1")
                objMapData.DocID = Convert.ToInt32(hdnNoteID.Value);
            else
                objMapData.DocID = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);
            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetDocuments()
    {
        objMapData.Screen = "SalesLead";
        objMapData.TempId = "0";
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());

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

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            }
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkAddNote_Click(object sender, EventArgs e)
    {
        objGeneralFunctions.ResetFormControlValues(pnlAttach);
        ModalPopup.Show();
        pnlContact.Visible = false;
        pnlAttach.Visible = true;
        ViewState["notesmode"] = 0;
    }

    protected void lnkEditNote_Click(object sender, EventArgs e)
    {
        objGeneralFunctions.ResetFormControlValues(pnlAttach);
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblID");
            Label lblSub = (Label)di.FindControl("lblSub");
            Label lblBody = (Label)di.FindControl("lblBody");

            if (chkSelect.Checked == true)
            {
                ModalPopup.Show();
                pnlContact.Visible = false;
                pnlAttach.Visible = true;

                txtNoteSub.Text = lblSub.Text;
                txtNoteBody.Text = lblBody.Text;
                hdnNoteID.Value = lblID.Text;
                ViewState["notesmode"] = 1;
            }
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
        //            objGeneral.AccountID = user;
        //            int MAXUID = objBL_General.GetMAXEmailUID(objGeneral);

        //            objGeneralFunctions.DownloadMailsIMAP(host, user, pass, port, Userid, Session["config"].ToString(), MAXUID, dsEmail);

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

    //private void DownloadMails(string host, string user, string pass, string port, int userid)
    //{
    //    Pop3Client pop3Client = new Pop3Client();

    //    try
    //    {
    //        if (pop3Client.Connected)
    //            pop3Client.Disconnect();

    //        pop3Client.Connect(host.Trim(), int.Parse(port.Trim()), true);
    //        pop3Client.Authenticate(user.Trim(), pass.Trim());

    //        int count = pop3Client.GetMessageCount();
    //        List<string> uids = pop3Client.GetMessageUids();

    //        objGeneral.ConnConfig = Session["config"].ToString();
    //        objGeneral.AccountID = user.Trim();
    //        DataSet ds = objBL_General.GetMsgUID(objGeneral);
    //        List<string> seenUids = ds.Tables[0].AsEnumerable()
    //                               .Select(r => r.Field<string>("UID"))
    //                               .ToList();

    //        for (int i = 0; i < uids.Count; i++)
    //        {
    //            string currentUidOnServer = uids[i];
    //            if (!seenUids.Contains(currentUidOnServer))
    //            {
    //                try
    //                {
    //                    Message unseenMessage = pop3Client.GetMessage(i + 1);

    //                    var AID = System.Guid.NewGuid();
    //                    objGeneral.From = Convert.ToString(unseenMessage.Headers.From.Address);
    //                    objGeneral.to = Convert.ToString(string.Join(",", objGeneralFunctions.toStringArray(unseenMessage.Headers.To)));
    //                    objGeneral.cc = Convert.ToString(string.Join(",", objGeneralFunctions.toStringArray(unseenMessage.Headers.Cc)));
    //                    objGeneral.bcc = Convert.ToString(string.Join(",", objGeneralFunctions.toStringArray(unseenMessage.Headers.Bcc)));
    //                    objGeneral.subject = Convert.ToString(unseenMessage.Headers.Subject);
    //                    objGeneral.sentdate = unseenMessage.Headers.DateSent;
    //                    //objGeneral.date = Convert.ToString(unseenMessage.Headers.Date);
    //                    objGeneral.Attachments = unseenMessage.FindAllAttachments().Count();
    //                    objGeneral.msgid = Convert.ToString(unseenMessage.Headers.MessageId);
    //                    objGeneral.uid = currentUidOnServer;
    //                    objGeneral.GUID = AID;
    //                    objGeneral.type = 0;
    //                    objGeneral.userid = userid;
    //                    objGeneral.AccountID = user.Trim();
    //                    objGeneral.ConnConfig = Session["config"].ToString();
    //                    int success = objBL_General.AddEmails(objGeneral);

    //                    if (success == 1)
    //                    {
    //                        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
    //                        string savepath = savepathconfig + @"\mails\";
    //                        if (!Directory.Exists(savepath))
    //                        {
    //                            Directory.CreateDirectory(savepath);
    //                        }
    //                        string filename = AID.ToString() + ".eml";
    //                        FileInfo file = new FileInfo(savepath + filename);
    //                        unseenMessage.Save(file);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
    //                    throw ex;
    //                }
    //            }
    //        }
    //    }
    //    catch (InvalidLoginException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
    //        throw new Exception("The server did not accept the user credentials!");
    //    }
    //    catch (PopServerNotFoundException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
    //        throw new Exception("The server could not be found");
    //    }
    //    catch (PopServerLockedException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
    //        throw new Exception("The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?");
    //    }
    //    catch (LoginDelayException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
    //        throw new Exception("Login not allowed. Server enforces delay between logins. Have you connected recently?");
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}


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
        ////System.Threading.Thread.Sleep(3000);
    }

    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    UpdateProgress up = (UpdateProgress)Page.Master.Master.FindControl("UpdateProgress1");
    //    up.Visible = false;
    //}

    [System.Web.Services.WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static string CheckEmail(string rol, int type, int uid)
    {
        int mails = 0;
        //DataSet ds = null;
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        if (rol.Trim() != string.Empty)
        {
            objGeneral.OrderBy = "";
            
            objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objGeneral.type = type;
            objGeneral.rol = Convert.ToInt32(rol);
            objGeneral.userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
            if (type == -2)
            {
                objGeneral.RegID = "[OP-" + uid.ToString() + "]";
                objGeneral.rol = 0;
            }
            mails = objBL_General.GetMailsCount(objGeneral);
        }
        return mails.ToString();
    }

}
